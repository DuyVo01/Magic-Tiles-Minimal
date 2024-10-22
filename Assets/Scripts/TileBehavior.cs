using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TileBehavior : MonoBehaviour
{
    private RectTransform scoringLine;
    private RectTransform rectTransform;
    private bool hasHitTheScoringLine = false;
    protected float tileTimelapse;

    public  void SetScoringLine(RectTransform scoringLine)
    {
        this.scoringLine = scoringLine;
    }

    public  void SetTilePosition(Vector2 position)
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        rectTransform.anchoredPosition = position;
    }

    public  void SetTilePivot(Vector2 pivot)
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

    protected virtual void Update()
    {
        // Check if the tile hit the scoring line
        if (scoringLine == null)
        {
            return;
        }

        if (hasHitTheScoringLine)
        {
            return;
        }

        if (rectTransform.position.y - scoringLine.position.y < 0.05f)
        {
            hasHitTheScoringLine = true;
            Debug.Log("Tile hit scoring line");
        }
    }
}
