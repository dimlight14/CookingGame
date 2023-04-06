using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIFoodButtonsPresenter
    {
        private readonly Button _saladButton;
        private readonly Button _porrigeButton;
        private readonly Button _meatButton;
        private readonly Button _fishButton;
        private readonly OrdersManager _ordersManager;
        
        public UIFoodButtonsPresenter(OrdersManager ordersManager, Button fishButton, Button meatButton, Button porrigeButton, Button saladButton)
        {
            _ordersManager = ordersManager;
            _fishButton = fishButton;
            _meatButton = meatButton;
            _porrigeButton = porrigeButton;
            _saladButton = saladButton;
            
            MapEvents();
        }

        private void MapEvents()
        {
            _saladButton.onClick.AddListener(() => _ordersManager.TryFulfillFoodItem(FoodType.Salad));
            _porrigeButton.onClick.AddListener(() => _ordersManager.TryFulfillFoodItem(FoodType.Porridge));
            _meatButton.onClick.AddListener(() => _ordersManager.TryFulfillFoodItem(FoodType.Meat));
            _fishButton.onClick.AddListener(() => _ordersManager.TryFulfillFoodItem(FoodType.Fish));
        }
    }
}