namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    partial class ProductKeyRequestForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CompanyNameTxt = new System.Windows.Forms.TextBox();
            this.AddressTxt = new System.Windows.Forms.TextBox();
            this.AuthorizedMailtxt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.contactNoTxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtMac = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.CompanyNameTxt);
            this.groupBox3.Controls.Add(this.AddressTxt);
            this.groupBox3.Controls.Add(this.AuthorizedMailtxt);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.contactNoTxt);
            this.groupBox3.Location = new System.Drawing.Point(16, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(448, 161);
            this.groupBox3.TabIndex = 85;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Client Details";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Company";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Authorized Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Company Address";
            // 
            // CompanyNameTxt
            // 
            this.CompanyNameTxt.Location = new System.Drawing.Point(136, 23);
            this.CompanyNameTxt.Name = "CompanyNameTxt";
            this.CompanyNameTxt.Size = new System.Drawing.Size(274, 20);
            this.CompanyNameTxt.TabIndex = 0;
            // 
            // AddressTxt
            // 
            this.AddressTxt.Location = new System.Drawing.Point(136, 49);
            this.AddressTxt.Name = "AddressTxt";
            this.AddressTxt.Size = new System.Drawing.Size(274, 20);
            this.AddressTxt.TabIndex = 1;
            // 
            // AuthorizedMailtxt
            // 
            this.AuthorizedMailtxt.Location = new System.Drawing.Point(136, 77);
            this.AuthorizedMailtxt.Name = "AuthorizedMailtxt";
            this.AuthorizedMailtxt.Size = new System.Drawing.Size(274, 20);
            this.AuthorizedMailtxt.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Contact No";
            // 
            // contactNoTxt
            // 
            this.contactNoTxt.Location = new System.Drawing.Point(136, 101);
            this.contactNoTxt.Name = "contactNoTxt";
            this.contactNoTxt.Size = new System.Drawing.Size(274, 20);
            this.contactNoTxt.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(152, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(274, 23);
            this.button1.TabIndex = 84;
            this.button1.Text = "Send Request";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtMac
            // 
            this.txtMac.Location = new System.Drawing.Point(152, 189);
            this.txtMac.Name = "txtMac";
            this.txtMac.Size = new System.Drawing.Size(274, 20);
            this.txtMac.TabIndex = 87;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 86;
            this.label5.Text = "Your Mac Address";
            // 
            // ProductKeyRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 257);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtMac);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ProductKeyRequestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Key Request";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProductKeyRequestForm_FormClosed);
            this.Load += new System.EventHandler(this.ProductKeyRequestForm_Load_1);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CompanyNameTxt;
        private System.Windows.Forms.TextBox AddressTxt;
        private System.Windows.Forms.TextBox AuthorizedMailtxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox contactNoTxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMac;
        private System.Windows.Forms.Label label5;
    }
}