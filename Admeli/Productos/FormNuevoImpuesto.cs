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

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class FormNuevoImpuesto : Form
    {
        public ImpuestoModel impuestoModel = new ImpuestoModel();
        public Impuesto currentImpuesto = new Impuesto();
        public FormNuevoImpuesto()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Verificar datos llenados
            if (validarDatos())
            {
                cargarObjetoImpuesto();
                executarGuardar();

            }
        }
        private void cargarObjetoImpuesto()
        {
            currentImpuesto = new Impuesto();
            currentImpuesto.porcentual = chkPorcentual.Checked;
            currentImpuesto.nombreImpuesto = textNombreImpuesto.Text.Trim();
            currentImpuesto.porDefecto = chkPorDefecto.Checked;
            currentImpuesto.estado = Convert.ToInt32(chkActivo.Checked);
            currentImpuesto.siglasImpuesto = textSiglasImpuesto.Text.Trim();
            currentImpuesto.valorImpuesto = textValorImpuesto.Text.Trim();
        }
        private async void executarGuardar()
        {
            try
            {
                Response response = await impuestoModel.guardar(currentImpuesto);
                MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Proceder a guardar Impuesto
        }
        private bool validarDatos()
        {
            if (textNombreImpuesto .Text == "")
            {
                errorProvider1.SetError(textNombreImpuesto, "Este campo esta vacío");
                textNombreImpuesto.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textSiglasImpuesto.Text == "")
            {
                errorProvider1.SetError(textSiglasImpuesto, "Este campo esta vacío");
                textSiglasImpuesto.Focus();
                return false;
            }
            if (textValorImpuesto.Text == "")
            {
                errorProvider1.SetError(textValorImpuesto, "Este campo esta vacío");
                textSiglasImpuesto.Focus();
                return false;
            }
            errorProvider1.Clear();
            return true;
        }

        private void textNombreImpuesto_Validating(object sender, CancelEventArgs e)
        {
            validarNombreSimboloImpuesto();
        }

        private async void validarNombreSimboloImpuesto()
        {
            // Validando si el campo esta vacio
            string nombreImpuesto =  textNombreImpuesto.Text.Trim();
            string simboloImpuesto = textSiglasImpuesto.Text.Trim();
            nombreImpuesto=nombreImpuesto.Replace(" ", "%20");
            if (String.IsNullOrEmpty(nombreImpuesto) || String.IsNullOrEmpty(simboloImpuesto))
            {
                return;
            }
            List<Impuesto> List = await impuestoModel.verificarNombreSiglasImpuesto(nombreImpuesto,simboloImpuesto);
            if (List.Count > 0)
            {
                if (List[0].nombreImpuesto != "0")
                {
                    errorProvider1.SetError(textNombreImpuesto, "Ya existe un impuesto con ese nombre");
                }
                if (List[0].idImpuesto != 0)
                {
                    errorProvider1.SetError(textSiglasImpuesto, "Ya esiste un impuesto con estas siglas");
                }
            }
            /*
            if (list.Count > 0)
            {
                Validator.textboxValidateColor(textCodigoProducto, 0);
                this.isFieldsValid = false;
                return;
            }
            */
        }

        private void textSiglasImpuesto_Validated(object sender, EventArgs e)
        {
            validarNombreSimboloImpuesto();
        }
    }
}
