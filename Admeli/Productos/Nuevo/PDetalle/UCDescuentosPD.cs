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
    public partial class UCDescuentosPD : UserControl
    {
        public bool lisenerKeyEvents { get; internal set; }
        private FormProductoNuevo formProductoNuevo;

        private DescuentoModel descuentoModel = new DescuentoModel();
        private OfertaModel ofertaModel = new OfertaModel();

        private List<Descuento> descuentos { get; set; }
        private List<Oferta> ofertas { get; set; }

        private Descuento currentDescuento { get; set; }
        private Oferta currentOferta { get; set; }

        public UCDescuentosPD()
        {
            InitializeComponent();
        }

        public UCDescuentosPD(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
        }

        #region ================================ Root Load ================================
        private void UCDescuentosPD_Load(object sender, EventArgs e)
        {
            this.darFormatoDecimales();
            this.reLoad();
        }
        private void darFormatoDecimales()
        {
            //Descuentos
            dataGridViewDescuento.Columns["cantidadMinima"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewDescuento.Columns["cantidadMaxima"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewDescuento.Columns["descuento"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewDescuento.Columns["cantidadMinima"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewDescuento.Columns["cantidadMaxima"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewDescuento.Columns["descuento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Ofertas
            dataGridViewOferta.Columns["descuentoOferta"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewOferta.Columns["descuentoOferta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }

        internal void reLoad()
        {
            cargarDescuentos();
            cargarOfertas();
        } 
        #endregion

        #region ============================= Paint and Decoration =============================
        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panelHeader);
        }

        private void decorationDescuento()
        {
            if (dataGridViewDescuento.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewDescuento.Rows)
            {
                int idDescuentoProductoGrupo = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                currentDescuento = descuentos.Find(x => x.idDescuentoProductoGrupo == idDescuentoProductoGrupo); // Buscando la categoria en las lista de categorias
                if (currentDescuento.estado == 0)
                {
                    dataGridViewDescuento.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }

        private void decorationOferta()
        {
            if (dataGridViewOferta.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewOferta.Rows)
            {
                int idOfertaProductoGrupo = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                currentOferta = ofertas.Find(x => x.idOfertaProductoGrupo == idOfertaProductoGrupo); // Buscando la categoria en las lista de categorias
                if (currentOferta.estado == 0)
                {
                    dataGridViewOferta.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        } 
        #endregion

        #region ================================== Load Data ==================================
        private async void cargarDescuentos()
        {
            loadState(true);
            try
            {
                List<Descuento> responseList = await descuentoModel.descuentos(formProductoNuevo.currentIDProducto);

                // Ingresando
                descuentos = responseList;
                descuentoBindingSource.DataSource = null;
                descuentoBindingSource.DataSource = descuentos;
                dataGridViewDescuento.Refresh();

                // Formato de celdas en el datagridview
                decorationDescuento();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
        }

        private async void cargarOfertas()
        {
            loadState(true);
            try
            {
                List<Oferta> responseList = await ofertaModel.ofertas(formProductoNuevo.currentIDProducto);

                // Ingresando
                ofertas = responseList;
                ofertaBindingSource.DataSource = ofertas;
                dataGridViewOferta.Refresh();

                // Formato de celdas en el datagridview
                decorationOferta();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
        } 
        #endregion

        #region ================================= LOAD STATES =================================
        private void loadState(bool state)
        {
            formProductoNuevo.appLoadState(state);
        }
        #endregion

        #region ================================ CRUD Descuento ================================
        private void dataGridViewDescuento_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarDescuento();
        }

        private void btnNuevoDescuento_Click(object sender, EventArgs e)
        {
            executeNuevoDescuento();
        }

        private void btnModificarDescuento_Click(object sender, EventArgs e)
        {
            executeModificarDescuento();
        }

        private void btnEliminarDescuento_Click(object sender, EventArgs e)
        {
            executeEliminarDescuento();
        }

        private void btnActualizarDescuento_Click(object sender, EventArgs e)
        {
            executeActualizarDescuento();
        }

        private void executeNuevoDescuento()
        {
            FormDescuentoNuevo formDescuento = new FormDescuentoNuevo(formProductoNuevo);
            formDescuento.ShowDialog();
            cargarDescuentos();
        }

        private void executeModificarDescuento()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewDescuento.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewDescuento.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idDescuentoProductoGrupo = Convert.ToInt32(dataGridViewDescuento.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentDescuento = descuentos.Find(x => x.idDescuentoProductoGrupo == idDescuentoProductoGrupo); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormDescuentoNuevo formDescuento = new FormDescuentoNuevo(formProductoNuevo, currentDescuento);
            formDescuento.ShowDialog();
            cargarDescuentos(); // recargando loas registros en el datagridview
        }

        private async void executeEliminarDescuento()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewDescuento.Rows.Count == 0)
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
                int index = dataGridViewDescuento.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentDescuento = new Descuento(); //creando una instancia del objeto categoria
                currentDescuento.idDescuentoProductoGrupo = Convert.ToInt32(dataGridViewDescuento.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await descuentoModel.eliminar(currentDescuento); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarDescuentos(); // recargando el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false); // cambiando el estado
            }
        }

        private void executeActualizarDescuento()
        {
            cargarDescuentos();
        }
        #endregion

        #region ============================= CRUD Oferta =============================
        private void dataGridViewOferta_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarOferta();
        }

        private void btnNuevoOferta_Click(object sender, EventArgs e)
        {
            executeNuevoOferta();
        }

        private void btnModificarOferta_Click(object sender, EventArgs e)
        {
            executeModificarOferta();
        }

        private void btnEliminarOferta_Click(object sender, EventArgs e)
        {
            executeEliminarOferta();
        }

        private void btnActualizarOferta_Click(object sender, EventArgs e)
        {
            executeActualizarOferta();
        }

        private void executeNuevoOferta()
        {
            FormOfertaNuevo formOferta = new FormOfertaNuevo(formProductoNuevo);
            formOferta.ShowDialog();
            cargarOfertas();
        }

        private void executeModificarOferta()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewOferta.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewOferta.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idOfertaProductoGrupo = Convert.ToInt32(dataGridViewOferta.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentOferta = ofertas.Find(x => x.idOfertaProductoGrupo == idOfertaProductoGrupo); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormOfertaNuevo formOferta = new FormOfertaNuevo(formProductoNuevo, currentOferta);
            formOferta.ShowDialog();
            cargarOfertas(); // recargando loas registros en el datagridview
        }

        private async void executeEliminarOferta()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewOferta.Rows.Count == 0)
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
                int index = dataGridViewOferta.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentOferta = new Oferta(); //creando una instancia del objeto categoria
                currentOferta.idOfertaProductoGrupo = Convert.ToInt32(dataGridViewOferta.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await ofertaModel.eliminar(currentOferta); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarOfertas(); // recargando el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false); // cambiando el estado
            }
        }

        private void executeActualizarOferta()
        {
            cargarOfertas();
        }
        #endregion

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeGuardar();
        }

        private void btnGuardarSalir_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeGuardarSalir();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeCerrar();
        }
    }
}
