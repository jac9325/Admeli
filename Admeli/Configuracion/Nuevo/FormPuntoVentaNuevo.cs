using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
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

namespace Admeli.Configuracion.Nuevo
{
    public partial class FormPuntoVentaNuevo : Form
    {
        private PuntoVentaModel puntoVentaModel = new PuntoVentaModel();
        private SucursalModel sucursalModel = new SucursalModel();

        private PuntoDeVenta puntoVenta { get; set; }
        private bool nuevo { get; set; }
        private int currentIDPuntoVenta { get; set; }

        #region ================================= Constructor =================================
        public FormPuntoVentaNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormPuntoVentaNuevo(PuntoDeVenta puntoDeVenta)
        {
            InitializeComponent();

            this.nuevo = false;
            this.currentIDPuntoVenta = puntoDeVenta.idPuntoVenta;
            this.puntoVenta = puntoDeVenta;
            textPuntoVenta.Text = puntoDeVenta.nombre;
            chkActivo.Checked = Convert.ToBoolean(puntoDeVenta.estado);
        } 
        #endregion

        private void FormPuntoVentaNuevo_Load(object sender, EventArgs e)
        {
            cargarSucursal();
        }

        #region ====================== Loads ======================
        private async void cargarSucursal()
        {
            sucursalBindingSource.DataSource = await sucursalModel.sucursales();
            if(!nuevo)
                cbxSucursalPV.SelectedValue = puntoVenta.idSucursal;
        } 
        #endregion

        #region ===================== Paint =====================
        private void panelFooter_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.topLine(panelFooter);
        }
        private void FormPuntoVentaNuevo_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel12, 157, 157, 157);
        }
        #endregion

        #region ===================== Guardar o guardar cambios =====================
        private bool validarCampos()
        {
            if (textPuntoVenta.Text.Trim() == "")
            {
                errorProvider1.SetError(textPuntoVenta, "Campo obligatorio");
                Validator.textboxValidateColor(textPuntoVenta, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textPuntoVenta, 1);

            if (cbxSucursalPV.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxSucursalPV, "Rellene este campo");
                cbxSucursalPV.Focus();
                return false;
            }
            errorProvider1.Clear();
            return true;
        }

        private async void guardar()
        {
            try
            {
                Bloqueo.bloquear(this, true);
                if (nuevo)
                {
                    Response response = await puntoVentaModel.guardar(puntoVenta);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await puntoVentaModel.modificar(puntoVenta);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Bloqueo.bloquear(this, false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Bloqueo.bloquear(this, false);
            }
        }

        private void crearObjeto()
        {
            puntoVenta = new PuntoDeVenta();
            if (!nuevo) puntoVenta.idPuntoVenta = currentIDPuntoVenta; // Llenar el id categoria cuando este en esdo modificar

            puntoVenta.nombre = textPuntoVenta.Text;
            puntoVenta.idSucursal = Convert.ToInt32(cbxSucursalPV.SelectedValue);
            puntoVenta.estado = Convert.ToInt32(chkActivo.Checked);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                crearObjeto();
                guardar();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ================================ Validacion en tiempo real ================================
        private void textPuntoVenta_Validated(object sender, EventArgs e)
        {
            if (textPuntoVenta.Text.Trim() == "")
            {
                errorProvider1.SetError(textPuntoVenta, "Campo obligatorio");
                Validator.textboxValidateColor(textPuntoVenta, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textPuntoVenta, 1);
        }
        #endregion

        private void FormPuntoVentaNuevo_Shown(object sender, EventArgs e)
        {
            cbxSucursalPV.Focus();
        }
    }
}
