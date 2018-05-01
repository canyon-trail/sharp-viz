using System;
using System.Diagnostics;
using System.IO;

namespace SharpViz
{
    public sealed class GraphBuilder
    {
        public static void GenAndOpenDotGraph(string dotSyntax)
        {
            GenAndOpen(dotSyntax, "dot.exe");
        }

        public static void GenAndOpenForceDirectedGraph(string dotSyntax)
        {
            GenAndOpen(dotSyntax, "neato.exe");
        }

        public static string GenerateDotGraph(string dotSyntax)
        {
            return GeneratePdf(dotSyntax, "dot.exe");
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
                StartInfo = new ProcessStartInfo(outFile)
                {
                    UseShellExecute = true
                }
            };
            fileOpenProcess.Start();
        }

        private static string GeneratePdf(string dotSyntax, string executable)
        {
            var dotFile = Path.GetTempFileName();
            var outFile = Path.GetTempFileName();
            File.Delete(outFile);
            outFile += ".pdf";
            File.WriteAllText(dotFile, dotSyntax);

            var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = @"C:\Program Files (x86)\Graphviz2.38\bin\" + executable;
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
    }
}
