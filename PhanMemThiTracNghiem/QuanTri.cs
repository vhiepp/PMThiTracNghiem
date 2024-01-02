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
            dgvMonHoc.Size = new Size(0, this.Height - 155);
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
            dgvMonHocCuaLop.Size = new Size(0, this.Height - 210);
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

        private void tabCauHoi_ThemCauHoiCuaMonHoc_Click(object sender, EventArgs e)
        {
            bool check = this.tbCauHoi.Text.Length > 0 &&
                         this.tbDapAnA.Text.Length > 0 &&
                         this.tbDapAnB.Text.Length > 0 &&
                         this.tbDapAnC.Text.Length > 0 &&
                         this.tbDapAnD.Text.Length > 0;

            if (check)
            {
                int id = 1;
                string maMH = GenerateSlug(cbMonHocTabCH.Text);
                string sql = $"SELECT COUNT(*) as TongCH FROM CAU_HOI WHERE MaMH='{maMH}';";
                DataTable dt = ketNoi.SelectDuLieu(sql);
                if (dt != null)
                {
                    id = int.Parse(dt.Rows[0]["TongCH"].ToString()) + 1;
                }
                string dapAnDung = "A";
                if (rbtnDapAnB.Checked)
                {
                    dapAnDung = "B";
                } else if (rbtnDapAnC.Checked)
                {
                    dapAnDung = "C";
                } else if (rbtnDapAnD.Checked)
                {
                    dapAnDung = "D";
                }
                sql = $"INSERT INTO CAU_HOI (MaCH, NoiDungCH, DapAnA, DapAnB, DapAnC, DapAnD, DapAnDung, MaMH) VALUES ('{id}-{maMH}', '{tbCauHoi.Text}', '{tbDapAnA.Text}', '{tbDapAnB.Text}', '{tbDapAnC.Text}', '{tbDapAnD.Text}', '{dapAnDung}', '{maMH}');";

                if (this.ketNoi.ExecuteNonQuery(sql))
                {
                    this.tbCauHoi.Text = "";
                    this.tbDapAnA.Text = "";
                    this.tbDapAnB.Text = "";
                    this.tbDapAnC.Text = "";
                    this.tbDapAnD.Text = "";
                    this.tabCauHoi_HienThiCauHoiCuaMonHoc();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }

        private void tabCauHoi_HienThiCauHoiCuaMonHoc()
        {
            string maMH = GenerateSlug(cbMonHocTabCH.Text);
            string sql = $"SELECT * FROM CAU_HOI WHERE MaMH='{maMH}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                int y = 0;
                pnCauHoi.Controls.Clear();
                pnCauHoi.Size = new Size(0, this.Height - 300);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    // Tạo một GroupBox chứa câu hỏi
                    GroupBox groupBoxQuestion = new GroupBox();
                    groupBoxQuestion.Text = $"Câu {i + 1}";
                    groupBoxQuestion.AutoSize = true;
                    groupBoxQuestion.Location = new Point(0, y);
                    groupBoxQuestion.Size = new Size(this.Width - 50, 240);
                    groupBoxQuestion.BackColor = System.Drawing.Color.Bisque;

                    // Tạo một Label trong GroupBox để hiển thị câu hỏi
                    Label lblQuestion = new Label();
                    lblQuestion.Text = dtRow["NoiDungCH"].ToString();
                    lblQuestion.AutoSize = true;
                    lblQuestion.Location = new Point(10, 20);
                    lblQuestion.MaximumSize = new Size(groupBoxQuestion.Width - 30, 0);
                    groupBoxQuestion.Controls.Add(lblQuestion);

                    // Tạo một Panel để chứa các phương án trả lời
                    Panel panelOptions = new Panel();
                    panelOptions.Location = new Point(20, lblQuestion.Location.Y + lblQuestion.Height + 25);
                    panelOptions.AutoSize = true;

                    // Tạo các RadioButton để hiển thị các phương án trả lời trong Panel
                    RadioButton radioButtonOption1 = new RadioButton();
                    radioButtonOption1.Text = "A. " + dtRow["DapAnA"].ToString();
                    radioButtonOption1.Checked = dtRow["DapAnDung"].ToString() == "A";
                    radioButtonOption1.Enabled = radioButtonOption1.Checked;
                    radioButtonOption1.AutoSize = true;
                    panelOptions.Controls.Add(radioButtonOption1);

                    RadioButton radioButtonOption2 = new RadioButton();
                    radioButtonOption2.Text = "B. " + dtRow["DapAnB"].ToString();
                    radioButtonOption2.Checked = dtRow["DapAnDung"].ToString() == "B";
                    radioButtonOption2.Enabled = radioButtonOption2.Checked;
                    radioButtonOption2.AutoSize = true;
                    radioButtonOption2.Location = new Point(0, radioButtonOption1.Location.Y + radioButtonOption1.Height + 15);
                    panelOptions.Controls.Add(radioButtonOption2);

                    RadioButton radioButtonOption3 = new RadioButton();
                    radioButtonOption3.Text = "C. " + dtRow["DapAnC"].ToString();
                    radioButtonOption3.Checked = dtRow["DapAnDung"].ToString() == "C";
                    radioButtonOption3.AutoSize = true;
                    radioButtonOption3.Enabled = radioButtonOption3.Checked;
                    radioButtonOption3.Location = new Point(0, radioButtonOption2.Location.Y + radioButtonOption2.Height + 15);
                    panelOptions.Controls.Add(radioButtonOption3);

                    RadioButton radioButtonOption4 = new RadioButton();
                    radioButtonOption4.Text = "D. " + dtRow["DapAnD"].ToString();
                    radioButtonOption4.Checked = dtRow["DapAnDung"].ToString() == "D";
                    radioButtonOption4.AutoSize = true;
                    radioButtonOption4.Enabled = radioButtonOption4.Checked;
                    radioButtonOption4.Location = new Point(0, radioButtonOption3.Location.Y + radioButtonOption3.Height + 15);
                    panelOptions.Controls.Add(radioButtonOption4);

                    groupBoxQuestion.Controls.Add(panelOptions);
                    y += groupBoxQuestion.Size.Height + 10;

                    pnCauHoi.Controls.Add(groupBoxQuestion);
                }
            }
        }

        private void tabCauHoi_Click()
        {
            this.Text = "Quản lý câu hỏi | Hệ thống thi trắc nghiệm";
            this.tabCauHoi_HienThiDropDownListMonHoc();
            this.tabCauHoi_HienThiCauHoiCuaMonHoc();
        }


        private void tabBoDe_HienThiDropDownListMonHoc()
        {
            string sql = "SELECT * FROM MON_HOC;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbMHTabBoDe.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["TenMH"].ToString();
                    cbMHTabBoDe.Items.Add(item);
                }
                cbMHTabBoDe.SelectedIndex = 0;
            }
        }

        private void tabBoDe_HienThiDropDownListBoDeCuaMonHoc()
        {
            string maMH = GenerateSlug(cbMHTabBoDe.Text);
            string sql = $"SELECT * FROM BO_DE WHERE MaMH='{maMH}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbBoDe.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["MaBoDe"].ToString();
                    cbBoDe.Items.Add(item);
                }
                cbBoDe.SelectedIndex = dt.Rows.Count - 1;
            }
        }

        private void tabBoDe_HienThiCheckBoxListCauHoiCuaMonHoc()
        {
            string maMH = GenerateSlug(cbMHTabBoDe.Text);
            string maBD = cbBoDe.Text;
            string sql = $"SELECT * FROM CAU_HOI WHERE MaMH='{maMH}' AND MaCH NOT IN (SELECT MaCH FROM CH_CUA_BD WHERE MaBoDe='{maBD}');";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cblTatCaCHTabBD.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["MaCH"].ToString() + "_" +dtRow["NoiDungCH"].ToString();
                    cblTatCaCHTabBD.Items.Add(item);
                }
            }
        }

        private void tabBoDe_HienThiCheckBoxListCauHoiCuaBoDe()
        {
            string maBD = cbBoDe.Text;
            string sql = $"SELECT * FROM CAU_HOI JOIN CH_CUA_BD WHERE CAU_HOI.MaCH = CH_CUA_BD.MaCH AND CH_CUA_BD.MaBoDe='{maBD}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cblCauHoiCuaBoDe.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["MaCH"].ToString() + "_" + dtRow["NoiDungCH"].ToString();
                    cblCauHoiCuaBoDe.Items.Add(item);
                }
                lbTongCHCuaBD.Text = dt.Rows.Count.ToString();
            }
        }

        private void tabBoDe_ThemBoDeMonHoc_Click(object sender, EventArgs e)
        {
            int id = 1;
            string maMH = GenerateSlug(cbMHTabBoDe.Text);
            string sql = $"SELECT COUNT(*) as TongBD FROM BO_DE WHERE MaMH='{maMH}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                id = int.Parse(dt.Rows[0]["TongBD"].ToString()) + 1;
            }
            string tempMaBD = id.ToString();
            if (tempMaBD.Length == 1)
            {
                tempMaBD = "00" + tempMaBD;
            } else if (tempMaBD.Length == 2)
            {
                tempMaBD = "0" + tempMaBD;
            }
            string maBD = tempMaBD + "_" + GenerateSlug(cbMHTabBoDe.Text);
            sql = $"INSERT INTO BO_DE (MaBoDe, MaMH) VALUES ('{maBD}', '{maMH}');";

            if (this.ketNoi.ExecuteNonQuery(sql))
            {
                tabBoDe_HienThiDropDownListBoDeCuaMonHoc();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra!");
            }
        }

        private void tabBoDe_XoaCauHoiKhoiBoDeMonHoc_Click(object sender, EventArgs e)
        {
            string maBD = cbBoDe.Text;
            foreach (var item in cblCauHoiCuaBoDe.CheckedItems)
            {
                string maCH = item.ToString().Split('_')[0];
                string sql = $"DELETE FROM CH_CUA_BD WHERE MaBoDe='{maBD}' AND MaCH='{maCH}';";
                if (this.ketNoi.ExecuteNonQuery(sql))
                {
                    
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra!");
                }
            }
            tabBoDe_HienThiCheckBoxListCauHoiCuaBoDe();
            tabBoDe_HienThiCheckBoxListCauHoiCuaMonHoc();
        }

        private void tabBoDe_ThemCauHoiVaoBoDeMonHoc_Click(object sender, EventArgs e)
        {
            string maBD = cbBoDe.Text;
            foreach (var item in cblTatCaCHTabBD.CheckedItems)
            {
                string maCH = item.ToString().Split('_')[0];
                string sql = $"INSERT INTO CH_CUA_BD (MaBoDe, MaCH) VALUES ('{maBD}', '{maCH}');";
                if (this.ketNoi.ExecuteNonQuery(sql))
                {

                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra!");
                }
            }
            tabBoDe_HienThiCheckBoxListCauHoiCuaBoDe();
            tabBoDe_HienThiCheckBoxListCauHoiCuaMonHoc();

        }

        private void tabBoDe_Click()
        {
            this.Text = "Quản lý bộ đề | Hệ thống thi trắc nghiệm";
            tabBoDe_HienThiDropDownListMonHoc();
            tabBoDe_HienThiDropDownListBoDeCuaMonHoc();
            tabBoDe_HienThiCheckBoxListCauHoiCuaBoDe();
            tabBoDe_HienThiCheckBoxListCauHoiCuaMonHoc();
        }

        private void tabKyThi_ThemKyThi_Click(object sender, EventArgs e)
        {
            bool check = this.tbMaKT.Text.Length > 0 &&
                         this.tbTenKT.Text.Length > 0;
            if (check)
            {
                if (this.ketNoi.SelectDuLieu($"SELECT * FROM KY_THI WHERE MaKT='{GenerateSlug(tbMaKT.Text)}'").Rows.Count == 0)
                {
                    string ngay = dtpNamHocTabKT.Value.Day.ToString();
                    string thang = dtpNamHocTabKT.Value.Month.ToString();
                    string nam = dtpNamHocTabKT.Value.Year.ToString();

                    string sql = $"INSERT INTO KY_THI (MaKT, TenKT, NamHoc) VALUES ('{GenerateSlug(this.tbMaKT.Text)}', '{this.tbTenKT.Text}', '{nam}-{thang}-{ngay}')";

                    if (this.ketNoi.ExecuteNonQuery(sql))
                    {
                        this.tbMaKT.Text = "";
                        this.tbTenKT.Text = "";
                        tabKyThi_HienThiKyThi();
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra!");
                    }
                }
                else
                {
                    MessageBox.Show("Mã kỳ thi đã tồn tại!");
                }

            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
            }
        }

        private void tabKyThi_HienThiKyThi()
        {
            dgvKyThi.Size = new Size(0, this.Height - 210);
            string sql = "SELECT *, YEAR(NamHoc) AS Nam FROM KY_THI";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("MaKT", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TenKT", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("NamHocKT", typeof(string)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowTemp = dtTemp.NewRow();
                    DataRow dtRow = dt.Rows[i];
                    rowTemp["MaKT"] = dtRow["MaKT"].ToString();
                    rowTemp["TenKT"] = dtRow["TenKT"].ToString();
                    rowTemp["NamHocKT"] = dtRow["Nam"].ToString();
                    dtTemp.Rows.Add(rowTemp);
                }
                this.dgvKyThi.DataSource = dtTemp;
            }
        }

        private void tabKyThi_HienThiDropDownListKyThi()
        {
            string sql = "SELECT * FROM KY_THI;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbKyThi.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["MaKT"].ToString() + "_" + dtRow["TenKT"].ToString();
                    cbKyThi.Items.Add(item);
                }
                cbKyThi.SelectedIndex = dt.Rows.Count - 1;
            }
        }

        private void tabKyThi_HienThiDropDownListMonThiTabKyThi()
        {
            string sql = "SELECT * FROM MON_HOC;";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbMonThiTabKT.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    string item = dtRow["TenMH"].ToString();
                    cbMonThiTabKT.Items.Add(item);
                }
                if (dt.Rows.Count > 0)
                {
                    cbMonThiTabKT.SelectedIndex = 0;
                }
            }
        }

        private void tabKyThi_HienThiDropDownListBoDeMonHocCuaKyThi()
        {
            string maMH = GenerateSlug(cbMonThiTabKT.Text);
            string sql = $"SELECT * FROM BO_DE WHERE MaMH='{maMH}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                cbBoDeTabKyThi.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    int count = int.Parse(ketNoi.SelectDuLieu($"SELECT COUNT(*) as SoCauHoi FROM CH_CUA_BD WHERE MaBoDe='{dtRow["MaBoDe"].ToString()}';").Rows[0]["SoCauHoi"].ToString());
                    string item = dtRow["MaBoDe"].ToString() + " " + count + " câu";
                    cbBoDeTabKyThi.Items.Add(item);
                }
                if (dt.Rows.Count > 0)
                {
                    cbBoDeTabKyThi.SelectedIndex = 0;
                }
            }
        }

        private void tabKyThi_ThemMonHocCuaKyThi_Click(object sender, EventArgs e)
        {
            bool check = this.cbBoDeTabKyThi.Text.Length > 0 &&
                         this.nbTGLamBai.Value > 0;
            string maKT = cbKyThi.Text.Split('_')[0];
            string maMH = GenerateSlug(cbMonThiTabKT.Text);
            string maBD = cbBoDeTabKyThi.Text.Split(' ')[0];
            
            if (check)
            {
                if (this.ketNoi.SelectDuLieu($"SELECT * FROM MON_THI WHERE MaKT='{maKT}' AND MaMH='{maMH}'").Rows.Count == 0)
                {
                    int count = int.Parse(ketNoi.SelectDuLieu($"SELECT COUNT(*) as SoCauHoi FROM CH_CUA_BD WHERE MaBoDe='{maBD}';").Rows[0]["SoCauHoi"].ToString());
                    if (count > 0)
                    {
                        string ngay = dtpNgayThi.Value.Day.ToString();
                        string thang = dtpNgayThi.Value.Month.ToString();
                        string nam = dtpNgayThi.Value.Year.ToString();

                        string sql = $"INSERT INTO MON_THI (MaKT, MaMH, MaBoDe, ThoiGian, NgayThi) VALUES ('{maKT}', '{maMH}', '{maBD}', '{nbTGLamBai.Value}','{nam}-{thang}-{ngay}')";

                        if (this.ketNoi.ExecuteNonQuery(sql))
                        {
                            tabMonThiCuaKyThi_HienThiMonThi();
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra!");
                        }
                    } else
                    {
                        MessageBox.Show("Bộ đề này chưa có câu hỏi nào!");
                    }
                }
                else
                {
                    MessageBox.Show("Môn thi đã có trong kỳ thi!");
                }

            }
            else
            {
                if (this.cbBoDeTabKyThi.Text.Length == 0)
                {
                    MessageBox.Show("Môn học chưa có bộ đề!");
                } else if (this.nbTGLamBai.Value <= 0)
                {
                    MessageBox.Show("Thời gian làm bài phải lớn hơn 0!");
                } else
                {
                    MessageBox.Show("Có lỗi xảy ra!");
                }
            }
        }



        private void tabKyThi_Click()
        {
            this.Text = "Quản lý kỳ thi | Hệ thống thi trắc nghiệm";
            tabKyThi_HienThiKyThi();
        }

        private void tabMonThiCuaKyThi_HienThiMonThi()
        {
            dgvMonThiCuaKT.Size = new Size(0, this.Height - 240);
            string maKT = cbKyThi.Text.Split('_')[0];
            string sql = $"SELECT MON_THI.*, MON_HOC.TenMH, DATE_FORMAT(MON_THI.NgayThi, '%d/%m/%Y') AS NT FROM MON_THI JOIN MON_HOC ON MON_HOC.MaMH = MON_THI.MaMH WHERE MON_THI.MaKT='{maKT}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);
            if (dt != null)
            {
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("TenMonThi", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("BoDeThi", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("TGLamBai", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("NgayThi", typeof(string)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowTemp = dtTemp.NewRow();
                    DataRow dtRow = dt.Rows[i];
                    rowTemp["TenMonThi"] = dtRow["TenMH"].ToString();
                    rowTemp["BoDeThi"] = dtRow["MaBoDe"].ToString();
                    rowTemp["TGLamBai"] = dtRow["ThoiGian"].ToString();
                    rowTemp["NgayThi"] = dtRow["NT"].ToString();
                    dtTemp.Rows.Add(rowTemp);
                }
                this.dgvMonThiCuaKT.DataSource = dtTemp;
            }
        }

        private void tabMonThiCuaKyThi_Click()
        {
            this.Text = "Quản lý môn thi | Hệ thống thi trắc nghiệm";
            tabKyThi_HienThiDropDownListKyThi();
            tabKyThi_HienThiDropDownListMonThiTabKyThi();
            tabKyThi_HienThiDropDownListBoDeMonHocCuaKyThi();
            tabMonThiCuaKyThi_HienThiMonThi();
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
                case 4:
                    tabKyThi_Click();
                    break;
                case 5:
                    tabCauHoi_Click();
                    break;
                case 6:
                    tabBoDe_Click();
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

        private void cbMonHocTabCH_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tabCauHoi_HienThiCauHoiCuaMonHoc();
        }

        private void QuanTri_SizeChanged(object sender, EventArgs e)
        {
            int tabDangChon = tabControl1.SelectedIndex;
            switch (tabDangChon)
            {
                case 3:
                    dgvMonHoc.Size = new Size(0, this.Height - 155);
                    dgvMonHocCuaLop.Size = new Size(0, this.Height - 210);
                    break;
                case 4:
                    dgvMonThiCuaKT.Size = new Size(0, this.Height - 240);
                    dgvKyThi.Size = new Size(0, this.Height - 210);
                    break;
                case 5:
                    this.tabCauHoi_HienThiCauHoiCuaMonHoc();
                    break;
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void cbMHTabBoDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabBoDe_HienThiDropDownListBoDeCuaMonHoc();
            tabBoDe_HienThiCheckBoxListCauHoiCuaMonHoc();
            tabBoDe_HienThiCheckBoxListCauHoiCuaBoDe();
        }

        private void cbBoDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabBoDe_HienThiCheckBoxListCauHoiCuaBoDe();
            tabBoDe_HienThiCheckBoxListCauHoiCuaMonHoc();
        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabDangChon = tabControl3.SelectedIndex;
            switch (tabDangChon)
            {
                case 0:
                    tabKyThi_Click();
                    break;
                case 1:
                    tabMonThiCuaKyThi_Click();
                    break;
            }
        }

        private void cbMonThiTabKT_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabKyThi_HienThiDropDownListBoDeMonHocCuaKyThi();
        }

        private void cbKyThi_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabMonThiCuaKyThi_HienThiMonThi();
        }
    }
}
