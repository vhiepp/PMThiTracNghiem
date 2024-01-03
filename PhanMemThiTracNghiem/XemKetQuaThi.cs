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
    public partial class XemKetQuaThi : Form
    {
        private HocSinh hocSinh;
        private string monThi;
        private string ngayThi;
        private string kyThi;
        private string maDe;
        private string tgThi;
        public int tongSoPhut {  get; set; }
        public int soGiay {  get; set; }
        public double tongDiem {  get; set; }
        public XemKetQuaThi(HocSinh hocSinh, string monThi, string ngayThi, string kyThi, string maDe, string tgThi)
        {
            InitializeComponent();
            this.hocSinh = hocSinh;
            this.monThi = monThi;
            this.ngayThi = ngayThi;
            this.kyThi = kyThi;
            this.maDe = maDe;
            this.tgThi = tgThi;

            tongSoPhut = 0;
            soGiay = 0;
        }

        private void XemKetQuaThi_Load(object sender, EventArgs e)
        {
            lbKyThi.Text = kyThi;
            lbMonThi.Text = monThi;
            lbNgayThi.Text = ngayThi;
            lbMaDe.Text = maDe;
            lbTGThi.Text = tgThi;

            lbTenHS.Text = hocSinh.hoTen;
            lbMaHS.Text = hocSinh.maHS;
            lbLop.Text = hocSinh.lopHoc;

            lbTongDiem.Text = $"{tongDiem:N2}";

            lbSoPhut.Text = (int.Parse(tgThi) - 1 - tongSoPhut).ToString();
            lbSoGiay.Text = (60 - soGiay).ToString();
        }

        private void XemKetQuaThi_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void XemKetQuaThi_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắn chắc muốn thoát khỏi ứng dụng?", "Thoát ứng dụng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
