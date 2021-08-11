namespace Riddhasoft.EHajiriPro.Desktop.Areas.Device
{
    partial class DeviceSetupFrm
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
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.deviceGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ipAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modelIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serialNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modelNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastActivityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkInOutIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deviceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceGridVmBindingSource)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(656, 24);
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
            this.label1.Location = new System.Drawing.Point(8, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 78;
            this.label1.Text = "Device Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "IP Address";
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(88, 32);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(156, 20);
            this.txtDeviceName.TabIndex = 0;
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(88, 59);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(156, 20);
            this.txtIPAddress.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(88, 120);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(169, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // deviceGridView
            // 
            this.deviceGridView.AllowUserToAddRows = false;
            this.deviceGridView.AllowUserToDeleteRows = false;
            this.deviceGridView.AutoGenerateColumns = false;
            this.deviceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.deviceGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.ipAddressDataGridViewTextBoxColumn,
            this.companyIdDataGridViewTextBoxColumn,
            this.modelIdDataGridViewTextBoxColumn,
            this.serialNumberDataGridViewTextBoxColumn,
            this.modelNameDataGridViewTextBoxColumn,
            this.lastActivityDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.deviceTypeDataGridViewTextBoxColumn,
            this.checkInOutIndexDataGridViewTextBoxColumn});
            this.deviceGridView.DataSource = this.deviceGridVmBindingSource;
            this.deviceGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.deviceGridView.Location = new System.Drawing.Point(0, 192);
            this.deviceGridView.Name = "deviceGridView";
            this.deviceGridView.ReadOnly = true;
            this.deviceGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.deviceGridView.Size = new System.Drawing.Size(656, 215);
            this.deviceGridView.TabIndex = 7;
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
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 180;
            // 
            // ipAddressDataGridViewTextBoxColumn
            // 
            this.ipAddressDataGridViewTextBoxColumn.DataPropertyName = "IpAddress";
            this.ipAddressDataGridViewTextBoxColumn.HeaderText = "IpAddress";
            this.ipAddressDataGridViewTextBoxColumn.Name = "ipAddressDataGridViewTextBoxColumn";
            this.ipAddressDataGridViewTextBoxColumn.ReadOnly = true;
            this.ipAddressDataGridViewTextBoxColumn.Width = 110;
            // 
            // companyIdDataGridViewTextBoxColumn
            // 
            this.companyIdDataGridViewTextBoxColumn.DataPropertyName = "Company_Id";
            this.companyIdDataGridViewTextBoxColumn.HeaderText = "Company_Id";
            this.companyIdDataGridViewTextBoxColumn.Name = "companyIdDataGridViewTextBoxColumn";
            this.companyIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.companyIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // modelIdDataGridViewTextBoxColumn
            // 
            this.modelIdDataGridViewTextBoxColumn.DataPropertyName = "ModelId";
            this.modelIdDataGridViewTextBoxColumn.HeaderText = "ModelId";
            this.modelIdDataGridViewTextBoxColumn.Name = "modelIdDataGridViewTextBoxColumn";
            this.modelIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.modelIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // serialNumberDataGridViewTextBoxColumn
            // 
            this.serialNumberDataGridViewTextBoxColumn.DataPropertyName = "SerialNumber";
            this.serialNumberDataGridViewTextBoxColumn.HeaderText = "Serial Number";
            this.serialNumberDataGridViewTextBoxColumn.Name = "serialNumberDataGridViewTextBoxColumn";
            this.serialNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.serialNumberDataGridViewTextBoxColumn.Width = 180;
            // 
            // modelNameDataGridViewTextBoxColumn
            // 
            this.modelNameDataGridViewTextBoxColumn.DataPropertyName = "ModelName";
            this.modelNameDataGridViewTextBoxColumn.HeaderText = "Model";
            this.modelNameDataGridViewTextBoxColumn.Name = "modelNameDataGridViewTextBoxColumn";
            this.modelNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.modelNameDataGridViewTextBoxColumn.Visible = false;
            this.modelNameDataGridViewTextBoxColumn.Width = 110;
            // 
            // lastActivityDataGridViewTextBoxColumn
            // 
            this.lastActivityDataGridViewTextBoxColumn.DataPropertyName = "LastActivity";
            this.lastActivityDataGridViewTextBoxColumn.HeaderText = "LastActivity";
            this.lastActivityDataGridViewTextBoxColumn.Name = "lastActivityDataGridViewTextBoxColumn";
            this.lastActivityDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastActivityDataGridViewTextBoxColumn.Visible = false;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Visible = false;
            // 
            // deviceTypeDataGridViewTextBoxColumn
            // 
            this.deviceTypeDataGridViewTextBoxColumn.DataPropertyName = "DeviceType";
            this.deviceTypeDataGridViewTextBoxColumn.HeaderText = "DeviceType";
            this.deviceTypeDataGridViewTextBoxColumn.Name = "deviceTypeDataGridViewTextBoxColumn";
            this.deviceTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.deviceTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // checkInOutIndexDataGridViewTextBoxColumn
            // 
            this.checkInOutIndexDataGridViewTextBoxColumn.DataPropertyName = "CheckInOutIndex";
            this.checkInOutIndexDataGridViewTextBoxColumn.HeaderText = "Check In  Out Index";
            this.checkInOutIndexDataGridViewTextBoxColumn.Name = "checkInOutIndexDataGridViewTextBoxColumn";
            this.checkInOutIndexDataGridViewTextBoxColumn.ReadOnly = true;
            this.checkInOutIndexDataGridViewTextBoxColumn.Width = 130;
            // 
            // deviceGridVmBindingSource
            // 
            this.deviceGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.DeviceGridVm);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2, 167);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(576, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(581, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 6;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 79;
            this.label3.Text = "Serial Number";
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Location = new System.Drawing.Point(88, 85);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(156, 20);
            this.txtSerialNumber.TabIndex = 2;
            // 
            // DeviceSetupFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 407);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.deviceGridView);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSerialNumber);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDeviceName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DeviceSetupFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Device Setup";
            this.Load += new System.EventHandler(this.DeviceSetupFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DeviceSetupFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deviceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceGridVmBindingSource)).EndInit();
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
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView deviceGridView;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.BindingSource deviceGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ipAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modelIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modelNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastActivityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkInOutIndexDataGridViewTextBoxColumn;
    }
}