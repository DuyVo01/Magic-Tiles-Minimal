using PrimeTween;
using UnityEngine;

public class HandGestureAnimation : MonoBehaviour
{
    [SerializeField]
    Vector2 startPostion;

    [SerializeField]
    Vector2 endPosition;

    [SerializeField]
    Ease startEase;

    [SerializeField]
    Ease endEase;
    RectTransform rectTransform;
    Sequence tweenGesture;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        tweenGesture = Sequence
            .Create(cycles: 100)
            .Chain(
                Tween.UIAnchoredPosition(
                    target: rectTransform,
                    endValue: endPosition,
                    duration: 1,
                    ease: startEase
                )
            )
            .Chain(
                Tween.UIAnchoredPosition(
                    target: rectTransform,
                    endValue: startPostion,
                    duration: 1,
                    ease: endEase
                )
            );
    }
}
