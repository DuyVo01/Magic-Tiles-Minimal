using System;
using System.Collections.Generic;
using UnityEngine;

public class TileScroller : MonoBehaviour
{
    [SerializeField]
    private SongObject songObject;

    [SerializeField]
    private GameObject baseTile;

    [SerializeField]
    private RectTransform scoringLine;
    private RectTransform rectTransform;
    public RectTransform ThisRectTransform
    {
        get
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            return rectTransform;
        }
        set => rectTransform = value;
    }

    private float totalTimeOfScrolling = 0f;
    private float scrollSpeed = 0f;

    private Vector2[] tilePositions;
    private float[] tilePositionX = new float[4];
    private int lastPositionXSpawned = 0;

    private void Start()
    {
        if (songObject == null || ThisRectTransform == null)
        {
            return;
        }

        float tileSizeX = ThisRectTransform.rect.width / 4;

        for (int i = 0; i < tilePositionX.Length; i++)
        {
            tilePositionX[i] = tileSizeX * i;
        }
        tilePositions = new Vector2[songObject.totalNumberOfNode];

        float rectHeight = 0;

        for (int i = 0; i < songObject.totalNumberOfNode; i++)
        {
            totalTimeOfScrolling += songObject.noteTimeLapse[i];
            int randomIndexForTilePositionX;

            do
            {
                randomIndexForTilePositionX = UnityEngine.Random.Range(0, tilePositionX.Length);
            } while (lastPositionXSpawned == randomIndexForTilePositionX);

            tilePositions[i] = new Vector2(tilePositionX[randomIndexForTilePositionX], rectHeight);

            rectHeight += GetRectHeightFromTimelapse(songObject.noteTimeLapse[i]);

            lastPositionXSpawned = randomIndexForTilePositionX;
        }

        for (int i = 0; i < tilePositions.Length; i++)
        {
            TileBehavior tile = Instantiate(baseTile, ThisRectTransform)
                .GetComponent<TileBehavior>();
            if (tile == null)
            {
                Debug.LogError("Tile is missing its behavioral component");
                return;
            }
            Vector2 pos = tilePositions[i];
            tile.SetTilePosition(pos);
            tile.SetTilePivot(new Vector2(0, 0));
            tile.SetScoringLine(scoringLine);
            tile.SetTileTimelapse(songObject.noteTimeLapse[i]);
        }

        ThisRectTransform.SetNewRectHeight(rectHeight);
        scrollSpeed = rectHeight / totalTimeOfScrolling;

        ThisRectTransform.anchoredPosition = new Vector2(0, 812);

        Debug.Log($"Scroll speed {scrollSpeed}");
        Debug.Log($"Total scrolling Time {totalTimeOfScrolling}");
    }

    private void Update()
    {
        //ThisRectTransform.anchoredPosition += new Vector2(0, -scrollSpeed * Time.deltaTime);
    }

    private float GetRectHeightFromTimelapse(float timelapse)
    {
        float baseHeight = 165.511f;
        float baseTimelapse = 0.2307689f;

        float result = timelapse * baseHeight / baseTimelapse;

        return result;
    }
}
