using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class KH_1 : Form
    {
        private string tenDangNhapHienTai;

        public KH_1(string tenDangNhap)
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap != null ? tenDangNhap.Trim() : "";
            LoadDanhSachSanPham();
        }

        public KH_1()
        {
            InitializeComponent();
            LoadDanhSachSanPham();
        }

        // Sự kiện Load Form
        private void KH_1_Load(object sender, EventArgs e)
        {
            CapNhatHinhTron_pt1();
            HienThiTenNguoiDung(); // Gọi hàm điền tên và căn chỉnh định dạng
        }

        // Hàm hiển thị tên người dùng (Căn giữa & In đậm)
        private void HienThiTenNguoiDung()
        {
            if (txt_ten != null)
            {
                txt_ten.Text = tenDangNhapHienTai;

                // Căn giữa dòng chữ
                txt_ten.TextAlign = HorizontalAlignment.Center;

                // Định dạng phông chữ in đậm (Bold)
                txt_ten.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
        }

        // Hàm hỗ trợ bo tròn PictureBox pt_1
        private void CapNhatHinhTron_pt1()
        {
            if (pt_1 != null && pt_1.Width > 0 && pt_1.Height > 0)
            {
                pt_1.SizeMode = PictureBoxSizeMode.StretchImage;
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, pt_1.Width, pt_1.Height);
                    pt_1.Region = new Region(path);
                }
            }
        }

        // Tự động bo tròn lại nếu pt_1 bị thay đổi kích thước
        private void pt_1_SizeChanged(object sender, EventArgs e)
        {
            CapNhatHinhTron_pt1();
        }

        // Danh sách ánh xạ 15 bánh với tên file ảnh
        private Dictionary<string, string> dsAnhThuCong = new Dictionary<string, string>()
        {
            { "B01", "Ảnh1.jpg" },
            { "B02", "Ảnh2.jpg" },
            { "B03", "Ảnh3.jpg" },
            { "B04", "Ảnh4.jpg" },
            { "B05", "Ảnh5.jpg" },
            { "B06", "Ảnh6.png" },
            { "B07", "Ảnh7.png" },
            { "B08", "Ảnh8.png" },
            { "B09", "Ảnh9.png" },
            { "B10", "Ảnh10.png" },
            { "B11", "Ảnh11.jpg" },
            { "B12", "Ảnh12.jpg" },
            { "B13", "Ảnh13.jpg" },
            { "B14", "Ảnh14.jpg" },
            { "B15", "Ảnh15.jpg" }
        };

        // Hàm load toàn bộ bánh lên flowPanelSanPham
        private void LoadDanhSachSanPham()
        {
            flowPanelSanPham.Controls.Clear();

            using (var db = new QuanlytiembanhEntities())
            {
                var danhSachBanh = db.SanPhams.ToList();

                foreach (var banh in danhSachBanh)
                {
                    string maBanhChuan = banh.MaBanh != null ? banh.MaBanh.Trim() : "";

                    // 1. Khung chứa bánh
                    Panel pnlItem = new Panel
                    {
                        Width = 160,
                        Height = 225,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        Margin = new Padding(10)
                    };

                    // 2. Hình ảnh bánh
                    PictureBox picBanh = new PictureBox
                    {
                        Width = 140,
                        Height = 95,
                        Left = 9,
                        Top = 8,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };

                    try
                    {
                        if (dsAnhThuCong.ContainsKey(maBanhChuan))
                        {
                            string tenFileAnh = dsAnhThuCong[maBanhChuan];
                            string folderAnh = Path.Combine(Application.StartupPath, "Anh");

                            if (!Directory.Exists(folderAnh))
                            {
                                folderAnh = Path.Combine(Application.StartupPath, @"..\..\Anh");
                            }

                            string fullPath = Path.Combine(folderAnh, tenFileAnh);
                            if (File.Exists(fullPath))
                            {
                                picBanh.Image = Image.FromFile(fullPath);
                            }
                            else
                            {
                                picBanh.BackColor = Color.LightGray;
                            }
                        }
                    }
                    catch
                    {
                        picBanh.BackColor = Color.LightGray;
                    }

                    // 3. Tên bánh
                    Label lblTen = new Label
                    {
                        Text = banh.TenBanh,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Width = 140,
                        Height = 32,
                        Left = 9,
                        Top = 105,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    // 4. Giá và Kho
                    Label lblThongTin = new Label
                    {
                        Text = $"Giá: {banh.DonGiaBan:N0} đ | Kho: {banh.SoLuongKho}",
                        Font = new Font("Segoe UI", 8, FontStyle.Regular),
                        ForeColor = Color.DarkRed,
                        Width = 140,
                        Height = 22,
                        Left = 9,
                        Top = 140,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    // 5. Nút "Thêm giỏ"
                    Button btnThemGio = new Button
                    {
                        Text = "Thêm giỏ",
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        Width = 68,
                        Height = 30,
                        Left = 8,
                        Top = 180,
                        BackColor = Color.Orange,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Tag = maBanhChuan
                    };
                    btnThemGio.FlatAppearance.BorderSize = 0;
                    btnThemGio.Click += BtnThemGio_Click;

                    // 6. Nút "Mua ngay"
                    Button btnMuaNgay = new Button
                    {
                        Text = "Mua ngay",
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        Width = 68,
                        Height = 30,
                        Left = 82,
                        Top = 180,
                        BackColor = Color.OrangeRed,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Tag = maBanhChuan
                    };
                    btnMuaNgay.FlatAppearance.BorderSize = 0;
                    btnMuaNgay.Click += BtnMuaNgay_Click;

                    // Gom các thành phần vào Panel
                    pnlItem.Controls.Add(picBanh);
                    pnlItem.Controls.Add(lblTen);
                    pnlItem.Controls.Add(lblThongTin);
                    pnlItem.Controls.Add(btnThemGio);
                    pnlItem.Controls.Add(btnMuaNgay);

                    flowPanelSanPham.Controls.Add(pnlItem);
                }
            }
        }

        // Sự kiện click nút "Thêm vào giỏ"
        private void BtnThemGio_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string maBanh = btn.Tag.ToString().Trim();

            if (string.IsNullOrEmpty(tenDangNhapHienTai))
            {
                MessageBox.Show("Vui lòng đăng nhập lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsGioHang = db.GioHangs.ToList();

                    // Lọc món đang ở trạng thái "Chờ đặt hàng" để tăng số lượng
                    var itemInCart = dsGioHang.FirstOrDefault(g =>
                        g.TenDangNhap != null && g.TenDangNhap.Trim().Equals(tenDangNhapHienTai, StringComparison.OrdinalIgnoreCase) &&
                        g.MaBanh != null && g.MaBanh.Trim().Equals(maBanh, StringComparison.OrdinalIgnoreCase) &&
                        (g.TrangThai == null || g.TrangThai.Trim().Equals("Chờ đặt hàng", StringComparison.OrdinalIgnoreCase)));

                    if (itemInCart != null)
                    {
                        itemInCart.SoLuong += 1;
                        itemInCart.TrangThai = "Chờ đặt hàng";
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
                    MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng (Trạng thái: Chờ đặt hàng)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm giỏ hàng: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút "Mua ngay"
        private void BtnMuaNgay_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string maBanh = btn.Tag.ToString().Trim();

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
                        MaBanh = maBanh,
                        TenDangNhap = tenDangNhapHienTai,
                        SoLuong = 1,
                        TrangThai = "Chờ xác nhận", // Trạng thái tạm trước khi điền thông tin
                        NgayDat = DateTime.Now
                    };

                    db.GioHangs.Add(dhMoi);
                    db.SaveChanges();
                }

                // Mở thẳng Form Xác Nhận Mua Hàng để người dùng điền Địa chỉ, SĐT
                XAC_NHAN_MUA_HANG frmXacNhan = new XAC_NHAN_MUA_HANG(tenDangNhapHienTai);
                frmXacNhan.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý mua ngay: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Chuyển sang Form Tìm Kiếm
        private void bt_timkiem_Click(object sender, EventArgs e)
        {
            TIM_KIEM frmTimKiem = new TIM_KIEM(tenDangNhapHienTai);
            frmTimKiem.Show();
        }

        // Chuyển form Giỏ Hàng
        private void bt_gh_Click(object sender, EventArgs e)
        {
            GIO_HANG frmGioHang = new GIO_HANG(tenDangNhapHienTai);
            frmGioHang.Show();
        }

        // Chuyển form Lịch Sử Mua Hàng
        private void bt_ls_Click(object sender, EventArgs e)
        {
            LICH_SU_MUA_HANG frmLichSu = new LICH_SU_MUA_HANG(tenDangNhapHienTai);
            frmLichSu.Show();
        }

        // Đăng xuất
        private void bt_dx_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 frmLogin = new Form1();
                frmLogin.Show();
                this.Close();
            }
        }

        // Chuyển form Đơn Hàng
        private void bt_dh_Click(object sender, EventArgs e)
        {
            DON_HANG frmDonHang = new DON_HANG(tenDangNhapHienTai);
            frmDonHang.Show();
            this.Close();
        }
    }
}