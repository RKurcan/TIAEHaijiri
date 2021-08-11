namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    partial class HolidaySetupFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HolidaySetupFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHolidayName = new System.Windows.Forms.TextBox();
            this.rtxtDescription = new System.Windows.Forms.RichTextBox();
            this.cmbHolidayType = new System.Windows.Forms.ComboBox();
            this.cmbApplicableGender = new System.Windows.Forms.ComboBox();
            this.cmbApplicableReligion = new System.Windows.Forms.ComboBox();
            this.chkSameDate = new System.Windows.Forms.CheckBox();
            this.holidayDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.holidayDetailsGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fiscalYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fiscalYearIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holidayDetailsVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbFiscalYear = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.holidayGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameNpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicableGenderNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicableReligionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holidayTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isOccuredInSameDateDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.applicableGenderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicableReligionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holidayGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.mtbFrom = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.mtbTo = new System.Windows.Forms.MaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.holidayDetailsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.holidayDetailsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holidayDetailsVmBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holidayGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holidayGridVmBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Holiday Name";
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
            this.menuStrip1.Size = new System.Drawing.Size(714, 24);
            this.menuStrip1.TabIndex = 16;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Holiday Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(333, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Applicable Religion";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(333, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Applicable Gender";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(333, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Occured On Same Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Description";
            // 
            // txtHolidayName
            // 
            this.txtHolidayName.Location = new System.Drawing.Point(96, 37);
            this.txtHolidayName.Name = "txtHolidayName";
            this.txtHolidayName.Size = new System.Drawing.Size(154, 20);
            this.txtHolidayName.TabIndex = 0;
            // 
            // rtxtDescription
            // 
            this.rtxtDescription.Location = new System.Drawing.Point(96, 94);
            this.rtxtDescription.Name = "rtxtDescription";
            this.rtxtDescription.Size = new System.Drawing.Size(214, 66);
            this.rtxtDescription.TabIndex = 4;
            this.rtxtDescription.Text = "";
            // 
            // cmbHolidayType
            // 
            this.cmbHolidayType.FormattingEnabled = true;
            this.cmbHolidayType.Items.AddRange(new object[] {
            "Religious",
            "Non Religious"});
            this.cmbHolidayType.Location = new System.Drawing.Point(96, 64);
            this.cmbHolidayType.Name = "cmbHolidayType";
            this.cmbHolidayType.Size = new System.Drawing.Size(154, 21);
            this.cmbHolidayType.TabIndex = 2;
            // 
            // cmbApplicableGender
            // 
            this.cmbApplicableGender.FormattingEnabled = true;
            this.cmbApplicableGender.Items.AddRange(new object[] {
            "All",
            "Male",
            "Female",
            "Others"});
            this.cmbApplicableGender.Location = new System.Drawing.Point(468, 64);
            this.cmbApplicableGender.Name = "cmbApplicableGender";
            this.cmbApplicableGender.Size = new System.Drawing.Size(137, 21);
            this.cmbApplicableGender.TabIndex = 3;
            // 
            // cmbApplicableReligion
            // 
            this.cmbApplicableReligion.FormattingEnabled = true;
            this.cmbApplicableReligion.Items.AddRange(new object[] {
            "All",
            "Hinduism",
            "Buddhism",
            "Islam",
            "Christianity",
            "Judaism"});
            this.cmbApplicableReligion.Location = new System.Drawing.Point(468, 37);
            this.cmbApplicableReligion.Name = "cmbApplicableReligion";
            this.cmbApplicableReligion.Size = new System.Drawing.Size(137, 21);
            this.cmbApplicableReligion.TabIndex = 1;
            // 
            // chkSameDate
            // 
            this.chkSameDate.AutoSize = true;
            this.chkSameDate.Location = new System.Drawing.Point(468, 96);
            this.chkSameDate.Name = "chkSameDate";
            this.chkSameDate.Size = new System.Drawing.Size(15, 14);
            this.chkSameDate.TabIndex = 5;
            this.chkSameDate.UseVisualStyleBackColor = true;
            // 
            // holidayDetailsGroupBox
            // 
            this.holidayDetailsGroupBox.Controls.Add(this.holidayDetailsGridView);
            this.holidayDetailsGroupBox.Location = new System.Drawing.Point(4, 157);
            this.holidayDetailsGroupBox.Name = "holidayDetailsGroupBox";
            this.holidayDetailsGroupBox.Size = new System.Drawing.Size(710, 98);
            this.holidayDetailsGroupBox.TabIndex = 25;
            this.holidayDetailsGroupBox.TabStop = false;
            this.holidayDetailsGroupBox.Text = "Holiday Details";
            // 
            // holidayDetailsGridView
            // 
            this.holidayDetailsGridView.AllowUserToAddRows = false;
            this.holidayDetailsGridView.AllowUserToDeleteRows = false;
            this.holidayDetailsGridView.AutoGenerateColumns = false;
            this.holidayDetailsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.holidayDetailsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.fiscalYearDataGridViewTextBoxColumn,
            this.fiscalYearIdDataGridViewTextBoxColumn,
            this.fromDataGridViewTextBoxColumn,
            this.toDataGridViewTextBoxColumn});
            this.holidayDetailsGridView.DataSource = this.holidayDetailsVmBindingSource;
            this.holidayDetailsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.holidayDetailsGridView.Location = new System.Drawing.Point(3, 16);
            this.holidayDetailsGridView.Name = "holidayDetailsGridView";
            this.holidayDetailsGridView.ReadOnly = true;
            this.holidayDetailsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.holidayDetailsGridView.Size = new System.Drawing.Size(704, 79);
            this.holidayDetailsGridView.TabIndex = 0;
            this.holidayDetailsGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.holidayDetailsGridView_CellContentClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // fiscalYearDataGridViewTextBoxColumn
            // 
            this.fiscalYearDataGridViewTextBoxColumn.DataPropertyName = "FiscalYear";
            this.fiscalYearDataGridViewTextBoxColumn.HeaderText = "Fiscal Year";
            this.fiscalYearDataGridViewTextBoxColumn.Name = "fiscalYearDataGridViewTextBoxColumn";
            this.fiscalYearDataGridViewTextBoxColumn.ReadOnly = true;
            this.fiscalYearDataGridViewTextBoxColumn.Width = 195;
            // 
            // fiscalYearIdDataGridViewTextBoxColumn
            // 
            this.fiscalYearIdDataGridViewTextBoxColumn.DataPropertyName = "FiscalYearId";
            this.fiscalYearIdDataGridViewTextBoxColumn.HeaderText = "FiscalYearId";
            this.fiscalYearIdDataGridViewTextBoxColumn.Name = "fiscalYearIdDataGridViewTextBoxColumn";
            this.fiscalYearIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.fiscalYearIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // fromDataGridViewTextBoxColumn
            // 
            this.fromDataGridViewTextBoxColumn.DataPropertyName = "From";
            this.fromDataGridViewTextBoxColumn.HeaderText = "From";
            this.fromDataGridViewTextBoxColumn.Name = "fromDataGridViewTextBoxColumn";
            this.fromDataGridViewTextBoxColumn.ReadOnly = true;
            this.fromDataGridViewTextBoxColumn.Width = 180;
            // 
            // toDataGridViewTextBoxColumn
            // 
            this.toDataGridViewTextBoxColumn.DataPropertyName = "To";
            this.toDataGridViewTextBoxColumn.HeaderText = "To";
            this.toDataGridViewTextBoxColumn.Name = "toDataGridViewTextBoxColumn";
            this.toDataGridViewTextBoxColumn.ReadOnly = true;
            this.toDataGridViewTextBoxColumn.Width = 180;
            // 
            // holidayDetailsVmBindingSource
            // 
            this.holidayDetailsVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.HolidayDetailsVm);
            // 
            // cmbFiscalYear
            // 
            this.cmbFiscalYear.FormattingEnabled = true;
            this.cmbFiscalYear.Location = new System.Drawing.Point(38, 256);
            this.cmbFiscalYear.Name = "cmbFiscalYear";
            this.cmbFiscalYear.Size = new System.Drawing.Size(169, 21);
            this.cmbFiscalYear.TabIndex = 6;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(592, 256);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(652, 256);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(53, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // holidayGridView
            // 
            this.holidayGridView.AllowUserToAddRows = false;
            this.holidayGridView.AllowUserToDeleteRows = false;
            this.holidayGridView.AutoGenerateColumns = false;
            this.holidayGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.holidayGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.nameDataGridViewTextBoxColumn,
            this.nameNpDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.applicableGenderNameDataGridViewTextBoxColumn,
            this.applicableReligionNameDataGridViewTextBoxColumn,
            this.holidayTypeDataGridViewTextBoxColumn,
            this.isOccuredInSameDateDataGridViewCheckBoxColumn,
            this.applicableGenderDataGridViewTextBoxColumn,
            this.applicableReligionDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn});
            this.holidayGridView.DataSource = this.holidayGridVmBindingSource;
            this.holidayGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.holidayGridView.Location = new System.Drawing.Point(0, 375);
            this.holidayGridView.Name = "holidayGridView";
            this.holidayGridView.ReadOnly = true;
            this.holidayGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.holidayGridView.Size = new System.Drawing.Size(714, 215);
            this.holidayGridView.TabIndex = 15;
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn1.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            this.idDataGridViewTextBoxColumn1.ReadOnly = true;
            this.idDataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Date";
            this.dataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 110;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 180;
            // 
            // nameNpDataGridViewTextBoxColumn
            // 
            this.nameNpDataGridViewTextBoxColumn.DataPropertyName = "NameNp";
            this.nameNpDataGridViewTextBoxColumn.HeaderText = "NameNp";
            this.nameNpDataGridViewTextBoxColumn.Name = "nameNpDataGridViewTextBoxColumn";
            this.nameNpDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameNpDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            // 
            // applicableGenderNameDataGridViewTextBoxColumn
            // 
            this.applicableGenderNameDataGridViewTextBoxColumn.DataPropertyName = "ApplicableGenderName";
            this.applicableGenderNameDataGridViewTextBoxColumn.HeaderText = "Applicable Gender";
            this.applicableGenderNameDataGridViewTextBoxColumn.Name = "applicableGenderNameDataGridViewTextBoxColumn";
            this.applicableGenderNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicableGenderNameDataGridViewTextBoxColumn.Width = 180;
            // 
            // applicableReligionNameDataGridViewTextBoxColumn
            // 
            this.applicableReligionNameDataGridViewTextBoxColumn.DataPropertyName = "ApplicableReligionName";
            this.applicableReligionNameDataGridViewTextBoxColumn.HeaderText = "Applicable Religion";
            this.applicableReligionNameDataGridViewTextBoxColumn.Name = "applicableReligionNameDataGridViewTextBoxColumn";
            this.applicableReligionNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicableReligionNameDataGridViewTextBoxColumn.Width = 180;
            // 
            // holidayTypeDataGridViewTextBoxColumn
            // 
            this.holidayTypeDataGridViewTextBoxColumn.DataPropertyName = "HolidayType";
            this.holidayTypeDataGridViewTextBoxColumn.HeaderText = "HolidayType";
            this.holidayTypeDataGridViewTextBoxColumn.Name = "holidayTypeDataGridViewTextBoxColumn";
            this.holidayTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.holidayTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // isOccuredInSameDateDataGridViewCheckBoxColumn
            // 
            this.isOccuredInSameDateDataGridViewCheckBoxColumn.DataPropertyName = "IsOccuredInSameDate";
            this.isOccuredInSameDateDataGridViewCheckBoxColumn.HeaderText = "IsOccuredInSameDate";
            this.isOccuredInSameDateDataGridViewCheckBoxColumn.Name = "isOccuredInSameDateDataGridViewCheckBoxColumn";
            this.isOccuredInSameDateDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isOccuredInSameDateDataGridViewCheckBoxColumn.Visible = false;
            // 
            // applicableGenderDataGridViewTextBoxColumn
            // 
            this.applicableGenderDataGridViewTextBoxColumn.DataPropertyName = "ApplicableGender";
            this.applicableGenderDataGridViewTextBoxColumn.HeaderText = "ApplicableGender";
            this.applicableGenderDataGridViewTextBoxColumn.Name = "applicableGenderDataGridViewTextBoxColumn";
            this.applicableGenderDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicableGenderDataGridViewTextBoxColumn.Visible = false;
            // 
            // applicableReligionDataGridViewTextBoxColumn
            // 
            this.applicableReligionDataGridViewTextBoxColumn.DataPropertyName = "ApplicableReligion";
            this.applicableReligionDataGridViewTextBoxColumn.HeaderText = "ApplicableReligion";
            this.applicableReligionDataGridViewTextBoxColumn.Name = "applicableReligionDataGridViewTextBoxColumn";
            this.applicableReligionDataGridViewTextBoxColumn.ReadOnly = true;
            this.applicableReligionDataGridViewTextBoxColumn.Visible = false;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // holidayGridVmBindingSource
            // 
            this.holidayGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.HolidayGridVm);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(638, 351);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 14;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(1, 351);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(631, 20);
            this.txtSearch.TabIndex = 13;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(321, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(233, 304);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // mtbFrom
            // 
            this.mtbFrom.Location = new System.Drawing.Point(223, 258);
            this.mtbFrom.Mask = "0000/00/00";
            this.mtbFrom.Name = "mtbFrom";
            this.mtbFrom.Size = new System.Drawing.Size(67, 20);
            this.mtbFrom.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(305, 260);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 75;
            this.label9.Text = "(YYYY/MM/DD)";
            // 
            // mtbTo
            // 
            this.mtbTo.Location = new System.Drawing.Point(407, 258);
            this.mtbTo.Mask = "0000/00/00";
            this.mtbTo.Name = "mtbTo";
            this.mtbTo.Size = new System.Drawing.Size(67, 20);
            this.mtbTo.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(485, 261);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 76;
            this.label7.Text = "(YYYY/MM/DD)";
            // 
            // HolidaySetupFrm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 590);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.mtbTo);
            this.Controls.Add(this.mtbFrom);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.holidayGridView);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cmbFiscalYear);
            this.Controls.Add(this.holidayDetailsGroupBox);
            this.Controls.Add(this.chkSameDate);
            this.Controls.Add(this.cmbApplicableReligion);
            this.Controls.Add(this.cmbApplicableGender);
            this.Controls.Add(this.cmbHolidayType);
            this.Controls.Add(this.rtxtDescription);
            this.Controls.Add(this.txtHolidayName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HolidaySetupFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Holiday";
            this.Load += new System.EventHandler(this.HolidaySetupFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HolidaySetupFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.holidayDetailsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.holidayDetailsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holidayDetailsVmBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holidayGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holidayGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHolidayName;
        private System.Windows.Forms.RichTextBox rtxtDescription;
        private System.Windows.Forms.ComboBox cmbHolidayType;
        private System.Windows.Forms.ComboBox cmbApplicableGender;
        private System.Windows.Forms.ComboBox cmbApplicableReligion;
        private System.Windows.Forms.CheckBox chkSameDate;
        private System.Windows.Forms.GroupBox holidayDetailsGroupBox;
        private System.Windows.Forms.DataGridView holidayDetailsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.ComboBox cmbFiscalYear;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.BindingSource holidayDetailsVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fiscalYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fiscalYearIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView holidayGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameNpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableGenderNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableReligionNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn holidayTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOccuredInSameDateDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableGenderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableReligionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource holidayGridVmBindingSource;
        private System.Windows.Forms.MaskedTextBox mtbFrom;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox mtbTo;
        private System.Windows.Forms.Label label7;
    }
}