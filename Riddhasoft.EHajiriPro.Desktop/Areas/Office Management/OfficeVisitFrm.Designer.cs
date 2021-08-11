namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    partial class OfficeVisitFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfficeVisitFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmpToTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.officeVisitGridView = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.rtxtRemark = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tmpFromTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.employeePictureBox = new System.Windows.Forms.PictureBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.txtEmployeeCode = new System.Windows.Forms.TextBox();
            this.txtEmployeeName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.mtbFromDate = new System.Windows.Forms.MaskedTextBox();
            this.mtbToDate = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isApproveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedByIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.officeVisitGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitGridVmBindingSource)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(705, 24);
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
            // tmpToTime
            // 
            this.tmpToTime.CustomFormat = "HH:mm";
            this.tmpToTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpToTime.Location = new System.Drawing.Point(303, 192);
            this.tmpToTime.Name = "tmpToTime";
            this.tmpToTime.ShowUpDown = true;
            this.tmpToTime.Size = new System.Drawing.Size(62, 20);
            this.tmpToTime.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 111;
            this.label3.Text = "Remark";
            // 
            // officeVisitGridView
            // 
            this.officeVisitGridView.AllowUserToAddRows = false;
            this.officeVisitGridView.AllowUserToDeleteRows = false;
            this.officeVisitGridView.AutoGenerateColumns = false;
            this.officeVisitGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.officeVisitGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.employeeNameDataGridViewTextBoxColumn,
            this.fromDataGridViewTextBoxColumn,
            this.toDataGridViewTextBoxColumn,
            this.remarkDataGridViewTextBoxColumn,
            this.isApproveDataGridViewCheckBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.approvedByIdDataGridViewTextBoxColumn,
            this.approvedOnDataGridViewTextBoxColumn,
            this.employeeIdDataGridViewTextBoxColumn});
            this.officeVisitGridView.DataSource = this.officeVisitGridVmBindingSource;
            this.officeVisitGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.officeVisitGridView.Location = new System.Drawing.Point(0, 354);
            this.officeVisitGridView.Name = "officeVisitGridView";
            this.officeVisitGridView.ReadOnly = true;
            this.officeVisitGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.officeVisitGridView.Size = new System.Drawing.Size(705, 225);
            this.officeVisitGridView.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 327);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 14;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(1, 328);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(622, 20);
            this.txtSearch.TabIndex = 13;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // rtxtRemark
            // 
            this.rtxtRemark.Location = new System.Drawing.Point(123, 218);
            this.rtxtRemark.Name = "rtxtRemark";
            this.rtxtRemark.Size = new System.Drawing.Size(242, 61);
            this.rtxtRemark.TabIndex = 10;
            this.rtxtRemark.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 133;
            this.label8.Text = "To";
            // 
            // tmpFromTime
            // 
            this.tmpFromTime.CustomFormat = "HH:mm";
            this.tmpFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpFromTime.Location = new System.Drawing.Point(303, 162);
            this.tmpFromTime.Name = "tmpFromTime";
            this.tmpFromTime.ShowUpDown = true;
            this.tmpFromTime.Size = new System.Drawing.Size(62, 20);
            this.tmpFromTime.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(206, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 131;
            this.label4.Text = "( YYYY/MM/DD )";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 129;
            this.label5.Text = "From";
            // 
            // employeePictureBox
            // 
            this.employeePictureBox.Location = new System.Drawing.Point(402, 26);
            this.employeePictureBox.Name = "employeePictureBox";
            this.employeePictureBox.Size = new System.Drawing.Size(97, 98);
            this.employeePictureBox.TabIndex = 128;
            this.employeePictureBox.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.search3;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(308, 26);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 22);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(123, 134);
            this.txtSection.Name = "txtSection";
            this.txtSection.ReadOnly = true;
            this.txtSection.Size = new System.Drawing.Size(242, 20);
            this.txtSection.TabIndex = 5;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(123, 108);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(242, 20);
            this.txtDepartment.TabIndex = 4;
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(123, 80);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.ReadOnly = true;
            this.txtDesignation.Size = new System.Drawing.Size(242, 20);
            this.txtDesignation.TabIndex = 3;
            // 
            // txtEmployeeCode
            // 
            this.txtEmployeeCode.Location = new System.Drawing.Point(123, 54);
            this.txtEmployeeCode.Name = "txtEmployeeCode";
            this.txtEmployeeCode.Size = new System.Drawing.Size(242, 20);
            this.txtEmployeeCode.TabIndex = 2;
            this.txtEmployeeCode.Leave += new System.EventHandler(this.txtEmployeeCode_Leave);
            // 
            // txtEmployeeName
            // 
            this.txtEmployeeName.Location = new System.Drawing.Point(123, 27);
            this.txtEmployeeName.Name = "txtEmployeeName";
            this.txtEmployeeName.ReadOnly = true;
            this.txtEmployeeName.Size = new System.Drawing.Size(182, 20);
            this.txtEmployeeName.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 126;
            this.label9.Text = "Section";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 125;
            this.label10.Text = "Department";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 124;
            this.label11.Text = "Designation";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 123;
            this.label12.Text = "Employee Code";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 122;
            this.label13.Text = "Employee Name";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(123, 285);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 11;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(204, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(191, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 17);
            this.label6.TabIndex = 134;
            this.label6.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(191, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 17);
            this.label2.TabIndex = 135;
            this.label2.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(371, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 136;
            this.label7.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(371, 195);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 17);
            this.label14.TabIndex = 137;
            this.label14.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(371, 238);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 17);
            this.label15.TabIndex = 138;
            this.label15.Text = "*";
            // 
            // mtbFromDate
            // 
            this.mtbFromDate.Location = new System.Drawing.Point(122, 164);
            this.mtbFromDate.Mask = "0000/00/00";
            this.mtbFromDate.Name = "mtbFromDate";
            this.mtbFromDate.Size = new System.Drawing.Size(67, 20);
            this.mtbFromDate.TabIndex = 6;
            // 
            // mtbToDate
            // 
            this.mtbToDate.Location = new System.Drawing.Point(122, 190);
            this.mtbToDate.Mask = "0000/00/00";
            this.mtbToDate.Name = "mtbToDate";
            this.mtbToDate.Size = new System.Drawing.Size(67, 20);
            this.mtbToDate.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(206, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 166;
            this.label1.Text = "( YYYY/MM/DD )";
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // employeeNameDataGridViewTextBoxColumn
            // 
            this.employeeNameDataGridViewTextBoxColumn.DataPropertyName = "EmployeeName";
            this.employeeNameDataGridViewTextBoxColumn.HeaderText = "Employee";
            this.employeeNameDataGridViewTextBoxColumn.Name = "employeeNameDataGridViewTextBoxColumn";
            this.employeeNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.employeeNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // fromDataGridViewTextBoxColumn
            // 
            this.fromDataGridViewTextBoxColumn.DataPropertyName = "From";
            this.fromDataGridViewTextBoxColumn.HeaderText = "From";
            this.fromDataGridViewTextBoxColumn.Name = "fromDataGridViewTextBoxColumn";
            this.fromDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // toDataGridViewTextBoxColumn
            // 
            this.toDataGridViewTextBoxColumn.DataPropertyName = "To";
            this.toDataGridViewTextBoxColumn.HeaderText = "To";
            this.toDataGridViewTextBoxColumn.Name = "toDataGridViewTextBoxColumn";
            this.toDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remarkDataGridViewTextBoxColumn
            // 
            this.remarkDataGridViewTextBoxColumn.DataPropertyName = "Remark";
            this.remarkDataGridViewTextBoxColumn.HeaderText = "Remark";
            this.remarkDataGridViewTextBoxColumn.Name = "remarkDataGridViewTextBoxColumn";
            this.remarkDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarkDataGridViewTextBoxColumn.Width = 150;
            // 
            // isApproveDataGridViewCheckBoxColumn
            // 
            this.isApproveDataGridViewCheckBoxColumn.DataPropertyName = "IsApprove";
            this.isApproveDataGridViewCheckBoxColumn.HeaderText = "Is Approved ?";
            this.isApproveDataGridViewCheckBoxColumn.Name = "isApproveDataGridViewCheckBoxColumn";
            this.isApproveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isApproveDataGridViewCheckBoxColumn.Width = 150;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // approvedByIdDataGridViewTextBoxColumn
            // 
            this.approvedByIdDataGridViewTextBoxColumn.DataPropertyName = "ApprovedById";
            this.approvedByIdDataGridViewTextBoxColumn.HeaderText = "ApprovedById";
            this.approvedByIdDataGridViewTextBoxColumn.Name = "approvedByIdDataGridViewTextBoxColumn";
            this.approvedByIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.approvedByIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // approvedOnDataGridViewTextBoxColumn
            // 
            this.approvedOnDataGridViewTextBoxColumn.DataPropertyName = "ApprovedOn";
            this.approvedOnDataGridViewTextBoxColumn.HeaderText = "ApprovedOn";
            this.approvedOnDataGridViewTextBoxColumn.Name = "approvedOnDataGridViewTextBoxColumn";
            this.approvedOnDataGridViewTextBoxColumn.ReadOnly = true;
            this.approvedOnDataGridViewTextBoxColumn.Visible = false;
            // 
            // employeeIdDataGridViewTextBoxColumn
            // 
            this.employeeIdDataGridViewTextBoxColumn.DataPropertyName = "EmployeeId";
            this.employeeIdDataGridViewTextBoxColumn.HeaderText = "EmployeeId";
            this.employeeIdDataGridViewTextBoxColumn.Name = "employeeIdDataGridViewTextBoxColumn";
            this.employeeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.employeeIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // officeVisitGridVmBindingSource
            // 
            this.officeVisitGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.OfficeVisitGridVm);
            // 
            // OfficeVisitFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 579);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mtbToDate);
            this.Controls.Add(this.mtbFromDate);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.rtxtRemark);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tmpFromTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.employeePictureBox);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSection);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.txtDesignation);
            this.Controls.Add(this.txtEmployeeCode);
            this.Controls.Add(this.txtEmployeeName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.officeVisitGridView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tmpToTime);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OfficeVisitFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Office Visit";
            this.Load += new System.EventHandler(this.OfficeVisitFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OfficeVisitFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DateTimePicker tmpToTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView officeVisitGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.RichTextBox rtxtRemark;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker tmpFromTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox employeePictureBox;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.TextBox txtEmployeeCode;
        private System.Windows.Forms.TextBox txtEmployeeName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isApproveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedByIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource officeVisitGridVmBindingSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.MaskedTextBox mtbFromDate;
        private System.Windows.Forms.MaskedTextBox mtbToDate;
        private System.Windows.Forms.Label label1;
    }
}