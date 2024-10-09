using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    private RawImage rawImage;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    public void OnTransparencyValueChanged(float value)
    {
        Color newColor = rawImage.color;
        newColor.a = value;

        rawImage.color = newColor;
    }
}
