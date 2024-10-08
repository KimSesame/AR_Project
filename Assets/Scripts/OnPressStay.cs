using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPressStay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] ArPainter arPainter;

    private bool isPressed = false;

    private void Start()
    {
        arPainter = GameManager.Instance.arPainter;
    }

    private void Update()
    {
        if (isPressed)
        {
            arPainter.StartDrawLine();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {   
        isPressed = false; 
        arPainter.StopDrawLine();
    }
}
