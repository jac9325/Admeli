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

namespace Admeli.Productos.Nuevo.PDetalle.web
{
    public partial class FormComentarioNuevo : Form
    {
        private int currentIDComentario { get; set; }
        private bool nuevo { get; set; }

        private FormProductoNuevo formProductoNuevo;
        private Comentario currentComentario { get; set; }

        private LocationModel locationModel = new LocationModel();
        private ComentarioModel comentarioModel = new ComentarioModel();

        #region ============================= Constructor =============================
        public FormComentarioNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormComentarioNuevo(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.nuevo = true;
        }

        public FormComentarioNuevo(FormProductoNuevo formProductoNuevo, Comentario currentComentario)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.currentComentario = currentComentario;
            this.nuevo = false;
        } 
        #endregion

        #region =================================== Root Load ===================================
        private void FormComentarioNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarPaises();
        } 
        #endregion

        #region ========================================= Load =========================================
        private async void cargarPaises()
        {
            paisBindingSource.DataSource = await locationModel.paises();
            cbxPais.SelectedIndex = -1;
            cargarDatosModificar();
        }

        private void cargarDatosModificar()
        {
            if (!nuevo)
            {
                currentIDComentario = currentComentario.idComentario;
                textNombreUsuario.Text = currentComentario.nombreUsuario;
                textCorreo.Text = currentComentario.correoElectronico;
                cbxPais.SelectedText = currentComentario.pais;
                textTituloComentario.Text = currentComentario.tituloComentario;
                textComentario.Text = currentComentario.comentario;
                textPuntos.Text = currentComentario.puntos.ToString();
                chkHavilitar.Checked = Convert.ToBoolean(currentComentario.estado);
            }
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
                crearObjetoComentario();
                if (nuevo)
                {
                    Response response = await comentarioModel.guardar(currentComentario);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await comentarioModel.modificar(currentComentario);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void crearObjetoComentario()
        {
            currentComentario = new Comentario();
            if (!nuevo) currentComentario.idComentario = currentIDComentario; // Llenar el id categoria cuando este en esdo modificar

            currentComentario.nombreUsuario = textNombreUsuario.Text;
            currentComentario.correoElectronico = textCorreo.Text;
            currentComentario.pais = cbxPais.Text;
            currentComentario.tituloComentario = textTituloComentario.Text;
            currentComentario.comentario = textComentario.Text;
            currentComentario.idProducto = formProductoNuevo.currentIDProducto;
            currentComentario.puntos = Convert.ToInt32(textPuntos.Text);
            currentComentario.estado = Convert.ToInt32(chkHavilitar.Checked);
        }

        private bool validarCampos()
        {
            if (textNombreUsuario.Text == "")
            {
                errorProvider1.SetError(textNombreUsuario, "Este campo esta bacía");
                textNombreUsuario.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textCorreo.Text == "")
            {
                errorProvider1.SetError(textCorreo, "Este campo esta bacía");
                textCorreo.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (!Validator.IsValidEmail(textCorreo.Text))
            {
                errorProvider1.SetError(textCorreo, "Correo no valido");
                textCorreo.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textTituloComentario.Text == "")
            {
                errorProvider1.SetError(textTituloComentario, "Este campo esta bacía");
                textTituloComentario.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textComentario.Text == "")
            {
                errorProvider1.SetError(textComentario, "Este campo esta bacía");
                textComentario.Focus();
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

        private void textPuntos_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }
    }
}
