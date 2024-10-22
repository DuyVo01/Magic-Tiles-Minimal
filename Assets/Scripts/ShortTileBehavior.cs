using UnityEngine;
using UnityEngine.EventSystems;

public class ShortTileBehavior : TileBehavior, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Hit a short tile");
    }
}
