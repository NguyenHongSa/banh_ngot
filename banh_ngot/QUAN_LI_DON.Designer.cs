namespace banh_ngot
{
    partial class QUAN_LI_DON
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
            this.bt_tk = new System.Windows.Forms.Button();
            this.bt_tl = new System.Windows.Forms.Button();
            this.bt_tc = new System.Windows.Forms.Button();
            this.bt_dd = new System.Windows.Forms.Button();
            this.dt_don = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_tk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_t = new System.Windows.Forms.Button();
            this.bt_ct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dt_don)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_tk
            // 
            this.bt_tk.Location = new System.Drawing.Point(756, 84);
            this.bt_tk.Name = "bt_tk";
            this.bt_tk.Size = new System.Drawing.Size(146, 32);
            this.bt_tk.TabIndex = 59;
            this.bt_tk.Text = "tìm kiếm";
            this.bt_tk.UseVisualStyleBackColor = true;
            this.bt_tk.Click += new System.EventHandler(this.bt_tk_Click);
            // 
            // bt_tl
            // 
            this.bt_tl.Location = new System.Drawing.Point(12, 22);
            this.bt_tl.Name = "bt_tl";
            this.bt_tl.Size = new System.Drawing.Size(146, 32);
            this.bt_tl.TabIndex = 58;
            this.bt_tl.Text = "trở lại";
            this.bt_tl.UseVisualStyleBackColor = true;
            this.bt_tl.Click += new System.EventHandler(this.bt_tl_Click);
            // 
            // bt_tc
            // 
            this.bt_tc.Location = new System.Drawing.Point(351, 525);
            this.bt_tc.Name = "bt_tc";
            this.bt_tc.Size = new System.Drawing.Size(146, 32);
            this.bt_tc.TabIndex = 57;
            this.bt_tc.Text = "Từ chối";
            this.bt_tc.UseVisualStyleBackColor = true;
            this.bt_tc.Click += new System.EventHandler(this.bt_tc_Click);
            // 
            // bt_dd
            // 
            this.bt_dd.Location = new System.Drawing.Point(70, 525);
            this.bt_dd.Name = "bt_dd";
            this.bt_dd.Size = new System.Drawing.Size(146, 32);
            this.bt_dd.TabIndex = 56;
            this.bt_dd.Text = "Duyệt đơn";
            this.bt_dd.UseVisualStyleBackColor = true;
            this.bt_dd.Click += new System.EventHandler(this.bt_dd_Click);
            // 
            // dt_don
            // 
            this.dt_don.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_don.Location = new System.Drawing.Point(12, 164);
            this.dt_don.Name = "dt_don";
            this.dt_don.RowHeadersWidth = 51;
            this.dt_don.RowTemplate.Height = 24;
            this.dt_don.Size = new System.Drawing.Size(1029, 338);
            this.dt_don.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 54;
            this.label2.Text = "Tìm kiếm";
            // 
            // txt_tk
            // 
            this.txt_tk.Location = new System.Drawing.Point(322, 94);
            this.txt_tk.Name = "txt_tk";
            this.txt_tk.Size = new System.Drawing.Size(398, 22);
            this.txt_tk.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(430, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 32);
            this.label1.TabIndex = 52;
            this.label1.Text = "ĐƠN HÀNG";
            // 
            // bt_t
            // 
            this.bt_t.Location = new System.Drawing.Point(799, 525);
            this.bt_t.Name = "bt_t";
            this.bt_t.Size = new System.Drawing.Size(146, 32);
            this.bt_t.TabIndex = 60;
            this.bt_t.Text = "Tùy chọn";
            this.bt_t.UseVisualStyleBackColor = true;
            this.bt_t.Click += new System.EventHandler(this.bt_t_Click);
            // 
            // bt_ct
            // 
            this.bt_ct.Location = new System.Drawing.Point(565, 525);
            this.bt_ct.Name = "bt_ct";
            this.bt_ct.Size = new System.Drawing.Size(146, 32);
            this.bt_ct.TabIndex = 61;
            this.bt_ct.Text = "Xem chi tiết";
            this.bt_ct.UseVisualStyleBackColor = true;
            this.bt_ct.Click += new System.EventHandler(this.bt_ct_Click);
            // 
            // QUAN_LI_DON
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 590);
            this.Controls.Add(this.bt_ct);
            this.Controls.Add(this.bt_t);
            this.Controls.Add(this.bt_tk);
            this.Controls.Add(this.bt_tl);
            this.Controls.Add(this.bt_tc);
            this.Controls.Add(this.bt_dd);
            this.Controls.Add(this.dt_don);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_tk);
            this.Controls.Add(this.label1);
            this.Name = "QUAN_LI_DON";
            this.Text = "QUAN_LI_DON";
            this.Load += new System.EventHandler(this.QUAN_LI_DON_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_don)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bt_tk;
        private System.Windows.Forms.Button bt_tl;
        private System.Windows.Forms.Button bt_tc;
        private System.Windows.Forms.Button bt_dd;
        private System.Windows.Forms.DataGridView dt_don;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_tk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_t;
        private System.Windows.Forms.Button bt_ct;
    }
}