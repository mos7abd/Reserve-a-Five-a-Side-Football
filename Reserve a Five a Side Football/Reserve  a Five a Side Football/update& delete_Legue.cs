using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Reserve__a_Five_a_Side_Football
{

    public partial class update__delete_Legue : Form
    {

        private readonly Reserve_a_Five_a_SideEntities context = new Reserve_a_Five_a_SideEntities();
        Legaue legaue=new Legaue(); 
        public update__delete_Legue()
        {
            InitializeComponent();
            show();
        }
        private void show()
        {
            var query = (from r in context.Legaues
                     select new
                     {
                         r.LegueID,
                         r.Legue_Name,
                         r.BeginDate,
                         r.EndDate,
                         r.EndReg,
                         r.StadiumName,
                         r.City,
                         r.TimePlay,
                     }).ToList();
            dataGridView1.DataSource = query;
        }

        private void delete_Btn_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {

                    int legueId = int.Parse(dataGridView1.Rows[0].Cells["LegueID"].Value.ToString());


                    var que = context.Legaues.FirstOrDefault(r => r.LegueID == legueId);


                    context.Legaues.Remove(que);
                    context.SaveChanges();
                    show();

                }

            }
        }

        private void update_Btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int leagueId = int.Parse(row.Cells["LegueID"].Value.ToString());
                    var league = context.Legaues.FirstOrDefault(r => r.LegueID == leagueId);

                    if (league != null)
                    {
                        DateTime beginDate;
                        if (DateTime.TryParse(begindate.Text, out beginDate))
                        {
                            league.BeginDate = beginDate;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Begin Date format");
                            return;
                        }

                        DateTime endDate;
                        if (DateTime.TryParse(enddate.Text, out endDate))
                        {
                            league.EndDate = endDate;
                        }
                        else
                        {
                            MessageBox.Show("Invalid End Date format");
                            return;
                        }

                        DateTime endRegDate;
                        if (DateTime.TryParse(endrange.Text, out endRegDate))
                        {
                            league.EndReg = endRegDate;
                        }
                        else
                        {
                            MessageBox.Show("Invalid End Registration Date format");
                            return;
                        }

                        DateTime timePlay;
                        if (DateTime.TryParse(timeplay.Text, out timePlay))
                        {
                            league.TimePlay = timePlay.TimeOfDay;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Time format");
                            return;
                        }

                        league.StadiumName = stadiunname.Text;
                    }
                }

                context.SaveChanges();
                show();

                // Reset text boxes outside the loop
                begindate.Text = "";
                enddate.Text = "";
                endrange.Text = "";
                stadiunname.Text = "";
                timeplay.Text = "";
            }
        }

    }
}
