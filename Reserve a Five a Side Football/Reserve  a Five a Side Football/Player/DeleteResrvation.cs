using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class DeleteResrvation : BaseForm
    {
        private readonly Reserve_a_Five_a_SideEntities context1 = new Reserve_a_Five_a_SideEntities();
        Reservation reservation = new Reservation();
        public DeleteResrvation()
        {
            InitializeComponent();
            show();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements

        }

        private void button1_Click(object sender, EventArgs e)
        {

            /* if (dataGridView1.SelectedRows.Count > 0)
             {
                 foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                 {

                     int reservationId = int.Parse(dataGridView1.Rows[0].Cells["ReservationID"].Value.ToString());


                     var q = context1.Reservations.FirstOrDefault(r => r.ReservationID == reservationId);


                     context1.Reservations.Remove(q);
                     context1.SaveChanges();
                     show();

                 }

             }*/
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int reservationId = Convert.ToInt32(row.Cells["RevID"].Value);
                    var reservationToDelete = context1.Reservations.FirstOrDefault(r => r.ReservationID == reservationId);

                    if (reservationToDelete != null)
                    {

                        DateTime currentDateTime = DateTime.UtcNow.AddHours(2);



                        DateTime minReservationDateTime = currentDateTime.AddHours(48);

                        if (reservationToDelete.Reservation_Date >= minReservationDateTime)
                        {
                            context1.Reservations.Remove(reservationToDelete);
                            context1.SaveChanges();
                            show();
                        }
                        else
                        {
                            MessageBox.Show("Can Not Delete This Rev Must Be More than 48 h");
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("select any row");
            }



        }

        private void show()
        {
            dataGridView1.Rows.Clear();
            int userLoginID = CurrentUserLogin.UserLogginID;


            var reservations = (from r in context1.Reservations
                                join p in context1.Players on r.Player_ID equals p.Player_ID
                                join u in context1.Users on p.UserID equals u.UserID
                                join res in context1.Stadium on r.StadiumID equals res.StadiumID
                                where u.UserID == userLoginID
                                select new
                                {

                                    r.ReservationID,
                                    res.Stad_Name,
                                    r.Reservation_Date,
                                    r.Reservation_Time,
                                    r.Payment,
                                    r.Reservation_Statues,
                                }).ToList();


            //dataGridView1.DataSource = reservations;
            foreach (var reser in reservations)
                dataGridView1.Rows.Add(reser.ReservationID, reser.Reservation_Date.Value.Date.ToString("yyyy-MM-dd"), reser.Reservation_Time, reser.Stad_Name, reser.Payment, reser.Reservation_Statues);


        }


    }
}














