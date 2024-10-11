using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCollector_ScreenCapture : MonoBehaviour
{
    [SerializeField] GameObject tester;

    public void CollectColor()
    {
        Color collectedColor;
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        collectedColor = texture.GetPixel(texture.width / 2, texture.height / 2);

        // Test
        tester.GetComponent<SpriteRenderer>().color = collectedColor;
    }
}
