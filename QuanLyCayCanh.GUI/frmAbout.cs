using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCayCanh.GUI
{
    public class frmAbout : frmBase
    {
        public frmAbout()
        {
            this.lblHeaderTitle.Text = "THÔNG TIN PHẦN MỀM";
            this.Size = new System.Drawing.Size(500, 300); // Nhỏ lại cho cân đối
        }
    }
}
