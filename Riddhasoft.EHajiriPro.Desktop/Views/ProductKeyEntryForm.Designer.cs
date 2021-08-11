namespace Riddhasoft.EHajiriPro.Desktop.Views
{
    partial class ProductKeyEntryForm
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
            this.RequestProductKeyBtn = new System.Windows.Forms.Button();
            this.btnValidateProductKey = new System.Windows.Forms.Button();
            this.txtProductKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RequestProductKeyBtn
            // 
            this.RequestProductKeyBtn.Location = new System.Drawing.Point(281, 105);
            this.RequestProductKeyBtn.Name = "RequestProductKeyBtn";
            this.RequestProductKeyBtn.Size = new System.Drawing.Size(131, 23);
            this.RequestProductKeyBtn.TabIndex = 13;
            this.RequestProductKeyBtn.Text = "Request Product Key";
            this.RequestProductKeyBtn.UseVisualStyleBackColor = true;
            this.RequestProductKeyBtn.Click += new System.EventHandler(this.RequestProductKeyBtn_Click);
            // 
            // btnValidateProductKey
            // 
            this.btnValidateProductKey.Location = new System.Drawing.Point(281, 74);
            this.btnValidateProductKey.Name = "btnValidateProductKey";
            this.btnValidateProductKey.Size = new System.Drawing.Size(131, 23);
            this.btnValidateProductKey.TabIndex = 14;
            this.btnValidateProductKey.Text = "validate";
            this.btnValidateProductKey.UseVisualStyleBackColor = true;
            this.btnValidateProductKey.Click += new System.EventHandler(this.btnValidateProductKey_Click);
            // 
            // txtProductKey
            // 
            this.txtProductKey.Location = new System.Drawing.Point(15, 48);
            this.txtProductKey.Name = "txtProductKey";
            this.txtProductKey.Size = new System.Drawing.Size(646, 20);
            this.txtProductKey.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(288, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Enter Your Product key";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(34, 167);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 90);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Powered By Riddha Soft";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Product : Hamro Hajiri Desktop";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Copyright © 2016 Attendance. All Rights Reserved";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Version 1.0";
            // 
            // ProductKeyEntryForm
            // 
            this.ClientSize = new System.Drawing.Size(673, 274);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RequestProductKeyBtn);
            this.Controls.Add(this.btnValidateProductKey);
            this.Controls.Add(this.txtProductKey);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ProductKeyEntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Key Entry";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProductKeyEntryForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RequestProductKeyBtn;
        private System.Windows.Forms.Button btnValidateProductKey;
        private System.Windows.Forms.TextBox txtProductKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}