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
using Entidad;
using Modelo;
using Admeli.Productos.Nuevo.PDetalle.sub;

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class UCAdicionalPD : UserControl
    {
        public bool lisenerKeyEvents { get; internal set; }
        private FormProductoNuevo formProductoNuevo;

        private PresentacionModel presentacionModel = new PresentacionModel();
        private VarianteModel varianteModel = new VarianteModel();
        private AlternativaModel alternativaModel = new AlternativaModel();

        private List<Presentacion> presentaciones { get; set; }
        private List<Variante> variantes { get; set; }
        private List<Alternativa> alternativas { get; set; }

        private Presentacion currentPresentacion { get; set; }
        private Variante currentVariante { get; set; }
        private Alternativa currentAlternativa { get; set; }

        public UCAdicionalPD()
        {
            InitializeComponent();
        }

        public UCAdicionalPD(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.chkVenderSinStock.Checked = formProductoNuevo.currentProducto.ventaVarianteSinStock;
        }

        #region ================================== Root Load ==================================
        private void UCAdicionalPD_Load(object sender, EventArgs e)
        {
            this.darFormatoDecimales();
            this.reLoad();
        }
        private void darFormatoDecimales()
        {
            
            dataGridViewPresentacion.Columns["cantidadUnitaria"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridViewPresentacion.Columns["cantidadUnitaria"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        internal void reLoad()
        {
            cargarPresentaciones();
            cargarVariantes();
            cargarAlternativas();
        }
        #endregion

        #region ================================= LOAD STATES =================================
        private void loadState(bool state)
        {
            formProductoNuevo.appLoadState(state);
        } 
        #endregion

        #region ================================== PAINT AND DECORATION ==================================
        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panelHeader);
        }

        private void panelFooter_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.topLine(panelHeader);
        }

        private void UCAdicionalPD_Paint(object sender, PaintEventArgs e)
        {
            int containerWidth = this.Size.Width;
            int itemWidth = containerWidth / 2;
            panelItem1.Size = new Size(itemWidth, 100);
        }

        private void decorationPresentacion()
        {
            if (dataGridViewPresentacion.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewPresentacion.Rows)
            {
                int idPresentacion = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                currentPresentacion = presentaciones.Find(x => x.idPresentacion == idPresentacion); // Buscando la categoria en las lista de categorias
                if (currentPresentacion.estado == 0)
                {
                    dataGridViewPresentacion.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }

        private void decorationVariante()
        {
            if (dataGridViewVariante.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewVariante.Rows)
            {
                int idVariante = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                currentVariante = variantes.Find(x => x.idVariante == idVariante); // Buscando la categoria en las lista de categorias
                if (currentVariante.estado == 0)
                {
                    dataGridViewVariante.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }

        private void decorationAlternativa()
        {
            if (dataGridViewAlternativa.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewAlternativa.Rows)
            {
                int idAlternativa = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                currentAlternativa = alternativas.Find(x => x.idAlternativa == idAlternativa); // Buscando la categoria en las lista de categorias
                if (currentAlternativa.estado == 0)
                {
                    dataGridViewAlternativa.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }

        #endregion

        #region ====================================== Loads ======================================
        private async void cargarPresentaciones()
        {
            loadState(true);
            try
            {
                List<Presentacion> responseList = await presentacionModel.presentaciones(formProductoNuevo.currentIDProducto);

                // Ingresando
                presentaciones = responseList;
                presentacionBindingSource.DataSource = presentaciones;
                dataGridViewPresentacion.Refresh();

                // Formato de celdas en el datagridview
                decorationPresentacion();
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

        private async void cargarVariantes()
        {
            loadState(true);
            try
            {
                List<Variante> responseList = await varianteModel.variantes(formProductoNuevo.currentIDProducto);

                // Ingresando
                variantes = responseList;
                varianteBindingSource.DataSource = variantes;
                dataGridViewVariante.Refresh();

                // Formato de celdas en el datagridview
                decorationVariante();
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

        private async void cargarAlternativas()
        {
            loadState(true);
            try
            {
                if (dataGridViewVariante.Rows.Count < 1) return;
                int index = dataGridViewVariante.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idVariante = Convert.ToInt32(dataGridViewVariante.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

                List<Alternativa> responseList = await alternativaModel.alternativas(idVariante);

                // Ingresando
                alternativas = responseList;
                alternativaBindingSource.DataSource = alternativas;
                dataGridViewAlternativa.Refresh();

                // Formato de celdas en el datagridview
                decorationAlternativa();
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



        #region ================================== CRUD Precentacion ==================================
        private void dataGridViewPresentacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //executeModificarPrecentacion();
        }

        private void btnNuevoPrecentacion_Click(object sender, EventArgs e)
        {
            this.executeNuevoPresentacion();
        }

        private void btnModificarPrecentacion_Click(object sender, EventArgs e)
        {
            this.executeModificarPrecentacion();
        }

        private void btnActualizarPrcentacion_Click(object sender, EventArgs e)
        {
            this.executeActualizarPrecentacion();
        }

        private void btnEliminarPrecentacion_Click(object sender, EventArgs e)
        {
            this.executeEliminarPrecentacion();
        }

        private void executeNuevoPresentacion()
        {
            FormPresentacionNuevo formPresentacion = new FormPresentacionNuevo(formProductoNuevo);
            formPresentacion.ShowDialog();
            cargarPresentaciones();
        }

        private void executeModificarPrecentacion()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewPresentacion.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewPresentacion.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idPresentacion = Convert.ToInt32(dataGridViewPresentacion.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentPresentacion = presentaciones.Find(x => x.idPresentacion == idPresentacion); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormPresentacionNuevo formPresentacion = new FormPresentacionNuevo(formProductoNuevo, currentPresentacion);
            formPresentacion.ShowDialog();
            cargarPresentaciones(); // recargando loas registros en el datagridview
        }

        private void executeActualizarPrecentacion()
        {
            cargarPresentaciones();
        }

        private async void executeEliminarPrecentacion()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewPresentacion.Rows.Count == 0)
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
                int index = dataGridViewPresentacion.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentPresentacion = new Presentacion(); //creando una instancia del objeto categoria
                currentPresentacion.idPresentacion = Convert.ToInt32(dataGridViewPresentacion.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await presentacionModel.eliminar(currentPresentacion); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarPresentaciones(); // recargando el datagridview
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
        #endregion

        #region ====================================== CRUD Variante ======================================
        private void dataGridViewVariante_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarVariante();
        }

        private void btnNuevoVariante_Click(object sender, EventArgs e)
        {
            this.executeNuevoVariante();
        }

        private void btnModificarVariante_Click(object sender, EventArgs e)
        {
            this.executeModificarVariante();
        }

        private void btnActualizarVariante_Click(object sender, EventArgs e)
        {
            this.executeActualizarVariante();
        }

        private void btnEliminarVariante_Click(object sender, EventArgs e)
        {
            this.executeEliminarVariante();
        }

        private void executeNuevoVariante()
        {
            FormVarianteNuevo formVariante = new FormVarianteNuevo(formProductoNuevo);
            formVariante.ShowDialog();
            cargarVariantes();
        }

        private void executeModificarVariante()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewVariante.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewVariante.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idVariante = Convert.ToInt32(dataGridViewVariante.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentVariante = variantes.Find(x => x.idVariante == idVariante); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormVarianteNuevo formVariante = new FormVarianteNuevo(formProductoNuevo, currentVariante);
            formVariante.ShowDialog();
            cargarVariantes(); // recargando loas registros en el datagridview
        }

        private void executeActualizarVariante()
        {
            cargarVariantes();
        }

        private async void executeEliminarVariante()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewVariante.Rows.Count == 0)
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
                int index = dataGridViewVariante.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentVariante = new Variante(); //creando una instancia del objeto categoria
                currentVariante.idVariante = Convert.ToInt32(dataGridViewVariante.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await varianteModel.eliminar(currentVariante); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarVariantes(); // recargando el datagridview
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
        #endregion

        #region ====================================== CRUD Alternativa ======================================
        private void dataGridViewAlternativa_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarAlternativa();
        }

        private void btnNuevoAlternativa_Click(object sender, EventArgs e)
        {
            this.executeNuevoAlternativa();
        }

        private void btnModificarAlternativa_Click(object sender, EventArgs e)
        {
            this.executeModificarAlternativa();
        }

        private void btnActualizarAlternativa_Click(object sender, EventArgs e)
        {
            this.executeActualizarAlternativa();
        }

        private void btnEliminarAlternativa_Click(object sender, EventArgs e)
        {
            this.executeEliminarAlternativa();
        }

        private void executeNuevoAlternativa()
        {
            if (dataGridViewVariante.Rows.Count == 0)
            {
                MessageBox.Show("No hay una variante seleccionada", "Nuevo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            int index = dataGridViewVariante.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idVariante = Convert.ToInt32(dataGridViewVariante.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            FormAlternativaNuevo formAlternativa = new FormAlternativaNuevo(formProductoNuevo, idVariante);
            formAlternativa.ShowDialog();
            cargarAlternativas();
        }

        private void executeModificarAlternativa()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewAlternativa.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Obteniendo el objeto a modificar
            int index = dataGridViewAlternativa.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idAlternativa = Convert.ToInt32(dataGridViewAlternativa.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            // Obteniendo el objeto padre de esta tabla
            int index2 = dataGridViewVariante.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idVariante = Convert.ToInt32(dataGridViewVariante.Rows[index2].Cells[0].Value); // obteniedo el idRegistro del datagridview
             
            // Actualizando los datos a modificar
            currentAlternativa = alternativas.Find(x => x.idAlternativa == idAlternativa); // Buscando la registro especifico en la lista de registros
            currentAlternativa.idVariante = idVariante;

            // Mostrando el formulario de modificacion
            FormAlternativaNuevo formAlternativa = new FormAlternativaNuevo(formProductoNuevo, currentAlternativa);
            formAlternativa.ShowDialog();
            cargarAlternativas(); // recargando loas registros en el datagridview
        }

        private void executeActualizarAlternativa()
        {
            cargarAlternativas();
        }

        private async void executeEliminarAlternativa()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewAlternativa.Rows.Count == 0)
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
                int index = dataGridViewAlternativa.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentAlternativa = new Alternativa(); //creando una instancia del objeto categoria
                currentAlternativa.idAlternativa = Convert.ToInt32(dataGridViewAlternativa.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await alternativaModel.eliminar(currentAlternativa); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarAlternativas(); // recargando el datagridview
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
        #endregion


        private void dataGridViewVariante_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cargarAlternativas();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            FormgGenerar formgGenerar = new FormgGenerar(formProductoNuevo);
            formgGenerar.ShowDialog();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            formProductoNuevo.currentProducto.ventaVarianteSinStock = chkVenderSinStock.Checked;
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

