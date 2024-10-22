using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongTileBehavior : TileBehavior, IPointerDownHandler, IPointerUpHandler
{
    CountdownTimer countdown;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("hit and hold a long tile");
        countdown = new CountdownTimer(tileTimelapse);
        countdown.Start();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        countdown.Stop();
        Debug.Log("a long tile is incomplete");
    }

    protected override void Update()
    {
        base.Update();
        if (countdown.IsFinished)
        {
            Debug.Log("Complete a long tile");
            countdown.Dispose();
        }
    }
}
