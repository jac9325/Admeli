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
using Entidad;
using Entidad.Configuracion;
using Modelo;

namespace Admeli.Configuracion.Nuevo
{
    public partial class FormTipoDocumentoNuevo : Form
    {
        private TipoDocumento currentTipoDocumento;
        private TipoDocumentoModel tipoDocumentoModel = new TipoDocumentoModel();

        #region ============================ Constructor ============================
        public FormTipoDocumentoNuevo()
        {
            InitializeComponent();
        }

        public FormTipoDocumentoNuevo(TipoDocumento currentTipoDocumento)
        {
            InitializeComponent();
            this.currentTipoDocumento = currentTipoDocumento;
        } 
        #endregion

        #region ============================ Root Load ============================
        private void FormTipoDocumentoNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarDatosModificar();
        }
        #endregion

        #region ============================== Load ==============================
        private void cargarDatosModificar()
        {
            textNombre.Text = currentTipoDocumento.nombre;
            textNombreLabel.Text = currentTipoDocumento.nombreLabel;
            textDescription.Text = currentTipoDocumento.descripcion;
            chkActivo.Checked = Convert.ToBoolean(currentTipoDocumento.estado);
        }
        #endregion

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarSucursal();
        }

        private async void guardarSucursal()
        {
            if (!validarCampos()) return;
            try
            {
                btnAceptar.Enabled = false;
                // Procediendo con el guardado
                crearObjetoSucursal();

                // Modificar
                Response response = await tipoDocumentoModel.modificar(currentTipoDocumento);
                MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnAceptar.Enabled = true;
            }
        }

        private void crearObjetoSucursal()
        {
            currentTipoDocumento.nombre = textNombre.Text;
            currentTipoDocumento.nombreLabel = textNombreLabel.Text;
            currentTipoDocumento.estado = Convert.ToInt32(chkActivo.Checked);
        }

        private bool validarCampos()
        {

            if (textNombreLabel.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreLabel, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreLabel, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombreLabel, 1);


            if (textNombre.Text == "")
            {
                errorProvider1.SetError(textNombre, "Este campo esta bacía");
                Validator.textboxValidateColor(textNombre, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombre, 1);

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}
