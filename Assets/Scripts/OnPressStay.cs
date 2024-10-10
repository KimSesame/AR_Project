using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPressStay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] ArPainter arPainter;

    private void Start()
    {
        arPainter = GameManager.Instance.arPainter;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        drawingCoroutine ??= StartCoroutine(DrawingRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (drawingCoroutine != null)
        {
            StopCoroutine(drawingCoroutine);
            drawingCoroutine = null;
        }

        arPainter.StopDrawLine();
    }

    Coroutine drawingCoroutine;
    IEnumerator DrawingRoutine()
    {
        arPainter.StartDrawLine();
        while (true)
        {
            yield return null;
        }
    }
}
