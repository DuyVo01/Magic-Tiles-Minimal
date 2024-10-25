using System;
using System.Security.Cryptography;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TileBehavior : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    protected CanvasGroup canvasGroup;

    [SerializeField]
    protected RectTransform upperLimit;

    [SerializeField]
    protected Image alertImage;

    private RectTransform scoringLine;
    protected RectTransform rectTransform;

    private bool hasHitTheScoringLine = false;
    private bool hasPastTheScoringLine = false;
    protected float tileTimelapse;
    public Action onTilePressed;

    public void SetScoringLine(RectTransform scoringLine)
    {
        this.scoringLine = scoringLine;
    }

    public void SetTilePosition(Vector2 position)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        rectTransform.anchoredPosition = position;
    }

    public void SetTilePivot(Vector2 pivot)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        rectTransform.pivot = pivot;
    }

    public void SetTileTimelapse(float timelapse)
    {
        tileTimelapse = timelapse;
    }

    public void SetHeight(float newHeight)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        rectTransform.SetNewRectHeight(newHeight);
    }

    public void EnablePress()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void AdditionalSetup()
    {
        SetCanvasGroup(1, true);
        canvasGroup.blocksRaycasts = false;
        alertImage.gameObject.SetActive(false);
    }

    public virtual void AdditionalReset()
    {
        onTilePressed = null;
    }

    public void OnTilePressed(Action callback)
    {
        onTilePressed = callback;
    }

    public void TileDisable()
    {
        canvasGroup.blocksRaycasts = false;

        alertImage.gameObject.SetActive(true);
        Tween
            .Alpha(
                alertImage,
                startValue: 0,
                endValue: 0.55f,
                0.3f,
                Ease.Linear,
                cycles: 4
            )
            .OnComplete(() =>
            {
                //alertImage.gameObject.SetActive(false);
            });
    }

    protected void SetCanvasGroup(float alpha, bool isInteractable)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = isInteractable;
    }

    protected virtual void Update()
    {
        // Check if the tile hit the scoring line
        if (scoringLine == null)
        {
            return;
        }

        if (!hasHitTheScoringLine)
        {
            if (rectTransform.position.y - scoringLine.position.y < 0.05f)
            {
                hasHitTheScoringLine = true;
            }
        }
        else
        {
            if (upperLimit.position.y < scoringLine.position.y && !hasPastTheScoringLine)
            {
                Debug.Log("Tile passed the line");
                hasPastTheScoringLine = true;
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (canvasGroup.blocksRaycasts)
        {
            onTilePressed?.Invoke();
        }
    }
}
