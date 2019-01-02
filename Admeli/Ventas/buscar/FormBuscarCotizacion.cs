using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.AlmacenBox.fecha;
using Admeli.AlmacenBox.Nuevo;
using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
using Modelo;


namespace Admeli.Ventas.buscar
{
    public partial class FormBuscarCotizacion : Form
    {


        CotizacionModel cotizacionModel = new CotizacionModel(); 
        private List<CotizacionBuscar> listCotizacion { get; set; }
        public CotizacionBuscar currentCotizacion { get; set; }           
        public FormBuscarCotizacion( )
        {
            InitializeComponent();
                     
        }    
        #region ================================ Root Load ================================

        private void FormNotaSalidaNew_Load(object sender, EventArgs e)
        {
            reLoad();

        }
        private void reLoad()
        {
            cargarListaCotizacione();

          
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
        #endregion

        #region ============================== Load ==============================      
        private async void cargarListaCotizacione()
        {
            loadState(true);
            try
            {

                listCotizacion= await cotizacionModel.listaCotizaciones(ConfigModel.sucursal.idSucursal);
                cotizacionBuscarBindingSource.DataSource = listCotizacion;

            }
             catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Cotizaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }



        }




        #endregion

        #region============eventos=========================================
        private void dgvNotaSalida_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            seleccionarCotizacion();
        }

        private void seleccionarCotizacion()
        {
            // Verificando la existencia de datos en el datagridview
            if (dgvNotaSalida.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dgvNotaSalida.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idCotizacion = Convert.ToInt32(dgvNotaSalida.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

            currentCotizacion = listCotizacion.Find(x => x.idCotizacion == idCotizacion); // Buscando la categoria en las lista de categorias
            this.Close();
         
        }

       

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            seleccionarCotizacion();
        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            loadState(true);
            BindingList<CotizacionBuscar> filtered = new BindingList<CotizacionBuscar>(listCotizacion.Where(obj => obj.rucDni.Contains(txtCliente.Text.Trim()) || obj.nombreCliente.ToUpper().Contains(txtCliente.Text.ToUpper())).ToList());
            cotizacionBuscarBindingSource.DataSource = filtered;
            dgvNotaSalida.Update();

            loadState(false);
        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {
            loadState(true);
            BindingList<CotizacionBuscar> filtered = new BindingList<CotizacionBuscar>(listCotizacion.Where(obj => obj.direccion.Trim().ToUpper() .Contains(txtDireccion.Text.Trim().ToUpper())).ToList());
            cotizacionBuscarBindingSource.DataSource = filtered;
            dgvNotaSalida.Update();
            loadState(false);

        }

        private void txtMoneda_TextChanged(object sender, EventArgs e)
        {
            loadState(true);
            BindingList<CotizacionBuscar> filtered = new BindingList<CotizacionBuscar>(listCotizacion.Where(obj => obj.moneda.Trim().ToUpper().Contains(txtMoneda.Text.Trim().ToUpper())).ToList());
            cotizacionBuscarBindingSource.DataSource = filtered;
            dgvNotaSalida.Update();
            loadState(false);
        }

        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            loadState(true);
            BindingList<CotizacionBuscar> filtered = new BindingList<CotizacionBuscar>(listCotizacion.Where(obj => obj.detalles.Trim().ToUpper().Contains(txtDetalles.Text.Trim().ToUpper())).ToList());
            cotizacionBuscarBindingSource.DataSource = filtered;
            dgvNotaSalida.Update();
            loadState(false);
        }

        private void txtSubTotal_TextChanged(object sender, EventArgs e)
        {
            loadState(true);
            BindingList<CotizacionBuscar> filtered = new BindingList<CotizacionBuscar>(listCotizacion.Where(obj => obj.tipo.Trim().ToUpper().Contains(txtTipo.Text.Trim().ToUpper())).ToList());
            cotizacionBuscarBindingSource.DataSource = filtered;
            dgvNotaSalida.Update();
            loadState(false);
        }

       



        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void txtSubTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

       
        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #endregion============eventos=========================================
    }
}
