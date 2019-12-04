using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JekaKurs
{
    public partial class AddTeam : Form
    {
        Database db;
        Dictionary<int, string> boatsDict;
        Dictionary<int, string> fishermensDict;
        Dictionary<int, string> teamsDict;
        List<string> boatsList;
        List<string> fishermensList;
        List<string> teamsList;

        string regexPatter = @"Id(?<id>\d+)*";

        public int BoatId { get; set; }
        public int FishermenId { get; set; }
        public int TeamId { get; set; }
        public bool IsSucceded { get; set; } = false;

        public AddTeam()
        {
            InitializeComponent();
            db = new Database();
            boatsDict = db.GetBoats();
            fishermensDict = db.GetFishermens();
            teamsDict = db.GetTeams();
            boatsList = new List<string>();
            fishermensList = new List<string>();
            teamsList = new List<string>();
            SpareCollection(boatsList, boatsDict);
            SpareCollection(fishermensList, fishermensDict);
            SpareCollection(teamsList, teamsDict);


            cbBoats.Items.AddRange(boatsList.ToArray());
            cbFishermens.Items.AddRange(fishermensList.ToArray());
            cbTeam.Items.AddRange(teamsList.ToArray());

            cbBoats.SelectedIndex = 0;
            cbFishermens.SelectedIndex = 0;
            cbTeam.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            BoatId = GetId((string)cbBoats.SelectedItem);/*cbBoats.SelectedIndex + 1;*/
            FishermenId = GetId((string)cbFishermens.SelectedItem); //cbFishermens.SelectedIndex + 1;
            TeamId = GetId((string)cbTeam.SelectedItem); // cbTeam.SelectedIndex + 1;
            IsSucceded = true;
            this.Close();
        }

        private void SpareCollection(List<string> collection, Dictionary<int, string> dict)
        {
            foreach (var item in dict)
            {
                string result = $"Id({item.Key}) - {item.Value}";
                collection.Add(result);
            }
        }

        private int GetId(string str)
        {
            string result = str.Split('(', ')')[1];

            return int.Parse(result);
        }
    }
}
