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
    public partial class FormVarianteNuevo : Form
    {
        private int currentIDVariante { get; set; }
        private bool nuevo { get; set; }
        
        private Variante currentVariante { get; set; }
        private VarianteModel varianteModel = new VarianteModel();
        private FormProductoNuevo formProductoNuevo;

        #region ================================= Constructor =================================
        public FormVarianteNuevo()
        {
            InitializeComponent();
        }

        public FormVarianteNuevo(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;

            // Cambiando al estado nuevo
            this.nuevo = true;
        }

        public FormVarianteNuevo(FormProductoNuevo formProductoNuevo, Variante currentVariante)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.currentVariante = currentVariante;
            this.currentIDVariante = currentVariante.idVariante;
            this.mostrarDatosModificar();

            // Cambiando al estado modificar
            this.nuevo = false;
        }
        #endregion

        private void mostrarDatosModificar()
        {
            textNombre.Text = currentVariante.nombreVariante;
            chkEsCombo.Checked = currentVariante.esCombo;
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
                    Response response = await varianteModel.guardar(currentVariante);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await varianteModel.modificar(currentVariante);
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

        private void crearObjetoSucursal()
        {
            currentVariante = new Variante();
            if (!nuevo) currentVariante.idVariante = currentIDVariante; // Llenar el id categoria cuando este en esdo modificar
            currentVariante.nombreVariante = textNombre.Text;
            currentVariante.estado = 1;
            currentVariante.esCombo = chkEsCombo.Checked;
            currentVariante.idProducto = formProductoNuevo.currentIDProducto;
        }

        private bool validarCampos()
        {
            if (textNombre.Text == "")
            {
                errorProvider1.SetError(textNombre, "Este campo esta bacía");
                textNombre.Focus();
                return false;
            }
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
