using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI;
using System.Windows.Forms;


namespace Reserve__a_Five_a_Side_Football
{
    public partial class OwnerAddLeague : BaseForm
    {
        Reserve_a_Five_a_SideEntities DB = new Reserve_a_Five_a_SideEntities();
        int id;
        private int leagueID;

        public OwnerAddLeague()
        {
            InitializeComponent();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
            var specificColumnsData = DB.Legaues
        .Select(l => new
        {
            l.LegueID,
            l.Legue_Name,
            l.BeginDate,
            l.EndDate,
            l.StadiumName,
            l.EndReg,
            l.Reward,
            l.City,
            l.TimePlay,
            l.NumberOfSubscribersInlegaue,
            l.CurrentSubscriberslegaue
        }).ToList();

            dataGridView1.DataSource = specificColumnsData;
            LoadUniqueAreas();
        }

        private void OwnerAddLeague_Load(object sender, EventArgs e)
        {
            //var StadiumName = DB.Stadium.Select(et => et.Stad_Name).ToList();
            //foreach (var item in StadiumName)
            //    StadiumNameCmb.Items.Add(item);


            TimePlayDate.Format = DateTimePickerFormat.Custom;
            TimePlayDate.CustomFormat = "hh:00:00";
            TimePlayDate.ShowUpDown = true;
        }
        private void LoadUniqueAreas()
        {
            var uniqueAreas = DB.Stadium.Select(s => s.Area).Distinct().ToArray();
            CityCmb.Items.AddRange(uniqueAreas);
        }


        private bool CheckForConflicts(DateTime BeginDate, DateTime EndDate, string selectedStadium, string selectedCity)
        {
            var existingLeagues = DB.Legaues
        .Where(l => l.StadiumName == selectedStadium && l.City == selectedCity &&
                    ((BeginDate >= l.BeginDate && BeginDate < l.EndDate) ||
                     (EndDate > l.BeginDate && EndDate <= l.EndDate) ||
                     (BeginDate < l.BeginDate && EndDate > l.EndDate))).ToList();

            return existingLeagues.Any();
        }

        // Over Load To Update
        private bool CheckForConflicts(DateTime BeginDate, DateTime EndDate, string selectedStadium, string selectedCity, int leagueID)
        {
            var existingLeagues = DB.Legaues
                .Where(l => l.LegueID != leagueID && // Exclude the league with the specified ID
                            l.StadiumName == selectedStadium && l.City == selectedCity &&
                            ((BeginDate >= l.BeginDate && BeginDate < l.EndDate) ||
                             (EndDate > l.BeginDate && EndDate <= l.EndDate) ||
                             (BeginDate < l.BeginDate && EndDate > l.EndDate)))
                .ToList();

            return existingLeagues.Any();
        }
        private bool ValidateInput()
        {

            DateTime currentDate = DateTime.Today;

            if (string.IsNullOrWhiteSpace(legaueNametxt.Text) ||
                string.IsNullOrWhiteSpace(Rewardtxt.Text) ||
                StadiumNameCmb.SelectedItem == null ||
                CityCmb.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }

            // 2. Date Validation
            DateTime beginDate = BeginDate.Value.Date;
            DateTime endDate = EndDate.Value.Date;
            DateTime endRegistration = EndRegistration.Value.Date;

            if (beginDate >= endDate)
            {
                MessageBox.Show("Begin Date must be before End Date.");
                return false;
            }

            if (beginDate <= currentDate)
            {
                MessageBox.Show("Begin Date must be in the future.");
                return false;
            }
            if (endDate <= beginDate || endDate <= currentDate)
            {
                MessageBox.Show("End Date must be greater than Begin Date and in the future.");
                return false;
            }

            if (endRegistration >= beginDate || endRegistration <= currentDate)
            {
                MessageBox.Show("Registration Date must be greater than Begin Date and EndDate and the current date.");
                return false;
            }
            // DateTime selectedTime = TimePlayDate.Value.Date;
            string selectedStadium = StadiumNameCmb.SelectedItem.ToString();
            string selectedCity = CityCmb.SelectedItem.ToString();

            if (CheckForConflicts(beginDate, endDate, selectedStadium, selectedCity))
            {
                MessageBox.Show("There is a league or event scheduled at the chosen stadium and city during the selected time.");
                return false;
            }

            if (SubNumUpDown.Value <= 3)
            {
                MessageBox.Show("Sorry League Must Be 4 Or More");
                return false;
            }

            return true;
        }


