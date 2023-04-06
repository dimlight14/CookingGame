using UnityEngine;

namespace DefaultNamespace
{
    public static class FoodColorsUtility
    {
        public static Color GetFoodColor(FoodType foodType)
        {
            return foodType switch
            {
                FoodType.Salad => new Color(0.04705883f, 0.7411765f, 0.145098f),
                FoodType.Porridge => new Color(0.6808527f, 0.745283f, 0.2207725f),
                FoodType.Meat => new Color(0.5283019f, 0.09668918f, 0.1141058f),
                FoodType.Fish => new Color(0.09681381f, 0.1882849f, 0.3490566f),
                _ => new Color(0.04705883f, 0.7411765f, 0.145098f)
            };
        }
    }
}