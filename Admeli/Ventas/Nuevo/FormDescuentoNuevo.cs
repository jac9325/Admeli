using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Admeli.Configuracion.Nuevo;
using Entidad;
using Modelo;

namespace Admeli.Ventas.Nuevo
{
    public partial class FormDescuentoNuevo : Form
    {




        private DatosDescuentosOfertas currentDescuento;
        private Descuento datosDescuentoG;

        private int currentIDDescuento { get; set; }
        private bool nuevo { get; set; }

        private SucursalModel sucursalModel = new SucursalModel();
        private GrupoClienteModel grupoClienteModel = new GrupoClienteModel();
        private ProductoModel productoModel = new ProductoModel();
        private DescuentoModel descuentoModel = new DescuentoModel();

        #region ====================== Constructor ======================
        public FormDescuentoNuevo()
        {
            InitializeComponent();
            nuevo = true;
        }

        public FormDescuentoNuevo(DatosDescuentosOfertas currentDescuento)
        {
            InitializeComponent();
            this.currentDescuento = currentDescuento;
            nuevo = false;
        }

      
        #endregion

        #region ========================== Root load ==========================
        private void FormDescuentoNuevo_Load(object sender, EventArgs e)
        {
            if (nuevo)
            {
                this.reLoad();
            }
            else
            {
                this.reLoad();
                cargarDatosDescuento();
            }
           
        }

        private void reLoad()
        {
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now;           
            cargarGrupoCliente();
            cargarSucursales();
        }
        #endregion

        #region ================================ Loads ================================

        private void cargarDatosDescuento()
        {

            textCodigo.Text = currentDescuento.codigo;        
            textCodigo.Enabled = false;
            dtpFechaInicio.Value = currentDescuento.fechaInicio.date;
            dtpFechaFin.Value = currentDescuento.fechaFin.date;
            textMaximaVenta.Text = currentDescuento.cantidadMaxima;
            textMinimaVenta.Text = currentDescuento.cantidadMinima;
            textDescuento.Text = currentDescuento.descuento;
            chkEstado.Checked = currentDescuento.estado == 1 ? true : false;
        }

        private async void cargarSucursales()
        {
            try
            {
                sucursalBindingSource.DataSource = await sucursalModel.sucursales();
                if (!nuevo)
                {
                    cbxSucursal.SelectedValue = currentDescuento.idSucursal;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarGrupoCliente()
        {
            try
            {
                grupoClienteBindingSource.DataSource = await grupoClienteModel.gclientes21();

                if (!nuevo)
                {

                    cbxGrupoCliente.SelectedValue = currentDescuento.idGrupoCliente;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
        #endregion

        #region ====================== Form Add Registers ======================
        private void btnAddGrupoCliente_Click(object sender, EventArgs e)
        {
            FormGrupoClienteNuevo formGrupoCliente = new FormGrupoClienteNuevo();
            formGrupoCliente.ShowDialog();
            cargarGrupoCliente();
        }

        private void btnAddSucursal_Click(object sender, EventArgs e)
        {
            FormSucursalNuevo formSucursal = new FormSucursalNuevo();
            formSucursal.ShowDialog();
            cargarSucursales();
        }
        #endregion

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            executeGuardar();
        }

        private async void executeGuardar()
        {
            if (!validarCampos()) return;
            try
            {
                crearObjetoSucursal();
                if (nuevo)
                {
                    

                    Response response = await descuentoModel.guardarTodo(datosDescuentoG);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await descuentoModel.modificarTodo(datosDescuentoG);
                    
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void crearObjetoSucursal()
        {

            datosDescuentoG = new Descuento();
            string dateFin = String.Format("{0:u}", dtpFechaFin.Value);
            dateFin = dateFin.Substring(0, dateFin.Length - 1);

            string dateInicio = String.Format("{0:u}", dtpFechaInicio.Value);
            dateInicio = dateInicio.Substring(0, dateInicio.Length - 1);


            datosDescuentoG.fechaFin = dateFin;
            datosDescuentoG.fechaInicio = dateInicio;
            datosDescuentoG.descuento = Decimal.Parse(textDescuento.Text);
            datosDescuentoG.cantidadMinima = Decimal.Parse(textMinimaVenta.Text.Trim());
            datosDescuentoG.cantidadMaxima = Decimal.Parse(textMaximaVenta.Text.Trim());
            datosDescuentoG.codigo = textCodigo.Text.Trim();
            datosDescuentoG.tipo = "General";         
            datosDescuentoG.idGrupoCliente = (int)cbxGrupoCliente.SelectedValue;
            datosDescuentoG.idAfectoProducto = 0;
            datosDescuentoG.idSucursal = (int)cbxSucursal.SelectedValue;
            datosDescuentoG.estado = chkEstado.Checked ? 1 :0;

        }

        private bool validarCampos()
        {
            if (textCodigo.Text == "")
            {
                errorProvider1.SetError(textCodigo, "Este campo esta vacía");
                textCodigo.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textMaximaVenta.Text == "")
            {
                errorProvider1.SetError(textMaximaVenta, "Este campo esta vacía");
                textMaximaVenta.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textMinimaVenta.Text == "")
            {
                errorProvider1.SetError(textMinimaVenta, "Este campo esta vacía");
                textMinimaVenta.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textDescuento.Text == "")
            {
                errorProvider1.SetError(textDescuento, "Este campo esta vacía");
                textDescuento.Focus();
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

        private void textMinimaVenta_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void textMinimaVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void textMaximaVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void textDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, textDescuento.Text);
        }
    }
}
