using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArPainter : Painter
{
    [Header("Color")]
    [SerializeField] Color selectedColor;

    [Header("Brush")]
    [SerializeField] Transform arBrush;

    [Header("Line")]
    [SerializeField] Transform linePool;
    [SerializeField] GameObject linePrefab;
    [SerializeField] List<LineRenderer> lineList = new();

    private LineRenderer lineRenderer;

    private void Awake()
    {
        SelectColor(Color.black);
    }

    public override void SelectColor(Color color) => selectedColor = color;
    public override void Paint()
    {
        if (startLine)
        {
            DrawLine();
        }
    }

    private bool startLine;
    public void StartDrawLine()
    {
        if (!startLine)
        {
            NextLine();
            startLine = true;
        }
    }

    public void StopDrawLine()
    {
        startLine = false;
        lineRenderer = null;
    }

    private void NextLine()
    {
        // Generate line
        GameObject newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, linePool);
        newLine.transform.localScale = Vector3.one;

        // Set line
        lineRenderer = newLine.GetComponent<LineRenderer>();
        lineRenderer.startColor = selectedColor;
        lineRenderer.endColor = selectedColor;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, arBrush.position);

        lineList.Add(lineRenderer);
    }

    private void DrawLine()
    {
        lineRenderer.SetPosition(lineRenderer.positionCount++, arBrush.position);
    }
}
