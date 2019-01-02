using Admeli.Componentes;
using Entidad;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Productos.Importar
{
    public partial class FormImportarProduto : Form
    {
        private List<ObjectSaveProducto> listaProductos = new List<ObjectSaveProducto>();
        private ProductoModel productoModel = new ProductoModel();
        public FormImportarProduto()
        {
            InitializeComponent();
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            appLoadState(true);
            try
            {
                dgvProductos.DataSource = ExternalFiles.ImporExcel();
                btnGuardar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message, "Importar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            appLoadState(false);
        }

        public void appLoadState(bool state)
        {
            if (state)
            {
                progressBarApp.Visible = true;
                progressBarApp.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                progressBarApp.Visible = false;
                progressBarApp.Style = ProgressBarStyle.Blocks;
            }
            this.Enabled = !state;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            extraerProductos();
            guardarProductos();
            // Usar el servicio de guardar varios productos
            // string request = JsonConvert.SerializeObject(productoFila);

        }
        private async void guardarProductos()
        {
            
            appLoadState(true);
            try
            {
                ResponseD response = await productoModel.guardarVariosProductos(listaProductos);
                MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            appLoadState(false);
        }
        private void extraerProductos()
        {
            appLoadState(true);
            btnGuardar.Enabled = false;
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                ObjectSaveProducto productoFila = new ObjectSaveProducto();
                productoFila.codigoProducto = row.Cells[0].Value.ToString();
                productoFila.nombreProducto = row.Cells[1].Value.ToString();
                productoFila.precioCompra = row.Cells[2].Value.ToString();
                productoFila.precioVenta = row.Cells[3].Value.ToString();
                productoFila.marca = row.Cells[4].Value.ToString();
                productoFila.unidad = row.Cells[5].Value.ToString();
                productoFila.stock = row.Cells[6].Value.ToString();
                productoFila.categoria= row.Cells[7].Value.ToString();


                listaProductos.Add(productoFila);
                //MessageBox.Show(row.Cells[0].Value.ToString() + " " + row.Cells[1].Value.ToString() + " " +
                //    row.Cells[2].Value.ToString() + " " + row.Cells[3].Value.ToString()+" "+ row.Cells[5].Value.ToString()+ " "+row.Cells[6].Value.ToString());
            }
            btnGuardar.Enabled = true;
            appLoadState(false);
        }
    }

    public class ObjectSaveProducto
    {
        public string codigoProducto { get; set; }
        public string nombreProducto { get; set; }
        public string precioCompra { get; set; }
        public string precioVenta { get; set; }
        public string marca { get; set; }
        public string unidad { get; set; }
        public string stock { get; set; }
        public string categoria { get; set; }
    }
}
