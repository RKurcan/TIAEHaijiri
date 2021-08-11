namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    partial class DashboardWidgetsMoreInfoFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardWidgetsMoreInfoFrm));
            this.dashboardInfoGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardInfoGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dashboardInfoGridView
            // 
            this.dashboardInfoGridView.AllowUserToAddRows = false;
            this.dashboardInfoGridView.AllowUserToDeleteRows = false;
            this.dashboardInfoGridView.AllowUserToOrderColumns = true;
            this.dashboardInfoGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dashboardInfoGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dashboardInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dashboardInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardInfoGridView.Location = new System.Drawing.Point(0, 0);
            this.dashboardInfoGridView.MultiSelect = false;
            this.dashboardInfoGridView.Name = "dashboardInfoGridView";
            this.dashboardInfoGridView.ReadOnly = true;
            this.dashboardInfoGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dashboardInfoGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dashboardInfoGridView.Size = new System.Drawing.Size(766, 418);
            this.dashboardInfoGridView.TabIndex = 87;
            // 
            // DashboardWidgetsMoreInfoFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 418);
            this.Controls.Add(this.dashboardInfoGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DashboardWidgetsMoreInfoFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dashboardInfoGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dashboardInfoGridView;
    }
}