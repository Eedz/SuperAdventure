using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public int Price { get; set; }
        public int Capacity { get; set; }
        public List<Item> Contents { get; set; }
        public int HappinessLevel { get; set; }

        public Item(int id, string name, string namePlural, int price)
        {
            ID = id;
            Name = name;
            NamePlural = namePlural;
            Price = price;
            Capacity = 0;
            Contents = new List<Item>();
            HappinessLevel = 0;
        }
    }
}
