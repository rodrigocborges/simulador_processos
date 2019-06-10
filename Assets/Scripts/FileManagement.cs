using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManagement : MonoBehaviour {

    private string defaultPath = Application.dataPath + "/";
    private string fileToInsert = "";

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

    public void StartLog(string path)
    {
        System.DateTime d = System.DateTime.Now;
        fileToInsert = path + "/logs/" + string.Format("{0}{1}{2}_{3}{4}{5}.log", d.Day, d.Month, d.Year, d.Hour, d.Minute, d.Second);
        StreamWriter writer = File.AppendText(fileToInsert);
        writer.WriteLine("Log iniciado em: " + d);
        writer.Close();
    }

    public void WriteLog(string info)
    {

        StreamWriter writer = File.AppendText(fileToInsert);
        writer.Write(System.DateTime.Now + " - ");
        writer.WriteLine(info);
        writer.Close();
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
