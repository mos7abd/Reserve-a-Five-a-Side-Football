using RegertrationPage;
using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class AddYourTeam : BaseForm
    {
        Reserve_a_Five_a_SideEntities DB = new Reserve_a_Five_a_SideEntities();

        public AddYourTeam()
        {
            InitializeComponent();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
        }


        private void AddYourTeam_Load(object sender, EventArgs e)
        {
            var StadiumName = DB.Legaues.Select(et => et.Legue_Name).ToList();
            foreach (var item in StadiumName)
                LeagueNameCmb.Items.Add(item);

        }



        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (ValidateInput())
        //    {
        //        Team team = new Team();
        //        var league_Id = DB.Legaues
        //        .Where(r => r.Legue_Name == LeagueNameCmb.SelectedItem.ToString())
        //          .Select(r => r.LegueID).FirstOrDefault();
        //        var playerID = DB.Players
        //        .Where(p => p.User.NationalID == CaptinIdtxt.Text)
        //        .Select(p => p.Player_ID)
        //        .FirstOrDefault();
        //        team.TeamName = TeamNametxt.Text;
        //        team.CaptainID = playerID;
        //        team.NationalID_Player1 = NationalIDplayer1txt.Text;
        //        team.NationalID_Player2 = NationalIDplayer2txt.Text;
        //        team.NationalID_Player3 = NationalIDplayer3txt.Text;
        //        team.NationalID_Player4 = NationalIDplayer4txt.Text;
        //        team.NationalID_Player5 = NationalIDplayer5txt.Text;
        //        team.LegueID = league_Id;

        //        var leagueMaxSubscribe = DB.Legaues.FirstOrDefault(l => l.LegueID == league_Id);
        //        if (leagueMaxSubscribe.NumberOfSubscribersInlegaue.Value <= leagueMaxSubscribe.CurrentSubscriberslegaue)
        //        {
        //            MessageBox.Show("Maximum subscriber count reached for this league.");
        //            return;
        //        }
        //        else
        //        {
        //            leagueMaxSubscribe.CurrentSubscriberslegaue++;

        //            DB.Teams.Add(team);
        //            DB.SaveChanges();
        //            MessageBox.Show("Register Completed");
        //            this.Close();
        //        }

        //    }


        //}


        private bool ValidateInput()
        {

            Team team = new Team();
            if (string.IsNullOrWhiteSpace(TeamNametxt.Text) ||
                string.IsNullOrWhiteSpace(CaptinIdtxt.Text) ||
                string.IsNullOrWhiteSpace(NationalIDplayer1txt.Text) ||
                string.IsNullOrWhiteSpace(NationalIDplayer2txt.Text) ||
                string.IsNullOrWhiteSpace(NationalIDplayer3txt.Text) ||
                string.IsNullOrWhiteSpace(NationalIDplayer4txt.Text)

                )
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            var nationalIDs = new HashSet<string>
                             {

                                CaptinIdtxt.Text,
                                NationalIDplayer1txt.Text,
                                NationalIDplayer2txt.Text,
                                NationalIDplayer3txt.Text,
                                NationalIDplayer4txt.Text,
                                NationalIDplayer5txt.Text
                            };

            if (nationalIDs.Count != 6)
            {
                MessageBox.Show("National IDs for players must be unique.");
                return false;
            }

            if (!CheckNationalIDExistence(NationalIDplayer1txt.Text))
            {
                MessageBox.Show("National ID for Player 1 is not registered.");
                button2.Visible = true;
                return false;
            }
            if (!CheckNationalIDExistence(NationalIDplayer2txt.Text))
            {
                MessageBox.Show("National ID for Player 2 is not registered.");
                button2.Visible = true;
                return false;
            }
            if (!CheckNationalIDExistence(NationalIDplayer3txt.Text))
            {
                MessageBox.Show("National ID for Player 3 is not registered.");
                button2.Visible = true;
                return false;
            }
            if (!CheckNationalIDExistence(NationalIDplayer4txt.Text))
            {
                MessageBox.Show("National ID for Player 4 is not registered.");
                button2.Visible = true;
                return false;
            }
            if (!CheckNationalIDExistence(NationalIDplayer5txt.Text))
            {
                MessageBox.Show("National ID for Player 5 is not registered.");
                button2.Visible = true;
                return false;
            }

            return true;

        }
        private bool CheckNationalIDExistence(string nationalID)
        {
            var existingUser = DB.Users.FirstOrDefault(u => u.NationalID == nationalID);
            return existingUser != null;
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    Regest regest = new Regest();
        //    regest.ShowDialog();
        //}

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Team team = new Team();
                var league_Id = DB.Legaues
                .Where(r => r.Legue_Name == LeagueNameCmb.SelectedItem.ToString())
                  .Select(r => r.LegueID).FirstOrDefault();
                var playerID = DB.Players
                .Where(p => p.User.NationalID == CaptinIdtxt.Text)
                .Select(p => p.Player_ID)
                .FirstOrDefault();
                team.TeamName = TeamNametxt.Text;
                team.CaptainID = playerID;
                team.NationalID_Player1 = NationalIDplayer1txt.Text;
                team.NationalID_Player2 = NationalIDplayer2txt.Text;
                team.NationalID_Player3 = NationalIDplayer3txt.Text;
                team.NationalID_Player4 = NationalIDplayer4txt.Text;
                team.NationalID_Player5 = NationalIDplayer5txt.Text;
                team.LegueID = league_Id;

                var leagueMaxSubscribe = DB.Legaues.FirstOrDefault(l => l.LegueID == league_Id);
                if (leagueMaxSubscribe.NumberOfSubscribersInlegaue.Value <= leagueMaxSubscribe.CurrentSubscriberslegaue)
                {
                    MessageBox.Show("Maximum subscriber count reached for this league.");
                    return;
                }
                else
                {
                    leagueMaxSubscribe.CurrentSubscriberslegaue++;

                    DB.Teams.Add(team);
                    DB.SaveChanges();
                    MessageBox.Show("Register Completed");
                    this.Close();
                }

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Regest regest = new Regest();
            regest.ShowDialog();
        }
    }
}
