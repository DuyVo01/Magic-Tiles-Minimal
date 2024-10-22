using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ImprovedTimer
{
    public static class TimerManager
    {
        private static readonly List<Timer> timers = new List<Timer>();

        public static void RegisterTimer(Timer timer) => timers.Add(timer);

        public static void DeregisterTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateTimers()
        {
            foreach (var timer in new List<Timer>(timers))
            {
                timer.Tick();
            }
        }

        public static void Clear() => timers.Clear();
    }
}
