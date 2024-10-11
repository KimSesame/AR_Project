using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArBrush : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] Color selectedColor;

    private SpriteRenderer spRenderer;

    private void Awake()
    {
        spRenderer = GetComponent<SpriteRenderer>();

        SelectColor(Color.black);
    }

    public void ToggleBrush()
    {
        spRenderer.enabled = (GameManager.Instance.CurMode == GameManager.Mode.AR);
    }

    public void SelectColor(Color color)
    {
        selectedColor = color;
        spRenderer.color = selectedColor;
    }
}
