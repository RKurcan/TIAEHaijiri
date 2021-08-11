namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    partial class LeaveQuataFrm
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
            this.leaveQuataGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicableGenderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isPaidLeaveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isLeaveCarryableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isMappedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.leaveQuataGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.leaveQuataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveQuataGridVmBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // leaveQuataGridView
            // 
            this.leaveQuataGridView.AllowUserToAddRows = false;
            this.leaveQuataGridView.AllowUserToDeleteRows = false;
            this.leaveQuataGridView.AutoGenerateColumns = false;
            this.leaveQuataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.leaveQuataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.leaveNameDataGridViewTextBoxColumn,
            this.balanceDataGridViewTextBoxColumn,
            this.applicableGenderDataGridViewTextBoxColumn,
            this.isPaidLeaveDataGridViewCheckBoxColumn,
            this.isLeaveCarryableDataGridViewCheckBoxColumn,
            this.isMappedDataGridViewCheckBoxColumn});
            this.leaveQuataGridView.DataSource = this.leaveQuataGridVmBindingSource;
            this.leaveQuataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.leaveQuataGridView.Location = new System.Drawing.Point(0, 0);
            this.leaveQuataGridView.Name = "leaveQuataGridView";
            this.leaveQuataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.leaveQuataGridView.Size = new System.Drawing.Size(413, 295);
            this.leaveQuataGridView.TabIndex = 9;
            this.leaveQuataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.leaveQuataGridView_DataError);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // leaveNameDataGridViewTextBoxColumn
            // 
            this.leaveNameDataGridViewTextBoxColumn.DataPropertyName = "LeaveName";
            this.leaveNameDataGridViewTextBoxColumn.HeaderText = "Leave ";
            this.leaveNameDataGridViewTextBoxColumn.Name = "leaveNameDataGridViewTextBoxColumn";
            this.leaveNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // balanceDataGridViewTextBoxColumn
            // 
            this.balanceDataGridViewTextBoxColumn.DataPropertyName = "Balance";
            this.balanceDataGridViewTextBoxColumn.HeaderText = "Balance";
            this.balanceDataGridViewTextBoxColumn.Name = "balanceDataGridViewTextBoxColumn";
            // 
            // applicableGenderDataGridViewTextBoxColumn
            // 
            this.applicableGenderDataGridViewTextBoxColumn.DataPropertyName = "ApplicableGender";
            this.applicableGenderDataGridViewTextBoxColumn.HeaderText = "ApplicableGender";
            this.applicableGenderDataGridViewTextBoxColumn.Name = "applicableGenderDataGridViewTextBoxColumn";
            this.applicableGenderDataGridViewTextBoxColumn.Visible = false;
            // 
            // isPaidLeaveDataGridViewCheckBoxColumn
            // 
            this.isPaidLeaveDataGridViewCheckBoxColumn.DataPropertyName = "IsPaidLeave";
            this.isPaidLeaveDataGridViewCheckBoxColumn.HeaderText = "IsPaidLeave";
            this.isPaidLeaveDataGridViewCheckBoxColumn.Name = "isPaidLeaveDataGridViewCheckBoxColumn";
            this.isPaidLeaveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // isLeaveCarryableDataGridViewCheckBoxColumn
            // 
            this.isLeaveCarryableDataGridViewCheckBoxColumn.DataPropertyName = "IsLeaveCarryable";
            this.isLeaveCarryableDataGridViewCheckBoxColumn.HeaderText = "IsLeaveCarryable";
            this.isLeaveCarryableDataGridViewCheckBoxColumn.Name = "isLeaveCarryableDataGridViewCheckBoxColumn";
            this.isLeaveCarryableDataGridViewCheckBoxColumn.Visible = false;
            // 
            // isMappedDataGridViewCheckBoxColumn
            // 
            this.isMappedDataGridViewCheckBoxColumn.DataPropertyName = "IsMapped";
            this.isMappedDataGridViewCheckBoxColumn.HeaderText = "IsMapped";
            this.isMappedDataGridViewCheckBoxColumn.Name = "isMappedDataGridViewCheckBoxColumn";
            // 
            // leaveQuataGridVmBindingSource
            // 
            this.leaveQuataGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.LeaveQuataGridVm);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(122, 299);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(203, 299);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LeaveQuataFrm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 325);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.leaveQuataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LeaveQuataFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LeaveQuataFrm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.leaveQuataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leaveQuataGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView leaveQuataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mappedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource leaveQuataGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicableGenderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPaidLeaveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLeaveCarryableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMappedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Button btnCancel;
    }
}