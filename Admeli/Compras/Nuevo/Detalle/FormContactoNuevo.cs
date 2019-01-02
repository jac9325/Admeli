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

namespace Admeli.Compras.Nuevo.Detalle
{
    public partial class FormContactoNuevo : Form
    {
        private ContactoModel contactoModel = new ContactoModel();
        private DocumentoIdentificacionModel documentoIdentificacion = new DocumentoIdentificacionModel();
        private FormProveedorNuevo formProveedorNuevo;

        private Contacto currentContacto { get; set; }
        private List<DocumentoIdentificacion> docIdentificaciones { get; set; }
        private int currentIDContacto { get; set; }
        private int currentIDProveedor { get; set; }
        private bool nuevo { get; set; }

        #region ===================================== Constructor =====================================
        public FormContactoNuevo()
        {
            InitializeComponent();
        }

        public FormContactoNuevo(FormProveedorNuevo formProveedorNuevo, Contacto currentContacto)
        {
            InitializeComponent();
            this.formProveedorNuevo = formProveedorNuevo;
            this.currentContacto = currentContacto;
            this.currentIDProveedor = currentContacto.idProveedor;
            this.nuevo = false;
        }

        public FormContactoNuevo(FormProveedorNuevo formProveedorNuevo)
        {
            InitializeComponent();
            this.formProveedorNuevo = formProveedorNuevo;
            this.nuevo = true;
        } 
        #endregion

        private void FormContactoNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            this.cargarDocumentosIdentificacion();
        }

        private void mostrarDatosModificar()
        {
            if (nuevo) return;
            textNombres.Text = currentContacto.nombres;
            textApellidos.Text = currentContacto.apellidos;
            textNDocumento.Text = currentContacto.numeroDocumento;
            textTelefono.Text = currentContacto.telefono;
            textCelular.Text = currentContacto.celular;
            textEmail.Text = currentContacto.email;
            textDireccion.Text = currentContacto.direccion;
            chkEstado.Checked = Convert.ToBoolean(currentContacto.estado);
            cbxTipoDocumento.SelectedValue = currentContacto.idDocumento;
        }

        private async void cargarDocumentosIdentificacion()
        {
            docIdentificaciones = await documentoIdentificacion.docIdentificacionNatural();
            documentoIdentificacionBindingSource.DataSource = docIdentificaciones;
            this.mostrarDatosModificar();
        }

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarData();
        }

        private async void guardarData()
        {
            if (!validarCampos()) return;
            try
            {
                crearObjetoSucursal();
                if (this.nuevo)
                {
                    Response response = await contactoModel.guardar(currentContacto);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await contactoModel.modificar(currentContacto);
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
            currentContacto = new Contacto();

            if (!this.nuevo) currentContacto.idProveedor = currentIDContacto; // Llenar el id categoria cuando este en esdo modificar

            currentContacto.nombres = textNombres.Text;
            currentContacto.apellidos = textApellidos.Text;
            currentContacto.numeroDocumento = textNDocumento.Text;
            currentContacto.telefono = textTelefono.Text;
            currentContacto.celular = textCelular.Text;
            currentContacto.email = textEmail.Text;
            currentContacto.direccion = textDireccion.Text;
            currentContacto.estado = Convert.ToInt32(chkEstado.Checked);
            currentContacto.idDocumento = Convert.ToInt32(cbxTipoDocumento.SelectedValue);
            currentContacto.idProveedor = formProveedorNuevo.currentIDProveedor;
        }

        private bool validarCampos()
        {
            if (textNombres.Text == "")
            {
                errorProvider1.SetError(textNombres, "Este campo esta bacía");
                textNombres.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textApellidos.Text == "")
            {
                errorProvider1.SetError(textApellidos, "Este campo esta bacía");
                textApellidos.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (cbxTipoDocumento.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxTipoDocumento, "Elija almenos uno");
                cbxTipoDocumento.Focus();
                return false;
            }
            errorProvider1.Clear();

            // Validando la longitud del documento
            DocumentoIdentificacion doctIdenti = docIdentificaciones.Find(x => x.idDocumento == Convert.ToInt32(cbxTipoDocumento.SelectedValue));
            if (doctIdenti.numeroDigitos != textNDocumento.Text.Length && textNDocumento.Text != "")
            {
                errorProvider1.SetError(textNDocumento, "La longitud del numero del documento debe ser" + doctIdenti.numeroDigitos.ToString());
                textNDocumento.Focus();
                return false;
            }

            // Validacion de correo electronico
            if (!Validator.IsValidEmail(textEmail.Text) && textEmail.Text != "")
            {
                errorProvider1.SetError(textEmail, "Dirección de correo electrónico inválido");
                textEmail.Focus();
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

        private void textTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void textNDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void textCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void textNombres_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isString(e);
        }

        private void textApellidos_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isString(e);
        }

        private void textEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.IsValidEmail(textEmail.Text);
        }
    }
}
