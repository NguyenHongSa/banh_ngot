using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class DON_HANG : Form
    {
        private string tenDangNhapHienTai = "";

        public DON_HANG()
        {
            InitializeComponent();
        }

        public DON_HANG(string tenDangNhap)
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap != null ? tenDangNhap.Trim() : "";
        }

        private void DON_HANG_Load(object sender, EventArgs e)
        {
            // Chỉ tải danh sách đơn hàng, không hiển thị hộp thoại thông báo
            TaiDanhSachDonHang();
        }

        // 1. Hàm tải danh sách đơn hàng đang hoạt động (Hiển thị thêm Tên Bánh)
        private void TaiDanhSachDonHang(string maTimKiem = "")
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsGioHang = db.GioHangs.ToList();
                    var dsSanPham = db.SanPhams.ToList();

                    // Lọc chỉ lấy các đơn "Đã đặt hàng", "Đang giao", "Yêu cầu hủy"
                    var query = dsGioHang.Where(g => !string.IsNullOrEmpty(g.TrangThai) &&
                                                     (g.TrangThai.Trim().Equals("Đã đặt hàng", StringComparison.OrdinalIgnoreCase) ||
                                                      g.TrangThai.Trim().Equals("Đang giao", StringComparison.OrdinalIgnoreCase) ||
                                                      g.TrangThai.Trim().Equals("Đang giao hàng", StringComparison.OrdinalIgnoreCase) ||
                                                      g.TrangThai.Trim().Equals("Yêu cầu hủy", StringComparison.OrdinalIgnoreCase))).AsEnumerable();

                    if (!string.IsNullOrEmpty(tenDangNhapHienTai))
                    {
                        query = query.Where(g => g.TenDangNhap != null &&
                                                 g.TenDangNhap.Trim().Equals(tenDangNhapHienTai.Trim(), StringComparison.OrdinalIgnoreCase));
                    }

                    if (!string.IsNullOrEmpty(maTimKiem))
                    {
                        string tk = maTimKiem.Trim();
                        query = query.Where(g => g.MaGioHang.ToString().Contains(tk));
                    }

                    // Kết hợp bảng GioHang với SanPham để lấy Tên Bánh
                    var danhSach = (from g in query
                                    join sp in dsSanPham
                                    on (g.MaBanh ?? "").Trim().ToLower()
                                    equals (sp.MaBanh ?? "").Trim().ToLower() into groupSP
                                    from sp in groupSP.DefaultIfEmpty()
                                    select new
                                    {
                                        MaGioHang = g.MaGioHang,
                                        MaBanh = g.MaBanh != null ? g.MaBanh.Trim() : "",
                                        TenBanh = sp != null ? sp.TenBanh : "N/A",
                                        SoLuong = g.SoLuong,
                                        TongTien = g.TongTien,
                                        PhuongThucThanhToan = g.PhuongThucThanhToan,
                                        TrangThai = g.TrangThai,
                                        NgayDat = g.NgayDat,
                                        HoTen = g.HoTen,
                                        SDT = g.SDT,
                                        DiaChi = g.DiaChi
                                    }).OrderByDescending(g => g.NgayDat).ToList();

                    dt_gh.DataSource = null;
                    dt_gh.Columns.Clear();
                    dt_gh.AutoGenerateColumns = true;
                    dt_gh.DataSource = danhSach;

                    // Đặt lại tên tiêu đề cột hiển thị trên DataGridView
                    if (dt_gh.Columns["MaGioHang"] != null) dt_gh.Columns["MaGioHang"].HeaderText = "Mã Đơn";
                    if (dt_gh.Columns["MaBanh"] != null) dt_gh.Columns["MaBanh"].HeaderText = "Mã Bánh";
                    if (dt_gh.Columns["TenBanh"] != null) dt_gh.Columns["TenBanh"].HeaderText = "Tên Bánh";
                    if (dt_gh.Columns["SoLuong"] != null) dt_gh.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dt_gh.Columns["TongTien"] != null) dt_gh.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    if (dt_gh.Columns["PhuongThucThanhToan"] != null) dt_gh.Columns["PhuongThucThanhToan"].HeaderText = "Phương Thức TT";
                    if (dt_gh.Columns["TrangThai"] != null) dt_gh.Columns["TrangThai"].HeaderText = "Trạng Thái";
                    if (dt_gh.Columns["NgayDat"] != null)
                    {
                        dt_gh.Columns["NgayDat"].HeaderText = "Ngày Đặt";
                        dt_gh.Columns["NgayDat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                    if (dt_gh.Columns["HoTen"] != null) dt_gh.Columns["HoTen"].HeaderText = "Họ Tên";
                    if (dt_gh.Columns["SDT"] != null) dt_gh.Columns["SDT"].HeaderText = "SĐT";
                    if (dt_gh.Columns["DiaChi"] != null) dt_gh.Columns["DiaChi"].HeaderText = "Địa Chỉ";

                    dt_gh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Sự kiện nút Tìm kiếm (bt_tk)
        private void bt_tk_Click(object sender, EventArgs e)
        {
            string maCanTim = txt_tk.Text.Trim();
            TaiDanhSachDonHang(maCanTim);
        }

        // 3. Sự kiện nút "Đã nhận hàng" (bt_nh) - CHỈ CHO PHÉP ĐƠN ĐANG GIAO ĐƯỢC NHẬN
        private void bt_nh_Click(object sender, EventArgs e)
        {
            if (dt_gh.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần xác nhận đã nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cellTrangThai = dt_gh.CurrentRow.Cells["TrangThai"].Value?.ToString() ?? "";
            string trangThai = cellTrangThai.Trim();

            // Kiểm tra: CHỈ đơn hàng có trạng thái "Đang giao" hoặc "Đang giao hàng" mới được phép bấm xác nhận
            if (!trangThai.Equals("Đang giao", StringComparison.OrdinalIgnoreCase) &&
                !trangThai.Equals("Đang giao hàng", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"Đơn hàng đang ở trạng thái '{trangThai}'. Chỉ những đơn hàng đang 'Đang giao' mới có thể xác nhận đã nhận!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maGioHang = Convert.ToInt32(dt_gh.CurrentRow.Cells["MaGioHang"].Value);

            DialogResult dialogResult = MessageBox.Show(
                $"Xác nhận bạn đã nhận thành công đơn hàng mã #{maGioHang}?",
                "Xác nhận nhận hàng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (var db = new QuanlytiembanhEntities())
                    {
                        var dh = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                        if (dh != null)
                        {
                            // Đổi trạng thái sang "Đã nhận hàng"
                            dh.TrangThai = "Đã nhận hàng";
                            SetPropValue(dh, "NgayNhan", DateTime.Now);

                            // Trừ số lượng tồn kho sản phẩm
                            string maBanh = (dh.MaBanh ?? "").Trim();
                            var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim().Equals(maBanh, StringComparison.OrdinalIgnoreCase));

                            if (sp != null)
                            {
                                int soLuongMua = dh.SoLuong ?? 0;
                                int soLuongKhoHienTai = sp.SoLuongKho ?? 0;

                                if (soLuongKhoHienTai >= soLuongMua)
                                {
                                    sp.SoLuongKho = soLuongKhoHienTai - soLuongMua;
                                }
                                else
                                {
                                    sp.SoLuongKho = 0;
                                }
                            }

                            db.SaveChanges();

                            MessageBox.Show("Xác nhận đã nhận hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Tải lại danh sách (Đơn "Đã nhận hàng" sẽ tự động biến mất khỏi Form)
                            TaiDanhSachDonHang();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật trạng thái đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 4. Nút Hủy đơn (bt_huy)
        private void bt_huy_Click(object sender, EventArgs e)
        {
            if (dt_gh.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cellTrangThai = dt_gh.CurrentRow.Cells["TrangThai"].Value?.ToString() ?? "";
            string trangThai = cellTrangThai.Trim();

            // Chặn tuyệt đối nếu đơn hàng đã/đang trong quá trình giao
            if (trangThai.Equals("Đang giao", StringComparison.OrdinalIgnoreCase) ||
                trangThai.Equals("Đang giao hàng", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Đơn hàng đang trên đường giao, không thể gửi yêu cầu hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra điều kiện 3 ngày đối với các đơn "Đã đặt hàng"
            var cellNgayDat = dt_gh.CurrentRow.Cells["NgayDat"].Value;
            if (cellNgayDat != null && cellNgayDat != DBNull.Value && DateTime.TryParse(cellNgayDat.ToString(), out DateTime ngayDat))
            {
                if (trangThai.Equals("Đã đặt hàng", StringComparison.OrdinalIgnoreCase))
                {
                    double soNgay = (DateTime.Now - ngayDat).TotalDays;

                    if (soNgay >= 3)
                    {
                        MessageBox.Show("Đơn hàng đã đặt từ 3 ngày trở lên nên không thể hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (MessageBox.Show("Bạn có muốn gửi yêu cầu hủy đơn này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int maGioHang = Convert.ToInt32(dt_gh.CurrentRow.Cells["MaGioHang"].Value);

                using (var db = new QuanlytiembanhEntities())
                {
                    var don = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                    if (don != null)
                    {
                        don.TrangThai = "Yêu cầu hủy";
                        db.SaveChanges();
                        MessageBox.Show("Đã gửi yêu cầu hủy đơn thành công! Vui lòng chờ quản lý duyệt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDanhSachDonHang();
                    }
                }
            }
        }

        // 5. Nút Trở lại (txt_tl)
        private void txt_tl_Click(object sender, EventArgs e)
        {
            KH_1 formKH = new KH_1(tenDangNhapHienTai);
            formKH.Show();
            this.Close();
        }

        // 6. Nút Xem chi tiết (txt_ct)
        private void txt_ct_Click(object sender, EventArgs e)
        {
            if (dt_gh.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maGioHang = Convert.ToInt32(dt_gh.CurrentRow.Cells["MaGioHang"].Value);

            using (var db = new QuanlytiembanhEntities())
            {
                var dh = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                if (dh == null) return;

                string maBanh = dh.MaBanh != null ? dh.MaBanh.Trim() : "";
                var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == maBanh);
                string tenBanh = sp != null ? sp.TenBanh : maBanh;
                decimal donGia = sp != null ? (sp.DonGiaBan ?? 0) : 0;

                Form formHoaDon = new Form
                {
                    Text = "CHI TIẾT HÓA ĐƠN",
                    Size = new Size(420, 520),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                RichTextBox rtbHoaDon = new RichTextBox
                {
                    Dock = DockStyle.Top,
                    Height = 400,
                    ReadOnly = true,
                    Font = new Font("Consolas", 10, FontStyle.Regular)
                };

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("==========================================");
                sb.AppendLine("           HÓA ĐƠN BÁN HÀNG               ");
                sb.AppendLine("==========================================");
                sb.AppendLine($"Mã đơn hàng : {dh.MaGioHang}");
                sb.AppendLine($"Ngày đặt    : {dh.NgayDat:dd/MM/yyyy HH:mm}");
                sb.AppendLine($"Khách hàng  : {dh.HoTen}");
                sb.AppendLine($"Số điện thoại: {dh.SDT}");
                sb.AppendLine($"Địa chỉ     : {dh.DiaChi}");
                sb.AppendLine("------------------------------------------");
                sb.AppendLine($"Sản phẩm    : {tenBanh}");
                sb.AppendLine($"Số lượng    : {dh.SoLuong}");
                sb.AppendLine($"Đơn giá     : {donGia:N0} VNĐ");
                sb.AppendLine($"Phí vận chuyển: {(dh.PhiShip ?? 0):N0} VNĐ");
                sb.AppendLine("------------------------------------------");
                sb.AppendLine($"TỔNG TIỀN   : {(dh.TongTien ?? 0):N0} VNĐ");
                sb.AppendLine($"Thanh toán  : {dh.PhuongThucThanhToan}");
                sb.AppendLine($"Trạng thái  : {dh.TrangThai}");
                sb.AppendLine("==========================================");

                rtbHoaDon.Text = sb.ToString();

                Button btnXuatExcel = new Button
                {
                    Text = "Xuất Excel (.csv)",
                    Size = new Size(140, 35),
                    Location = new Point(130, 420)
                };

                // Logic cho nút Xuất Excel (.csv) khớp hoàn toàn với Popup CHI TIẾT HÓA ĐƠN
                // Logic xuất Excel (.csv) xếp theo chiều dọc (Cột A: Thông tin, Cột B: Giá trị)
                // Logic xuất Excel (.csv) có tiêu đề HÓA ĐƠN BÁN HÀNG ở trên cùng
                btnXuatExcel.Click += (s, ev) =>
                {
                    SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "Excel CSV File (*.csv)|*.csv",
                        FileName = $"HoaDon_{dh.MaGioHang}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                            {
                                // 1. Ghi tiêu đề HÓA ĐƠN BÁN HÀNG ở dòng đầu tiên
                                sw.WriteLine("\"HÓA ĐƠN BÁN HÀNG\"");
                                sw.WriteLine(); // Dòng trống phân cách tiêu đề với dữ liệu

                                // 2. Trích xuất dữ liệu từ đối tượng dh
                                string maDon = dh.MaGioHang.ToString();
                                string ngayDat = dh.NgayDat.HasValue ? dh.NgayDat.Value.ToString("dd/MM/yyyy HH:mm") : DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                string khachHang = !string.IsNullOrEmpty(dh.HoTen) ? dh.HoTen : "";
                                string sdt = !string.IsNullOrEmpty(dh.SDT) ? dh.SDT : "";
                                string diaChi = !string.IsNullOrEmpty(dh.DiaChi) ? dh.DiaChi : "";
                                int soLuong = dh.SoLuong ?? 1;
                                string phiShip = (dh.PhiShip ?? 0).ToString("#,##0") + " VNĐ";
                                string tongTien = (dh.TongTien ?? 0).ToString("#,##0") + " VNĐ";
                                string thanhToan = !string.IsNullOrEmpty(dh.PhuongThucThanhToan) ? dh.PhuongThucThanhToan : "";
                                string trangThai = !string.IsNullOrEmpty(dh.TrangThai) ? dh.TrangThai : "";

                                // 3. Ghi dữ liệu chi tiết theo từng dòng
                                sw.WriteLine($"\"Mã đơn hàng\",\"{maDon}\"");
                                sw.WriteLine($"\"Ngày đặt\",\"{ngayDat}\"");
                                sw.WriteLine($"\"Khách hàng\",\"{khachHang}\"");
                                sw.WriteLine($"\"Số điện thoại\",=\"{sdt}\""); // Dùng ="" để giữ nguyên số 0 đầu
                                sw.WriteLine($"\"Địa chỉ\",\"{diaChi}\"");
                                sw.WriteLine($"\"Mã bánh\",\"{maBanh}\"");
                                sw.WriteLine($"\"Sản phẩm\",\"{tenBanh}\"");
                                sw.WriteLine($"\"Số lượng\",\"{soLuong}\"");
                                sw.WriteLine($"\"Phí vận chuyển\",\"{phiShip}\"");
                                sw.WriteLine($"\"TỔNG TIỀN\",\"{tongTien}\"");
                                sw.WriteLine($"\"Thanh toán\",\"{thanhToan}\"");
                                sw.WriteLine($"\"Trạng thái\",\"{trangThai}\"");
                            }

                            MessageBox.Show("Xuất hóa đơn ra file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception exEx)
                        {
                            MessageBox.Show("Lỗi khi xuất file: " + exEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                formHoaDon.Controls.Add(rtbHoaDon);
                formHoaDon.Controls.Add(btnXuatExcel);
                formHoaDon.ShowDialog();
            }
        }

        // 7. Hàm bổ trợ SetPropValue
        private void SetPropValue(object obj, string propName, object value)
        {
            if (obj == null) return;
            var pi = obj.GetType().GetProperty(propName);
            if (pi != null && pi.CanWrite)
            {
                Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                pi.SetValue(obj, safeValue, null);
            }
        }
    }
}