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

namespace Admeli.Configuracion.Nuevo
{
    public partial class FormImpuestoNuevo : Form
    {
        private ImpuestoModel impuestoModel = new ImpuestoModel();

        private Impuesto impuesto { get; set; }
        private bool nuevo { get; set; }
        private int currentIDImpuesto { get; set; }

        public FormImpuestoNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormImpuestoNuevo(Impuesto impuesto)
        {
            InitializeComponent();
            this.currentIDImpuesto = impuesto.idImpuesto;
            this.nuevo = false;

            textNombreImpuesto.Text = impuesto.nombreImpuesto;
            textSiglasImpuesto.Text = impuesto.siglasImpuesto;
            textValorImpuesto.Text =  impuesto.valorImpuesto.ToString();
            chkPorcentualImpuesto.Checked = impuesto.porcentual;
            chkDefaultImpuesto.Checked = impuesto.porDefecto;
            chkActivo.Checked = Convert.ToBoolean(impuesto.estado);
        }

        #region ========================= Paint =========================
        private void panelFooter_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.topLine(panelFooter);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panel2);
        } 
        #endregion

        private bool validarCampos()
        {
            if (textNombreImpuesto.Text == "")
            {
                errorProvider1.SetError(textNombreImpuesto, "Rellene este campo");
                textNombreImpuesto.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textSiglasImpuesto.Text == "")
            {
                errorProvider1.SetError(textSiglasImpuesto, "Rellene este campo");
                textSiglasImpuesto.Focus();
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
                    Response response = await impuestoModel.guardar(impuesto);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await impuestoModel.modificar(impuesto);
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
            impuesto = new Impuesto();
            if (!nuevo) impuesto.idImpuesto = currentIDImpuesto; // Llenar el id categoria cuando este en esdo modificar

            impuesto.nombreImpuesto = textNombreImpuesto.Text;
            impuesto.siglasImpuesto = textSiglasImpuesto.Text;
            impuesto.porcentual = chkPorcentualImpuesto.Checked;
            impuesto.valorImpuesto =double.Parse( textValorImpuesto.Text);
            impuesto.porDefecto = chkDefaultImpuesto.Checked;
            impuesto.estado = Convert.ToInt32(chkActivo.Checked);
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

        private void textValorImpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, textValorImpuesto.Text);
        }
    }
}
