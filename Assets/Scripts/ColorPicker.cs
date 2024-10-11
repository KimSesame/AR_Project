using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public UnityEvent<Color> OnColorPicked;

    public Color color;

    [SerializeField] Image paletteThumbnail; 
    [SerializeField] Image palette; 
    [SerializeField] Image picker;

    private Image paletteImage;
    private BoxCollider2D paletteCollider;
    private Image paletteThumbnailImage;
    private BoxCollider2D paletteThumbnailCollider;

    private void Awake()
    {
        paletteImage = palette.GetComponent<Image>();
        paletteCollider = palette.GetComponent<BoxCollider2D>();
        paletteThumbnailImage = paletteThumbnail.GetComponent<Image>();
        paletteThumbnailCollider = paletteThumbnail.GetComponent<BoxCollider2D>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MagnifyPalette(true);
        SelectColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SelectColor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MagnifyPalette(false);
        RepositionPicker();
    }

    private void MagnifyPalette(bool flag)
    {
        paletteImage.enabled = flag;
        paletteCollider.enabled = flag;
        paletteThumbnailImage.enabled = !flag;
        paletteThumbnailCollider.enabled = !flag;
    }

    private void RepositionPicker()
    {
        RectTransform paletteRect = palette.GetComponent<RectTransform>();
        RectTransform paletteThumbnailRect = paletteThumbnail.GetComponent<RectTransform>();
        Vector2 localPoint = picker.rectTransform.anchoredPosition;

        // Calculate RectTransfrom coordinate
        float offsetX = paletteRect.rect.width / 2 - paletteThumbnailRect.rect.width / 2;
        float offsetY = (-paletteRect.rect.height / 2) + paletteThumbnailRect.rect.height / 2;
        float newLocalX = localPoint.x * paletteThumbnailRect.rect.width / paletteRect.rect.width + offsetX;
        float newLocalY = localPoint.y * paletteThumbnailRect.rect.height / paletteRect.rect.height + offsetY;

        // Reposition
        picker.rectTransform.anchoredPosition = new Vector2(newLocalX, newLocalY);
    }    

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
        int textureX = Mathf.RoundToInt(ratioX * textureRect.width);
        int textureY = Mathf.RoundToInt(ratioY * textureRect.height);

        pickedColor = paletteTexture.GetPixel(textureX, textureY);

        return pickedColor;
    }
}
