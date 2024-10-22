using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerExample : MonoBehaviour
{
    [SerializeField]
    private float startTime;
    private CountdownTimer countdownTimer;

    void Start()
    {
        countdownTimer = new CountdownTimer(startTime);
        countdownTimer.Start();
        countdownTimer.OnTimeStop += OnTimeStop;
    }

    void Update()
    {
        // if (!countdownTimer.IsFinished)
        // {
        //     Debug.Log(countdownTimer.CurrentTime);
        // }
    }

    void OnTimeStop()
    {
        Debug.Log(countdownTimer.CurrentTime);
    }
}
