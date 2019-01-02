using Entidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Ventas
{
    public partial class FormVentaTouch : Form
    {
        public FormVentaTouch()
        {
            InitializeComponent();

            List<Producto> list = new List<Producto>()
            {
                new Producto
                {
                    nombre = "Hola"
                },
                new Producto
                {
                    nombre = "Otro"
                }
            };

            productoBindingSource.DataSource = list;

            DataGridViewButtonColumn btnDeleteProduct = new DataGridViewButtonColumn();
            btnDeleteProduct.HeaderText = "Action";
            btnDeleteProduct.Name = "bntDeleteProductCard";
            btnDeleteProduct.Text = "ELIMINAR";
            btnDeleteProduct.FlatStyle = FlatStyle.Flat;
            btnDeleteProduct.UseColumnTextForButtonValue = true;
            dgvVentaTouch.Columns.Add(btnDeleteProduct);
        }
    }
}
