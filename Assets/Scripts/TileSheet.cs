using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using R3;
using UnityEngine;

public class TileSheet : MonoBehaviour
{
    [SerializeField]
    private SongObject songObject;

    [SerializeField]
    private GameObject tileLinePrefab;

    [SerializeField]
    private RectTransform scoringLine;

    [SerializeField]
    private RectTransform bottom;

    [SerializeField]
    private bool isDebug;

    [SerializeField]
    private float debugSpeedReduction;

    [SerializeField]
    private Vector2 restartPosition;

    private RectTransform rectTransform;

    private float totalTimeOfScrolling = 0f;
    private float scrollSpeed = 0f;
    private float[] tilePositionX = new float[4];
    private HashSet<int> occupiedPosX = new HashSet<int>();
    private float currentRectHeight = 0;
    private int lastSpawnedIndex = 0;
    private TileLine lastSpawnedTileLine;

    private GameObjectPool tileLinePool;

    private bool shouldScroll;

    DisposableBag subscription;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        shouldScroll = true;
        tileLinePool = new GameObjectPool(
            tileLinePrefab.gameObject,
            songObject.totalNumberOfNode,
            transform
        );

        float tileSizeX = rectTransform.rect.width / 4;

        for (int i = 0; i < tilePositionX.Length; i++)
        {
            tilePositionX[i] = tileSizeX * i;
        }

        float preCalculateHeight = 0;
        for (int i = 0; i < songObject.totalNumberOfNode; i++)
        {
            float height = GetRectHeightFromTimelapse(songObject.tiles[i].timelapse);

            if (i <= 5)
            {
                ConfigureTile(i);
            }
            preCalculateHeight += height;

            totalTimeOfScrolling += songObject.tiles[i].timelapse;
        }
        rectTransform.SetNewRectHeight(preCalculateHeight);
        scrollSpeed = preCalculateHeight / totalTimeOfScrolling;

        GameManager.Instance.SetSong(songObject);

        GameManager
            .Instance.IsGameRunning.Subscribe(r =>
            {
                shouldScroll = r;
            })
            .AddTo(ref subscription);
    }

    void OnDestroy()
    {
        subscription.Dispose();
    }

    private void Update()
    {
        if (!shouldScroll)
        {
            return;
        }
        if (isDebug)
        {
            rectTransform.anchoredPosition += new Vector2(
                0,
                -scrollSpeed * Time.deltaTime / debugSpeedReduction
            );
        }
        else
        {
            rectTransform.anchoredPosition += new Vector2(0, -scrollSpeed * Time.deltaTime);
        }

        if (GameManager.Instance.NumberOfTileLineLeft == 0)
        {
            RestartSong();
        }
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

    private void AddNewTileLine()
    {
        int spawnIndex = lastSpawnedIndex + 1;
        if (spawnIndex > songObject.totalNumberOfNode - 1)
        {
            return;
        }

        ConfigureTile(spawnIndex);
    }

    private void ConfigureTile(int index)
    {
        float height = GetRectHeightFromTimelapse(songObject.tiles[index].timelapse);

        float[] tilesPos = GetAvailablePosition(songObject.tiles[index].tileCount);

        TileLine tileLine = tileLinePool.Get().GetComponent<TileLine>();
        tileLine.ReSetState();

        tileLine.SetSize(
            new Vector2(
                rectTransform.rect.width,
                GetRectHeightFromTimelapse(songObject.tiles[index].timelapse)
            )
        );
        tileLine.SetPositionY(currentRectHeight);
        tileLine.SetScoringLine(scoringLine);
        tileLine.GenerateTiles(
            tilesPos,
            songObject.tiles[index].tileType,
            songObject.tiles[index].timelapse
        );
        tileLine.OnPassingTheBottomLine(
            (tileLine) =>
            {
                tileLinePool.Return(tileLine.gameObject);
                AddNewTileLine();
            }
        );
        tileLine.SetBottomPoint(bottom.position);

        if (index == 0)
        {
            tileLine.EnableTiles();
            tileLine.SetFirstTile();
        }
        tileLine.OnPassingTheLineUnPressed(HandlerUnPressedTilePass);
        tileLine.OnMissClick(() =>
        {
            shouldScroll = false;
        });
        if (!lastSpawnedTileLine.IsNull())
        {
            tileLine.SubscribeToLastTileLine(lastSpawnedTileLine);
        }

        lastSpawnedTileLine = tileLine;
        lastSpawnedIndex = index;
        currentRectHeight += height;
    }

    private void HandlerUnPressedTilePass(float unPressedTileHeight)
    {
        Tween
            .UIAnchoredPositionY(
                rectTransform,
                rectTransform.anchoredPosition.y + unPressedTileHeight,
                0.2f,
                Ease.Linear
            )
            .OnComplete(() =>
            {
                GameManager.Instance.SetGameOver(true);
            });
        shouldScroll = false;
    }

    private void RestartSong()
    {
        lastSpawnedIndex = 0;
        lastSpawnedTileLine = null;
        rectTransform.anchoredPosition = restartPosition;
        currentRectHeight = 0;

        for (int i = 0; i < 10; i++)
        {
            ConfigureTile(i);
        }

        GameManager.Instance.GameCycle += 1;
        GameManager.Instance.ResetSong();
    }
}
