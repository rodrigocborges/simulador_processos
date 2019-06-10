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
        string local = path + "/logs/" + System.DateTime.Now.Day + "_" + System.DateTime.Now.Month + ".log";
        if (File.Exists(local))
        {
            fileToInsert = local;
        }
        else
        {
            try
            {
                StreamWriter writer = new StreamWriter(local);
                writer.WriteLine("Log iniciado em: " + System.DateTime.Now);
                writer.Close();
            }
            catch (System.IO.IOException ex)
            {
                print(ex.Message);
            }
        }
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
