using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace BaiTapLon2
{
    public partial class FormPrint : Form
    {
        public FormPrint()
        {
            InitializeComponent();
        }

        // Thêm thuộc tính công khai để truy cập crystalReportViewer1
        public CrystalDecisions.Windows.Forms.CrystalReportViewer CrystalReportViewer
        {
            get { return crystalReportViewer1; }
        }
    }
}