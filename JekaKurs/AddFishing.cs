using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JekaKurs
{
    public partial class AddFishing : Form
    {
        Database db;
        Dictionary<int, string> fishDict;
        Dictionary<int, string> fishermensDict;
        Dictionary<int, string> teamsDict;
        List<string> teamsList;
        List<string> fishermensList;
        List<string> fishList;

        public int TeamId { get; set; }
        public int FishermenId { get; set; }
        public int FishId { get; set; }
        public bool IsSucceded { get; set; } = false;

        public AddFishing()
        {
            InitializeComponent();
            db = new Database();
            teamsList = new List<string>();
            fishermensList = new List<string>();
            fishList = new List<string>();
            teamsDict = db.GetTeams();
            fishDict = db.GetFish();

            SpareCollection(fishList, fishDict);
            SpareCollection(teamsList, teamsDict);

            cbTeams.Items.AddRange(teamsList.ToArray());
            cbFish.Items.AddRange(fishList.ToArray());

            cbTeams.SelectedIndex = 0;
            cbFish.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            TeamId = GetId((string)cbTeams.SelectedItem);
            FishermenId = GetId((string)cbFishermen.SelectedItem);
            FishId = GetId((string)cbFish.SelectedItem);
            IsSucceded = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFishermen.Items.Clear();
            fishermensDict = db.GetFishermens(GetId((string)cbTeams.SelectedItem));
            SpareCollection(fishermensList, fishermensDict);
            if (fishermensList.Count > 0)
            {
                cbFishermen.Enabled = true;
                cbFishermen.Items.AddRange(fishermensList.ToArray());
                cbFishermen.SelectedIndex = 0;
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
                cbFishermen.Enabled = false;
            }
        }

        private int GetId(string str)
        {
            string result = str.Split('(', ')')[1];

            return int.Parse(result);
        }

        private void SpareCollection(List<string> collection, Dictionary<int, string> dict)
        {
            foreach (var item in dict)
            {
                string result = $"Id({item.Key}) - {item.Value}";
                collection.Add(result);
            }
        }


    }
}
