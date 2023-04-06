using System.Collections.Generic;

namespace DefaultNamespace
{
    public class OrdersFactory
    {
        private const int MAX_FOOD = 3;
        private readonly List<OrderView> _orderViews;

        public OrdersFactory(List<OrderView> orderViews)
        {
            _orderViews = orderViews;
        }

        public Order CreateOrder(int screenPosition)
        {
            var order = new Order(_orderViews[screenPosition], MAX_FOOD);

            return order;
        }
    }
}