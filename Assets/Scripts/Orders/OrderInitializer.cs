using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class OrderInitializer
    {
        private readonly int _maxOrdersOnScreen;
        private readonly Func<int, Order> _createOrderMethod;
        private readonly NumberOfFoodsCalculator _foodNumberCalculator;
        private readonly int _maxFoodTypes;

        private int _randomOrdersRemaining;
        private int _randomFoodRemainig;
        private int _maxFoodPerOrder;
        private int _currentOrderNumber;
        private LevelSettings _currentSettings;
        private List<Order> _ordersByScreenPositions;

        public OrderInitializer(Func<int, Order> createOrderMethod, NumberOfFoodsCalculator foodNumberCalculator, int maxOrdersOnScreen)
        {
            _createOrderMethod = createOrderMethod;
            _foodNumberCalculator = foodNumberCalculator;
            _maxOrdersOnScreen = maxOrdersOnScreen;
            
            _maxFoodTypes = Enum.GetNames(typeof(FoodType)).Length;
            InitializePoolOfOrders();
        }

        public void SetNewLevel(LevelSettings levelSettings)
        {
            _currentSettings = levelSettings;

            _randomOrdersRemaining = _currentSettings.OrdersTotal - levelSettings.PremadeOrders.Count;
            _randomFoodRemainig = _currentSettings.FoodTotal - GetTotalFoodInPremadeOrders();
            _maxFoodPerOrder = _currentSettings.MaxFoodPerOrder;

            _currentOrderNumber = 0;
        }

        public Order InitializeNewOrder(int positionOnScreen)
        {
            _currentOrderNumber++;
            var orderToInitialize = _ordersByScreenPositions[positionOnScreen];
            if (!orderToInitialize.IsFulfield)
            {
                orderToInitialize.ClearAndHide();
                Debug.LogError("Trying to initialize order that is still used");
            }

            if (TryFindPremadeOrder(_currentOrderNumber, out var premadeOrder))
            {
                SetAsPremade(orderToInitialize, premadeOrder);
            }
            else
            {
                SetOrderAsRandom(orderToInitialize);
            }

            orderToInitialize.PositionOnScreen = positionOnScreen;
            orderToInitialize.Show();

            return orderToInitialize;
        }

        public void Clear()
        {
            if (_ordersByScreenPositions == null)
            {
                return;
            }

            foreach (var order in _ordersByScreenPositions)
            {
                order.ClearAndHide();
            }
        }

        private void InitializePoolOfOrders()
        {
            _ordersByScreenPositions = new List<Order>(_maxOrdersOnScreen);
            for (var i = 0; i < _maxOrdersOnScreen; i++)
            {
                _ordersByScreenPositions.Add(_createOrderMethod.Invoke(i));
            }
        }
        private int GetTotalFoodInPremadeOrders()
        {
            var totalFood = 0;
            foreach (var premadeOrder in _currentSettings.PremadeOrders)
            {
                totalFood += premadeOrder.FoodInOrder.Count;
            }

            return totalFood;
        }

        private void SetAsPremade(Order orderToInitialize, PremadeOrder premadeOrder)
        {
            foreach (var food in premadeOrder.FoodInOrder)
            {
                orderToInitialize.AddFoodToOrder(food);
            }
        }

        private bool TryFindPremadeOrder(int currentOrder, out PremadeOrder premadeOrder)
        {
            foreach (var settingsOrder in _currentSettings.PremadeOrders)
            {
                if (currentOrder == settingsOrder.OrderNumber)
                {
                    premadeOrder = settingsOrder;
                    return true;
                }
            }

            premadeOrder = null;
            return false;
        }

        private void SetOrderAsRandom(Order orderToInitialize)
        {
            var numberOfFoodItems = _foodNumberCalculator.CalculateFoodNumber(_randomOrdersRemaining, _randomFoodRemainig, _maxFoodPerOrder);
            for (var i = 0; i < numberOfFoodItems; i++)
            {
                orderToInitialize.AddFoodToOrder(GetRandomFood());
            }

            _randomOrdersRemaining--;
            _randomFoodRemainig -= numberOfFoodItems;
        }

        private FoodType GetRandomFood()
        {
            var chance = Random.Range(1, _maxFoodTypes + 1);
            return (FoodType)chance;
        }
    }
}