using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArPainter : Painter
{
    [Header("Brush")]
    [SerializeField] Transform arBrush;

    [Header("Line")]
    [SerializeField] Transform linePool;
    [SerializeField] GameObject linePrefab;
    [SerializeField] List<LineRenderer> lineList = new();

    private LineRenderer lineRenderer;

    public override void Paint()
    {
        if(isUsed && startLine)
        {
            DrawLine();
        }
    }

    private bool isUsed;
    private bool startLine;
    public void StartDrawLine()
    {
        isUsed = true;
        if (!startLine)
        {
            NextLine();
        }
    }

    public void StopDrawLine()
    {
        isUsed = false;
        startLine = false;
        lineRenderer = null;
    }

    private void NextLine()
    {
        // Generate line
        GameObject newLine = Instantiate(linePrefab);
        newLine.transform.SetParent(linePool);
        newLine.transform.position = Vector3.zero;
        newLine.transform.localScale = Vector3.one;

        // Set line
        lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, arBrush.position);

        startLine = true;
        lineList.Add(lineRenderer);
    }

    private void DrawLine()
    {
        lineRenderer.SetPosition(lineRenderer.positionCount++, arBrush.position);
    }
}
