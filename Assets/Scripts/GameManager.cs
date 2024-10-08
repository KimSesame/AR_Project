using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance {  get; private set; }

    [Header("Painter")]
    public Painter curPainter;
    public ArPainter arPainter;
    public CanvasPainter canvasPainter;

    public Painter CurPainter { get { return curPainter; } private set { curPainter = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        curPainter = arPainter;
    }

    private void Update()
    {
        curPainter.Paint();
    }

    public void TogglePainter()
    {
        if (curPainter.Equals(arPainter))
            curPainter = canvasPainter;
        else if (curPainter.Equals(canvasPainter))
            curPainter = arPainter;
    }
}
