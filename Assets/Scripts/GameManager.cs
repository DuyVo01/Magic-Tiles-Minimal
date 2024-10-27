using System.Security.Cryptography;
using R3;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ReactiveProperty<bool> HasHitPerfectScore { get; private set; }
    public ReactiveProperty<bool> HasHitNormalScore { get; private set; }
    public ReactiveProperty<int> GameScore {get; private set;}
    public ReactiveProperty<bool> IsGameRunning{get; private set;}
    public ReactiveProperty<bool> IsGameOver{get; private set;}
    public int GameCycle { get => gameCycle; set => gameCycle = value; }
    public int NumberOfTileLineLeft { get => numberOfTileLineLeft; set => numberOfTileLineLeft = value; }

    private int gameCycle = 1;
    private int numberOfTileLineLeft = 0;
    private SongObject currentSong;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        HasHitPerfectScore = new ReactiveProperty<bool>(false);
        HasHitNormalScore = new ReactiveProperty<bool>(false);
        GameScore = new ReactiveProperty<int>(0);
        IsGameRunning = new ReactiveProperty<bool>(false);
        IsGameOver = new ReactiveProperty<bool>(false);
    }

    public void SetHitPerfectScore(bool isHit)
    {
        HasHitPerfectScore.Value = isHit;
    }

    public void SetHitNormalScore(bool isHit)
    {
        HasHitNormalScore.Value = isHit;
    }

    public void SetSong(SongObject songObject)
    {
        currentSong = songObject;
        NumberOfTileLineLeft = currentSong.totalNumberOfNode;
    }

    public void ResetSong()
    {
        NumberOfTileLineLeft = currentSong.totalNumberOfNode;
    }

    public void AddGameScore(int scoreToAdd)
    {
        GameScore.Value += scoreToAdd;
    }

    public void SetGameRun(bool isStart)
    {
        IsGameRunning.Value = isStart;
    }

    public void SetGameOver(bool isOver)
    {
        IsGameOver.Value = isOver;
    }
}
