using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public UnityEvent<Mode> OnModeChanged;

    public enum Mode
    {
        AR, Canvas, Collect
    }

    static public GameManager Instance {  get; private set; }

    [SerializeField] Mode curMode;

    [Header("Painter")]
    public Painter curPainter;
    public ArPainter arPainter;
    public CanvasPainter canvasPainter;
    private Mode lastMode;

    public Mode CurMode { get { return curMode; } set { curMode = value; OnModeChanged?.Invoke(curMode); } }

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
        CurMode = Mode.AR;
        curPainter = arPainter;
    }

    private void Update()
    {
        switch (CurMode)
        {
            case Mode.AR:
                curPainter = arPainter;
                curPainter.Paint();
                break;
            case Mode.Canvas:
                curPainter = canvasPainter;
                curPainter.Paint();
                break;
            case Mode.Collect:
                break;
            default:
                break;
        }
    }

    public void ChangeMode(Mode newMode)
    {
        lastMode = CurMode;
        CurMode = newMode;
    }

    public void TogglePainter()
    {
        if (CurMode == Mode.AR)
            ChangeMode(Mode.Canvas);
        else if (CurMode == Mode.Canvas)
            ChangeMode(Mode.AR);
    }

    public void ToggleCollector()
    {
        if (CurMode != Mode.Collect)
            ChangeMode(Mode.Collect);
        else
            ChangeMode(lastMode);
    }
}
