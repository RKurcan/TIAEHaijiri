namespace Riddhasoft.EHajiriPro.Desktop.Areas.Setup
{
    partial class DesignationFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignationFrm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mtbDesignationLvl = new System.Windows.Forms.NumericUpDown();
            this.txtMaxSalary = new System.Windows.Forms.TextBox();
            this.txtMinSalary = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.designationGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.designationLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxSalaryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minSalaryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.designationGridVmBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtbDesignationLvl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.designationGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.designationGridVmBindingSource)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(683, 24);
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
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(105, 59);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(144, 20);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 90;
            this.label1.Text = "Name";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(105, 33);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(94, 20);
            this.txtCode.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 91;
            this.label2.Text = "Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 90;
            this.label3.Text = "MaxSalary";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 92;
            this.label4.Text = "Designation Level";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 93;
            this.label5.Text = "MinSalary";
            // 
            // mtbDesignationLvl
            // 
            this.mtbDesignationLvl.Location = new System.Drawing.Point(105, 138);
            this.mtbDesignationLvl.Name = "mtbDesignationLvl";
            this.mtbDesignationLvl.Size = new System.Drawing.Size(94, 20);
            this.mtbDesignationLvl.TabIndex = 4;
            // 
            // txtMaxSalary
            // 
            this.txtMaxSalary.Location = new System.Drawing.Point(105, 85);
            this.txtMaxSalary.Name = "txtMaxSalary";
            this.txtMaxSalary.Size = new System.Drawing.Size(94, 20);
            this.txtMaxSalary.TabIndex = 2;
            // 
            // txtMinSalary
            // 
            this.txtMinSalary.Location = new System.Drawing.Point(105, 111);
            this.txtMinSalary.Name = "txtMinSalary";
            this.txtMinSalary.Size = new System.Drawing.Size(94, 20);
            this.txtMinSalary.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(606, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 8;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // designationGridView
            // 
            this.designationGridView.AllowUserToAddRows = false;
            this.designationGridView.AllowUserToDeleteRows = false;
            this.designationGridView.AutoGenerateColumns = false;
            this.designationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.designationGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.designationLevelDataGridViewTextBoxColumn,
            this.branchIdDataGridViewTextBoxColumn,
            this.maxSalaryDataGridViewTextBoxColumn,
            this.minSalaryDataGridViewTextBoxColumn});
            this.designationGridView.DataSource = this.designationGridVmBindingSource;
            this.designationGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.designationGridView.Location = new System.Drawing.Point(0, 235);
            this.designationGridView.Name = "designationGridView";
            this.designationGridView.ReadOnly = true;
            this.designationGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.designationGridView.Size = new System.Drawing.Size(683, 215);
            this.designationGridView.TabIndex = 9;
            this.designationGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.designationGridView_CellClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
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
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 125;
            // 
            // designationLevelDataGridViewTextBoxColumn
            // 
            this.designationLevelDataGridViewTextBoxColumn.DataPropertyName = "DesignationLevel";
            this.designationLevelDataGridViewTextBoxColumn.HeaderText = "Designation Level";
            this.designationLevelDataGridViewTextBoxColumn.Name = "designationLevelDataGridViewTextBoxColumn";
            this.designationLevelDataGridViewTextBoxColumn.ReadOnly = true;
            this.designationLevelDataGridViewTextBoxColumn.Width = 115;
            // 
            // branchIdDataGridViewTextBoxColumn
            // 
            this.branchIdDataGridViewTextBoxColumn.DataPropertyName = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.HeaderText = "BranchId";
            this.branchIdDataGridViewTextBoxColumn.Name = "branchIdDataGridViewTextBoxColumn";
            this.branchIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.branchIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // maxSalaryDataGridViewTextBoxColumn
            // 
            this.maxSalaryDataGridViewTextBoxColumn.DataPropertyName = "MaxSalary";
            this.maxSalaryDataGridViewTextBoxColumn.HeaderText = "Max Salary";
            this.maxSalaryDataGridViewTextBoxColumn.Name = "maxSalaryDataGridViewTextBoxColumn";
            this.maxSalaryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // minSalaryDataGridViewTextBoxColumn
            // 
            this.minSalaryDataGridViewTextBoxColumn.DataPropertyName = "MinSalary";
            this.minSalaryDataGridViewTextBoxColumn.HeaderText = "Min Salary";
            this.minSalaryDataGridViewTextBoxColumn.Name = "minSalaryDataGridViewTextBoxColumn";
            this.minSalaryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // designationGridVmBindingSource
            // 
            this.designationGridVmBindingSource.DataSource = typeof(Riddhasoft.EHajiriPro.Desktop.ViewModel.DesignationGridVm);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(2, 211);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(597, 20);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(193, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(105, 166);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(205, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 17);
            this.label10.TabIndex = 99;
            this.label10.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(254, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 17);
            this.label6.TabIndex = 100;
            this.label6.Text = "*";
            // 
            // DesignationFrm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.designationGridView);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtMinSalary);
            this.Controls.Add(this.txtMaxSalary);
            this.Controls.Add(this.mtbDesignationLvl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DesignationFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Designation";
            this.Load += new System.EventHandler(this.DesignationFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DesignationFrm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtbDesignationLvl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.designationGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.designationGridVmBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown mtbDesignationLvl;
        private System.Windows.Forms.TextBox txtMaxSalary;
        private System.Windows.Forms.TextBox txtMinSalary;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView designationGridView;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn designationLevelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxSalaryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minSalaryDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource designationGridVmBindingSource;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
    }
}