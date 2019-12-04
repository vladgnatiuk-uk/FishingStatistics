using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JekaKurs
{
    public partial class FishCompany : Form
    {
        Database db;
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FishCompany;Integrated Security=True";
        List<string> tasksList;
        string ResultChoose { get; set; }

        public FishCompany()
        {
            InitializeComponent();
            db = new Database();
            db.StartUpDatabase();
            tasksList = new List<string>()
            {
                "Количество команд",
                "Количество рыбаков",
                "Количество кораблей",
                "Количество рыб",
                "Улов команды",
                "Улов рыбака"
            };
            cbResult.Items.AddRange(tasksList.ToArray());
        }

        private void FishCompany_Load(object sender, EventArgs e)
        {
            UploadGridsViews();
        }

        private void UploadGridsViews()
        {
            UploadTeamGrid();
            UploadFishingGrid();
            UploadBoatsGrid();
            UploadFishGrid();
            UploadFishermensGrid();
            UploadTeamsIdentGrid();

            cbSex.SelectedIndex = 0;
        }

        private void UploadTeamGrid()
        {
            string sql = $@"SELECT * FROM Teams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                dgvTeams.DataSource = ds.Tables[0];
            }
        }

        private void UploadFishingGrid()
        {
            string sql = $@"SELECT * FROM Fishing";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                dgvFishings.DataSource = ds.Tables[0];
            }
        }

        private void UploadBoatsGrid()
        {
            string sql = $@"SELECT * FROM Boats";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                dgvBoats.DataSource = ds.Tables[0];
            }
        }

        private void UploadFishGrid()
        {
            string sql = $@"SELECT * FROM Fish";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                dgvFish.DataSource = ds.Tables[0];
            }
        }

        private void UploadFishermensGrid()
        {
            string sql = $@"SELECT * FROM Fishermens";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                dgvFishermens.DataSource = ds.Tables[0];
            }
        }

        private void UploadTeamsIdentGrid()
        {
            string sql = $@"SELECT * FROM TeamIdentification";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();

                adapter.Fill(ds);

                dgvTeamIdent.DataSource = ds.Tables[0];
            }
        }

        private void dgvTeams_MouseClick(object sender, MouseEventArgs e)
        {
            txtTId.Text = dgvTeams.CurrentRow.Cells[0].Value.ToString();
            txtTBoatId.Text = dgvTeams.CurrentRow.Cells[1].Value.ToString();
            txtTFishermenId.Text = dgvTeams.CurrentRow.Cells[2].Value.ToString();
            txtTTName.Text = dgvTeams.CurrentRow.Cells[3].Value.ToString();
            txtTPosition.Text = dgvTeams.CurrentRow.Cells[4].Value.ToString();
            dateTReceiptDate.Value = Convert.ToDateTime(dgvTeams.CurrentRow.Cells[5].Value);
        }

        private void dgvFishings_MouseClick(object sender, MouseEventArgs e)
        {
            txtFId.Text = dgvFishings.CurrentRow.Cells[0].Value.ToString();
            txtFBoatId.Text = dgvFishings.CurrentRow.Cells[1].Value.ToString();
            txtFFishermenId.Text = dgvFishings.CurrentRow.Cells[2].Value.ToString();
            txtFFishId.Text = dgvFishings.CurrentRow.Cells[3].Value.ToString();
            dateFOutDate.Value = Convert.ToDateTime(dgvFishings.CurrentRow.Cells[4].Value);
            dateFRecieveDate.Value = Convert.ToDateTime(dgvFishings.CurrentRow.Cells[5].Value);
            txtFWeight.Text = dgvFishings.CurrentRow.Cells[6].Value.ToString();
        }

        private void dgvBoats_MouseClick(object sender, MouseEventArgs e)
        {
            txtBId.Text = dgvBoats.CurrentRow.Cells[0].Value.ToString();
            txtBName.Text = dgvBoats.CurrentRow.Cells[1].Value.ToString();
            txtBType.Text = dgvBoats.CurrentRow.Cells[2].Value.ToString();
            txtBDisplacement.Text = dgvBoats.CurrentRow.Cells[3].Value.ToString();
            dateBDateCreation.Value = Convert.ToDateTime(dgvBoats.CurrentRow.Cells[4].Value);
        }

        private void dgvFish_MouseClick(object sender, MouseEventArgs e)
        {
            txtFsId.Text = dgvFish.CurrentRow.Cells[0].Value.ToString();
            txtFsName.Text = dgvFish.CurrentRow.Cells[1].Value.ToString();
            txtFsSName.Text = dgvFish.CurrentRow.Cells[2].Value.ToString();
            txtFsPlace.Text = dgvFish.CurrentRow.Cells[3].Value.ToString();
            txtFsAW.Text = dgvFish.CurrentRow.Cells[4].Value.ToString();
        }

        private void dgvFishermens_MouseClick(object sender, MouseEventArgs e)
        {
            txtRId.Text = dgvFishermens.CurrentRow.Cells[0].Value.ToString();
            txtRName.Text = dgvFishermens.CurrentRow.Cells[1].Value.ToString();
            txtRSurname.Text = dgvFishermens.CurrentRow.Cells[2].Value.ToString();
            txtRExpirience.Text = dgvFishermens.CurrentRow.Cells[3].Value.ToString();
            txtRPNumber.Text = dgvFishermens.CurrentRow.Cells[4].Value.ToString();
            txtRAddress.Text = dgvFishermens.CurrentRow.Cells[5].Value.ToString();
            cbSex.SelectedItem = GetSex(cbSex, dgvFishermens.CurrentRow.Cells[6].Value.ToString());
        }

        private int GetSex(ComboBox comboBox, string value)
        {
            int index = -1;
            int counter = 0;
            foreach (var item in comboBox.Items)
            {
                if (((string)item) == value)
                {
                    index = counter;
                    break;
                }
                else
                {
                    counter++;
                }
            }

            return index;
        }

        private void btnTAdd_Click(object sender, EventArgs e)
        {
            AddTeam add = new AddTeam();
            add.ShowDialog();

            if (add.IsSucceded)
            {
                int boatId = add.BoatId;
                int fishermenId = add.FishermenId;
                int teamId = add.TeamId;
                string position = txtTPosition.Text;
                string date = GetDate(dateTReceiptDate.Value);

                string addQuery = string.Format(@"INSERT INTO Teams
                                              VALUES
                                              ({0},{1},{2},'{3}','{4}');", boatId, fishermenId, teamId, position, date);

                int result = db.AddTeam(addQuery);

                if (result == 1)
                {
                    MessageBox.Show("Данные были успешно добавлены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Данные не были добавлены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                UploadTeamGrid();

            }
        }

        private void btnTUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtTId.Text);
            int boatId = int.Parse(txtTBoatId.Text);
            int fishermenId = int.Parse(txtTFishermenId.Text);
            int teamId = int.Parse(txtTTName.Text);
            string position = txtTPosition.Text;
            string date = GetDate(dateTReceiptDate.Value);

            string insert = string.Format(@"UPDATE Teams
                              SET BoatId = {1},
                                  FishermenId = {2},
                                  TeamId = {3},
                                  Position = '{4}',
                                  ReceiptDate = '{5}'
                              WHERE Id = {0}", id, boatId, fishermenId, teamId, position, date);

            int result = db.UpdateTeamRelations(insert);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно изменены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были изменены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadTeamGrid();
        }

        private void btnTDelte_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtTId.Text);
            int result = db.DeleteTeamRelations(id);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно удалены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UploadTeamGrid();
            }
            else
            {
                MessageBox.Show("Данные не были удалены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UploadTeamGrid();
            }
        }

        private string GetDate(DateTime datetime)
        {
            string[] parsedDate = datetime.ToString("MM/dd/yyyy").Split('.');
            string date = string.Empty;

            foreach (var item in parsedDate)
            {
                date += item + "/";
            }
            date = date.TrimEnd('/');
            return date;
        }

        private void btnFAdd_Click(object sender, EventArgs e)
        {
            AddFishing add = new AddFishing();
            add.ShowDialog();

            if (add.IsSucceded)
            {
                int teamId = add.TeamId;
                int fishermenId = add.FishermenId;
                int fishId = add.FishId;
                string dateO = GetDate(dateFOutDate.Value);
                string dateI = GetDate(dateFRecieveDate.Value);
                float weight = float.Parse(txtFWeight.Text);

                string addQuery = string.Format(@"INSERT INTO Fishing
                                              (TeamId, FishermenId, FishId, OutDate, ReturnDate, Weight)
                                              VALUES                                              
                                              ({0}, {1}, {2}, '{3}', '{4}', @weight);", teamId, fishermenId, fishId, dateO, dateI);

                int result = db.AddFishing(addQuery, weight);

                if (result == 1)
                {
                    MessageBox.Show("Данные были успешно добавлены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Данные не были добавлены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                UploadFishingGrid();

            }
        }

        private void btnFUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtFId.Text);
            int teamId = int.Parse(txtFBoatId.Text);
            int fishermenId = int.Parse(txtFFishermenId.Text);
            int fishId = int.Parse(txtFFishId.Text);
            string dateO = GetDate(dateFOutDate.Value);
            string dateI = GetDate(dateFRecieveDate.Value);
            float weight = float.Parse(txtFWeight.Text);

            string insert = string.Format(@"UPDATE Fishing
                              SET TeamId = {1},
                                  FishermenId = {2},
                                  FishId = {3},
                                  OutDate = '{4}',
                                  ReturnDate = '{5}',
                                  Weight = @weight
                              WHERE Id = {0}", id, teamId, fishermenId, fishId, dateO, dateI);

            int result = db.UpdateFishing(insert, weight);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно изменены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были изменены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishingGrid();
        }

        private void btnFDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtFId.Text);
            int result = db.DeleteFishing(id);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно удалены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были удалены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishingGrid();

        }

        private void btnBAdd_Click(object sender, EventArgs e)
        {
            string name = txtBName.Text;
            string type = txtBType.Text;
            float displacement = float.Parse(txtBDisplacement.Text);
            string date = GetDate(dateFOutDate.Value);

            string addQuery = string.Format(@"INSERT INTO Boats
                                              VALUES
                                              ('{0}', '{1}', @displac, '{2}');", name, type, date);

            int result = db.AddBoats(addQuery, displacement);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно добавлены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были добавлены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadBoatsGrid();

        }

        private void btnBUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtBId.Text);
            string name = txtBName.Text;
            string type = txtBType.Text;
            float displacement = float.Parse(txtBDisplacement.Text);
            string date = GetDate(dateFOutDate.Value);

            string insert = string.Format(@"UPDATE Boats
                              SET Name = '{1}',
                                  Type = '{2}',
                                  Displacement = @displac,
                                  DateOfConstruction = '{3}'
                              WHERE Id = {0}", id, name, type, date);

            int result = db.UpdateBoats(insert, displacement);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно изменены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были изменены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadBoatsGrid();

        }

        private void btnBDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtBId.Text);
            int result = db.DeleteBoat(id);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно удалены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были удалены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadBoatsGrid();

        }

        private void btnFsAdd_Click(object sender, EventArgs e)
        {
            string name = txtFsName.Text;
            string sname = txtFsSName.Text;
            string habitat = txtFsPlace.Text;
            float weight = float.Parse(txtFsAW.Text);

            string addQuery = string.Format(@"INSERT INTO Fish
                                              VALUES
                                              ('{0}', '{1}', '{2}', @weight);", name, sname, habitat);

            int result = db.AddFish(addQuery, weight);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно добавлены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были добавлены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishGrid();

        }

        private void btnFsUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtFsId.Text);
            string name = txtFsName.Text;
            string sname = txtFsSName.Text;
            string habitat = txtFsPlace.Text;
            float weight = float.Parse(txtFsAW.Text);

            string insert = string.Format(@"UPDATE Fish
                              SET Name = '{1}',
                                  ScienceName = '{2}',
                                  Habitat = '{3}',
                                  AverageWeight = @weight
                              WHERE Id = {0}", id, name, sname, habitat);

            int result = db.UpdateFish(insert, weight);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно изменены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были изменены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishGrid();

        }

        private void btnFsDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtFsId.Text);
            int result = db.DeleteFish(id);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно удалены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были удалены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishGrid();

        }

        private void btnRAdd_Click(object sender, EventArgs e)
        {
            string name = txtRName.Text;
            string surname = txtRSurname.Text;
            int expirience = int.Parse(txtRExpirience.Text);
            string phone = txtRPNumber.Text;
            string address = txtRAddress.Text;
            string sex = (string)cbSex.SelectedItem;

            string addQuery = string.Format(@"INSERT INTO Fishermens
                                              VALUES
                                              ('{0}', '{1}', {2}, '{3}', '{4}', '{5}');", name, surname, expirience, phone, address, sex);

            int result = db.AddFishermen(addQuery);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно добавлены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были добавлены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishermensGrid();

        }

        private void btnRUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtRId.Text);
            string name = txtRName.Text;
            string surname = txtRSurname.Text;
            int expirience = int.Parse(txtRExpirience.Text);
            string phone = txtRPNumber.Text;
            string address = txtRAddress.Text;
            string sex = (string)cbSex.SelectedItem;

            string insert = string.Format(@"UPDATE Fishermens
                              SET Name = '{1}',
                                  Surname = '{2}',
                                  Expirience = {3},
                                  Phone = '{4}',
                                  Address = '{5}',
                                  Sex = '{6}'
                              WHERE Id = {0}", id, name, surname, expirience, phone, address, sex);

            int result = db.UpdateFishermen(insert);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно изменены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были изменены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishermensGrid();

        }

        private void btnRDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtRId.Text);
            int result = db.DeleteFishermen(id);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно удалены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были удалены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadFishermensGrid();

        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            string task = (string)cbResult.SelectedItem;
            int result = 0;

            switch (task)
            {
                case "Количество команд":
                    result = db.CountTeams();
                    labelResult.Text = task;
                    txtRes.Text = Convert.ToString(result);
                    break;
                case "Количество рыбаков":
                    result = db.CountFishermens();
                    labelResult.Text = task;
                    txtRes.Text = Convert.ToString(result);
                    break;
                case "Количество кораблей":
                    result = db.CountBoats();
                    labelResult.Text = task;
                    txtRes.Text = Convert.ToString(result);
                    break;
                case "Количество рыб":
                    result = db.CountFish();
                    labelResult.Text = task;
                    txtRes.Text = Convert.ToString(result);
                    break;
                case "Улов команд":
                    result = db.CountFish();
                    labelResult.Text = task;
                    txtRes.Text = Convert.ToString(result);
                    break;
                case "Улов команды":
                    cbFishermens.Items.Clear();
                    var teamsDict = db.GetTeams();
                    List<string> teamList = new List<string>();
                    SpareCollection(teamList, teamsDict);
                    cbFishermens.Items.AddRange(teamList.ToArray());
                    cbFishermens.SelectedIndex = 0;

                    lblFisher.Text = "Выберите команду";

                    lblFisher.Visible = true;
                    cbFishermens.Visible = true;
                    btnFisherChoose.Visible = true;

                    ResultChoose = "Улов команды";

                    break;
                case "Улов рыбака":
                    cbFishermens.Items.Clear();
                    var fishermensDict = db.GetFishermens();
                    List<string> fishermensList = new List<string>();
                    SpareCollection(fishermensList, fishermensDict);
                    cbFishermens.Items.AddRange(fishermensList.ToArray());
                    cbFishermens.SelectedIndex = 0;

                    lblFisher.Text = "Выберите рыбака";

                    lblFisher.Visible = true;
                    cbFishermens.Visible = true;
                    btnFisherChoose.Visible = true;

                    ResultChoose = "Улов рыбака";

                    break;
            }

        }

        private void btnFisherChoose_Click(object sender, EventArgs e)
        {
            switch (ResultChoose)
            {
                case "Улов рыбака":
                    {
                        dgvResFishingTeams.Visible = true;
                        int id = GetId((string)cbFishermens.SelectedItem);
                        string task = (string)cbResult.SelectedItem;

                        int result = db.CountFishermenFish(id);
                        labelResult.Text = task;
                        txtRes.Text = Convert.ToString(result);

                        string sql = $@"SELECT ti.Name as TeamName, fm.Name + ' ' + fm.Surname as FishermenName, f.Name as Fish, fish.OutDate, fish.ReturnDate, fish.Weight
                                        FROM Fishing as fish
                                        INNER JOIN TeamIdentification as ti
                                        ON fish.TeamId = ti.Id
                                        INNER JOIN Fishermens as fm
                                        ON fish.FishermenId = fm.Id
                                        INNER JOIN Fish as f
                                        ON fish.FishId = f.Id
                                        WHERE fm.Id = @id";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlDataAdapter adapter = new SqlDataAdapter();

                            SqlParameter parameter1 = new SqlParameter("@id", id);

                            adapter.SelectCommand = new SqlCommand(sql, connection);
                            adapter.SelectCommand.Parameters.Add(parameter1);


                            DataSet ds = new DataSet();

                            adapter.Fill(ds);

                            dgvResFishingTeams.DataSource = ds.Tables[0];
                        }
                    }
                    break;
                case "Улов команды":
                    {
                        dgvResFishingTeams.Visible = true;
                        int id = GetId((string)cbFishermens.SelectedItem);
                        string task = (string)cbResult.SelectedItem;

                        int result = db.CountFishingTeams(id);
                        labelResult.Text = task;
                        txtRes.Text = Convert.ToString(result);

                        string sql = $@"SELECT ti.Name as TeamName, fm.Name + ' ' + fm.Surname as FishermenName, f.Name as Fish, fish.OutDate, fish.ReturnDate, fish.Weight
                                        FROM Fishing as fish
                                        INNER JOIN TeamIdentification as ti
                                        ON fish.TeamId = ti.Id
                                        INNER JOIN Fishermens as fm
                                        ON fish.FishermenId = fm.Id
                                        INNER JOIN Fish as f
                                        ON fish.FishId = f.Id
                                        WHERE TeamId = @id";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            SqlDataAdapter adapter = new SqlDataAdapter();

                            SqlParameter parameter1 = new SqlParameter("@id", id);

                            adapter.SelectCommand = new SqlCommand(sql, connection);
                            adapter.SelectCommand.Parameters.Add(parameter1);


                            DataSet ds = new DataSet();

                            adapter.Fill(ds);

                            dgvResFishingTeams.DataSource = ds.Tables[0];
                        }

                        break;
                    }
            }
        }

        private void cbResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblFisher.Visible = false;
            cbFishermens.Visible = false;
            btnFisherChoose.Visible = false;
            dgvResFishingTeams.Visible = false;
        }

        private void btnTIAdd_Click(object sender, EventArgs e)
        {
            string name = txtTeamIdentName.Text;

            string addQuery = string.Format(@"INSERT INTO TeamIdentification
                                              VALUES
                                              ('{0}');", name);

            int result = db.AddTeamIdent(addQuery);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно добавлены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были добавлены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadTeamsIdentGrid();
        }

        private void btnTIC_Click(object sender, EventArgs e)
        {

            int id = int.Parse(txtTeamIdent.Text);
            string name = txtTeamIdentName.Text;


            string insert = string.Format(@"UPDATE TeamIdentification
                              SET Name = '{1}'                                 
                              WHERE Id = {0}", id, name);

            int result = db.UpdateTeamIdent(insert);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно изменены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были изменены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadTeamsIdentGrid();
        }

        private void btnTID_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtTeamIdent.Text);
            int result = db.DeleteTeamIdent(id);

            if (result == 1)
            {
                MessageBox.Show("Данные были успешно удалены!", "Система", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Данные не были удалены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UploadTeamsIdentGrid();
        }

        private void dgvTeamIdent_MouseClick(object sender, MouseEventArgs e)
        {
            txtTeamIdent.Text = dgvTeamIdent.CurrentRow.Cells[0].Value.ToString();
            txtTeamIdentName.Text = dgvTeamIdent.CurrentRow.Cells[1].Value.ToString();
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
