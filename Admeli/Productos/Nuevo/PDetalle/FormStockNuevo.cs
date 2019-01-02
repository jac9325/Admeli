using Admeli.Configuracion.Nuevo;
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
    public partial class FormStockNuevo : Form
    {
        private AlmacenModel almacenModel = new AlmacenModel();
        private StockModel stockModel = new StockModel();
        private Stock currentStock;
        private FormProductoNuevo formProductoNuevo;

        private int currentIdStock { get; set; }
        private bool nuevo { get; set; }

        public FormStockNuevo(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.nuevo = true;
        }

        public FormStockNuevo(Stock currentStock, FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.currentStock = currentStock;
            this.currentIdStock = currentStock.idProductoStockAlmacen;
            this.formProductoNuevo = formProductoNuevo;
            this.nuevo = false;
        }

        #region ============================= Root load =============================
        private void FormStockNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarAlmacenes();
        } 
        #endregion

        private async void cargarAlmacenes() {
            almacenBindingSource.DataSource = await almacenModel.almacenes();

            // cargando los datos en modificar
            cargarDatosModificar();
        }

        private void cargarDatosModificar()
        {
            if (this.nuevo) return;

            textStock.Text = currentStock.stock.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            textStockIdeal.Text = currentStock.stockIdeal.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            textStockMinimo.Text = currentStock.stockMinimo.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            textStockAlerta.Text = currentStock.alertaStock.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            cbxAlmace.SelectedValue = Convert.ToInt32(currentStock.idAlmacen);
        }

        private void btnAddAlmacen_Click(object sender, EventArgs e)
        {
            FormAlmacenNuevo formAlmacen = new FormAlmacenNuevo();
            formAlmacen.ShowDialog();
            this.cargarAlmacenes();
        }

        #region ============================= Guardar y actualizar =======================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                cargarObjeto();
                guardar();
            }
        }

        private bool validarCampos()
        {
            if (textStockIdeal.Text == "")
            {
                errorProvider1.SetError(textStockIdeal, "Rellene este campo");
                textStockIdeal.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textStockMinimo.Text == "")
            {
                errorProvider1.SetError(textStockMinimo, "Rellene este campo");
                textStockMinimo.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textStockAlerta.Text == "")
            {
                errorProvider1.SetError(textStockAlerta, "Rellene este campo");
                textStockAlerta.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (cbxAlmace.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxAlmace, "Elija almenos uno");
                cbxAlmace.Focus();
                return false;
            }
            errorProvider1.Clear();
            return true;
        }
        private async void guardar()
        {
            bloquear(true);
            try
            {
                if (nuevo)
                {
                    Response response = await stockModel.guardar(currentStock);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await stockModel.modificar(currentStock);
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

        private void cargarObjeto()
        {
            currentStock = new Stock(); // crea una nueva instancia de la categoria
            if (!nuevo) currentStock.idProductoStockAlmacen = currentIdStock; // Llenar el id categoria cuando este en esdo modificar
            currentStock.stock = Decimal.Parse(textStock.Text);
            currentStock.stockIdeal = Decimal.Parse(textStockIdeal.Text);
            currentStock.stockMinimo = Decimal.Parse(textStockMinimo.Text);
            currentStock.alertaStock = Decimal.Parse(textStockAlerta.Text);
            currentStock.estado = Convert.ToInt32(chkActivoStock.Checked);
            currentStock.idAlmacen = Convert.ToInt32(cbxAlmace.SelectedValue);
            currentStock.idProducto = formProductoNuevo.currentIDProducto;
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void bloquear(bool state)
        {
            if (state) { Cursor.Current = Cursors.WaitCursor; }
            else { Cursor.Current = Cursors.Default; }
            this.Enabled = !state;
        }
    }
}
