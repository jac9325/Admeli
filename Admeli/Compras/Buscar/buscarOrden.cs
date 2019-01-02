using Admeli.AlmacenBox.fecha;
using Admeli.Compras.Nuevo;
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

namespace Admeli.Compras.Buscar
{
    public partial class buscarOrden : Form
    {
        private ProveedorModel proveedorModel = new ProveedorModel();
        private OrdenCompraModel OrdenCompraModel = new OrdenCompraModel();

        public Proveedor currentProveedor { get; set; }
        public List<Proveedor> proveedores { get; set; }
        private DataTable dt = new DataTable();
        private List<OrdenCompraSinComprar> sinComprarOrden { get; set; }

        public OrdenCompraSinComprar currentOrdenCompra { get; set; }




        public buscarOrden()
        {
            InitializeComponent();

        }

        private void BuscarProveedor_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarProceedoresLike();
        }



        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                // HERE IS WHERE THE ERROR IS THROWN FOR NULLABLE TYPES
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }



        private async void cargarProceedoresLike()
        {
            loadState(true);
            try
            {

                sinComprarOrden = await OrdenCompraModel.listarOrdenCompraSinCompra(ConfigModel.sucursal.idSucursal);

                ordenCompraSinComprarBindingSource1.DataSource = sinComprarOrden;
                //  mostrarPaginado();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
        }

        private void loadState(bool state)
        {
            if (state)
            {
                Cursor.Current = Cursors.WaitCursor;
                progressBarApp.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                progressBarApp.Style = ProgressBarStyle.Blocks;
            }
        }

        private void textBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            seleccionarOrden();
        }

        private void dgvOrdenCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            seleccionarOrden();
        }



        public void seleccionarOrden()
        {
                if (dgvOrdenCompra.Rows.Count == 0) return;

                try
                {
                    int index = dgvOrdenCompra.CurrentRow.Index; // Identificando la fila actual del datagridview
                    int idProveedor = Convert.ToInt32(dgvOrdenCompra.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview
                    currentOrdenCompra = sinComprarOrden.Find(x => x.idOrdenCompra == idProveedor); // Buscando la registro especifico en la lista de registros
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error! " + ex.Message, "Error proveedor", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }


        }
        private void ordenCompraSinComprarBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

      

        private void txtProveerdor_TextChanged(object sender, EventArgs e)
        {
            BindingList<OrdenCompraSinComprar> filtered = new BindingList<OrdenCompraSinComprar>(sinComprarOrden.Where(obj => obj.rucDni.Contains(txtProveerdor.Text.Trim()) || obj.nombreProveedor.ToUpper().Contains(txtProveerdor.Text.ToUpper())).ToList());
            ordenCompraSinComprarBindingSource1.DataSource = filtered;
            dgvOrdenCompra.Update();
        }

        private void btnfechasalida_Click(object sender, EventArgs e)
        {
            FormFecha fecha = new FormFecha();
            fecha.ShowDialog();
            BindingList<OrdenCompraSinComprar> filtered = new BindingList<OrdenCompraSinComprar>(sinComprarOrden.Where(obj => obj.DateView >= fecha.desde && obj.DateView <= fecha.hasta).ToList());
            ordenCompraSinComprarBindingSource1.DataSource = filtered;
            dgvOrdenCompra.Update();
        }

        private void txtMoneda_TextChanged(object sender, EventArgs e)
        {
            BindingList<OrdenCompraSinComprar> filtered = new BindingList<OrdenCompraSinComprar>(sinComprarOrden.Where(obj => obj.rucDni.ToUpper().Contains(txtRuc.Text.Trim().ToUpper())).ToList());
            ordenCompraSinComprarBindingSource1.DataSource = filtered;
            dgvOrdenCompra.Update();
        }

        private void txtdireccion_TextChanged(object sender, EventArgs e)
        {
            BindingList<OrdenCompraSinComprar> filtered = new BindingList<OrdenCompraSinComprar>(sinComprarOrden.Where(obj => obj.direccion.ToUpper().Contains(txtdireccion.Text.Trim().ToUpper())).ToList());
            ordenCompraSinComprarBindingSource1.DataSource = filtered;
            dgvOrdenCompra.Update();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            seleccionarOrden();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FormOrdenCompraNew formOrdenCompraNew = new FormOrdenCompraNew();
            formOrdenCompraNew.ShowDialog();
            cargarProceedoresLike();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BindingList<OrdenCompraSinComprar> filtered = new BindingList<OrdenCompraSinComprar>(sinComprarOrden.Where(obj => obj.detalles.ToUpper().Contains(txtDetalles.Text.Trim().ToUpper())).ToList());
            ordenCompraSinComprarBindingSource1.DataSource = filtered;
            dgvOrdenCompra.Update();
        }
    }
}
