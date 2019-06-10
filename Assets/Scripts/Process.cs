using System;
using UnityEngine;

public class Process
{
    private int id;
    private int incoming;
    private int execution;
    private int quantum;
    private int executedTime = 1;
    private Color color;
    private Texture2D texture;

    public Process(int id, int incoming, int execution, Color color, int quantum = 2)
    {
        this.id = id;
        this.incoming = incoming;
        this.quantum = quantum;
        this.execution = execution;
        this.color = color;

        texture = GenerateTexture(32, 32, this.color);
    }

    //Função auxiliar para gerar uma textura 2D com tamanho e cor configurável
    Texture2D GenerateTexture(int width, int height, Color c)
    {
        Texture2D t = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
            for(int j = 0; j < height; j++)
                t.SetPixel(i, j, c);
        t.Apply();
        return t;
    }


    public void SetExecutedTime(int value)
    {
        executedTime = value;
    }

    public int GetExecutedTime() { return executedTime; }
    public int GetID() { return id; }
    public int GetIncoming() { return incoming; }
    public int GetExecution() { return execution; }
    public int GetQuantum() { return quantum; }
    public Color GetColor() { return color; }
    public Texture2D GetTexture() { return texture; }

}
