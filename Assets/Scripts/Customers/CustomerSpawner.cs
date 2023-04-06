using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CustomerSpawner : IUICustomerProvider
    {
        public event Action<int, int> OnCustomersChanged;

        private readonly int _maxCustomersPerScreen;
        private readonly CustomerFactory _customerFactory;
        private readonly Updater _updater;
        private readonly float _customerSpawnDelay;
        private readonly CustomersMarkUp _customersMarkUp;
        private readonly OrdersManager _ordersManager;

        private readonly List<int> _vacantSpots;
        private readonly Dictionary<int, Customer> _activeCustomers;
        private readonly List<Customer> _exitingCustomers;

        private float _spawnDelayRemaining;
        private int _customersRemaining;

        public CustomerSpawner(
            CustomerFactory customerFactory,
            Updater updater,
            float customerSpawnDelay,
            CustomersMarkUp customersMarkUp,
            OrdersManager ordersManager,
            int maxCustomersPerScreen
        )
        {
            _customerFactory = customerFactory;
            _updater = updater;
            _customerSpawnDelay = customerSpawnDelay;
            _customersMarkUp = customersMarkUp;
            _ordersManager = ordersManager;
            _maxCustomersPerScreen = maxCustomersPerScreen;

            _activeCustomers = new Dictionary<int, Customer>(_maxCustomersPerScreen);
            _vacantSpots = new List<int>(_maxCustomersPerScreen);
            _exitingCustomers = new List<Customer>(_maxCustomersPerScreen);

            _ordersManager.OnOrderFulfilled += OnOrderFulfilledHandler;
        }

        public void SetNewLevel(int customersPerLevel)
        {
            _customersRemaining = customersPerLevel;
            _updater.OnUpdate += Update;
            _spawnDelayRemaining = 0;

            _activeCustomers.Clear();
            _vacantSpots.Clear();
            _exitingCustomers.Clear();
            for (var i = 0; i < _maxCustomersPerScreen; i++)
            {
                _vacantSpots.Add(i);
            }

            OnCustomersChanged?.Invoke(_activeCustomers.Count, _customersRemaining + _activeCustomers.Count);
        }

        public void Clear()
        {
            foreach (var customer in _exitingCustomers)
            {
                _customerFactory.Despawn(customer);
            }

            foreach (var customerPair in _activeCustomers)
            {
                _customerFactory.Despawn(customerPair.Value);
            }
        }

        private void OnOrderFulfilledHandler(int screenPosition)
        {
            var middleSpot = (float)(_maxCustomersPerScreen - 1) / 2;
            var goal = screenPosition >= middleSpot ? _customersMarkUp.RightPosition : _customersMarkUp.LeftPosition;
            _activeCustomers[screenPosition].SetGoal(goal.x, OnCustomerReachedExit);

            _exitingCustomers.Add(_activeCustomers[screenPosition]);
            _activeCustomers.Remove(screenPosition);
            _vacantSpots.Add(screenPosition);

            OnCustomersChanged?.Invoke(_activeCustomers.Count, _customersRemaining + _activeCustomers.Count);
        }

        private void Update(float timeDelta)
        {
            if (_customersRemaining <= 0)
            {
                _updater.OnUpdate -= Update;
                return;
            }

            if (_spawnDelayRemaining > 0)
            {
                _spawnDelayRemaining -= timeDelta;
            }

            if (_spawnDelayRemaining > 0 || _vacantSpots.Count == 0)
            {
                return;
            }

            SpawnCustomer();
            _spawnDelayRemaining = _customerSpawnDelay;
        }

        private void SpawnCustomer()
        {
            var spot = _vacantSpots[Random.Range(0, _vacantSpots.Count)];

            var middleSpot = (float)(_maxCustomersPerScreen - 1) / 2;
            var fromRight = spot < middleSpot ? false : true;
            var startingPosiion = fromRight ? _customersMarkUp.RightPosition : _customersMarkUp.LeftPosition;
            var newCustomer = _customerFactory.GetCustomer();
            newCustomer.Initialize(startingPosiion, spot);
            newCustomer.SetGoal(_customersMarkUp.GetPosition(spot), OnCustomerReachedPosition);

            _activeCustomers.Add(spot, newCustomer);
            _vacantSpots.Remove(spot);
            _customersRemaining--;
            OnCustomersChanged?.Invoke(_activeCustomers.Count, _customersRemaining + _activeCustomers.Count);
        }

        private void OnCustomerReachedPosition(Customer customer)
        {
            _ordersManager.SpawnOrderAt(customer.ScreenPosition);
        }

        private void OnCustomerReachedExit(Customer customer)
        {
            _customerFactory.Despawn(customer);
            _exitingCustomers.Remove(customer);
        }
    }
}