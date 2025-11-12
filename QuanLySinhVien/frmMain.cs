using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frmMain : Form
    {
        private bool isAdmin;   
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(bool trangthai) 
        {
            InitializeComponent();
            this.isAdmin = trangthai;
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (this.isAdmin == false)
            {
                danhMucToolStripMenuItem.Enabled = false;
                nghiepVuToolStripMenuItem.Enabled = false;

                //heThongToolStripMenuItem.Enabled = false;
            }
        }

        private void quảnLýKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); 

            frmQuanLyKhoa fKhoa = new frmQuanLyKhoa();
            fKhoa.ShowDialog(); 

            this.Show();
        }

        private void quảnLýLopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); 

            frmQuanLyLop fLop = new frmQuanLyLop();
            fLop.ShowDialog(); 

            this.Show();
        }

        private void quảnLýMônHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); 

            frmQuanLyMonHoc fMonHoc = new frmQuanLyMonHoc();
            fMonHoc.ShowDialog();

            this.Show();
        }

        private void quảnLýSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); 

            frmQuanLySinhVien fSinhVien = new frmQuanLySinhVien();
            fSinhVien.ShowDialog(); 

            this.Show();
        }

        private void nhậpĐiểmTíchLũyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmDiemTichLuy fDiem = new frmDiemTichLuy();
            fDiem.ShowDialog();

            this.Show();
        }

        private void nhậpĐiểmRènLuyệnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmDiemRenLuyen fDiemRL = new frmDiemRenLuyen();
            fDiemRL.ShowDialog();

            this.Show();
        }

        private void sinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmTraCuuThongTin fTraCuu = new frmTraCuuThongTin();
            fTraCuu.ShowDialog();
            this.Show();
        }
    }
}
