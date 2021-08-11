namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    partial class MonthlyRosterSetupFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonthlyRosterSetupFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.departmentChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.DepartmentSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sectionSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.SectionChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EmployeeSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.EmployeeChkBoxList = new System.Windows.Forms.CheckedListBox();
            this.btnRefreshRoster = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MonthCmb = new System.Windows.Forms.ComboBox();
            this.YearTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.ManualRosterGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ManualRosterGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1176, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_cancel_48;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "Exit";
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
            this.groupBox2.Location = new System.Drawing.Point(257, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 216);
            this.groupBox2.TabIndex = 146;
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
            this.groupBox1.TabIndex = 145;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Department";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EmployeeSelectAllChkBox);
            this.groupBox3.Controls.Add(this.EmployeeChkBoxList);
            this.groupBox3.Location = new System.Drawing.Point(514, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 216);
            this.groupBox3.TabIndex = 150;
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
            // btnRefreshRoster
            // 
            this.btnRefreshRoster.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRefreshRoster.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_calendar_48;
            this.btnRefreshRoster.Location = new System.Drawing.Point(823, 70);
            this.btnRefreshRoster.Name = "btnRefreshRoster";
            this.btnRefreshRoster.Size = new System.Drawing.Size(100, 74);
            this.btnRefreshRoster.TabIndex = 2;
            this.btnRefreshRoster.Text = "Refresh Roster";
            this.btnRefreshRoster.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefreshRoster.UseVisualStyleBackColor = true;
            this.btnRefreshRoster.Click += new System.EventHandler(this.btnRefreshRoster_Click_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MonthCmb);
            this.panel1.Controls.Add(this.YearTxt);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnRefreshRoster);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1176, 233);
            this.panel1.TabIndex = 153;
            // 
            // MonthCmb
            // 
            this.MonthCmb.FormattingEnabled = true;
            this.MonthCmb.Location = new System.Drawing.Point(823, 43);
            this.MonthCmb.Name = "MonthCmb";
            this.MonthCmb.Size = new System.Drawing.Size(100, 21);
            this.MonthCmb.TabIndex = 1;
            // 
            // YearTxt
            // 
            this.YearTxt.Location = new System.Drawing.Point(823, 12);
            this.YearTxt.Name = "YearTxt";
            this.YearTxt.Size = new System.Drawing.Size(100, 20);
            this.YearTxt.TabIndex = 0;
            this.YearTxt.Leave += new System.EventHandler(this.YearTxt_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(773, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 151;
            this.label4.Text = "Month";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(773, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 151;
            this.label2.Text = "Year";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 257);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1176, 28);
            this.panel2.TabIndex = 154;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(181, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 17);
            this.label3.TabIndex = 157;
            this.label3.Text = "Shift";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 156;
            this.label1.Text = "Day";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.ManualRosterGridView);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 285);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1176, 359);
            this.panel3.TabIndex = 158;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(4, 321);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // ManualRosterGridView
            // 
            this.ManualRosterGridView.AllowUserToAddRows = false;
            this.ManualRosterGridView.AllowUserToDeleteRows = false;
            this.ManualRosterGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ManualRosterGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.ManualRosterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ManualRosterGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.ManualRosterGridView.Location = new System.Drawing.Point(0, 0);
            this.ManualRosterGridView.MultiSelect = false;
            this.ManualRosterGridView.Name = "ManualRosterGridView";
            this.ManualRosterGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ManualRosterGridView.Size = new System.Drawing.Size(1176, 317);
            this.ManualRosterGridView.TabIndex = 159;
            // 
            // MonthlyRosterSetupFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 644);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MonthlyRosterSetupFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MonthlyRosterSetupFrm";
            this.Load += new System.EventHandler(this.MonthlyRosterSetupFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MonthlyRosterSetupFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ManualRosterGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox departmentChkboxlist;
        private System.Windows.Forms.CheckBox DepartmentSelectAllChkBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox sectionSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox SectionChkboxlist;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox EmployeeSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox EmployeeChkBoxList;
        private System.Windows.Forms.Button btnRefreshRoster;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox MonthCmb;
        private System.Windows.Forms.TextBox YearTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView ManualRosterGridView;
    }
}