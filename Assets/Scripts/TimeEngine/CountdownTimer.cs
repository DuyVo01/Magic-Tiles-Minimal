using System.Collections;
using System.Collections.Generic;
using ImprovedTimer;
using UnityEngine;

public class CountdownTimer : Timer
{
    public CountdownTimer(float value) : base(value)
    {
    }

    public override bool IsFinished => CurrentTime <= 0;

    public override void Tick()
    {
        if(IsRunning && CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime   ;
        }

        if(IsRunning && CurrentTime <= 0)
        {
            Stop();
        }
    }
}
