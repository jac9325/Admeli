using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Reportes
{
    public partial class UCReporteIngresoVentas : UserControl
    {
        private FormPrincipal formPrincipal;
        public UCReporteIngresoVentas()
        {
            InitializeComponent();
        }
        public UCReporteIngresoVentas(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
        }
        public void reLoad()
        {

        }
    }
}
