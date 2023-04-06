using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class OrdersManager
    {
        public event Action<int> OnOrderFulfilled;

        private readonly List<Order> _activeOrders;
        private readonly OrderInitializer _orderInitializer;

        public OrdersManager(OrderInitializer orderInitializer, int maxOrdersOnScreen)
        {
            _orderInitializer = orderInitializer;
            _activeOrders = new List<Order>(maxOrdersOnScreen);
        }

        public void SpawnOrderAt(int screenPosition)
        {
            var newOrder = _orderInitializer.InitializeNewOrder(screenPosition);
            _activeOrders.Add(newOrder);
        }

        public void TryFulfillFoodItem(FoodType foodType)
        {
            var orderAffectedIndex = -1;
            for (var i = 0; i < _activeOrders.Count; i++)
            {
                if (_activeOrders[i].TryFulfillFoodItem(foodType))
                {
                    orderAffectedIndex = i;
                    break;
                }
            }

            if (orderAffectedIndex != -1 && _activeOrders[orderAffectedIndex].IsFulfield)
            {
                CompleteOrder(_activeOrders[orderAffectedIndex]);
            }
        }

        public bool TryUseBooster()
        {
            if (_activeOrders.Count == 0)
            {
                return false;
            }

            CompleteOrder(_activeOrders[0]);
            return true;
        }

        public void Clear()
        {
            _activeOrders.Clear();
            _orderInitializer.Clear();
        }

        private void CompleteOrder(Order orderToComplete)
        {
            OnOrderFulfilled?.Invoke(orderToComplete.PositionOnScreen);
            orderToComplete.ClearAndHide();
            _activeOrders.Remove(orderToComplete);
        }
    }
}