namespace banh_ngot
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.bt_dn = new System.Windows.Forms.Button();
            this.bt_dk = new System.Windows.Forms.Button();
            this.đ = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_role = new System.Windows.Forms.ComboBox();
            this.txt_ten = new System.Windows.Forms.TextBox();
            this.txt_mk = new System.Windows.Forms.TextBox();
            this.bt_qmk = new System.Windows.Forms.Button();
            this.pt_1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pt_1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_dn
            // 
            this.bt_dn.BackColor = System.Drawing.Color.Wheat;
            this.bt_dn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bt_dn.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_dn.Location = new System.Drawing.Point(215, 370);
            this.bt_dn.Name = "bt_dn";
            this.bt_dn.Size = new System.Drawing.Size(174, 57);
            this.bt_dn.TabIndex = 0;
            this.bt_dn.Text = "ĐĂNG NHẬP";
            this.bt_dn.UseVisualStyleBackColor = false;
            this.bt_dn.Click += new System.EventHandler(this.bt_dn_Click);
            // 
            // bt_dk
            // 
            this.bt_dk.Location = new System.Drawing.Point(158, 458);
            this.bt_dk.Name = "bt_dk";
            this.bt_dk.Size = new System.Drawing.Size(268, 32);
            this.bt_dk.TabIndex = 1;
            this.bt_dk.Text = "Bạn chưa có tài khoản? Đăng ký";
            this.bt_dk.UseVisualStyleBackColor = true;
            this.bt_dk.Click += new System.EventHandler(this.bt_dk_Click);
            // 
            // đ
            // 
            this.đ.AutoSize = true;
            this.đ.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.đ.Location = new System.Drawing.Point(176, 65);
            this.đ.Name = "đ";
            this.đ.Size = new System.Drawing.Size(0, 32);
            this.đ.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Vai trò :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(43, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tên tài khoản :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(43, 311);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 25);
            this.label4.TabIndex = 5;
            this.label4.Text = "Mật khẩu :";
            // 
            // cb_role
            // 
            this.cb_role.FormattingEnabled = true;
            this.cb_role.Items.AddRange(new object[] {
            "Khách hàng",
            "Quản lí"});
            this.cb_role.Location = new System.Drawing.Point(200, 187);
            this.cb_role.Name = "cb_role";
            this.cb_role.Size = new System.Drawing.Size(244, 24);
            this.cb_role.TabIndex = 6;
            // 
            // txt_ten
            // 
            this.txt_ten.Location = new System.Drawing.Point(200, 253);
            this.txt_ten.Name = "txt_ten";
            this.txt_ten.Size = new System.Drawing.Size(244, 22);
            this.txt_ten.TabIndex = 8;
            // 
            // txt_mk
            // 
            this.txt_mk.Location = new System.Drawing.Point(200, 311);
            this.txt_mk.Name = "txt_mk";
            this.txt_mk.Size = new System.Drawing.Size(244, 22);
            this.txt_mk.TabIndex = 10;
            // 
            // bt_qmk
            // 
            this.bt_qmk.Location = new System.Drawing.Point(158, 496);
            this.bt_qmk.Name = "bt_qmk";
            this.bt_qmk.Size = new System.Drawing.Size(268, 30);
            this.bt_qmk.TabIndex = 11;
            this.bt_qmk.Text = "Bạn quên mật khẩu?";
            this.bt_qmk.UseVisualStyleBackColor = true;
            this.bt_qmk.Click += new System.EventHandler(this.bt_qmk_Click);
            // 
            // pt_1
            // 
            this.pt_1.Image = ((System.Drawing.Image)(resources.GetObject("pt_1.Image")));
            this.pt_1.Location = new System.Drawing.Point(-10, -8);
            this.pt_1.Name = "pt_1";
            this.pt_1.Size = new System.Drawing.Size(604, 626);
            this.pt_1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pt_1.TabIndex = 12;
            this.pt_1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(151, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 40);
            this.label1.TabIndex = 13;
            this.label1.Text = "TIỆM BÁNH 2T2Y";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 608);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_qmk);
            this.Controls.Add(this.txt_mk);
            this.Controls.Add(this.txt_ten);
            this.Controls.Add(this.cb_role);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.đ);
            this.Controls.Add(this.bt_dk);
            this.Controls.Add(this.bt_dn);
            this.Controls.Add(this.pt_1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pt_1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_dn;
        private System.Windows.Forms.Button bt_dk;
        private System.Windows.Forms.Label đ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_role;
        private System.Windows.Forms.TextBox txt_ten;
        private System.Windows.Forms.TextBox txt_mk;
        private System.Windows.Forms.Button bt_qmk;
        private System.Windows.Forms.PictureBox pt_1;
        private System.Windows.Forms.Label label1;
    }
}

