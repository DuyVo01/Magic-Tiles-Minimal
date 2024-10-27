using System;
using PrimeTween;
using R3;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Ease ease;

    IDisposable subscription;

    Sequence tween;

    RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        subscription = GameManager.Instance.GameScore.Subscribe(s =>
        {
            scoreText.text = s.ToString();

            tween.Stop();
            tween = Tween
                .Scale(
                    target: rectTransform,
                    startValue: Vector2.one,
                    endValue: Vector2.one * 1.15f,
                    duration: 0.1f,
                    ease: ease
                )
                .Chain(
                    Tween.Scale(
                        target: rectTransform,
                        endValue: Vector2.one,
                        duration: 0.1f,
                        ease: ease
                    )
                );
        });
    }

    void OnDestroy()
    {
        subscription.Dispose();
    }
}
