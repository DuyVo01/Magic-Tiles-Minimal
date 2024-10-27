using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongTileBehavior : TileBehavior, IPointerUpHandler
{
    [SerializeField]
    private RectTransform filler;
    CountdownTimer countdown;
    private bool hasPressed;
    private float fillerTime;

    Tween fillerTween;
    private Vector2 fillerOriginalSize;
    private bool hasCompletedPress;

    public override void AdditionalSetup()
    {
        base.AdditionalSetup();
        gameScore = 2;
        countdown = new CountdownTimer(tileTimelapse);
        hasPressed = false;

        fillerTime = tileTimelapse;
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
        }
        hasPressed = true;
        hasCompletedPress = false;
        Debug.Log("hit and hold a long tile");

        countdown.Start();
        filler.gameObject.SetActive(true);

        // Convert press position to local coordinates
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.pressPosition,
            eventData.pressEventCamera,
            out localPoint
        );

        float offsetPressY = localPoint.y + 132/2;

        filler.SetNewRectHeight(offsetPressY);
        float remainingDistance = rectTransform.rect.height - offsetPressY;

        fillerTime = remainingDistance / rectTransform.rect.height * tileTimelapse;

        fillerTween = Tween
            .UISizeDelta(
                target: filler,
                endValue: new Vector2(filler.sizeDelta.x, rectTransform.rect.height),
                duration: fillerTime,
                ease: Ease.Linear
            )
            .OnComplete(() =>
            {
                SetCanvasGroup(0.5f, false);
                hasCompletedPress = true;
                GameManager.Instance.AddGameScore(2);
            });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        fillerTween.Stop();
        countdown.Stop();
        SetCanvasGroup(0.5f, false);

        if (!hasCompletedPress)
        {
            Debug.Log("a long tile is incomplete");
        }
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
