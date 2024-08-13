using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace QuickSearch
{
    // https://stackoverflow.com/questions/61937342/launch-visual-studio-code-programmatically
    public static class ProcessHelper
    {
        public static void Open(string app, string args)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = app;
                    process.StartInfo.Arguments = args;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }
            }
            catch (Exception e)
            {
                Debug.Log($"This {app} has not been found on your system: {e.Message}");
            }
        }

        // Open app
        public static void OpenApp(string filePath, string app) => Open(app.ToLower(), filePath);
    }
}