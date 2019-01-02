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
    public partial class FormMarcaNuevo : Form
    {
        private Marca marca { get; set; }
        private MarcaModel marcaModel = new MarcaModel();
        private int currentIdMarca { get; set; }
        private bool nuevo { get; set; }

        #region =================================== CONSTRUCTOR ===================================
        public FormMarcaNuevo()
        {
            InitializeComponent();
            chkActivoMarca.Checked = true;
            nuevo = true;
        }

        public FormMarcaNuevo(Marca marca)
        {
            InitializeComponent();
            this.marca = marca;
            this.currentIdMarca = marca.idMarca;
            this.nuevo = false;
            this.btnAceptar.Text = "Guardar cambios";
            this.cargarRegistrosModificar();
        }
        #endregion

        #region ============================== ROOT LOAD ==============================
        private void FormMarcaNuevo_Shown(object sender, EventArgs e)
        {
            textNombreMarca.Focus();
        } 
        #endregion

        #region =================================== LOAD ===================================
        private void cargarRegistrosModificar()
        {
            textNombreMarca.Text = marca.nombreMarca;
            textWebMarca.Text = marca.sitioWeb;
            textDescripcionMarca.Text = marca.descripcion;
            chkActivoMarca.Checked = Convert.ToBoolean(marca.estado);
        } 
        #endregion

        #region =================================== SAVE AND UPDATE ===================================
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                cargarObjeto();
                guardar();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool validarCampos()
        {
            if (textNombreMarca.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreMarca, "Rellene este campo");
                Validator.textboxValidateColor(textNombreMarca, 0);
                textNombreMarca.Focus();
                return false;
            }
            Validator.textboxValidateColor(textNombreMarca, 1);
            errorProvider1.Clear();
            return true;
        }

        private async void guardar()
        {
            try
            {
                appLoadding(true, 25);
                if (nuevo)
                {
                    Response response = await marcaModel.guardar(marca);
                    appLoadding(false, 100);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await marcaModel.modificar(marca);
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
            marca = new Marca();
            if (!nuevo) marca.idMarca = currentIdMarca; // Llenar el id categoria cuando este en esdo modificar

            marca.nombreMarca = textNombreMarca.Text;
            marca.sitioWeb = textWebMarca.Text;
            marca.descripcion = textDescripcionMarca.Text;
            marca.estado = Convert.ToInt32(chkActivoMarca.Checked);
            marca.captionImagen = "";
            marca.ubicacionLogo = "";
            marca.tieneRegistros = "";
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
        private void textWebMarca_Validating(object sender, CancelEventArgs e)
        {
            Validator.textboxValidateColor(textWebMarca, 1);
        }

        private void textNombreMarca_Validated(object sender, EventArgs e)
        {
            if (textNombreMarca.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreMarca, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreMarca, 0);
                return;
            }
            Validator.textboxValidateColor(textNombreMarca, 1);
        }
        #endregion
    }
}
