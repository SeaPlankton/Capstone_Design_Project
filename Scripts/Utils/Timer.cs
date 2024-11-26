using System;

namespace Miku.Utils
{
    public abstract class Timer
    {
        protected float initialTime;
        protected float Time { get; set; }
        public bool IsRunning { get; protected set; }

        public float Progress => Time / initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            Time = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;   

        public abstract void Tick(float deltaTime);
    }


    /// <summary>
    /// 카운트다운 클래스, 초기 시간에서 0으로 음수 방향
    /// </summary>
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }
            // 시간이 다 되었으므로 작동 중지
            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }

        // 시간이 다 되었는가?
        public bool IsFinished => Time <= 0;
        // 처음 시간으로 초기화
        public void Reset()
        {
            Time = initialTime;
        }
        // 특정 시간을 처음 시간으로 잡고 초기화
        public void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }

        /// <summary>
        /// 스톱워치 클래스, 초기 시간이 0에서 양수 방향
        /// </summary>
        public class StopwatchTimer : Timer
        {
            // 
            public StopwatchTimer() : base(0) { }

            public override void Tick(float deltaTime)
            {
                if (IsRunning)
                {
                    Time += deltaTime;
                }
            }

            public void Reset() => Time = 0;

            public float GetTime() => Time;
        }
    }
}
