using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class GIO_HANG : Form
    {
        private string tenDangNhapHienTai;

        public GIO_HANG(string tenDangNhap)
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap != null ? tenDangNhap.Trim() : "";
        }

        public GIO_HANG()
        {
            InitializeComponent();
        }

        private void GIO_HANG_Load(object sender, EventArgs e)
        {
            LoadDataGioHang();
        }

        // 1. Tải dữ liệu giỏ hàng - CHỈ LỌC THEO MÃ SỐ GIỎ HÀNG
        public void LoadDataGioHang(string maTimKiem = "")
        {
            if (string.IsNullOrEmpty(tenDangNhapHienTai))
            {
                MessageBox.Show("Chưa nhận diện được tài khoản đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsGioHang = db.GioHangs.ToList();
                    var dsSanPham = db.SanPhams.ToList();

                    var query = dsGioHang.Where(gh => gh.TenDangNhap != null &&
                                                      gh.TenDangNhap.Trim().Equals(tenDangNhapHienTai, StringComparison.OrdinalIgnoreCase) &&
                                                      (string.IsNullOrEmpty(gh.TrangThai) || gh.TrangThai.Trim().Equals("Chờ đặt hàng", StringComparison.OrdinalIgnoreCase)));

                    if (!string.IsNullOrEmpty(maTimKiem))
                    {
                        string tk = maTimKiem.Trim();
                        query = query.Where(gh => gh.MaGioHang.ToString().Contains(tk));
                    }

                    var danhSachHienThi = (from gh in query
                                           join sp in dsSanPham
                                           on (gh.MaBanh ?? "").Trim().ToLower()
                                           equals (sp.MaBanh ?? "").Trim().ToLower() into groupSP
                                           from sp in groupSP.DefaultIfEmpty()
                                           select new
                                           {
                                               MaGioHang = gh.MaGioHang,
                                               MaBanh = (gh.MaBanh ?? "").Trim(),
                                               TenBanh = sp != null ? sp.TenBanh : "N/A",
                                               DonGia = sp != null ? (sp.DonGiaBan ?? 0) : 0,
                                               SoLuong = gh.SoLuong ?? 0,
                                               ThanhTien = (gh.SoLuong ?? 0) * (sp != null ? (sp.DonGiaBan ?? 0) : 0),
                                               PhuongThucThanhToan = gh.PhuongThucThanhToan ?? "Chưa chọn",
                                               TrangThai = string.IsNullOrEmpty(gh.TrangThai) ? "Chờ đặt hàng" : gh.TrangThai,
                                               NgayThem = gh.NgayThem
                                           }).ToList();

                    dt_gh.DataSource = null;
                    dt_gh.DataSource = danhSachHienThi;

                    if (dt_gh.Columns["MaGioHang"] != null) dt_gh.Columns["MaGioHang"].HeaderText = "Mã Giỏ Hàng";
                    if (dt_gh.Columns["MaBanh"] != null) dt_gh.Columns["MaBanh"].HeaderText = "Mã Bánh";
                    if (dt_gh.Columns["TenBanh"] != null) dt_gh.Columns["TenBanh"].HeaderText = "Tên Bánh";
                    if (dt_gh.Columns["DonGia"] != null) dt_gh.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dt_gh.Columns["SoLuong"] != null) dt_gh.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dt_gh.Columns["ThanhTien"] != null) dt_gh.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                    if (dt_gh.Columns["PhuongThucThanhToan"] != null) dt_gh.Columns["PhuongThucThanhToan"].HeaderText = "Phương Thức TT";
                    if (dt_gh.Columns["TrangThai"] != null) dt_gh.Columns["TrangThai"].HeaderText = "Trạng Thái";
                    if (dt_gh.Columns["NgayThem"] != null) dt_gh.Columns["NgayThem"].HeaderText = "Ngày Thêm";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giỏ hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Sự kiện nhấn nút Tìm kiếm
        private void bt_tk_Click(object sender, EventArgs e)
        {
            string maCanTim = txt_tk.Text.Trim();
            LoadDataGioHang(maCanTim);
        }

        // 3. NÚT MUA HÀNG (ĐÃ SỬA: Lấy MaGioHang đang chọn và truyền sang Form Xác Nhận)
        private void bt_muahang_Click(object sender, EventArgs e)
        {
            if (dt_gh.CurrentRow == null || dt_gh.CurrentRow.Cells["MaGioHang"].Value == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần mua!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy Mã Giỏ Hàng được chọn trên bảng
                int maGioHangChon = Convert.ToInt32(dt_gh.CurrentRow.Cells["MaGioHang"].Value);

                // Mở Form Xác Nhận Mua Hàng truyền cả tên đăng nhập VÀ mã giỏ hàng
                XAC_NHAN_MUA_HANG frmXacNhan = new XAC_NHAN_MUA_HANG(tenDangNhapHienTai, maGioHangChon);
                frmXacNhan.ShowDialog();

                // Làm mới lại bảng giỏ hàng sau khi hoàn tất đặt hàng
                LoadDataGioHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. NÚT HỦY ĐƠN (XÓA DỮ LIỆU KHỎI CSDL)
        private void bt_huy_Click(object sender, EventArgs e)
        {
            if (dt_gh.CurrentRow == null || dt_gh.CurrentRow.Cells["MaGioHang"].Value == null)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa khỏi giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa món này khỏi giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int maGioHang = Convert.ToInt32(dt_gh.CurrentRow.Cells["MaGioHang"].Value);

                    using (var db = new QuanlytiembanhEntities())
                    {
                        var item = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                        if (item != null)
                        {
                            db.GioHangs.Remove(item); // Xóa dòng dữ liệu khỏi SQL Server
                            db.SaveChanges();
                        }
                    }

                    MessageBox.Show("Đã xóa món khỏi giỏ hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGioHang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 5. NÚT TRỜ LẠI
        private void bt_tl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}