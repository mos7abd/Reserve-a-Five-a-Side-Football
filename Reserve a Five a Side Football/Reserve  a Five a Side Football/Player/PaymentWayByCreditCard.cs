using System;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class PaymentWayByCreditCard : Form
    {
        private decimal? totalPaymentAmount;

        public PaymentWayByCreditCard(decimal? totalPaymentAmount)
        {
            InitializeComponent();
            this.totalPaymentAmount = totalPaymentAmount;
            Paymentlbl.Text = this.totalPaymentAmount.ToString() + " EGP";
        }

        private void confirmbtn_Click(object sender, EventArgs e)
        {
            if (ValidatePaymentData())
            {
                if (ProcessPayment())
                {
                    MessageBox.Show("Payment Successful. Reservation Confirmed", "Payment Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show("Payment Failed. Please try again.", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Invalid Data", "Confirm Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private bool ValidatePaymentData()
        {
            if (cardnum.TextLength != 16 || name.TextLength < 20
                || date.TextLength != 5 || cvc.TextLength != 3) // date.TextLength Must Be 1-12/ 30
                return false;

            return true;
        }

        private bool ProcessPayment()
            => cardnum.TextLength % 2 == 0;
    }
}
