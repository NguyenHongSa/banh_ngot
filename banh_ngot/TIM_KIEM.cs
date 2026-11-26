using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class TIM_KIEM : Form
    {
        private string tenDangNhapHienTai;
        private SanPham sanPhamTimDuoc = null;

        // Bảng ánh xạ mã bánh với tên file ảnh
        private Dictionary<string, string> dsAnhThuCong = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "B01", "Ảnh1.jpg" }, { "B02", "Ảnh2.jpg" }, { "B03", "Ảnh3.jpg" },
            { "B04", "Ảnh4.jpg" }, { "B05", "Ảnh5.jpg" }, { "B06", "Ảnh6.png" },
            { "B07", "Ảnh7.png" }, { "B08", "Ảnh8.png" }, { "B09", "Ảnh9.png" },
            { "B10", "Ảnh10.png" }, { "B11", "Ảnh11.jpg" }, { "B12", "Ảnh12.jpg" },
            { "B13", "Ảnh13.jpg" }, { "B14", "Ảnh14.jpg" }, { "B15", "Ảnh15.jpg" }
        };

        public TIM_KIEM(string tenDangNhap = "")
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap;
        }

        public TIM_KIEM()
        {
            InitializeComponent();
        }

        // ================= 1. NÚT TÌM KIẾM (bt_tk) ================= //
        private void bt_tk_Click(object sender, EventArgs e)
        {
            string tuKhoa = txt_ten != null ? txt_ten.Text.Trim() : "";

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập tên bánh cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    sanPhamTimDuoc = db.SanPhams.AsNoTracking()
                        .FirstOrDefault(sp => sp.TenBanh.ToLower().Contains(tuKhoa.ToLower()));

                    if (sanPhamTimDuoc == null)
                    {
                        MessageBox.Show("Tiệm không có bánh này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        XoaThongTinHienThi();
                        return;
                    }

                    HienThiThongTinBanh(sanPhamTimDuoc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Điền thông tin vào TextBox và PictureBox
        private void HienThiThongTinBanh(SanPham sp)
        {
            if (txt_banh != null) txt_banh.Text = sp.TenBanh;
            if (txt_ma != null) txt_ma.Text = sp.MaBanh != null ? sp.MaBanh.Trim() : "";
            if (txt_dv != null) txt_dv.Text = sp.DonViTinh;
            if (txt_sl != null) txt_sl.Text = (sp.SoLuongKho ?? 0).ToString();
            if (txt_gia != null) txt_gia.Text = string.Format("{0:N0} VNĐ", sp.DonGiaBan);

            if (pt_1 != null)
            {
                string maBanhClean = sp.MaBanh != null ? sp.MaBanh.Trim() : "";
                string tenFileAnh = dsAnhThuCong.ContainsKey(maBanhClean) ? dsAnhThuCong[maBanhClean] : "";
                LoadAnhVaoPictureBox(pt_1, tenFileAnh);
            }
        }

        private void XoaThongTinHienThi()
        {
            if (txt_banh != null) txt_banh.Clear();
            if (txt_ma != null) txt_ma.Clear();
            if (txt_dv != null) txt_dv.Clear();
            if (txt_sl != null) txt_sl.Clear();
            if (txt_gia != null) txt_gia.Clear();
            if (pt_1 != null) pt_1.Image = null;
            sanPhamTimDuoc = null;
        }

        // ================= 2. NÚT TRỞ LẠI (bt_tl) ================= //
        private void bt_tl_Click(object sender, EventArgs e)
        {
            KH_1 frmKH = new KH_1(tenDangNhapHienTai);
            frmKH.Show();
            this.Close();
        }

        // ================= 3. NÚT THÊM VÀO GIỎ HÀNG (bt_them) ================= //
        private void bt_them_Click(object sender, EventArgs e)
        {
            if (sanPhamTimDuoc == null)
            {
                MessageBox.Show("Vui lòng tìm kiếm sản phẩm trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(tenDangNhapHienTai))
            {
                MessageBox.Show("Vui lòng đăng nhập lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    string maBanh = sanPhamTimDuoc.MaBanh.Trim();

                    var itemInCart = db.GioHangs.FirstOrDefault(g =>
                        g.TenDangNhap != null && g.TenDangNhap.Trim().Equals(tenDangNhapHienTai, StringComparison.OrdinalIgnoreCase) &&
                        g.MaBanh != null && g.MaBanh.Trim().Equals(maBanh, StringComparison.OrdinalIgnoreCase) &&
                        (g.TrangThai == null || g.TrangThai.Trim().Equals("Chờ đặt hàng", StringComparison.OrdinalIgnoreCase)));

                    if (itemInCart != null)
                    {
                        itemInCart.SoLuong += 1;
                        itemInCart.NgayThem = DateTime.Now;
                    }
                    else
                    {
                        GioHang gh = new GioHang
                        {
                            TenDangNhap = tenDangNhapHienTai,
                            MaBanh = maBanh,
                            SoLuong = 1,
                            NgayThem = DateTime.Now,
                            TrangThai = "Chờ đặt hàng"
                        };
                        db.GioHangs.Add(gh);
                    }

                    db.SaveChanges();
                    MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm giỏ hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================= 4. NÚT MUA HÀNG (bt_mua) ================= //
        private void bt_mua_Click(object sender, EventArgs e)
        {
            if (sanPhamTimDuoc == null)
            {
                MessageBox.Show("Vui lòng tìm kiếm sản phẩm trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(tenDangNhapHienTai))
            {
                MessageBox.Show("Vui lòng đăng nhập lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    GioHang dhMoi = new GioHang
                    {
                        MaBanh = sanPhamTimDuoc.MaBanh.Trim(),
                        TenDangNhap = tenDangNhapHienTai,
                        SoLuong = 1,
                        TrangThai = "Chờ xác nhận",
                        NgayDat = DateTime.Now
                    };

                    db.GioHangs.Add(dhMoi);
                    db.SaveChanges();
                }

                XAC_NHAN_MUA_HANG frmXacNhan = new XAC_NHAN_MUA_HANG(tenDangNhapHienTai);
                frmXacNhan.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý mua hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm hỗ trợ đọc và hiển thị hình ảnh
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
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch { }
        }
    }
}