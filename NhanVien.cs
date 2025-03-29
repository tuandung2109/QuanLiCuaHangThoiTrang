/*

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BaiTapLon2
{
    public partial class NhanVien : Form
    {
        public NhanVien()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            LoadNhanVien();

            // Thêm các chức vụ vào ComboBox
            comboBox1.Items.Add("Thu ngân");
            comboBox1.Items.Add("Bộ phận kho");
            comboBox1.Items.Add("Bán hàng");
        }

        private void LoadNhanVien()
        {
            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT * FROM NhanVien";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["HoTen"].Value.ToString();
                comboBox1.Text = row.Cells["ChucVu"].Value.ToString();
                textBox3.Text = row.Cells["SoDienThoai"].Value.ToString();
                textBox2.Text = row.Cells["Email"].Value.ToString();
                textBox4.Text = row.Cells["DiaChi"].Value.ToString();
                textBox6.Text = row.Cells["MaSoThue"].Value?.ToString();
            }
        }

        private bool CheckMaSoThueExists(string maSoThue, int? maNV = null)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT COUNT(*) FROM NhanVien WHERE MaSoThue = @MaSoThue AND (@MaNV IS NULL OR MaNV != @MaNV)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSoThue", maSoThue);
                        cmd.Parameters.AddWithValue("@MaNV", (object)maNV ?? DBNull.Value);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kiểm tra mã số thuế: " + ex.Message);
                    return true; // Giả định lỗi là trùng để an toàn
                }
            }
        }

        private bool IsValidMaSoThue(string maSoThue)
        {
            // Kiểm tra xem có chỉ chứa số không
            if (!maSoThue.All(char.IsDigit))
            {
                return false;
            }
            // Kiểm tra độ dài
            if (maSoThue.Length != 10)
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin, bao gồm Mã số thuế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng MaSoThue
            if (!IsValidMaSoThue(textBox6.Text))
            {
                MessageBox.Show("Mã số thuế phải là 10 chữ số và không chứa ký tự nào ngoài số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra MaSoThue có trùng không
            if (CheckMaSoThueExists(textBox6.Text))
            {
                MessageBox.Show("Mã số thuế này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "INSERT INTO NhanVien (HoTen, ChucVu, SoDienThoai, Email, DiaChi, MaSoThue) VALUES (@HoTen, @ChucVu, @SoDienThoai, @Email, @DiaChi, @MaSoThue)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@HoTen", textBox1.Text);
                        cmd.Parameters.AddWithValue("@ChucVu", comboBox1.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", textBox4.Text);
                        cmd.Parameters.AddWithValue("@MaSoThue", textBox6.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin, bao gồm Mã số thuế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng MaSoThue
            if (!IsValidMaSoThue(textBox6.Text))
            {
                MessageBox.Show("Mã số thuế phải là 10 chữ số và không chứa ký tự nào ngoài số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaNV"].Value);
            if (CheckMaSoThueExists(textBox6.Text, maNV))
            {
                MessageBox.Show("Mã số thuế này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "UPDATE NhanVien SET HoTen=@HoTen, ChucVu=@ChucVu, SoDienThoai=@SoDienThoai, Email=@Email, DiaChi=@DiaChi, MaSoThue=@MaSoThue WHERE MaNV=@MaNV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@HoTen", textBox1.Text);
                        cmd.Parameters.AddWithValue("@ChucVu", comboBox1.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", textBox4.Text);
                        cmd.Parameters.AddWithValue("@MaSoThue", textBox6.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaNV"].Value);
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "DELETE FROM NhanVien WHERE MaNV=@MaNV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                LoadNhanVien();
                return;
            }

            if (!int.TryParse(textBox5.Text, out int maNV))
            {
                MessageBox.Show("Mã nhân viên phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@MaNV", maNV);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên với mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

*/

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Linq; // Thêm dòng này để sử dụng LINQ

namespace BaiTapLon2
{
    public partial class NhanVien : Form
    {
        public NhanVien()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            LoadNhanVien();

            // Thêm các chức vụ vào ComboBox
            comboBox1.Items.Add("Thu ngân");
            comboBox1.Items.Add("Bộ phận kho");
            comboBox1.Items.Add("Bán hàng");
        }

        private void LoadNhanVien()
        {
            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT * FROM NhanVien";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["HoTen"].Value.ToString();
                comboBox1.Text = row.Cells["ChucVu"].Value.ToString();
                textBox3.Text = row.Cells["SoDienThoai"].Value.ToString();
                textBox2.Text = row.Cells["Email"].Value.ToString();
                textBox4.Text = row.Cells["DiaChi"].Value.ToString();
                textBox6.Text = row.Cells["MaSoThue"].Value?.ToString();
            }
        }

        private bool CheckMaSoThueExists(string maSoThue, int? maNV = null)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT COUNT(*) FROM NhanVien WHERE MaSoThue = @MaSoThue AND (@MaNV IS NULL OR MaNV != @MaNV)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSoThue", maSoThue);
                        cmd.Parameters.AddWithValue("@MaNV", (object)maNV ?? DBNull.Value);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kiểm tra mã số thuế: " + ex.Message);
                    return true;
                }
            }
        }

        private bool IsValidMaSoThue(string maSoThue)
        {
            if (!maSoThue.All(char.IsDigit) || maSoThue.Length != 10)
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin, bao gồm Mã số thuế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidMaSoThue(textBox6.Text))
            {
                MessageBox.Show("Mã số thuế phải là 10 chữ số và không chứa ký tự nào ngoài số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CheckMaSoThueExists(textBox6.Text))
            {
                MessageBox.Show("Mã số thuế này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "INSERT INTO NhanVien (HoTen, ChucVu, SoDienThoai, Email, DiaChi, MaSoThue) VALUES (@HoTen, @ChucVu, @SoDienThoai, @Email, @DiaChi, @MaSoThue)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@HoTen", textBox1.Text);
                        cmd.Parameters.AddWithValue("@ChucVu", comboBox1.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", textBox4.Text);
                        cmd.Parameters.AddWithValue("@MaSoThue", textBox6.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin, bao gồm Mã số thuế!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidMaSoThue(textBox6.Text))
            {
                MessageBox.Show("Mã số thuế phải là 10 chữ số và không chứa ký tự nào ngoài số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaNV"].Value);
            if (CheckMaSoThueExists(textBox6.Text, maNV))
            {
                MessageBox.Show("Mã số thuế này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "UPDATE NhanVien SET HoTen=@HoTen, ChucVu=@ChucVu, SoDienThoai=@SoDienThoai, Email=@Email, DiaChi=@DiaChi, MaSoThue=@MaSoThue WHERE MaNV=@MaNV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@HoTen", textBox1.Text);
                        cmd.Parameters.AddWithValue("@ChucVu", comboBox1.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", textBox4.Text);
                        cmd.Parameters.AddWithValue("@MaSoThue", textBox6.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaNV"].Value);
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "DELETE FROM NhanVien WHERE MaNV=@MaNV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                LoadNhanVien();
                return;
            }

            if (!int.TryParse(textBox5.Text, out int maNV))
            {
                MessageBox.Show("Mã nhân viên phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@MaNV", maNV);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên với mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT * FROM NhanVien";

            try
            {
                DataSet3 ds = new DataSet3();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(ds.Tables["NhanVien"]);
                }

                CrystalReport3 report = new CrystalReport3();
                report.SetDataSource(ds.Tables["NhanVien"]);

                InDanhSach form = new InDanhSach();
                form.SetReportSource(report);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}