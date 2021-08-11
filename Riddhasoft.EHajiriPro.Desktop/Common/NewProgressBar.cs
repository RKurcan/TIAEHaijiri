
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Common
{
    public class NewProgressBar : ProgressBar
    {
        public Brush Color { get; set; }
        public NewProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(new SolidBrush(BackColor), 2, 2, rec.Width, rec.Height);
        }
    }
}
