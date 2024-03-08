using ReservationPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class HomeScreen : Form
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void LeagueButton_Click(object sender, EventArgs e)
        {
            var League = new LeagueForm();
            League.Show();
        }

        private void StadiumButton_Click(object sender, EventArgs e)
        {
/*            var Stadium = new StadiumForm();
            Stadium.Show();*/
        }

        private void ReservationsButton_Click(object sender, EventArgs e)
        {
            var Reservation = new ReservationForm();
            Reservation.Show();
        }
    }
}
