using Engine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SuperAdventure
{
    // TODO Randomly generate monsters and loot for each location when player enters the location
    // TODO create save slots
    // TODO create Load Game screen
    // TODO bear adopts family
    // TODO bear cave
    // TODO self image meter (other dad's are being more/less successful than you)
    public partial class SuperAdventure : Form
    {

        private string DataFile { get; set; }
        private Player _player;
        

        public SuperAdventure(string playerDataFile)
        {
            InitializeComponent();
            
            DataFile = playerDataFile;

            _player = Player.CreatePlayerFromXmlString(File.ReadAllText(DataFile));

            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");
            barWater.DataBindings.Add("Value", _player, "Water");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;

            dgvInventory.DataSource = _player.Inventory;

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;

            dgvQuests.DataSource = _player.Quests;

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "IsCompleted"
            });

            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "Id";

            if (_player.CurrentWeapon != null)
            {
                cboWeapons.SelectedItem = _player.CurrentWeapon;
            }

            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboPotions.DataSource = _player.Potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "Id";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _player.OnMessage += DisplayMessage;

            _player.MoveTo(_player.CurrentLocation);
        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.Text += messageEventArgs.Message + Environment.NewLine;

            if (messageEventArgs.AddExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Weapons")
            {
                cboWeapons.DataSource = _player.Weapons;
                if (!_player.Weapons.Any())
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }
            if (propertyChangedEventArgs.PropertyName == "Potions")
            {
                cboPotions.DataSource = _player.Potions;
                if (!_player.Potions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "CurrentLocation")
            {
                // Show/hide available movement buttons
                UpdateCompass(_player.CurrentLocation);

                // Update Map
                foreach (Button c in pnlWorldMap.Controls)
                {
                    c.Text = "??";
                    c.Image = null;
                }

                UpdateMap(_player.CurrentLocation, btnMap22);
                btnMap22.ForeColor = Color.Red;
                btnMap22.FlatStyle = FlatStyle.Flat;
                btnMap22.FlatAppearance.BorderColor = Color.Red;
                btnMap22.FlatAppearance.BorderSize = 2;

                // Display current location name and description
                rtbLocation.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtbLocation.Text += _player.CurrentLocation.Description + Environment.NewLine;

                // TODO Randomly generate some loot for this location
                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (InventoryItem invItem in _player.CurrentLocation.ItemsAvailableForPickup)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= 33)
                    {

                        
                        //_player.CurrentLocation.ItemsAvailableForPickup.Add(new LootItem(invItem.Details, RandomNumberGenerator.NumberBetween(1, 3), false));

                    }

                }

                // Check if a monster is here TODO randomly generate a monster?
                if (_player.CurrentLocation.MonsterLivingHere == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                }
                else
                {
                    cboWeapons.Visible = _player.Weapons.Any();
                    cboPotions.Visible = _player.Potions.Any();
                    btnUseWeapon.Visible = _player.Weapons.Any();
                    btnUsePotion.Visible = _player.Potions.Any();
                }

                btnTrade.Visible = (_player.CurrentLocation.VendorWorkingHere != null);

                btnPickUpItems.Visible = (_player.CurrentLocation.ItemsAvailableForPickup.Count != 0);
            }
            

            }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            _player.MoveNorth();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _player.MoveEast();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _player.MoveSouth();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            _player.MoveWest();
        }

        private void btnTrade_Click(object sender, EventArgs e)
        {
            TradingScreen tradingScreen = new TradingScreen(_player, false);
            tradingScreen.StartPosition = FormStartPosition.CenterParent;
            tradingScreen.ShowDialog(this);
        }

        private void btnPickUpItems_Click(object sender, EventArgs e)
        {
            LootScreen lootScreen = new LootScreen(_player, true);
            lootScreen.StartPosition = FormStartPosition.CenterParent;
            lootScreen.ShowDialog(this);
        }

        private void UpdateCompass(Location newLocation)
        {
            // Show/hide compass buttons
            btnNorth.Enabled = (newLocation.LocationToNorth != null);
            btnEast.Enabled = (newLocation.LocationToEast != null);
            btnSouth.Enabled = (newLocation.LocationToSouth != null);
            btnWest.Enabled = (newLocation.LocationToWest != null);

            // label known locations
            if (_player.Atlas.HasLocation(newLocation.LocationToNorth))
                lblNorth.Text = newLocation.LocationToNorth.Name;
            else
                lblNorth.Text = "??";
            
            if (_player.Atlas.HasLocation(newLocation.LocationToEast))
                lblEast.Text = newLocation.LocationToEast.Name;
            else
                lblEast.Text = "??";

            if (_player.Atlas.HasLocation(newLocation.LocationToSouth))
                lblSouth.Text = newLocation.LocationToSouth.Name;
            else
                lblSouth.Text = "??";

            if (_player.Atlas.HasLocation(newLocation.LocationToWest))
                lblWest.Text = newLocation.LocationToWest.Name;
            else
                lblWest.Text = "??";

            // clear the labels if there is no location in a particular direction
            if (newLocation.LocationToNorth == null)
                lblNorth.Text = "";

            if (newLocation.LocationToEast == null)
                lblEast.Text = "";

            if (newLocation.LocationToSouth == null)
                lblSouth.Text = "";

            if (newLocation.LocationToWest == null)
                lblWest.Text = "";

            
        }

        private void UpdateMap(Location currentLocation, Button btnCenter)
        {
            // Get the X and Y co-ords of the center button
            int x, y;
            x = Int32.Parse(btnCenter.Name.Substring(6, 1));
            y = Int32.Parse(btnCenter.Name.Substring(7, 1));

            Button north =null, east=null, south = null, west = null;

            foreach (Control c in pnlWorldMap.Controls)
            {
                if (c.Name.Equals("btnMap" + x + (y + 1)))
                {
                    north = (Button)c;
                } else if (c.Name.Equals("btnMap" + (x+1) + (y)))
                {
                    east = (Button)c;
                } else if (c.Name.Equals("btnMap" + x + (y - 1)))
                {
                    south = (Button)c;
                } else if (c.Name.Equals("btnMap" + (x-1) + (y)))
                {
                    west = (Button)c;
                }
            }

            // Set the picture of the button
            SetButtonPicture(currentLocation, btnCenter);
            // Label the center button with the current location's name
            //btnCenter.Text = currentLocation.Name;

            // Fill out area to north of center
            if (north != null && north.Text.Equals("??"))
            {
                if (_player.Atlas.HasLocation(currentLocation.LocationToNorth))
                {
                    north.Text = currentLocation.LocationToNorth.Name;
                    UpdateMap(currentLocation.LocationToNorth, north);
                }
            }

            if (east != null && east.Text.Equals("??"))
            {
                if (_player.Atlas.HasLocation(currentLocation.LocationToEast))
                {
                    east.Text = currentLocation.LocationToEast.Name;
                    UpdateMap(currentLocation.LocationToEast, east);
                }
            }

            if (south != null && south.Text.Equals("??"))
            {
                if (_player.Atlas.HasLocation(currentLocation.LocationToSouth))
                {
                    south.Text = currentLocation.LocationToSouth.Name;
                    UpdateMap(currentLocation.LocationToSouth, south);
                }
            }

            if (west != null && west.Text.Equals("??"))
            {
                if (_player.Atlas.HasLocation(currentLocation.LocationToWest))
                {
                    west.Text = currentLocation.LocationToWest.Name;
                    UpdateMap(currentLocation.LocationToWest, west);
                }
            }


        }

        private void SetButtonPicture(Location loc, Button btn)
        {
            if (loc.Name.Contains("East/North/South"))
            {
                btn.Text = "";
                btn.Image = Properties.Resources.RoadENS;
            }
            else if (loc.Name.Contains("North/East"))
            { 
                btn.Text = "";
                btn.Image = Properties.Resources.RoadNE;
            }
            else if (loc.Name.Contains("North/South"))
            {
                btn.Text = "";
                btn.Image = Properties.Resources.RoadNS;
            } else if (loc.Name.Contains("East/West"))
            {
                btn.Text = "";
                btn.Image = Properties.Resources.RoadEW;
            } else if (loc.Name.Contains("All"))
            {
                btn.Text = "";
                btn.Image = Properties.Resources.RoadAll;
            }
            else if (loc.Name.Equals("Front Gate"))
            {
                btn.Text = "";
                btn.Image = Properties.Resources.Gatehouse;
            }
            else if (loc.Name.Contains("Site"))
            {
                btn.Text = "";
                btn.Image = Properties.Resources.Campsite;
            }
            else
            {
                btn.Image = null;
                btn.Text = loc.Name;
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Get the currently selected weapon from the cboWeapons ComboBox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            _player.UseWeapon(currentWeapon);
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            _player.UsePotion(potion);
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DataFile.StartsWith("Default"))
                DataFile = DataFile.Replace("Default", "");

            File.WriteAllText(DataFile, _player.ToXmlString());
            
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        
    }
}
