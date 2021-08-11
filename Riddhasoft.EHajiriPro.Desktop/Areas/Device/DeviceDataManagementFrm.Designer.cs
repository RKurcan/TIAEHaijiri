namespace Riddhasoft.EHajiriPro.Desktop.Areas.Device
{
    partial class DeviceDataManagementFrm
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
            this.btnGetTime = new System.Windows.Forms.Button();
            this.btnSetAdmin = new System.Windows.Forms.Button();
            this.btnClearAdmin = new System.Windows.Forms.Button();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.panelBtn = new System.Windows.Forms.Panel();
            this.btnDownloadEmployee = new System.Windows.Forms.Button();
            this.btnDeleteAllLog = new System.Windows.Forms.Button();
            this.btnSetNameToDevice = new System.Windows.Forms.Button();
            this.btnDeleteEnrollment = new System.Windows.Forms.Button();
            this.btnUploadEnrollment = new System.Windows.Forms.Button();
            this.btnDownloadEnrollment = new System.Windows.Forms.Button();
            this.btnDownloadAllLog = new System.Windows.Forms.Button();
            this.btnDownloadNewLog = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panelDevices = new System.Windows.Forms.Panel();
            this.deviceChkboxList = new System.Windows.Forms.CheckedListBox();
            this.panelSelectAllDevice = new System.Windows.Forms.Panel();
            this.chkSelectAllMachines = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.templateGridPanel = new System.Windows.Forms.Panel();
            this.templateGridView = new System.Windows.Forms.DataGridView();
            this.fingerPrintBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.userIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userPasswordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fingerPrintIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.previledgeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTmpDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkValidateLastActivityDate = new System.Windows.Forms.CheckBox();
            this.panelBtn.SuspendLayout();
            this.panelDevices.SuspendLayout();
            this.panelSelectAllDevice.SuspendLayout();
            this.templateGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.templateGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fingerPrintBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetTime
            // 
            this.btnGetTime.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_clock_32;
            this.btnGetTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGetTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGetTime.Location = new System.Drawing.Point(0, 0);
            this.btnGetTime.Name = "btnGetTime";
            this.btnGetTime.Size = new System.Drawing.Size(98, 62);
            this.btnGetTime.TabIndex = 0;
            this.btnGetTime.Text = "Get Time";
            this.btnGetTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGetTime.UseVisualStyleBackColor = true;
            this.btnGetTime.Click += new System.EventHandler(this.btnGetTime_Click);
            // 
            // btnSetAdmin
            // 
            this.btnSetAdmin.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_admin_settings_male_32;
            this.btnSetAdmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSetAdmin.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSetAdmin.Location = new System.Drawing.Point(196, 0);
            this.btnSetAdmin.Name = "btnSetAdmin";
            this.btnSetAdmin.Size = new System.Drawing.Size(98, 62);
            this.btnSetAdmin.TabIndex = 2;
            this.btnSetAdmin.Text = "Set Admin";
            this.btnSetAdmin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSetAdmin.UseVisualStyleBackColor = true;
            this.btnSetAdmin.Click += new System.EventHandler(this.btnSetAdmin_Click);
            // 
            // btnClearAdmin
            // 
            this.btnClearAdmin.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_admin_settings_male_32___Copy;
            this.btnClearAdmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClearAdmin.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClearAdmin.Location = new System.Drawing.Point(294, 0);
            this.btnClearAdmin.Name = "btnClearAdmin";
            this.btnClearAdmin.Size = new System.Drawing.Size(98, 62);
            this.btnClearAdmin.TabIndex = 3;
            this.btnClearAdmin.Text = "Clear Admin";
            this.btnClearAdmin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClearAdmin.UseVisualStyleBackColor = true;
            this.btnClearAdmin.Click += new System.EventHandler(this.btnClearAdmin_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_clock_32__1_;
            this.btnSetTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSetTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSetTime.Location = new System.Drawing.Point(98, 0);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(98, 62);
            this.btnSetTime.TabIndex = 1;
            this.btnSetTime.Text = "Set Time";
            this.btnSetTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // panelBtn
            // 
            this.panelBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBtn.Controls.Add(this.btnDownloadEmployee);
            this.panelBtn.Controls.Add(this.btnDeleteAllLog);
            this.panelBtn.Controls.Add(this.btnSetNameToDevice);
            this.panelBtn.Controls.Add(this.btnDeleteEnrollment);
            this.panelBtn.Controls.Add(this.btnUploadEnrollment);
            this.panelBtn.Controls.Add(this.btnDownloadEnrollment);
            this.panelBtn.Controls.Add(this.btnDownloadAllLog);
            this.panelBtn.Controls.Add(this.btnDownloadNewLog);
            this.panelBtn.Controls.Add(this.btnClearAdmin);
            this.panelBtn.Controls.Add(this.btnSetAdmin);
            this.panelBtn.Controls.Add(this.btnSetTime);
            this.panelBtn.Controls.Add(this.btnGetTime);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBtn.Location = new System.Drawing.Point(0, 0);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(1255, 64);
            this.panelBtn.TabIndex = 4;
            // 
            // btnDownloadEmployee
            // 
            this.btnDownloadEmployee.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_download_resume_32;
            this.btnDownloadEmployee.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDownloadEmployee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownloadEmployee.Location = new System.Drawing.Point(1126, 0);
            this.btnDownloadEmployee.Name = "btnDownloadEmployee";
            this.btnDownloadEmployee.Size = new System.Drawing.Size(127, 62);
            this.btnDownloadEmployee.TabIndex = 11;
            this.btnDownloadEmployee.Text = "Download Device Emp.";
            this.btnDownloadEmployee.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDownloadEmployee.UseVisualStyleBackColor = true;
            this.btnDownloadEmployee.Click += new System.EventHandler(this.btnDownloadEmployee_Click);
            // 
            // btnDeleteAllLog
            // 
            this.btnDeleteAllLog.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_remove_32;
            this.btnDeleteAllLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteAllLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDeleteAllLog.Location = new System.Drawing.Point(1044, 0);
            this.btnDeleteAllLog.Name = "btnDeleteAllLog";
            this.btnDeleteAllLog.Size = new System.Drawing.Size(82, 62);
            this.btnDeleteAllLog.TabIndex = 10;
            this.btnDeleteAllLog.Text = "Delete All Log";
            this.btnDeleteAllLog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteAllLog.UseVisualStyleBackColor = true;
            this.btnDeleteAllLog.Click += new System.EventHandler(this.btnDeleteAllLog_Click);
            // 
            // btnSetNameToDevice
            // 
            this.btnSetNameToDevice.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_name_32;
            this.btnSetNameToDevice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSetNameToDevice.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSetNameToDevice.Location = new System.Drawing.Point(927, 0);
            this.btnSetNameToDevice.Name = "btnSetNameToDevice";
            this.btnSetNameToDevice.Size = new System.Drawing.Size(117, 62);
            this.btnSetNameToDevice.TabIndex = 9;
            this.btnSetNameToDevice.Text = "Set Name To Device";
            this.btnSetNameToDevice.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSetNameToDevice.UseVisualStyleBackColor = true;
            this.btnSetNameToDevice.Click += new System.EventHandler(this.btnSetNameToDevice_Click);
            // 
            // btnDeleteEnrollment
            // 
            this.btnDeleteEnrollment.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_resume_32;
            this.btnDeleteEnrollment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteEnrollment.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDeleteEnrollment.Location = new System.Drawing.Point(828, 0);
            this.btnDeleteEnrollment.Name = "btnDeleteEnrollment";
            this.btnDeleteEnrollment.Size = new System.Drawing.Size(99, 62);
            this.btnDeleteEnrollment.TabIndex = 8;
            this.btnDeleteEnrollment.Text = "Delete Enrollment";
            this.btnDeleteEnrollment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteEnrollment.UseVisualStyleBackColor = true;
            this.btnDeleteEnrollment.Click += new System.EventHandler(this.btnDeleteEnrollment_Click);
            // 
            // btnUploadEnrollment
            // 
            this.btnUploadEnrollment.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_submit_resume_32;
            this.btnUploadEnrollment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadEnrollment.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUploadEnrollment.Location = new System.Drawing.Point(724, 0);
            this.btnUploadEnrollment.Name = "btnUploadEnrollment";
            this.btnUploadEnrollment.Size = new System.Drawing.Size(104, 62);
            this.btnUploadEnrollment.TabIndex = 7;
            this.btnUploadEnrollment.Text = "Upload Enrollment";
            this.btnUploadEnrollment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUploadEnrollment.UseVisualStyleBackColor = true;
            this.btnUploadEnrollment.Click += new System.EventHandler(this.btnUploadEnrollment_Click);
            // 
            // btnDownloadEnrollment
            // 
            this.btnDownloadEnrollment.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_download_resume_32;
            this.btnDownloadEnrollment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDownloadEnrollment.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDownloadEnrollment.Location = new System.Drawing.Point(609, 0);
            this.btnDownloadEnrollment.Name = "btnDownloadEnrollment";
            this.btnDownloadEnrollment.Size = new System.Drawing.Size(115, 62);
            this.btnDownloadEnrollment.TabIndex = 6;
            this.btnDownloadEnrollment.Text = "Download Enrollment";
            this.btnDownloadEnrollment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDownloadEnrollment.UseVisualStyleBackColor = true;
            this.btnDownloadEnrollment.Click += new System.EventHandler(this.btnDownloadEnrollment_Click);
            // 
            // btnDownloadAllLog
            // 
            this.btnDownloadAllLog.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_desktop_download_32;
            this.btnDownloadAllLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDownloadAllLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDownloadAllLog.Location = new System.Drawing.Point(511, 0);
            this.btnDownloadAllLog.Name = "btnDownloadAllLog";
            this.btnDownloadAllLog.Size = new System.Drawing.Size(98, 62);
            this.btnDownloadAllLog.TabIndex = 5;
            this.btnDownloadAllLog.Text = "Download All Log";
            this.btnDownloadAllLog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDownloadAllLog.UseVisualStyleBackColor = true;
            this.btnDownloadAllLog.Click += new System.EventHandler(this.btnDownloadAllLog_Click);
            // 
            // btnDownloadNewLog
            // 
            this.btnDownloadNewLog.BackgroundImage = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_download_32;
            this.btnDownloadNewLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDownloadNewLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDownloadNewLog.Location = new System.Drawing.Point(392, 0);
            this.btnDownloadNewLog.Name = "btnDownloadNewLog";
            this.btnDownloadNewLog.Size = new System.Drawing.Size(119, 62);
            this.btnDownloadNewLog.TabIndex = 4;
            this.btnDownloadNewLog.Text = "Download New Log";
            this.btnDownloadNewLog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDownloadNewLog.UseVisualStyleBackColor = true;
            this.btnDownloadNewLog.Click += new System.EventHandler(this.btnDownloadNewLog_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(5, 5);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(20, 16);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "...";
            // 
            // panelDevices
            // 
            this.panelDevices.Controls.Add(this.deviceChkboxList);
            this.panelDevices.Controls.Add(this.panelSelectAllDevice);
            this.panelDevices.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDevices.Location = new System.Drawing.Point(0, 64);
            this.panelDevices.Name = "panelDevices";
            this.panelDevices.Size = new System.Drawing.Size(301, 397);
            this.panelDevices.TabIndex = 5;
            // 
            // deviceChkboxList
            // 
            this.deviceChkboxList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deviceChkboxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceChkboxList.FormattingEnabled = true;
            this.deviceChkboxList.Location = new System.Drawing.Point(0, 54);
            this.deviceChkboxList.Name = "deviceChkboxList";
            this.deviceChkboxList.Size = new System.Drawing.Size(301, 343);
            this.deviceChkboxList.TabIndex = 0;
            // 
            // panelSelectAllDevice
            // 
            this.panelSelectAllDevice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSelectAllDevice.Controls.Add(this.chkValidateLastActivityDate);
            this.panelSelectAllDevice.Controls.Add(this.chkSelectAllMachines);
            this.panelSelectAllDevice.Controls.Add(this.label2);
            this.panelSelectAllDevice.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSelectAllDevice.Location = new System.Drawing.Point(0, 0);
            this.panelSelectAllDevice.Name = "panelSelectAllDevice";
            this.panelSelectAllDevice.Size = new System.Drawing.Size(301, 54);
            this.panelSelectAllDevice.TabIndex = 0;
            // 
            // chkSelectAllMachines
            // 
            this.chkSelectAllMachines.AutoSize = true;
            this.chkSelectAllMachines.Location = new System.Drawing.Point(3, 27);
            this.chkSelectAllMachines.Name = "chkSelectAllMachines";
            this.chkSelectAllMachines.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAllMachines.TabIndex = 0;
            this.chkSelectAllMachines.Text = "Select All";
            this.chkSelectAllMachines.UseVisualStyleBackColor = true;
            this.chkSelectAllMachines.CheckedChanged += new System.EventHandler(this.chkSelectAllMachines_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Machines";
            // 
            // templateGridPanel
            // 
            this.templateGridPanel.Controls.Add(this.templateGridView);
            this.templateGridPanel.Controls.Add(this.panel1);
            this.templateGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateGridPanel.Location = new System.Drawing.Point(301, 64);
            this.templateGridPanel.Name = "templateGridPanel";
            this.templateGridPanel.Size = new System.Drawing.Size(954, 397);
            this.templateGridPanel.TabIndex = 6;
            // 
            // templateGridView
            // 
            this.templateGridView.AllowUserToAddRows = false;
            this.templateGridView.AllowUserToDeleteRows = false;
            this.templateGridView.AutoGenerateColumns = false;
            this.templateGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.templateGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.selectDataGridViewCheckBoxColumn,
            this.userIdDataGridViewTextBoxColumn,
            this.userPasswordDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.fingerPrintIdDataGridViewTextBoxColumn,
            this.previledgeDataGridViewTextBoxColumn,
            this.sTmpDateDataGridViewTextBoxColumn});
            this.templateGridView.DataSource = this.fingerPrintBindingSource;
            this.templateGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateGridView.Location = new System.Drawing.Point(0, 54);
            this.templateGridView.Name = "templateGridView";
            this.templateGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.templateGridView.Size = new System.Drawing.Size(954, 343);
            this.templateGridView.TabIndex = 0;
            // 
            // fingerPrintBindingSource
            // 
            this.fingerPrintBindingSource.DataSource = typeof(Riddhasoft.Attendance.Entities.FingerPrint);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 54);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(369, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Templates";
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // selectDataGridViewCheckBoxColumn
            // 
            this.selectDataGridViewCheckBoxColumn.DataPropertyName = "Select";
            this.selectDataGridViewCheckBoxColumn.HeaderText = "Select";
            this.selectDataGridViewCheckBoxColumn.Name = "selectDataGridViewCheckBoxColumn";
            // 
            // userIdDataGridViewTextBoxColumn
            // 
            this.userIdDataGridViewTextBoxColumn.DataPropertyName = "UserId";
            this.userIdDataGridViewTextBoxColumn.HeaderText = "Device User Id";
            this.userIdDataGridViewTextBoxColumn.Name = "userIdDataGridViewTextBoxColumn";
            this.userIdDataGridViewTextBoxColumn.Width = 150;
            // 
            // userPasswordDataGridViewTextBoxColumn
            // 
            this.userPasswordDataGridViewTextBoxColumn.DataPropertyName = "UserPassword";
            this.userPasswordDataGridViewTextBoxColumn.HeaderText = "UserPassword";
            this.userPasswordDataGridViewTextBoxColumn.Name = "userPasswordDataGridViewTextBoxColumn";
            this.userPasswordDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Device User Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 350;
            // 
            // fingerPrintIdDataGridViewTextBoxColumn
            // 
            this.fingerPrintIdDataGridViewTextBoxColumn.DataPropertyName = "FingerPrintId";
            this.fingerPrintIdDataGridViewTextBoxColumn.HeaderText = "Device Finger Print";
            this.fingerPrintIdDataGridViewTextBoxColumn.Name = "fingerPrintIdDataGridViewTextBoxColumn";
            this.fingerPrintIdDataGridViewTextBoxColumn.Width = 200;
            // 
            // previledgeDataGridViewTextBoxColumn
            // 
            this.previledgeDataGridViewTextBoxColumn.DataPropertyName = "Previledge";
            this.previledgeDataGridViewTextBoxColumn.HeaderText = "Previledge";
            this.previledgeDataGridViewTextBoxColumn.Name = "previledgeDataGridViewTextBoxColumn";
            this.previledgeDataGridViewTextBoxColumn.Visible = false;
            // 
            // sTmpDateDataGridViewTextBoxColumn
            // 
            this.sTmpDateDataGridViewTextBoxColumn.DataPropertyName = "sTmpDate";
            this.sTmpDateDataGridViewTextBoxColumn.HeaderText = "sTmpDate";
            this.sTmpDateDataGridViewTextBoxColumn.Name = "sTmpDateDataGridViewTextBoxColumn";
            this.sTmpDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // chkValidateLastActivityDate
            // 
            this.chkValidateLastActivityDate.AutoSize = true;
            this.chkValidateLastActivityDate.Checked = true;
            this.chkValidateLastActivityDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkValidateLastActivityDate.Location = new System.Drawing.Point(104, 25);
            this.chkValidateLastActivityDate.Name = "chkValidateLastActivityDate";
            this.chkValidateLastActivityDate.Size = new System.Drawing.Size(140, 17);
            this.chkValidateLastActivityDate.TabIndex = 8;
            this.chkValidateLastActivityDate.Text = "Validate Last Sync Date";
            this.chkValidateLastActivityDate.UseVisualStyleBackColor = true;
            // 
            // DeviceDataManagementFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 461);
            this.Controls.Add(this.templateGridPanel);
            this.Controls.Add(this.panelDevices);
            this.Controls.Add(this.panelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "DeviceDataManagementFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Device Data Management";
            this.Load += new System.EventHandler(this.DeviceDataManagementFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DeviceDataManagementFrm_KeyDown);
            this.panelBtn.ResumeLayout(false);
            this.panelDevices.ResumeLayout(false);
            this.panelSelectAllDevice.ResumeLayout(false);
            this.panelSelectAllDevice.PerformLayout();
            this.templateGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.templateGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fingerPrintBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetTime;
        private System.Windows.Forms.Button btnSetAdmin;
        private System.Windows.Forms.Button btnClearAdmin;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.Panel panelBtn;
        private System.Windows.Forms.Panel panelDevices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkSelectAllMachines;
        private System.Windows.Forms.Panel panelSelectAllDevice;
        private System.Windows.Forms.CheckedListBox deviceChkboxList;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnDownloadNewLog;
        private System.Windows.Forms.Panel templateGridPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView templateGridView;
        private System.Windows.Forms.BindingSource fingerPrintBindingSource;
        private System.Windows.Forms.Button btnDownloadAllLog;
        private System.Windows.Forms.Button btnDownloadEnrollment;
        private System.Windows.Forms.Button btnUploadEnrollment;
        private System.Windows.Forms.Button btnDeleteEnrollment;
        private System.Windows.Forms.Button btnSetNameToDevice;
        private System.Windows.Forms.Button btnDeleteAllLog;
        private System.Windows.Forms.Button btnDownloadEmployee;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userPasswordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fingerPrintIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn previledgeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTmpDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.CheckBox chkValidateLastActivityDate;
    }
}