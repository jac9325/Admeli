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
    public partial class FormCompraNuevo2 : Form
    {


        List<DetalleC> detalleC;
        public FormCompraNuevo2()
        {
            InitializeComponent();

            List<DetalleCompra> list = new List<DetalleCompra>();
            DetalleCompra detalleCompra = new DetalleCompra();
            detalleCompra.cantidad = 50;
            detalleCompra.cantidadUnitaria = 10;
            detalleCompra.descripcion = "fffffff";
            list.Add(detalleCompra);
            detalleCompra = new DetalleCompra();
            detalleCompra.cantidad = 50;
            detalleCompra.cantidadUnitaria = 10;
            detalleCompra.descripcion = "fffffff";
            list.Add(detalleCompra);
            detalleCompra = new DetalleCompra();
            detalleCompra.cantidad = 50;
            detalleCompra.cantidadUnitaria = 10;
            detalleCompra.descripcion = "fffffff";
            list.Add(detalleCompra);
            detalleCompra = new DetalleCompra();
            detalleCompra.cantidad = 50;
            detalleCompra.cantidadUnitaria = 10;
            detalleCompra.descripcion = "fffffff";
            list.Add(detalleCompra);
            detalleCompra = new DetalleCompra();
            detalleCompra.cantidad = 50;
            detalleCompra.cantidadUnitaria = 10;
            detalleCompra.descripcion = "fffffff";
            list.Add(detalleCompra);

            detalleCompraBindingSource.DataSource = list;
        }
    }
}
