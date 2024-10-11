using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPainter : Painter
{
    [Header("Color")]
    [SerializeField] Color selectedColor;

    [Header("Canvas")]
    [SerializeField] GameObject canvas;
    [SerializeField] RawImage canvasImage;

    private Texture2D drawingTexture;
    private Vector2 prevPos;

    private void Awake()
    {
        SelectColor(Color.black);
        prevPos = Vector2.zero;
    }

    private void Start()
    {
        canvasImage = canvas.GetComponent<RawImage>();

        InitCanvas();
    }

    public void ToggleCanvas()
    {
        canvas.SetActive(GameManager.Instance.CurMode == GameManager.Mode.Canvas);
    }

    public override void SelectColor(Color color) => selectedColor = color;
    public override void Paint()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    prevPos = touchPos;
                    break;
                // Draw when touch holds
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Draw(touchPos);
                    prevPos = touchPos;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                default:
                    break;
            }
        }
    }

    private void InitCanvas()
    {
        drawingTexture = new Texture2D(512, 512);
        canvasImage.texture = drawingTexture;
        ClearCanvas();
    }

    private void ClearCanvas()
    {
        Color32[] pixels = drawingTexture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        drawingTexture.SetPixels32(pixels);
        drawingTexture.Apply();
    }

    private void Draw(Vector2 position)
    {
        RectTransform canvasRect = canvasImage.GetComponent<RectTransform>();

        // Transform current coordinate Screen to RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, position, null, out Vector2 localPoint);

        // Check canvas boundary
        if (localPoint.x < canvasRect.rect.xMin || localPoint.x > canvasRect.rect.xMax ||
            localPoint.y < canvasRect.rect.yMin || localPoint.y > canvasRect.rect.yMax)
        {
            return;
        }

        float widthRatio = (localPoint.x - canvasRect.rect.xMin) / canvasRect.rect.width;
        float heightRatio = (localPoint.y - canvasRect.rect.yMin) / canvasRect.rect.height;

        int x = Mathf.RoundToInt(widthRatio * drawingTexture.width);
        int y = Mathf.RoundToInt(heightRatio * drawingTexture.height);

        // Transform previous coordinate Screen to RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, prevPos, null, out Vector2 prevLocalPoint);

        float prevWidthRatio = (prevLocalPoint.x - canvasRect.rect.xMin) / canvasRect.rect.width;
        float prevHeightRatio = (prevLocalPoint.y - canvasRect.rect.yMin) / canvasRect.rect.height;

        int prevX = Mathf.RoundToInt(prevWidthRatio * drawingTexture.width);
        int prevY = Mathf.RoundToInt(prevHeightRatio * drawingTexture.height);

        // Draw
        DrawLine(prevX, prevY, x, y, selectedColor);

        drawingTexture.Apply();
        prevPos = position;
    }

    private void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            drawingTexture.SetPixel(x0, y0, color);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }
}
