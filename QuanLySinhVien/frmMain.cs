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
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

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
    }
}
