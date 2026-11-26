namespace banh_ngot
{
    partial class DON_HANG
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
            this.bt_tl = new System.Windows.Forms.Button();
            this.bt_huy = new System.Windows.Forms.Button();
            this.dt_gh = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_tk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_tk = new System.Windows.Forms.Button();
            this.bt_dn = new System.Windows.Forms.Button();
            this.bt_ct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dt_gh)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_tl
            // 
            this.bt_tl.Location = new System.Drawing.Point(136, 44);
            this.bt_tl.Name = "bt_tl";
            this.bt_tl.Size = new System.Drawing.Size(146, 32);
            this.bt_tl.TabIndex = 49;
            this.bt_tl.Text = "trở lại";
            this.bt_tl.UseVisualStyleBackColor = true;
            this.bt_tl.Click += new System.EventHandler(this.txt_tl_Click);
            // 
            // bt_huy
            // 
            this.bt_huy.Location = new System.Drawing.Point(756, 550);
            this.bt_huy.Name = "bt_huy";
            this.bt_huy.Size = new System.Drawing.Size(146, 32);
            this.bt_huy.TabIndex = 48;
            this.bt_huy.Text = "hủy đơn";
            this.bt_huy.UseVisualStyleBackColor = true;
            this.bt_huy.Click += new System.EventHandler(this.bt_huy_Click);
            // 
            // dt_gh
            // 
            this.dt_gh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_gh.Location = new System.Drawing.Point(12, 186);
            this.dt_gh.Name = "dt_gh";
            this.dt_gh.RowHeadersWidth = 51;
            this.dt_gh.RowTemplate.Height = 24;
            this.dt_gh.Size = new System.Drawing.Size(1029, 338);
            this.dt_gh.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 45;
            this.label2.Text = "Tìm kiếm";
            // 
            // txt_tk
            // 
            this.txt_tk.Location = new System.Drawing.Point(378, 116);
            this.txt_tk.Name = "txt_tk";
            this.txt_tk.Size = new System.Drawing.Size(398, 22);
            this.txt_tk.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(519, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 32);
            this.label1.TabIndex = 43;
            this.label1.Text = "ĐƠN HÀNG";
            // 
            // bt_tk
            // 
            this.bt_tk.Location = new System.Drawing.Point(812, 106);
            this.bt_tk.Name = "bt_tk";
            this.bt_tk.Size = new System.Drawing.Size(146, 32);
            this.bt_tk.TabIndex = 50;
            this.bt_tk.Text = "tìm kiếm";
            this.bt_tk.UseVisualStyleBackColor = true;
            this.bt_tk.Click += new System.EventHandler(this.bt_tk_Click);
            // 
            // bt_dn
            // 
            this.bt_dn.Location = new System.Drawing.Point(284, 550);
            this.bt_dn.Name = "bt_dn";
            this.bt_dn.Size = new System.Drawing.Size(146, 32);
            this.bt_dn.TabIndex = 51;
            this.bt_dn.Text = "đã nhận hàng";
            this.bt_dn.UseVisualStyleBackColor = true;
            this.bt_dn.Click += new System.EventHandler(this.bt_nh_Click);
            // 
            // bt_ct
            // 
            this.bt_ct.Location = new System.Drawing.Point(512, 550);
            this.bt_ct.Name = "bt_ct";
            this.bt_ct.Size = new System.Drawing.Size(146, 32);
            this.bt_ct.TabIndex = 47;
            this.bt_ct.Text = "xem chi tiết";
            this.bt_ct.UseVisualStyleBackColor = true;
            this.bt_ct.Click += new System.EventHandler(this.txt_ct_Click);
            // 
            // DON_HANG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 627);
            this.Controls.Add(this.bt_dn);
            this.Controls.Add(this.bt_tk);
            this.Controls.Add(this.bt_tl);
            this.Controls.Add(this.bt_huy);
            this.Controls.Add(this.bt_ct);
            this.Controls.Add(this.dt_gh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_tk);
            this.Controls.Add(this.label1);
            this.Name = "DON_HANG";
            this.Text = "DON_HANG";
            this.Load += new System.EventHandler(this.DON_HANG_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_gh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bt_tl;
        private System.Windows.Forms.Button bt_huy;
        private System.Windows.Forms.DataGridView dt_gh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_tk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_tk;
        private System.Windows.Forms.Button bt_dn;
        private System.Windows.Forms.Button bt_ct;
    }
}