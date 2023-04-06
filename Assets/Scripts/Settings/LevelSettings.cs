using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class LevelSettings
    {
        [Range(1,3)]
        public int MaxFoodPerOrder = 3;
        public int OrdersTotal;
        public int FoodTotal;
        public int TimeTotal;
        public List<PremadeOrder> PremadeOrders = new List<PremadeOrder>();
    }
}