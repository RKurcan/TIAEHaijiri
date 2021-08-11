namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    partial class EmployeeSearchFrm
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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.employeeSearchGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iDCardNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.departmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.designationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeSearchVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.employeeSearchGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeSearchVmBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(0, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(591, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // employeeSearchGridView
            // 
            this.employeeSearchGridView.AllowUserToAddRows = false;
            this.employeeSearchGridView.AllowUserToDeleteRows = false;
            this.employeeSearchGridView.AutoGenerateColumns = false;
            this.employeeSearchGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.employeeSearchGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.iDCardNoDataGridViewTextBoxColumn,
            this.departmentDataGridViewTextBoxColumn,
            this.mobileDataGridViewTextBoxColumn,
            this.designationDataGridViewTextBoxColumn});
            this.employeeSearchGridView.DataSource = this.employeeSearchVmBindingSource;
            this.employeeSearchGridView.Location = new System.Drawing.Point(0, 28);
            this.employeeSearchGridView.Name = "employeeSearchGridView";
            this.employeeSearchGridView.ReadOnly = true;
            this.employeeSearchGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.employeeSearchGridView.Size = new System.Drawing.Size(659, 250);
            this.employeeSearchGridView.TabIndex = 2;
            this.employeeSearchGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.employeeSearchGridView_CellContentClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Device Code / Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 150;
            // 
            // iDCardNoDataGridViewTextBoxColumn
            // 
            this.iDCardNoDataGridViewTextBoxColumn.DataPropertyName = "IDCardNo";
            this.iDCardNoDataGridViewTextBoxColumn.HeaderText = "IDCardNo";
            this.iDCardNoDataGridViewTextBoxColumn.Name = "iDCardNoDataGridViewTextBoxColumn";
            this.iDCardNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // departmentDataGridViewTextBoxColumn
            // 
            this.departmentDataGridViewTextBoxColumn.DataPropertyName = "Department";
            this.departmentDataGridViewTextBoxColumn.HeaderText = "Department";
            this.departmentDataGridViewTextBoxColumn.Name = "departmentDataGridViewTextBoxColumn";
            this.departmentDataGridViewTextBoxColumn.ReadOnly = true;
            this.departmentDataGridViewTextBoxColumn.Width = 130;
            // 
            // mobileDataGridViewTextBoxColumn
            // 
            this.mobileDataGridViewTextBoxColumn.DataPropertyName = "Mobile";
            this.mobileDataGridViewTextBoxColumn.HeaderText = "Mobile";
            this.mobileDataGridViewTextBoxColumn.Name = "mobileDataGridViewTextBoxColumn";
            this.mobileDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // designationDataGridViewTextBoxColumn
            // 
            this.designationDataGridViewTextBoxColumn.DataPropertyName = "Designation";
            this.designationDataGridViewTextBoxColumn.HeaderText = "Designation";
            this.designationDataGridViewTextBoxColumn.Name = "designationDataGridViewTextBoxColumn";
            this.designationDataGridViewTextBoxColumn.ReadOnly = true;
            this.designationDataGridViewTextBoxColumn.Width = 130;
            // 
            // employeeSearchVmBindingSource
            // 
            this.employeeSearchVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.EmployeeSearchVm);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(659, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Select Employee";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSearch.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.search3;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(596, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(63, 22);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // EmployeeSearchFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 307);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.employeeSearchGridView);
            this.Controls.Add(this.txtSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EmployeeSearchFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Employee";
            this.Load += new System.EventHandler(this.EmployeeSearchFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.employeeSearchGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeSearchVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView employeeSearchGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource employeeSearchVmBindingSource;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDCardNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn designationDataGridViewTextBoxColumn;
    }
}