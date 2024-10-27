using PrimeTween;
using R3;
using UnityEngine;

public class PerfectScoreEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private RectTransform perfectScoreRect;

    [SerializeField]
    private float perfectScoreTweenDuration;

    [SerializeField]
    private Ease perfectScoreTweenMode;

    [SerializeField]
    private RectTransform backgroundEffectRect;

    [SerializeField]
    private float backgroundEffectTweenDuration;

    private CanvasGroup canvasGroup;

    private DisposableBag subscriptions;

    private float backgroundEffectDesiredX = -50;

    Sequence sequence;
    Sequence sequenceAfterDelay;
    Tween delay;
    Vector2 endPerfectScoreTweenValue = Vector2.one;
    Vector2 startPerfectScoreTweenValue = Vector2.zero;

    void Start()
    {
        canvasGroup = perfectScoreRect.GetComponent<CanvasGroup>();
        GameManager
            .Instance.HasHitPerfectScore.Subscribe(hasHitPerfectScore =>
            {
                if (hasHitPerfectScore)
                {
                    TriggerEffect();
                }
            })
            .AddTo(ref subscriptions);

        particle.Stop();
    }

    void OnDestroy()
    {
        subscriptions.Dispose();
    }

    private void TriggerEffect()
    {
        sequence.Stop();
        sequenceAfterDelay.Stop();
        delay.Stop();
        particle.Play();

        sequence = Tween
            .Scale(
                target: perfectScoreRect,
                startValue: startPerfectScoreTweenValue,
                endValue: endPerfectScoreTweenValue,
                duration: perfectScoreTweenDuration,
                ease: Ease.OutBack
            )
            .Group(
                Tween.Alpha(
                    target: canvasGroup,
                    startValue: 0,
                    endValue: 1,
                    duration: 0.1f,
                    ease: Ease.Linear
                )
            )
            .OnComplete(() =>
            {
                delay = Tween.Delay(
                    target: perfectScoreRect,
                    duration: .3f,
                    () =>
                    {
                        sequenceAfterDelay = Tween
                            .Alpha(
                                target: canvasGroup,
                                endValue: 0,
                                duration: 0.1f,
                                ease: Ease.Linear
                            )
                            .Group(
                                Tween.Scale(
                                    target: perfectScoreRect,
                                    endValue: endPerfectScoreTweenValue * 0.75f,
                                    duration: 0.1f,
                                    ease: Ease.Linear
                                )
                            )
                            .OnComplete(() =>
                            {
                                perfectScoreRect.localScale = Vector2.zero;
                                particle.Stop();
                            });
                    }
                );
            });
        

        Tween.UIAnchoredPositionX(
            target: backgroundEffectRect,
            startValue: 0,
            endValue: backgroundEffectDesiredX,
            duration: backgroundEffectTweenDuration,
            ease: Ease.Linear
        );
    }
}
