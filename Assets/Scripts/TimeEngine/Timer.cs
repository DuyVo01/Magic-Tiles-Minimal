using System;
using UnityEngine;

namespace ImprovedTimer
{
    public abstract class Timer:IDisposable
    {
        private bool disposed;
        public float CurrentTime{get; protected set;}
        public bool IsRunning {get; private set;}

        protected float initialTime;

        public float Progress => Mathf.Clamp(CurrentTime / initialTime, 0, 1);

        public Action OnTimeStart = delegate {};
        public Action OnTimeStop = delegate {};

        protected Timer(float value)
        {
            initialTime = value;
        }

        public void Start()
        {
            CurrentTime = initialTime;
            if(!IsRunning)
            {
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                OnTimeStart.Invoke();
            }
        }
        
        public void Stop()
        {
            if(IsRunning)
            {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnTimeStop.Invoke();
            }
        }
        public abstract void Tick();

        public abstract bool IsFinished {get; }
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public virtual void Reset() => CurrentTime = initialTime;
        public virtual void Reset(float value)
        {
            initialTime = value;
            Reset();
        }

        ~Timer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed)
            {
                return;
            }

            if(disposing)
            {
                TimerManager.DeregisterTimer(this);
            }

            disposed = true;
        }
    }
}
