using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityUtils.LowLevel;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace ImprovedTimer
{
    internal static class TimerBootstrapper
    {
        private static PlayerLoopSystem timerSystem;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize()
        {
            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0))
            {
                Debug.LogWarning(
                    "Improved timers not initialized, unable to register TimeManager into update loop"
                );
                return;
            }
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
            PlayerLoopUtils.PrintPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged += OnPlayModeState;
#endif
        }

        private static void OnPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

                RemoveTimerManager<Update>(ref currentPlayerLoop);
                PlayerLoop.SetPlayerLoop(currentPlayerLoop);

                TimerManager.Clear();
            }
        }

        private static void RemoveTimerManager<T>(ref PlayerLoopSystem loop)
        {
            PlayerLoopUtils.RemoveSystem<T>(ref loop, in timerSystem);
        }

        private static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int index)
        {
            timerSystem = new PlayerLoopSystem()
            {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateTimers,
                subSystemList = null,
            };

            return PlayerLoopUtils.InsertSystem<T>(ref loop, timerSystem, index);
        }
    }
}
