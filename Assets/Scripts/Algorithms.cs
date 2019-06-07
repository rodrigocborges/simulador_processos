using System;
using System.Collections.Generic;

public class Algorithms
{
    public static Process InExecution = null;

    public static List<Process> SJF(List<Process> processes, float executionTime)
    {
        List<Process> aux = new List<Process>();

        for(int i = 0; i < processes.Count; i++)
        {
            if(processes[i].GetIncoming() <= executionTime)
                aux.Add(processes[i]);
        }

        processes.RemoveAt(0);
        processes.Sort((a, b) => {
            return a.GetExecution().CompareTo(b.GetExecution());
        });
        processes.Insert(0, InExecution);
        return processes;
    }
}