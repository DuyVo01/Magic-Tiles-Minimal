using Unity.VisualScripting;
using UnityEngine;

public static class GlobalExtension 
{
    public static void SetNewRectHeight(this RectTransform rectTransform, float newHeight )
    {
        float i = rectTransform.sizeDelta.y - rectTransform.rect.height + newHeight;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, i);
    }
    public static void SetNewRectWidth(this RectTransform rectTransform, float newWidth )
    {
        float i = rectTransform.sizeDelta.x - rectTransform.rect.width + newWidth;
        rectTransform.sizeDelta = new Vector2(i, rectTransform.sizeDelta.y);
    }
    public static bool IsNull(this Object obj)
    {
        if(obj == null)
        {
            return true;
        }
        return false;
    }
}
