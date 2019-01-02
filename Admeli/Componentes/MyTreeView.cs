using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Componentes
{
    public partial class MyTreeView : TreeView
    {

        public long DoubleclickTicks = 0;
        public object DoubleclickTicksLock = new object();

        protected override void WndProc(ref Message m)
        {
            // Suppress WM_LBUTTONDBLCLK
            if (m.Msg == 0x203)
            {
                long ticksdifference;
                lock (DoubleclickTicksLock)
                {
                    ticksdifference = DateTime.Now.Ticks - DoubleclickTicks;
                }
                if (ticksdifference < TimeSpan.TicksPerSecond)
                    m.Result = IntPtr.Zero;
                else
                    base.WndProc(ref m);
            }
            else base.WndProc(ref m);
        }
    }
}
