using System.Linq;
using System.Windows.Forms;
using System;
using Reserve__a_Five_a_Side_Football;
using Reserve__a_Five_a_Side_Football.Database;
using RegertrationPage;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Login_
{
    public partial class Login_Form : Form
    {
        Reserve_a_Five_a_SideEntities Context;
        public Login_Form()
        {
            Context = new Reserve_a_Five_a_SideEntities();
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
                e.Cancel = true;
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
                textBox2.PasswordChar = '\0';
            
            else
                textBox2.PasswordChar = '*';
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            var user = Context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();

            if (user == null)
                MessageBox.Show("Invalid email or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
            {
                CurrentUserLogin.UserLogginID = user.UserID;
                MessageBox.Show("Login Successful", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                designForm designForm = new designForm();
                this.Hide();
                designForm.ShowDialog();
                this.Close();

            }

        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
            => this.Close();


        private void RegesterNow_Click(object sender, EventArgs e)
        {
            Regest regest = new Regest();
            this.Hide();
            regest.ShowDialog();
            this.Close();
        }

    }
}
