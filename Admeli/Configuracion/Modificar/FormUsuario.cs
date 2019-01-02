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
using Modelo;

namespace Admeli.Configuracion.Modificar
{
    public partial class FormUsuario : Form
    {
        private Personal currentPersonal;
        private bool isValid { get; set; }
        private PersonalModel personalModel = new PersonalModel();
        private CurrentSaveData currentSaveData;

        public FormUsuario()
        {
            InitializeComponent();
        }

        public FormUsuario(Personal currentPersonal)
        {
            InitializeComponent();
            this.currentPersonal = currentPersonal;
            currentSaveData = new CurrentSaveData();
            currentSaveData.idPersonal = currentPersonal.idPersonal;
            currentPersonal.usuario = currentPersonal.usuario;
            currentPersonal.password = currentPersonal.password;
        }

        private void FormUsuario_Load(object sender, EventArgs e)
        {
            textUsuario.Text = currentPersonal.usuario;
            if (currentPersonal.usuario.Trim() != "")
            {
                lblPassword1.Text = "Contraseña Actual";
                lblPassword2.Text = "Nueva Contraseña";
            }
            else
            {
                lblPassword1.Text = "Contraseña";
                lblPassword2.Text = "Repetir Contraseña";
            }
        }

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarSucursal();
        }
        private async void guardarSucursal()
        {
            bloquear(true);
            if (!validarCampos()) { bloquear(false); return; }
            try
            {
                crearObjetoSucursal();
                btnAceptar.Enabled = false;
                if (currentPersonal.usuario.Trim() != "")
                {
                    Response response = await personalModel.usuarioCambiar(currentSaveData);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await personalModel.usuarioActualizar(currentSaveData);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                bloquear(false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                bloquear(false);
                btnAceptar.Enabled = true;
            }
        }

        struct CurrentSaveData
        {
            public int idPersonal { get; set; }
            public string password { get; set; }
            public string passwordNew { get; set; }
            public string usuario { get; set; }
        }

        private void crearObjetoSucursal()
        {
            currentSaveData.password = textPassword1.Text;
            currentSaveData.passwordNew = textPassword2.Text;
            currentSaveData.usuario = textUsuario.Text;
        }

        private bool validarCampos()
        {
            if (textUsuario.Text.Trim() == "")
            {
                errorProvider1.SetError(textUsuario, "Campo obligatorio");
                Validator.textboxValidateColor(textUsuario, 0);
                this.isValid = false;
            }

            if (textPassword1.Text.Trim() == "")
            {
                errorProvider1.SetError(textPassword1, "Campo obligatorio");
                Validator.textboxValidateColor(textPassword1, 1);
                this.isValid = false;
            }

            if (textPassword2.Text.Trim() == "")
            {
                errorProvider1.SetError(textPassword2, "Campo obligatorio");
                Validator.textboxValidateColor(textPassword2, 0);
                this.isValid = false;
            }

            // Validacion de la contraseña
            if (currentPersonal.usuario.Trim() == "")
            {
                if (textPassword1.Text != textPassword2.Text )
                {
                    errorProvider1.SetError(textPassword1, "Las contraseñas son diferentes");
                    Validator.textboxValidateColor(textPassword1, 0);
                    this.isValid = false;

                    errorProvider1.SetError(textPassword2, "Las contraseñas son diferentes");
                    Validator.textboxValidateColor(textPassword2, 0);
                    this.isValid = false;
                }
            }

            // Retornando el valor si es valido
            this.isValid = true;
            errorProvider1.Clear();
            return (!this.isValid) ? false : true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        public void bloquear(bool state)
        {
            if (state)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
            this.Enabled = !state;
        }

    }
}
