using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemThiTracNghiem
{
    public partial class MonThiCuaBan : Form
    {
        private HocSinh hocSinh;
        private KetNoiDuLieu ketNoi;
        public MonThiCuaBan(HocSinh hocSinh)
        {
            InitializeComponent();
            this.hocSinh = hocSinh;
            this.ketNoi = new KetNoiDuLieu();
        }

        public string GenerateSlug(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            string slug = input.ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            slug = slug.Replace(" ", "-");

            slug = Regex.Replace(slug, @"-{2,}", "-");

            return slug;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HienThiDanhSachMonThi()
        {
            DateTime currentDate = DateTime.Now;
            string day = currentDate.Day.ToString();
            string month = currentDate.Month.ToString();
            string year = currentDate.Year.ToString();

            string sql = $"SELECT MON_THI.*, MON_HOC.TenMH, LOP.TenLop, KY_THI.TenKT, DATE_FORMAT(MON_THI.NgayThi, '%d/%m/%Y') AS NT FROM MON_THI JOIN LOP_CO_MH ON MON_THI.MaMH = LOP_CO_MH.MaMH JOIN LOP ON LOP.MaLop = LOP_CO_MH.MaLop JOIN MON_HOC ON MON_THI.MaMH = MON_HOC.MaMH JOIN KY_THI ON MON_THI.MaKT = KY_THI.MaKT WHERE MON_THI.NgayThi = '{year}-{month}-{day}' AND LOP.MaLop = '{hocSinh.maLop}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);

            if (dt != null)
            {
                // Tạo và khởi tạo TableLayoutPanel
                TableLayoutPanel tableLayoutPanel1 = new TableLayoutPanel();
                tableLayoutPanel1.Dock = DockStyle.Fill; // Đặt DockStyle để lấp đầy hết kích thước của Form
                tableLayoutPanel1.ColumnCount = 6; // Số cột
                tableLayoutPanel1.RowCount = dt.Rows.Count + 1; // Số hàng
                this.Controls.Add(tableLayoutPanel1);

                Label label00 = new Label();
                label00.Dock = DockStyle.Fill;
                label00.Text = "STT";
                tableLayoutPanel1.Controls.Add(label00, 0, 0);
                Label label10 = new Label();
                label10.Dock = DockStyle.Fill;
                label10.Text = "Tên môn học";
                tableLayoutPanel1.Controls.Add(label10, 1, 0);
                Label labelt = new Label();
                labelt.Dock = DockStyle.Fill;
                labelt.Text = "Kỳ thi";
                tableLayoutPanel1.Controls.Add(labelt, 2, 0);
                Label label20 = new Label();
                label20.Dock = DockStyle.Fill;
                label20.Text = "Ngày thi";
                tableLayoutPanel1.Controls.Add(label20, 3, 0);
                Label label30 = new Label();
                label30.Dock = DockStyle.Fill;
                label30.Text = "Thời gian thi";
                tableLayoutPanel1.Controls.Add(label30, 4, 0);
                Label label40 = new Label();
                label40.Dock = DockStyle.Fill;
                label40.Text = "Làm bài";
                tableLayoutPanel1.Controls.Add(label40, 5, 0);

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5f));

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
                    DataRow dtRow = dt.Rows[row];

                    Label label0 = new Label();
                    label0.Dock = DockStyle.Fill;
                    label0.Text = (row + 1).ToString();
                    label0.Font = new Font("Arial", 12, FontStyle.Regular);
                    tableLayoutPanel1.Controls.Add(label0, 0, row + 1);

                    Label label1 = new Label();
                    label1.Dock = DockStyle.Fill;
                    label1.Text = dtRow["TenMH"].ToString();
                    label1.Font = new Font("Arial", 12, FontStyle.Regular);
                    tableLayoutPanel1.Controls.Add(label1, 1, row + 1);

                    Label label5 = new Label();
                    label5.Dock = DockStyle.Fill;
                    label5.Text = dtRow["TenKT"].ToString();
                    label5.Font = new Font("Arial", 12, FontStyle.Regular);
                    tableLayoutPanel1.Controls.Add(label5, 2, row + 1);

                    Label label2 = new Label();
                    label2.Dock = DockStyle.Fill;
                    label2.Text = dtRow["NT"].ToString();
                    label2.Font = new Font("Arial", 12, FontStyle.Regular);
                    tableLayoutPanel1.Controls.Add(label2, 3, row + 1);

                    Label label3 = new Label();
                    label3.Dock = DockStyle.Fill;
                    label3.Text = dtRow["ThoiGian"].ToString() + " phút";
                    label3.Font = new Font("Arial", 12, FontStyle.Regular);
                    tableLayoutPanel1.Controls.Add(label3, 4, row + 1);

                    Button button = new Button();
                    button.Text = "Vào thi";
                    button.Font = new Font("Arial", 12, FontStyle.Regular);
                    button.Click += btnThi_Click;
                    button.Name = GenerateSlug(dtRow["TenMH"].ToString()) + "@" + dtRow["MaKT"].ToString();
                    tableLayoutPanel1.Controls.Add(button, 5, row + 1);

                }

                pnMonThiCuaBan.Controls.Add(tableLayoutPanel1);
            }
        }

        private void btnThi_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string maMH = button.Name.Split('@')[0];
                string maKT = button.Name.Split('@')[1];
                LamBaiThi frmlamBaiThi = new LamBaiThi(hocSinh, maKT, maMH);
                frmlamBaiThi.Show();
                this.Hide();
            }
        }

        private void MonThiCuaBan_Load(object sender, EventArgs e)
        {
            lbHoTen.Text = hocSinh.hoTen;
            lbLopHoc.Text = hocSinh.lopHoc;
            HienThiDanhSachMonThi();
        }
    }
}
