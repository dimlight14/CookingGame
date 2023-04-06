using System.Collections.Generic;

namespace DefaultNamespace
{
    public class Order
    {
        public int PositionOnScreen;
        
        private readonly List<FoodType> _foodRequired;
        private readonly OrderView _view;

        public bool IsFulfield => _foodRequired.Count == 0;

        public Order(OrderView view, int maxFood)
        {
            _view = view;
            _foodRequired = new List<FoodType>(maxFood);
        }

        public void AddFoodToOrder(FoodType foodType)
        {
            _foodRequired.Add(foodType);
        }

        public bool TryFulfillFoodItem(FoodType foodType)
        {
            var foodToRemove = -1;
            for (var i = 0; i < _foodRequired.Count; i++)
            {
                if (_foodRequired[i] == foodType)
                {
                    foodToRemove = i;
                    break;
                }
            }

            if (foodToRemove != -1)
            {
                _view.FulfillFoodItem(foodType);
                _foodRequired.RemoveAt(foodToRemove);
                return true;
            }

            return false;
        }

        public void Show()
        {
            _view.InitializeAndShow(_foodRequired);
        }
        public void ClearAndHide()
        {
            _foodRequired.Clear();
            _view.Hide();
        }
    }
}