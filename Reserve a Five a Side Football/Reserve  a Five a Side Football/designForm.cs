using Login_;
using RegertrationPage;
using ReservationPage;
using Reserve__a_Five_a_Side_Football.Database;
using Reserve__a_Five_a_Side_Football.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Messaging.Design;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class designForm : Form
    {

        private int currentIndex = 0;
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        Reserve_a_Five_a_SideEntities context;
        string GetOwner;
        string GetTextLeague;
        string GetTextIfNull;
        //Constructor
        public designForm()
        {
            InitializeComponent();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
            random = new Random();
            btnCloseChildForm.Visible = false;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            context = new Reserve_a_Five_a_SideEntities();
            GetOwner = GetIDforOwner();
            CountMessageForPlayer();
            if (GetOwner == "Owner")
            {
                button6.Visible = true;
                button10.Visible = true;
                button11.Visible = true;
                button1.Text = "All Reservation";
                button3.Text = "Add League";
                button4.Text = "Add Reservation";
                button5.Text = "Stadiums";
            }
            GetTextIfNull =  addTextLeagueMessageToLabel();
            if (string.IsNullOrEmpty(GetTextIfNull))
            {
                GetTextIfNull = "Currently, there are no leagues available for registration. Please stay tuned for updates on upcoming leagues. Thank you for your patience and continued support.";
                label1.Text = GetTextIfNull;
                timer1.Interval = 350; // Adjust speed here (in milliseconds)
                timer1.Start();
            }
            else
            {
                GetTextLeague = label1.Text;
                timer1.Interval = 350; // Adjust speed here (in milliseconds)
                timer1.Start();
            }

        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    currentButton = (Button)btnSender;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    btnCloseChildForm.Visible = true;
                }
            }
        }

        private void DisableButton()
        {
            foreach (Control previousBtn in panelmenue.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(0, 64, 64);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void OpenChildForm(Form childForm, object btnSender)
        {
            if (activeForm != null)
                activeForm.Close();
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktopPane.Controls.Add(childForm);
            this.panelDesktopPane.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitle.Text = childForm.Text;

        }
        public string GetIDforOwner()
        {
            var CurrentOwner = context.Users
                .Where(e => e.UserID == CurrentUserLogin.UserLogginID)
                .Select(e => e.TypeOFUser).FirstOrDefault();

            return CurrentOwner;
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            if (GetOwner == "Owner")
                OpenChildForm(new ShowAllRevToOwner(), sender);
            else
                OpenChildForm(new ReservationForm(), sender);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (GetOwner == "Owner")
                OpenChildForm(new OwnerAddLeague(), sender);
            else
                OpenChildForm(new LeagueForm(), sender);
        }


        private void btn3_Click(object sender, EventArgs e)
        {
            if (GetOwner == "Owner")
                OpenChildForm(new addReservationByOwner(), sender);

            else
                OpenChildForm(new DeleteResrvation(), sender);
        }


        private void btn4_Click(object sender, EventArgs e)
        {
            if (GetOwner == "Owner")
                OpenChildForm(new Add_update_del_Stadium(), sender);
            else
                OpenChildForm(new TeamInformation(), sender);
        }

        private void button6_Click(object sender, EventArgs e)
           => OpenChildForm(new Player_Data(), sender);

        private void button10_Click_1(object sender, EventArgs e)
            => OpenChildForm(new ShowTeamsToOwner(), sender);

        private void button11_Click(object sender, EventArgs e)
             => OpenChildForm(new Add_IncomeingPrice(), sender);

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            Reset();
        }

        private void Reset()
        {
            DisableButton();
            lblTitle.Text = "Reserve a Five-a-Side Football";
            panelTitleBar.BackColor = Color.Teal;
            panellogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            btnCloseChildForm.Visible = false;
        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void button7_Click(object sender, EventArgs e)
            => Application.Exit();


        private void button8_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void button9_Click(object sender, EventArgs e)
            => this.WindowState = FormWindowState.Minimized;



        private void CountMessageForPlayer()
        {
            var unreadMessages = context.ReservationMessages
                .Where(e => e.PlayerID == CurrentUserLogin.UserLogginID)
                .Select(e => new { e.MessageID, e.MessageContent, e.IsRead })
                .OrderByDescending(e => e.MessageID).ToList();

            lblMessage.Text = unreadMessages.Count.ToString();

            guna2DataGridView1.Rows.Clear();

            // Add column to DataGridView if not already added
            if (guna2DataGridView1.Columns.Count == 0)
            {
                guna2DataGridView1.Columns.Add("Message", "Message");
            }

            int count = 0;
            foreach (var message in unreadMessages)
            {
                var row = guna2DataGridView1.Rows.Add(message.MessageContent);

                if (message.IsRead == false)
                {
                    count++;
                    // Set the font style to bold for unread messages
                    guna2DataGridView1.Rows[row].Cells[0].Style.Font = new Font(guna2DataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                }
            }

            lblMessage.Text = count.ToString();
            guna2DataGridView1.AutoResizeRows();
        }


        private void pictMessageBox_Click(object sender, EventArgs e)
        {
            CountMessageForPlayer();
            guna2DataGridView1.Visible = !guna2DataGridView1.Visible;
        }


        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Check if the font style is bold for the specific cell
                bool isBold = (cell.Style.Font != null && cell.Style.Font.Style.HasFlag(FontStyle.Bold));

                if (isBold)
                {
                    cell.Style.Font = new Font(cell.Style.Font, FontStyle.Regular);
                    lblMessage.Text = (int.Parse(lblMessage.Text) - 1).ToString();

                    // Update the message status to read
                    var message = context.ReservationMessages
                        .FirstOrDefault(m => m.PlayerID == CurrentUserLogin.UserLogginID && m.MessageContent == cell.Value.ToString());

                    if (message != null)
                    {
                        message.IsRead = true;
                        context.SaveChanges();
                    }
                }
            }
        }



        // Log Out
        private void button12_Click(object sender, EventArgs e)
        {
            CurrentUserLogin.UserLogginID = 0;
            Login_Form login_Form = new Login_Form();
            this.Hide();
            login_Form.ShowDialog();
            this.Close();
        }

        private string addTextLeagueMessageToLabel()
        {
            var latestMessage = context.LeagueMessages
                        .Where(e => e.DeleteTimestamp > DateTime.Now)
                        .OrderByDescending(e => e.MessageID)
                        .Select(e => e.MessageContent)
                        .FirstOrDefault();
            label1.Text = latestMessage;
            return latestMessage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (GetTextIfNull == "Currently, there are no leagues available for registration. Please stay tuned for updates on upcoming leagues. Thank you for your patience and continued support.")
            {
                
            string fullText = GetTextIfNull + GetTextIfNull.Substring(0, currentIndex);

            // Update label text with scrolling effect
            if (currentIndex == GetTextIfNull.Length)
            {// Reset to the beginning if reached the end of the news text
                currentIndex = 0;
            }
            label1.Text = fullText.Substring(currentIndex);
            currentIndex++;
            }
            else
            {
                string fullText = GetTextLeague + GetTextLeague.Substring(0, currentIndex);

                // Update label text with scrolling effect
                if (currentIndex == GetTextLeague.Length)
                {
                    // Reset to the beginning if reached the end of the news text
                    currentIndex = 0;
                }
                label1.Text = fullText.Substring(currentIndex);
                currentIndex++;
            }
        }

    }
}
