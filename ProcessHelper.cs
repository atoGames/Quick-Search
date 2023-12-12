using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

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

    public static void OpenVsCode(string filePath, string app = "code")
    {
        if (app == "Visual studio")
            app = "devenv";
        else if (app == "Rider")
            app = "rider64";
        else
            app = "code";

        // Open
        Open(app.ToLower(), filePath);
    }
}