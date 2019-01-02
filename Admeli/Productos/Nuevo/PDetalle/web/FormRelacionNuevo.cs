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

namespace Admeli.Productos.Nuevo.PDetalle.web
{
    public partial class FormRelacionNuevo : Form
    {
        private int currentIDProductoRelacion { get; set; }
        private bool nuevo { get; set; }

        private FormProductoNuevo formProductoNuevo;
        private ProductoRelacion currentProductoRelacion { get; set; }

        private ProductoModel productoModel = new ProductoModel();
        private ProductoRelacionModel productoRelacionModel = new ProductoRelacionModel();

        #region ============================== Constructor ==============================
        public FormRelacionNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormRelacionNuevo(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.nuevo = true;
        }

        public FormRelacionNuevo(FormProductoNuevo formProductoNuevo, ProductoRelacion currentProductoRelacion)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.currentProductoRelacion = currentProductoRelacion;
            this.nuevo = false;
        }
        #endregion

        #region ================================= Root Load =================================
        private void FormRelacionNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargaraComponentes();
            productoRelacion();
        }
        #endregion

        #region ================================ Load ================================
        private void cargaraComponentes()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idRelacion", typeof(string));
            table.Columns.Add("relacion", typeof(string));

            table.Rows.Add("complementaria", "Complementaria al Producto");
            table.Rows.Add("vertical", "Relacionada con el Producto");

            cbxTipoRelacion.DataSource = table;
            cbxTipoRelacion.DisplayMember = "relacion";
            cbxTipoRelacion.ValueMember = "idRelacion";
            cbxTipoRelacion.SelectedIndex = 1;
        }

        private async void productoRelacion()
        {
            productoRelacionBindingSource.DataSource = await productoModel.productoRelacionado(formProductoNuevo.currentIDProducto, cbxTipoRelacion.SelectedValue.ToString(), 0);
            cargarDatosModificar();
        }

        private void cargarDatosModificar()
        {
            if (!nuevo)
            {
                currentIDProductoRelacion = currentProductoRelacion.idProductoRelacion;
                cbxTipoRelacion.SelectedValue = currentProductoRelacion.tipoRelacion;
                textPosicion.Text = currentProductoRelacion.ordenPosicion.ToString();
                chkActivo.Checked = Convert.ToBoolean(currentProductoRelacion.estado);
                cbxProductoRelacion.SelectedValue = currentProductoRelacion.idRelacionProducto;
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
                crearObjetoGuardar();
                if (nuevo)
                {
                    Response response = await productoRelacionModel.guardar(currentProductoRelacion);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await productoRelacionModel.modificar(currentProductoRelacion);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void crearObjetoGuardar()
        {
            currentProductoRelacion = new ProductoRelacion();
            if (!nuevo) currentProductoRelacion.idProductoRelacion = currentIDProductoRelacion; // Llenar el id categoria cuando este en esdo modificar
            currentProductoRelacion.tipoRelacion = cbxTipoRelacion.SelectedValue.ToString();
            currentProductoRelacion.ordenPosicion = Convert.ToInt32(textPosicion.Text);
            currentProductoRelacion.idProducto = formProductoNuevo.currentIDProducto;
            currentProductoRelacion.estado = Convert.ToInt32(chkActivo.Checked);
            currentProductoRelacion.idRelacionProducto = Convert.ToInt32(cbxProductoRelacion.SelectedValue);
        }

        private bool validarCampos()
        {
            if (cbxTipoRelacion.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxTipoRelacion, "Sleeccione almenos una");
                cbxTipoRelacion.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (cbxProductoRelacion.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxProductoRelacion, "Sleeccione almenos una");
                cbxProductoRelacion.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textPosicion.Text == "")
            {
                errorProvider1.SetError(textPosicion, "Este campo esta bacía");
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

    }

    public  class RelacionGuardar
    {

    }
}
