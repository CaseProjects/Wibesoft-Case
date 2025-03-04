using System;
using Events;
using R3;
using UnityEngine;
using Utilities;
using Zenject;

namespace MainHandlers
{
    public class GameStateManager : IInitializable, IDisposable
    {
        public enum GameStates
        {
            IdleState,
            InGameState,
            FailState,
            FinishState,
            None
        }

        #region INJECT

        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();


        private GameStateManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        #endregion

        public readonly ReactiveProperty<GameStates> GameStateReactiveProperty = new(GameStates.None);

        public void Initialize()
        {
            Application.targetFrameRate = 60;

            CheckStateChanges();
            ChangeStateToIdle();
            ChangeToInGameWhenPress();

            void CheckStateChanges()
            {
                GameStateReactiveProperty.Subscribe(CheckState).AddTo(_compositeDisposable);
            }

            void ChangeStateToIdle()
            {
                Observable.Timer(TimeSpan.FromSeconds(0.01))
                    .Subscribe(x => ChangeState(GameStates.IdleState))
                    .AddTo(_compositeDisposable);
            }

            void ChangeToInGameWhenPress()
            {
                Observable.EveryUpdate().Select(x => GameStateReactiveProperty.Value)
                    .Where(x => GameStateReactiveProperty.Value == GameStates.IdleState &&
                                Input.GetMouseButtonDown(0))
                    .Subscribe(x => ChangeState(GameStates.InGameState)).AddTo(_compositeDisposable);
            }
        }


        public void ChangeState(GameStates nextState) => GameStateReactiveProperty.Value = nextState;

        private void CheckState(GameStates state)
        {
            switch (state)
            {
                case GameStates.IdleState:
                    _signalBus.AbstractFire(new SignalChangeState(0, "IDLE"));
                    break;
                case GameStates.InGameState:
                    _signalBus.AbstractFire(new SignalChangeState(5, "RUN"));
                    break;
                case GameStates.FailState:
                    _signalBus.AbstractFire(new SignalChangeState(0, "FAIL"));
                    break;
                case GameStates.FinishState:
                    _signalBus.AbstractFire(new SignalChangeState(0, "FINISH"));
                    break;
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}