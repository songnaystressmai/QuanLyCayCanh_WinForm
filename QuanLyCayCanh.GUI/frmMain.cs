using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCayCanh.GUI
{
    public class frmMain : Form
    {
        Panel pnlSidebar;

        public frmMain()
        {
            this.Size = new Size(1300, 850);
            this.IsMdiContainer = true; // Cho phép chứa các form con bên trong
            this.Text = "HỆ THỐNG QUẢN LÝ CỬA HÀNG CÂY CẢNH";

            pnlSidebar = new Panel { Dock = DockStyle.Left, Width = 230, BackColor = Color.FromArgb(28, 40, 51) };

            string[] menus = { "Bán Hàng", "Cây Cảnh", "Đơn Hàng", "Khách Hàng", "Nhân Viên", "Khuyến Mãi", "Báo Cáo" };
            int y = 50;
            foreach (string menu in menus)
            {
                Button btn = new Button
                {
                    Text = menu,
                    Location = new Point(0, y),
                    Size = new Size(230, 50),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Gainsboro,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(20, 0, 0, 0),
                    Font = new Font("Segoe UI", 11)
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += MenuClick;
                pnlSidebar.Controls.Add(btn);
                y += 50;
            }
            this.Controls.Add(pnlSidebar);
        }

        private void MenuClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Form f = null;
            switch (btn.Text)
            {
                case "Bán Hàng": f = new frmBanHang(); break;
                case "Cây Cảnh": f = new frmCayCanh(); break;
                case "Đơn Hàng": f = new frmDonHang(); break;
                case "Khách Hàng": f = new frmKhachHang(); break;
                case "Nhân Viên": f = new frmNhanVien(); break;
                case "Khuyến Mãi": f = new frmKhuyenMai(); break;
                case "Báo Cáo": f = new frmBaoCao(); break;
                case "Danh Mục": f = new frmDanhMuc(); break;
                    // ... Thêm các case khác tương tự cho đủ 15 form
            }
            if (f != null)
            {
                f.ShowDialog();
            }
        }
    }
}