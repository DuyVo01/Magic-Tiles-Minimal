using System;
using PrimeTween;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;
    CanvasGroup canvasGroup;

    IDisposable subcription;

    RectTransform buttonRect;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        buttonRect = restartButton.GetComponent<RectTransform>();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        subcription = GameManager.Instance.IsGameOver.Subscribe(g =>
        {
            if (g)
            {
                ActiveGameOverUI();
            }
        });

        restartButton.onClick.AddListener(Restart);
    }

    void OnDestroy()
    {
        subcription.Dispose();
        restartButton.onClick.RemoveListener(Restart);
    }

    private void ActiveGameOverUI()
    {
        Tween
            .Alpha(target: canvasGroup, endValue: 1, duration: 2f, ease: Ease.Linear)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;

                Sequence
                    .Create(cycles: 100)
                    .Chain(
                        Tween.Scale(
                            target: buttonRect,
                            endValue: Vector2.one * 1.1f,
                            duration: 0.15f,
                            Ease.Linear
                        )
                    )
                    .Chain(
                        Tween.Scale(
                            target: buttonRect,
                            endValue: Vector2.one,
                            duration: 0.15f,
                            Ease.Linear
                        )
                    )
                    .Chain(Tween.Delay(duration: .5f));
            });
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
