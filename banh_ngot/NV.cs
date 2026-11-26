using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class NV : Form
    {
        private string tenTaiKhoan;

        private Dictionary<string, string> dsAnhThuCong = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "B01", "Ảnh1.jpg" }, { "B02", "Ảnh2.jpg" }, { "B03", "Ảnh3.jpg" },
            { "B04", "Ảnh4.jpg" }, { "B05", "Ảnh5.jpg" }, { "B06", "Ảnh6.png" },
            { "B07", "Ảnh7.png" }, { "B08", "Ảnh8.png" }, { "B09", "Ảnh9.png" },
            { "B10", "Ảnh10.png" }, { "B11", "Ảnh11.jpg" }, { "B12", "Ảnh12.jpg" },
            { "B13", "Ảnh13.jpg" }, { "B14", "Ảnh14.jpg" }, { "B15", "Ảnh15.jpg" }
        };

        public NV(string ten = "")
        {
            InitializeComponent();
            this.tenTaiKhoan = ten;

            // Gán tên trực tiếp ngay khi khởi tạo Form
            HienThiTenNguoiDung();
        }

        private void HienThiTenNguoiDung()
        {
            if (txt_ten != null)
            {
                txt_ten.Text = string.IsNullOrWhiteSpace(tenTaiKhoan) ? "Nhân viên" : tenTaiKhoan;
                txt_ten.Font = new Font(txt_ten.Font.FontFamily, 10f, FontStyle.Bold);
                txt_ten.TextAlign = HorizontalAlignment.Center;
            }
        }

        private void NV_Load(object sender, EventArgs e)
        {
            HienThiTenNguoiDung();

            // Bo tròn PictureBox avatar
            try
            {
                if (pt_1 != null && pt_1.Width > 0 && pt_1.Height > 0)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(0, 0, pt_1.Width, pt_1.Height);
                    pt_1.Region = new Region(path);
                    pt_1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch { }

            if (flowPanelSanPham != null)
            {
                flowPanelSanPham.AutoScroll = true;
                flowPanelSanPham.Focus();
            }

            // Tải danh sách bánh lên giao diện
            LoadDanhSachSanPhamKho();
        }

        // ================= CÁC SỰ KIỆN CLICK NÚT ================= //

        // Quản lý Kho: Đổi sang luồng Hide -> ShowDialog -> Show -> Reload đồng bộ toàn bộ App
        private void btn_qlk_Click(object sender, EventArgs e)
        {
            string nguoiDungHienTai = !string.IsNullOrWhiteSpace(this.tenTaiKhoan)
                ? this.tenTaiKhoan
                : (txt_ten != null ? txt_ten.Text.Trim() : "Nhân viên");

            QUAN_LI_KHO frmKho = new QUAN_LI_KHO(nguoiDungHienTai);
            this.Hide();
            frmKho.ShowDialog();
            this.Show();
            LoadDanhSachSanPhamKho(); // Cập nhật kho lập tức ngay khi vừa đóng form Kho
        }

        private void Bt_qld_Click(object sender, EventArgs e)
        {
            QUAN_LI_DON f = new QUAN_LI_DON();
            this.Hide();
            f.ShowDialog();
            this.Show();
            LoadDanhSachSanPhamKho();
        }

        private void Bt_qltk_Click(object sender, EventArgs e)
        {
            QUAN_LI_TK f = new QUAN_LI_TK();
            this.Hide();
            f.ShowDialog();
            this.Show();
            LoadDanhSachSanPhamKho();
        }

        private void Bt_qlhd_Click(object sender, EventArgs e)
        {
            QUAN_LI_TIEN f = new QUAN_LI_TIEN();
            this.Hide();
            f.ShowDialog();
            this.Show();
            LoadDanhSachSanPhamKho();
        }

        private void Bt_dx_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 f = new Form1();
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
        }

        // ================= HIỂN THỊ SẢN PHẨM ================= //

        public void LoadDanhSachSanPhamKho()
        {
            try
            {
                if (flowPanelSanPham == null) return;

                // Xóa và giải phóng bộ nhớ các Control/Hình ảnh cũ trước khi vẽ mới
                XoaControlCuTrongPanel();

                using (var db = new QuanlytiembanhEntities())
                {
                    // AsNoTracking() loại bỏ cache Entity Framework, buộc truy vấn dữ liệu mới nhất từ CSDL
                    var dsSanPham = db.SanPhams.AsNoTracking().ToList();

                    foreach (var sp in dsSanPham)
                    {
                        Panel card = TaoCardSanPham(sp);
                        flowPanelSanPham.Controls.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Giải phóng hình ảnh & control cũ tránh giật lag hoặc rò rỉ bộ nhớ (Memory Leak)
        private void XoaControlCuTrongPanel()
        {
            if (flowPanelSanPham == null) return;

            foreach (Control ctrl in flowPanelSanPham.Controls)
            {
                foreach (Control subCtrl in ctrl.Controls)
                {
                    if (subCtrl is PictureBox pic && pic.Image != null)
                    {
                        pic.Image.Dispose();
                        pic.Image = null;
                    }
                    subCtrl.Dispose();
                }
                ctrl.Dispose();
            }
            flowPanelSanPham.Controls.Clear();
        }

        private Panel TaoCardSanPham(SanPham sp)
        {
            Panel card = new Panel
            {
                Size = new Size(180, 320),
                Margin = new Padding(6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            PictureBox pic = new PictureBox
            {
                Size = new Size(160, 100),
                Location = new Point(10, 8),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            string maBanhClean = sp.MaBanh != null ? sp.MaBanh.Trim() : "";
            string tenFileAnh = dsAnhThuCong.ContainsKey(maBanhClean) ? dsAnhThuCong[maBanhClean] : "";
            LoadAnhVaoPictureBox(pic, tenFileAnh);

            Label lblTen = new Label
            {
                Text = string.IsNullOrEmpty(sp.TenBanh) ? "Chưa có tên" : sp.TenBanh.Trim(),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(5, 112),
                Size = new Size(170, 38),
                TextAlign = ContentAlignment.TopCenter
            };

            string maDM = string.IsNullOrEmpty(sp.MaDanhMuc) ? "N/A" : sp.MaDanhMuc.Trim();
            Label lblDanhMuc = new Label
            {
                Text = $"Danh mục: {maDM}",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                Location = new Point(8, 153),
                AutoSize = true
            };

            string dvt = string.IsNullOrEmpty(sp.DonViTinh) ? "" : sp.DonViTinh.Trim();
            int soLuongKho = sp.SoLuongKho ?? 0;
            Label lblKho = new Label
            {
                Text = $"Tồn kho: {soLuongKho} ({dvt})",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                // Nếu tồn kho = 0 thì báo màu đỏ, > 0 báo màu xanh
                ForeColor = soLuongKho > 0 ? Color.DarkGreen : Color.Red,
                Location = new Point(8, 175),
                AutoSize = true
            };

            decimal giaBan = sp.DonGiaBan ?? 0;
            Label lblGiaBan = new Label
            {
                Text = $"Giá bán: {giaBan:N0} VNĐ",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                Location = new Point(8, 197),
                AutoSize = true
            };

            decimal giaNhap = sp.DonGiaNhap ?? 0;
            Label lblGiaNhap = new Label
            {
                Text = $"Giá nhập: {giaNhap:N0} VNĐ",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Location = new Point(8, 219),
                AutoSize = true
            };

            // Xử lý an toàn khi HanSuDung bị NULL trong CSDL
            string strHSD = (sp.HanSuDung != null) ? $"{sp.HanSuDung} ngày" : "Chưa cập nhật";
            Label lblHSD = new Label
            {
                Text = $"Hạn dùng: {strHSD}",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                Location = new Point(8, 241),
                AutoSize = true
            };

            card.Controls.Add(pic);
            card.Controls.Add(lblTen);
            card.Controls.Add(lblDanhMuc);
            card.Controls.Add(lblKho);
            card.Controls.Add(lblGiaBan);
            card.Controls.Add(lblGiaNhap);
            card.Controls.Add(lblHSD);

            return card;
        }

        private void LoadAnhVaoPictureBox(PictureBox pic, string tenFileAnh)
        {
            if (string.IsNullOrEmpty(tenFileAnh)) return;
            try
            {
                string[] paths = new string[]
                {
                    Path.Combine(Application.StartupPath, "Anh", tenFileAnh),
                    Path.Combine(Application.StartupPath, "Images", tenFileAnh),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Anh", tenFileAnh),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Images", tenFileAnh)
                };

                string pathChon = paths.FirstOrDefault(p => File.Exists(p));
                if (!string.IsNullOrEmpty(pathChon))
                {
                    byte[] bytes = File.ReadAllBytes(pathChon);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        pic.Image = Image.FromStream(ms);
                    }
                }
            }
            catch { }
        }
    }
}