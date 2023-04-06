using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Customer
    {
        private readonly GameObject _customerView;
        private readonly Updater _updater;
        private readonly float _speed;

        private float _goalPosition;
        private Action<Customer> _onGoalReached;
        private bool _directionToRight;

        public int ScreenPosition { get; private set; }

        public Customer(GameObject customerView, Updater updater, float speed)
        {
            _customerView = customerView;
            _updater = updater;
            _speed = speed;
        }

        public void Initialize(Vector3 position, int spot)
        {
            ScreenPosition = spot;
            _customerView.transform.position = position;
            _customerView.gameObject.SetActive(true);
        }

        public void SetGoal(float goalPosition, Action<Customer> onGoalReached)
        {
            _goalPosition = goalPosition;
            _onGoalReached = onGoalReached;
            _directionToRight = _customerView.transform.position.x < goalPosition;
            _updater.OnUpdate += Update;
        }

        public void Hide()
        {
            _updater.OnUpdate -= Update;
            _customerView.gameObject.SetActive(false);
        }

        private void ReachGoal()
        {
            _updater.OnUpdate -= Update;
            _onGoalReached(this);
        }

        private void Update(float timeDelta)
        {
            var viewPosition = _customerView.transform.position;
            viewPosition.x = _directionToRight ? viewPosition.x + _speed * timeDelta : viewPosition.x - _speed * timeDelta;
            _customerView.transform.position = viewPosition;

            if (_directionToRight)
            {
                if (viewPosition.x >= _goalPosition)
                {
                    ReachGoal();
                }
            }
            else if (viewPosition.x <= _goalPosition)
            {
                ReachGoal();
            }
        }
    }
}