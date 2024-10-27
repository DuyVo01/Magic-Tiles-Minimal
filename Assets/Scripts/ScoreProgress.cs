using System;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreProgress : MonoBehaviour
{
    [SerializeField]
    private SongObject song;

    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private OnReachCheckPoint onReachCheckPoint;

    private float totalProgress = 0f;
    private float oneNodePercent = 0f;
    private float currentPercentage = 0;

    DisposableBag subscriptions;

    private int checkPointCount = 0;

    [SerializeField]
    private float[] progressValues = new float[6];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < song.totalNumberOfNode; i++)
        {
            totalProgress = totalProgress + 1 * song.tiles[i].tileCount;
        }

        for (int i = 0; i < progressValues.Length; i++)
        {
            progressValues[i] = 1f / 6f * (i + 1);
        }

        totalProgress *= 2;
        oneNodePercent = 1 / totalProgress;
        currentPercentage = 0;
        checkPointCount = 0;

        GameManager
            .Instance.HasHitNormalScore.Subscribe(t =>
            {
                if (t == true)
                {
                    HandleProgress();
                }
            })
            .AddTo(ref subscriptions);

        GameManager
            .Instance.HasHitPerfectScore.Subscribe(t =>
            {
                if (t == true)
                {
                    HandleProgress();
                }
            })
            .AddTo(ref subscriptions);

        progressSlider.onValueChanged.AddListener(TriggerEffect);
    }

    void OnDestroy()
    {
        subscriptions.Dispose();
        progressSlider.onValueChanged.RemoveListener(TriggerEffect);
    }

    private void HandleProgress()
    {
        if (GameManager.Instance.GameCycle == 2)
        {
            currentPercentage += oneNodePercent * 0.75f;
        }
        else if (GameManager.Instance.GameCycle == 3)
        {
            currentPercentage += oneNodePercent * 0.25f;
        }
        else
        {
            currentPercentage += oneNodePercent;
        }
        progressSlider.value = currentPercentage;
    }

    private void TriggerEffect(float progressValue)
    {
        if (progressValue >= progressValues[checkPointCount])
        {
            Debug.Log($"check point: {checkPointCount}");

            onReachCheckPoint?.Invoke(checkPointCount);
            checkPointCount++;
        }
    }

    [Serializable]
    public class OnReachCheckPoint : UnityEvent<int> { }
}
