namespace FortniteAccountSwitcher
{
    partial class Form2
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
            this.linkGoogle = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAuthCode = new System.Windows.Forms.TextBox();
            this.txtAccountID = "";
            this.txtUsername = "";
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkGoogle
            // 
            this.linkGoogle.AutoSize = true;
            this.linkGoogle.Location = new System.Drawing.Point(12, 9);
            this.linkGoogle.Name = "linkGoogle";
            this.linkGoogle.Size = new System.Drawing.Size(72, 13);
            this.linkGoogle.TabIndex = 0;
            this.linkGoogle.TabStop = true;
            this.linkGoogle.Text = "google.com";
            this.linkGoogle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGoogle_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Add Code!";
            // 
            // txtAuthCode
            // 
            this.txtAuthCode.Location = new System.Drawing.Point(12, 47);
            this.txtAuthCode.Name = "txtAuthCode";
            this.txtAuthCode.Size = new System.Drawing.Size(100, 20);
            this.txtAuthCode.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(118, 45);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 80);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtAuthCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkGoogle);
            this.Name = "Form2";
            this.Text = "Add Account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkGoogle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAuthCode;
        private System.Windows.Forms.Button btnOk;
        public string txtUsername;
        public string txtAccountID;
    }
}