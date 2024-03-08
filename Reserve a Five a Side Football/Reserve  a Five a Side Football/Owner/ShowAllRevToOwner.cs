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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Reserve__a_Five_a_Side_Football.Owner
{
    public partial class ShowAllRevToOwner : BaseForm
    {
        Reserve_a_Five_a_SideEntities dbContext;
        public ShowAllRevToOwner()
        {
            dbContext = new Reserve_a_Five_a_SideEntities();
            InitializeComponent();
            LoadUniqueAreas();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
        }

        private void LoadUniqueAreas()
        {
            var uniqueAreas = dbContext.Stadium.Select(s => s.Area).Distinct().ToArray();
            CityComboBox1.Items.AddRange(uniqueAreas);
        }

        private void CityComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCity = CityComboBox1.SelectedItem?.ToString();
            if (selectedCity != null)
            {
                var stadiums = dbContext.Stadium
                    .Where(s => s.Area == selectedCity && s.Stad_Status == "Active")
                    .Select(s => s.Stad_Name)
                    .ToArray();
                StadiunComboBox1.Items.Clear();
                StadiunComboBox1.Items.AddRange(stadiums);
            }
        }

        private void ShowSelectedbtn_Click(object sender, EventArgs e)
        {
            guna2DataGridView1.Rows.Clear();

            var fromDate = FromDateTimePicker.Value.Date;
            var ToDate = ToDateTimePicker.Value.Date;

            var city = CityComboBox1?.SelectedItem?.ToString();
            var stadiumName = StadiunComboBox1?.SelectedItem?.ToString();


            int? stadiumId = null;
            if (!string.IsNullOrEmpty(stadiumName))
            {
                stadiumId = dbContext.Stadium
                    .Where(s => s.Stad_Name == stadiumName)
                    .Select(s => (int?)s.StadiumID)
                    .FirstOrDefault();
            }

            var selectedReservations = dbContext.Reservations
         .Where(s =>
             (stadiumId == null || s.StadiumID == stadiumId) && // Filter by stadium if Not Null
             (string.IsNullOrEmpty(city) || s.Stadium.Area == city) && // Filter by City if Not Null
             s.Reservation_Date >= fromDate && s.Reservation_Date <= ToDate)
         .Select(s => new
         {
             s.Player.User.NationalID,
             FullName = s.Player.User.FName + " " + s.Player.User.LName,
             s.Stadium.Area,
             s.Stadium.Stad_Name,
             s.Reservation_Date,
             s.Reservation_Time,
             s.Payment,
             s.Reservation_Statues,
         }).ToList();


            foreach (var Res in selectedReservations)
            {
                guna2DataGridView1.Rows.Add(
                    Res.NationalID,
                    Res.FullName,
                    Res.Area,
                    Res.Stad_Name,
                    Res.Reservation_Date.Value.Date.ToString("yyyy-MM-dd"),
                    Res.Reservation_Time,
                    Res.Payment,
                    Res.Reservation_Statues);
            }

        }

       
    }
}
