using System;
using System.Windows.Forms;
using System.Data.SqlClient; // Thư viện để kết nối SQL Server

namespace BaiTapLon2
{
    public partial class DangNhap : Form
    {
        // Chuỗi kết nối tới cơ sở dữ liệu SQL Server
        private string connectionString = @"Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";

        public DangNhap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenDangNhap = textBox1.Text.Trim(); // Lấy tên đăng nhập từ textBox1
            string matKhau = textBox2.Text.Trim();     // Lấy mật khẩu từ textBox2

            // Kiểm tra xem người dùng đã nhập đủ thông tin chưa
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kết nối tới cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Câu lệnh SQL để kiểm tra thông tin đăng nhập
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Thêm tham số để tránh SQL Injection
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                        // Thực thi câu lệnh và lấy kết quả
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0) // Nếu tìm thấy bản ghi khớp
                        {
                            MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở form TrangChu và đóng form DangNhap
                            TrangChu trangChu = new TrangChu();
                            trangChu.Show();
                            this.Hide(); // Đóng form DangNhap
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi nhấn nút "Đăng ký" (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            // Mở form DangKy và đóng form DangNhap
            DangKy dangKy = new DangKy();
            dangKy.Show();
            this.Hide(); // Đóng form DangNhap để chuyển sang DangKy
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Để trống vì không cần xử lý sự kiện này
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Để trống vì không cần xử lý sự kiện này
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Để trống vì không cần xử lý sự kiện này
        }
    }
}