using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class App : MonoBehaviour {

	[Header("UI Elements")]
	public Dropdown processesInfo; 
	public Text processTypeInfo;

    //Algorithm
    private List<string> algNames = new List<string>();
    private List<string> algDescs = new List<string>();

    void LoadAlgorithmsInfo()
    {
        string path = Application.dataPath + "/Scripts/algorithm.txt";
        try
        {
            StreamReader r = new StreamReader(path);
            string all = r.ReadToEnd();
            string[] content = all.Split(new char[] { ':', ';' }, System.StringSplitOptions.RemoveEmptyEntries); // NAME:DESC;
            for(int i = 0; i < content.Length-1; i++)
            {
                //pares = nome e impares = descrição
                if (i % 2 == 0)
                    algNames.Add(content[i]);
                else
                    algDescs.Add(content[i]);
            }
        }
        catch(System.Exception ex)
        {
            print(ex.Message);
        }
    }

    public void ShowInfo(){
        processTypeInfo.text = algDescs[processesInfo.value];
    }

    void Start () {
        LoadAlgorithmsInfo();
        List<Dropdown.OptionData> od = new List<Dropdown.OptionData>();
        foreach(string s in algNames)
        {
            Dropdown.OptionData d = new Dropdown.OptionData(s.Substring(1));
            od.Add(d);
        }
        processesInfo.AddOptions(od);
    }
	
	void Update () {
		
	}
}
