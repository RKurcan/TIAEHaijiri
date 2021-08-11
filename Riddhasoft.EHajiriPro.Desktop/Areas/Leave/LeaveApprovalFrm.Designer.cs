namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    partial class LeaveApprovalFrm
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
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.leaveApprovalGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveMasterNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalLeaveDayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveStatusNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveMasterIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedByIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approveByUserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveDayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveDayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveApplicationGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.rtxtDescription = new System.Windows.Forms.RichTextBox();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.txtEmployeeCode = new System.Windows.Forms.TextBox();
            this.txtEmployeeName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLeaveType = new System.Windows.Forms.TextBox();
            this.txtLeaveDay = new System.Windows.Forms.TextBox();
            this.txtApproveBy = new System.Windows.Forms.TextBox();
            this.btnRevert = new System.Windows.Forms.Button();
            this.employeePictureBox = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.MtbFrom = new System.Windows.Forms.TextBox();
            this.mtbTo = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApprovalGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApplicationGridVmBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(822, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.View;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_cancel_48;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(745, 351);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 21);
            this.button3.TabIndex = 16;
            this.button3.Text = "Search";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2, 352);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(737, 20);
            this.txtSearch.TabIndex = 15;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // leaveApprovalGridView
            // 
            this.leaveApprovalGridView.AllowUserToAddRows = false;
            this.leaveApprovalGridView.AllowUserToDeleteRows = false;
            this.leaveApprovalGridView.AutoGenerateColumns = false;
            this.leaveApprovalGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.leaveApprovalGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.employeeIdDataGridViewTextBoxColumn,
            this.employeeNameDataGridViewTextBoxColumn,
            this.leaveMasterNameDataGridViewTextBoxColumn,
            this.fromDataGridViewTextBoxColumn,
            this.toDataGridViewTextBoxColumn,
            this.totalLeaveDayDataGridViewTextBoxColumn,
            this.leaveStatusNameDataGridViewTextBoxColumn,
            this.leaveMasterIdDataGridViewTextBoxColumn,
            this.createdByIdDataGridViewTextBoxColumn,
            this.transactionDateDataGridViewTextBoxColumn,
            this.leaveStatusDataGridViewTextBoxColumn,
            this.approvedByIdDataGridViewTextBoxColumn,
            this.approveByUserDataGridViewTextBoxColumn,
            this.approvedOnDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.leaveDayDataGridViewTextBoxColumn,
            this.leaveDayNameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.leaveApprovalGridView.DataSource = this.leaveApplicationGridVmBindingSource;
            this.leaveApprovalGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.leaveApprovalGridView.Location = new System.Drawing.Point(0, 377);
            this.leaveApprovalGridView.Name = "leaveApprovalGridView";
            this.leaveApprovalGridView.ReadOnly = true;
            this.leaveApprovalGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.leaveApprovalGridView.Size = new System.Drawing.Size(822, 273);
            this.leaveApprovalGridView.TabIndex = 17;
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
            this.employeeNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // leaveMasterNameDataGridViewTextBoxColumn
            // 
            this.leaveMasterNameDataGridViewTextBoxColumn.DataPropertyName = "LeaveMasterName";
            this.leaveMasterNameDataGridViewTextBoxColumn.HeaderText = "Leave Type";
            this.leaveMasterNameDataGridViewTextBoxColumn.Name = "leaveMasterNameDataGridViewTextBoxColumn";
            this.leaveMasterNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.leaveMasterNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // fromDataGridViewTextBoxColumn
            // 
            this.fromDataGridViewTextBoxColumn.DataPropertyName = "From";
            this.fromDataGridViewTextBoxColumn.HeaderText = "From";
            this.fromDataGridViewTextBoxColumn.Name = "fromDataGridViewTextBoxColumn";
            this.fromDataGridViewTextBoxColumn.ReadOnly = true;
            this.fromDataGridViewTextBoxColumn.Width = 130;
            // 
            // toDataGridViewTextBoxColumn
            // 
            this.toDataGridViewTextBoxColumn.DataPropertyName = "To";
            this.toDataGridViewTextBoxColumn.HeaderText = "To";
            this.toDataGridViewTextBoxColumn.Name = "toDataGridViewTextBoxColumn";
            this.toDataGridViewTextBoxColumn.ReadOnly = true;
            this.toDataGridViewTextBoxColumn.Width = 130;
            // 
            // totalLeaveDayDataGridViewTextBoxColumn
            // 
            this.totalLeaveDayDataGridViewTextBoxColumn.DataPropertyName = "TotalLeaveDay";
            this.totalLeaveDayDataGridViewTextBoxColumn.HeaderText = "Days";
            this.totalLeaveDayDataGridViewTextBoxColumn.Name = "totalLeaveDayDataGridViewTextBoxColumn";
            this.totalLeaveDayDataGridViewTextBoxColumn.ReadOnly = true;
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
            // approveByUserDataGridViewTextBoxColumn
            // 
            this.approveByUserDataGridViewTextBoxColumn.DataPropertyName = "ApproveByUser";
            this.approveByUserDataGridViewTextBoxColumn.HeaderText = "ApproveByUser";
            this.approveByUserDataGridViewTextBoxColumn.Name = "approveByUserDataGridViewTextBoxColumn";
            this.approveByUserDataGridViewTextBoxColumn.ReadOnly = true;
            this.approveByUserDataGridViewTextBoxColumn.Visible = false;
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
            // leaveDayNameDataGridViewTextBoxColumn
            // 
            this.leaveDayNameDataGridViewTextBoxColumn.DataPropertyName = "LeaveDayName";
            this.leaveDayNameDataGridViewTextBoxColumn.HeaderText = "LeaveDayName";
            this.leaveDayNameDataGridViewTextBoxColumn.Name = "leaveDayNameDataGridViewTextBoxColumn";
            this.leaveDayNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.leaveDayNameDataGridViewTextBoxColumn.Visible = false;
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
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(379, 238);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 139;
            this.label14.Text = "Approve By";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(379, 209);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 136;
            this.label11.Text = "To";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(379, 183);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 135;
            this.label10.Text = "From";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(379, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 133;
            this.label9.Text = "Leave Day";
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(187, 313);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(47, 23);
            this.btnReject.TabIndex = 12;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(129, 313);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(55, 23);
            this.btnApprove.TabIndex = 11;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // rtxtDescription
            // 
            this.rtxtDescription.Location = new System.Drawing.Point(129, 210);
            this.rtxtDescription.Name = "rtxtDescription";
            this.rtxtDescription.ReadOnly = true;
            this.rtxtDescription.Size = new System.Drawing.Size(210, 96);
            this.rtxtDescription.TabIndex = 6;
            this.rtxtDescription.Text = "";
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(129, 146);
            this.txtSection.Name = "txtSection";
            this.txtSection.ReadOnly = true;
            this.txtSection.Size = new System.Drawing.Size(210, 20);
            this.txtSection.TabIndex = 4;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(129, 120);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(210, 20);
            this.txtDepartment.TabIndex = 3;
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(129, 92);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.ReadOnly = true;
            this.txtDesignation.Size = new System.Drawing.Size(210, 20);
            this.txtDesignation.TabIndex = 2;
            // 
            // txtEmployeeCode
            // 
            this.txtEmployeeCode.Location = new System.Drawing.Point(129, 66);
            this.txtEmployeeCode.Name = "txtEmployeeCode";
            this.txtEmployeeCode.ReadOnly = true;
            this.txtEmployeeCode.Size = new System.Drawing.Size(210, 20);
            this.txtEmployeeCode.TabIndex = 1;
            // 
            // txtEmployeeName
            // 
            this.txtEmployeeName.Location = new System.Drawing.Point(129, 39);
            this.txtEmployeeName.Name = "txtEmployeeName";
            this.txtEmployeeName.ReadOnly = true;
            this.txtEmployeeName.Size = new System.Drawing.Size(210, 20);
            this.txtEmployeeName.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 120;
            this.label8.Text = "Description";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 118;
            this.label6.Text = "Leave Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 117;
            this.label5.Text = "Section";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 116;
            this.label4.Text = "Department";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 115;
            this.label3.Text = "Designation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 114;
            this.label2.Text = "Employee Code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 113;
            this.label1.Text = "Employee Name";
            // 
            // txtLeaveType
            // 
            this.txtLeaveType.Location = new System.Drawing.Point(129, 174);
            this.txtLeaveType.Name = "txtLeaveType";
            this.txtLeaveType.ReadOnly = true;
            this.txtLeaveType.Size = new System.Drawing.Size(210, 20);
            this.txtLeaveType.TabIndex = 5;
            // 
            // txtLeaveDay
            // 
            this.txtLeaveDay.Location = new System.Drawing.Point(479, 145);
            this.txtLeaveDay.Name = "txtLeaveDay";
            this.txtLeaveDay.ReadOnly = true;
            this.txtLeaveDay.Size = new System.Drawing.Size(92, 20);
            this.txtLeaveDay.TabIndex = 7;
            // 
            // txtApproveBy
            // 
            this.txtApproveBy.Location = new System.Drawing.Point(479, 234);
            this.txtApproveBy.Name = "txtApproveBy";
            this.txtApproveBy.ReadOnly = true;
            this.txtApproveBy.Size = new System.Drawing.Size(92, 20);
            this.txtApproveBy.TabIndex = 10;
            // 
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(236, 313);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(47, 23);
            this.btnRevert.TabIndex = 13;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // employeePictureBox
            // 
            this.employeePictureBox.Location = new System.Drawing.Point(382, 39);
            this.employeePictureBox.Name = "employeePictureBox";
            this.employeePictureBox.Size = new System.Drawing.Size(97, 98);
            this.employeePictureBox.TabIndex = 128;
            this.employeePictureBox.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(289, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MtbFrom
            // 
            this.MtbFrom.Location = new System.Drawing.Point(479, 177);
            this.MtbFrom.Name = "MtbFrom";
            this.MtbFrom.ReadOnly = true;
            this.MtbFrom.Size = new System.Drawing.Size(92, 20);
            this.MtbFrom.TabIndex = 140;
            // 
            // mtbTo
            // 
            this.mtbTo.Location = new System.Drawing.Point(479, 205);
            this.mtbTo.Name = "mtbTo";
            this.mtbTo.ReadOnly = true;
            this.mtbTo.Size = new System.Drawing.Size(92, 20);
            this.mtbTo.TabIndex = 140;
            // 
            // LeaveApprovalFrm
            // 
            this.AcceptButton = this.btnApprove;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 650);
            this.Controls.Add(this.mtbTo);
            this.Controls.Add(this.MtbFrom);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.txtApproveBy);
            this.Controls.Add(this.txtLeaveDay);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.rtxtDescription);
            this.Controls.Add(this.employeePictureBox);
            this.Controls.Add(this.txtLeaveType);
            this.Controls.Add(this.txtSection);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.txtDesignation);
            this.Controls.Add(this.txtEmployeeCode);
            this.Controls.Add(this.txtEmployeeName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.leaveApprovalGridView);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LeaveApprovalFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Leave Approval";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LeaveApprovalFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApprovalGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveApplicationGridVmBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView leaveApprovalGridView;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.RichTextBox rtxtDescription;
        private System.Windows.Forms.PictureBox employeePictureBox;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.TextBox txtEmployeeCode;
        private System.Windows.Forms.TextBox txtEmployeeName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource leaveApplicationGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveMasterNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalLeaveDayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveStatusNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveMasterIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedByIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approveByUserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveDayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveDayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txtLeaveType;
        private System.Windows.Forms.TextBox txtLeaveDay;
        private System.Windows.Forms.TextBox txtApproveBy;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox MtbFrom;
        private System.Windows.Forms.TextBox mtbTo;
    }
}