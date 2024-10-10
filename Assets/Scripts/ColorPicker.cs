using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public UnityEvent<Color> OnColorPicked;

    public Color color;

    [SerializeField] Image palette; 
    [SerializeField] Image picker;

    private void SelectColor()
    {
        Touch touch = Input.GetTouch(0);
        RectTransform paletteRect = palette.GetComponent<RectTransform>();

        // Transform palette coordinate Screen to RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(paletteRect, touch.position, null, out Vector2 localPoint);

        // Restrict picker be inside palette boundary
        localPoint.x = Mathf.Clamp(localPoint.x, -paletteRect.rect.width / 2, paletteRect.rect.width / 2);
        localPoint.y = Mathf.Clamp(localPoint.y, -paletteRect.rect.height / 2, paletteRect.rect.height / 2);

        picker.rectTransform.anchoredPosition = localPoint;
        
        color = GetColorFromPalette();
        OnColorPicked?.Invoke(color);
    }

    private Color GetColorFromPalette()
    {
        Color pickedColor;

        RectTransform paletteRect = palette.GetComponent<RectTransform>();
        Texture2D paletteTexture = palette.sprite.texture;
        Rect textureRect = palette.sprite.textureRect;
        Vector2 localPoint = picker.rectTransform.anchoredPosition;

        // Calculate texture coordinate
        float ratioX = (localPoint.x + paletteRect.rect.width / 2) / paletteRect.rect.width;
        float ratioY = (localPoint.y + paletteRect.rect.height / 2) / paletteRect.rect.height;
        int textureX = Mathf.RoundToInt(ratioX * textureRect.width + textureRect.x);
        int textureY = Mathf.RoundToInt(ratioY * textureRect.height + textureRect.y);

        pickedColor = paletteTexture.GetPixel(textureX, textureY);

        return pickedColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SelectColor();
    }
}
