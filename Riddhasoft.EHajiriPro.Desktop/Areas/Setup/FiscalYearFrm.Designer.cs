namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    partial class FiscalYearFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiscalYearFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFY = new System.Windows.Forms.TextBox();
            this.chkCurrentFY = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.eFiscalYearBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Searchtxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.fiscalYearGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fiscalYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentFiscalYearDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.startDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.FromMtb = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ToMtb = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.eFiscalYearBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fiscalYearGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fiscal Year";
            // 
            // txtFY
            // 
            this.txtFY.Location = new System.Drawing.Point(94, 40);
            this.txtFY.MaxLength = 6;
            this.txtFY.Name = "txtFY";
            this.txtFY.Size = new System.Drawing.Size(106, 20);
            this.txtFY.TabIndex = 0;
            // 
            // chkCurrentFY
            // 
            this.chkCurrentFY.AutoSize = true;
            this.chkCurrentFY.Location = new System.Drawing.Point(336, 43);
            this.chkCurrentFY.Name = "chkCurrentFY";
            this.chkCurrentFY.Size = new System.Drawing.Size(76, 17);
            this.chkCurrentFY.TabIndex = 1;
            this.chkCurrentFY.Text = "Current FY";
            this.chkCurrentFY.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(185, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "(YYYY/MM/DD)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(294, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "To";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(94, 95);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // eFiscalYearBindingSource
            // 
            this.eFiscalYearBindingSource.DataSource = typeof(Riddhasoft.OfficeSetup.Entities.EFiscalYear);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.excelToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(556, 24);
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
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.excelIcon;
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.excelToolStripMenuItem.Text = "Excel";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Riddhasoft.EHajiriPro.Desktop.Properties.Resources.icons8_cancel_48;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Searchtxt
            // 
            this.Searchtxt.Location = new System.Drawing.Point(2, 132);
            this.Searchtxt.Name = "Searchtxt";
            this.Searchtxt.Size = new System.Drawing.Size(475, 20);
            this.Searchtxt.TabIndex = 5;
            this.Searchtxt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Searchtxt_KeyUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(480, 131);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 6;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // fiscalYearGridView
            // 
            this.fiscalYearGridView.AllowUserToAddRows = false;
            this.fiscalYearGridView.AllowUserToDeleteRows = false;
            this.fiscalYearGridView.AutoGenerateColumns = false;
            this.fiscalYearGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fiscalYearGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.fiscalYearDataGridViewTextBoxColumn,
            this.currentFiscalYearDataGridViewCheckBoxColumn,
            this.startDateDataGridViewTextBoxColumn,
            this.endDateDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.branchDataGridViewTextBoxColumn});
            this.fiscalYearGridView.DataSource = this.eFiscalYearBindingSource;
            this.fiscalYearGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fiscalYearGridView.Location = new System.Drawing.Point(0, 156);
            this.fiscalYearGridView.Name = "fiscalYearGridView";
            this.fiscalYearGridView.ReadOnly = true;
            this.fiscalYearGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.fiscalYearGridView.Size = new System.Drawing.Size(556, 215);
            this.fiscalYearGridView.TabIndex = 7;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // fiscalYearDataGridViewTextBoxColumn
            // 
            this.fiscalYearDataGridViewTextBoxColumn.DataPropertyName = "FiscalYear";
            this.fiscalYearDataGridViewTextBoxColumn.HeaderText = "FiscalYear";
            this.fiscalYearDataGridViewTextBoxColumn.Name = "fiscalYearDataGridViewTextBoxColumn";
            this.fiscalYearDataGridViewTextBoxColumn.ReadOnly = true;
            this.fiscalYearDataGridViewTextBoxColumn.Width = 150;
            // 
            // currentFiscalYearDataGridViewCheckBoxColumn
            // 
            this.currentFiscalYearDataGridViewCheckBoxColumn.DataPropertyName = "CurrentFiscalYear";
            this.currentFiscalYearDataGridViewCheckBoxColumn.HeaderText = "Current Fiscal Year";
            this.currentFiscalYearDataGridViewCheckBoxColumn.Name = "currentFiscalYearDataGridViewCheckBoxColumn";
            this.currentFiscalYearDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // startDateDataGridViewTextBoxColumn
            // 
            this.startDateDataGridViewTextBoxColumn.DataPropertyName = "StartDate";
            this.startDateDataGridViewTextBoxColumn.HeaderText = "StartDate";
            this.startDateDataGridViewTextBoxColumn.Name = "startDateDataGridViewTextBoxColumn";
            this.startDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.startDateDataGridViewTextBoxColumn.Width = 130;
            // 
            // endDateDataGridViewTextBoxColumn
            // 
            this.endDateDataGridViewTextBoxColumn.DataPropertyName = "EndDate";
            this.endDateDataGridViewTextBoxColumn.HeaderText = "EndDate";
            this.endDateDataGridViewTextBoxColumn.Name = "endDateDataGridViewTextBoxColumn";
            this.endDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.endDateDataGridViewTextBoxColumn.Width = 130;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // branchDataGridViewTextBoxColumn
            // 
            this.branchDataGridViewTextBoxColumn.DataPropertyName = "Branch";
            this.branchDataGridViewTextBoxColumn.HeaderText = "Branch";
            this.branchDataGridViewTextBoxColumn.Name = "branchDataGridViewTextBoxColumn";
            this.branchDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchDataGridViewTextBoxColumn.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(206, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 17);
            this.label6.TabIndex = 72;
            this.label6.Text = "*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(167, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 17);
            this.label7.TabIndex = 73;
            this.label7.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(409, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 17);
            this.label8.TabIndex = 74;
            this.label8.Text = "*";
            // 
            // FromMtb
            // 
            this.FromMtb.Location = new System.Drawing.Point(94, 66);
            this.FromMtb.Mask = "0000/00/00";
            this.FromMtb.Name = "FromMtb";
            this.FromMtb.Size = new System.Drawing.Size(67, 20);
            this.FromMtb.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(428, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 163;
            this.label4.Text = "(YYYY/MM/DD)";
            // 
            // ToMtb
            // 
            this.ToMtb.Location = new System.Drawing.Point(336, 66);
            this.ToMtb.Mask = "0000/00/00";
            this.ToMtb.Name = "ToMtb";
            this.ToMtb.Size = new System.Drawing.Size(67, 20);
            this.ToMtb.TabIndex = 2;
            // 
            // FiscalYearFrm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 371);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ToMtb);
            this.Controls.Add(this.FromMtb);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.fiscalYearGridView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Searchtxt);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkCurrentFY);
            this.Controls.Add(this.txtFY);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FiscalYearFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fiscal Year";
            this.Load += new System.EventHandler(this.FiscalYearFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FiscalYearFrm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.eFiscalYearBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fiscalYearGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFY;
        private System.Windows.Forms.CheckBox chkCurrentFY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource eFiscalYearBindingSource;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox Searchtxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.DataGridView fiscalYearGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fiscalYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn currentFiscalYearDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox FromMtb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox ToMtb;
    }
}