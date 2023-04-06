using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrderView : MonoBehaviour
    {
        [SerializeField] private List<OrderItemView> orderVisuals;

        public void InitializeAndShow(List<FoodType> orderItems)
        {
            var maxItems = orderVisuals.Count;
            
            if (orderItems.Count > maxItems)
            {
                Debug.LogError("OrderView: there are more items in the order than visual cells");
            }

            var lastItemIndex = orderItems.Count > maxItems ? maxItems - 1 : orderItems.Count - 1;
            for (var i = 0; i < orderVisuals.Count; i++)
            {
                if (i > lastItemIndex)
                {
                    orderVisuals[i].Hide();
                }
                else
                {
                    orderVisuals[i].InitializeAndShow(orderItems[i]);
                }
            }

            gameObject.SetActive(true);
        }

        public void FulfillFoodItem(FoodType foodType)
        {
            foreach (var visual in orderVisuals)
            {
                if (!visual.IsFulfilled && visual.FoodType == foodType)
                {
                    visual.SetFulfilled(true);
                    return;
                }
            }

            Debug.LogError($"Order view: wasn't able to find a visual of type {foodType} when fulfilling an order!");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}