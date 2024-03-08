using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Reserve__a_Five_a_Side_Football.ShowTeamsToOwner;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class TeamInformation : BaseForm
    {
        Reserve_a_Five_a_SideEntities context;

        public TeamInformation()
        {
            context = new Reserve_a_Five_a_SideEntities();
            InitializeComponent();
            GetTeamsForUser();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
        }

        public void GetTeamsForUser()
        {
            int idloginuser = CurrentUserLogin.UserLogginID;
            var test1 = context.Teams.Where(e => e.CaptainID == e.Player.Player_ID && e.Player.User.UserID == idloginuser)
                .Select(e => e.CaptainID).FirstOrDefault();


            var playerID = context.Users
                 .Where(p => p.UserID == idloginuser)
                 .Select(p => p.NationalID)
                 .FirstOrDefault();

            if (test1 == idloginuser)
            {
                var query1 = context.Database.SqlQuery<TeamData>($"Exec Team_DetailsForUserCaptin {test1}").ToList();
                foreach (var item in query1)
                {
                    guna2DataGridView1.Rows.Add(item.TeamID, item.TeamName, item.Captain_Name,
                         item.Player1_Name, item.Player2_Name,
                         item.Player3_Name, item.Player4_Name, item.Player5_Name, item.LeagueName);
                }
            }
            else
            {
                var query = context.Database.SqlQuery<TeamData>($"Exec Team_DetailsForUser {playerID}").ToList();
                foreach (var item in query)
                {
                    guna2DataGridView1.Rows.Add(item.TeamID, item.TeamName, item.Captain_Name,
                         item.Player1_Name, item.Player2_Name,
                         item.Player3_Name, item.Player4_Name, item.Player5_Name, item.LeagueName);
                }

            }


        }

        private void guna2DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                if (selectedRow != null && selectedRow.Cells.Count > 0 && !selectedRow.Cells.Cast<DataGridViewCell>().Any(cell => cell.Value == null || cell.Value == DBNull.Value || string.IsNullOrWhiteSpace(cell.Value.ToString())))
                {

                    string player1Name = selectedRow.Cells["Player1"].Value.ToString();
                    string player2Name = selectedRow.Cells["Player2"].Value.ToString();
                    string player3Name = selectedRow.Cells["Player3"].Value.ToString();
                    string player4Name = selectedRow.Cells["Player4"].Value.ToString();
                    string player5Name = selectedRow.Cells["Player5"].Value.ToString();


                    var Teamid = selectedRow.Cells["TeamID"].Value.ToString();

                    var league = context.Teams.FirstOrDefault(l => l.LegueID.ToString() == l.Legaue.LegueID.ToString());
                    var hoursDifference = (league.Legaue.BeginDate - DateTime.Now).TotalHours;
                    if (hoursDifference <= 72)
                    {
                        MessageBox.Show("The league is starting within the next 72 hours Cant Updated", "League Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var GetIDsforTeam = context.Teams.FirstOrDefault(t => t.TeamID.ToString() == Teamid);

                    Player1TextBox.Text = GetIDsforTeam.NationalID_Player1;
                    Player2TextBox.Text = GetIDsforTeam.NationalID_Player2;
                    Player3TextBox.Text = GetIDsforTeam.NationalID_Player3;
                    Player4TextBox.Text = GetIDsforTeam.NationalID_Player4;
                    Player5TextBox.Text = GetIDsforTeam.NationalID_Player5;
                }
                else
                    MessageBox.Show("No Teams To Select ");

            }
            else
                MessageBox.Show("Selected row is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            int idloginuser = CurrentUserLogin.UserLogginID;

            var captinid = context.Teams
                .Where(a => a.CaptainID == idloginuser).Select(a => a.CaptainID).FirstOrDefault();

            try
            {
                if (idloginuser == captinid)
                {
                    int teamId = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["TeamID"].Value);
                    var teamToUpdate = context.Teams.FirstOrDefault(t => t.TeamID == teamId);

                    // Validate each National ID
                    if (!IsValidNationalID(Player1TextBox.Text) ||
                        !IsValidNationalID(Player2TextBox.Text) ||
                        !IsValidNationalID(Player3TextBox.Text) ||
                        !IsValidNationalID(Player4TextBox.Text) ||
                        !IsValidNationalID(Player5TextBox.Text))
                    {
                        MessageBox.Show("Invalid National ID format. Please enter a valid 14-digit National ID for each player.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Check if each National ID exists in the database
                    if (!NationalIDExists(Player1TextBox.Text) ||
                        !NationalIDExists(Player2TextBox.Text) ||
                        !NationalIDExists(Player3TextBox.Text) ||
                        !NationalIDExists(Player4TextBox.Text) ||
                        !NationalIDExists(Player5TextBox.Text))
                    {
                        MessageBox.Show("One or more National IDs do not exist in the database. Please Enter valid National IDs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    HashSet<string> nationalIDs = new HashSet<string>();
                    nationalIDs.Add(Player1TextBox.Text);
                    nationalIDs.Add(Player2TextBox.Text);
                    nationalIDs.Add(Player3TextBox.Text);
                    nationalIDs.Add(Player4TextBox.Text);
                    nationalIDs.Add(Player5TextBox.Text);

                    if (nationalIDs.Count != 5)
                    {
                        MessageBox.Show("Duplicate National IDs found. Each player must have a unique National ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update player IDs in the database
                    teamToUpdate.NationalID_Player1 = Player1TextBox.Text;
                    teamToUpdate.NationalID_Player2 = Player2TextBox.Text;
                    teamToUpdate.NationalID_Player3 = Player3TextBox.Text;
                    teamToUpdate.NationalID_Player4 = Player4TextBox.Text;
                    teamToUpdate.NationalID_Player5 = Player5TextBox.Text;

                    context.SaveChanges();
                    MessageBox.Show("Team updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Player1TextBox.Text = "";
                    Player2TextBox.Text = "";
                    Player3TextBox.Text = "";
                    Player4TextBox.Text = "";
                    Player5TextBox.Text = "";

                    guna2DataGridView1.Rows.Clear();
                    GetTeamsForUser();


                }
                else
                {
                    MessageBox.Show("Cannot to Update", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the team: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        // Check if the input  14 characters 
        private bool IsValidNationalID(string nationalID)
             => !string.IsNullOrEmpty(nationalID) && nationalID.Length == 14 && nationalID.All(char.IsDigit);


        // Check National ID exists in database
        private bool NationalIDExists(string nationalID)
             => context.Users.Any(u => u.NationalID == nationalID);



        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            int idloginuser = CurrentUserLogin.UserLogginID;

            var captinid = context.Teams
                .Where(a => a.CaptainID == idloginuser).Select(a => a.CaptainID).FirstOrDefault();


            if (idloginuser == captinid)
            {


                foreach (DataGridViewRow row in guna2DataGridView1.SelectedRows)
                {
                    if (row.Cells["LeagueName"].Value != null)
                    {
                        string displayedLeagueName = row.Cells["LeagueName"].Value.ToString();

                        var league = context.Teams.FirstOrDefault(l => l.Legaue.Legue_Name == displayedLeagueName);

                        if (league != null)
                        {
                            var hoursDifference = (league.Legaue.BeginDate - DateTime.Now).TotalHours;
                            if (hoursDifference > 72)
                            {
                                guna2DataGridView1.Rows.Remove(row);
                                context.Teams.Remove(league);
                                context.SaveChanges();
                            }
                            else
                                MessageBox.Show("Cannot Delete Team. The league is starting within the next 72 hours.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        MessageBox.Show("No Teams To Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Cannot to Delete", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            


        }
    }
}
