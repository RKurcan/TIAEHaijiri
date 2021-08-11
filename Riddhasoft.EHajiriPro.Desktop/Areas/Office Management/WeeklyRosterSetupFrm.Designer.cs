namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    partial class WeeklyRosterSetupFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeeklyRosterSetupFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DepartmentSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.departmentChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sectionSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.SectionChkboxlist = new System.Windows.Forms.CheckedListBox();
            this.btnRefreshRoster = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EmployeeSelectAllChkBox = new System.Windows.Forms.CheckBox();
            this.EmployeeChkBoxList = new System.Windows.Forms.CheckedListBox();
            this.weeklyRosterGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weeklyRosterGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_cancel_48;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.departmentChkboxlist);
            this.groupBox1.Controls.Add(this.DepartmentSelectAllChkBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 216);
            this.groupBox1.TabIndex = 133;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Department";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sectionSelectAllChkBox);
            this.groupBox2.Controls.Add(this.SectionChkboxlist);
            this.groupBox2.Location = new System.Drawing.Point(264, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 216);
            this.groupBox2.TabIndex = 134;
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
            // btnRefreshRoster
            // 
            this.btnRefreshRoster.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRefreshRoster.Location = new System.Drawing.Point(784, 41);
            this.btnRefreshRoster.Name = "btnRefreshRoster";
            this.btnRefreshRoster.Size = new System.Drawing.Size(90, 74);
            this.btnRefreshRoster.TabIndex = 0;
            this.btnRefreshRoster.Text = "Refresh Roster";
            this.btnRefreshRoster.UseVisualStyleBackColor = true;
            this.btnRefreshRoster.Click += new System.EventHandler(this.btnRefreshRoster_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EmployeeSelectAllChkBox);
            this.groupBox3.Controls.Add(this.EmployeeChkBoxList);
            this.groupBox3.Location = new System.Drawing.Point(521, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 216);
            this.groupBox3.TabIndex = 141;
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
            // weeklyRosterGridView
            // 
            this.weeklyRosterGridView.AllowUserToAddRows = false;
            this.weeklyRosterGridView.AllowUserToDeleteRows = false;
            this.weeklyRosterGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.weeklyRosterGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.weeklyRosterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.weeklyRosterGridView.Location = new System.Drawing.Point(13, 296);
            this.weeklyRosterGridView.MultiSelect = false;
            this.weeklyRosterGridView.Name = "weeklyRosterGridView";
            this.weeklyRosterGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.weeklyRosterGridView.Size = new System.Drawing.Size(1026, 166);
            this.weeklyRosterGridView.TabIndex = 138;
            this.weeklyRosterGridView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.weeklyRosterGridView_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(83, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 139;
            this.label1.Text = "Day";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(208, 278);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 17);
            this.label3.TabIndex = 142;
            this.label3.Text = "Shift";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 471);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // WeeklyRosterSetupFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 495);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.weeklyRosterGridView);
            this.Controls.Add(this.btnRefreshRoster);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WeeklyRosterSetupFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weekly Roster ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WeeklyRosterSetupFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weeklyRosterGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.CheckBox DepartmentSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox departmentChkboxlist;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox SectionChkboxlist;
        private System.Windows.Forms.CheckBox sectionSelectAllChkBox;
        private System.Windows.Forms.Button btnRefreshRoster;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox EmployeeSelectAllChkBox;
        private System.Windows.Forms.CheckedListBox EmployeeChkBoxList;
        private System.Windows.Forms.DataGridView weeklyRosterGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
    }
}