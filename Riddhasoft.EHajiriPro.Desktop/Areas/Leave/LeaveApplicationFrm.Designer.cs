namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    partial class LeaveApplicationFrm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEmployeeName = new System.Windows.Forms.TextBox();
            this.txtEmployeeCode = new System.Windows.Forms.TextBox();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.cmbLeaveType = new System.Windows.Forms.ComboBox();
            this.employeePictureBox = new System.Windows.Forms.PictureBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtRemaningLeave = new System.Windows.Forms.TextBox();
            this.rtxtDescription = new System.Windows.Forms.RichTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbLeaveDay = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbApproveBy = new System.Windows.Forms.ComboBox();
            this.leaveApplicationGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveMasterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalLeaveDayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveDayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approveByUserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveStatusNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveMasterIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedByIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveDayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveApplicationGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.MtbFrom = new System.Windows.Forms.MaskedTextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.mtbTo = new System.Windows.Forms.MaskedTextBox();
            this.lblToDatePlaceHolder = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApplicationGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApplicationGridVmBindingSource)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 13;
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
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 78;
            this.label1.Text = "Employee Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "Employee Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 80;
            this.label3.Text = "Designation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 81;
            this.label4.Text = "Department";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 82;
            this.label5.Text = "Section";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 83;
            this.label6.Text = "Leave Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 84;
            this.label7.Text = "Remaning Leave";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 84;
            this.label8.Text = "Description";
            // 
            // txtEmployeeName
            // 
            this.txtEmployeeName.Location = new System.Drawing.Point(129, 39);
            this.txtEmployeeName.Name = "txtEmployeeName";
            this.txtEmployeeName.ReadOnly = true;
            this.txtEmployeeName.Size = new System.Drawing.Size(151, 20);
            this.txtEmployeeName.TabIndex = 85;
            // 
            // txtEmployeeCode
            // 
            this.txtEmployeeCode.Location = new System.Drawing.Point(129, 66);
            this.txtEmployeeCode.Name = "txtEmployeeCode";
            this.txtEmployeeCode.Size = new System.Drawing.Size(210, 20);
            this.txtEmployeeCode.TabIndex = 1;
            this.txtEmployeeCode.Leave += new System.EventHandler(this.txtEmployeeCode_Leave);
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(129, 92);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.ReadOnly = true;
            this.txtDesignation.Size = new System.Drawing.Size(210, 20);
            this.txtDesignation.TabIndex = 2;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(129, 120);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(210, 20);
            this.txtDepartment.TabIndex = 3;
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(129, 146);
            this.txtSection.Name = "txtSection";
            this.txtSection.ReadOnly = true;
            this.txtSection.Size = new System.Drawing.Size(210, 20);
            this.txtSection.TabIndex = 4;
            // 
            // cmbLeaveType
            // 
            this.cmbLeaveType.FormattingEnabled = true;
            this.cmbLeaveType.Location = new System.Drawing.Point(129, 177);
            this.cmbLeaveType.Name = "cmbLeaveType";
            this.cmbLeaveType.Size = new System.Drawing.Size(210, 21);
            this.cmbLeaveType.TabIndex = 2;
            this.cmbLeaveType.SelectedIndexChanged += new System.EventHandler(this.cmbLeaveType_SelectedIndexChanged);
            // 
            // employeePictureBox
            // 
            this.employeePictureBox.Location = new System.Drawing.Point(382, 39);
            this.employeePictureBox.Name = "employeePictureBox";
            this.employeePictureBox.Size = new System.Drawing.Size(97, 98);
            this.employeePictureBox.TabIndex = 92;
            this.employeePictureBox.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.search3;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(282, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 22);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtRemaningLeave
            // 
            this.txtRemaningLeave.Location = new System.Drawing.Point(129, 204);
            this.txtRemaningLeave.Name = "txtRemaningLeave";
            this.txtRemaningLeave.ReadOnly = true;
            this.txtRemaningLeave.Size = new System.Drawing.Size(210, 20);
            this.txtRemaningLeave.TabIndex = 6;
            // 
            // rtxtDescription
            // 
            this.rtxtDescription.Location = new System.Drawing.Point(129, 235);
            this.rtxtDescription.Name = "rtxtDescription";
            this.rtxtDescription.Size = new System.Drawing.Size(210, 96);
            this.rtxtDescription.TabIndex = 3;
            this.rtxtDescription.Text = "";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(129, 338);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(210, 338);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(379, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 97;
            this.label9.Text = "Leave Day";
            // 
            // cmbLeaveDay
            // 
            this.cmbLeaveDay.FormattingEnabled = true;
            this.cmbLeaveDay.Items.AddRange(new object[] {
            "Full Day",
            "Early Leave",
            "Late Leave"});
            this.cmbLeaveDay.Location = new System.Drawing.Point(479, 144);
            this.cmbLeaveDay.Name = "cmbLeaveDay";
            this.cmbLeaveDay.Size = new System.Drawing.Size(105, 21);
            this.cmbLeaveDay.TabIndex = 4;
            this.cmbLeaveDay.SelectedIndexChanged += new System.EventHandler(this.cmbLeaveDay_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(379, 183);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 99;
            this.label10.Text = "From";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(379, 209);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 100;
            this.lblTo.Text = "To";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(379, 238);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 105;
            this.label14.Text = "Approve By";
            // 
            // cmbApproveBy
            // 
            this.cmbApproveBy.FormattingEnabled = true;
            this.cmbApproveBy.Items.AddRange(new object[] {
            "Full Day",
            "Early Leave",
            "Late Leave"});
            this.cmbApproveBy.Location = new System.Drawing.Point(479, 232);
            this.cmbApproveBy.Name = "cmbApproveBy";
            this.cmbApproveBy.Size = new System.Drawing.Size(105, 21);
            this.cmbApproveBy.TabIndex = 7;
            // 
            // leaveApplicationGridView
            // 
            this.leaveApplicationGridView.AllowUserToAddRows = false;
            this.leaveApplicationGridView.AllowUserToDeleteRows = false;
            this.leaveApplicationGridView.AutoGenerateColumns = false;
            this.leaveApplicationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.leaveApplicationGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.employeeIdDataGridViewTextBoxColumn,
            this.employeeNameDataGridViewTextBoxColumn,
            this.leaveMasterNameDataGridViewTextBoxColumn,
            this.fromDataGridViewTextBoxColumn,
            this.toDataGridViewTextBoxColumn,
            this.totalLeaveDayDataGridViewTextBoxColumn,
            this.leaveDayNameDataGridViewTextBoxColumn,
            this.approveByUserDataGridViewTextBoxColumn,
            this.leaveStatusNameDataGridViewTextBoxColumn,
            this.leaveMasterIdDataGridViewTextBoxColumn,
            this.createdByIdDataGridViewTextBoxColumn,
            this.transactionDateDataGridViewTextBoxColumn,
            this.leaveStatusDataGridViewTextBoxColumn,
            this.approvedByIdDataGridViewTextBoxColumn,
            this.approvedOnDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.leaveDayDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.leaveApplicationGridView.DataSource = this.leaveApplicationGridVmBindingSource;
            this.leaveApplicationGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.leaveApplicationGridView.Location = new System.Drawing.Point(0, 415);
            this.leaveApplicationGridView.Name = "leaveApplicationGridView";
            this.leaveApplicationGridView.ReadOnly = true;
            this.leaveApplicationGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.leaveApplicationGridView.Size = new System.Drawing.Size(883, 273);
            this.leaveApplicationGridView.TabIndex = 12;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // employeeIdDataGridViewTextBoxColumn
            // 
            this.employeeIdDataGridViewTextBoxColumn.DataPropertyName = "EmployeeId";
            this.employeeIdDataGridViewTextBoxColumn.HeaderText = "EmployeeId";
            this.employeeIdDataGridViewTextBoxColumn.Name = "employeeIdDataGridViewTextBoxColumn";
            this.employeeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.employeeIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // employeeNameDataGridViewTextBoxColumn
            // 
            this.employeeNameDataGridViewTextBoxColumn.DataPropertyName = "EmployeeName";
            this.employeeNameDataGridViewTextBoxColumn.HeaderText = "Employee";
            this.employeeNameDataGridViewTextBoxColumn.Name = "employeeNameDataGridViewTextBoxColumn";
            this.employeeNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.employeeNameDataGridViewTextBoxColumn.Width = 130;
            // 
            // leaveMasterNameDataGridViewTextBoxColumn
            // 
            this.leaveMasterNameDataGridViewTextBoxColumn.DataPropertyName = "LeaveMasterName";
            this.leaveMasterNameDataGridViewTextBoxColumn.HeaderText = "Leave Type";
            this.leaveMasterNameDataGridViewTextBoxColumn.Name = "leaveMasterNameDataGridViewTextBoxColumn";
            this.leaveMasterNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.leaveMasterNameDataGridViewTextBoxColumn.Width = 130;
            // 
            // fromDataGridViewTextBoxColumn
            // 
            this.fromDataGridViewTextBoxColumn.DataPropertyName = "From";
            this.fromDataGridViewTextBoxColumn.HeaderText = "From";
            this.fromDataGridViewTextBoxColumn.Name = "fromDataGridViewTextBoxColumn";
            this.fromDataGridViewTextBoxColumn.ReadOnly = true;
            this.fromDataGridViewTextBoxColumn.Width = 80;
            // 
            // toDataGridViewTextBoxColumn
            // 
            this.toDataGridViewTextBoxColumn.DataPropertyName = "To";
            this.toDataGridViewTextBoxColumn.HeaderText = "To";
            this.toDataGridViewTextBoxColumn.Name = "toDataGridViewTextBoxColumn";
            this.toDataGridViewTextBoxColumn.ReadOnly = true;
            this.toDataGridViewTextBoxColumn.Width = 80;
            // 
            // totalLeaveDayDataGridViewTextBoxColumn
            // 
            this.totalLeaveDayDataGridViewTextBoxColumn.DataPropertyName = "TotalLeaveDay";
            this.totalLeaveDayDataGridViewTextBoxColumn.HeaderText = "Days";
            this.totalLeaveDayDataGridViewTextBoxColumn.Name = "totalLeaveDayDataGridViewTextBoxColumn";
            this.totalLeaveDayDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalLeaveDayDataGridViewTextBoxColumn.Width = 80;
            // 
            // leaveDayNameDataGridViewTextBoxColumn
            // 
            this.leaveDayNameDataGridViewTextBoxColumn.DataPropertyName = "LeaveDayName";
            this.leaveDayNameDataGridViewTextBoxColumn.HeaderText = "Leave Day";
            this.leaveDayNameDataGridViewTextBoxColumn.Name = "leaveDayNameDataGridViewTextBoxColumn";
            this.leaveDayNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // approveByUserDataGridViewTextBoxColumn
            // 
            this.approveByUserDataGridViewTextBoxColumn.DataPropertyName = "ApproveByUser";
            this.approveByUserDataGridViewTextBoxColumn.HeaderText = "Approved By";
            this.approveByUserDataGridViewTextBoxColumn.Name = "approveByUserDataGridViewTextBoxColumn";
            this.approveByUserDataGridViewTextBoxColumn.ReadOnly = true;
            this.approveByUserDataGridViewTextBoxColumn.Width = 120;
            // 
            // leaveStatusNameDataGridViewTextBoxColumn
            // 
            this.leaveStatusNameDataGridViewTextBoxColumn.DataPropertyName = "LeaveStatusName";
            this.leaveStatusNameDataGridViewTextBoxColumn.HeaderText = "Status";
            this.leaveStatusNameDataGridViewTextBoxColumn.Name = "leaveStatusNameDataGridViewTextBoxColumn";
            this.leaveStatusNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // leaveMasterIdDataGridViewTextBoxColumn
            // 
            this.leaveMasterIdDataGridViewTextBoxColumn.DataPropertyName = "LeaveMasterId";
            this.leaveMasterIdDataGridViewTextBoxColumn.HeaderText = "LeaveMasterId";
            this.leaveMasterIdDataGridViewTextBoxColumn.Name = "leaveMasterIdDataGridViewTextBoxColumn";
            this.leaveMasterIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.leaveMasterIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // createdByIdDataGridViewTextBoxColumn
            // 
            this.createdByIdDataGridViewTextBoxColumn.DataPropertyName = "CreatedById";
            this.createdByIdDataGridViewTextBoxColumn.HeaderText = "CreatedById";
            this.createdByIdDataGridViewTextBoxColumn.Name = "createdByIdDataGridViewTextBoxColumn";
            this.createdByIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdByIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // transactionDateDataGridViewTextBoxColumn
            // 
            this.transactionDateDataGridViewTextBoxColumn.DataPropertyName = "TransactionDate";
            this.transactionDateDataGridViewTextBoxColumn.HeaderText = "TransactionDate";
            this.transactionDateDataGridViewTextBoxColumn.Name = "transactionDateDataGridViewTextBoxColumn";
            this.transactionDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // leaveStatusDataGridViewTextBoxColumn
            // 
            this.leaveStatusDataGridViewTextBoxColumn.DataPropertyName = "LeaveStatus";
            this.leaveStatusDataGridViewTextBoxColumn.HeaderText = "LeaveStatus";
            this.leaveStatusDataGridViewTextBoxColumn.Name = "leaveStatusDataGridViewTextBoxColumn";
            this.leaveStatusDataGridViewTextBoxColumn.ReadOnly = true;
            this.leaveStatusDataGridViewTextBoxColumn.Visible = false;
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
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // leaveDayDataGridViewTextBoxColumn
            // 
            this.leaveDayDataGridViewTextBoxColumn.DataPropertyName = "LeaveDay";
            this.leaveDayDataGridViewTextBoxColumn.HeaderText = "LeaveDay";
            this.leaveDayDataGridViewTextBoxColumn.Name = "leaveDayDataGridViewTextBoxColumn";
            this.leaveDayDataGridViewTextBoxColumn.ReadOnly = true;
            this.leaveDayDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            // 
            // leaveApplicationGridVmBindingSource
            // 
            this.leaveApplicationGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.LeaveApplicationGridVm);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(808, 389);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 21);
            this.button3.TabIndex = 11;
            this.button3.Text = "Search";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2, 390);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(800, 20);
            this.txtSearch.TabIndex = 10;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(345, 183);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 17);
            this.label15.TabIndex = 112;
            this.label15.Text = "*";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(345, 273);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(13, 17);
            this.label16.TabIndex = 113;
            this.label16.Text = "*";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(552, 181);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(13, 17);
            this.label17.TabIndex = 114;
            this.label17.Text = "*";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(552, 211);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(13, 17);
            this.label18.TabIndex = 115;
            this.label18.Text = "*";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(590, 238);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(13, 17);
            this.label19.TabIndex = 116;
            this.label19.Text = "*";
            // 
            // MtbFrom
            // 
            this.MtbFrom.Location = new System.Drawing.Point(479, 176);
            this.MtbFrom.Mask = "0000/00/00";
            this.MtbFrom.Name = "MtbFrom";
            this.MtbFrom.Size = new System.Drawing.Size(67, 20);
            this.MtbFrom.TabIndex = 5;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Location = new System.Drawing.Point(563, 181);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(91, 13);
            this.label20.TabIndex = 165;
            this.label20.Text = "( YYYY/MM/DD )";
            // 
            // mtbTo
            // 
            this.mtbTo.Location = new System.Drawing.Point(479, 206);
            this.mtbTo.Mask = "0000/00/00";
            this.mtbTo.Name = "mtbTo";
            this.mtbTo.Size = new System.Drawing.Size(67, 20);
            this.mtbTo.TabIndex = 6;
            // 
            // lblToDatePlaceHolder
            // 
            this.lblToDatePlaceHolder.AutoSize = true;
            this.lblToDatePlaceHolder.BackColor = System.Drawing.Color.Transparent;
            this.lblToDatePlaceHolder.Location = new System.Drawing.Point(563, 209);
            this.lblToDatePlaceHolder.Name = "lblToDatePlaceHolder";
            this.lblToDatePlaceHolder.Size = new System.Drawing.Size(91, 13);
            this.lblToDatePlaceHolder.TabIndex = 168;
            this.lblToDatePlaceHolder.Text = "( YYYY/MM/DD )";
            // 
            // LeaveApplicationFrm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 688);
            this.Controls.Add(this.lblToDatePlaceHolder);
            this.Controls.Add(this.mtbTo);
            this.Controls.Add(this.MtbFrom);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.leaveApplicationGridView);
            this.Controls.Add(this.cmbApproveBy);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbLeaveDay);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.rtxtDescription);
            this.Controls.Add(this.txtRemaningLeave);
            this.Controls.Add(this.employeePictureBox);
            this.Controls.Add(this.cmbLeaveType);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSection);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.txtDesignation);
            this.Controls.Add(this.txtEmployeeCode);
            this.Controls.Add(this.txtEmployeeName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LeaveApplicationFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Leave Application";
            this.Load += new System.EventHandler(this.LeaveApplicationFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LeaveApplicationFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApplicationGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApplicationGridVmBindingSource)).EndInit();
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEmployeeName;
        private System.Windows.Forms.TextBox txtEmployeeCode;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbLeaveType;
        private System.Windows.Forms.PictureBox employeePictureBox;
        private System.Windows.Forms.TextBox txtRemaningLeave;
        private System.Windows.Forms.RichTextBox rtxtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbLeaveDay;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbApproveBy;
        private System.Windows.Forms.DataGridView leaveApplicationGridView;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.BindingSource leaveApplicationGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveMasterNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalLeaveDayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveDayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approveByUserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveStatusNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveMasterIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedByIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveDayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.MaskedTextBox MtbFrom;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.MaskedTextBox mtbTo;
        private System.Windows.Forms.Label lblToDatePlaceHolder;
    }
}