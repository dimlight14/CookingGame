using System;

namespace DefaultNamespace
{
    public interface IUICustomerProvider
    {
        public event Action<int, int> OnCustomersChanged;
    }
}