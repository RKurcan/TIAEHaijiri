namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    partial class MonthlyAttendanceReportDailougeFrm
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EmployeeSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.EmployeeChkBoxList = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sectionSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.SectionChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.departmentChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.DepartmentSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.cmbEnglishMonth = new System.Windows.Forms.ComboBox();
            this.yearTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.toDateMtb = new System.Windows.Forms.MaskedTextBox();
            this.cmbNepaliMonth = new System.Windows.Forms.ComboBox();
            this.FromMtb = new System.Windows.Forms.MaskedTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbtnStatistic = new System.Windows.Forms.RadioButton();
            this.rbtnSummary = new System.Windows.Forms.RadioButton();
            this.rbtnAttendance = new System.Windows.Forms.RadioButton();
            this.chkOrderByCode = new System.Windows.Forms.CheckBox();
            this.chkOrderByName = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(451, 319);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 151;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(369, 319);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(66, 23);
            this.btnReset.TabIndex = 150;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(286, 319);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(66, 23);
            this.btnGenerate.TabIndex = 149;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EmployeeSelectAllChkBox);
            this.groupBox3.Controls.Add(this.EmployeeChkBoxList);
            this.groupBox3.Location = new System.Drawing.Point(560, 93);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 216);
            this.groupBox3.TabIndex = 148;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Employee";
            // 
            // EmployeeSelectAllChkBox
            // 
            this.EmployeeSelectAllChkBox.AutoSize = true;
            this.EmployeeSelectAllChkBox.Location = new System.Drawing.Point(166, -1);
            this.EmployeeSelectAllChkBox.Name = "EmployeeSelectAllChkBox";
            this.EmployeeSelectAllChkBox.Size = new System.Drawing.Size(70, 17);
            this.EmployeeSelectAllChkBox.TabIndex = 135;
            this.EmployeeSelectAllChkBox.Text = "Select All";
            this.EmployeeSelectAllChkBox.UseVisualStyleBackColor = true;
            this.EmployeeSelectAllChkBox.CheckedChanged += new System.EventHandler(this.EmployeeSelectAllChkBox_CheckedChanged);
            // 
            // EmployeeChkBoxList
            // 
            this.EmployeeChkBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmployeeChkBoxList.FormattingEnabled = true;
            this.EmployeeChkBoxList.Location = new System.Drawing.Point(3, 16);
            this.EmployeeChkBoxList.Name = "EmployeeChkBoxList";
            this.EmployeeChkBoxList.Size = new System.Drawing.Size(227, 197);
            this.EmployeeChkBoxList.TabIndex = 125;
            this.EmployeeChkBoxList.SelectedValueChanged += new System.EventHandler(this.EmployeeChkBoxList_SelectedValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sectionSelectAllChkBox);
            this.groupBox2.Controls.Add(this.SectionChkboxlist);
            this.groupBox2.Location = new System.Drawing.Point(285, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 216);
            this.groupBox2.TabIndex = 147;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Section";
            // 
            // sectionSelectAllChkBox
            // 
            this.sectionSelectAllChkBox.AutoSize = true;
            this.sectionSelectAllChkBox.Location = new System.Drawing.Point(168, -1);
            this.sectionSelectAllChkBox.Name = "sectionSelectAllChkBox";
            this.sectionSelectAllChkBox.Size = new System.Drawing.Size(70, 17);
            this.sectionSelectAllChkBox.TabIndex = 135;
            this.sectionSelectAllChkBox.Text = "Select All";
            this.sectionSelectAllChkBox.UseVisualStyleBackColor = true;
            this.sectionSelectAllChkBox.CheckedChanged += new System.EventHandler(this.sectionSelectAllChkBox_CheckedChanged);
            // 
            // SectionChkboxlist
            // 
            this.SectionChkboxlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionChkboxlist.FormattingEnabled = true;
            this.SectionChkboxlist.Location = new System.Drawing.Point(3, 16);
            this.SectionChkboxlist.Name = "SectionChkboxlist";
            this.SectionChkboxlist.Size = new System.Drawing.Size(227, 197);
            this.SectionChkboxlist.TabIndex = 125;
            this.SectionChkboxlist.SelectedValueChanged += new System.EventHandler(this.SectionChkboxlist_SelectedValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.departmentChkboxlist);
            this.groupBox1.Controls.Add(this.DepartmentSelectAllChkBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 216);
            this.groupBox1.TabIndex = 146;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Department";
            // 
            // departmentChkboxlist
            // 
            this.departmentChkboxlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.departmentChkboxlist.FormattingEnabled = true;
            this.departmentChkboxlist.Location = new System.Drawing.Point(3, 16);
            this.departmentChkboxlist.Name = "departmentChkboxlist";
            this.departmentChkboxlist.Size = new System.Drawing.Size(227, 197);
            this.departmentChkboxlist.TabIndex = 131;
            this.departmentChkboxlist.SelectedValueChanged += new System.EventHandler(this.departmentChkboxlist_SelectedValueChanged);
            // 
            // DepartmentSelectAllChkBox
            // 
            this.DepartmentSelectAllChkBox.AutoSize = true;
            this.DepartmentSelectAllChkBox.Location = new System.Drawing.Point(168, 0);
            this.DepartmentSelectAllChkBox.Name = "DepartmentSelectAllChkBox";
            this.DepartmentSelectAllChkBox.Size = new System.Drawing.Size(70, 17);
            this.DepartmentSelectAllChkBox.TabIndex = 132;
            this.DepartmentSelectAllChkBox.Text = "Select All";
            this.DepartmentSelectAllChkBox.UseVisualStyleBackColor = true;
            this.DepartmentSelectAllChkBox.CheckedChanged += new System.EventHandler(this.DepartmentSelectAllChkBox_CheckedChanged);
            // 
            // cmbEnglishMonth
            // 
            this.cmbEnglishMonth.FormattingEnabled = true;
            this.cmbEnglishMonth.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cmbEnglishMonth.Location = new System.Drawing.Point(277, 9);
            this.cmbEnglishMonth.Name = "cmbEnglishMonth";
            this.cmbEnglishMonth.Size = new System.Drawing.Size(121, 21);
            this.cmbEnglishMonth.TabIndex = 152;
            this.cmbEnglishMonth.Visible = false;
            this.cmbEnglishMonth.SelectedValueChanged += new System.EventHandler(this.cmbEnglishMonth_SelectedValueChanged);
            // 
            // yearTxt
            // 
            this.yearTxt.Location = new System.Drawing.Point(44, 9);
            this.yearTxt.Name = "yearTxt";
            this.yearTxt.Size = new System.Drawing.Size(67, 20);
            this.yearTxt.TabIndex = 153;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 154;
            this.label1.Text = "Year";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 155;
            this.label2.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 156;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(232, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 157;
            this.label4.Text = "To";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(117, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 160;
            this.label7.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(130, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 159;
            this.label5.Text = "(YYYY/MM/DD )";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(350, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 17);
            this.label6.TabIndex = 163;
            this.label6.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(367, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 162;
            this.label8.Text = "(YYYY/MM/DD )";
            // 
            // toDateMtb
            // 
            this.toDateMtb.Location = new System.Drawing.Point(277, 42);
            this.toDateMtb.Mask = "0000/00/00";
            this.toDateMtb.Name = "toDateMtb";
            this.toDateMtb.Size = new System.Drawing.Size(67, 20);
            this.toDateMtb.TabIndex = 161;
            // 
            // cmbNepaliMonth
            // 
            this.cmbNepaliMonth.FormattingEnabled = true;
            this.cmbNepaliMonth.Items.AddRange(new object[] {
            "Baisakh",
            "Jestha",
            "Ashad",
            "Shrawan",
            "Bhadra",
            "Asoj",
            "Kartik",
            "Mangshir",
            "Poush",
            "Magh",
            "Falgun",
            "Chaitra"});
            this.cmbNepaliMonth.Location = new System.Drawing.Point(277, 9);
            this.cmbNepaliMonth.Name = "cmbNepaliMonth";
            this.cmbNepaliMonth.Size = new System.Drawing.Size(121, 21);
            this.cmbNepaliMonth.TabIndex = 164;
            this.cmbNepaliMonth.Visible = false;
            this.cmbNepaliMonth.SelectedValueChanged += new System.EventHandler(this.cmbNepaliMonth_SelectedValueChanged);
            // 
            // FromMtb
            // 
            this.FromMtb.Location = new System.Drawing.Point(44, 41);
            this.FromMtb.Mask = "0000/00/00";
            this.FromMtb.Name = "FromMtb";
            this.FromMtb.Size = new System.Drawing.Size(67, 20);
            this.FromMtb.TabIndex = 161;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbtnStatistic);
            this.groupBox4.Controls.Add(this.rbtnSummary);
            this.groupBox4.Controls.Add(this.rbtnAttendance);
            this.groupBox4.Location = new System.Drawing.Point(12, 313);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(230, 53);
            this.groupBox4.TabIndex = 169;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Report Param";
            // 
            // rbtnStatistic
            // 
            this.rbtnStatistic.AutoSize = true;
            this.rbtnStatistic.Location = new System.Drawing.Point(163, 19);
            this.rbtnStatistic.Name = "rbtnStatistic";
            this.rbtnStatistic.Size = new System.Drawing.Size(62, 17);
            this.rbtnStatistic.TabIndex = 2;
            this.rbtnStatistic.Text = "Statistic";
            this.rbtnStatistic.UseVisualStyleBackColor = true;
            // 
            // rbtnSummary
            // 
            this.rbtnSummary.AutoSize = true;
            this.rbtnSummary.Location = new System.Drawing.Point(90, 19);
            this.rbtnSummary.Name = "rbtnSummary";
            this.rbtnSummary.Size = new System.Drawing.Size(68, 17);
            this.rbtnSummary.TabIndex = 1;
            this.rbtnSummary.Text = "Summary";
            this.rbtnSummary.UseVisualStyleBackColor = true;
            // 
            // rbtnAttendance
            // 
            this.rbtnAttendance.AutoSize = true;
            this.rbtnAttendance.Checked = true;
            this.rbtnAttendance.Location = new System.Drawing.Point(6, 19);
            this.rbtnAttendance.Name = "rbtnAttendance";
            this.rbtnAttendance.Size = new System.Drawing.Size(80, 17);
            this.rbtnAttendance.TabIndex = 0;
            this.rbtnAttendance.TabStop = true;
            this.rbtnAttendance.Text = "Attendance";
            this.rbtnAttendance.UseVisualStyleBackColor = true;
            // 
            // chkOrderByCode
            // 
            this.chkOrderByCode.AutoSize = true;
            this.chkOrderByCode.Checked = true;
            this.chkOrderByCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrderByCode.Location = new System.Drawing.Point(41, 22);
            this.chkOrderByCode.Name = "chkOrderByCode";
            this.chkOrderByCode.Size = new System.Drawing.Size(51, 17);
            this.chkOrderByCode.TabIndex = 170;
            this.chkOrderByCode.Text = "Code";
            this.chkOrderByCode.UseVisualStyleBackColor = true;
            // 
            // chkOrderByName
            // 
            this.chkOrderByName.AutoSize = true;
            this.chkOrderByName.Location = new System.Drawing.Point(41, 45);
            this.chkOrderByName.Name = "chkOrderByName";
            this.chkOrderByName.Size = new System.Drawing.Size(54, 17);
            this.chkOrderByName.TabIndex = 171;
            this.chkOrderByName.Text = "Name";
            this.chkOrderByName.UseVisualStyleBackColor = true;
            this.chkOrderByName.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkOrderByCode);
            this.groupBox5.Controls.Add(this.chkOrderByName);
            this.groupBox5.Location = new System.Drawing.Point(560, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(165, 71);
            this.groupBox5.TabIndex = 172;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Order By";
            // 
            // MonthlyAttendanceReportDailougeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 373);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cmbNepaliMonth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FromMtb);
            this.Controls.Add(this.toDateMtb);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yearTxt);
            this.Controls.Add(this.cmbEnglishMonth);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MonthlyAttendanceReportDailougeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Montly Base Attendance Report";
            this.Load += new System.EventHandler(this.MonthlyAttendanceReportDailougeFrm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox EmployeeSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox EmployeeChkBoxList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox sectionSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox SectionChkboxlist;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox departmentChkboxlist;
        private System.Windows.Forms.CheckBox DepartmentSelectAllChkBox;
        private System.Windows.Forms.ComboBox cmbEnglishMonth;
        private System.Windows.Forms.TextBox yearTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox toDateMtb;
        private System.Windows.Forms.ComboBox cmbNepaliMonth;
        private System.Windows.Forms.MaskedTextBox FromMtb;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbtnStatistic;
        private System.Windows.Forms.RadioButton rbtnSummary;
        private System.Windows.Forms.RadioButton rbtnAttendance;
        private System.Windows.Forms.CheckBox chkOrderByCode;
        private System.Windows.Forms.CheckBox chkOrderByName;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}