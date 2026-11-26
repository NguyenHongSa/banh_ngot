using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class QUAN_LI_TIEN : Form
    {
        // Lưu chế độ lọc hiện tại: "ALL", "XUAT", "NHAP"
        private string loaiLocHienTai = "ALL";

        public QUAN_LI_TIEN()
        {
            InitializeComponent();
        }

        private void QUAN_LI_TIEN_Load(object sender, EventArgs e)
        {
            LoadDataDoanhThu(loaiLocHienTai);
        }

        // 1. Tải và tổng hợp dữ liệu doanh thu
        private void LoadDataDoanhThu(string filterMode)
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    // A. Đơn bán ra từ GioHang: Lấy TẤT CẢ phương thức thanh toán (Tiền mặt, Chuyển khoản,...) 
                    // Miễn là thuộc trạng thái đã giao thành công / đã nhận hàng
                    var dsGioHang = db.GioHangs
                        .Where(g => g.TrangThai == "Đã nhận hàng" || g.TrangThai == "Giao hàng thành công")
                        .ToList()
                        .Select(g => new
                        {
                            MaGiaoDich = "GH_" + g.MaGioHang,
                            LoaiGiaoDich = "Bán ra (Xuất)",
                            LoaiPhieu = "XUAT",
                            NgayThucHien = g.NgayDat ?? g.NgayThem,
                            TongTien = (g.TongTien ?? 0) + (g.PhiShip ?? 0),
                            GhiChu = $"Khách: {g.HoTen} ({(g.TenDangNhap ?? "").Trim()})"
                        }).ToList();

                    // B. Hóa đơn nhập kho từ HoaDonKhoes
                    var dsHoaDonKho = db.HoaDonKhoes
                        .ToList()
                        .Select(h => new
                        {
                            MaGiaoDich = "HDK_" + h.MaPhieu,
                            LoaiGiaoDich = "Nhập kho",
                            LoaiPhieu = "NHAP",
                            NgayThucHien = (DateTime?)(h.NgayNhapXuat),
                            TongTien = Convert.ToDecimal(h.TongTien),
                            GhiChu = "Tiền trả nhập kho"
                        }).ToList();

                    // C. Tính toán ô TỔNG linh hoạt theo chế độ lọc hiện tại
                    decimal tongBanRa = dsGioHang.Sum(x => x.TongTien);
                    decimal tongNhapKho = dsHoaDonKho.Sum(x => x.TongTien);

                    if (filterMode == "XUAT")
                    {
                        txt_tong.Text = tongBanRa.ToString("#,##0 VNĐ");
                    }
                    else if (filterMode == "NHAP")
                    {
                        txt_tong.Text = tongNhapKho.ToString("#,##0 VNĐ");
                    }
                    else // Chế độ "ALL": Tính doanh thu ròng (Bán ra - Nhập kho)
                    {
                        decimal tongDoanhThuRong = tongBanRa - tongNhapKho;
                        txt_tong.Text = tongDoanhThuRong.ToString("#,##0 VNĐ");
                    }

                    // D. Gộp danh sách và lọc theo nút tùy chọn để hiển thị lên bảng
                    var dsTongHop = dsGioHang.Concat(dsHoaDonKho).AsQueryable();

                    if (filterMode == "XUAT")
                    {
                        dsTongHop = dsTongHop.Where(x => x.LoaiPhieu == "XUAT");
                    }
                    else if (filterMode == "NHAP")
                    {
                        dsTongHop = dsTongHop.Where(x => x.LoaiPhieu == "NHAP");
                    }

                    var ketQuaHienThi = dsTongHop
                        .OrderByDescending(x => x.NgayThucHien)
                        .Select(x => new
                        {
                            x.MaGiaoDich,
                            x.LoaiGiaoDich,
                            x.NgayThucHien,
                            x.TongTien,
                            x.GhiChu
                        }).ToList();

                    dt_tien.DataSource = null;
                    dt_tien.DataSource = ketQuaHienThi;

                    // E. Định dạng DataGridView
                    if (dt_tien.Columns["MaGiaoDich"] != null) dt_tien.Columns["MaGiaoDich"].HeaderText = "Mã Giao Dịch";
                    if (dt_tien.Columns["LoaiGiaoDich"] != null) dt_tien.Columns["LoaiGiaoDich"].HeaderText = "Loại Giao Dịch";
                    if (dt_tien.Columns["NgayThucHien"] != null)
                    {
                        dt_tien.Columns["NgayThucHien"].HeaderText = "Ngày Thực Hiện";
                        dt_tien.Columns["NgayThucHien"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                    if (dt_tien.Columns["TongTien"] != null)
                    {
                        dt_tien.Columns["TongTien"].HeaderText = "Tổng Tiền";
                        dt_tien.Columns["TongTien"].DefaultCellStyle.Format = "#,##0 VNĐ";
                    }
                    if (dt_tien.Columns["GhiChu"] != null) dt_tien.Columns["GhiChu"].HeaderText = "Ghi Chú";

                    dt_tien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Nút Tùy chọn (bt_tc): Menu chọn xem Tất cả / Bán ra / Nhập kho
        private void bt_tc_Click(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            var itemAll = contextMenu.Items.Add("Tất cả giao dịch");
            var itemXuat = contextMenu.Items.Add("Chỉ đơn Bán ra (Xuất)");
            var itemNhap = contextMenu.Items.Add("Chỉ hóa đơn Nhập kho");

            itemAll.Click += (s, ev) => { loaiLocHienTai = "ALL"; LoadDataDoanhThu(loaiLocHienTai); };
            itemXuat.Click += (s, ev) => { loaiLocHienTai = "XUAT"; LoadDataDoanhThu(loaiLocHienTai); };
            itemNhap.Click += (s, ev) => { loaiLocHienTai = "NHAP"; LoadDataDoanhThu(loaiLocHienTai); };

            contextMenu.Show(bt_tc, new System.Drawing.Point(0, bt_tc.Height));
        }

        // 3. NÚT "XEM HÓA ĐƠN" (bt_hd): Mở Menu chọn Xem Chi Tiết hoặc Xuất Excel
        private void bt_hd_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menuHoaDon = new ContextMenuStrip();

            var itemXemChiTiet = menuHoaDon.Items.Add("👁 Xem chi tiết hóa đơn đã chọn");
            var itemXuatExcel = menuHoaDon.Items.Add("📊 Xuất báo cáo ra file Excel");

            itemXemChiTiet.Click += (s, ev) => XemChiTietHoaDon();
            itemXuatExcel.Click += (s, ev) => XuatBaoCaoExcel();

            menuHoaDon.Show(bt_hd, new System.Drawing.Point(0, bt_hd.Height));
        }

        // Hàm 3A: Xem chi tiết 1 hóa đơn/giao dịch đang chọn trên bảng
        private void XemChiTietHoaDon()
        {
            if (dt_tien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn 1 dòng hóa đơn cần xem!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dt_tien.CurrentRow;
            string maGD = row.Cells["MaGiaoDich"].Value?.ToString() ?? "N/A";
            string loaiGD = row.Cells["LoaiGiaoDich"].Value?.ToString() ?? "N/A";
            string ngay = row.Cells["NgayThucHien"].Value != null ? Convert.ToDateTime(row.Cells["NgayThucHien"].Value).ToString("dd/MM/yyyy HH:mm") : "N/A";
            string tongTien = Convert.ToDecimal(row.Cells["TongTien"].Value ?? 0).ToString("#,##0 VNĐ");
            string ghiChu = row.Cells["GhiChu"].Value?.ToString() ?? "N/A";

            string thongTinChiTiet = $"=== CHI TIẾT GIAO DỊCH / HÓA ĐƠN ===\n\n" +
                                     $"• Mã Giao Dịch: {maGD}\n" +
                                     $"• Loại Giao Dịch: {loaiGD}\n" +
                                     $"• Ngày Thực Hiện: {ngay}\n" +
                                     $"• Số Tiền: {tongTien}\n" +
                                     $"• Ghi Chú: {ghiChu}\n\n" +
                                     $"----------------------------------------\n" +
                                     $"Giá trị hiển thị ở ô Tổng: {txt_tong.Text}";

            MessageBox.Show(thongTinChiTiet, "CHI TIẾT HÓA ĐƠN", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Hàm 3B: Xuất danh sách doanh thu ra Excel (.csv)
        private void XuatBaoCaoExcel()
        {
            if (dt_tien.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        // Tiêu đề cột
                        string[] columnHeaders = { "Mã Giao Dịch", "Loại Giao Dịch", "Ngày Thực Hiện", "Tổng Tiền (VNĐ)", "Ghi Chú" };
                        sw.WriteLine(string.Join(",", columnHeaders.Select(h => $"\"{h}\"")));

                        // Xuất từng dòng dữ liệu
                        foreach (DataGridViewRow row in dt_tien.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string maGD = row.Cells["MaGiaoDich"].Value?.ToString() ?? "";
                                string loaiGD = row.Cells["LoaiGiaoDich"].Value?.ToString() ?? "";
                                string ngay = row.Cells["NgayThucHien"].Value != null ? Convert.ToDateTime(row.Cells["NgayThucHien"].Value).ToString("dd/MM/yyyy HH:mm") : "";
                                string tongTien = Convert.ToDecimal(row.Cells["TongTien"].Value ?? 0).ToString("#,##0");
                                string ghiChu = row.Cells["GhiChu"].Value?.ToString() ?? "";

                                sw.WriteLine($"\"{maGD}\",\"{loaiGD}\",\"{ngay}\",\"{tongTien}\",\"{ghiChu}\"");
                            }
                        }

                        // Dòng tổng kết ở cuối
                        sw.WriteLine();
                        sw.WriteLine($"\"\",\"\",\"TỔNG GIÁ TRỊ:\",\"{txt_tong.Text}\",\"\"");
                    }

                    MessageBox.Show("Xuất báo cáo doanh thu ra file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 4. Nút Trở lại (bt_tl)
        private void bt_tl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}