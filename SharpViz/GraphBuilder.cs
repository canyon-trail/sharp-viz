using System;
using System.Diagnostics;
using System.IO;

namespace SharpViz
{
    public sealed class GraphBuilder
    {
        public static void GenAndOpenDotGraph(string dotSyntax)
        {
            GenAndOpen(dotSyntax, "dot");
        }

        public static void GenAndOpenForceDirectedGraph(string dotSyntax)
        {
            GenAndOpen(dotSyntax, "neato");
        }

        public static string GenerateDotGraph(string dotSyntax)
        {
            return GeneratePdf(dotSyntax, "dot");
        }

        private static void GenAndOpen(string dotSyntax, string executable)
        {
            var outFile = GeneratePdf(dotSyntax, executable);

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

        private static string GeneratePdf(string dotSyntax, string executable)
        {
            var dotFile = Path.GetTempPath() + Path.GetRandomFileName() + ".dot";
            var outFile = Path.GetTempPath() + Path.GetRandomFileName() + ".pdf";
            var execPath = GetFilePath(executable);

            if(!File.Exists(execPath)) {
                Console.Error.Write("{0} does not exist. Please make sure Graphviz is installed", execPath);
                return null;
            }

            File.WriteAllText(dotFile, dotSyntax);

            var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = execPath;
            process.StartInfo.Arguments = $"-o\"{outFile}\" -Tpdf \"{dotFile}\"";
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
