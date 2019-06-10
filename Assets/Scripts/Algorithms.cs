using System;
using System.Collections.Generic;
using UnityEngine;

public class Algorithms
{
    public static Process InExecution = null;

    public static List<Process> FCFS(List<Process> processes)
    {
        processes.Sort((a, b) =>
        {
            return a.GetIncoming().CompareTo(b.GetIncoming());
        }); //ordenação por tempo de chegada (menor para o maior)
        return processes;
    }

    public static List<Process> SJF(List<Process> processes)
    {
        processes.Sort((a, b) => {
            return a.GetExecution().CompareTo(b.GetExecution());
        });
        InExecution = processes[0];
        return processes;
    }

    public static List<Process> RR(List<Process> processes) {
        processes.Add(processes[0]);
        processes.RemoveAt(0);
        return processes;
    }
}