namespace Riddhasoft.EHajiriPro.Desktop.Areas.User_Management
{
    partial class UserFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UserGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordCharDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photoURLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isSuspendedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isDeletedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.userGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.Searchtxt = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userGridVmBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Full Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(81, 35);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(165, 20);
            this.txtFullName.TabIndex = 0;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(81, 62);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(165, 20);
            this.txtUserName.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(81, 88);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(165, 20);
            this.txtPassword.TabIndex = 2;
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
            this.menuStrip1.Size = new System.Drawing.Size(504, 24);
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
            // UserGridView
            // 
            this.UserGridView.AllowUserToAddRows = false;
            this.UserGridView.AllowUserToDeleteRows = false;
            this.UserGridView.AutoGenerateColumns = false;
            this.UserGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.passwordDataGridViewTextBoxColumn,
            this.passwordCharDataGridViewTextBoxColumn,
            this.roleIdDataGridViewTextBoxColumn,
            this.userTypeDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.fullNameDataGridViewTextBoxColumn,
            this.photoURLDataGridViewTextBoxColumn,
            this.isSuspendedDataGridViewCheckBoxColumn,
            this.isDeletedDataGridViewCheckBoxColumn});
            this.UserGridView.DataSource = this.userGridVmBindingSource;
            this.UserGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UserGridView.Location = new System.Drawing.Point(0, 192);
            this.UserGridView.Name = "UserGridView";
            this.UserGridView.ReadOnly = true;
            this.UserGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UserGridView.Size = new System.Drawing.Size(504, 189);
            this.UserGridView.TabIndex = 7;
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
            this.nameDataGridViewTextBoxColumn.Width = 155;
            // 
            // passwordDataGridViewTextBoxColumn
            // 
            this.passwordDataGridViewTextBoxColumn.DataPropertyName = "Password";
            this.passwordDataGridViewTextBoxColumn.HeaderText = "Password";
            this.passwordDataGridViewTextBoxColumn.Name = "passwordDataGridViewTextBoxColumn";
            this.passwordDataGridViewTextBoxColumn.ReadOnly = true;
            this.passwordDataGridViewTextBoxColumn.Visible = false;
            // 
            // passwordCharDataGridViewTextBoxColumn
            // 
            this.passwordCharDataGridViewTextBoxColumn.DataPropertyName = "PasswordChar";
            this.passwordCharDataGridViewTextBoxColumn.HeaderText = "Password";
            this.passwordCharDataGridViewTextBoxColumn.Name = "passwordCharDataGridViewTextBoxColumn";
            this.passwordCharDataGridViewTextBoxColumn.ReadOnly = true;
            this.passwordCharDataGridViewTextBoxColumn.Width = 135;
            // 
            // roleIdDataGridViewTextBoxColumn
            // 
            this.roleIdDataGridViewTextBoxColumn.DataPropertyName = "RoleId";
            this.roleIdDataGridViewTextBoxColumn.HeaderText = "RoleId";
            this.roleIdDataGridViewTextBoxColumn.Name = "roleIdDataGridViewTextBoxColumn";
            this.roleIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.roleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // userTypeDataGridViewTextBoxColumn
            // 
            this.userTypeDataGridViewTextBoxColumn.DataPropertyName = "UserType";
            this.userTypeDataGridViewTextBoxColumn.HeaderText = "UserType";
            this.userTypeDataGridViewTextBoxColumn.Name = "userTypeDataGridViewTextBoxColumn";
            this.userTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.userTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // fullNameDataGridViewTextBoxColumn
            // 
            this.fullNameDataGridViewTextBoxColumn.DataPropertyName = "FullName";
            this.fullNameDataGridViewTextBoxColumn.HeaderText = "Full Name";
            this.fullNameDataGridViewTextBoxColumn.Name = "fullNameDataGridViewTextBoxColumn";
            this.fullNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.fullNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // photoURLDataGridViewTextBoxColumn
            // 
            this.photoURLDataGridViewTextBoxColumn.DataPropertyName = "PhotoURL";
            this.photoURLDataGridViewTextBoxColumn.HeaderText = "PhotoURL";
            this.photoURLDataGridViewTextBoxColumn.Name = "photoURLDataGridViewTextBoxColumn";
            this.photoURLDataGridViewTextBoxColumn.ReadOnly = true;
            this.photoURLDataGridViewTextBoxColumn.Visible = false;
            // 
            // isSuspendedDataGridViewCheckBoxColumn
            // 
            this.isSuspendedDataGridViewCheckBoxColumn.DataPropertyName = "IsSuspended";
            this.isSuspendedDataGridViewCheckBoxColumn.HeaderText = "IsSuspended";
            this.isSuspendedDataGridViewCheckBoxColumn.Name = "isSuspendedDataGridViewCheckBoxColumn";
            this.isSuspendedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isSuspendedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // isDeletedDataGridViewCheckBoxColumn
            // 
            this.isDeletedDataGridViewCheckBoxColumn.DataPropertyName = "IsDeleted";
            this.isDeletedDataGridViewCheckBoxColumn.HeaderText = "IsDeleted";
            this.isDeletedDataGridViewCheckBoxColumn.Name = "isDeletedDataGridViewCheckBoxColumn";
            this.isDeletedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isDeletedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // userGridVmBindingSource
            // 
            this.userGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.UserGridVm);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(428, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 6;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Searchtxt
            // 
            this.Searchtxt.Location = new System.Drawing.Point(1, 167);
            this.Searchtxt.Name = "Searchtxt";
            this.Searchtxt.Size = new System.Drawing.Size(421, 20);
            this.Searchtxt.TabIndex = 5;
            this.Searchtxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Searchtxt_KeyUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(171, 129);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(81, 129);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // UserFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 381);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.UserGridView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Searchtxt);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "UserFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User";
            this.Load += new System.EventHandler(this.UserFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView UserGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Searchtxt;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource userGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passwordCharDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn roleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoURLDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isSuspendedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDeletedDataGridViewCheckBoxColumn;
    }
}