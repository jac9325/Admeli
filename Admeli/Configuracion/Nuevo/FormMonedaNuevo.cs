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
    public partial class FormMonedaNuevo : Form
    {
        private MonedaModel monedaModel = new MonedaModel();

        private Moneda moneda { get; set; }
        private bool nuevo { get; set; }
        private int currentIDMoneda { get; set; }

        public FormMonedaNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormMonedaNuevo(Moneda moneda)
        {
            InitializeComponent();
            this.nuevo = false;

            this.currentIDMoneda = moneda.idMoneda;
            textNombreMoneda.Text = moneda.moneda;
            textSimboloMoneda.Text = moneda.simbolo;
            chkDefaultImpuesto.Checked = moneda.porDefecto;
            chkActivo.Checked = Convert.ToBoolean(moneda.estado);

            currentIDMoneda = moneda.idMoneda;
        }

        #region ========================== Paint ==========================
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panel2);
        }

        private void panelFooter_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.topLine(panelFooter);
        }
        #endregion

        #region ================================ SAVE AND UPDATE ================================
        private bool validarCampos()
        {
            if (textNombreMoneda.Text == "")
            {
                errorProvider1.SetError(textNombreMoneda, "Rellene este campo");
                textNombreMoneda.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textSimboloMoneda.Text == "")
            {
                errorProvider1.SetError(textSimboloMoneda, "Rellene este campo");
                textSimboloMoneda.Focus();
                return false;
            }
            errorProvider1.Clear();
            return true;
        }

        private async void guardar()
        {
            Bloqueo.bloquear(this, true);
            try
            {
                if (nuevo)
                {
                    Response response = await monedaModel.guardar(moneda);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await monedaModel.modificar(moneda);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Bloqueo.bloquear(this, false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Bloqueo.bloquear(this, false);
            }
        }

        private void crearObjeto()
        {
            moneda = new Moneda();
            if (!nuevo) moneda.idMoneda = currentIDMoneda; // Llenar el id categoria cuando este en esdo modificar

            moneda.moneda = textNombreMoneda.Text;
            moneda.simbolo = textSimboloMoneda.Text;
            moneda.porDefecto = chkDefaultImpuesto.Checked;
            moneda.estado = Convert.ToInt32(chkActivo.Checked);
            moneda.tieneRegistros = "0";
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

        private void textSimboloMoneda_KeyPress(object sender, KeyPressEventArgs e)
        {

            Validator.isDecimal(e, textSimboloMoneda.Text);
        }
    }
}
