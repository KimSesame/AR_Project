using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCollector_ScreenCapture : MonoBehaviour
{
    public static Action<Color> OnColorCollected;
    [SerializeField] GameObject viewer;
    [SerializeField] RawImage rIamge;

    public void CollectColor()
    {
        Color collectedColor;
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        rIamge.texture = texture;

        // Get color of center pixel
        collectedColor = texture.GetPixel(texture.width / 2, texture.height / 2);
        OnColorCollected?.Invoke(collectedColor);

        // View
        viewer.GetComponent<Image>().color = collectedColor;
    }
}
