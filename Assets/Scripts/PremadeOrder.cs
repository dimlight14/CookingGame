using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    [Serializable]
    public class PremadeOrder
    {
        public int OrderNumber;
        public List<FoodType> FoodInOrder;
    }
}