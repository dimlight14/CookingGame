using UnityEngine;

namespace DefaultNamespace
{
    public class CustomerFactory
    {
        private readonly GameObject _customerImageReference;
        private readonly Updater _updater;
        private readonly Transform _parentTransform;
        private readonly ItemPool<Customer> _customerPool;
        private readonly float _speed;

        public CustomerFactory(GameObject customerImageReference, Updater updater, Transform parentTransform, ItemPool<Customer> customerPool, float speed)
        {
            _customerImageReference = customerImageReference;
            _updater = updater;
            _parentTransform = parentTransform;
            _customerPool = customerPool;
            _speed = speed;
        }

        public Customer GetCustomer()
        {
            if (_customerPool.HasItems)
            {
                return _customerPool.GetItem();
            }

            var newImage = Object.Instantiate(_customerImageReference, _parentTransform);
            return new Customer(newImage, _updater, _speed);
        }

        public void Despawn(Customer customer)
        {
            customer.Hide();
            _customerPool.PushItem(customer);   
        }
    }
}