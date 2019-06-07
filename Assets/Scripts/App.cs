using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process
{
    private int id;
    private int incoming;
    private int execution;
    private int quantum;
    private Color color;

    public Process(int id, int incoming, int execution, Color color, int quantum = 2)
    {
        this.id = id;
        this.incoming = incoming;
        this.quantum = quantum;
        this.execution = execution;
        this.color = color;
    }

    public int GetID() { return id; }
    public int GetIncoming() { return incoming; }
    public int GetExecution() { return execution; }
    public int GetQuantum() { return quantum; }
    public Color GetColor() { return color; }

}

public class App : MonoBehaviour
{
    [SerializeField] private GUISkin guiSkin;
    [SerializeField] private Texture2D logo;
    private Vector2 guiAreaSize;
    private Vector2 scrollView;
    private Vector2 hScrollView;
    private string appTitle = "SIMPOP - SIMULADOR DE PROCESSOS";
    private string filename = "";
    private string pAmount = "";
    private int appView = 0;
    private int maxPAmount = 60;
    private int algSelected = 0;
    private string[] algs = new string[] { "FCFS", "SJF", "RR" };
    private string[] algsDesc = new string[] {
        "Processos alocados na fila seguindo a ordem de chegada dos mesmos.",
        "Processos adicionados seguindo a ordem, crescente, de tempo de clock.",
        "Processos são divididos em intervalos de tempo (quantum)."
    };
    private string textToShow = "";
    private string btnPausedText;
    private bool messageBox = false;
    private bool paused = false;
    private float timeSpeed = 10f;
    private float currentClockTime = 0;
    private float executionTime = 0;

    private List<Process> listProcess = new List<Process>();
    private Queue<Process> queueProcess = new Queue<Process>();

    void Start()
    {
        //Redimensionar objetos baseado no tamanho da tela
        guiAreaSize = new Vector2(Screen.width - 100, Screen.height);
        //Caminho de arquivo de entrada padrão
        filename = System.IO.Directory.GetCurrentDirectory() + "/entrada.txt";
        pAmount = "0"; // Quantidade padrão de processos aleatórios para ser gerado
    }

    void Update()
    {
        if(paused)
        {
            btnPausedText = "Despausar";
            Time.timeScale = 0;
        }
        else
        {
            btnPausedText = "Pausar";
            Time.timeScale = 1;
        }

        if (appView == 2) //começa a contar o time somente quando estiver na view Running
        {
            currentClockTime += Time.deltaTime * timeSpeed;
            if (currentClockTime >= 5)
            {
                ++executionTime;
                currentClockTime = 0;
            }
        }
        else
        {
            executionTime = 0;
        }
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
            StartCoroutine(MessageBox(1.5f, "Digite uma quantidade válida para gerar processos!"));
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
        {
            //Verifica se tem processos, senão pede para importar ou gerar aleatoriamente
            if (listProcess.Count == 0)
                StartCoroutine(MessageBox(1.5f, "Sem processos na lista, importe ou gere!"));
            else
                appView = 2;
        }
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
        listProcess.Sort((a, b) =>
        {
            return a.GetIncoming().CompareTo(b.GetIncoming());
        }); //ordenação por tempo de chegada (menor para o maior)
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
        int pExecutions = 0;
        /*
            algSelected: 0 - FCFS | 1 - SJF | 2 - RR
            FCFS: fila
            SJF: list com sort
            RR: ??
        */

        hScrollView = GUILayout.BeginScrollView(hScrollView, true, false, GUILayout.Height(60));
        GUILayout.BeginHorizontal();

        if(algSelected == 0) //FCFS
        {
            for (int i = 0; i < listProcess.Count; i++)
            {
                if (listProcess[i].GetIncoming() >= executionTime && listProcess[i].GetExecution() < executionTime)
                {
                    ++pExecutions;
                    GUILayout.Box("P" + listProcess[i].GetID(), GUILayout.Width(size + (listProcess[i].GetQuantum() * 10)), GUILayout.Height(32));
                }
            }
        }
        else if(algSelected == 1) //SJF
        {
            Algorithms.InExecution = listProcess[0];
            listProcess = Algorithms.SJF(listProcess, executionTime);
            for (int i = 0; i < listProcess.Count; i++)
            {
                if(listProcess[i].GetIncoming() <= executionTime)
                {
                    ++pExecutions;
                    GUILayout.Box("P" + listProcess[i].GetID(), GUILayout.Width(size + (listProcess[i].GetQuantum() * 10)), GUILayout.Height(32));
                }
            }
        }
        else //RR
        {

        }

        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

        GUILayout.Box("Tempo de execução: " + executionTime + " | Aceleração do tempo (%): " + timeSpeed + " | Processos: " + pExecutions + " de " + listProcess.Count);
        GUILayout.Box("Estratégia de escalonamento sendo utilizada: " + algs[algSelected]);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Retroceder"))
        {
            if(executionTime >= 0)
                --executionTime;
        }

        if (GUILayout.Button(btnPausedText))
            paused = !paused;

        if (GUILayout.Button("Acelerar"))
        {
            if(timeSpeed < 100)
                ++timeSpeed;
        }

        if (GUILayout.Button("Desacelerar"))
        {
            if (timeSpeed > 0)
                --timeSpeed;
        }

        if (GUILayout.Button("Avançar"))
        {
            ++executionTime;
        }
        GUILayout.EndHorizontal();
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;
        GUILayout.BeginArea(new Rect(Screen.width / 2 - (guiAreaSize.x / 2), Screen.height / 2 - (guiAreaSize.y / 2), guiAreaSize.x, guiAreaSize.y));
        GUILayout.Box(logo, GUILayout.Height(100));
        GUILayout.Box("Desenvolvido por Rodrigo Borges e Bruno Lobell - AIC 2 (FURG, 2019)");

        if (appView != 0)
        {
            if (GUILayout.Button("Voltar"))
                appView = 0;
        }

        //Máquina de estados para as views
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

        //Se houver uma mensagem, exibe
        if (messageBox)
            GUILayout.Box(textToShow);

        GUILayout.EndArea();
    }

}