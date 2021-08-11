namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    partial class OfficeVisitApprovalFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfficeVisitApprovalFrm));
            this.officeVisitApprovalGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedByIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.approvedOnDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isApproveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.employeeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.officeVisitGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rtxtRemark = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tmpFromTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.employeePictureBox = new System.Windows.Forms.PictureBox();
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
            this.label3 = new System.Windows.Forms.Label();
            this.tmpToTime = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnReject = new System.Windows.Forms.Button();
            this.mtbFromDate = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mtbToDate = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitApprovalGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitGridVmBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // officeVisitApprovalGridView
            // 
            this.officeVisitApprovalGridView.AllowUserToAddRows = false;
            this.officeVisitApprovalGridView.AllowUserToDeleteRows = false;
            this.officeVisitApprovalGridView.AutoGenerateColumns = false;
            this.officeVisitApprovalGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.officeVisitApprovalGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.employeeNameDataGridViewTextBoxColumn,
            this.fromDataGridViewTextBoxColumn,
            this.toDataGridViewTextBoxColumn,
            this.remarkDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.approvedByIdDataGridViewTextBoxColumn,
            this.approvedOnDataGridViewTextBoxColumn,
            this.isApproveDataGridViewCheckBoxColumn,
            this.employeeIdDataGridViewTextBoxColumn});
            this.officeVisitApprovalGridView.DataSource = this.officeVisitGridVmBindingSource;
            this.officeVisitApprovalGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.officeVisitApprovalGridView.Location = new System.Drawing.Point(0, 361);
            this.officeVisitApprovalGridView.Name = "officeVisitApprovalGridView";
            this.officeVisitApprovalGridView.ReadOnly = true;
            this.officeVisitApprovalGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.officeVisitApprovalGridView.Size = new System.Drawing.Size(710, 235);
            this.officeVisitApprovalGridView.TabIndex = 0;
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
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 150;
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
            // isApproveDataGridViewCheckBoxColumn
            // 
            this.isApproveDataGridViewCheckBoxColumn.DataPropertyName = "IsApprove";
            this.isApproveDataGridViewCheckBoxColumn.HeaderText = "IsApprove";
            this.isApproveDataGridViewCheckBoxColumn.Name = "isApproveDataGridViewCheckBoxColumn";
            this.isApproveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isApproveDataGridViewCheckBoxColumn.Visible = false;
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(710, 24);
            this.menuStrip1.TabIndex = 4;
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
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(126, 294);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 1;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(283, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rtxtRemark
            // 
            this.rtxtRemark.Location = new System.Drawing.Point(126, 227);
            this.rtxtRemark.Name = "rtxtRemark";
            this.rtxtRemark.Size = new System.Drawing.Size(231, 61);
            this.rtxtRemark.TabIndex = 139;
            this.rtxtRemark.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 157;
            this.label8.Text = "To";
            // 
            // tmpFromTime
            // 
            this.tmpFromTime.CustomFormat = "HH:mm";
            this.tmpFromTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpFromTime.Location = new System.Drawing.Point(303, 171);
            this.tmpFromTime.Name = "tmpFromTime";
            this.tmpFromTime.ShowUpDown = true;
            this.tmpFromTime.Size = new System.Drawing.Size(54, 20);
            this.tmpFromTime.TabIndex = 136;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 155;
            this.label5.Text = "From";
            // 
            // employeePictureBox
            // 
            this.employeePictureBox.Location = new System.Drawing.Point(381, 35);
            this.employeePictureBox.Name = "employeePictureBox";
            this.employeePictureBox.Size = new System.Drawing.Size(97, 98);
            this.employeePictureBox.TabIndex = 154;
            this.employeePictureBox.TabStop = false;
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(126, 143);
            this.txtSection.Name = "txtSection";
            this.txtSection.ReadOnly = true;
            this.txtSection.Size = new System.Drawing.Size(231, 20);
            this.txtSection.TabIndex = 147;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(126, 117);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(231, 20);
            this.txtDepartment.TabIndex = 146;
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(126, 89);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.ReadOnly = true;
            this.txtDesignation.Size = new System.Drawing.Size(231, 20);
            this.txtDesignation.TabIndex = 145;
            // 
            // txtEmployeeCode
            // 
            this.txtEmployeeCode.Location = new System.Drawing.Point(126, 63);
            this.txtEmployeeCode.Name = "txtEmployeeCode";
            this.txtEmployeeCode.Size = new System.Drawing.Size(231, 20);
            this.txtEmployeeCode.TabIndex = 144;
            this.txtEmployeeCode.Leave += new System.EventHandler(this.txtEmployeeCode_Leave);
            // 
            // txtEmployeeName
            // 
            this.txtEmployeeName.Location = new System.Drawing.Point(126, 36);
            this.txtEmployeeName.Name = "txtEmployeeName";
            this.txtEmployeeName.ReadOnly = true;
            this.txtEmployeeName.Size = new System.Drawing.Size(231, 20);
            this.txtEmployeeName.TabIndex = 153;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 152;
            this.label9.Text = "Section";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 151;
            this.label10.Text = "Department";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 150;
            this.label11.Text = "Designation";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 149;
            this.label12.Text = "Employee Code";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 148;
            this.label13.Text = "Employee Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 143;
            this.label3.Text = "Remark";
            // 
            // tmpToTime
            // 
            this.tmpToTime.CustomFormat = "HH:mm";
            this.tmpToTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmpToTime.Location = new System.Drawing.Point(303, 198);
            this.tmpToTime.Name = "tmpToTime";
            this.tmpToTime.ShowUpDown = true;
            this.tmpToTime.Size = new System.Drawing.Size(54, 20);
            this.tmpToTime.TabIndex = 138;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(634, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 159;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(1, 338);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(625, 20);
            this.txtSearch.TabIndex = 158;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(205, 294);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 2;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // mtbFromDate
            // 
            this.mtbFromDate.Location = new System.Drawing.Point(126, 169);
            this.mtbFromDate.Mask = "0000/00/00";
            this.mtbFromDate.Name = "mtbFromDate";
            this.mtbFromDate.Size = new System.Drawing.Size(67, 20);
            this.mtbFromDate.TabIndex = 165;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(207, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 166;
            this.label2.Text = "( YYYY/MM/DD )";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(194, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 167;
            this.label7.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(194, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 17);
            this.label1.TabIndex = 168;
            this.label1.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(207, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 169;
            this.label4.Text = "( YYYY/MM/DD )";
            // 
            // mtbToDate
            // 
            this.mtbToDate.Location = new System.Drawing.Point(126, 196);
            this.mtbToDate.Mask = "0000/00/00";
            this.mtbToDate.Name = "mtbToDate";
            this.mtbToDate.Size = new System.Drawing.Size(67, 20);
            this.mtbToDate.TabIndex = 165;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(361, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 17);
            this.label6.TabIndex = 171;
            this.label6.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(361, 175);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 17);
            this.label14.TabIndex = 170;
            this.label14.Text = "*";
            // 
            // OfficeVisitApprovalFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 596);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mtbToDate);
            this.Controls.Add(this.mtbFromDate);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.rtxtRemark);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tmpFromTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.employeePictureBox);
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
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tmpToTime);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.officeVisitApprovalGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OfficeVisitApprovalFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Office Visit Approve";
            this.Load += new System.EventHandler(this.OfficeVisitApprovalFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OfficeVisitApprovalFrm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitApprovalGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.officeVisitGridVmBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView officeVisitApprovalGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RichTextBox rtxtRemark;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker tmpFromTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox employeePictureBox;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker tmpToTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedByIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn approvedOnDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isApproveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource officeVisitGridVmBindingSource;
        private System.Windows.Forms.MaskedTextBox mtbFromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox mtbToDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label14;
    }
}