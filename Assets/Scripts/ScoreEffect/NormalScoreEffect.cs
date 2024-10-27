using PrimeTween;
using R3;
using UnityEngine;

public class NormalScoreEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private RectTransform rectTransform;

    private CanvasGroup canvasGroup;

    DisposableBag disposableBag;

    Sequence sequence;
    Sequence sequenceAfterDelay;
    Tween delay;

    void Start()
    {
        canvasGroup = rectTransform.GetComponent<CanvasGroup>();

        GameManager
            .Instance.HasHitNormalScore.Subscribe(hasHitNormalScore =>
            {
                if (hasHitNormalScore)
                {
                    TriggerEffect();
                }
            })
            .AddTo(ref disposableBag);

        particle?.Stop();
    }

    private void TriggerEffect()
    {
        sequence.Stop();
        delay.Stop();
        sequenceAfterDelay.Stop();
        particle?.Play();

        sequence = Tween
            .Scale(
                target: rectTransform,
                startValue: Vector2.zero,
                endValue: Vector2.one,
                duration: 0.1f,
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
                    duration: .3f,
                    () =>
                    {
                        sequenceAfterDelay = Tween
                            .Scale(
                                target: rectTransform,
                                endValue: Vector2.zero,
                                duration: 0.1f,
                                ease: Ease.Linear
                            )
                            .Group(
                                Tween.Alpha(
                                    target: canvasGroup,
                                    endValue: 0,
                                    duration: 0.1f,
                                    ease: Ease.Linear
                                )
                            )
                            .OnComplete(() =>
                            {
                                rectTransform.localScale = Vector2.zero;
                                particle?.Stop();
                            });
                    }
                );
            });
    }
}
