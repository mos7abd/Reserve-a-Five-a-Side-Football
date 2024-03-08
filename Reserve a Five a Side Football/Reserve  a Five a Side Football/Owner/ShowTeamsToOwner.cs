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

namespace Reserve__a_Five_a_Side_Football
{
    public partial class ShowTeamsToOwner : BaseForm
    {
        Reserve_a_Five_a_SideEntities context;
        public ShowTeamsToOwner()
        {
            context = new Reserve_a_Five_a_SideEntities();
            InitializeComponent();
            label1.Visible = false;
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
        }

        private void ShowTeamsBtn_Click(object sender, EventArgs e)
        {
            guna2DataGridView1.Rows.Clear();
            var query = context.Database.SqlQuery<TeamData>("EXEC GetTeamsData").ToList();

            foreach (var item in query)
                guna2DataGridView1.Rows.Add(item.Captain_Name, item.TeamName,
                                            item.Player1_Name, item.Player2_Name,
                                            item.Player3_Name, item.Player4_Name,
                                            item.Player5_Name, item.LeagueName);

        }

        #region         // Class To take Data from Proc To GridView

        public class TeamData
        {
            public int TeamID { get; set; }
            public string Captain_Name { get; set; }
            public string TeamName { get; set; }
            public string Player1_Name { get; set; }
            public string Player2_Name { get; set; }
            public string Player3_Name { get; set; }
            public string Player4_Name { get; set; }
            public string Player5_Name { get; set; }
            public string LeagueName { get; set; }
        }

        #endregion



        // Get Team By ID
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2DataGridView1.Rows.Clear();
            string ValueTxtBox = guna2TextBox1.Text;

            if (string.IsNullOrEmpty(ValueTxtBox))
            {
                label1.Visible = true;
                label1.Text = "Please Enter National ID";
                return;
            }
            int getCaptinid = context.Players
                .Where(c => c.User.NationalID == guna2TextBox1.Text)
                .Select(ec=>ec.Player_ID).FirstOrDefault();

            if (!context.Teams.Any(team => team.CaptainID == getCaptinid))
            {
                label1.Visible = true;
                label1.Text = "Invalid Data Please Enter a Correct National ID.";
            }
            else
            {
                label1.Text = "";
                //var TeamValue = context.Teams
                var TeamValue = from team in context.Teams
                                .Where(es => es.CaptainID == getCaptinid)
                                select new
                                {
                                    Captain_Name = context.Users.FirstOrDefault(u => u.UserID == team.CaptainID).FName + " " + context.Users.FirstOrDefault(u => u.UserID == team.CaptainID).LName,
                                    teamName = team.TeamName,
                                    Player1_Name = context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player1).FName + " " + context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player1).LName,
                                    Player2_Name = context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player2).FName + " " + context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player2).LName,
                                    Player3_Name = context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player3).FName + " " + context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player3).LName,
                                    Player4_Name = context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player4).FName + " " + context.Users.FirstOrDefault(u => u.NationalID == team.NationalID_Player4).LName,
                                    LeagueName = context.Legaues.FirstOrDefault(u => u.LegueID == team.LegueID)
                                };

                foreach (var item in TeamValue)
                    guna2DataGridView1.Rows.Add(item.Captain_Name, item.teamName, item.Player1_Name, item.Player2_Name, item.Player3_Name, item.Player4_Name, item.LeagueName.Legue_Name);

            }

        }
    }
}




