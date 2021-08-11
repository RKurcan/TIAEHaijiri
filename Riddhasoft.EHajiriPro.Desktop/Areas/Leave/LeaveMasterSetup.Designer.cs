namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    partial class LeaveMasterSetup
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkIsReplacementLeave = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.DisablePanel = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.chkIsLeaveCarryable = new System.Windows.Forms.CheckBox();
            this.chkIsPaidLeave = new System.Windows.Forms.CheckBox();
            this.cmbApplicableGender = new System.Windows.Forms.ComboBox();
            this.cmbNumberOfDays = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.leaveMasterGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isPaidLeaveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isLeaveCarryableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.applicableGenderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicableGenderNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isReplacementLeaveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.leaveMasterGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchTxt = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.DisablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNumberOfDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveMasterGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveMasterGridVmBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(915, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_plus_48;
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.createToolStripMenuItem.Text = "Create";
            this.createToolStripMenuItem.Click += new System.EventHandler(this.createToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_edit_file_48;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_trash_48;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_cancel_48;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 78;
            this.label1.Text = "Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "Leave Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(496, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Is Replacement Leave";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(112, 35);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(118, 20);
            this.txtCode.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(345, 34);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(132, 20);
            this.txtName.TabIndex = 1;
            // 
            // chkIsReplacementLeave
            // 
            this.chkIsReplacementLeave.AutoSize = true;
            this.chkIsReplacementLeave.Location = new System.Drawing.Point(613, 38);
            this.chkIsReplacementLeave.Name = "chkIsReplacementLeave";
            this.chkIsReplacementLeave.Size = new System.Drawing.Size(15, 14);
            this.chkIsReplacementLeave.TabIndex = 2;
            this.chkIsReplacementLeave.UseVisualStyleBackColor = true;
            this.chkIsReplacementLeave.CheckedChanged += new System.EventHandler(this.chkIsReplacementLeave_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(112, 198);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(196, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DisablePanel
            // 
            this.DisablePanel.Controls.Add(this.label11);
            this.DisablePanel.Controls.Add(this.txtDescription);
            this.DisablePanel.Controls.Add(this.chkIsLeaveCarryable);
            this.DisablePanel.Controls.Add(this.chkIsPaidLeave);
            this.DisablePanel.Controls.Add(this.cmbApplicableGender);
            this.DisablePanel.Controls.Add(this.cmbNumberOfDays);
            this.DisablePanel.Controls.Add(this.label6);
            this.DisablePanel.Controls.Add(this.label8);
            this.DisablePanel.Controls.Add(this.label7);
            this.DisablePanel.Controls.Add(this.label5);
            this.DisablePanel.Controls.Add(this.label3);
            this.DisablePanel.Location = new System.Drawing.Point(0, 60);
            this.DisablePanel.Name = "DisablePanel";
            this.DisablePanel.Size = new System.Drawing.Size(631, 130);
            this.DisablePanel.TabIndex = 92;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(479, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 17);
            this.label11.TabIndex = 112;
            this.label11.Text = "*";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(112, 65);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(159, 61);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.Text = "";
            // 
            // chkIsLeaveCarryable
            // 
            this.chkIsLeaveCarryable.AutoSize = true;
            this.chkIsLeaveCarryable.Location = new System.Drawing.Point(613, 9);
            this.chkIsLeaveCarryable.Name = "chkIsLeaveCarryable";
            this.chkIsLeaveCarryable.Size = new System.Drawing.Size(15, 14);
            this.chkIsLeaveCarryable.TabIndex = 2;
            this.chkIsLeaveCarryable.UseVisualStyleBackColor = true;
            // 
            // chkIsPaidLeave
            // 
            this.chkIsPaidLeave.AutoSize = true;
            this.chkIsPaidLeave.Location = new System.Drawing.Point(112, 40);
            this.chkIsPaidLeave.Name = "chkIsPaidLeave";
            this.chkIsPaidLeave.Size = new System.Drawing.Size(15, 14);
            this.chkIsPaidLeave.TabIndex = 3;
            this.chkIsPaidLeave.UseVisualStyleBackColor = true;
            // 
            // cmbApplicableGender
            // 
            this.cmbApplicableGender.FormattingEnabled = true;
            this.cmbApplicableGender.Items.AddRange(new object[] {
            "All",
            "Male",
            "Female",
            "Others"});
            this.cmbApplicableGender.Location = new System.Drawing.Point(112, 6);
            this.cmbApplicableGender.Name = "cmbApplicableGender";
            this.cmbApplicableGender.Size = new System.Drawing.Size(118, 21);
            this.cmbApplicableGender.TabIndex = 0;
            // 
            // cmbNumberOfDays
            // 
            this.cmbNumberOfDays.Location = new System.Drawing.Point(346, 6);
            this.cmbNumberOfDays.Name = "cmbNumberOfDays";
            this.cmbNumberOfDays.Size = new System.Drawing.Size(132, 20);
            this.cmbNumberOfDays.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 91;
            this.label6.Text = "Description";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(497, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Is Leave Carryable";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 93;
            this.label7.Text = "Is Paid Leave";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 94;
            this.label5.Text = "Applicable Gender";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 90;
            this.label3.Text = "Number Of Days";
            // 
            // leaveMasterGridView
            // 
            this.leaveMasterGridView.AllowUserToAddRows = false;
            this.leaveMasterGridView.AllowUserToDeleteRows = false;
            this.leaveMasterGridView.AutoGenerateColumns = false;
            this.leaveMasterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.leaveMasterGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.balanceDataGridViewTextBoxColumn,
            this.isPaidLeaveDataGridViewCheckBoxColumn,
            this.isLeaveCarryableDataGridViewCheckBoxColumn,
            this.applicableGenderDataGridViewTextBoxColumn,
            this.applicableGenderNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.createdOnDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.isReplacementLeaveDataGridViewCheckBoxColumn});
            this.leaveMasterGridView.DataSource = this.leaveMasterGridVmBindingSource;
            this.leaveMasterGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.leaveMasterGridView.Location = new System.Drawing.Point(0, 264);
            this.leaveMasterGridView.Name = "leaveMasterGridView";
            this.leaveMasterGridView.ReadOnly = true;
            this.leaveMasterGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.leaveMasterGridView.Size = new System.Drawing.Size(915, 215);
            this.leaveMasterGridView.TabIndex = 7;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 150;
            // 
            // balanceDataGridViewTextBoxColumn
            // 
            this.balanceDataGridViewTextBoxColumn.DataPropertyName = "Balance";
            this.balanceDataGridViewTextBoxColumn.HeaderText = "Balance";
            this.balanceDataGridViewTextBoxColumn.Name = "balanceDataGridViewTextBoxColumn";
            this.balanceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isPaidLeaveDataGridViewCheckBoxColumn
            // 
            this.isPaidLeaveDataGridViewCheckBoxColumn.DataPropertyName = "IsPaidLeave";
            this.isPaidLeaveDataGridViewCheckBoxColumn.HeaderText = "Is Paid Leave";
            this.isPaidLeaveDataGridViewCheckBoxColumn.Name = "isPaidLeaveDataGridViewCheckBoxColumn";
            this.isPaidLeaveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isPaidLeaveDataGridViewCheckBoxColumn.Width = 110;
            // 
            // isLeaveCarryableDataGridViewCheckBoxColumn
            // 
            this.isLeaveCarryableDataGridViewCheckBoxColumn.DataPropertyName = "IsLeaveCarryable";
            this.isLeaveCarryableDataGridViewCheckBoxColumn.HeaderText = "Is Leave Carryable";
            this.isLeaveCarryableDataGridViewCheckBoxColumn.Name = "isLeaveCarryableDataGridViewCheckBoxColumn";
            this.isLeaveCarryableDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isLeaveCarryableDataGridViewCheckBoxColumn.Width = 130;
            // 
            // applicableGenderDataGridViewTextBoxColumn
            // 
            this.applicableGenderDataGridViewTextBoxColumn.DataPropertyName = "ApplicableGender";
            this.applicableGenderDataGridViewTextBoxColumn.HeaderText = "ApplicableGender";
            this.applicableGenderDataGridViewTextBoxColumn.Name = "applicableGenderDataGridViewTextBoxColumn";
            this.applicableGenderDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicableGenderDataGridViewTextBoxColumn.Visible = false;
            // 
            // applicableGenderNameDataGridViewTextBoxColumn
            // 
            this.applicableGenderNameDataGridViewTextBoxColumn.DataPropertyName = "ApplicableGenderName";
            this.applicableGenderNameDataGridViewTextBoxColumn.HeaderText = "Applicable Gender";
            this.applicableGenderNameDataGridViewTextBoxColumn.Name = "applicableGenderNameDataGridViewTextBoxColumn";
            this.applicableGenderNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicableGenderNameDataGridViewTextBoxColumn.Width = 130;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdOnDataGridViewTextBoxColumn
            // 
            this.createdOnDataGridViewTextBoxColumn.DataPropertyName = "CreatedOn";
            this.createdOnDataGridViewTextBoxColumn.HeaderText = "CreatedOn";
            this.createdOnDataGridViewTextBoxColumn.Name = "createdOnDataGridViewTextBoxColumn";
            this.createdOnDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdOnDataGridViewTextBoxColumn.Visible = false;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // isReplacementLeaveDataGridViewCheckBoxColumn
            // 
            this.isReplacementLeaveDataGridViewCheckBoxColumn.DataPropertyName = "IsReplacementLeave";
            this.isReplacementLeaveDataGridViewCheckBoxColumn.HeaderText = "Is Replacement Leave";
            this.isReplacementLeaveDataGridViewCheckBoxColumn.Name = "isReplacementLeaveDataGridViewCheckBoxColumn";
            this.isReplacementLeaveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isReplacementLeaveDataGridViewCheckBoxColumn.Width = 130;
            // 
            // leaveMasterGridVmBindingSource
            // 
            this.leaveMasterGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.LeaveMasterGridVm);
            // 
            // searchTxt
            // 
            this.searchTxt.Location = new System.Drawing.Point(1, 240);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(832, 20);
            this.searchTxt.TabIndex = 5;
            this.searchTxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTxt_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(839, 238);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(234, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 17);
            this.label10.TabIndex = 112;
            this.label10.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(479, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 17);
            this.label4.TabIndex = 113;
            this.label4.Text = "*";
            // 
            // LeaveMasterSetup
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 479);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.searchTxt);
            this.Controls.Add(this.leaveMasterGridView);
            this.Controls.Add(this.DisablePanel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkIsReplacementLeave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LeaveMasterSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Leave Master";
            this.Load += new System.EventHandler(this.LeaveMasterSetup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LeaveMasterSetup_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.DisablePanel.ResumeLayout(false);
            this.DisablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNumberOfDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveMasterGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveMasterGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkIsReplacementLeave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel DisablePanel;
        private System.Windows.Forms.RichTextBox txtDescription;
        private System.Windows.Forms.CheckBox chkIsLeaveCarryable;
        private System.Windows.Forms.CheckBox chkIsPaidLeave;
        private System.Windows.Forms.ComboBox cmbApplicableGender;
        private System.Windows.Forms.NumericUpDown cmbNumberOfDays;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView leaveMasterGridView;
        private System.Windows.Forms.TextBox searchTxt;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPaidLeaveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLeaveCarryableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableGenderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableGenderNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isReplacementLeaveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource leaveMasterGridVmBindingSource;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
    }
}