using UnityEngine;
using UnityEngine.EventSystems;

public class ShortTileBehavior : TileBehavior, IPointerDownHandler
{
    public override void AdditionalSetup()
    {
        base.AdditionalSetup();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Debug.Log("Hit a short tile");
        SetCanvasGroup(0.5f, false);
    }

}
