using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ScoreProgressEffect : MonoBehaviour
{
    [SerializeField]
    private Sprite idleSprite;

    [SerializeField]
    private Sprite scoredSprite;

    [SerializeField]
    private bool isUseRotation;

    private RectTransform rectTransform;
    private Image imageIcon;

    Sequence sequence;
    Tween tweenAfterDelay;
    Tween delay;
    Tween rotationTween;

    Vector3 rotationStart = new Vector3(0, 0, -180);
    Vector3 rotationEnd = new Vector3(0, 0, -360);

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageIcon = GetComponent<Image>();

        imageIcon.sprite = idleSprite;
    }

    public void TriggerEffect()
    {
        imageIcon.sprite = scoredSprite;

        sequence.Stop();
        rotationTween.Stop();
        delay.Stop();

        sequence = Tween
            .Scale(
                target: rectTransform,
                startValue: Vector2.one,
                endValue: Vector2.one * 3,
                duration: 0.2f,
                ease: Ease.Linear
            )
            .Chain(Tween.Scale(target: rectTransform, endValue: Vector3.one * 1.5f, duration: 0.2f))
            .OnComplete(() =>
            {
                delay = Tween.Delay(
                    duration: 1,
                    () =>
                    {
                        tweenAfterDelay = Tween.Scale(
                            target: rectTransform,
                            startValue: Vector3.one * 1.5f,
                            endValue: Vector2.one,
                            duration: 0.1f,
                            ease: Ease.Linear
                        );
                    }
                );
            });

        if (isUseRotation)
        {
            rectTransform.rotation = quaternion.Euler(new Vector3(0, 0, -180));

            rotationTween = Tween.Rotation(
                target: rectTransform,
                startValue: rotationStart,
                endValue: rotationEnd,
                duration: 0.5f,
                ease: Ease.OutSine
            );
        }
    }
}
