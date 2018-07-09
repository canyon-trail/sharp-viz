using System;
using System.Diagnostics;
using System.IO;

namespace SharpViz
{
    public sealed class GraphBuilder
    {
        public static void GenAndOpenDotGraph(string dotSyntax)
        {
            GenAndOpen(dotSyntax, "dot", "pdf");
        }

        public static void GenAndOpenForceDirectedGraph(string dotSyntax)
        {
            GenAndOpen(dotSyntax, "neato", "pdf");
        }

        public static string GenerateDotGraph(string dotSyntax, string format = "pdf")
        {
            return GenerateFile(dotSyntax, "dot", format);
        }

        private static void GenAndOpen(string dotSyntax, string executable, string format)
        {
            var outFile = GenerateFile(dotSyntax, executable, format);

            if (outFile == null)
            {
                return;
            }

            var fileOpenProcess = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = OS() == PlatformID.Unix || OS() == PlatformID.MacOSX ? "open" : outFile,
                    Arguments = OS() == PlatformID.Unix || OS() == PlatformID.MacOSX ? outFile : ""
                }
            };
            fileOpenProcess.Start();
        }

        private static string GenerateFile(string dotSyntax, string executable, string format)
        {
            var dotFile = Path.GetTempPath() + Path.GetRandomFileName() + ".dot";
            var outFile = Path.GetTempPath() + Path.GetRandomFileName() + "." + format;
            var execPath = GetFilePath(executable);

            if(!File.Exists(execPath)) {
                Console.Error.Write("{0} does not exist. Please make sure Graphviz is installed", execPath);
                return null;
            }

            File.WriteAllText(dotFile, dotSyntax);

            var dpiArg = format == "pdf" ? "" : $"-Gdpi=300";

            var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = execPath;
            process.StartInfo.Arguments = $"-o\"{outFile}\" -T{format} {dpiArg} \"{dotFile}\"";

            process.Start();
            process.WaitForExit();

            File.Delete(dotFile);

            if(process.ExitCode == 0)
            {
                return outFile;
            }

            Console.Error.Write(process.StandardError.ReadToEnd());
            Console.Read();

            return null;
        }

        private static string GeneratePdf(string dotSyntax, string executable)
        {
            return GenerateFile(dotSyntax, executable, "pdf");
        }

        private static string GetFilePath(string executable)
        {
            switch(OS()) {
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return String.Format("/usr/local/bin/{0}", executable);
                default:
                    return String.Format(@"C:\Program Files (x86)\Graphviz2.38\bin\{0}.exe", executable);
            }
        }

        private static PlatformID OS()
        {
            return Environment.OSVersion.Platform;
        }
    }
}
