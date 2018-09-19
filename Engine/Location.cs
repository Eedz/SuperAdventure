using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Engine
{

    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Item ItemRequiredToEnter { get; set; }
        public Quest QuestAvailableHere { get; set; }
        public List<Quest> QuestsAvailableHere { get; set; }
        public Monster MonsterLivingHere { get; set; }
        public Location LocationToNorth { get; set; }
        public Location LocationToEast { get; set; }
        public Location LocationToSouth { get; set; }
        public Location LocationToWest { get; set; }
        public Vendor VendorWorkingHere { get; set; }
        public BindingList<InventoryItem> ItemsAvailableForPickup { get; set; }
        public bool HasWater { get; set; }

        //public Point LocationInWorld { get; set; }


        public Location(int id, string name, string description, Item itemRequiredToEnter = null, Quest questAvailableHere = null, Monster monsterLivingHere = null)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemRequiredToEnter = itemRequiredToEnter;
            QuestAvailableHere = questAvailableHere;
            MonsterLivingHere = monsterLivingHere;
            QuestsAvailableHere = new List<Quest>();
            ItemsAvailableForPickup = new BindingList<InventoryItem>();
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = ItemsAvailableForPickup.SingleOrDefault(
            ii => ii.Details.ID == itemToRemove.ID);
            if (item == null)
            {
                // The item is not in the player's inventory, so ignore it.
                // We might want to raise an error for this situation
            }
            else
            {
                // They have the item in their inventory, so decrease the quantity
                item.Quantity -= quantity;
                // Don't allow negative quantities. We might want to raise an error for this situation
                if (item.Quantity < 0)
                {
                    item.Quantity = 0;
                }
                // If the quantity is zero, remove the item from the list
                if (item.Quantity == 0)
                {
                    ItemsAvailableForPickup.Remove(item);
                }
                // Notify the UI that the inventory has changed
                //RaiseInventoryChangedEvent(itemToRemove);
            }
        }
    }
}
