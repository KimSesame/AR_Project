using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArPipette : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void TogglePipette()
    {
        image.enabled = (GameManager.Instance.CurMode == GameManager.Mode.Collect);
    }
}
