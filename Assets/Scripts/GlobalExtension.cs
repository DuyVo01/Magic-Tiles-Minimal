using Unity.VisualScripting;
using UnityEngine;

public static class GlobalExtension 
{
    public static void SetNewRectHeight(this RectTransform rectTransform, float newHeight )
    {
        float i = rectTransform.sizeDelta.y - rectTransform.rect.height + newHeight;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, i);
    }
}
