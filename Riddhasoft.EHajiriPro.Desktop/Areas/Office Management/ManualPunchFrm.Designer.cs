namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    partial class ManualPunchFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualPunchFrm));
            this.manualPunchGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.manualPunchGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.txtEmployeeCode = new System.Windows.Forms.TextBox();
            this.txtEmployeeName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.employeePictureBox = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mtbTIme = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.rtxtRemark = new System.Windows.Forms.RichTextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.mtbDate = new System.Windows.Forms.MaskedTextBox();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.manualPunchGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.manualPunchGridVmBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // manualPunchGridView
            // 
            this.manualPunchGridView.AllowUserToAddRows = false;
            this.manualPunchGridView.AllowUserToDeleteRows = false;
            this.manualPunchGridView.AutoGenerateColumns = false;
            this.manualPunchGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.manualPunchGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.employeeDataGridViewTextBoxColumn,
            this.dateTimeDataGridViewTextBoxColumn,
            this.remarkDataGridViewTextBoxColumn,
            this.employeeIdDataGridViewTextBoxColumn,
            this.companyIdDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn});
            this.manualPunchGridView.DataSource = this.manualPunchGridVmBindingSource;
            this.manualPunchGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.manualPunchGridView.Location = new System.Drawing.Point(0, 348);
            this.manualPunchGridView.Name = "manualPunchGridView";
            this.manualPunchGridView.ReadOnly = true;
            this.manualPunchGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.manualPunchGridView.Size = new System.Drawing.Size(727, 215);
            this.manualPunchGridView.TabIndex = 9;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // employeeDataGridViewTextBoxColumn
            // 
            this.employeeDataGridViewTextBoxColumn.DataPropertyName = "Employee";
            this.employeeDataGridViewTextBoxColumn.HeaderText = "Employee";
            this.employeeDataGridViewTextBoxColumn.Name = "employeeDataGridViewTextBoxColumn";
            this.employeeDataGridViewTextBoxColumn.ReadOnly = true;
            this.employeeDataGridViewTextBoxColumn.Width = 200;
            // 
            // dateTimeDataGridViewTextBoxColumn
            // 
            this.dateTimeDataGridViewTextBoxColumn.DataPropertyName = "DateTime";
            this.dateTimeDataGridViewTextBoxColumn.HeaderText = "Date Time";
            this.dateTimeDataGridViewTextBoxColumn.Name = "dateTimeDataGridViewTextBoxColumn";
            this.dateTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.dateTimeDataGridViewTextBoxColumn.Width = 170;
            // 
            // remarkDataGridViewTextBoxColumn
            // 
            this.remarkDataGridViewTextBoxColumn.DataPropertyName = "Remark";
            this.remarkDataGridViewTextBoxColumn.HeaderText = "Remark";
            this.remarkDataGridViewTextBoxColumn.Name = "remarkDataGridViewTextBoxColumn";
            this.remarkDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarkDataGridViewTextBoxColumn.Width = 300;
            // 
            // employeeIdDataGridViewTextBoxColumn
            // 
            this.employeeIdDataGridViewTextBoxColumn.DataPropertyName = "EmployeeId";
            this.employeeIdDataGridViewTextBoxColumn.HeaderText = "EmployeeId";
            this.employeeIdDataGridViewTextBoxColumn.Name = "employeeIdDataGridViewTextBoxColumn";
            this.employeeIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.employeeIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // companyIdDataGridViewTextBoxColumn
            // 
            this.companyIdDataGridViewTextBoxColumn.DataPropertyName = "CompanyId";
            this.companyIdDataGridViewTextBoxColumn.HeaderText = "CompanyId";
            this.companyIdDataGridViewTextBoxColumn.Name = "companyIdDataGridViewTextBoxColumn";
            this.companyIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.companyIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // manualPunchGridVmBindingSource
            // 
            this.manualPunchGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.ManualPunchGridVm);
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
            this.menuStrip1.Size = new System.Drawing.Size(727, 24);
            this.menuStrip1.TabIndex = 10;
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
            // btnSearch
            // 
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.search3;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(311, 29);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 22);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(123, 138);
            this.txtSection.Name = "txtSection";
            this.txtSection.ReadOnly = true;
            this.txtSection.Size = new System.Drawing.Size(245, 20);
            this.txtSection.TabIndex = 92;
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(123, 112);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(245, 20);
            this.txtDepartment.TabIndex = 91;
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(123, 84);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.ReadOnly = true;
            this.txtDesignation.Size = new System.Drawing.Size(245, 20);
            this.txtDesignation.TabIndex = 90;
            // 
            // txtEmployeeCode
            // 
            this.txtEmployeeCode.Location = new System.Drawing.Point(123, 58);
            this.txtEmployeeCode.Name = "txtEmployeeCode";
            this.txtEmployeeCode.Size = new System.Drawing.Size(245, 20);
            this.txtEmployeeCode.TabIndex = 1;
            this.txtEmployeeCode.Leave += new System.EventHandler(this.txtEmployeeCode_Leave);
            // 
            // txtEmployeeName
            // 
            this.txtEmployeeName.Location = new System.Drawing.Point(123, 31);
            this.txtEmployeeName.Name = "txtEmployeeName";
            this.txtEmployeeName.ReadOnly = true;
            this.txtEmployeeName.Size = new System.Drawing.Size(184, 20);
            this.txtEmployeeName.TabIndex = 98;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 97;
            this.label5.Text = "Section";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 96;
            this.label4.Text = "Department";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 95;
            this.label3.Text = "Designation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 94;
            this.label2.Text = "Employee Code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 93;
            this.label1.Text = "Employee Name";
            // 
            // employeePictureBox
            // 
            this.employeePictureBox.Location = new System.Drawing.Point(393, 30);
            this.employeePictureBox.Name = "employeePictureBox";
            this.employeePictureBox.Size = new System.Drawing.Size(97, 98);
            this.employeePictureBox.TabIndex = 99;
            this.employeePictureBox.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 100;
            this.label6.Text = "Date Time";
            // 
            // mtbTIme
            // 
            this.mtbTIme.CustomFormat = "HH:mm";
            this.mtbTIme.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.mtbTIme.Location = new System.Drawing.Point(306, 166);
            this.mtbTIme.Name = "mtbTIme";
            this.mtbTIme.ShowUpDown = true;
            this.mtbTIme.Size = new System.Drawing.Size(62, 20);
            this.mtbTIme.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 196);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 104;
            this.label8.Text = "Remark";
            // 
            // rtxtRemark
            // 
            this.rtxtRemark.Location = new System.Drawing.Point(123, 196);
            this.rtxtRemark.Name = "rtxtRemark";
            this.rtxtRemark.Size = new System.Drawing.Size(245, 61);
            this.rtxtRemark.TabIndex = 4;
            this.rtxtRemark.Text = "";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(123, 263);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(204, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(1, 324);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(645, 20);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(649, 323);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 8;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(194, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 17);
            this.label9.TabIndex = 110;
            this.label9.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(372, 219);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 17);
            this.label10.TabIndex = 111;
            this.label10.Text = "*";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(372, 170);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 17);
            this.label11.TabIndex = 112;
            this.label11.Text = "*";
            // 
            // mtbDate
            // 
            this.mtbDate.Location = new System.Drawing.Point(123, 166);
            this.mtbDate.Mask = "0000/00/00";
            this.mtbDate.Name = "mtbDate";
            this.mtbDate.Size = new System.Drawing.Size(67, 20);
            this.mtbDate.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(205, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 165;
            this.label12.Text = "( YYYY/MM/DD )";
            // 
            // ManualPunchFrm
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 563);
            this.Controls.Add(this.mtbDate);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.rtxtRemark);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.mtbTIme);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.employeePictureBox);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSection);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.txtDesignation);
            this.Controls.Add(this.txtEmployeeCode);
            this.Controls.Add(this.txtEmployeeName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.manualPunchGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ManualPunchFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manual Punch";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ManualPunchFrm_FormClosed);
            this.Load += new System.EventHandler(this.ManualPunchFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManualPunchFrm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.manualPunchGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.manualPunchGridVmBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView manualPunchGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.TextBox txtEmployeeCode;
        private System.Windows.Forms.TextBox txtEmployeeName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox employeePictureBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker mtbTIme;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox rtxtRemark;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource manualPunchGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.MaskedTextBox mtbDate;
        private System.Windows.Forms.Label label12;
    }
}