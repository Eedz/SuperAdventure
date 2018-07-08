using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
namespace Engine
{
    public class World
    {
        private const string WORLD_DATA_FILE_NAME = "WorldData.xml";

        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        //public const int ITEM_ID_ATLAS = 0;
        //public const int ITEM_ID_RUSTY_SWORD = 1;
        //public const int ITEM_ID_RAT_TAIL = 2;
        //public const int ITEM_ID_PIECE_OF_FUR = 3;
        //public const int ITEM_ID_SNAKE_FANG = 4;
        //public const int ITEM_ID_SNAKESKIN = 5;
        //public const int ITEM_ID_CLUB = 6;
        //public const int ITEM_ID_HEALING_POTION = 7;
        //public const int ITEM_ID_SPIDER_FANG = 8;
        //public const int ITEM_ID_SPIDER_SILK = 9;
        //public const int ITEM_ID_ADVENTURER_PASS = 10;


        //public const int MONSTER_ID_RAT = 1;
        //public const int MONSTER_ID_SNAKE = 2;
        //public const int MONSTER_ID_GIANT_SPIDER = 3;

        //public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        //public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        static World() {


            if (File.Exists(WORLD_DATA_FILE_NAME))
            {
                CreateWorldFromXmlString(File.ReadAllText(WORLD_DATA_FILE_NAME));
            }
            else
            {
                //PopulateItems();
                //PopulateMonsters();
                //PopulateQuests();
                //PopulateLocations();
            }

        }

        //private static void PopulateItems()
        //{

        //    Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5));
        //    Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails"));
        //    Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur"));
        //    Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs"));
        //    Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
        //    Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10));
        //    Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5));
        //    Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs"));
        //    Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks"));
        //    Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes"));
        //}

        //private static void PopulateMonsters()
        //{
        //    Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 3, 10, 3, 3);
        //    rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
        //    rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

        //    Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 3, 10, 3, 3);
        //    snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
        //    snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

        //    Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant spider", 20, 5, 40, 10, 10);
        //    giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
        //    giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));

        //    Monsters.Add(rat);
        //    Monsters.Add(snake);
        //    Monsters.Add(giantSpider);
        //}

        //private static void PopulateQuests()
        //{
        //    Quest clearAlchemistGarden = new Quest(QUEST_ID_CLEAR_ALCHEMIST_GARDEN, "Clear the alchemist's garden",
        //    "Kill rats in the alchemist's garden and bring back 3 rat tails. You will receive a healing potion and 10 gold pieces.", 20, 10);

        //    clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

        //    clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

        //    Quest clearFarmersField = new Quest(QUEST_ID_CLEAR_FARMERS_FIELD, "Clear the farmer's field", "Kill snakes in the farmer's field and bring back 3 snake fangs. You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

        //    clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(
        //        ItemByID(ITEM_ID_SNAKE_FANG), 3));

        //    clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

        //    Quests.Add(clearAlchemistGarden);
        //    Quests.Add(clearFarmersField);
        //}

        //private static void PopulateLocations()
        //{
        //    // Create each location
        //    Location home = new Location(LOCATION_ID_HOME, "Home", "Your house. You really need to clean up the place.");

        //    Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town square", "You see a fountain.");

        //    Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's hut", "There are many strange plants on the shelves.");
        //    alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

        //    Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Alchemist's garden", "Many plants are growing here.");
        //    alchemistsGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

        //    Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a small farmhouse, with a farmer in front.");
        //    farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

        //    Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's field", "You see rows of vegetables growing here.");
        //    farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

        //    Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a large, tough-looking guard here.",
        //    ItemByID(ITEM_ID_ADVENTURER_PASS));
        //    Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wide river.");

        //    Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "You see spider webs covering covering the trees in this forest.");
        //    spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

        //    // Link the locations together
        //    home.LocationToNorth = townSquare;

        //    townSquare.LocationToNorth = alchemistHut;
        //    townSquare.LocationToSouth = home;
        //    townSquare.LocationToEast = guardPost;
        //    townSquare.LocationToWest = farmhouse;

        //    farmhouse.LocationToEast = townSquare;
        //    farmhouse.LocationToWest = farmersField;

        //    farmersField.LocationToEast = farmhouse;

        //    alchemistHut.LocationToSouth = townSquare;
        //    alchemistHut.LocationToNorth = alchemistsGarden;

        //    alchemistsGarden.LocationToSouth = alchemistHut;

        //    guardPost.LocationToEast = bridge;
        //    guardPost.LocationToWest = townSquare;

        //    bridge.LocationToWest = guardPost;
        //    bridge.LocationToEast = spiderField;

        //    spiderField.LocationToWest = bridge;

        //    // Add the locations to the static list
        //    Locations.Add(home);
        //    Locations.Add(townSquare);
        //    Locations.Add(guardPost);
        //    Locations.Add(alchemistHut);
        //    Locations.Add(alchemistsGarden);
        //    Locations.Add(farmhouse);
        //    Locations.Add(farmersField);
        //    Locations.Add(bridge);
        //    Locations.Add(spiderField);
        //}

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

                    try
                    {
                        int minimumDmg = Convert.ToInt32(node.SelectSingleNode("MinimumDamage").InnerText);
                        int maximumDmg = Convert.ToInt32(node.SelectSingleNode("MaximumDamage").InnerText);
                        Items.Add(new Weapon(id, name, namePlural, minimumDmg, maximumDmg));
                    }
                    catch
                    {
                        Items.Add(new Item(id, name, namePlural));
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
                        

                        // Quest Rewards
                        foreach (XmlNode item in node["RewardItems"].ChildNodes)
                        {
                            int iid = Convert.ToInt32(item.Attributes["ID"].Value);

                            QuestByID(id).RewardItem = ItemByID(iid);
                        }
                   // }


                }
                
                // Populate Locations

                // Populate Locations
                foreach (XmlNode node in worldData.SelectNodes("/World/Locations/Zone"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    string name = node.Attributes["Name"].Value;
                    string description = node.Attributes["Description"].Value;

                    Locations.Add(new Location(id, name, description));
                }

                
                // Connections
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
                        LocationByID(id).QuestAvailableHere = QuestByID(Convert.ToInt32(node["Quest"].Attributes["ID"].Value));
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
                }

                

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                //If there was an error with the XML data, populate default world 
                //PopulateItems();
                //PopulateMonsters();
                //PopulateQuests();
                //PopulateLocations();
            }
        }
    }
}