        private bool ValidateInput(int idForLeagu)
        {

            DateTime currentDate = DateTime.Today;

            if (string.IsNullOrWhiteSpace(legaueNametxt.Text) ||
                string.IsNullOrWhiteSpace(Rewardtxt.Text) ||
                StadiumNameCmb.SelectedItem == null ||
                CityCmb.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }

            // 2. Date Validation
            DateTime beginDate = BeginDate.Value.Date;
            DateTime endDate = EndDate.Value.Date;
            DateTime endRegistration = EndRegistration.Value.Date;

            if (beginDate >= endDate)
            {
                MessageBox.Show("Begin Date must be before End Date.");
                return false;
            }

            if (beginDate <= currentDate)
            {
                MessageBox.Show("Begin Date must be in the future.");
                return false;
            }
            if (endDate <= beginDate || endDate <= currentDate)
            {
                MessageBox.Show("End Date must be greater than Begin Date and in the future.");
                return false;
            }

            if (endRegistration >= beginDate || endRegistration <= currentDate)
            {
                MessageBox.Show("Registration Date must be greater than Begin Date and EndDate and the current date.");
                return false;
            }
            // DateTime selectedTime = TimePlayDate.Value.Date;
            string selectedStadium = StadiumNameCmb.SelectedItem.ToString();
            string selectedCity = CityCmb.SelectedItem.ToString();

            if (CheckForConflicts(beginDate, endDate, selectedStadium, selectedCity, idForLeagu))
            {
                MessageBox.Show("There is a league or event scheduled at the chosen stadium and city during the selected time.");
                return false;
            }



            return true;
        }
        private void AddNewLegauebtn_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Legaue legaue = new Legaue();
                var stadiumIds = DB.Stadium
                .Where(r => r.Stad_Name == StadiumNameCmb.SelectedItem.ToString())
                 .Select(r => r.StadiumID).FirstOrDefault();
                legaue.Legue_Name = legaueNametxt.Text;
                legaue.BeginDate = BeginDate.Value;
                legaue.EndDate = EndDate.Value;
                legaue.EndReg = EndRegistration.Value;
                legaue.StadiumName = StadiumNameCmb.Text;
                legaue.StadiumID = stadiumIds;
                legaue.City = CityCmb.SelectedItem.ToString();
                legaue.Reward = Rewardtxt.Text;
                legaue.TimePlay = TimeSpan.Parse(TimePlayDate.Text);
                legaue.NumberOfSubscribersInlegaue = int.Parse(SubNumUpDown.Value.ToString());
                legaue.CurrentSubscriberslegaue = 0;

                DB.Legaues.Add(legaue);
                LeagueMessage Add_Message = new LeagueMessage
                {
                    leagueID = legaue.LegueID,  //CurrentUserLogin.UserLogginID;
                    MessageContent = $"In Stadium {legaue.StadiumName} League {legaue.Legue_Name} Start in" +
                    $" Date {legaue.BeginDate.ToString("yyyy-MM-dd")} and End on {legaue.EndDate.ToString("yyyy-MM-dd")} Final Regestration in {legaue.EndReg.ToString("yyyy-MM-dd")} ",
                    DeleteTimestamp = legaue.EndReg,
                };
                DB.LeagueMessages.Add(Add_Message);

