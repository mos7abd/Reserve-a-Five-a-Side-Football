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
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Reserve__a_Five_a_Side_Football
{
    public partial class Add_IncomeingPrice : BaseForm
    {
        private readonly Reserve_a_Five_a_SideEntities Context;

        public Add_IncomeingPrice()
        {
            InitializeComponent();
            Context = new Reserve_a_Five_a_SideEntities();
            LoadUniqueAreas();
            this.MinimumSize = new Size(1400, 800); // Adjust1374, 812 the size according to your requirements
            this.MaximumSize = new Size(1700, 900); // Adjust1374, 812 the size according to your requirements
        }
        private void LoadUniqueAreas()
        {
            var uniqueAreas = Context.Stadium.Select(s => s.Area).Distinct().ToArray();
            CityComboBox1.Items.AddRange(uniqueAreas);
        }
        

        // GetStadiumNames
        private void GetReports()
        {
            var fromDate = FromDatePicker.Value;
            var toDate = dateTimePicker1.Value;
            var selectedCity = CityComboBox1.SelectedItem;
            var selectedStadium = guna2ComboBox1.SelectedItem;

            var query = from s in Context.Stadium
                        join r in Context.Reservations on s.StadiumID equals r.StadiumID
                        where r.Reservation_Date >= fromDate && r.Reservation_Date <= toDate
                        select new { s, r };

            // Apply city filter if a city is selected
            if (selectedCity != null)
            {
                query = query.Where(x => x.s.Area == selectedCity);
            }

            // Apply stadium filter if a stadium is selected
            if (selectedStadium != null)
            {
                query = query.Where(x => x.s.Stad_Name == selectedStadium);
            }

            var result = query.GroupBy(x => new { x.s.Stad_Name, x.r.Reservation_Date, x.s.Hourly_Price })
                              .Select(grp => new
                              {
                                  grp.Key.Stad_Name,
                                  grp.Key.Reservation_Date,
                                  grp.Key.Hourly_Price,
                                  TotalHourlyPrice = grp.Sum(x => x.s.Hourly_Price),
                                  Reservation_Count = grp.Count()
                              });

            dataGridView1.Rows.Clear();
            foreach (var item in result)
            {
                dataGridView1.Rows.Add(
                    item.Stad_Name,
                    item.Hourly_Price.ToString(),
                    item.Reservation_Date?.ToString("yyyy-MM-dd"),
                    item.Reservation_Count,
                    item.TotalHourlyPrice.ToString()
                );
            }
        }




        private void ShowData_btn_Click(object sender, EventArgs e)
        {
            GetReports();
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["TotalPricePerDay"].Value != null && decimal.TryParse(row.Cells["TotalPricePerDay"].Value.ToString(), out decimal price))
                    totalAmount += price;
            }
            TotalAmountTxtBox.Text = totalAmount.ToString() + " EGP";
        }



        private void CityComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCity = CityComboBox1.SelectedItem?.ToString();
            if (selectedCity != null)
            {
                var stadiums = Context.Stadium
                    .Where(s => s.Area == selectedCity && s.Stad_Status == "Active")
                    .Select(s => s.Stad_Name)
                    .ToArray();
                guna2ComboBox1.Items.Clear();
                guna2ComboBox1.Items.AddRange(stadiums);
            }
        }
    }
}
