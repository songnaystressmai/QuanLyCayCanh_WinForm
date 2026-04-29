using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCayCanh.GUI
{
    public class frmBanHang : frmBase
    {
        public frmBanHang()
        {
            this.lblHeaderTitle.Text = "🛒 HỆ THỐNG BÁN HÀNG";
            SetupPOS();
        }

        private void SetupPOS()
        {
            // Chia màn hình làm 2: Bên trái chọn cây, bên phải là hóa đơn
            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));

            // 1. Danh sách cây cảnh (FlowLayoutPanel để hiện các Card sản phẩm)
            FlowLayoutPanel flpProducts = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
            // Code giả lập thêm 5 card sản phẩm cho đẹp
            for (int i = 1; i <= 6; i++)
            {
                Panel card = new Panel { Size = new Size(180, 220), BackColor = Color.FromArgb(248, 249, 250), Margin = new Padding(10) };
                card.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.Gainsboro), 0, 0, 179, 219);

                Label name = new Label { Text = "Cây Kim Tiền " + i, Dock = DockStyle.Bottom, Height = 40, TextAlign = ContentAlignment.MiddleCenter };
                PictureBox img = new PictureBox { Dock = DockStyle.Fill, BackColor = Color.FromArgb(200, 230, 201), SizeMode = PictureBoxSizeMode.CenterImage };

                card.Controls.Add(img);
                card.Controls.Add(name);
                flpProducts.Controls.Add(card);
            }

            // 2. Khu vực hóa đơn
            Panel pnlBill = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(236, 240, 241), Padding = new Padding(15) };
            Label lblTotal = new Label
            {
                Text = "TỔNG TIỀN: 0 VNĐ",
                Dock = DockStyle.Bottom,
                Height = 60,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                TextAlign = ContentAlignment.MiddleRight
            };
            ListBox lstCart = new ListBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11) };
            Button btnPay = new Button
            {
                Text = "THANH TOÁN (F5)",
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            pnlBill.Controls.Add(lstCart);
            pnlBill.Controls.Add(lblTotal);
            pnlBill.Controls.Add(btnPay);

            tlp.Controls.Add(flpProducts, 0, 0);
            tlp.Controls.Add(pnlBill, 1, 0);
            this.Controls.Add(tlp);
        }
    }
}