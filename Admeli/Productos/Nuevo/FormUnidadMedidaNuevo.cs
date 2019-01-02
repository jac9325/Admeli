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

namespace Admeli.Productos.Nuevo
{
    public partial class FormUnidadMedidaNuevo : Form
    {
        private UnidadMedidaModel unidadMedidaModel = new UnidadMedidaModel();
        private UnidadMedida unidadMedida { get; set; }
        private UnidadMedida unidad;
        private int currentIdUnidadMedida { get; set; }
        private bool nuevo { get; set; }

        #region ===================================== CONSTRUCTOR =====================================
        public FormUnidadMedidaNuevo()
        {
            InitializeComponent();
            nuevo = true;
            chkActivoUM.Checked = true;
        }

        public FormUnidadMedidaNuevo(UnidadMedida unidad)
        {
            InitializeComponent();
            this.unidad = unidad;
            this.nuevo = false;
            btnAceptar.Text = "Guardar cambios";
            this.cargarRegistrosModificar();
        }
        #endregion

        #region =============================== ROOT LOAD ===============================
        private void FormUnidadMedidaNuevo_Shown(object sender, EventArgs e)
        {
            textNombreUM.Focus();
        } 
        #endregion

        #region ========================================= LOAD =========================================
        private void cargarRegistrosModificar()
        {
            currentIdUnidadMedida = unidad.idUnidadMedida;
            textNombreUM.Text = unidad.nombreUnidad;
            textSimboloUM.Text = unidad.simbolo;
            chkActivoUM.Checked = Convert.ToBoolean(unidad.estado);
        } 
        #endregion

        #region ==================================== SAVE AND UPDATE ====================================
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private bool validarCampos()
        {
            bool isValid = true;// IS Valid ============ TRUE

            if (textNombreUM.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreUM, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreUM, 0);
                isValid = false;
            }
            if (textSimboloUM.Text.Trim() == "")
            {
                errorProvider1.SetError(textSimboloUM, "Campo obligatorio");
                Validator.textboxValidateColor(textSimboloUM, 0);
                isValid = false;
            }

            return (!isValid) ? false : true;
        }

        private async void guardar()
        {
            if (!validarCampos()) return; // Validacion del los campos
            
            try
            {
                cargarObjeto(); // cargando el objeto para guardar
                appLoadding(true, 25);
                if (nuevo)
                {
                    Response response = await unidadMedidaModel.guardar(unidadMedida);
                    appLoadding(false, 100);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await unidadMedidaModel.modificar(unidadMedida);
                    appLoadding(false, 100);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                appLoadding(false, 0);
            }
        }

        private void cargarObjeto()
        {
            unidadMedida = new UnidadMedida();
            if (!nuevo) unidadMedida.idUnidadMedida = currentIdUnidadMedida; // Llenar el id categoria cuando este en esdo modificar

            unidadMedida.nombreUnidad = textNombreUM.Text;
            unidadMedida.simbolo = textSimboloUM.Text;
            unidadMedida.estado = Convert.ToInt32(chkActivoUM.Checked);
            unidadMedida.tieneRegistros = "";
        }
        #endregion

        #region ==================================== LOADING ====================================
        public void appLoadding(bool state, int progress = 100, bool increment = false)
        {
            btnAceptar.Enabled = !state;
            progressBar.Value = (increment) ? progressBar.Value + progress : progress;
        }
        #endregion

        #region ================================ VALIDATE REAL TIME ================================
        private void textNombreUM_Validated(object sender, EventArgs e)
        {
            if (textNombreUM.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreUM, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreUM, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombreUM, 1);
        }

        private void textSimboloUM_Validated(object sender, EventArgs e)
        {
            if (textSimboloUM.Text.Trim() == "")
            {
                errorProvider1.SetError(textSimboloUM, "Campo obligatorio");
                Validator.textboxValidateColor(textSimboloUM, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textSimboloUM, 1);
        } 
        #endregion
    }
}
