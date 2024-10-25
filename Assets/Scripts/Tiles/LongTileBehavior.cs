using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongTileBehavior : TileBehavior, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform filler;
    CountdownTimer countdown;
    private bool hasPressed;
    private float fillerTime;

    Tween fillerTween;
    private Vector2 fillerOriginalSize;

    public override void AdditionalSetup()
    {
        base.AdditionalSetup();
        countdown = new CountdownTimer(tileTimelapse);
        hasPressed = false;

        fillerTime = tileTimelapse / 1.7f;
        fillerOriginalSize = filler.sizeDelta;
        filler.gameObject.SetActive(false);

        countdown.Reset();
    }

    public override void AdditionalReset()
    {
        base.AdditionalReset();
        filler.sizeDelta = fillerOriginalSize;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (eventData.position.y > upperLimit.position.y)
        {
            Debug.Log("outside the touchable zone");

            return;
        }
        hasPressed = true;
        Debug.Log("hit and hold a long tile");

        countdown.Start();
        filler.gameObject.SetActive(true);
        fillerTween = Tween
            .UISizeDelta(
                filler,
                new Vector2(filler.sizeDelta.x, rectTransform.rect.height),
                fillerTime,
                Ease.Linear
            )
            .OnComplete(() =>
            {
                SetCanvasGroup(0.5f, false);
            });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        fillerTween.Stop();
        countdown.Stop();
        SetCanvasGroup(0.5f, false);
        Debug.Log("a long tile is incomplete");
    }

    protected override void Update()
    {
        base.Update();
        if (!hasPressed)
        {
            return;
        }
    }
}
