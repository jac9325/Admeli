using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modelo;
using Entidad;
using Admeli.Componentes;
using Admeli.Ventas.Nuevo;

namespace Admeli.Ventas
{
    public partial class UCDescuentosOfertas : UserControl
    {
        private FormPrincipal formPrincipal;
        
        public bool lisenerKeyEvents { get; set; }
        private DescuentoModel descuentoModel = new DescuentoModel();
        private Paginacion paginacion;
        private List<DatoCuentaCobrar> listaCuentasCobrar { get; set; }
        private List<DatosDescuentosOfertas> listaDescuentos;

        private DatosDescuentosOfertas currentDatosDescuentosOfertas { get; set; }
       

        #region =================================== CONSTRUCTOR ===================================
        public UCDescuentosOfertas()
        {
            InitializeComponent();
            lblSpeedPages.Text = ConfigModel.configuracionGeneral.itemPorPagina.ToString();     // carganto los items por página
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));

        }

        public UCDescuentosOfertas(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));
            this.reLoad();
                     

        }
        #endregion

        #region =========================== Estados ===========================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelNavigation.Enabled = !state;
            dataGridView.Enabled = !state;
        }
        #endregion

        #region ==================================== ROOT LOAD ====================================
        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarRegistros();
            }
            lisenerKeyEvents = true; // Active lisener key events
        }

        #endregion

        #region ======================= Loads =======================
        private void cargarComponentes()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idEstado", typeof(string));
            table.Columns.Add("estado", typeof(string));

            table.Rows.Add("todos", "Todos los estados");
            table.Rows.Add("0", "Anulados");
            table.Rows.Add("1", "Activos");

            //cbxEstados.DataSource = table;
            //cbxEstados.DisplayMember = "estado";
            //cbxEstados.ValueMember = "idEstado";
            //cbxEstados.ComboBox.SelectedIndex = 0;
        }

        private async void cargarRegistros()
        {
            loadState(true);
            try
            {
                
                ObjetosDescuentosOfertas rootData = await descuentoModel.descuentoofertacodigo(paginacion.currentPage, paginacion.speed);
                if (rootData.nro_registros == 0) return;

                paginacion.itemsCount = rootData.nro_registros;
                paginacion.reload();
                listaDescuentos = rootData.datos;                             
                datosDescuentosOfertasBindingSource.DataSource = listaDescuentos;
                dataGridView.Refresh();

                // Mostrando la paginacion
                mostrarPaginado();

                // formato de celdas
                decorationDataGridView();

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

        #region ===================== Eventos Páginación =====================

        private void mostrarPaginado()
        {
            lblCurrentPage.Text = paginacion.currentPage.ToString();
            lblPageAllItems.Text = String.Format("{0} Registros", paginacion.itemsCount.ToString());
            lblPageCount.Text = paginacion.pageCount.ToString();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (lblCurrentPage.Text != "1")
            {
                paginacion.previousPage();
                cargarRegistros();
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (lblCurrentPage.Text != "1")
            {
                paginacion.firstPage();
                cargarRegistros();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lblPageCount.Text == "0") return;
            if (lblPageCount.Text != lblCurrentPage.Text)
            {
                paginacion.nextPage();
                cargarRegistros();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (lblPageCount.Text == "0") return;
            if (lblPageCount.Text != lblCurrentPage.Text)
            {
                paginacion.lastPage();
                cargarRegistros();
            }
        }

        private void lblSpeedPages_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paginacion.speed = Convert.ToInt32(lblSpeedPages.Text);
                paginacion.currentPage = 1;
                cargarRegistros();
            }
        }

        private void lblCurrentPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paginacion.reloadPage(Convert.ToInt32(lblCurrentPage.Text));
                cargarRegistros();
            }
        }

        private void lblCurrentPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void lblSpeedPages_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }
        #endregion


        #region ================================ PAINT AND DECORATION ================================

        private void decorationDataGridView()
        {
            //Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string codigo = Convert.ToString(row.Cells[0].Value); // obteniedo el idCategoria del datagridview

                DatosDescuentosOfertas dDescuentosOfertas  = listaDescuentos.Find(x => x.codigo == codigo); // Buscando la categoria en las lista de categorias
                if (dDescuentosOfertas != null )
                {
                    if (dDescuentosOfertas.estado == 0)
                    {
                        dataGridView.ClearSelection();
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                    }
                }
            }
                

        }
        #endregion

        #region ==================== CRUD ====================
        private void btnNuevo_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
    
    }

        private void descuentosOfertasBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            executeAnular();
        }
        private async void executeAnular()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Desactivar o anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

           
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //executeModificar();
        }

        #endregion

        private void btnDescuento_Click(object sender, EventArgs e)
        {
            FormDescuentoNuevo formDescuento = new FormDescuentoNuevo();
            formDescuento.ShowDialog();
            cargarRegistros();


        }

        private void btnOfertas_Click(object sender, EventArgs e)
        {
            FormOfertaNuevo formOferta = new FormOfertaNuevo();
            formOferta.ShowDialog();
            cargarRegistros();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            executeModificar();
        }

        public  void executeModificar()
        {

            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
            string codigo= Convert.ToString(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

            currentDatosDescuentosOfertas = listaDescuentos.Find(x => x.codigo == codigo); // Buscando la categoria en las lista de categorias





            if (currentDatosDescuentosOfertas.tipoDescuento == "Oferta")
            {

                FormOfertaNuevo formOfertaNuevo = new FormOfertaNuevo(currentDatosDescuentosOfertas);
                formOfertaNuevo.ShowDialog();



            }

            else
            {

                FormDescuentoNuevo formOfertaNuevo = new FormDescuentoNuevo(currentDatosDescuentosOfertas);
                formOfertaNuevo.ShowDialog();


            }
            // Mostrando el formulario de modificacion

            cargarRegistros(); // recargando loas registros en el datagridview
        }



    }
}
