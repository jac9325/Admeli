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
    public partial class FormGrupoClienteNuevo : Form
    {
        private GrupoClienteModel grupoClienteModel = new GrupoClienteModel();

        private GrupoCliente grupoCliente { get; set; }
        private bool nuevo { get; set; }
        private int currentIDGrupoCLiente { get; set; }

        public FormGrupoClienteNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormGrupoClienteNuevo(GrupoCliente grupoCliente)
        {
            InitializeComponent();
            this.nuevo = false;
            textNombreGCL.Text = grupoCliente.nombreGrupo;
            textDescripcionGCL.Text = grupoCliente.descripcion;
            textMinimoOrdenGCL.Text = grupoCliente.minimoOrden.ToString();
            chkActivoGCL.Checked = Convert.ToBoolean(grupoCliente.estado);
            currentIDGrupoCLiente = grupoCliente.idGrupoCliente;
        }

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

        private void FormGrupoClienteNuevo_Load(object sender, EventArgs e)
        {
            //
        }

        private bool validarCampos()
        {
            if (textNombreGCL.Text == "")
            {
                errorProvider1.SetError(textNombreGCL, "Rellene este campo");
                textNombreGCL.Focus();
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
                    Response response = await grupoClienteModel.guardar(grupoCliente);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await grupoClienteModel.modificar(grupoCliente);
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
            grupoCliente = new GrupoCliente();
            if (!nuevo) grupoCliente.idGrupoCliente = currentIDGrupoCLiente; // Llenar el id categoria cuando este en esdo modificar

            grupoCliente.nombreGrupo = textNombreGCL.Text;
            grupoCliente.descripcion = textDescripcionGCL.Text;
            grupoCliente.minimoOrden = (textMinimoOrdenGCL.Text == "") ? 0 : Convert.ToInt32(textMinimoOrdenGCL.Text);
            grupoCliente.estado = Convert.ToInt32(chkActivoGCL.Checked);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                crearObjeto();
                guardar();
            }
        }

        private void textMinimoOrdenGCL_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }
    }
}
