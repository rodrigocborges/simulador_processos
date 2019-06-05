using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Tempo de chegada / Tempo de execução
public class Process
{
    private int id;
    private int incoming;
    private int execution;
    private int quantum;
    private Color color;
    private Texture2D texture;
    private Vector2 pos = Vector2.zero;

    public Process(int id, int incoming, int execution, Color color, int quantum = 2)
    {
        this.id = id;
        this.incoming = incoming;
        this.quantum = quantum;
        this.execution = execution;
        this.color = color;
    }

    public void SetPos(Vector2 p)
    {
        pos = p;
    }

    public int GetID() { return id; }
    public int GetIncoming() { return incoming; }
    public int GetExecution() { return execution; }
    public int GetQuantum() { return quantum; }
    public Color GetColor() { return color; }
    public Texture2D GetTexture()
    {
        int size = 64;
        Texture2D t = new Texture2D(size, size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
                t.SetPixel(i, j, color);
        }
        t.Apply();
        return t;
    }
    public Vector2 GetPos() { return pos; }

}

public class App : MonoBehaviour
{
    [SerializeField] private GUISkin guiSkin;
    private Vector2 guiAreaSize = new Vector2(600, 500);
    private string appTitle = "SIMULADOR DE PROCESSOS";
    private int appView = 0;
    private string filename = "";
    private string pAmount = "";
    private int maxPAmount = 60;
    private int algSelected = 0;
    private string[] algs = new string[] { "FCFS", "SJF", "RR" };
    private string[] algsDesc = new string[] {
        "Processos alocados na fila seguindo a ordem de chegada dos mesmos.",
        "Processos adicionados seguindo a ordem, crescente, de tempo de clock.",
        "Processos são divididos em intervalos de tempo (quantum)."
    };
    private Vector2 scrollView;
    private Vector2 hScrollView;
    private string textToShow = "";
    private bool messageBox = false;

    private List<Process> listProcess = new List<Process>();
    private Queue<Process> queueProcess = new Queue<Process>();

    void Start()
    {
        filename = System.IO.Directory.GetCurrentDirectory();
        pAmount = "0";
    }

    void Update()
    {

    }

    //Função auxiliar para gerar uma textura 2D com tamanho e cor configurável
    Texture2D GenerateTexture(int size, Color color)
    {
        Texture2D t = new Texture2D(size, size);
        for (int i = 0; i < size; i++)
            t.SetPixel(i, i, color);
        t.Apply();
        return t;
    }

    //Evento para gerar processos aleatórios baseado numa quantidade pré-definida
    void GenerateRandomProcess(int amount)
    {
        if (amount <= 0)
            StartCoroutine(MessageBox(1.5f, "Digite uma quantia válida para gerar processos!"));
        else if(amount > maxPAmount)
            StartCoroutine(MessageBox(1.5f, string.Format("Valor ultrapassou o limite de {0} de processos para gerar!", maxPAmount)));
        else
        {
            listProcess.Clear(); //limpa lista e só depois gera
            for (int i = 0; i < amount; i++)
                listProcess.Add(new Process(i + 1, Random.Range(0, 900), Random.Range(1, 10), Random.ColorHSV()));
            StartCoroutine(MessageBox(1.5f, "Processos aleatórios gerados com sucesso!"));
        }
    }

    //Função auxiliar para exibir uma mensagem/informação em uma caixa
    IEnumerator MessageBox(float timeToShow, string aux)
    {
        messageBox = true;
        textToShow = aux;
        yield return new WaitForSeconds(timeToShow);
        messageBox = false;
        textToShow = string.Empty;
        StopAllCoroutines();
    }

    void HomeView()
    {
        GUILayout.Label("Local do arquivo de entrada de processos: ");
        filename = GUILayout.TextField(filename);
        if (GUILayout.Button("Importar"))
        {
            FileManagement fs = new FileManagement();
            List<string> fileIn = fs.ReadFile(filename);
            ConvertFileInToProcess(fileIn);
            StartCoroutine(MessageBox(1.5f, "Importando..."));
        }
        GUILayout.Space(50);

        GUILayout.Box("Processos: ");
        GUILayout.Label("Quantidade de processos para gerar aleatoriamente: ");
        pAmount = GUILayout.TextField(pAmount, maxPAmount);
        GUILayout.Label("Algoritmo de Escalonamento: ");
        algSelected = GUILayout.SelectionGrid(algSelected, algs, 3);
        GUILayout.Label("Descrição: " + algsDesc[algSelected]);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Gerar Aleatoriamente"))
            GenerateRandomProcess(int.Parse(pAmount));
        if (GUILayout.Button("Checar Processos Gerados"))
            appView = 1;
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
        GUILayout.MinHeight(40);
        if (GUILayout.Button("Executar"))
            appView = 2;

        if(messageBox)
            GUILayout.Box(textToShow);
    }

    //Função auxiliar para pegar linha a linha do arquivo e colocar os devidos parametros de cada processo na lista
    void ConvertFileInToProcess(List<string> fileIn)
    {
        for(int i = 0; i < fileIn.Count; i++)
        {
            string[] aux = fileIn[i].Split(' ');
            int id = int.Parse(aux[0]);
            int incoming = int.Parse(aux[1]);
            int execution = int.Parse(aux[2]);
            listProcess.Add(new Process(id, incoming, execution, Color.red));
        }
        listProcess.Sort();
    }

    void GenerateProcessesView()
    {
        scrollView = GUILayout.BeginScrollView(scrollView, false, true);
        foreach (Process p in listProcess)
        {
            GUILayout.Box(string.Format("[P{0}] - Entrada [{1}] - T. Execução [{2}] - Quantum [{3}]", p.GetID(), p.GetIncoming(), p.GetExecution(), p.GetQuantum()));
        }
        GUILayout.EndScrollView();
    }

    //TODO: Corrigir
    void RunningView()
    {
        int size = 32;
        /*
            algSelected: 0 - FCFS | 1 - SJF | 2 - RR
            FCFS: fila
            SJF: list com sort
            RR: ??
        */

        hScrollView = GUILayout.BeginScrollView(hScrollView, true, false, GUILayout.Height(60));
        GUILayout.BeginHorizontal();
        for (int i = 0; i < listProcess.Count; i++)
        {
            GUILayout.Box("P" + listProcess[i].GetID() + " (" + listProcess[i].GetQuantum() + ")", GUILayout.Width(size + (listProcess[i].GetQuantum() * 10)));
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Retroceder"))
            print("Retroceder");

        if (GUILayout.Button("Avançar"))
            print("Avançar");
        GUILayout.EndHorizontal();
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;
        GUILayout.BeginArea(new Rect(Screen.width / 2 - (guiAreaSize.x / 2), Screen.height / 2 - (guiAreaSize.y / 2), guiAreaSize.x, guiAreaSize.y));
        GUILayout.Box(appTitle);

        if(appView != 0)
        {
            if (GUILayout.Button("Voltar"))
                appView = 0;
        }

        switch (appView)
        {
            case 0:
                HomeView();
                break;
            case 1:
                GenerateProcessesView();
                break;
            case 2:
                RunningView();
                break;
            default:
                HomeView();
                break;
        }
        GUILayout.EndArea();
    }

}