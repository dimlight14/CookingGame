using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class GameTest : IUITimerProvider
    {
        public event Action<bool> OnGameEnded;
        public event Action OnGameStarted;
        public event Action<float> OnTimeChanged;

        private readonly OrdersManager _ordersManager;
        private readonly OrderInitializer _orderInitializer;
        private readonly CustomerSpawner _customerSpawner;
        private readonly Updater _updater;
        private readonly LevelSettings _levelSettings;

        private float _timeRemaining;

        public GameTest(
            OrdersManager ordersManager,
            OrderInitializer orderInitializer,
            CustomerSpawner customerSpawner,
            Updater updater,
            LevelSettings levelSettings
        )
        {
            _ordersManager = ordersManager;
            _orderInitializer = orderInitializer;
            _customerSpawner = customerSpawner;
            _updater = updater;
            _levelSettings = levelSettings;
        }

        public void RestartGame()
        {
            StartGame();
        }

        public void StartGame()
        {
            _ordersManager.Clear();
            _customerSpawner.Clear();
            _updater.OnUpdate += Update;
            _updater.Resume();
            OnGameStarted?.Invoke();

            _timeRemaining = _levelSettings.TimeTotal;
            _orderInitializer.SetNewLevel(_levelSettings);
            _customerSpawner.SetNewLevel(_levelSettings.OrdersTotal);
            _customerSpawner.OnCustomersChanged += (_, x) => CheckWin(x);
        }

        private void CheckWin(int customersRemaining)
        {
            if (customersRemaining == 0)
            {
                WinGame();
            }
        }

        private void Update(float deltaTime)
        {
            _timeRemaining -= deltaTime;
            OnTimeChanged?.Invoke(_timeRemaining);

            if (_timeRemaining <= 0)
            {
                LoseGame();
            }
        }

        private void LoseGame()
        {
            _updater.Pause();
            _updater.OnUpdate -= Update;
            OnGameEnded?.Invoke(false);
        }

        private void WinGame()
        {
            _updater.Pause();
            _updater.OnUpdate -= Update;
            OnGameEnded?.Invoke(true);
        }
    }
}