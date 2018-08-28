using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class FoodItem : Item
    {
        public Item Details { get; set; }
        public int FoodValue { get; set; }
        public int HappinessValue { get; set; }

        public FoodItem(int id, string name, string namePlural, int price, int foodValue, int happinessValue) : base(id, name, namePlural, price)
        {
            FoodValue = foodValue;
            HappinessValue = happinessValue;
        }
    }
}
