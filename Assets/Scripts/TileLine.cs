using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

public class TileLine : MonoBehaviour
{
    [SerializeField]
    private Transform upperLimitPoint;

    [SerializeField]
    private GameObject baseTilePrefab;

    [SerializeField]
    private GameObject baseLongTilePrefab;

    [SerializeField]
    private GameObject missedTilePrefab;

    private GameObjectPool baseTilePrefabPool;
    private GameObjectPool baseLongTilePrefabPool;
    private GameObjectPool missedTilePrefabPool;
    private RectTransform scoringLine;
    Vector2 bottomPoint;
    RectTransform rectTransform;
    public Action<TileLine> onPassingTheBottomLine;
    public Action<float> onPassingThelineUnPressed;
    public Action onMissClick;
    private bool hasPassTheBottomLine = false;
    private List<TileBehavior> tileTrackers = new List<TileBehavior>();
    private List<MissedTileBehavior> missedTileTrackers = new List<MissedTileBehavior>();
    public ReactiveProperty<int> NumberOfPressedTiles { get; private set; }
    private float[] tilePositionX = new float[4];

    DisposableBag eventSubscription;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        baseTilePrefabPool = new GameObjectPool(baseTilePrefab, 3, transform);
        baseLongTilePrefabPool = new GameObjectPool(baseLongTilePrefab, 3, transform);
        missedTilePrefabPool = new GameObjectPool(missedTilePrefab, 3, transform);

        NumberOfPressedTiles = new ReactiveProperty<int>();
    }

    void OnEnable()
    {
        hasPassTheBottomLine = false;
    }

    void Update()
    {
        if (!hasPassTheBottomLine)
        {
            HasPassTheBottomLine();
        }
    }

    void OnDestroy()
    {
        baseTilePrefabPool.ClearPool();
        onPassingTheBottomLine = null;
    }

    public void GenerateTiles(float[] positions, TileType tileType, float timeLapse)
    {
        TileBehavior tile;

        for (int i = 0; i < positions.Length; i++)
        {
            if (tileType == TileType.shortTile)
            {
                tile = baseTilePrefabPool.Get().GetComponent<TileBehavior>();
            }
            else
            {
                tile = baseLongTilePrefabPool.Get().GetComponent<TileBehavior>();
                tile.SetHeight(rectTransform.rect.height);
            }
            tile.AdditionalReset();
            tile.SetTilePosition(new Vector2(positions[i], 0));
            tile.SetTilePivot(new Vector2(0, 0));
            tile.SetTileTimelapse(timeLapse);
            tile.SetScoringLine(scoringLine);
            tile.AdditionalSetup();
            tile.OnTilePressed(() =>
            {
                NumberOfPressedTiles.Value--;
            });
            NumberOfPressedTiles.Subscribe(OnTilesPressedHandler).AddTo(ref eventSubscription);
            tileTrackers.Add(tile);
        }

        List<float> unusedPos = new List<float>(tilePositionX);
        MissedTileBehavior missedTile;

        foreach (var item in unusedPos)
        {
            if (positions.Contains(item))
            {
                continue;
            }

            missedTile = missedTilePrefabPool.Get().GetComponent<MissedTileBehavior>();
            missedTile.SetPos(new Vector2(item, 0));
            missedTile.SetSize(
                new Vector2(rectTransform.rect.width / 4, rectTransform.rect.height)
            );
            missedTile.OnMissedTilePressed(() =>
            {
                onMissClick?.Invoke();
                AudioManager.Instance.StopSong();
            });
            missedTile.ResetStats();
            missedTileTrackers.Add(missedTile);
        }
        NumberOfPressedTiles.Value = tileTrackers.Count;
    }

    public void SetPositionY(float newPosY)
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newPosY);
        rectTransform.pivot = new Vector2(0, 0);
    }

    public void SetSize(Vector2 newSize)
    {
        rectTransform.SetNewRectHeight(newSize.y);
        rectTransform.SetNewRectWidth(newSize.x);

        float tileSizeX = rectTransform.rect.width / 4;

        for (int i = 0; i < tilePositionX.Length; i++)
        {
            tilePositionX[i] = tileSizeX * i;
        }
    }

    public void SetScoringLine(RectTransform scoringLine)
    {
        this.scoringLine = scoringLine;
    }

    public void HasPassTheBottomLine()
    {
        if (upperLimitPoint.position.y < bottomPoint.y)
        {
            GameManager.Instance.NumberOfTileLineLeft--;

            hasPassTheBottomLine = true;

            if (NumberOfPressedTiles.CurrentValue == tileTrackers.Count)
            {
                onPassingThelineUnPressed?.Invoke(rectTransform.rect.height);
                AudioManager.Instance.StopSong();

                foreach (var tile in tileTrackers)
                {
                    tile.TileDisable();
                }
            }
            else
            {
                onPassingTheBottomLine?.Invoke(this);

                foreach (var tile in tileTrackers)
                {
                    if (tile is LongTileBehavior)
                    {
                        baseLongTilePrefabPool.Return(tile.gameObject);
                    }
                    else if (tile is ShortTileBehavior)
                    {
                        baseTilePrefabPool.Return(tile.gameObject);
                    }
                }
            }

            foreach (var item in missedTileTrackers)
            {
                missedTilePrefabPool.Return(item.gameObject);
            }
            //eventSubscription.Dispose();
        }
    }

    public void OnPassingTheBottomLine(Action<TileLine> callback)
    {
        onPassingTheBottomLine = callback;
    }

    public void OnPassingTheLineUnPressed(Action<float> callback)
    {
        onPassingThelineUnPressed = callback;
    }

    public void OnMissClick(Action callback)
    {
        onMissClick = callback;
    }

    public void SetBottomPoint(Vector2 bottomPoint)
    {
        this.bottomPoint = bottomPoint;
    }

    public Vector2 GetAnchoredPos()
    {
        return rectTransform.anchoredPosition;
    }

    public void ReSetState()
    {
        hasPassTheBottomLine = false;
        tileTrackers.Clear();
        missedTileTrackers.Clear();
    }

    public void SubscribeToLastTileLine(TileLine tileLine)
    {
        tileLine
            .NumberOfPressedTiles.Subscribe(n =>
            {
                if (n == 0)
                    EnableTiles();
            })
            .AddTo(ref eventSubscription);
    }

    public void EnableTiles()
    {
        foreach (var item in tileTrackers)
        {
            item.EnablePress();
        }
    }

    public void SetFirstTile()
    {
        foreach (var item in tileTrackers)
        {
            item.SetFirstTile();
        }
    }

    private void OnTilesPressedHandler(int numberOfUnPressedTileLeft)
    {
        if (numberOfUnPressedTileLeft == 0)
        {
            foreach (var item in missedTileTrackers)
            {
                item.DisablePress();
            }
        }
    }

    
}
