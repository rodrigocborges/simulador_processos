using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManagement : MonoBehaviour {

    private string defaultPath = Application.dataPath + "/";
    private string path = "";
    private string date = System.DateTime.Now.Day + "_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Year + "__";
    private string hour = System.DateTime.Now.Hour + "_" + System.DateTime.Now.Minute + "_" + System.DateTime.Now.Second;

    public FileManagement(string path = "")
    {
        if (path.Equals(string.Empty))
            path = defaultPath;

        //print("File Management instantiated!");
        if (!Directory.Exists(defaultPath + "logs/"))
            Directory.CreateDirectory(defaultPath + "logs/");
    }

    public string[] ReadFile(string file)
    {
        return null;
    }

    public void WriteLog(List<string> info)
    {
        StreamWriter writer = new StreamWriter(defaultPath + "logs/" + date + hour + ".log");
        foreach(string s in info)
        {
            writer.WriteLine(s);
        }
        writer.Dispose();
    }

    public void DeleteAllLogs()
    {
        string logPath = defaultPath + "logs/";
        if (Directory.Exists(logPath))
        {
            string[] files = Directory.GetFiles(logPath, "*.log");
            if (files.Length != 0)
            {
                foreach (string f in files)
                {
                    if(File.Exists(f))
                        File.Delete(f);
                }
            }
        }
    }

}
