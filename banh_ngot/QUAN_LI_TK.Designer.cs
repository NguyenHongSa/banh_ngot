namespace banh_ngot
{
    partial class QUAN_LI_TK
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
            this.bt_m = new System.Windows.Forms.Button();
            this.bt_d = new System.Windows.Forms.Button();
            this.dt_tk = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_tk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dt_tk)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_tk
            // 
            this.bt_tk.Location = new System.Drawing.Point(645, 98);
            this.bt_tk.Name = "bt_tk";
            this.bt_tk.Size = new System.Drawing.Size(146, 32);
            this.bt_tk.TabIndex = 68;
            this.bt_tk.Text = "tìm kiếm";
            this.bt_tk.UseVisualStyleBackColor = true;
            this.bt_tk.Click += new System.EventHandler(this.bt_tk_Click);
            // 
            // bt_tl
            // 
            this.bt_tl.Location = new System.Drawing.Point(12, 40);
            this.bt_tl.Name = "bt_tl";
            this.bt_tl.Size = new System.Drawing.Size(146, 32);
            this.bt_tl.TabIndex = 67;
            this.bt_tl.Text = "trở lại";
            this.bt_tl.UseVisualStyleBackColor = true;
            this.bt_tl.Click += new System.EventHandler(this.bt_tl_Click);
            // 
            // bt_m
            // 
            this.bt_m.Location = new System.Drawing.Point(444, 550);
            this.bt_m.Name = "bt_m";
            this.bt_m.Size = new System.Drawing.Size(146, 32);
            this.bt_m.TabIndex = 66;
            this.bt_m.Text = "Mở";
            this.bt_m.UseVisualStyleBackColor = true;
            this.bt_m.Click += new System.EventHandler(this.bt_m_Click);
            // 
            // bt_d
            // 
            this.bt_d.Location = new System.Drawing.Point(163, 550);
            this.bt_d.Name = "bt_d";
            this.bt_d.Size = new System.Drawing.Size(146, 32);
            this.bt_d.TabIndex = 65;
            this.bt_d.Text = "Đóng";
            this.bt_d.UseVisualStyleBackColor = true;
            this.bt_d.Click += new System.EventHandler(this.bt_d_Click);
            // 
            // dt_tk
            // 
            this.dt_tk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_tk.Location = new System.Drawing.Point(156, 182);
            this.dt_tk.Name = "dt_tk";
            this.dt_tk.RowHeadersWidth = 51;
            this.dt_tk.RowTemplate.Height = 24;
            this.dt_tk.Size = new System.Drawing.Size(478, 338);
            this.dt_tk.TabIndex = 64;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 63;
            this.label2.Text = "Tìm kiếm";
            // 
            // txt_tk
            // 
            this.txt_tk.Location = new System.Drawing.Point(126, 108);
            this.txt_tk.Name = "txt_tk";
            this.txt_tk.Size = new System.Drawing.Size(508, 22);
            this.txt_tk.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(208, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 32);
            this.label1.TabIndex = 61;
            this.label1.Text = "TÀI KHOẢN NGƯỜI DÙNG";
            // 
            // QUAN_LI_TK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 615);
            this.Controls.Add(this.bt_tk);
            this.Controls.Add(this.bt_tl);
            this.Controls.Add(this.bt_m);
            this.Controls.Add(this.bt_d);
            this.Controls.Add(this.dt_tk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_tk);
            this.Controls.Add(this.label1);
            this.Name = "QUAN_LI_TK";
            this.Text = "QUAN_LI_TK";
            this.Load += new System.EventHandler(this.QUAN_LI_TK_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_tk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_tk;
        private System.Windows.Forms.Button bt_tl;
        private System.Windows.Forms.Button bt_m;
        private System.Windows.Forms.Button bt_d;
        private System.Windows.Forms.DataGridView dt_tk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_tk;
        private System.Windows.Forms.Label label1;
    }
}