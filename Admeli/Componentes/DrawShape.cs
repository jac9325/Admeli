using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace Admeli.Componentes
{
    public class DrawShape
    {
        public void lineBorder(Panel panel, int red = 221, int green = 225, int blue = 228, int width = 1)
        {
            Graphics line = panel.CreateGraphics();

            Brush brush = new SolidBrush(Color.FromArgb(red, green, 217));
            Pen penColor = new Pen(brush, width);

            line.DrawLine(penColor, 0, 0, panel.Width, 0);         // Top Line
            line.DrawLine(penColor, 0, (panel.Height - width), (panel.Width - width), (panel.Height - width)); // Bottom Line
            line.DrawLine(penColor, 0, 0, 0, panel.Height);        // Left Line
            line.DrawLine(penColor, (panel.Width - width), 0, (panel.Width - width), panel.Height);     // Rigth Line
        }
        public void rightLine(Panel panel, int red = 221, int green = 225, int blue = 228, int width = 1)
        {
            Graphics line = panel.CreateGraphics();

            Brush brush = new SolidBrush(Color.FromArgb(red, green, 217));
            Pen penColor = new Pen(brush, width);

            line.DrawLine(penColor, (panel.Width - width), 0, (panel.Width - width), panel.Height);     // Rigth Line
        }
        public void leftLine(Panel panel, int red = 221, int green = 225, int blue = 228, int width = 1)
        {
            Graphics line = panel.CreateGraphics();

            Brush brush = new SolidBrush(Color.FromArgb(red, green, 217));
            Pen penColor = new Pen(brush, width);

            line.DrawLine(penColor, 0, 0, 0, panel.Height);        // Left Line
        }
        public void topLine(Panel panel, int red = 221, int green = 225, int blue = 228, int width = 1)
        {
            Graphics line = panel.CreateGraphics();

            Brush brush = new SolidBrush(Color.FromArgb(red, green, 217));
            Pen penColor = new Pen(brush, width);

            line.DrawLine(penColor, 0, 0, panel.Width, 0);         // Top Line
        }
        public void bottomLine(Panel panel, int red = 221, int green = 225, int blue = 228, int width = 1)
        {
            Graphics line = panel.CreateGraphics();

            Brush brush = new SolidBrush(Color.FromArgb(red, green, 217));
            Pen penColor = new Pen(brush, width);

            line.DrawLine(penColor, 0, (panel.Height - width), (panel.Width - width), (panel.Height - width)); // Bottom Line
        }
    }
}
