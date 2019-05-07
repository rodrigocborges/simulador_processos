using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour {

	void Start () {
        FileManagement fm = new FileManagement();
        /*List<string> info = new List<string>();
        info.Add("LOG LINE 1");
        info.Add("LOG LINE 2");
        fm.WriteLog(info);*/
        List<string> r = fm.ReadFile("entrada.txt");
        foreach(string s in r)
        {
            print(s);
        }

    }

    float ct = 0;
    const int max = 100;
    int a, b, c;
    int pa, pb, pc;

    private void OnGUI()
    {
        ct += Time.deltaTime;
        if(ct > 0.5)
        {
            a += Random.Range(0, 10);
            b += Random.Range(0, 10);
            c += Random.Range(0, 10);
            ct = 0;
        }
        GUI.Box(new Rect(10, 0, 200, 20), string.Format("PA: {0} | PB: {1} | PC: {2}", pa, pb, pc));
        GUI.Box(new Rect(a, 30, 64, 64), "(PA)" + a);
        GUI.Box(new Rect(b, 104, 64, 64), "(PB)" + b);
        GUI.Box(new Rect(c, 178, 64, 64), "(PC)" + c);
        
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
}
