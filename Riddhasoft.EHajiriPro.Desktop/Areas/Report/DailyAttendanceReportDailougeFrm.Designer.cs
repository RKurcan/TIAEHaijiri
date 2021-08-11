namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    partial class DailyAttendanceReportDailougeFrm
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
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.departmentChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.DepartmentSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sectionSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.SectionChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EmployeeSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.EmployeeChkBoxList = new System.Windows.Forms.CheckedListBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.FromMtb = new System.Windows.Forms.MaskedTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbtnEarlyIn = new System.Windows.Forms.RadioButton();
            this.rbtnLateIn = new System.Windows.Forms.RadioButton();
            this.rbtnAttendance = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(130, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 76;
            this.label7.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(148, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 75;
            this.label3.Text = "( YYYY/MM/DD )";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 77;
            this.label1.Text = "From";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.departmentChkboxlist);
            this.groupBox1.Controls.Add(this.DepartmentSelectAllChkBox);
            this.groupBox1.Location = new System.Drawing.Point(15, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 216);
            this.groupBox1.TabIndex = 134;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sectionSelectAllChkBox);
            this.groupBox2.Controls.Add(this.SectionChkboxlist);
            this.groupBox2.Location = new System.Drawing.Point(288, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 216);
            this.groupBox2.TabIndex = 135;
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EmployeeSelectAllChkBox);
            this.groupBox3.Controls.Add(this.EmployeeChkBoxList);
            this.groupBox3.Location = new System.Drawing.Point(563, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 216);
            this.groupBox3.TabIndex = 142;
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
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(291, 302);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(66, 23);
            this.btnGenerate.TabIndex = 143;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(373, 302);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(66, 23);
            this.btnReset.TabIndex = 144;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(454, 302);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 145;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FromMtb
            // 
            this.FromMtb.Location = new System.Drawing.Point(57, 31);
            this.FromMtb.Mask = "0000/00/00";
            this.FromMtb.Name = "FromMtb";
            this.FromMtb.Size = new System.Drawing.Size(67, 20);
            this.FromMtb.TabIndex = 167;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbtnEarlyIn);
            this.groupBox4.Controls.Add(this.rbtnLateIn);
            this.groupBox4.Controls.Add(this.rbtnAttendance);
            this.groupBox4.Location = new System.Drawing.Point(18, 296);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(230, 53);
            this.groupBox4.TabIndex = 168;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Report Param";
            // 
            // rbtnEarlyIn
            // 
            this.rbtnEarlyIn.AutoSize = true;
            this.rbtnEarlyIn.Location = new System.Drawing.Point(156, 19);
            this.rbtnEarlyIn.Name = "rbtnEarlyIn";
            this.rbtnEarlyIn.Size = new System.Drawing.Size(60, 17);
            this.rbtnEarlyIn.TabIndex = 2;
            this.rbtnEarlyIn.TabStop = true;
            this.rbtnEarlyIn.Text = "Early In";
            this.rbtnEarlyIn.UseVisualStyleBackColor = true;
            // 
            // rbtnLateIn
            // 
            this.rbtnLateIn.AutoSize = true;
            this.rbtnLateIn.Location = new System.Drawing.Point(92, 19);
            this.rbtnLateIn.Name = "rbtnLateIn";
            this.rbtnLateIn.Size = new System.Drawing.Size(58, 17);
            this.rbtnLateIn.TabIndex = 1;
            this.rbtnLateIn.TabStop = true;
            this.rbtnLateIn.Text = "Late In";
            this.rbtnLateIn.UseVisualStyleBackColor = true;
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
            // DailyAttendanceReportDailougeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 359);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.FromMtb);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DailyAttendanceReportDailougeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daily Base Attendance Report";
            this.Load += new System.EventHandler(this.DailyAttendanceReportDailougeFrm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox departmentChkboxlist;
        private System.Windows.Forms.CheckBox DepartmentSelectAllChkBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox sectionSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox SectionChkboxlist;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox EmployeeSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox EmployeeChkBoxList;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.MaskedTextBox FromMtb;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbtnAttendance;
        private System.Windows.Forms.RadioButton rbtnEarlyIn;
        private System.Windows.Forms.RadioButton rbtnLateIn;
    }
}