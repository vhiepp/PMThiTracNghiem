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
    public partial class LamBaiThi : Form
    {
        private HocSinh hocSinh;
        private string maKT;
        private string maMH;
        private string maBoDe;
        private KetNoiDuLieu ketNoi;
        private Timer countdownTimer;
        private int countDownValue;
        private int countDownSecondValue;
        private DataTable cauHoi;
        private double tongDiem;
        private XemKetQuaThi frmXemKetQuaThi;
        public LamBaiThi(HocSinh hocSinh, string maKT, string maMH)
        {
            InitializeComponent();
            this.hocSinh = hocSinh;
            this.maKT = maKT;
            this.maMH = maMH;
            this.ketNoi = new KetNoiDuLieu();

            countdownTimer = new Timer();
            countdownTimer.Interval = 1000;
            countDownSecondValue = 60;
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (countDownSecondValue > 0)
            {
                countDownSecondValue--;
            } else
            {
                countDownSecondValue = 59;
                countDownValue--;
            }
            lbTGConLaiGiay.Text = countDownSecondValue.ToString();
            if (countDownValue >= 0)
            {
               lbThoiGianDemNguoc.Text = countDownValue.ToString();
            }
            else
            {
                lbThoiGianDemNguoc.Text = countDownValue.ToString();
                countdownTimer.Stop();
                NopBai();
            }
        }

        private void HienThiCauHoi()
        {
            string sql = $"SELECT CAU_HOI.* FROM CH_CUA_BD JOIN CAU_HOI ON CH_CUA_BD.MaCH = CAU_HOI.MaCH WHERE CH_CUA_BD.MaBoDe = '{maBoDe}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);

            if (dt != null)
            {
                int y = 10;
                pnCauHoi.Controls.Clear();
                pnCauHoi.Size = new Size(0, this.Height - 300);
                DataRow[] rows = dt.Select();
                Random random = new Random();
                DataRow[] shuffledRows = rows.OrderBy(x => random.Next()).ToArray();
                DataTable shuffledTable = dt.Clone();
                foreach (DataRow row in shuffledRows)
                {
                    shuffledTable.ImportRow(row);
                }
                dt = shuffledTable;
                cauHoi = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dtRow = dt.Rows[i];
                    // Tạo một GroupBox chứa câu hỏi
                    GroupBox groupBoxQuestion = new GroupBox();
                    groupBoxQuestion.Text = $"Câu {i + 1}";
                    groupBoxQuestion.AutoSize = true;
                    groupBoxQuestion.Location = new Point(10, y);
                    groupBoxQuestion.Size = new Size(this.Width - 20, 240);
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
                    radioButtonOption1.Name = dtRow["MaCH"].ToString() + "@A" + "@" + dtRow["DapAnDung"].ToString();
                    radioButtonOption1.AutoSize = true;
                    panelOptions.Controls.Add(radioButtonOption1);

                    RadioButton radioButtonOption2 = new RadioButton();
                    radioButtonOption2.Text = "B. " + dtRow["DapAnB"].ToString();
                    radioButtonOption2.Name = dtRow["MaCH"].ToString() + "@B" + "@" + dtRow["DapAnDung"].ToString();
                    radioButtonOption2.AutoSize = true;
                    radioButtonOption2.Location = new Point(0, radioButtonOption1.Location.Y + radioButtonOption1.Height + 15);
                    panelOptions.Controls.Add(radioButtonOption2);

                    RadioButton radioButtonOption3 = new RadioButton();
                    radioButtonOption3.Text = "C. " + dtRow["DapAnC"].ToString();
                    radioButtonOption3.Name = dtRow["MaCH"].ToString() + "@C" + "@" + dtRow["DapAnDung"].ToString();
                    radioButtonOption3.AutoSize = true;
                    radioButtonOption3.Location = new Point(0, radioButtonOption2.Location.Y + radioButtonOption2.Height + 15);
                    panelOptions.Controls.Add(radioButtonOption3);

                    RadioButton radioButtonOption4 = new RadioButton();
                    radioButtonOption4.Text = "D. " + dtRow["DapAnD"].ToString();
                    radioButtonOption4.Name = dtRow["MaCH"].ToString() + "@D" + "@" + dtRow["DapAnDung"].ToString();
                    radioButtonOption4.AutoSize = true;
                    radioButtonOption4.Location = new Point(0, radioButtonOption3.Location.Y + radioButtonOption3.Height + 15);
                    panelOptions.Controls.Add(radioButtonOption4);

                    groupBoxQuestion.Controls.Add(panelOptions);
                    y += groupBoxQuestion.Size.Height + 10;

                    pnCauHoi.Controls.Add(groupBoxQuestion);
                }
            }
        }

        private void KetThucLamBai()
        {
            countdownTimer.Stop();
            frmXemKetQuaThi.soGiay = countDownSecondValue;
            frmXemKetQuaThi.tongSoPhut = countDownValue;
            frmXemKetQuaThi.tongDiem = tongDiem;
            frmXemKetQuaThi.Show();
            this.Hide();
        }

        private void NopBai()
        {
            int dem = 0;
            tongDiem = 0;
            int demDapAnDung = 0;
            double diemMotCau = 10.0 / cauHoi.Rows.Count;
            foreach (Control control_pnCauHoi in pnCauHoi.Controls)
            {
                if (control_pnCauHoi is GroupBox groupBox)
                {
                    foreach (Control control_groupBox in groupBox.Controls)
                    {
                        if (control_groupBox is Panel panel)
                        {
                            foreach (Control control_Panel in panel.Controls)
                            {
                                if (control_Panel is RadioButton radioButton && radioButton.Checked)
                                {
                                    dem++;
                                    string dapAnChon = radioButton.Name.Split('@')[1];
                                    string dapAnDung = radioButton.Name.Split('@')[2];
                                    if (dapAnChon == dapAnDung)
                                    {
                                        demDapAnDung++;
                                    }
                                }
                            
                            }
                        }
                    }   
                }
            }
            if (dem < cauHoi.Rows.Count && (countDownValue > 0 || (countDownValue == 0 && countDownSecondValue > 0)))
            {
                MessageBox.Show("Có câu hỏi chưa được chọn!");
            } else
            {
                tongDiem = demDapAnDung * diemMotCau;
                if (countDownValue > 0 || (countDownValue == 0 && countDownSecondValue > 0))
                {
                    // Tạo một Form tùy chỉnh
                    Form customMessageBox = new Form();
                    customMessageBox.Text = "Nộp bài";
                    customMessageBox.StartPosition = FormStartPosition.CenterScreen;

                    // Thiết lập kích thước cho Form
                    customMessageBox.Size = new Size(500, 300);
                    customMessageBox.MaximumSize = new Size(500, 300);
                    customMessageBox.MinimumSize = new Size(500, 300);

                    Label messageLabel = new Label();
                    messageLabel.Text = "Bạn thực sự muốn nộp bài?";
                    messageLabel.Font = new Font("Arial", 16);
                    messageLabel.ForeColor = Color.Red;
                    messageLabel.Location = new Point(20, 20);
                    messageLabel.Size = new Size(500, 150);
                    messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    messageLabel.Dock = DockStyle.Top;

                    Button cancelButton = new Button();
                    cancelButton.Text = "Hủy";
                    cancelButton.DialogResult = DialogResult.Cancel;
                    cancelButton.Location = new Point(customMessageBox.ClientSize.Width - cancelButton.Width - 10, customMessageBox.ClientSize.Height - cancelButton.Height - 10);


                    Button okButton = new Button();
                    okButton.Text = "Nộp bài";
                    okButton.DialogResult = DialogResult.OK;
                    okButton.Location = new Point(cancelButton.Left - okButton.Width - 5, cancelButton.Top);

                    customMessageBox.Controls.Add(messageLabel);
                    customMessageBox.Controls.Add(cancelButton);
                    customMessageBox.Controls.Add(okButton);

                    DialogResult result = customMessageBox.ShowDialog();
                    
                    if (result == DialogResult.OK)
                    {
                        KetThucLamBai();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                    }
                    customMessageBox.Dispose();
                } else
                {
                    MessageBox.Show("Hết giờ làm bài!", "Hết giờ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    KetThucLamBai();
                }
            }
        }

        private void LamBaiThi_Load(object sender, EventArgs e)
        {
            lbHoTenHS.Text = hocSinh.hoTen;
            lbMaHS.Text = hocSinh.maHS;
            lbLop.Text = hocSinh.lopHoc;

            string sql = $"SELECT MON_THI.*, MON_HOC.TenMH, KY_THI.TenKT, DATE_FORMAT(MON_THI.NgayThi, '%d/%m/%Y') AS NT FROM MON_THI JOIN KY_THI ON KY_THI.MaKT = MON_THI.MaKT JOIN MON_HOC ON MON_THI.MaMH = MON_HOC.MaMH WHERE MON_THI.MaKT = '{maKT}' AND MON_THI.MaMH = '{maMH}';";
            DataTable dt = ketNoi.SelectDuLieu(sql);

            if (dt != null)
            {
                DataRow dtRow = dt.Rows[0];

                lbTenKT.Text = dtRow["TenKT"].ToString();
                lbNgayThi.Text = dtRow["NT"].ToString();
                lbTenMH.Text = dtRow["TenMH"].ToString();
                lbMaDe.Text = dtRow["MaBoDe"].ToString();
                maBoDe = dtRow["MaBoDe"].ToString();
                lbThoiGian.Text = dtRow["ThoiGian"].ToString();
                countDownValue = int.Parse(dtRow["ThoiGian"].ToString()) - 1;
                lbThoiGianDemNguoc.Text = countDownValue.ToString();
                HienThiCauHoi();
                countdownTimer.Start();

                frmXemKetQuaThi = new XemKetQuaThi(
                                        hocSinh,
                                        dtRow["TenMH"].ToString(),
                                        dtRow["NT"].ToString(),
                                        dtRow["TenKT"].ToString(),
                                        dtRow["MaBoDe"].ToString(),
                                        dtRow["ThoiGian"].ToString()
                                      );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NopBai();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void LamBaiThi_FormClosing(object sender, FormClosingEventArgs e)
        {
            countdownTimer.Stop();
        }
    }
}
