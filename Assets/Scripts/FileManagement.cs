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
        defaultPath = path;
        if (!Directory.Exists(defaultPath + "logs/"))
            Directory.CreateDirectory(defaultPath + "logs/");
    }

    public List<string> ReadFile(string file)
    {
        List<string> r = new List<string>();
        if (File.Exists(defaultPath + file))
        {
            StreamReader reader = new StreamReader(defaultPath + file);
            string currentLine = "";
            while((currentLine = reader.ReadLine()) != null)
            {
                r.Add(currentLine);
            }
            reader.Dispose();
        }
        else
        {
            print("File don't exists!");
        }
        return r;
    } //retorna linha a linha do arquivo

    public void WriteLog(List<string> info)
    {
        StreamWriter writer = new StreamWriter(defaultPath + "logs/" + date + hour + ".log");
        foreach(string s in info)
        {
            writer.WriteLine(s);
        }
        writer.Dispose();
    } //escreve linha a linha uma informação

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
