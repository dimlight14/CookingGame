using System;

namespace DefaultNamespace
{
    public class BoosterService
    {
        public event Action OnBuyBoosterShow;

        private int _boostersRemaining;
        private readonly OrdersManager _ordersManager;

        public BoosterService(int initialBoosters, OrdersManager ordersManager)
        {
            _ordersManager = ordersManager;
            _boostersRemaining = initialBoosters;
        }

        public void BuyBooster()
        {
            _boostersRemaining++;
        }

        public void OnBoosterClicked()
        {
            if (_boostersRemaining > 0)
            {
                if (_ordersManager.TryUseBooster())
                {
                    _boostersRemaining--;
                }
            }
            else
            {
                OnBuyBoosterShow?.Invoke();
            }
        }
    }
}