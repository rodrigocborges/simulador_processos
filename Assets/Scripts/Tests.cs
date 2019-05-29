using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process
{
    private int id;
    private int incoming;
    private int execution;
    private int quantum;

    public Process(int id, int incoming, int quantum = 0, int execution)
    {
        this.id = id;
        this.incoming = incoming;
        this.quantum = quantum;
        this.execution = execution;
    }
}

public class Tests : MonoBehaviour
{

    float ct = 0;
    int max = 0;
    int a, b, c;
    int pa, pb, pc;
    Texture2D t1;
    Texture2D t2;
    Texture2D t3;

    Texture2D GenerateTexture(Color c)
    {
        int size = 64;
        Texture2D t = new Texture2D(size, size, TextureFormat.RGBA32, true, true);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
                t.SetPixel(i, j, c);
        }
        t.Apply();
        return t;
    }
    /*
    ID | Tempo de Chegada | Tempo de Execução
    */
    Queue<Process> processes = new Queue<Process>();
    List<Process> p = new List<Process>();

    void AddProcess()
    { 
        processes.Enqueue(new Process(Random.Range(0, 9999), Random.Range(0, 1000), Random.Range(1, 100), Random.Range(10, 100)));
    }

    void Start()
    {
        // FileManagement fm = new FileManagement();
        /*List<string> info = new List<string>();
        info.Add("LOG LINE 1");
        info.Add("LOG LINE 2");
        fm.WriteLog(info);*/

        /*List<string> r = fm.ReadFile("entrada.txt");
        foreach(string s in r)
        {
            print(s);
        }*/
        max = Screen.width - 64;
        t1 = GenerateTexture(Color.red);
        t2 = GenerateTexture(Color.green);
        t3 = GenerateTexture(Color.blue);
    }

    void Update()
    {
        ct += Time.deltaTime;
        if (ct > 1/50f)
        {
            a += Random.Range(0, 10);
            b += Random.Range(0, 10);
            c += Random.Range(0, 10);
            ct = 0;
        }

        if (a > max)
        {
            a = 0;
            ++pa;
        }
        if (b > max)
        {
            b = 0;
            ++pb;
        }
        if (c > max)
        {
            c = 0;
            ++pc;
        }

        if (Input.GetKeyDown(KeyCode.X))
            AddProcess();
        else if (Input.GetKeyDown(KeyCode.Z))
            processes.Dequeue();
    }

    void OnGUI()
    {
        //GUI.Box(new Rect(10, 0, 200, 20), string.Format("PA: {0} | PB: {1} | PC: {2}", pa, pb, pc));
        //GUI.Box(new Rect(a, 30, 16, 16), t1);

        for(int i = 0; i < processes.Count; i++)
        {
            GUI.Box(new Rect(10, 10 + ((i + 1) * 20), 200, 20), processes.ToArray()[i].id.ToString());
        }
    }
}