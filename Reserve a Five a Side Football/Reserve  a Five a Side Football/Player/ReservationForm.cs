using Reserve__a_Five_a_Side_Football;
using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ReservationPage
{
    public partial class ReservationForm : BaseForm
    {
        private Reserve_a_Five_a_SideEntities dbContext;

        public ReservationForm()
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

            decimal? PaymentAmount = dbContext.Stadium
                .Where(ea => ea.StadiumID == stadiumId)
                .Select(ea => ea.Hourly_Price).Single();

            var GetPlayerID = dbContext.Players
                .Where(es => es.UserID == CurrentUserLogin.UserLogginID)
                .Select(es => es.Player_ID).FirstOrDefault();

            var newReservation = new Reservation
            {
                Reservation_Date = selectedDate,
                Reservation_Time = selectedTime,
                Player_ID = GetPlayerID,
                Payment = payment,
                StadiumID = stadiumId
            };

            if (payment == "Credit Card")
            {
                var creditCardPaymentForm = new PaymentWayByCreditCard(PaymentAmount);
                var result = creditCardPaymentForm.ShowDialog();
                newReservation.Reservation_Statues = "Confirmed";
                if (result != DialogResult.OK) return;
            }
            else if (payment == "Cash Wallet")
            {
                var cashPaymentForm = new PaymentByWallet(PaymentAmount);
                var result = cashPaymentForm.ShowDialog();
                newReservation.Reservation_Statues = "Confirmed";
                if (result != DialogResult.OK) return;
            }
            else
            {
                newReservation.Reservation_Statues = "Pending";
            }

            ReservationMessage Add_Message = new ReservationMessage
            {
                PlayerID = GetPlayerID,
                MessageContent = $"Your Reservation Confirmed in Stadium {stadiumName}" +
                $" in Date {newReservation.Reservation_Date?.ToString("yyyy-MM-dd")} on {newReservation.Reservation_Time} Clock Don't Late ^_^ ",
                IsRead = false
            };


            dbContext.ReservationMessages.Add(Add_Message);
            dbContext.Reservations.Add(newReservation);
            dbContext.SaveChanges();

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


    }
}

