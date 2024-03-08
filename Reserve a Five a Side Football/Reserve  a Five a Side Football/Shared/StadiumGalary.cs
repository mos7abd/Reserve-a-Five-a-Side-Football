using Login_;
using RegertrationPage;
using ReservationPage;
using Reserve__a_Five_a_Side_Football.Database;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class StadiumGalary : Form
    {
        private Reserve_a_Five_a_SideEntities context = new Reserve_a_Five_a_SideEntities();

        PictureBox pictureBox;
        private Label price;
        private Label area;
        private Label name;
        private Button button;

        public EventHandler button_Clicked { get; private set; }

        public StadiumGalary()
        {
            InitializeComponent();

        }

        public void GetData()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();
                if (context.Stadium == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }
                var query = from a in context.Stadium
                            select a;

                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.Stadium_Image))
                    {
                        price = new Label();
                        price.Text = (item.Hourly_Price.ToString()) + "e.g";
                        price.BackColor = Color.AliceBlue;
                        price.TextAlign = ContentAlignment.MiddleCenter;
                        price.Font = new Font("", 13);
                        // price.Dock = DockStyle.Bottom;

                        area = new Label();
                        area.Text = item.Area.ToString();

                        area.BackColor = Color.White;
                        area.TextAlign = ContentAlignment.MiddleCenter;
                        area.Font = new Font("Calibri", 12);
                        area.Dock = DockStyle.Top;

                        name = new Label();
                        name.Text = item.Stad_Name.ToString();
                        name.BackColor = Color.White;
                        name.TextAlign = ContentAlignment.MiddleCenter;
                        name.Dock = DockStyle.Top;

                        button = new Button();
                        button.Width = 50;
                        button.Height = 40;
                        button.Text = item.Stad_Name + " , " + item.Area;
                        button.BackgroundImageLayout = ImageLayout.Stretch;
                        button.Click += new System.EventHandler(this.Button_Clicked);
                        button.Dock = DockStyle.Bottom;

                        //flowLayoutPanel2.Controls.Add(button);

                        pictureBox = new PictureBox
                        {
                            Width = 300,
                            Height = 200,
                            BackgroundImageLayout = ImageLayout.Stretch,
                            Location = new Point(150, 150),
                            BorderStyle = BorderStyle.FixedSingle

                        };

                        try
                        {

                            string currentPath = System.IO.Directory.GetCurrentDirectory();

                           

                            Bitmap bitmap = new Bitmap(currentPath + "\\Photos\\" + item.Stadium_Image);
                            pictureBox.BackgroundImage = bitmap;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading image for stadium: {ex.Message}");
                        }

                        pictureBox.Controls.Add(price);
                        //pictureBox.Controls.Add(name);
                        //pictureBox.Controls.Add(area);
                        pictureBox.Controls.Add(button);

                        flowLayoutPanel1.Controls.Add(pictureBox);

                        //flowLayoutPanel1.Controls.Add(name);
                        // name.Controls.Add(area);
                        // flowLayoutPanel1.Controls.Add(area);

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void StadiumGalary_Load(object sender, EventArgs e)
        {

            GetData();
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            ReservationForm res1 = new ReservationForm();
            res1.ShowDialog();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            HomeScreen homeScreen = new HomeScreen();
            homeScreen.ShowDialog();
            //guna2Button1.Enabled = false;
        }

        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
            Regest regest = new Regest();
            regest.Show();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            login_Form.ShowDialog();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ReservationForm res1 = new ReservationForm();
            res1.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LeagueForm leagueForm = new LeagueForm();
            leagueForm.ShowDialog();

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            AddYourTeam addYourTeam = new AddYourTeam();
            addYourTeam.ShowDialog();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Add_update_del_Stadium newstadium = new Add_update_del_Stadium();
            newstadium.ShowDialog();
        }

        private void StadiumGalary_FormClosing(object sender, FormClosingEventArgs e)
        {

            var result = MessageBox.Show("Are you sure To close  ", "Close Form",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            e.Cancel = (result == DialogResult.No);
        }
    }
}
