
using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class PaymentByWallet : Form
    {
        private Reserve_a_Five_a_SideEntities context;
        private decimal? totalPaymentAmount;
        public PaymentByWallet(decimal? totalPaymentAmount)
        {
            InitializeComponent();
            context = new Reserve_a_Five_a_SideEntities();
            this.totalPaymentAmount = totalPaymentAmount;
            Walletlbl.Text = this.totalPaymentAmount.ToString() + " EGP";
        }

        private void confirmbtn_Click(object sender, EventArgs e)
        {
            if (ValidateWalletData())
            {
                MessageBox.Show("Payment Successful. Reservation Confirmed", "Payment Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Payment Failed. Please try again.", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private bool ValidateWalletData()
        {
            if (phonenum.TextLength != 11 || name.TextLength < 20)
            {
                MessageBox.Show("Invalid Data", "Confirm Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
       
    }
}
