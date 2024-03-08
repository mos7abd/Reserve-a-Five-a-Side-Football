using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Reserve__a_Five_a_Side_Football
{
    public partial class Player_Data : BaseForm
    {
        private readonly Reserve_a_Five_a_SideEntities context;

        public Player_Data()
        {
            InitializeComponent();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
            context = new Reserve_a_Five_a_SideEntities();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string userIDStr = textBox1.Text;

            if (userIDStr.Length != 14)
            {
                label3.Visible = true;
                label3.Text = "ID must contain 14 digits";

            }

            var query = (from r in context.Users
                         join s in context.Reservations on r.UserID equals s.Player_ID
                         join t in context.Stadium on s.StadiumID equals t.StadiumID
                         where (r.NationalID == userIDStr)
                         select new
                         {
                             Name = r.FName + " " + r.LName,
                             Status = r.AccountStatus,
                             mail = r.Email,
                             Reserv_Date = s.Reservation_Date,
                             time =s.Reservation_Time,
                             stadname = t.Stad_Name

                         }).FirstOrDefault();

            dataGridView1.Rows.Clear();

            if (query != null)
                dataGridView1.Rows.Add(query.Name, query.mail,query.Status,query.Reserv_Date,query.time,query.stadname);
            
            else
                MessageBox.Show("No data exists for the entered user ID.");
            
        }

    }
}