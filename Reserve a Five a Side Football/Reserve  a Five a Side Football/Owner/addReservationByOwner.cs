using Reserve__a_Five_a_Side_Football;
using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReservationPage
{
    public partial class addReservationByOwner : BaseForm
    {
        private Reserve_a_Five_a_SideEntities dbContext;

        public addReservationByOwner()
        {
            InitializeComponent();
            InitializeForm();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
        }

        private void InitializeForm()
        {
            datealarm.Visible = false;
            stadalarm.Visible = false;
            payalarm.Visible = false;
            dbContext = new Reserve_a_Five_a_SideEntities();
            LoadUniqueAreas();
        }

        private void LoadUniqueAreas()
        {
            var uniqueAreas = dbContext.Stadium.Select(s => s.Area).Distinct().ToArray();
            CityCompoBox.Items.AddRange(uniqueAreas);
        }

        private void Reserve_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close?", "Close Form",
                              MessageBoxButtons.YesNo,
                              MessageBoxIcon.Question);
            e.Cancel = (result == DialogResult.No);
        }

        private void CityCompoBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCity = CityCompoBox.SelectedItem?.ToString();
            if (selectedCity != null)
            {
                var stadiums = dbContext.Stadium
                    .Where(s => s.Area == selectedCity && s.Stad_Status == "Active")
                    .Select(s => s.Stad_Name)
                    .ToArray();
                stadbx.Items.Clear();
                stadbx.Items.AddRange(stadiums);
            }
        }

        private void stadbx_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (stadbx.SelectedIndex != -1 && datebx.Value != null)
            {
                var selectedStadium = stadbx.SelectedItem?.ToString();
                var selectedDate = datebx.Value.Date;

                var availableTimeslots = GetAvailableTimeslots(selectedStadium, selectedDate);
                PopulateTimeSlots(availableTimeslots);
            }
        }

        private List<TimeSpan> GetAvailableTimeslots(string selectedStadium, DateTime selectedDate)
        {
            var stadiumId = dbContext.Stadium
                .Where(s => s.Stad_Name == selectedStadium)
                .Select(s => s.StadiumID)
                .FirstOrDefault();

            var reservedTimeslots = dbContext.Reservations
                .Where(r => r.StadiumID == stadiumId && r.Reservation_Date == selectedDate.Date)
                .Select(r => r.Reservation_Time)
                .ToList();

            var allTimeslots = Enumerable.Range(0, 24)
                .Select(hour => new TimeSpan(hour, 0, 0))
                .ToList();

            var availableTimeslots = new List<TimeSpan>();
            foreach (var timeslot in allTimeslots)
            {
                if (!reservedTimeslots.Contains(timeslot))
                    availableTimeslots.Add(timeslot);
            }
            return availableTimeslots;
        }

        private void PopulateTimeSlots(List<TimeSpan> availableTimeslots)
        {
            timeComboBox.Items.Clear();
            foreach (var timeSlot in availableTimeslots)
            {
                timeComboBox.Items.Add(timeSlot.ToString("hh\\:mm"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
            {
                MessageBox.Show("Invalid Data", "Confirm Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var stadiumName = stadbx.SelectedItem?.ToString();
            var selectedDate = datebx.Value.Date;
            var selectedTime = TimeSpan.Parse(timeComboBox.SelectedItem.ToString());
            var payment = paybx.SelectedItem.ToString();

            var stadiumId = dbContext.Stadium
                .Where(s => s.Stad_Name == stadiumName)
                .Select(s => s.StadiumID)
                .FirstOrDefault();


            if (NationalTextBox1.Text.Length != 14)
            {
                NationalLbl.Text = "National ID must be 14 digits long.";
                return;
            }


            var nationalID = NationalTextBox1.Text;
            var player = dbContext.Players.FirstOrDefault(p => p.User.NationalID == nationalID);
            if (player == null || player.User.AccountStatus == false)
            {
                NationalLbl.Text = "Invalid National ID";
                return;
            }

            var newReservation = new Reservation
            {
                Reservation_Date = selectedDate,
                Reservation_Time = selectedTime,
                Payment = payment,
                Player_ID = player.Player_ID,
                OwnarID = CurrentUserLogin.UserLogginID,
                StadiumID = stadiumId,
                Reservation_Statues = "Confirmed"
            };
            try
            {
                dbContext.Reservations.Add(newReservation);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
            }
            

            MessageBox.Show("Success Confirm", "Confirm Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetForm();
        }


        private bool ValidateFormData()
        {
            if (datebx.Value < DateTime.Now || stadbx.SelectedIndex == -1 || paybx.SelectedIndex == -1 || timeComboBox.SelectedIndex == -1)
            {
                ShowValidationErrors();
                return false;
            }
            return true;
        }

        private void ShowValidationErrors()
        {
            datealarm.Visible = datebx.Value < DateTime.Now;
            stadalarm.Visible = stadbx.SelectedIndex == -1;
            payalarm.Visible = paybx.SelectedIndex == -1;
            lblcCity.Visible = CityCompoBox.SelectedIndex == -1;
            Timelbl.Visible = timeComboBox.SelectedIndex == -1;
        }

        private void ResetForm()
        {
            datealarm.Visible = false;
            stadalarm.Visible = false;
            payalarm.Visible = false;
            stadbx.SelectedIndex = -1;
            paybx.SelectedIndex = -1;
            timeComboBox.Items.Clear();
        }

        private void datebx_ValueChanged(object sender, EventArgs e)
              => timeComboBox.Items.Clear();

        private void addReservationByOwner_Load(object sender, EventArgs e)
        {

        }
    }
}
