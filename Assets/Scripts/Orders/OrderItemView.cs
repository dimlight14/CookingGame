using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class OrderItemView : MonoBehaviour
    {
        [SerializeField] private Image foodTypeImage;
        [SerializeField] private GameObject checkMark;
        
        public bool IsFulfilled { get;private set; }
        public FoodType FoodType { get; private set; }

        public void InitializeAndShow(FoodType foodType)
        {
            gameObject.SetActive(true);
            SetFoodType(foodType);
            SetFulfilled(false);
        }

        public void SetFulfilled(bool isFulfilled)
        {
            IsFulfilled = isFulfilled;
            checkMark.SetActive(isFulfilled);
        }

        private void SetFoodType(FoodType foodType)
        {
            FoodType = foodType;
            foodTypeImage.color = FoodColorsUtility.GetFoodColor(foodType);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}