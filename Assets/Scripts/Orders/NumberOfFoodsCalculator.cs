using UnityEngine;

namespace DefaultNamespace
{
    public class NumberOfFoodsCalculator
    {
        private readonly float _additionalIncreaseChance;
        private readonly float _additionalDecreaseChance;

        public NumberOfFoodsCalculator(float additionalIncreaseChance, float additionalDecreaseChance)
        {
            _additionalIncreaseChance = additionalIncreaseChance;
            _additionalDecreaseChance = additionalDecreaseChance;
        }

        public int CalculateFoodNumber(int ordersRemaining, int foodRemaining, int maxFoodPerOrder)
        {
            var average = (float)foodRemaining / ordersRemaining;
            if (average >= maxFoodPerOrder)
            {
                return Mathf.FloorToInt(average);
            }

            var baseValue = RoundByFractionPercentage(average);

            var canBeDecreased = CanBeDecreased(baseValue, ordersRemaining, foodRemaining, maxFoodPerOrder);
            var canBeIncreased = CanBeIncreased(baseValue, ordersRemaining, foodRemaining, maxFoodPerOrder);

            if (!canBeDecreased && !canBeIncreased)
            {
                return baseValue;
            }

            var chance = Random.Range(0f, 1f);
            
            if (canBeDecreased && chance <= _additionalDecreaseChance)
            {
                return baseValue - 1;
            }
            if (canBeIncreased && chance <= _additionalIncreaseChance + _additionalDecreaseChance && chance > _additionalDecreaseChance)
            {
                return baseValue + 1;
            }

            return baseValue;
        }

        private bool CanBeDecreased(int baseValue, int ordersRemaining, int foodRemaining, int maxFoodPerOrder)
        {
            if (baseValue == 1 || Mathf.Approximately(_additionalDecreaseChance, 0))
            {
                return false;
            }

            var maxCapacityAfterOrder = (ordersRemaining - 1) * maxFoodPerOrder;
            var foodRemainingAfterDecrease = foodRemaining - (baseValue - 1);
            return maxCapacityAfterOrder >= foodRemainingAfterDecrease;
        }

        private bool CanBeIncreased(int baseValue, int ordersRemaining, int foodRemaining, int maxFoodPerOrder)
        {
            if (baseValue >= maxFoodPerOrder || Mathf.Approximately(_additionalIncreaseChance, 0))
            {
                return false;
            }

            var foodSurplus = (foodRemaining - baseValue) - (ordersRemaining - 1);
            return foodSurplus > 0;
        }

        private int RoundByFractionPercentage(float inputNumber)
        {
            var wholeNumber = Mathf.FloorToInt(inputNumber);
            if (Mathf.Approximately(wholeNumber, inputNumber))
            {
                return wholeNumber;
            }

            var fraction = inputNumber - wholeNumber;
            var chance = Random.Range(0f, 1f);

            return chance <= fraction ? wholeNumber + 1 : wholeNumber;
        }
    }
}