using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileScroller : MonoBehaviour
{
    [SerializeField]
    private SongObject songObject;

    [SerializeField]
    private GameObject baseTilePrefab;

    [SerializeField]
    private GameObject baseLongTilePrefab;

    [SerializeField]
    private RectTransform scoringLine;


    private RectTransform rectTransform;

    private float totalTimeOfScrolling = 0f;
    private float scrollSpeed = 0f;
    private float[] tilePositionX = new float[4];
    private HashSet<int> occupiedPosX = new HashSet<int>();
    List<TileLineX> tileLines = new List<TileLineX>();

    public RectTransform ThisRectTransform
    {
        get { return rectTransform ??= GetComponent<RectTransform>(); }
        set => rectTransform = value;
    }

    private void Start()
    {
        if (songObject == null || ThisRectTransform == null)
        {
            return;
        }

        // *****************
        float tileSizeX = ThisRectTransform.rect.width / 4;

        for (int i = 0; i < tilePositionX.Length; i++)
        {
            tilePositionX[i] = tileSizeX * i;
        }
        // ******************

        // ********************
        float rectHeight = 0;
        for (int i = 0; i < songObject.totalNumberOfNode; i++)
        {
            float[] tilesPos = GetAvailablePosition(songObject.tiles[i].tileCount);

            for (int j = 0; j < tilesPos.Length; j++)
            {
                tileLines.Add(
                    new TileLineX
                    {
                        lineIndex = i,
                        tileType = songObject.tiles[i].tileType,
                        height = rectHeight,
                        tilePositionsX = tilesPos,
                        lineTimeLapse = songObject.tiles[i].timelapse,
                    }
                );
            }

            rectHeight += GetRectHeightFromTimelapse(songObject.tiles[i].timelapse);

            totalTimeOfScrolling += songObject.tiles[i].timelapse;
        }

        // ********************

        for (int i = 0; i < tileLines.Count; i++)
        {
            TileBehavior tile;

            if (i + 1 < tileLines.Count)
            {
                tileLines[i].nextTileLine = tileLines[i + 1];
            }

            for (int j = 0; j < tileLines[i].tilePositionsX.Length; j++)
            {
                Vector2 tilePos = new Vector2(tileLines[i].tilePositionsX[j], tileLines[i].height);

                if (tileLines[i].tileType == TileType.shortTile)
                {
                    tile = Instantiate(baseTilePrefab, ThisRectTransform)
                        .GetComponent<TileBehavior>();
                }
                else
                {
                    tile = Instantiate(baseLongTilePrefab, ThisRectTransform)
                        .GetComponent<TileBehavior>();
                    tile.SetHeight(GetRectHeightFromTimelapse(tileLines[i].lineTimeLapse));
                }

                if (tile == null)
                {
                    Debug.LogError("Tile is missing its behavioral component");
                    return;
                }

                tile.SetTilePosition(tilePos);
                tile.SetTilePivot(new Vector2(0, 0));
                tile.SetScoringLine(scoringLine);
                tile.SetTileTimelapse(tileLines[i].lineTimeLapse);
                tile.AdditionalSetup();

                tileLines[i].tileItem.Add(tile);
            }
        }

        ThisRectTransform.SetNewRectHeight(rectHeight);
        scrollSpeed = rectHeight / totalTimeOfScrolling;

        ThisRectTransform.anchoredPosition = new Vector2(0, 812);
    }

    private void Update()
    {
        ThisRectTransform.anchoredPosition += new Vector2(0, -scrollSpeed * Time.deltaTime);
    }

    private float GetRectHeightFromTimelapse(float timelapse)
    {
        float baseHeight = 165.511f;
        float baseTimelapse = 0.2307689f;

        float result = timelapse * baseHeight / baseTimelapse;

        return result;
    }

    private float[] GetAvailablePosition(int numberOfPosToGet)
    {
        HashSet<int> availableIndicesOfPosX = new HashSet<int>();

        for (int i = 0; i < tilePositionX.Length; i++)
        {
            if (!occupiedPosX.Contains(i))
            {
                if (availableIndicesOfPosX.Count > 0)
                {
                    if (Mathf.Abs(i - availableIndicesOfPosX.Last()) != 2)
                    {
                        continue;
                    }
                }
                availableIndicesOfPosX.Add(i);
            }
        }

        occupiedPosX.Clear();
        float[] position = new float[numberOfPosToGet];

        for (int i = 0; i < numberOfPosToGet; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableIndicesOfPosX.Count);
            int occupiedIndex = availableIndicesOfPosX.ElementAt(randomIndex);

            position[i] = tilePositionX[occupiedIndex];
            occupiedPosX.Add(occupiedIndex);
            availableIndicesOfPosX.Remove(occupiedIndex);
        }

        return position;
    }
}

public class TileLineX
{
    public int lineIndex;
    public TileType tileType;
    public float[] tilePositionsX;
    public float height;
    public float lineTimeLapse;
    public List<TileBehavior> tileItem;
    public TileLineX nextTileLine;
}
