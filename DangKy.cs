using System;
using System.Windows.Forms;
using System.Data.SqlClient; // Thư viện để kết nối SQL Server

namespace BaiTapLon2
{
    public partial class DangKy : Form
    {
        // Chuỗi kết nối tới cơ sở dữ liệu SQL Server
        private string connectionString = @"Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";

        public DangKy()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các textBox
            string tenDangNhap = textBox1.Text.Trim(); // Tên đăng nhập
            string tuoiStr = textBox2.Text.Trim();     // Tuổi (chuỗi)
            string matKhau = textBox3.Text.Trim();     // Mật khẩu
            string nhapLaiMatKhau = textBox4.Text.Trim(); // Nhập lại mật khẩu

            // 1. Kiểm tra các trường có rỗng không
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(tuoiStr) ||
                string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(nhapLaiMatKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kiểm tra tuổi phải là số và lớn hơn 10
            if (!int.TryParse(tuoiStr, out int tuoi) || tuoi <= 10)
            {
                MessageBox.Show("Tuổi phải là số và lớn hơn 10!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Kiểm tra mật khẩu và nhập lại mật khẩu có khớp không
            if (matKhau != nhapLaiMatKhau)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kết nối tới cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra xem TenDangNhap đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Thêm tài khoản mới vào bảng TaiKhoan
                    string insertQuery = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau) VALUES (@TenDangNhap, @MatKhau)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                        cmd.ExecuteNonQuery(); // Thực thi lệnh INSERT
                    }

                    MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Chuyển sang form DangNhap
                    DangNhap dangNhap = new DangNhap();
                    dangNhap.Show();
                    this.Close(); // Đóng form DangKy
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}