                DB.SaveChanges();
                MessageBox.Show("Done");
                PopulateDataGridview();
            }


        }



        private void UpdateLeague(int LeagueID)
        {
            if (ValidateInput(leagueID))
            {
                var leagueToUpdate = DB.Legaues.SingleOrDefault(x => x.LegueID == LeagueID);


                // Update league properties
                leagueToUpdate.Legue_Name = legaueNametxt.Text;
                leagueToUpdate.BeginDate = BeginDate.Value;
                leagueToUpdate.EndDate = EndDate.Value;
                leagueToUpdate.EndReg = EndRegistration.Value;
                leagueToUpdate.StadiumName = StadiumNameCmb.Text;
                leagueToUpdate.City = CityCmb.SelectedItem.ToString();
                leagueToUpdate.Reward = Rewardtxt.Text;
                leagueToUpdate.TimePlay = TimeSpan.Parse(TimePlayDate.Text);

                DB.SaveChanges();

                MessageBox.Show("League updated successfully");
                PopulateDataGridview();
            }
        }




        private void Updatebtn_Click(object sender, EventArgs e)
             => UpdateLeague(leagueID);




        private void SearchLegaueNametxt_TextChanged(object sender, EventArgs e)
        {
            string searchText = SearchLegaueNametxt.Text;
            dataGridView1.DataSource = DB.Legaues
                .Where(l => l.Legue_Name.Contains(searchText))
                .ToList();
        }

        private void SearchstadiumNametxt_TextChanged(object sender, EventArgs e)
        {
            string searchText = SearchstadiumNametxt.Text;
            dataGridView1.DataSource = DB.Legaues
                .Where(l => l.StadiumName.Contains(searchText))
                .ToList();

        }


        private void PopulateControlsFromSelectedRow()
        {
            if (dataGridView1.CurrentRow != null)
            {
                id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                var result = DB.Legaues.SingleOrDefault(x => x.LegueID == id);
                if (result != null)
                {
                    legaueNametxt.Text = result.Legue_Name;
                    BeginDate.Value = result.BeginDate;
                    EndDate.Value = result.EndDate;
                    EndRegistration.Value = result.EndReg;
                    StadiumNameCmb.Text = result.StadiumName;
                    //legaue.StadiumID = stadiumIds;
                    CityCmb.SelectedItem = result.City;
                    Rewardtxt.Text = result.Reward;
                    TimePlayDate.Text = result.TimePlay.ToString("hh\\:mm");

                }
            }
        }



        private void Deletebtn_Click(object sender, EventArgs e)
        {
            try
            {
                var result = DB.Legaues.Find(id);

                if (result != null)
                {
                    DB.Legaues.Remove(result);
                    DB.SaveChanges();
                    MessageBox.Show("Delete Done");
                    PopulateDataGridview();
                }
                else
                {
                    MessageBox.Show("No such entity exists to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int SelectLegueID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            leagueID = SelectLegueID;
            PopulateControlsFromSelectedRow();

        }


        private void PopulateDataGridview()
        {
            dataGridView1.DataSource = null; // Clear the current data source
            var specificColumnsData = DB.Legaues
                .Select(l => new
                {
                    l.LegueID,
                    l.Legue_Name,
                    l.BeginDate,
                    l.EndDate,
                    l.StadiumName,
                    l.EndReg,
                    l.Reward,
                    l.City,
                    l.TimePlay,
                    l.NumberOfSubscribersInlegaue,
                    l.CurrentSubscriberslegaue
                }).ToList();

            dataGridView1.DataSource = specificColumnsData;
        }

        private void CityCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCity = CityCmb.SelectedItem?.ToString();
            if (selectedCity != null)
            {
                var stadiums = DB.Stadium
                    .Where(s => s.Area == selectedCity && s.Stad_Status == "Active")
                    .Select(s => s.Stad_Name)
                    .ToArray();
                StadiumNameCmb.Items.Clear();
                StadiumNameCmb.Items.AddRange(stadiums);
            }
        }
    }
}
