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
    public partial class FormDenominacionNuevo : Form
    {
        private Denominacion currentDenominacion;
        private DenominacionModel denominacionModel = new DenominacionModel();
        private MonedaModel monedaModel = new MonedaModel();

        internal int currentIDDenominacion { get; set; }
        internal bool nuevo { get; set; }

        #region ================================= Constrcutor =================================
        public FormDenominacionNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormDenominacionNuevo(Denominacion currentDenominacion)
        {
            InitializeComponent();
            this.currentDenominacion = currentDenominacion;
            this.nuevo = false;
        } 
        #endregion

        #region =============================== Tipo Moneda ===============================
        private void FormDenominacionNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarMonedas();
            cargarTipoMoneda();
        }
        #endregion

        #region ================================== Loads ==================================
        private void cargarDatosModificar()
        {
            currentIDDenominacion = currentDenominacion.idDenominacion;
            textNombre.Text = currentDenominacion.nombre;
            textValor.Text = currentDenominacion.valor;
            cbxMoneda.SelectedValue = currentDenominacion.idMoneda;
            cbxTipoMoneda.Text = currentDenominacion.tipoMoneda;
            chkEstado.Checked = Convert.ToBoolean(currentDenominacion.estado);
        }

        private async void cargarMonedas()
        {
            try
            {
                monedaBindingSource.DataSource = await monedaModel.monedas();
                if (!nuevo) cargarDatosModificar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } 

        private void cargarTipoMoneda()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idTipoMoneda", typeof(string));
            table.Columns.Add("tipoMoneda", typeof(string));

            table.Rows.Add("1", "BILLETE");
            table.Rows.Add("2", "MONEDA");

            cbxTipoMoneda.DataSource = table;
            cbxTipoMoneda.DisplayMember = "tipoMoneda";
            cbxTipoMoneda.ValueMember = "idTipoMoneda";
            cbxTipoMoneda.SelectedIndex = 0;
        }

        #endregion

        #region ========================== Paint Decoration ==========================
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel4, 157, 157, 157);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel2, 157, 157, 157);
        }
        #endregion

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarSucursal();
        }

        private async void guardarSucursal()
        {
            Bloqueo.bloquear(this, true);
            if (!validarCampos()) { Bloqueo.bloquear(this, false); return; }
            try
            {
                crearObjetoDenominacion();
                if (nuevo)
                {
                    Response response = await denominacionModel.guardar(currentDenominacion);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await denominacionModel.modificar(currentDenominacion);
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

        private void crearObjetoDenominacion()
        {
            if (nuevo)
            {
                currentDenominacion = new Denominacion();
                currentDenominacion.anular = "0";
                currentDenominacion.imagen = "";
            }
            else
            {
                currentDenominacion.idDenominacion = currentIDDenominacion; // Llenar el id categoria cuando este en esdo modificar
            }
            currentDenominacion.idMoneda = Convert.ToInt32(cbxMoneda.SelectedValue);
            currentDenominacion.moneda = cbxMoneda.Text;
            currentDenominacion.tipoMoneda = cbxTipoMoneda.Text;
            currentDenominacion.nombre = textNombre.Text;
            currentDenominacion.valor = textValor.Text;
            currentDenominacion.estado = Convert.ToInt32(chkEstado.Checked);
        }

        private bool validarCampos()
        {
            if (cbxMoneda.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxMoneda, "Este campo esta bacía");
                cbxMoneda.Focus();
                return false;
            }
            errorProvider1.Clear();

            // validacion monto
            if (textNombre.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombre, "Campo obligatorio");
                Validator.textboxValidateColor(textNombre, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombre, 1);

            // validacion motivo
            if (textValor.Text.Trim() == "")
            {
                errorProvider1.SetError(textValor, "Campo obligatorio");
                Validator.textboxValidateColor(textValor, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textValor, 1);

            // Toda las validaciones correctas
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ====================================== Validaciones Tiempo Real ======================================
        private void textValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private async void textNombre_Validated(object sender, EventArgs e)
        {
            try
            {
                // Verificando campo vacio
                if (!validarCampos()) return;
                int idDenominacion = (nuevo) ? 0 : currentIDDenominacion;
                List<Denominacion> list = await denominacionModel.verificarDenominaciones(textNombre.Text, Convert.ToDouble(textValor.Text), Convert.ToInt32(cbxMoneda.SelectedValue), idDenominacion);
                if (list.Count > 0)
                {
                    errorProvider1.SetError(textNombre, "Ya existe");
                    Validator.textboxValidateColor(textNombre, 0);
                    return;
                }
                errorProvider1.Clear();
                Validator.textboxValidateColor(textNombre, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void textValor_Validated(object sender, EventArgs e)
        {
            try
            {
                // Verificando campo vacio
                if (!validarCampos()) return;
                int idDenominacion = (nuevo) ? 0 : currentIDDenominacion;
                List<Denominacion> list = await denominacionModel.verificarDenominaciones(textNombre.Text, Convert.ToDouble(textValor.Text), Convert.ToInt32(cbxMoneda.SelectedValue), idDenominacion);
                if (list.Count > 0)
                {
                    errorProvider1.SetError(textValor, "Ya existe");
                    Validator.textboxValidateColor(textValor, 0);
                    return;
                }
                errorProvider1.Clear();
                Validator.textboxValidateColor(textValor, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        private void FormDenominacionNuevo_Shown(object sender, EventArgs e)
        {
            cbxMoneda.Focus();
        }
    }
}
