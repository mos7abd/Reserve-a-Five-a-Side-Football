namespace Reserve__a_Five_a_Side_Football
{
    partial class update__delete_Legue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.begindate = new System.Windows.Forms.TextBox();
            this.enddate = new System.Windows.Forms.TextBox();
            this.endrange = new System.Windows.Forms.TextBox();
            this.stadiunname = new System.Windows.Forms.TextBox();
            this.timeplay = new System.Windows.Forms.TextBox();
            this.update_Btn = new System.Windows.Forms.Button();
            this.delete_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(230, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Legue For Player";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 74);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(674, 172);
            this.dataGridView1.TabIndex = 1;
            // 
            // begindate
            // 
            this.begindate.Location = new System.Drawing.Point(12, 266);
            this.begindate.Name = "begindate";
            this.begindate.Size = new System.Drawing.Size(100, 20);
            this.begindate.TabIndex = 2;
            // 
            // enddate
            // 
            this.enddate.Location = new System.Drawing.Point(12, 292);
            this.enddate.Name = "enddate";
            this.enddate.Size = new System.Drawing.Size(100, 20);
            this.enddate.TabIndex = 3;
            // 
            // endrange
            // 
            this.endrange.Location = new System.Drawing.Point(12, 323);
            this.endrange.Name = "endrange";
            this.endrange.Size = new System.Drawing.Size(100, 20);
            this.endrange.TabIndex = 4;
            // 
            // stadiunname
            // 
            this.stadiunname.Location = new System.Drawing.Point(130, 266);
            this.stadiunname.Name = "stadiunname";
            this.stadiunname.Size = new System.Drawing.Size(100, 20);
            this.stadiunname.TabIndex = 5;
            // 
            // timeplay
            // 
            this.timeplay.Location = new System.Drawing.Point(130, 292);
            this.timeplay.Name = "timeplay";
            this.timeplay.Size = new System.Drawing.Size(100, 20);
            this.timeplay.TabIndex = 6;
            // 
            // update_Btn
            // 
            this.update_Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.update_Btn.Location = new System.Drawing.Point(587, 289);
            this.update_Btn.Name = "update_Btn";
            this.update_Btn.Size = new System.Drawing.Size(86, 33);
            this.update_Btn.TabIndex = 7;
            this.update_Btn.Text = "Update";
            this.update_Btn.UseVisualStyleBackColor = true;
            this.update_Btn.Click += new System.EventHandler(this.update_Btn_Click);
            // 
            // delete_Btn
            // 
            this.delete_Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delete_Btn.Location = new System.Drawing.Point(469, 289);
            this.delete_Btn.Name = "delete_Btn";
            this.delete_Btn.Size = new System.Drawing.Size(90, 33);
            this.delete_Btn.TabIndex = 8;
            this.delete_Btn.Text = "Delete";
            this.delete_Btn.UseVisualStyleBackColor = true;
            this.delete_Btn.Click += new System.EventHandler(this.delete_Btn_Click);
            // 
            // update__delete_Legue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 355);
            this.Controls.Add(this.delete_Btn);
            this.Controls.Add(this.update_Btn);
            this.Controls.Add(this.timeplay);
            this.Controls.Add(this.stadiunname);
            this.Controls.Add(this.endrange);
            this.Controls.Add(this.enddate);
            this.Controls.Add(this.begindate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "update__delete_Legue";
            this.Text = "update__delete_Legue";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox begindate;
        private System.Windows.Forms.TextBox enddate;
        private System.Windows.Forms.TextBox endrange;
        private System.Windows.Forms.TextBox stadiunname;
        private System.Windows.Forms.TextBox timeplay;
        private System.Windows.Forms.Button update_Btn;
        private System.Windows.Forms.Button delete_Btn;
    }
}