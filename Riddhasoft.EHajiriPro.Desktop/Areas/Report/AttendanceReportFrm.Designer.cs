namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    partial class AttendanceReportFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttendanceReportFrm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDailyAttendance = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMonthlyAttendance = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.btnDailyAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.btnMonthlyAttendance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDailyAttendance);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnDailyAttendance
            // 
            this.btnDailyAttendance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(144)))), ((int)(((byte)(158)))));
            this.btnDailyAttendance.Controls.Add(this.pictureBox1);
            this.btnDailyAttendance.Controls.Add(this.label1);
            this.btnDailyAttendance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDailyAttendance.Location = new System.Drawing.Point(3, 19);
            this.btnDailyAttendance.Name = "btnDailyAttendance";
            this.btnDailyAttendance.Size = new System.Drawing.Size(232, 208);
            this.btnDailyAttendance.TabIndex = 3;
            this.btnDailyAttendance.Click += new System.EventHandler(this.btnDailyAttendance_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.pie_chart_normal;
            this.pictureBox1.Location = new System.Drawing.Point(57, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(126, 125);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(94, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Daily";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMonthlyAttendance);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(238, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 230);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnMonthlyAttendance
            // 
            this.btnMonthlyAttendance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(88)))), ((int)(((byte)(129)))));
            this.btnMonthlyAttendance.Controls.Add(this.pictureBox2);
            this.btnMonthlyAttendance.Controls.Add(this.label2);
            this.btnMonthlyAttendance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMonthlyAttendance.Location = new System.Drawing.Point(3, 19);
            this.btnMonthlyAttendance.Name = "btnMonthlyAttendance";
            this.btnMonthlyAttendance.Size = new System.Drawing.Size(250, 208);
            this.btnMonthlyAttendance.TabIndex = 4;
            this.btnMonthlyAttendance.Click += new System.EventHandler(this.btnMonthlyAttendance_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.pie_chart_normal;
            this.pictureBox2.Location = new System.Drawing.Point(63, 23);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(126, 125);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(88, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Monthly";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // AttendanceReportFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 230);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AttendanceReportFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Attendance Report";
            this.groupBox1.ResumeLayout(false);
            this.btnDailyAttendance.ResumeLayout(false);
            this.btnDailyAttendance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.btnMonthlyAttendance.ResumeLayout(false);
            this.btnMonthlyAttendance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel btnDailyAttendance;
        private System.Windows.Forms.Panel btnMonthlyAttendance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}