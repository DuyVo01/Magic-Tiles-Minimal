using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissedTileBehavior : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Image image;
    RectTransform rectTransform;
    private Action onMissedTilePressed;

    private bool hasPressed;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hasPressed)
        {
            return;
        }
        hasPressed = true;
        onMissedTilePressed?.Invoke();
        Tween
            .Alpha(image, 0, 0.55f, 0.2f, Ease.Linear, cycles: 4)
            .OnComplete(() =>
            {
                GameManager.Instance.SetGameOver(true);
            });
    }

    public void OnMissedTilePressed(Action callback)
    {
        onMissedTilePressed = callback;
    }

    public void SetPos(Vector2 newPos)
    {
        rectTransform.anchoredPosition = newPos;
    }

    public void SetSize(Vector2 newSize)
    {
        rectTransform.SetNewRectHeight(newSize.y);
        rectTransform.SetNewRectWidth(newSize.x);
    }

    public void ResetStats()
    {
        //gameObject.SetActive(false);
        Color color = image.color;
        color.a = 0;
        image.color = color;
        hasPressed = false;
    }

    public void DisablePress()
    {
        hasPressed = true;
    }
}
