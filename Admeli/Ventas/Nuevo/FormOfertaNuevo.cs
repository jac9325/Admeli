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
    public partial class FormOfertaNuevo : Form
    {

        private DatosDescuentosOfertas currentOferta;
        private OfertaG datosOfertaG;
        private int currentIDOferta { get; set; }
        private bool nuevo { get; set; }
        
        private SucursalModel sucursalModel = new SucursalModel();
        private GrupoClienteModel grupoClienteModel = new GrupoClienteModel();
        private ProductoModel productoModel = new ProductoModel();
        private OfertaModel ofertaModel = new OfertaModel();

        #region ================================ Constructor ================================
        public FormOfertaNuevo()
        {
            InitializeComponent();
            nuevo = true;
        }

       

        public FormOfertaNuevo(DatosDescuentosOfertas currentOferta)
        {
            InitializeComponent();           
            this.currentOferta = currentOferta;
            nuevo = false;
        }
        #endregion

        #region ================================== Root load ==================================
        private void FormOfertaNuevo_Load(object sender, EventArgs e)
        {

            if (nuevo)
            {
                this.reLoad();

            }
            else
            {
                this.reLoad();
                cargarDatosOferta();

            }
            
        }

        internal void reLoad()
        {
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now;        
            cargarGrupoCliente();
            cargarSucursales();
        } 
        #endregion

        #region ================================ Loads ================================



        private void cargarDatosOferta()
        {

            textCodigoOferta.Text = currentOferta.codigo;
            textCodigoOferta.Enabled = false;
            dtpFechaInicio.Value = currentOferta.fechaInicio.date;
            dtpFechaFin.Value = currentOferta.fechaFin.date;

            textDescuento.Text = currentOferta.descuento;
            chkEstado.Checked = currentOferta.estado == 1 ? true : false;


                       
        }
        private async void cargarSucursales()
        {
            try
            {
                sucursalBindingSource.DataSource = await sucursalModel.sucursales();
                if (!nuevo)
                {
                    cbxSucursal.SelectedValue = currentOferta.idSucursal;
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

                    cbxGrupoCliente.SelectedValue = currentOferta.idGrupoCliente;
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
                    Response response = await ofertaModel.guardarTodo(datosOfertaG);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await ofertaModel.modificarTodo(datosOfertaG);
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
            datosOfertaG = new OfertaG();
            string dateFin = String.Format("{0:u}", dtpFechaFin.Value);
            dateFin = dateFin.Substring(0, dateFin.Length - 1);

            string dateInicio = String.Format("{0:u}", dtpFechaInicio.Value);
            dateInicio = dateInicio.Substring(0, dateInicio.Length - 1);

            datosOfertaG.codigo =textCodigoOferta.Text.Trim();
            datosOfertaG.fechaInicio = dateInicio;
            datosOfertaG.fechaFin = dateFin;

            datosOfertaG.descuento =textDescuento.Text.Trim();
            datosOfertaG.estado = chkEstado.Checked ? 1 : 0;
            datosOfertaG.idGrupoCliente = (int)cbxGrupoCliente.SelectedValue;
            datosOfertaG.idSucursal = (int)cbxSucursal.SelectedValue;
            datosOfertaG.tipo = "General";      
            datosOfertaG.idAfectoProducto = 0;    
            // currentOferta.fechaFin = dtpFechaFin.Value;
        }

        private bool validarCampos()
        {
            if (textCodigoOferta.Text == "")
            {
                errorProvider1.SetError(textCodigoOferta, "Este campo esta bacía");
                textCodigoOferta.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textDescuento.Text == "")
            {
                errorProvider1.SetError(textDescuento, "Este campo esta bacía");
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

        private void textDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, textDescuento.Text);
        }
    }
}
