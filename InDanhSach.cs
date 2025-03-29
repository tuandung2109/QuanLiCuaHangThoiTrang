/*

using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace BaiTapLon2
{
    public partial class InDanhSach : Form
    {
        public InDanhSach()
        {
            InitializeComponent();
        }

        public void SetReportSource(ReportDocument report)
        {
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }
    }
}

*/

using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Linq;

namespace BaiTapLon2
{
    public partial class InDanhSach : Form
    {
        public InDanhSach()
        {
            InitializeComponent();
        }

        public void SetReportSource(ReportDocument report)
        {
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";
            string query = "SELECT * FROM NhanVien WHERE MaSoThue BETWEEN @MaSoThueStart AND @MaSoThueEnd";

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập cả Mã số thuế bắt đầu và kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidMaSoThue(textBox1.Text) || !IsValidMaSoThue(textBox2.Text))
            {
                MessageBox.Show("Mã số thuế phải là 10 chữ số và chỉ chứa số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataSet3 ds = new DataSet3();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSoThueStart", textBox1.Text);
                        cmd.Parameters.AddWithValue("@MaSoThueEnd", textBox2.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds.Tables["NhanVien"]);
                    }
                }

                if (ds.Tables["NhanVien"].Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào trong khoảng Mã số thuế này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    crystalReportViewer1.ReportSource = null;
                    return;
                }

                CrystalReport3 report = new CrystalReport3();
                report.SetDataSource(ds.Tables["NhanVien"]);
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidMaSoThue(string maSoThue)
        {
            return maSoThue.Length == 10 && maSoThue.All(char.IsDigit);
        }
    }
}