using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemThiTracNghiem
{
    public partial class DangNhap : Form
    {
        private KetNoiDuLieu ketNoi;
        public DangNhap()
        {
            InitializeComponent();
            this.ketNoi = new KetNoiDuLieu();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string tenDangNhap = tbTenDangNhap.Text;
            string matKhau = tbMatKhau.Text;
            if (tenDangNhap.Length > 0 &&  matKhau.Length > 0)
            {
                if (radioButton1.Checked)
                {
                    MessageBox.Show("OK");
                } else if (radioButton2.Checked)
                {
                    string sql = $"SELECT * FROM ADMIN WHERE TenDangNhap='{tenDangNhap}' AND MatKhau='{matKhau}';";
                    
                    if (ketNoi.SelectDuLieu(sql).Rows.Count > 0)
                    {
                        QuanTri formQuanTri = new QuanTri();
                        formQuanTri.Show();
                        this.Hide();
                    } else
                    {
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                    }    
                }
            } else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
            }    
        }
    }
}
