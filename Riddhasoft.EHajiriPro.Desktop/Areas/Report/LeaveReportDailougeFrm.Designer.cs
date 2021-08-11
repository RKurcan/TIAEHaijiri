namespace Riddhasoft.EHajiriPro.Desktop.Areas.Report
{
    partial class LeaveReportDailougeFrm
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
            this.LeaveReportParamGB = new System.Windows.Forms.GroupBox();
            this.rbtnEmpWise = new System.Windows.Forms.RadioButton();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.LeaveReportParamGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(415, 237);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 157;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(333, 237);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(66, 23);
            this.btnReset.TabIndex = 156;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(250, 237);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(66, 23);
            this.btnGenerate.TabIndex = 155;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EmployeeSelectAllChkBox);
            this.groupBox3.Controls.Add(this.EmployeeChkBoxList);
            this.groupBox3.Location = new System.Drawing.Point(497, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 216);
            this.groupBox3.TabIndex = 154;
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
            this.groupBox2.Location = new System.Drawing.Point(249, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 216);
            this.groupBox2.TabIndex = 153;
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
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 216);
            this.groupBox1.TabIndex = 152;
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
            // LeaveReportParamGB
            // 
            this.LeaveReportParamGB.Controls.Add(this.rbtnEmpWise);
            this.LeaveReportParamGB.Location = new System.Drawing.Point(13, 232);
            this.LeaveReportParamGB.Name = "LeaveReportParamGB";
            this.LeaveReportParamGB.Size = new System.Drawing.Size(222, 50);
            this.LeaveReportParamGB.TabIndex = 158;
            this.LeaveReportParamGB.TabStop = false;
            this.LeaveReportParamGB.Text = "Report Param";
            // 
            // rbtnEmpWise
            // 
            this.rbtnEmpWise.AutoSize = true;
            this.rbtnEmpWise.Checked = true;
            this.rbtnEmpWise.Location = new System.Drawing.Point(7, 20);
            this.rbtnEmpWise.Name = "rbtnEmpWise";
            this.rbtnEmpWise.Size = new System.Drawing.Size(98, 17);
            this.rbtnEmpWise.TabIndex = 0;
            this.rbtnEmpWise.TabStop = true;
            this.rbtnEmpWise.Text = "Employee Wise";
            this.rbtnEmpWise.UseVisualStyleBackColor = true;
            // 
            // LeaveReportDailougeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 294);
            this.Controls.Add(this.LeaveReportParamGB);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LeaveReportDailougeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Leave Report";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.LeaveReportParamGB.ResumeLayout(false);
            this.LeaveReportParamGB.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox LeaveReportParamGB;
        private System.Windows.Forms.RadioButton rbtnEmpWise;
    }
}