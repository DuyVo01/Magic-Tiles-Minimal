using UnityEngine;
using UnityEngine.EventSystems;

public class ShortTileBehavior : TileBehavior
{
    public override void AdditionalSetup()
    {
        base.AdditionalSetup();
        gameScore = 1;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Debug.Log("Hit a short tile");
        SetCanvasGroup(0.5f, false);
    }

}
