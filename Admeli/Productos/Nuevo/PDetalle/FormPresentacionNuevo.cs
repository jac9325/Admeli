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
using Modelo;

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class FormPresentacionNuevo : Form
    {
        private int currentIDPresentacion { get; set; }
        private bool nuevo { get; set; }

        private Presentacion currentPresentacion { get; set; }
        private PresentacionModel presentacionModel = new PresentacionModel();
        private FormProductoNuevo formProductoNuevo;

        #region ============================== Constructor ==============================
        public FormPresentacionNuevo()
        {
            InitializeComponent();
        }

        public FormPresentacionNuevo(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.nuevo = true;
            //Traer las presentaciones relacionadas
            cargarPresentacion(formProductoNuevo.currentIDProducto);
        }

        public FormPresentacionNuevo(FormProductoNuevo formProductoNuevo, Presentacion currentPresentacion)
        {
            InitializeComponent();

            // Parametros
            this.formProductoNuevo = formProductoNuevo;
            this.currentPresentacion = currentPresentacion;
            this.currentIDPresentacion = currentPresentacion.idPresentacion;
            // Mostrando los datos modificar
            mostrarDatosModificar();
            // Cambiando el estado a modificar
            this.nuevo = false;
            //CargarPresentaciones Base
            cargarPresentacion(formProductoNuevo.currentIDProducto);

        } 
        #endregion


        private void mostrarDatosModificar()
        {
            textNombre.Text = currentPresentacion.nombrePresentacion;
            textSimbolo.Text = currentPresentacion.simboloPresentacion;
            textCantidad.Text = currentPresentacion.cantidadUnitaria.ToString();
            chkEstado.Checked = Convert.ToBoolean(currentPresentacion.estado);
        }

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            executeGuardar();
        }
        private async void executeGuardar()
        {
            bloquear(true);
            if (!validarCampos()) { bloquear(false); return; }
            try
            {
                crearObjetoSucursal();
                if (nuevo)
                {
                    Response response = await presentacionModel.guardar(currentPresentacion);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await presentacionModel.modificar(currentPresentacion);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bloquear(false);
        }

        internal async void cargarPresentacion(int idProducto)
        {
            try
            {
                presentacionBindingSource.DataSource = await presentacionModel.presentaciones(idProducto);
                //if (!formProductoNuevo.nuevo) cbxUnidadMedida.SelectedValue = formProductoNuevo.currentProducto.idUnidadMedida;
                if (!nuevo) cbxPresentacionBase.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Nueva Presentación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void crearObjetoSucursal()
        {
            currentPresentacion = new Presentacion();

            if (!nuevo) currentPresentacion.idPresentacion = currentIDPresentacion; // Llenar el id categoria cuando este en esdo modificar

            currentPresentacion.nombrePresentacion = textNombre.Text;
            currentPresentacion.simboloPresentacion = textSimbolo.Text;
            currentPresentacion.cantidadUnitaria = Decimal.Parse(textCantidad.Text);
            currentPresentacion.estado = Convert.ToInt32(chkEstado.Checked);
            currentPresentacion.idProducto = formProductoNuevo.currentIDProducto;
            currentPresentacion.idPresentacionBase = Convert.ToInt32(cbxPresentacionBase.SelectedValue);
            currentPresentacion.codigo = textCodigo.Text;
            currentPresentacion.codigoBarras = "";
            currentPresentacion.descripcion = "";
            currentPresentacion.precioCompra = textPrecio.Text;
        }

        private bool validarCampos()
        {
            if (textNombre.Text == "")
            {
                errorProvider1.SetError(textNombre, "Este campo esta bacía");
                textNombre.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textSimbolo.Text == "")
            {
                errorProvider1.SetError(textSimbolo, "Este campo esta bacía");
                textSimbolo.Focus();
                return false;
            }
            errorProvider1.Clear();

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        public void bloquear(bool state)
        {
            if (state) { Cursor.Current = Cursors.WaitCursor; }
            else { Cursor.Current = Cursors.Default; }
            this.Enabled = !state;
        }

    }
}
