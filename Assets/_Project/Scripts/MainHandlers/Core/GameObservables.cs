using R3;
using UnityEngine;
using Zenject;

namespace MainHandlers
{
    public class GameObservables
    {
        #region INJECT

        private readonly TickableManager _tickableManager;
        private readonly GameStateManager _gameStateManager;

        private GameObservables(TickableManager tickableManager, GameStateManager gameStateManager)
        {
            _tickableManager = tickableManager;
            _gameStateManager = gameStateManager;
        }

        #endregion


        #region INPUT

        public Observable<Touch> InputObservable
            => _tickableManager.TickStream.Where(x => Input.touchCount > 0).Select(x => Input.GetTouch(0));

        #endregion


        public Observable<GameStateManager.GameStates> GameStateObservable =>
            _gameStateManager.GameStateReactiveProperty;

        public Observable<GameStateManager.GameStates> GameStateUpdateObservable => _tickableManager.TickStream
            .Select(x => _gameStateManager.GameStateReactiveProperty.Value);
    }
}