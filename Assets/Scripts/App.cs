using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test
{
    public int id;
    public int amount;
    public string hash;

    public Test(int id, int amount)
    {
        System.Random r = new System.Random();
        this.id = id;
        this.amount = amount;
        this.hash = r.Next(9999).ToString();
    }
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
    private string textToShow = "";
    private bool messageBox = false;

    void Start()
    {
        filename = System.IO.Directory.GetCurrentDirectory();
        pAmount = "0";
    }

    void Update()
    {

    }

    //Função auxiliar para gerar uma textura 2D com tamanho e cor configurável
    Texture2D GenerateTexture(int width, int height, Color color)
    {
        Texture2D t = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                t.SetPixel(i, j, color);
        }
        t.Apply();
        return t;
    }

    //Função auxiliar para configurar os estilos da parte gráfica do app
    GUIStyle HUDStyle(int fontSize, FontStyle fontStyle = FontStyle.Normal, Texture2D background = null)
    {
        GUIStyle g = new GUIStyle();
        g.fontSize = fontSize;
        g.fontStyle = FontStyle.Bold;
        g.alignment = TextAnchor.MiddleCenter;
        g.normal.background = background;
        return g;
    }

    void HomeView()
    {
        GUILayout.Label("Local do arquivo de entrada de processos: ");
        filename = GUILayout.TextField(filename);
        if (GUILayout.Button("Importar"))
            StartCoroutine(MessageBox(1.5f, "Importando..."));
        GUILayout.Space(50);

        GUILayout.Box("Processos: ");
        GUILayout.Label("Quantidade de processos para gerar aleatoriamente: ");
        pAmount = GUILayout.TextField(pAmount, maxPAmount);
        GUILayout.Label("Algoritmo de Escalonamento: ");
        algSelected = GUILayout.SelectionGrid(algSelected, algs, 3);
        GUILayout.Label("Descrição: " + algsDesc[algSelected]);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Gerar Aleatoriamente"))
            print("Gerar...");
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

    List<Test> tests = new List<Test>() {
        new Test(1, 1000),
        new Test(2, 3000),
        new Test(3, 6000),
        new Test(4, 22000),
        new Test(5, 32320),
        new Test(6, 323230),
        new Test(7, 30234400),
        new Test(8, 23),
        new Test(9, 1212),
        new Test(10, 2323),
        new Test(11, 3434),
        new Test(12, 2323),
        new Test(13, 1111),
        new Test(14, 2222),
        new Test(15, 3333),
        new Test(16, 444),
        new Test(17, 1),
        new Test(18, 0),
        new Test(19, 444)
    };
    void GenerateProcessesView()
    {
        scrollView = GUILayout.BeginScrollView(scrollView, false, true);
        foreach (Test t in tests)
        {
            GUILayout.Box(string.Format("[{0}] {1} / {2}", t.id, t.amount, t.hash));
        }
        GUILayout.EndScrollView();
    }

    void RunningView()
    {
        int size = 32;
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
                GUI.Box(new Rect(i * size * 1.2f, j * size * 1.2f + 60, size, size), i.ToString());
        }
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