using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _Project.Scripts
{
    public class TimerObject : MonoBehaviour
    {
        public string TimerName { get; private set; }
        private Observable<Unit> _timerObservable { get; set; }
        public double TimeLeft { get; private set; }
        public TimeSpan Duration { get; private set; }
        public UniTask HalfTimeTask => _halfTimeTcs.Task.AsUniTask();
        public UniTask CompletionTask => _timeCompletionTcs.Task.AsUniTask();
        private TaskCompletionSource<bool> _halfTimeTcs;
        private TaskCompletionSource<bool> _timeCompletionTcs;


        public void Init(int duration, string timerName)
        {
            TimerName = timerName;
            TimeLeft = duration;
            Duration = TimeSpan.FromSeconds(duration);
            _halfTimeTcs = new TaskCompletionSource<bool>();
            _timeCompletionTcs = new TaskCompletionSource<bool>();

            _timerObservable = Observable.EveryUpdate().TakeUntil(Observable.Timer(TimeSpan.FromSeconds(duration)));
            _timerObservable.Subscribe(UpdateTimer, onCompleted: OnTimerComplete);
        }

        private void OnTimerComplete(Result obj)
        {
            TimeLeft = 0;
            if (!HalfTimeTask.Status.IsCompleted())
                _halfTimeTcs.TrySetCanceled();
            _timeCompletionTcs.TrySetResult(true);

            Destroy(this);
        }

        private void UpdateTimer(Unit _)
        {
            TimeLeft -= Time.deltaTime;

            if (TimeLeft <= Duration.TotalSeconds / 2 && !_halfTimeTcs.Task.IsCompleted)
            {
                _halfTimeTcs.TrySetResult(true);
            }
        }

        public string TimeLeftString()
        {
            var time = TimeSpan.FromSeconds(TimeLeft);
            return time.Seconds.ToString("00") + ":" + (time.Milliseconds / 10).ToString("00");
        }
    }
}