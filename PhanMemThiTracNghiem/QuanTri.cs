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
    public partial class QuanTri : Form
    {
        private KetNoiDuLieu ketNoi;
        private DataTable lopHoc;
        private DataTable hocSinh;
        public QuanTri()
        {
            InitializeComponent();
            ketNoi = new KetNoiDuLieu();
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabLopHoc_HienThiLopHoc()
        {
            string sql = "SELECT LOP.*, COUNT(HOC_SINH.MaHS) AS SiSo, YEAR(LOP.NienKhoa) AS NamHoc FROM LOP LEFT JOIN HOC_SINH ON LOP.MaLop = HOC_SINH.MaLop GROUP BY LOP.MaLop, LOP.TenLop;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                this.lopHoc = dt;
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("TenLop", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenGVCN", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("NamHoc", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("SiSo", typeof(int)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowTemp = dtTemp.NewRow();
                    DataRow dtRow = dt.Rows[i];
                    rowTemp["TenLop"] = dtRow["TenLop"].ToString();
                    rowTemp["TenGVCN"] = dtRow["TenGVCN"].ToString();
                    rowTemp["NamHoc"] = dtRow["NamHoc"].ToString();
                    rowTemp["SiSo"] = dtRow["SiSo"];
                    dtTemp.Rows.Add(rowTemp);
                }
                this.dgvLopHoc.DataSource = dtTemp;
            }
        }

        private void tabLopHoc_Click()
        {
            this.Text = "Quản lý lớp học | Hệ thống thi trắc nghiệm";
            this.tabLopHoc_HienThiLopHoc();
        }

        private void tabLopHoc_ThemLopHoc_Click(object sender, EventArgs e)
        {
            bool check = this.tbTenLop.Text.Length > 0 && this.tbTenGVCN.Text.Length > 0 && this.tbNienKhoa.Text.Length > 0;
            if (check)
            {
                if (this.ketNoi.SelectDuLieu($"SELECT * FROM LOP WHERE MaLop='{this.GenerateSlug(this.tbTenLop.Text + " " + this.tbNienKhoa.Text)}'").Rows.Count == 0)
                {
                    string sql = $"INSERT INTO LOP (MaLop, TenLop, TenGVCN, NienKhoa) VALUES ('{this.GenerateSlug(this.tbTenLop.Text + " " + this.tbNienKhoa.Text)}', '{this.tbTenLop.Text}', '{this.tbTenGVCN.Text}', '{this.tbNienKhoa.Text}-01-01')";

                    if (this.ketNoi.ExecuteNonQuery(sql))
                    {
                        this.tbTenLop.Text = "";
                        this.tbTenGVCN.Text = "";
                        this.tbNienKhoa.Text = "";
                        this.tabLopHoc_HienThiLopHoc();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra!");
                    }
                } else
                {
                    MessageBox.Show("Lớp đã tồn tại!");
                }
                
            } else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }

        private void tabHocSinh_HienThiDropDownListLopHoc()
        {
            string sql = "SELECT LOP.*, COUNT(HOC_SINH.MaHS) AS SiSo, YEAR(LOP.NienKhoa) AS NamHoc FROM LOP LEFT JOIN HOC_SINH ON LOP.MaLop = HOC_SINH.MaLop GROUP BY LOP.MaLop, LOP.TenLop;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbLopHoc.Items.Clear();
                this.lopHoc = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["TenLop"].ToString() + " " +dtRow["NamHoc"].ToString();
                    cbLopHoc.Items.Add(item);
                }
                cbLopHoc.SelectedIndex = 0;
            }
        }

        private void tabMonHoc_HienThiDropDownListLopHoc()
        {
            string sql = "SELECT LOP.*, COUNT(HOC_SINH.MaHS) AS SiSo, YEAR(LOP.NienKhoa) AS NamHoc FROM LOP LEFT JOIN HOC_SINH ON LOP.MaLop = HOC_SINH.MaLop GROUP BY LOP.MaLop, LOP.TenLop;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbLopTabMH.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["TenLop"].ToString() + " " + dtRow["NamHoc"].ToString();
               
                    cbLopTabMH.Items.Add(item);
                }
                cbLopTabMH.SelectedIndex = 0;
            }
        }

        private void tabMonHoc_HienThiDropDownListMonHoc()
        {
            string sql = "SELECT * FROM MON_HOC;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbMonHoc.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["TenMH"].ToString();
                    cbMonHoc.Items.Add(item);
                }
                cbMonHoc.SelectedIndex = 0;
            }
        }

        private void tabCauHoi_HienThiDropDownListMonHoc()
        {
            string sql = "SELECT * FROM MON_HOC;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbMonHocTabCH.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["TenMH"].ToString();
                    cbMonHocTabCH.Items.Add(item);
                }
                cbMonHocTabCH.SelectedIndex = 0;
            }
        }

        private void tabHocSinh_HienThiHocSinh()
        {
            string maLop = GenerateSlug(cbLopHoc.Text);
            string sql = $"SELECT HOC_SINH.*, LOP.TenLop, YEAR(LOP.NienKhoa) AS NamHoc, DATE_FORMAT(HOC_SINH.NamSinh, '%d/%m/%Y') AS NgaySinh FROM HOC_SINH JOIN LOP ON LOP.MaLop = HOC_SINH.MaLop WHERE HOC_SINH.MaLop='{maLop}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                this.lopHoc = dt;
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("MaHS", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenHS", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("NgaySinh", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("GioiTinh", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenLopHoc", typeof(string)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowTemp = dtTemp.NewRow();
                    DataRow dtRow = dt.Rows[i];
                    rowTemp["MaHS"] = dtRow["MaHS"].ToString();
                    rowTemp["TenHS"] = dtRow["TenHS"].ToString();
                    rowTemp["NgaySinh"] = dtRow["NgaySinh"].ToString();
                    rowTemp["GioiTinh"] = dtRow["GioiTinh"].ToString();
                    rowTemp["TenLopHoc"] = dtRow["TenLop"].ToString() + " " + dtRow["NamHoc"].ToString();
                    dtTemp.Rows.Add(rowTemp);
                }
                this.dgvHocSinh.DataSource = dtTemp;
            }
        }

        private void tabHocSinh_ThemHocSinh_Click(object sender, EventArgs e)
        {
            bool check = this.tbTenHS.Text.Length > 0 &&
                         this.tbMaHS.Text.Length > 0 &&
                         this.ddlGioiTinhHS.Text.Length > 0 &&
                         this.cbLopHoc.Text.Length > 0;
            if (check)
            {
                if (this.ketNoi.SelectDuLieu($"SELECT * FROM HOC_SINH WHERE MaHS='{tbMaHS.Text}'").Rows.Count == 0)
                {
                    string ngaySinh = dtpNgaySinhHS.Value.Day.ToString();
                    string thangSinh = dtpNgaySinhHS.Value.Month.ToString();
                    string namSinh = dtpNgaySinhHS.Value.Year.ToString();
                    if (ngaySinh.Length == 1) ngaySinh = "0" + ngaySinh;
                    if (thangSinh.Length == 1) thangSinh = "0" + thangSinh;
                    string sql = $"INSERT INTO HOC_SINH (MaHS, TenHS, NamSinh, GioiTinh, MatKhau, MaLop) VALUES ('{this.tbMaHS.Text}', '{this.tbTenHS.Text}', '{namSinh}-{thangSinh}-{ngaySinh}', '{this.ddlGioiTinhHS.Text}', '{GenerateSlug(ngaySinh + thangSinh + namSinh)}', '{GenerateSlug(this.cbLopHoc.Text)}')";
            
                    if (this.ketNoi.ExecuteNonQuery(sql))
                    {
                        this.tbTenHS.Text = "";
                        int maHS = int.Parse(this.tbMaHS.Text) + 1;
                        this.tbMaHS.Text = maHS.ToString();
                        this.tabHocSinh_HienThiHocSinh();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra!");
                    }
                }
                else
                {
                    MessageBox.Show("Mã học sinh đã tồn tại!");
                }

            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }

        private void tabHocSinh_Click()
        {
            this.Text = "Quản lý học sinh | Hệ thống thi trắc nghiệm";
            this.tabHocSinh_HienThiDropDownListLopHoc();
            this.tabHocSinh_HienThiHocSinh();
        }

        private void tabMonHoc_HienThiMonHoc()
        {
            string sql = "SELECT * FROM MON_HOC";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                this.lopHoc = dt;
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("MaMH", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenMH", typeof(string)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowTemp = dtTemp.NewRow();
                    DataRow dtRow = dt.Rows[i];
                    rowTemp["MaMH"] = dtRow["MaMH"].ToString();
                    rowTemp["TenMH"] = dtRow["TenMH"].ToString();
                    dtTemp.Rows.Add(rowTemp);
                }
                this.dgvMonHoc.DataSource = dtTemp;
            }
        }

        private void tabMonHoc_HienThiMonHocCuaLop()
        {
            string maLop = GenerateSlug(cbLopTabMH.Text);
            string sql = $"SELECT MON_HOC.*, LOP_CO_MH.TenGV FROM MON_HOC JOIN LOP_CO_MH ON MON_HOC.MaMH = LOP_CO_MH.MaMH JOIN LOP ON LOP_CO_MH.MaLop = LOP.MaLop WHERE LOP.MaLop = '{maLop}';";
            
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                this.lopHoc = dt;
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("MaMH", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenMH", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenGV", typeof(string)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowTemp = dtTemp.NewRow();
                    DataRow dtRow = dt.Rows[i];
                    rowTemp["MaMH"] = dtRow["MaMH"].ToString();
                    rowTemp["TenMH"] = dtRow["TenMH"].ToString();
                    rowTemp["TenGV"] = dtRow["TenGV"].ToString();
                    dtTemp.Rows.Add(rowTemp);
                }
                this.dgvMonHocCuaLop.DataSource = dtTemp;
            }
        }

        private void tabMonHoc_ThemMonHoc_Click(object sender, EventArgs e)
        {
            bool check = this.tbTenMH.Text.Length > 0;
            if (check)
            {
                if (this.ketNoi.SelectDuLieu($"SELECT * FROM MON_HOC WHERE MaMH='{GenerateSlug(tbTenMH.Text)}'").Rows.Count == 0)
                {
                    string sql = $"INSERT INTO MON_HOC (MaMH, TenMH) VALUES ('{GenerateSlug(tbTenMH.Text)}', '{this.tbTenMH.Text}');";

                    if (this.ketNoi.ExecuteNonQuery(sql))
                    {
                        this.tbTenMH.Text = "";
                        this.tabMonHoc_HienThiMonHoc();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra!");
                    }
                }
                else
                {
                    MessageBox.Show("Môn học đã tồn tại!");
                }

            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }

        private void tabMonHoc_ThemMonHocCuaLop_Click(object sender, EventArgs e)
        {
            bool check = this.tbTenGVGD.Text.Length > 0;
            if (check)
            {
                string maLop = GenerateSlug(cbLopTabMH.Text);
                string maMH = GenerateSlug(cbMonHoc.Text);

                if (this.ketNoi.SelectDuLieu($"SELECT * FROM LOP_CO_MH WHERE MaMH='{maMH}' AND MaLop='{maLop}'").Rows.Count == 0)
                {
                    string sql = $"INSERT INTO LOP_CO_MH (MaMH, MaLop, TenGV) VALUES ('{maMH}', '{maLop}', '{tbTenGVGD.Text}');";

                    if (this.ketNoi.ExecuteNonQuery(sql))
                    {
                        this.tbTenGVGD.Text = "";
                        this.tabMonHoc_HienThiMonHocCuaLop();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra!");
                    }
                }
                else
                {
                    MessageBox.Show("Môn học đã có trong lớp!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }

        private void tabMonHoc_Click()
        {
            this.Text = "Quản lý môn học | Hệ thống thi trắc nghiệm";
            this.tabMonHoc_HienThiMonHoc();
        }

        private void tabCauHoi_Click()
        {
            this.Text = "Quản lý câu hỏi | Hệ thống thi trắc nghiệm";
            this.tabCauHoi_HienThiDropDownListMonHoc();

            // Tạo một GroupBox chứa câu hỏi
            GroupBox groupBoxQuestion = new GroupBox();
            groupBoxQuestion.Text = "Câu hỏi 1";
            groupBoxQuestion.AutoSize = true;

            // Tạo một Label trong GroupBox để hiển thị câu hỏi
            Label lblQuestion = new Label();
            lblQuestion.Text = "Đây là câu hỏi trắc nghiệm? Đây là câu hỏi trắc nghiệm? Đây là câu hỏi trắc nghiệm?Đây là câu hỏi trắc nghiệm?Đây là câu hỏi trắc nghiệm? Đây là câu hỏi trắc nghiệm?Đây là câu hỏi trắc nghiệm? Đây là câu hỏi trắc nghiệm? Đây là câu hỏi trắc nghiệm? Đây là câu hỏi trắc nghiệm?Đây là câu hỏi trắc nghiệm?Đây là câu hỏi trắc nghiệm?";
            lblQuestion.AutoSize = true;
            lblQuestion.Location = new Point(10, 20);
            groupBoxQuestion.Controls.Add(lblQuestion);

            // Tạo một Panel để chứa các phương án trả lời
            Panel panelOptions = new Panel();
            panelOptions.Location = new Point(20, lblQuestion.Location.Y + lblQuestion.Height + 25);
            panelOptions.AutoSize = true;

            // Tạo các RadioButton để hiển thị các phương án trả lời trong Panel
            RadioButton radioButtonOption1 = new RadioButton();
            radioButtonOption1.Text = "Phương án A";
            radioButtonOption1.AutoSize = true;
            panelOptions.Controls.Add(radioButtonOption1);

            RadioButton radioButtonOption2 = new RadioButton();
            radioButtonOption2.Text = "Phương án B";
            radioButtonOption2.AutoSize = true;
            radioButtonOption2.Location = new Point(0, radioButtonOption1.Location.Y + radioButtonOption1.Height + 15);
            panelOptions.Controls.Add(radioButtonOption2);

            RadioButton radioButtonOption3 = new RadioButton();
            radioButtonOption3.Text = "Phương án C";
            radioButtonOption3.AutoSize = true;
            radioButtonOption3.Location = new Point(0, radioButtonOption2.Location.Y + radioButtonOption2.Height + 15);
            panelOptions.Controls.Add(radioButtonOption3);

            RadioButton radioButtonOption4 = new RadioButton();
            radioButtonOption4.Text = "Phương án D";
            radioButtonOption4.AutoSize = true;
            radioButtonOption4.Location = new Point(0, radioButtonOption3.Location.Y + radioButtonOption3.Height + 15);
            panelOptions.Controls.Add(radioButtonOption4);

            groupBoxQuestion.Controls.Add(panelOptions);

            pnCauHoi.Controls.Add(groupBoxQuestion);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabDangChon = tabControl1.SelectedIndex;
            switch (tabDangChon)
            {
                case 1:
                    tabLopHoc_Click();
                    break;
                case 2:
                    tabHocSinh_Click();
                    break;
                case 3:
                    tabMonHoc_Click();
                    break;
                case 5:
                    tabCauHoi_Click();
                    break;
            }
        }

        private void QuanTri_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void tabMonHocCuaLop_Click()
        {
            this.Text = "Quản lý môn học của lớp | Hệ thống thi trắc nghiệm";
            this.tabMonHoc_HienThiDropDownListLopHoc();
            this.tabMonHoc_HienThiMonHocCuaLop();
            this.tabMonHoc_HienThiDropDownListMonHoc();
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabDangChon = tabControl2.SelectedIndex;
            switch (tabDangChon)
            {
                case 0:
                    tabMonHoc_Click();
                    break;
                case 1:
                    tabMonHocCuaLop_Click();
                    break;
            }
        }

        private void cbLopTabMH_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabMonHoc_HienThiMonHocCuaLop();
        }

        private void cbLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabHocSinh_HienThiHocSinh();
        }
    }
}
