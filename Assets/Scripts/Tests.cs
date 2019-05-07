using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour {

    float ct = 0;
    const int max = 100;
    int a, b, c;
    int pa, pb, pc;
    Texture2D t1;
    Texture2D t2;
    Texture2D t3;

    Texture2D GenerateTexture(Color c)
    {
        int size = 64;
        Texture2D t = new Texture2D(size, size, TextureFormat.RGBA32, true, true);
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
                t.SetPixel(i, j, c);
        }
        t.Apply();
        return t;
    }

    void Start()
    {
        FileManagement fm = new FileManagement();
        /*List<string> info = new List<string>();
        info.Add("LOG LINE 1");
        info.Add("LOG LINE 2");
        fm.WriteLog(info);*/

        /*List<string> r = fm.ReadFile("entrada.txt");
        foreach(string s in r)
        {
            print(s);
        }*/

        t1 = GenerateTexture(Color.red);
        t2 = GenerateTexture(Color.green);
        t3 = GenerateTexture(Color.blue);
    }

    void Update()
    {
        ct += Time.deltaTime;
        if (ct > (1/30))
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
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 0, 200, 20), string.Format("PA: {0} | PB: {1} | PC: {2}", pa, pb, pc));
        GUI.Box(new Rect(a, 30, 64, 64), t1);
        GUI.Box(new Rect(b, 104, 64, 64), t2);
        GUI.Box(new Rect(c, 178, 64, 64), t3);
    }
}
