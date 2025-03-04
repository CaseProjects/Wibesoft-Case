namespace Utilities.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public static class TaskExtensions
    {
        #region DELAY

        public static async UniTask<TResult> AddReturnDelay<TResult>(Func<TResult> getter, float seconds,
            [Optional] CancellationToken? cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds),
                cancellationToken: cancellationToken ?? CancellationToken.None);
            return getter();
        }


        public static void AddMethodDelay(Action getter, float seconds, [Optional] CancellationToken? cancellationToken)
        {
            AsyncAddMethodDelay(getter, seconds, cancellationToken).Forget();
        }

        private static async UniTask AsyncAddMethodDelay(Action getter, float seconds,
            [Optional] CancellationToken? cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds),
                cancellationToken: cancellationToken ?? CancellationToken.None);
            getter.Invoke();
        }

        #endregion
    }
}