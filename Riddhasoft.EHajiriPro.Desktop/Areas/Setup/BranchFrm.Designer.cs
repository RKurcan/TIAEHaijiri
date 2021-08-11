namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    partial class BranchFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BranchFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtContactNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBranchAddress = new System.Windows.Forms.TextBox();
            this.txtBranchName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBranchCode = new System.Windows.Forms.TextBox();
            this.branchGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameNpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressNpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isHeadOfficeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.branchGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.branchGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.branchGridVmBindingSource)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(765, 24);
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
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(335, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 17);
            this.label12.TabIndex = 99;
            this.label12.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(698, 76);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 17);
            this.label13.TabIndex = 98;
            this.label13.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(698, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 17);
            this.label14.TabIndex = 97;
            this.label14.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(336, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 17);
            this.label9.TabIndex = 95;
            this.label9.Text = "*";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(116, 103);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(214, 20);
            this.txtEmail.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 86;
            this.label6.Text = "Email";
            // 
            // txtContactNumber
            // 
            this.txtContactNumber.Location = new System.Drawing.Point(479, 73);
            this.txtContactNumber.Name = "txtContactNumber";
            this.txtContactNumber.Size = new System.Drawing.Size(214, 20);
            this.txtContactNumber.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(372, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 85;
            this.label4.Text = "Contact Number";
            // 
            // txtBranchAddress
            // 
            this.txtBranchAddress.Location = new System.Drawing.Point(116, 74);
            this.txtBranchAddress.Name = "txtBranchAddress";
            this.txtBranchAddress.Size = new System.Drawing.Size(214, 20);
            this.txtBranchAddress.TabIndex = 2;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Location = new System.Drawing.Point(479, 41);
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Size = new System.Drawing.Size(214, 20);
            this.txtBranchName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 82;
            this.label3.Text = "Branch Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(372, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 81;
            this.label2.Text = "Branch Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 80;
            this.label1.Text = "Code";
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Location = new System.Drawing.Point(116, 41);
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Size = new System.Drawing.Size(87, 20);
            this.txtBranchCode.TabIndex = 0;
            // 
            // branchGridView
            // 
            this.branchGridView.AllowUserToAddRows = false;
            this.branchGridView.AllowUserToDeleteRows = false;
            this.branchGridView.AutoGenerateColumns = false;
            this.branchGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.branchGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.companyIdDataGridViewTextBoxColumn,
            this.companyNameDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.nameNpDataGridViewTextBoxColumn,
            this.addressDataGridViewTextBoxColumn,
            this.addressNpDataGridViewTextBoxColumn,
            this.contactNoDataGridViewTextBoxColumn,
            this.emailDataGridViewTextBoxColumn,
            this.isHeadOfficeDataGridViewCheckBoxColumn});
            this.branchGridView.DataSource = this.branchGridVmBindingSource;
            this.branchGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.branchGridView.Location = new System.Drawing.Point(0, 197);
            this.branchGridView.Name = "branchGridView";
            this.branchGridView.ReadOnly = true;
            this.branchGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.branchGridView.Size = new System.Drawing.Size(765, 187);
            this.branchGridView.TabIndex = 9;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // companyIdDataGridViewTextBoxColumn
            // 
            this.companyIdDataGridViewTextBoxColumn.DataPropertyName = "CompanyId";
            this.companyIdDataGridViewTextBoxColumn.HeaderText = "CompanyId";
            this.companyIdDataGridViewTextBoxColumn.Name = "companyIdDataGridViewTextBoxColumn";
            this.companyIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.companyIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // companyNameDataGridViewTextBoxColumn
            // 
            this.companyNameDataGridViewTextBoxColumn.DataPropertyName = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn.HeaderText = "Company";
            this.companyNameDataGridViewTextBoxColumn.Name = "companyNameDataGridViewTextBoxColumn";
            this.companyNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.companyNameDataGridViewTextBoxColumn.Width = 130;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Branch Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 130;
            // 
            // nameNpDataGridViewTextBoxColumn
            // 
            this.nameNpDataGridViewTextBoxColumn.DataPropertyName = "NameNp";
            this.nameNpDataGridViewTextBoxColumn.HeaderText = "NameNp";
            this.nameNpDataGridViewTextBoxColumn.Name = "nameNpDataGridViewTextBoxColumn";
            this.nameNpDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameNpDataGridViewTextBoxColumn.Visible = false;
            // 
            // addressDataGridViewTextBoxColumn
            // 
            this.addressDataGridViewTextBoxColumn.DataPropertyName = "Address";
            this.addressDataGridViewTextBoxColumn.HeaderText = "Address";
            this.addressDataGridViewTextBoxColumn.Name = "addressDataGridViewTextBoxColumn";
            this.addressDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressDataGridViewTextBoxColumn.Width = 120;
            // 
            // addressNpDataGridViewTextBoxColumn
            // 
            this.addressNpDataGridViewTextBoxColumn.DataPropertyName = "AddressNp";
            this.addressNpDataGridViewTextBoxColumn.HeaderText = "AddressNp";
            this.addressNpDataGridViewTextBoxColumn.Name = "addressNpDataGridViewTextBoxColumn";
            this.addressNpDataGridViewTextBoxColumn.ReadOnly = true;
            this.addressNpDataGridViewTextBoxColumn.Visible = false;
            // 
            // contactNoDataGridViewTextBoxColumn
            // 
            this.contactNoDataGridViewTextBoxColumn.DataPropertyName = "ContactNo";
            this.contactNoDataGridViewTextBoxColumn.HeaderText = "ContactNo";
            this.contactNoDataGridViewTextBoxColumn.Name = "contactNoDataGridViewTextBoxColumn";
            this.contactNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.contactNoDataGridViewTextBoxColumn.Width = 120;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            this.emailDataGridViewTextBoxColumn.ReadOnly = true;
            this.emailDataGridViewTextBoxColumn.Width = 120;
            // 
            // isHeadOfficeDataGridViewCheckBoxColumn
            // 
            this.isHeadOfficeDataGridViewCheckBoxColumn.DataPropertyName = "IsHeadOffice";
            this.isHeadOfficeDataGridViewCheckBoxColumn.HeaderText = "IsHeadOffice";
            this.isHeadOfficeDataGridViewCheckBoxColumn.Name = "isHeadOfficeDataGridViewCheckBoxColumn";
            this.isHeadOfficeDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isHeadOfficeDataGridViewCheckBoxColumn.Visible = false;
            // 
            // branchGridVmBindingSource
            // 
            this.branchGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.BranchGridVm);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(689, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 8;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2, 175);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(681, 20);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(204, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(116, 141);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(209, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 17);
            this.label5.TabIndex = 105;
            this.label5.Text = "*";
            // 
            // BranchFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 384);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.branchGridView);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtContactNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBranchAddress);
            this.Controls.Add(this.txtBranchName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBranchCode);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BranchFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Branch";
            this.Load += new System.EventHandler(this.BranchFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BranchFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.branchGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.branchGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtContactNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBranchAddress;
        private System.Windows.Forms.TextBox txtBranchName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBranchCode;
        private System.Windows.Forms.DataGridView branchGridView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource branchGridVmBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameNpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressNpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isHeadOfficeDataGridViewCheckBoxColumn;
    }
}