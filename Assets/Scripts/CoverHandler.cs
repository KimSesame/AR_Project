using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CoverData
{
    public bool[] covers = new bool[8 * 9];
}

public class CoverHandler : MonoBehaviour
{
    [SerializeField] Transform paletteCovers;
    [SerializeField] Transform paletteThumbnailCovers;
    [SerializeField] Texture2D paletteTexture;

    private const int COVERS_WIDTH = 9;
    private const int COVERS_HEIGHT = 8;
    private bool[,] covers = new bool[COVERS_HEIGHT, COVERS_WIDTH];
    private Color[] paletteColors;

    private void Start()
    {
        ColorCollector_ScreenCapture.OnColorCollected += RevealArea;

        paletteColors = paletteTexture.GetPixels();

        LoadCoversState();
        InitCovers();
    }

    private void OnDestroy()
    {
        ColorCollector_ScreenCapture.OnColorCollected -= RevealArea;
    }

    private void InitCovers()
    {
        for (int i = 0; i < COVERS_HEIGHT; i++)
            for (int j = 0; j < COVERS_WIDTH; j++)
            {
                if (covers[i, j] == false)
                {
                    RevealArea(i, j);
                }
            }
    }

    private void RevealArea(Color color)
    {
        // Skip gray-scale color
        if (Mathf.Approximately(color.r, color.g) && Mathf.Approximately(color.g, color.b))
            return;

        // Match color to cover
        Vector2Int closestCover = FindClosestCover(color);
        if (closestCover == new Vector2Int(-1, -1))
            return;

        Vector2Int gridCoords = PixelToGridCoords(closestCover);

        RevealArea(gridCoords.x, gridCoords.y);
        SaveCoversState();
    }

    private void RevealArea(int w, int h)
    {
        int targetChild = w * COVERS_WIDTH + h;
        
        // Reveal
        covers[w, h] = false;
        paletteCovers.GetChild(targetChild).gameObject.SetActive(false);
        paletteThumbnailCovers.GetChild(targetChild).gameObject.SetActive(false);
    }

    private string GetSaveFilePath() => Application.persistentDataPath + "/CoversData1.json";
    private void SaveCoversState()
    {
        Debug.Log("SaveCoversState");
        CoverData coverData = new CoverData();

        // Copy
        for (int i = 0; i < COVERS_HEIGHT; i++)
            for (int j = 0; j < COVERS_WIDTH; j++)
            {
                coverData.covers[i * COVERS_WIDTH + j] = covers[i, j];
            }

        // Save
        string json = JsonUtility.ToJson(coverData);
        File.WriteAllText(GetSaveFilePath(), json);

        Debug.Log($"{json}");
    }

    private void LoadCoversState()
    {
        Debug.Log("LoadCoversState");
        string path = GetSaveFilePath();

        // Load json
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CoverData coverData = JsonUtility.FromJson<CoverData>(json);

            // Load
            for (int i = 0; i < COVERS_HEIGHT; i++)
                for (int j = 0; j < COVERS_WIDTH; j++)
                {
                    covers[i, j] = coverData.covers[i * COVERS_WIDTH + j];
                }
        }
        // Make json
        else
        {
            // Init covers array
            for (int i = 0; i < COVERS_HEIGHT; i++)
                for (int j = 0; j < COVERS_WIDTH; j++)
                {
                    covers[i, j] = true;
                }

            SaveCoversState();
        }
    }

    private Vector2Int FindClosestCover(Color targetColor)
    {
        float minDistance = float.MaxValue;
        Vector2Int closestCover = new Vector2Int(-1, -1);

        // Inspect all pixels
        for (int y = 0; y < paletteTexture.height; y++)
            for (int x = 0; x < paletteTexture.width; x++)
            {
                Color pixelColor = paletteColors[y * paletteTexture.width + x];
                float distance = ColorDistance(targetColor, pixelColor);

                // Update min color distance
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCover = new Vector2Int(x, y);
                }
            }

        return closestCover;
    }

    private float ColorDistance(Color color1, Color color2)
    {
        float rDiff = color1.r - color2.r;
        float gDiff = color1.g - color2.g;
        float bDiff = color1.b - color2.b;
        return rDiff * rDiff + gDiff * gDiff + bDiff * bDiff;
    }

    private Vector2Int PixelToGridCoords(Vector2Int pixelCoords)
    {
        float cellWidth = (float)paletteTexture.width / COVERS_WIDTH;
        float cellHeight = (float)paletteTexture.height / COVERS_HEIGHT;
        int gridW = Mathf.FloorToInt(pixelCoords.x / cellWidth);
        int gridH = Mathf.FloorToInt(pixelCoords.y / cellHeight);

        // Transform
        gridW = Mathf.Clamp(gridW, 0, COVERS_WIDTH - 1);
        gridH = Mathf.Clamp(gridH, 0, COVERS_HEIGHT - 1);

        return new Vector2Int(gridH, gridW);
    }
}
