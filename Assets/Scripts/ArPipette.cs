using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArPipette : MonoBehaviour
{
    private SpriteRenderer spRenderer;

    private void Awake()
    {
        spRenderer = GetComponent<SpriteRenderer>();
    }

    public void TogglePipette()
    {
        spRenderer.enabled = (GameManager.Instance.CurMode == GameManager.Mode.Collect);
    }
}
