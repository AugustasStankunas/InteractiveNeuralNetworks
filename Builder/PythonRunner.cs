using System.Diagnostics;

namespace Builder
{
    public static class PythonRunner
    {
        public static void RunScript()
        {
            string pathToPythonServer = "../../../../Python-server/";
            string pathToPython = pathToPythonServer + ".venv/Scripts/python.exe";
            string pathToScript = "main.py";


            var startInfo = new ProcessStartInfo
            {
                FileName = pathToPython,
                Arguments = pathToScript,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = pathToPythonServer
            };

            Task.Run(() =>
            {
                using (var process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    //process.WaitForExit();

                    if (!string.IsNullOrEmpty(errors))
                        throw new Exception($"Python error: {errors}");
                }
            });
        }
    }
}
