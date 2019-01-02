using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Modelo;
using Entidad;

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class UCStockPD : UserControl
    {
        private PrecioModel precioModel = new PrecioModel();
        private StockModel stockModel = new StockModel();

        private Precio currentPrecio { get; set; }
        private Stock currentStock { get; set; }

        private List<Precio> precios { get; set; }
        private List<Stock> stocks { get; set; }

        public bool lisenerKeyEvents { get; internal set; }
        private FormProductoNuevo formProductoNuevo;

        #region ================================ Constructor ================================
        public UCStockPD()
        {
            InitializeComponent();
        }

        public UCStockPD(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
        }
        #endregion

        #region =========================== Root Load ===========================
        private void UCStockPD_Load(object sender, EventArgs e)
        {
            this.darFormatoDecimales();
            this.reLoad();
        }

        private void darFormatoDecimales()
        {
            //Precio
            dataGridViewPrecios.Columns["precioCompetencia"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewPrecios.Columns["utilidad"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewPrecios.Columns["precioVenta"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewPrecios.Columns["precioCompetencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewPrecios.Columns["utilidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewPrecios.Columns["precioVenta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Stock
            dataGridViewStock.Columns["stock"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewStock.Columns["stockIdeal"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewStock.Columns["stockMin"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewStock.Columns["alertaStock"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewStock.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewStock.Columns["stockIdeal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewStock.Columns["stockMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewStock.Columns["alertaStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        internal void reLoad()
        {
            cargarPrecios();
            cargarStock();
        }
        #endregion

        #region =========================== Loads ===========================
        private async void cargarPrecios()
        {
            try
            {
                formProductoNuevo.appLoadState(true);
                precios = await precioModel.precioProducto(formProductoNuevo.currentIDProducto);
                precioBindingSource.DataSource = precios;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                formProductoNuevo.appLoadState(false);
            }
        }

        private async void cargarStock()
        {
            try
            {
                formProductoNuevo.appLoadState(true);
                stocks = await stockModel.stockProducto(formProductoNuevo.currentIDProducto);
                stockBindingSource.DataSource = stocks;

                // dando formato en las celdas
                decorationDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                formProductoNuevo.appLoadState(false);
            }
        } 
        #endregion

        #region ============================== Paint decoration ==============================
        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panelHeader);
        }

        private void decorationDataGridView()
        {
            if (dataGridViewStock.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewStock.Rows)
            {
                int idProductoStockAlmacen = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                currentStock = stocks.Find(x => x.idProductoStockAlmacen == idProductoStockAlmacen); // Buscando la categoria en las lista de categorias
                if (currentStock.estado == 0)
                {
                    dataGridViewStock.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }
        #endregion

        #region ==================== CRUD ====================
        private void dataGridViewStocks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarStock();
        }

        private void btnNuevoStock_Click(object sender, EventArgs e)
        {
            executeNuevoStock();
        }

        private void dataGridViewPrecios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarPrecio();
        }

        private void executeNuevoStock()
        {
            FormStockNuevo formStock = new FormStockNuevo(this.formProductoNuevo);
            formStock.ShowDialog();
            this.cargarStock();
        }

        private void executeModificarPrecio()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewPrecios.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewPrecios.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idPrecioProducto = Convert.ToInt32(dataGridViewPrecios.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentPrecio = precios.Find(x => x.idPrecioProducto == idPrecioProducto); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormPrecioDetalle formPrecioDetalle = new FormPrecioDetalle(currentPrecio,formProductoNuevo.currentProducto.precioCompra,formProductoNuevo.currentIDProducto);
            formPrecioDetalle.ShowDialog();
            this.cargarPrecios(); // recargando loas registros en el datagridview
        }

        private void executeModificarStock()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewStock.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewStock.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idProductoStockAlmacen = Convert.ToInt32(dataGridViewStock.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentStock = stocks.Find(x => x.idProductoStockAlmacen == idProductoStockAlmacen); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormStockNuevo formStock = new FormStockNuevo(currentStock, this.formProductoNuevo);
            formStock.ShowDialog();
            this.cargarStock(); // recargando los registros
        }

        private async void executeAnularPrecio()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewPrecios.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Desactivar o anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                formProductoNuevo.appLoadState(true); // estado cargando

                int index = dataGridViewPrecios.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentPrecio = new Precio(); //creando una instancia del objeto correspondiente
                currentPrecio.idPrecioProducto = Convert.ToInt32(dataGridViewPrecios.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

                // Comprobando si el registro ya esta desactivado
                if (precios.Find(x => x.idPrecioProducto == currentPrecio.idPrecioProducto).estado == 0)
                {
                    MessageBox.Show("Este registro ya esta desactivado", "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Procediendo con las desactivacion
                Response response = await precioModel.desactivar(currentPrecio);
                MessageBox.Show(response.msj, "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cargarPrecios(); // recargando los registros en el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Anular", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                formProductoNuevo.appLoadState(false);
            }
        }

        private async void executeEliminarStock()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewStock.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Pregunta de seguridad de eliminacion
            DialogResult dialog = MessageBox.Show("¿Está seguro de eliminar este registro?", "Eliminar",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialog == DialogResult.No) return;


            try
            {
                formProductoNuevo.appLoadState(true); // cambiando el estado

                int index = dataGridViewStock.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentStock = new Stock(); //creando una instancia del objeto categoria
                currentStock.idProductoStockAlmacen = Convert.ToInt32(dataGridViewStock.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                Response response = await stockModel.eliminar(currentStock); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarStock(); // recargando el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                formProductoNuevo.appLoadState(false); // cambiando el estado
            }
        }


        /*
private async void executeAnularStock()
{
   // Verificando la existencia de datos en el datagridview
   if (dataGridViewStocks.Rows.Count == 0)
   {
       MessageBox.Show("No hay un registro seleccionado", "Desactivar o anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
       return;
   }

   try
   {
       int index = dataGridViewStocks.CurrentRow.Index; // Identificando la fila actual del datagridview
       currentStock = new Stock(); //creando una instancia del objeto correspondiente
       currentStock.idProductoStockAlmacen = Convert.ToInt32(dataGridViewStocks.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

       // Comprobando si el registro ya esta desactivado
       if (stocks.Find(x => x.idProductoStockAlmacen == currentStock.idProductoStockAlmacen).estado == 0)
       {
           MessageBox.Show("Este registro ya esta desactivado", "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           return;
       }

       // Procediendo con las desactivacion
       Response response = await stockModel.desactivar(currentStock);
       MessageBox.Show(response.msj, "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Information);
       this.cargarStock(); // recargando los registros en el datagridview
   }
   catch (Exception ex)
   {
       MessageBox.Show("Error: " + ex.Message, "Anular", MessageBoxButtons.OK, MessageBoxIcon.Warning);
   }
}*/
        #endregion

        private void btnEditarStock_Click(object sender, EventArgs e)
        {
            executeModificarStock();
        }

        private void btnEliminarStock_Click(object sender, EventArgs e)
        {
            executeEliminarStock();
        }

        private void btnActualizarStock_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGuardarSalir_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeGuardarSalir();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeGuardar();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeCerrar();
        }
    }
}
