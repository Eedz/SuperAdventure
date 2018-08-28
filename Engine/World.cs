using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
namespace Engine
{
    public class World
    {
        //private const string WORLD_DATA_FILE_NAME = "WorldData.xml";
        private const string WORLD_DATA_FILE_NAME = "CampgroundData.xml";

        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Vendor> Vendors = new List<Vendor>();

        public const int UNSELLABLE_ITEM_PRICE = -1;

        public const int LOCATION_ID_HOME = 1;

        static World() {


            if (File.Exists(WORLD_DATA_FILE_NAME))
            {
                CreateWorldFromXmlString(File.ReadAllText(WORLD_DATA_FILE_NAME));
            }
            else
            {
                // raise error message?
            }

        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in Items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public static Quest QuestByRequiredLocationID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.RequiredLocation.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }

            return null;
        }

        public static Vendor VendorByID(int id)
        {
            foreach (Vendor vendor in Vendors)
            {
                if (vendor.ID == id)
                {
                    return vendor;
                }
            }

            return null;
        }

        public static bool PlayerMeetsRequirements(Quest q, Player p)
        {
            return true;
        }

        public static void CreateWorldFromXmlString(string xmlWorldData)
        {
            try
            {
                XmlDocument worldData = new XmlDocument();

                worldData.LoadXml(xmlWorldData);


                // Popuate Items
                foreach (XmlNode node in worldData.SelectNodes("/World/Items/Item"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    string namePlural = node.Attributes["Plural"].Value;
                    int price = Convert.ToInt32(node.SelectSingleNode("Price").InnerText);

                    try
                    {
                        int minimumDmg = Convert.ToInt32(node.SelectSingleNode("MinimumDamage").InnerText);
                        int maximumDmg = Convert.ToInt32(node.SelectSingleNode("MaximumDamage").InnerText);
                        Items.Add(new Weapon(id, name, namePlural, minimumDmg, maximumDmg, price));
                    }
                    catch
                    {
                        Items.Add(new Item(id, name, namePlural, price));
                    }

                }

                // Populate Monsters
                foreach (XmlNode node in worldData.SelectNodes("/World/Monsters/Monster"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    int maximumDmg = Convert.ToInt32(node.Attributes["MaximumDamage"].Value);
                    int exp = Convert.ToInt32(node.Attributes["RewardExperiencePoints"].Value);
                    int gold = Convert.ToInt32(node.Attributes["RewardGold"].Value);
                    int currentHP = Convert.ToInt32(node.Attributes["CurrentHP"].Value);
                    int maxHP = Convert.ToInt32(node.Attributes["MaximumHP"].Value);
                    Monsters.Add(new Monster(id, name, maximumDmg, exp, gold, currentHP, maxHP));

                    if (node.HasChildNodes)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            int lid = Convert.ToInt32(child.Attributes["ID"].Value);
                            int dropRate = Convert.ToInt32(child.Attributes["DropRate"].Value);
                            bool defaultItem = Convert.ToBoolean(child.Attributes["DeafultItem"].Value);
                            MonsterByID(id).LootTable.Add(new LootItem(ItemByID(lid), dropRate, defaultItem));
                        }
                    }


                }

                // Populate NPCs
                foreach (XmlNode node in worldData.SelectNodes("/World/NPCs/NPC"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    int currentHP = Convert.ToInt32(node.Attributes["CurrentHP"].Value);
                    int maxHP = Convert.ToInt32(node.Attributes["MaximumHP"].Value);
                    Vendors.Add(new Vendor(id, name, currentHP, maxHP));
                    try
                    {
                        foreach (XmlNode item in node["InventoryItems"])
                        {
                            int iid = Convert.ToInt32(item.Attributes["ID"].Value);
                            int quantity = Convert.ToInt32(item.Attributes["Quantity"].Value);

                            VendorByID(id).AddItemToInventory(ItemByID(iid), quantity);
                        }
                    }
                    catch { }

                    

                }

                // Populate Locations
                foreach (XmlNode node in worldData.SelectNodes("/World/Locations/Zone"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    string description = node.Attributes["Description"].Value;

                    Locations.Add(new Location(id, name, description));

                    try
                    {
                        foreach (XmlNode item in node["ItemsRequired"])
                        {
                            LocationByID(id).ItemRequiredToEnter = ItemByID(Convert.ToInt32(item.Attributes["ID"].Value));
                        }
                    }
                    catch { }
                }

                // Populate Quests
                foreach (XmlNode node in worldData.SelectNodes("/World/Quests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    string description = node.Attributes["Description"].Value;
                    int rewardExperiencePoints = Convert.ToInt32(node.SelectSingleNode("RewardExperience").InnerText);
                    int rewardGold = Convert.ToInt32(node.SelectSingleNode("RewardGold").InnerText);
                    Quests.Add(new Quest(id, name, description, rewardExperiencePoints, rewardGold));

                    // Quest Completion Items
                    foreach (XmlNode item in node["RequiredItems"])
                    {
                        int iid = Convert.ToInt32(item.Attributes["ID"].Value);
                        int quantity = Convert.ToInt32(item.Attributes["Quantity"].Value); ;
                        QuestByID(id).QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(iid), quantity));
                    }

                    // Quest Completion Locations
                    foreach (XmlNode item in node["RequiredLocation"].ChildNodes)
                    {
                        int iid = Convert.ToInt32(item.Attributes["ID"].Value);

                        QuestByID(id).RequiredLocation = LocationByID(iid);
                    }

                    // Quest Rewards
                    foreach (XmlNode item in node["RewardItems"].ChildNodes)
                    {
                        int iid = Convert.ToInt32(item.Attributes["ID"].Value);

                        QuestByID(id).RewardItem = ItemByID(iid);
                    }



                }

                // Populate Location connections, vendors, monsters, quests
                foreach (XmlNode node in worldData.SelectNodes("/World/Locations/Zone"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);

                    // Connections
                    LocationByID(id).LocationToNorth = LocationByID(Convert.ToInt32(node["LocationToNorth"].Attributes["ID"].Value));
                    LocationByID(id).LocationToEast = LocationByID(Convert.ToInt32(node["LocationToEast"].Attributes["ID"].Value));
                    LocationByID(id).LocationToSouth = LocationByID(Convert.ToInt32(node["LocationToSouth"].Attributes["ID"].Value));
                    LocationByID(id).LocationToWest = LocationByID(Convert.ToInt32(node["LocationToWest"].Attributes["ID"].Value));

                    try
                    {
                        foreach (XmlNode item in node["Quests"])
                        {
                            LocationByID(id).QuestsAvailableHere.Add(QuestByID(Convert.ToInt32(item.Attributes["ID"].Value)));
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        LocationByID(id).MonsterLivingHere = MonsterByID(Convert.ToInt32(node["Monster"].Attributes["ID"].Value));
                    }
                    catch
                    {

                    }

                    try
                    {
                        foreach (XmlNode item in node["Vendors"])
                        {
                            LocationByID(id).VendorWorkingHere = VendorByID(Convert.ToInt32(item.Attributes["ID"].Value));
                        }

                    }
                    catch
                    {

                    }
                }

                // Populate vendors quests
                foreach (XmlNode node in worldData.SelectNodes("/World/NPCs/NPC"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    try
                    {
                        foreach (XmlNode item in node["Quests"])
                        {
                            VendorByID(id).QuestsAvailable.Add(QuestByID(Convert.ToInt32(item.Attributes["ID"].Value)));
                        }

                    }
                    catch
                    {

                    }
                }
            }

            catch (Exception e)
            {
                throw new Exception("XML Document improperly formatted.", e);

                //If there was an error with the XML data, populate default world 

            }
        }
    }
}
