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
    public partial class FormAlternativaNuevo : Form
    {
        private int currentIDAlternativa { get; set; }
        private bool nuevo { get; set; }

        private Alternativa currentAlternativa { get; set; }
        private AlternativaModel alternativaModel = new AlternativaModel();

        private FormProductoNuevo formProductoNuevo;
        private int idVariante;

        #region ================================= Constructor =================================
        public FormAlternativaNuevo()
        {
            InitializeComponent();
        }

        public FormAlternativaNuevo(FormProductoNuevo formProductoNuevo, Alternativa currentAlternativa)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.currentAlternativa = currentAlternativa;
            this.currentIDAlternativa = currentAlternativa.idAlternativa;
            mostrarDatosModificar();

            // cambiando estado modificar
            this.nuevo = false;
        }

        public FormAlternativaNuevo(FormProductoNuevo formProductoNuevo, int idVariante)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;

            // cambiando estado nuevo
            this.nuevo = true;
            this.idVariante = idVariante;
        }
        #endregion

        private void mostrarDatosModificar()
        {
            textValorVariante.Text = currentAlternativa.descripcionAlternativa;
            textPosicion.Text = currentAlternativa.ordenPosicion.ToString();
            chkEsSeleccionado.Checked = currentAlternativa.seleccionado;
            this.idVariante = currentAlternativa.idVariante;
        }

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            executeGuardar();
        }

        private async void executeGuardar()
        {
            appLoadState(true);
            if (!validarCampos()) { appLoadState(false); return; }
            try
            {
                crearObjetoSucursal();
                if (nuevo)
                {
                    Response response = await alternativaModel.guardar(currentAlternativa);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await alternativaModel.modificar(currentAlternativa);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            appLoadState(false);
        }

        private void crearObjetoSucursal()
        {
            currentAlternativa = new Alternativa();
            if (!nuevo) currentAlternativa.idAlternativa = currentIDAlternativa; // Llenar el id categoria cuando este en esdo modificar

            currentAlternativa.descripcionAlternativa = textValorVariante.Text;
            currentAlternativa.estado = 1;
            currentAlternativa.ordenPosicion = Convert.ToInt32(textPosicion.Text);
            currentAlternativa.seleccionado = chkEsSeleccionado.Checked;
            currentAlternativa.idVariante = this.idVariante;
        }

        private bool validarCampos()
        {
            if (textValorVariante.Text == "")
            {
                errorProvider1.SetError(textValorVariante, "Este campo esta vacío");
                textValorVariante.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textPosicion.Text == "")
            {
                errorProvider1.SetError(textPosicion, "Este campo esta vacío");
                textPosicion.Focus();
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

        public void appLoadState(bool state)
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
