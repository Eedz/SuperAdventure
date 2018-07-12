using System;
using System.IO;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class SuperAdventureMenu : Form
    {
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        private const string DEFAULT_PLAYER_DATA_FILE_NAME = "DefaultPlayerData.xml";

        public SuperAdventureMenu()
        {
            InitializeComponent();

            // Only enable the Load button if a player data file exists.    
            btnLoadGame.Enabled = File.Exists(PLAYER_DATA_FILE_NAME);
            
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            try
            {
                LoadWorld();
            }
            catch (Exception ex)
            {
                MessageBox.Show("CampgroundData.xml failed to load.");
                Application.Exit();
                return;
            }
            
           
            SuperAdventure mainForm = new SuperAdventure(DEFAULT_PLAYER_DATA_FILE_NAME);
            mainForm.Show();
            //this.Hide();
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            LoadWorld();
            SuperAdventure mainForm = new SuperAdventure(PLAYER_DATA_FILE_NAME);
            mainForm.Show();
            //this.Hide();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadWorld()
        {
            World.ItemByID(1);
        }
        
    }
}
