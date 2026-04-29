using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace QuanLyCayCanh.GUI
{
    public partial class frmBase : Form
    {
        protected Panel pnlHeader;
        protected Label lblHeaderTitle;
        protected Button btnClose;

        public frmBase()
        {
            // Thiết lập cơ bản cho Form
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(240, 243, 247);
            this.Size = new Size(1000, 650);
            this.Font = new Font("Segoe UI", 10);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tạo Header bar màu tối sang trọng
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(33, 47, 61)
            };

            // Tiêu đề Form
            lblHeaderTitle = new Label
            {
                ForeColor = Color.White,
                Location = new Point(15, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "FORM TITLE"
            };

            // Nút đóng (X)
            btnClose = new Button
            {
                Text = "X",
                Dock = DockStyle.Right,
                Width = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.Red;
            btnClose.Click += (s, e) => this.Close();

            // Thêm các control vào Header
            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(btnClose);

            // Thêm Header vào Form
            this.Controls.Add(pnlHeader);

            // Cho phép di chuyển Form khi kéo Header
            pnlHeader.MouseDown += Header_MouseDown;
        }

        // Code hỗ trợ kéo di chuyển Form không viền
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}