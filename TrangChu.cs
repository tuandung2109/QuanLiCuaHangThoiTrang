using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyBanQuanAo; // Thêm namespace chứa các form như HoaDon, ChiTietHoaDon

namespace BaiTapLon2
{
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
            this.IsMdiContainer = true; // Thiết lập Form cha
        }

        private void OpenChildForm(Form childForm)
        {
            // Đóng form con hiện tại nếu có
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }

            // Mở form con mới
            childForm.MdiParent = this;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.Show();
        }

        private void TrangChu_Click(object sender, EventArgs e)
        {
            CloseAllChildForms(); // Đóng tất cả form con để quay lại trang chủ
        }

        private void ThoiTrangNam_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ThoiTrangNam());
        }

        private void ThoiTrangNu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ThoiTrangNu());
        }

        private void NhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new NhanVien());
        }

        private void KhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new KhachHang());
        }

        private void HoaDon_Click(object sender, EventArgs e)
        {
            OpenChildForm(new HoaDon());
        }

        // Bỏ comment và sửa lại nếu cần mở ChiTietHoaDon trực tiếp từ menu
        /*
        private void ChiTietHoaDon_Click(object sender, EventArgs e)
        {
            // ChiTietHoaDon cần MaHD, nên không thể mở trực tiếp từ đây
            // Thay vào đó, mở từ form HoaDon
            OpenChildForm(new ChiTietHoaDon(1)); // Ví dụ với MaHD = 1, cần truyền MaHD thực tế
        }
        */

        private void CloseAllChildForms()
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }
        }

        // Sự kiện khi nhấn "Đăng xuất" trong MenuStrip
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị lại form DangNhap
            DangNhap dangNhap = new DangNhap();
            dangNhap.Show();

            // Đóng form TrangChu
            this.Close();
        }
    }
}