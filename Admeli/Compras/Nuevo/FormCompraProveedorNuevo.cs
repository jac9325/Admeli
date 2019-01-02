using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidad;

namespace Admeli.Compras.Nuevo
{
    public partial class FormCompraProveedorNuevo : Form
    {
        private OrdenCompra currentOrdenCompra;

        public FormCompraProveedorNuevo()
        {
            InitializeComponent();
        }

        public FormCompraProveedorNuevo(OrdenCompra currentOrdenCompra)
        {
            this.currentOrdenCompra = currentOrdenCompra;
        }
    }
}
