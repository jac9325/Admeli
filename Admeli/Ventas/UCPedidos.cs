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
using Admeli.Ventas.Nuevo;
using Modelo;
using Entidad;
using Admeli.Properties;
using Admeli.Ventas.buscar;
using System.Globalization;
using Admeli.Ventas.Nuevo.detalle;

namespace Admeli.Ventas
{
    public partial class UCPedidos : UserControl
    {
        private FormPrincipal formPrincipal;
        public bool lisenerKeyEvents { get; set; }

        private Cotizacion currentCotizacion { get; set; }
        SaveObjectEgreso currentSaveObject { get; set; }

        private List<DetalleV> detallePedidos { get; set; }
        public List<Proveedor> listProveedoresEscogidos { get; set; }
        public List<Proveedor> listProveedores { get; set; }
        public List<Proveedor> listProveedorescombo { get; set; }
        List<DetalleV> pedidoSeleccionados = new List<DetalleV>();

        private List<AdelantoCotizacion> list { get; set; }
        private  Dictionary<int, string> estadoPedidos{ get; set; }
        private Dictionary<string, int> pedidos = new Dictionary<string, int>();
        private DetalleV currentPedido { get; set; }
        private Paginacion paginacion;
        private CotizacionModel cotizacionModel = new CotizacionModel();
        private PersonalModel personalModel = new PersonalModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private ProveedorModel proveedorModel = new ProveedorModel();
        private CajaModel cajaModel = new CajaModel();
        private EgresoModel egresoModel = new EgresoModel();

        private bool iniciado = false;
        private bool enOrden = false;
        private bool enCamino = false;
        private bool arribado = false;
        private bool esperando = false;
        private bool instalando = false;
        private bool instalado = false;
        private bool entregado = false;

        private Dictionary<string, bool> columnSort = new Dictionary<string, bool>();

        List<int> idProveedores = new List<int>();
        #region ================================ CONSTRUCTOR ================================
        public UCPedidos()
        {
            InitializeComponent();

            lblSpeedPages.Text = ConfigModel.configuracionGeneral.itemPorPagina.ToString();     // carganto los items por página
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));
        }

        public UCPedidos(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            listProveedoresEscogidos = new List<Proveedor>();
            lblSpeedPages.Text = ConfigModel.configuracionGeneral.itemPorPagina.ToString();     // carganto los items por página
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));
            InicializarColumnasSort();
        }

        private void InicializarColumnasSort()
        {
            foreach(DataGridViewColumn columns in dataGridView.Columns)
            {
                string name = columns.Name;
                columnSort.Add(name, false);
            }
        }


        private double toDouble(string texto)
        {
            if (texto == "")
            {

                return 0;
            }
            return double.Parse(texto, CultureInfo.GetCultureInfo("en-US"));
        }
        #endregion

        #region =================================== ROOT LOAD ===================================
        private void UCCotizacionCliente_Load(object sender, EventArgs e)
        {
            darFormatoDecimales();
            this.reLoad();

            // Preparando para los eventos de teclado
            this.ParentChanged += ParentChange; // Evetno que se dispara cuando el padre cambia // Este eveto se usa para desactivar lisener key events de este modulo
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }
        }

        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarSucursales();
                cargarPersonales();
                cargarRegistros();
                cargarEstadosEnvio();
                cargarProveedores();
            }
            lisenerKeyEvents = true; // Active lisener key events
        }



        #endregion

        #region ================================== PAINT ==================================
        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelContainer);
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel10);
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel13);
        }
        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel7);
        }
        #endregion

        #region ======================== KEYBOARD =================================
        // Evento que se dispara cuando el padre cambia
        private void ParentChange(object sender, EventArgs e)
        {
            // cambiar la propiedad de lisenerKeyEvents de este modulo
            if (lisenerKeyEvents) lisenerKeyEvents = false;
        }

        // Escuchando los Eventos de teclado
        private void TopLevelControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!lisenerKeyEvents) return;
            switch (e.KeyCode)
            {
                case Keys.F3:
                    executeNuevo();
                    break;
                case Keys.F4:
                    executeModificar();
                    break;
                case Keys.F5:
                    cargarRegistros();
                    break;
                case Keys.F6:
                    executeEliminar();
                    break;
                case Keys.F7:
                    executeAnular();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region =========================== Decoration ===========================
        private void decorationDataGridView()
        {
            
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0) return;
            try
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    int idDetalleCotizacion = Convert.ToInt32(row.Cells["idDetalleCotizacion"].Value); // obteniedo el idCategoria del datagridview
                    if (idDetalleCotizacion > 0)
                    {
                        DetalleV venta = detallePedidos.Find(x => x.idDetalleCotizacion == idDetalleCotizacion); // Buscando la categoria en las lista de categorias

                        if (venta.idP1 == 0 && venta.idP2 ==0 && venta.idP3 == 0)
                        {
                            if (venta.tipo == "ordenPedido")
                            {
                                dataGridView.ClearSelection();
                                row.DefaultCellStyle.BackColor = Color.FromArgb(252, 246, 243);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(177, 75, 9);
                            }
                            else
                            {
                                dataGridView.ClearSelection();
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 236, 228);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                            }
                        }
                        else
                        {
                            if (venta.tipo == "ordenPedido")
                            {
                                dataGridView.ClearSelection();
                                row.DefaultCellStyle.BackColor = Color.FromArgb(232, 255, 234);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(20, 117, 30);
                            }
                        }
                    }
                }
            }
            catch(Exception ex )
            {
                MessageBox.Show("Error: " + ex.Message, "Asignación de Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }
        #endregion

        #region ======================= Loads =======================

        private void darFormatoDecimales()
        {
            dataGridView.Columns["cantidad"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridView.Columns["cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;          
        }
        private async void cargarProveedores()
        {


            loadState(true);
            try
            {
                listProveedores = await proveedorModel.listaProveedoresSeleccion();
                listProveedorescombo = await proveedorModel.listaProveedorescogidos();
                Proveedor proveedor = new Proveedor();
                proveedor.idProveedor = 0;
                proveedor.razonSocial = "Todos";

                listProveedorescombo.Add(proveedor);
                proveedorBindingSource.DataSource = listProveedorescombo;

                cbxProveedores.SelectedValue = 0;
                listProveedoresEscogidos.Clear();

                foreach (Proveedor p in listProveedores)
                {
                    if (p.escogido > 0)
                    {

                        listProveedoresEscogidos.Add(p);

                    }

                }
                //cargarProveedoresSeleccionados();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            finally
            {
                loadState(false);
            }
        }

        //public void cargarProveedoresSeleccionados()
        //{

           
        //    int nro = listProveedoresEscogidos.Count;
        //    switch (nro)
        //    {
        //        case 1:
        //            btnProveedor1.Visible = false;
        //            btnProveedor2.Visible = false;
        //            btnProveedor3.Visible = true;
        //            Proveedor proveedor1 = listProveedoresEscogidos.Find(X => X.escogido == 1);
        //            btnProveedor3.Text = proveedor1.razonSocial;
        //            break;
        //        case 2:
        //            btnProveedor1.Visible = false;
        //            btnProveedor2.Visible = true;
        //            btnProveedor3.Visible = true;
        //            Proveedor proveedor21 = listProveedoresEscogidos.Find(X => X.escogido == 1);
        //            Proveedor proveedor22 = listProveedoresEscogidos.Find(X => X.escogido == 2);
        //            btnProveedor2.Text = proveedor21.razonSocial;
        //            btnProveedor3.Text = proveedor22.razonSocial;
        //            break;
        //        case 3:
        //            btnProveedor1.Visible = true;
        //            btnProveedor2.Visible = true;
        //            btnProveedor3.Visible = true;

        //            Proveedor proveedor31 = listProveedoresEscogidos.Find(X => X.escogido == 1);
        //            Proveedor proveedor32 = listProveedoresEscogidos.Find(X => X.escogido == 2);
        //            Proveedor proveedor33 = listProveedoresEscogidos.Find(X => X.escogido == 3);
        //            btnProveedor1.Text = proveedor31.razonSocial;
        //            btnProveedor2.Text = proveedor32.razonSocial;
        //            btnProveedor3.Text = proveedor33.razonSocial;

        //            break;

        //    }
        //}
        private void cargarEstadosEnvio()
        {
            estadoPedidos = new Dictionary<int, string>();
            estadoPedidos.Add(0, "Iniciado");
            estadoPedidos.Add(1, "Comprobado");
            estadoPedidos.Add(2, "En Orden");
            estadoPedidos.Add(3, "En camino");
            estadoPedidos.Add(4, "Arribado");
            estadoPedidos.Add(5, "Esperando Cliente");
            estadoPedidos.Add(6, "Instalando");
            estadoPedidos.Add(7, "Instalado");
            estadoPedidos.Add(8, "Entregado");
            estadoPedidos.Add(9, "Finalizado");

        }
        private void cargarSucursales()
        {
            try
            {
                List<Sucursal> listSucCargar = new List<Sucursal>();
                List<Sucursal> listSuc = ConfigModel.listSucursales;
                Sucursal sucursal = new Sucursal();
                sucursal.idSucursal = 0;
                sucursal.nombre = "Todas las sucursales";
                listSucCargar.Add(sucursal);
                listSucCargar.AddRange(listSuc);
                sucursalBindingSource.DataSource = listSucCargar;                
                cbxSucursales.SelectedValue = 0;

                //sucursalBindingSource.DataSource = await sucursalModel.listarSucursalesActivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarPersonales()
        {
            try
            {
                //Si el personal logeado es gerente o administrador debe listar a todos los cliente
                if(ConfigModel.asignacionPersonal.idPuntoGerencia != 0 || ConfigModel.asignacionPersonal.idPuntoAdministracion != 0)
                {
                    
                    personalBindingSource.DataSource = await personalModel.listarPersonalVenta(0);
                    cbxPersonales.SelectedValue = PersonalModel.personal.idPersonal;
                }
                else
                {
                    List<Personal> listaPersonal = new List<Personal>();
                    Personal personal = new Personal();
                    personal.idPersonal = PersonalModel.personal.idPersonal;
                    personal.nombres = PersonalModel.personal.nombres;
                    Personal personal2 = new Personal();
                    personal2.idPersonal = 0;
                    personal2.nombres = "Todos los Personales";
                    listaPersonal.Add(personal);
                    listaPersonal.Add(personal2);
                    personalBindingSource.DataSource = listaPersonal;
                    cbxPersonales.SelectedValue = PersonalModel.personal.idPersonal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void cargarEstadoPedido()
        {
            pedidos.Clear();
            int i = 0;
        
            if (iniciado)
            {
                pedidos.Add("id" + i++, 0);
            }
            if (enOrden)
            {
                pedidos.Add("id" + i++, 2);
            }
            if (enCamino)
            {
                pedidos.Add("id" + i++, 3);
            }
            if (arribado)
            {
                pedidos.Add("id" + i++, 4);
            }
            if (esperando)
            {
                pedidos.Add("id" + i++, 5);
            }
            if (instalando)
            {
                pedidos.Add("id" + i++, 6);
            }
            if (instalado)
            {
                pedidos.Add("id" + i++, 7);

            }
            if (entregado)
            {
                pedidos.Add("id" + i++, 8);
            }
        }

        public  void cargarDatosPedidos()
        {
            if (cbxProveedores.SelectedIndex == -1) return;
            int idProveedor = (int)cbxProveedores.SelectedValue;

            if (idProveedor != 0)
            {
                //cargar lo datos actuales para le proveedor
                foreach (DetalleV D  in detallePedidos)
                {
                    if(D.idP1== idProveedor)
                    {
                        D.pc = D.pc1;
                        D.cant = D.cant1;
                        D.obsevaciones = D.observacionesP1;
                    }
                    if (D.idP2 == idProveedor)
                    {
                        D.pc = D.pc2;
                        D.cant = D.cant2;
                        D.obsevaciones = D.observacionesP2;
                    }
                    if (D.idP3 == idProveedor)
                    {
                        D.pc = D.pc3;
                        D.cant = D.cant3;
                        D.obsevaciones = D.observacionesP3;
                    }
                }
                detalleVBindingSource.DataSource = detallePedidos;
                dataGridView.Refresh();
                dataGridView.Columns["pc1"].Visible = true;
                //dataGridView.Columns["pc2"].Visible = false;
                //dataGridView.Columns["pc3"].Visible = false;
                dataGridView.Columns["cant1"].Visible = true;
                //dataGridView.Columns["cant2"].Visible = false;
                //dataGridView.Columns["cant3"].Visible = false;
                dataGridView.Columns["cant"].Visible =false;
                //dataGridView.Columns["observacion"].Visible = true;
                // 

            }
            else
            {
                dataGridView.Columns["pc1"].Visible = true;
                //dataGridView.Columns["observacion"].Visible = true;
                //dataGridView.Columns["pc2"].Visible = true;
                //dataGridView.Columns["pc3"].Visible = true;
                dataGridView.Columns["cant1"].Visible = true;
                //dataGridView.Columns["cant2"].Visible = true;
                //dataGridView.Columns["cant3"].Visible = true;
                dataGridView.Columns["cant"].Visible =false;

            }
            



        }
        private async void cargarRegistros()
        {
            loadState(true);
            string Filtrado = txtfiltrado.Text.Trim();
            cargarEstadoPedido();
            if (Filtrado == "")
            {
                cargarRegistrosSinfiltro();
                return;
            }

            try
            {
                int personalId = (cbxPersonales.SelectedIndex == -1) ? PersonalModel.personal.idPersonal : Convert.ToInt32(cbxPersonales.SelectedValue);
                int sucursalId = (cbxSucursales.SelectedIndex == -1) ? ConfigModel.sucursal.idSucursal : Convert.ToInt32(cbxSucursales.SelectedValue);
                int proveedorId = (cbxProveedores.SelectedIndex == -1) ? 0 : Convert.ToInt32(cbxProveedores.SelectedValue);

                RootObject<DetalleV> rootData = await cotizacionModel.PedidosFiltro(pedidos,sucursalId, proveedorId,personalId, Filtrado,paginacion.currentPage, paginacion.speed);
                // actualizando datos de páginacón
                paginacion.itemsCount = rootData.nro_registros;
                paginacion.reload();

                // Ingresando
                detallePedidos = rootData.datos;
                detalleVBindingSource.DataSource = detallePedidos;
                dataGridView.DataSource = detalleVBindingSource;

                dataGridView.Refresh();
                mostrarPaginado();
                cargarDatosPedidos();
                dataGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Registros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pedidos.Clear();
            }
            finally
            {
                decorationDataGridView();
                loadState(false);
            }
        }



        private async void cargarRegistrosSinfiltro()
        {
            loadState(true);
            try
            {
                int personalId = (cbxPersonales.SelectedIndex == -1) ? PersonalModel.personal.idPersonal : Convert.ToInt32(cbxPersonales.SelectedValue);
                int sucursalId = (cbxSucursales.SelectedIndex == -1) ? ConfigModel.sucursal.idSucursal : Convert.ToInt32(cbxSucursales.SelectedValue);
                int proveedorId = (cbxProveedores.SelectedIndex == -1) ? 0 : Convert.ToInt32(cbxProveedores.SelectedValue);

                RootObject<DetalleV> rootData = await cotizacionModel.Pedidos(pedidos, sucursalId, proveedorId, personalId, paginacion.currentPage, paginacion.speed);

                // actualizando datos de páginacón
                paginacion.itemsCount = rootData.nro_registros;
                paginacion.reload();

                // Ingresando
                detallePedidos = rootData.datos;
                detalleVBindingSource.DataSource = detallePedidos;
                dataGridView.DataSource = detalleVBindingSource;
                dataGridView.Refresh();
                mostrarPaginado();
                cargarDatosPedidos();
                dataGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                decorationDataGridView();
                loadState(false);
            }
        }



        #endregion

        #region =========================== Estados ===========================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelNavigation.Enabled = !state;
            panelCrud.Enabled = !state;
            dataGridView.Enabled = !state;
            panelTools.Enabled = !state;
            panel4.Enabled = !state;
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

        #region ==================== CRUD ====================
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Activar edición de ciertos campos
            //executeModificar();
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
            dataGridView.ReadOnly = false;

            foreach (DataGridViewRow R in dataGridView.Rows)
            {
                R.ReadOnly = true;
                foreach (DataGridViewCell C in R.Cells)
                {
                    C.Style.SelectionBackColor = R.DefaultCellStyle.SelectionBackColor;
                }
            }

            int idDetalleCotizacion = (int)dataGridView.CurrentRow.Cells["idDetalleCotizacion"].Value;
            DetalleV detalleV = detallePedidos.Find(X => X.idDetalleCotizacion == idDetalleCotizacion);
            if (detalleV.estadoPedido != 0)
            {
                return;
            }

            dataGridView.Rows[index].ReadOnly = true;

            dataGridView.Rows[index].Cells["pc1"].ReadOnly = false;
            dataGridView.Rows[index].Cells["pc1"].Selected = true;
            dataGridView.Rows[index].Cells["pc1"].Style.SelectionBackColor = Color.FromArgb(255, 247, 178);
            dataGridView.Rows[index].Cells["pc1"].Style.SelectionForeColor = Color.Black;
            //dataGridView.Rows[index].Cells["cant1"].ReadOnly = false;
            //dataGridView.Rows[index].Cells["cant1"].Selected = true;
            //dataGridView.Rows[index].Cells["cant1"].Style.SelectionBackColor = Color.FromArgb(255, 247, 178);
            //dataGridView.Rows[index].Cells["cant1"].Style.SelectionForeColor = Color.Black;
            
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            executeNuevo();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            executeEliminar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            executeModificar();
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            executeAnular();
        }

        private void executeNuevo()
        {
            FormCotizacionaNew formCotizacionNuevo = new FormCotizacionaNew(true);
            formCotizacionNuevo.ShowDialog();
            cargarRegistros();
            formPrincipal.cargarDatosAsideRight();
        }

        private async void executeModificar()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            loadState(true);
            try
            {

                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idCotizacion = Convert.ToInt32(dataGridView.Rows[index].Cells["idCotizacion"].Value); // obteniedo el idCategoria del datagridview

                currentCotizacion = await cotizacionModel.cotizacion(idCotizacion); // Buscando la categoria en las lista de categorias

                // Mostrando el formulario de modificacion
                FormCotizacionaNew formCotizacionNuevo = new FormCotizacionaNew(currentCotizacion, true);
                formCotizacionNuevo.ShowDialog();
                cargarRegistros(); // recargando loas registros en el datagridview
                formPrincipal.cargarDatosAsideRight();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error "+ ex.Message, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            finally
            {
                loadState(false);
            }
           
        }


        private  async void executeModificarPedido()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "actualizar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            loadState(true);
            try
            {

                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idDetalleCotizacion = Convert.ToInt32(dataGridView.Rows[index].Cells["idDetalleCotizacion"].Value); // obteniedo el idCategoria del datagridview

                currentPedido = detallePedidos.Find(X=>X.idDetalleCotizacion== idDetalleCotizacion ); // Buscando la categoria en las lista de categorias
                int estadoSiguiente = currentPedido.estadoPedido+1;
                // Mostrando el formulario de modificacion
                DetalleV detalle = new DetalleV();
                detalle.idDetalleCotizacion = idDetalleCotizacion;
                detalle.estadoPedido = estadoSiguiente;
                Response response = await cotizacionModel.modificarPedido(detalle);
                if (response.id > 0)
                {
                    MessageBox.Show( response.msj +" correctamente el estado del pedido", "Actualizar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnReservada.Text = estadoPedidos[estadoSiguiente];
                    btnRealizada.Text = estadoPedidos[estadoSiguiente + 1];
                    if (estadoSiguiente == 6)
                    {

                        btnRealizada.Enabled = false;
                    }
                    cargarRegistros();
                }
                else
                {
                    MessageBox.Show("Error " + response.msj, "Actualizar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "ctualizar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            finally
            {
                loadState(false);
            }


        }

       
        private async void executeAnular()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Desactivar o anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Pregunta de seguridad de anular
            DialogResult dialog = MessageBox.Show("¿Está seguro de anular este registro?", "Anular",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialog == DialogResult.No) return;

            try
            {
                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentCotizacion = new Cotizacion(); //creando una instancia del objeto categoria
                currentCotizacion.idCotizacion = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                // Comprobando si la categoria ya esta desactivado
                //if (cotizaciones.Find(x => x.idCotizacion == currentCotizacion.idCotizacion).estado == 0)
                //{
                //    MessageBox.Show("Este registro ya esta desactivado", "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}

                // Procediendo con las desactivacion
                Response response = await cotizacionModel.desactivar(currentCotizacion);
                MessageBox.Show(response.msj, "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarRegistros(); // recargando los registros en el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
       

        private void btnReservada_Click(object sender, EventArgs e)
        {

        }

        private void btnRealizada_Click(object sender, EventArgs e)
        {
            executeModificarPedido();
        }

        private void txtfiltrado_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paginacion.currentPage = 1;
             
                cargarRegistros();
                          
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
//            loadState(true);
            try
            {
                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idDetalleCotizacion = Convert.ToInt32(dataGridView.Rows[index].Cells["idDetalleCotizacion"].Value); // obteniedo el idCategoria del datagridview

                currentPedido = detallePedidos.Find(X => X.idDetalleCotizacion == idDetalleCotizacion); // Buscando la categoria en las lista de categorias

                // Mostrando el formulario de modificacion
                btnReservada.Text = " Estado: " +  currentPedido.StatusEnvio;// estado actual
                int estadoPedido = currentPedido.estadoPedido;
                if (estadoPedido >= 4)
                {
                    plNext.Visible = true;
                    btnRealizada.Visible = true;
                    if (estadoPedido > 6) 
                    {
                        btnRealizada.Enabled = false;
                        btnRealizada.Text =estadoPedidos[++estadoPedido];
                    }
                    else
                    {

                        btnRealizada.Enabled = true;
                        btnRealizada.Text = estadoPedidos[++estadoPedido];

                    }


                }
                else {
                    plNext.Visible = false;
                    btnRealizada.Visible = false;

                }//

                //mostrar las opcion que se puede hacer en btnEliminar
                int estado = currentPedido.estado;
                if (estado == 1)
                {
                    btnEliminar.Text = "Desactivar o Eliminar";

                }
                else
                {
                    if(estado==9)
                        btnEliminar.Text = "Eliminar";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            finally
            {
                //loadState(false);
            }
           
        }

        private void btnProveedor1_Click(object sender, EventArgs e)
        {
            int nro = listProveedoresEscogidos.Count;
            if (nro == 3)
            {
                seleccionarPedidos();
                if (pedidoSeleccionados.Count == 0)
                {

                    MessageBox.Show("ningun pedido Seleccionado", "Seleccionar Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Proveedor proveedor = listProveedoresEscogidos.Find(X => X.escogido == 1);
                FormOrdenPedido formOrden = new FormOrdenPedido(pedidoSeleccionados, proveedor);
                formOrden.ShowDialog();
            }
            cargarRegistros();

        }
        private  void seleccionarPedidos()
        {
            pedidoSeleccionados.Clear();
            idProveedores.Clear();
            int idMoneda = 0;
            if (dataGridView.SelectedRows.Count > 0)
            {

                List<int> idProveedores = new List<int>();
                int idDetalle = (int) dataGridView.SelectedRows[0].Cells["idDetalleCotizacion"].Value;
                DetalleV detallefirst = detallePedidos.Find(X => X.idDetalleCotizacion == idDetalle);
                idMoneda = detallefirst.idMoneda;
            }
            
            foreach (DataGridViewRow dr in dataGridView.SelectedRows)
            {
                int idDetalleCotizacion = (int)dr.Cells["idDetalleCotizacion"].Value;

                DetalleV detalleV = detallePedidos.Find(X => X.idDetalleCotizacion == idDetalleCotizacion);
                idProveedores.Add(detalleV.idP1);
                idProveedores.Add(detalleV.idP2);
                idProveedores.Add(detalleV.idP3);

                if (detalleV.estadoPedido == 0 && detalleV.idMoneda==idMoneda)
                {
                    pedidoSeleccionados.Add(detalleV);
                }              
            }
            this.idProveedores = idProveedores.Distinct().ToList();
        }
        private void seleccionarPedidosOrden()
        {
            pedidoSeleccionados.Clear();
            foreach (DetalleV dr in detallePedidos)
            {             
                if (dr.estadoPedido == 0)
                {
                    pedidoSeleccionados.Add(dr);
                }
            }
        }

        private void btnEscogerProveedorw_Click(object sender, EventArgs e)
        {
            seleccionarPedidos();
            FormSeleccion form = null;
            if (pedidoSeleccionados.Count == 0)
            {
                MessageBox.Show("Ningún pedido seleccionado tiene estado \"Inciado\"", "Escoger Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
                form = new FormSeleccion();
                form.ShowDialog();
                if(form.salir)
                    cargarRegistros();
                return;
            }
           
            form = new FormSeleccion(pedidoSeleccionados, this.idProveedores );
            form.ShowDialog();
            if (form.guardar)
            {
                cargarRegistros();
                cargarProveedores();
            }

            ///// esto sera modificado
            //listProveedoresEscogidos = form.listProveedoresEscogidos;
            //cargarProveedoresSeleccionados();

        }

        private void btnProveedor2_Click(object sender, EventArgs e)
        {
            int nro = listProveedoresEscogidos.Count;
            

                Proveedor proveedor = listProveedoresEscogidos.Find(X => X.idProveedor ==(int) cbxProveedores.SelectedValue);
                FormOrdenPedido formOrden = new FormOrdenPedido(pedidoSeleccionados, proveedor);
                formOrden.ShowDialog();
         
            
            cargarRegistros();
        }

        private void btnProveedor3_Click(object sender, EventArgs e)
        {
            if ((int)cbxProveedores.SelectedValue==0)
            {
                MessageBox.Show("Debe seleccionar un proveedor", "Crear Orden de Compra", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbxProveedores.Focus();
                return;
            }
            seleccionarPedidos();
            if (pedidoSeleccionados.Count == 0)
            {
                seleccionarPedidosOrden();
            }   
            if (pedidoSeleccionados.Count == 0)
            {
                MessageBox.Show("Ningun pedido seleccionado tiene estado \"Iniciado\"", "Crear Orden de Compra", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Proveedor proveedor = listProveedoresEscogidos.Find(X => X.idProveedor == (int)cbxProveedores.SelectedValue);
            FormOrdenPedido formOrden = new FormOrdenPedido(pedidoSeleccionados, proveedor);
            formOrden.ShowDialog();
            cargarRegistros();         
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnEntregado_Click(object sender, EventArgs e)
        {
            if (!entregado)
            {
                Image image = Resources.comprobadoblue;
                btnEntregado.Image = image;
                entregado = true;


            }
            else
            {

                Image image = Resources.verificarme;
                btnEntregado.Image = image;
                entregado = false;
            
            }
            cargarRegistros();
        }

        private void btnIniciado_Click(object sender, EventArgs e)
        {
            if (!iniciado)
            {
                Image image = Resources.comprobadoblue;
                btnIniciado.Image = image;
                iniciado = true;
                //153; 150; 151
                //86; 92; 86
            }
            else
            {
                Image image = Resources.verificarme;
                btnIniciado.Image = image;
                iniciado = false; 
            }
            cargarRegistros();


        }

        private void btnEnOrden_Click(object sender, EventArgs e)
        {
            if (!enOrden)
            {
                Image image = Resources.comprobadoblue;
                btnEnOrden.Image = image;
                enOrden = true;
                //5; 50; 160
              

            }
            else
            {

                Image image = Resources.verificarme;
                btnEnOrden.Image = image;
                enOrden = false;
               
            }
            cargarRegistros();
        }

        private void btnCamino_Click(object sender, EventArgs e)
        {
            if (!enCamino)
            {
                Image image = Resources.comprobadoblue;
                btnCamino.Image = image;
                enCamino = true;
              

            }
            else
            {

                Image image = Resources.verificarme;
                btnCamino.Image = image;
                enCamino = false;
              
            }
            cargarRegistros();
        }

        private void btnArribado_Click(object sender, EventArgs e)
        {
            if (!arribado)
            {
                Image image = Resources.comprobadoblue;
                btnArribado.Image = image;
                arribado = true;
                //209; 43; 33
                //161; 31; 25
              

            }
            else
            {

                Image image = Resources.verificarme;
                btnArribado.Image = image;
                arribado = false;
                
            }
            cargarRegistros();
        }

        private void btnEsperando_Click(object sender, EventArgs e)
        {
            if (!esperando)
            {
                Image image = Resources.comprobadoblue;
                btnEsperando.Image = image;
                esperando = true;
                //224; 108; 51
                //161; 78; 37
               

            }
            else
            {

                Image image = Resources.verificarme;
                btnEsperando.Image = image;
                esperando = false;
              
            }
            cargarRegistros();
        }
        private void btnInstalando_Click(object sender, EventArgs e)
        {
            if (!instalando)
            {
                Image image = Resources.comprobadoblue;
                btnInstalando.Image = image;
                instalando = true;
                //224; 108; 51
                //161; 78; 37


            }
            else
            {

                Image image = Resources.verificarme;
                btnInstalando.Image = image;
                instalando = false;

            }
            cargarRegistros();
        }
        private void btnInstaldo_Click(object sender, EventArgs e)
        {
            if (!instalado)
            {
                Image image = Resources.comprobadoblue;
                btnInstaldo.Image = image;
                instalado = true;
            }
            else
            {

                Image image = Resources.verificarme;
                btnInstaldo.Image = image;
                instalado = false;
             
            }
            cargarRegistros();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            executeEliminar();
        }

        private async void executeEliminar()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }                         
            try
            {
                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentCotizacion = new Cotizacion(); //creando una instancia del objeto categoria
                int idCotizacion = Convert.ToInt32(dataGridView.Rows[index].Cells["idCotizacion"].Value); // obteniedo el idCategoria del datagridvie
                currentPedido = detallePedidos.Find(X => X.idCotizacion == idCotizacion);
                List<DetalleV> detalle = detallePedidos.Where(X => X.idCotizacion == idCotizacion).ToList();
                // ver si alguno esta en estado}
                bool hayEstados = false;// PARA SI HAY ESTADOS INSTALADO Y ENTREGADO
                foreach (DetalleV dar in detalle)
                {
                    if (dar.estadoPedido >= 7)
                    {
                        hayEstados = true;

                    }
                }

                if (hayEstados)
                {
                    MessageBox.Show("No se puede eliminar porque exite pedidos ya entregados!! ", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                loadState(true); // cambiando el estadom 
                // ver si caja esta iniciado y si hay dinero suficiente para devolucion
               



                if (currentPedido.estado == 1)
                {
                    // Pregunta de seguridad de eliminacion
                    DialogResult dialog = MessageBox.Show("¿Está seguro de eliminar este registro?", "Eliminar",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialog == DialogResult.No)
                    {

                        DialogResult dialogDesactivar = MessageBox.Show("¿Está seguro de desactivar este registro?", "Desactivar",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dialogDesactivar == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {

                            Response response = await cotizacionModel.desactivarPedido(currentPedido);// cambiar estado a 9

                        }


                    }
                    else
                    {
                        // ver si caja esta abierta y hay dinero suficiente
                        bool cumple = false;
                        if (ConfigModel.cajaIniciada && ConfigModel.cajaSesion != null)
                        {
                            foreach (Entidad.Configuracion.Moneda M in await cajaModel.cierreCajaIngresoMenosEgreso(1, ConfigModel.cajaSesion.idCajaSesion))
                            {
                                // ver si hay suficiente dinero
                                double adelanto = 0;
                                list = await cotizacionModel.Adelanto(currentPedido.idCotizacion);
                                if (list.Count > 0)
                                {

                                    adelanto = toDouble(list[0].adelantos);
                                }
                                if (currentPedido.idMoneda == M.idMoneda)
                                {
                                    if (adelanto <= M.total)
                                    {
                                        cumple = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se inicio la caja", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }



                        if (cumple)
                        {
                            string ade = "0";
                            try
                            {

                                list = await cotizacionModel.Adelanto(currentPedido.idCotizacion);
                                if (list.Count > 0)
                                {

                                    ade = list[0].adelantos;
                                }


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message, "cargar adelanto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            }



                            Response response = await cotizacionModel.eliminarPedido(currentPedido); // Eliminando con el webservice correspondiente
                                                                                                       // realizar el egreso
                            currentSaveObject = new SaveObjectEgreso();
                            //currentEgreso = new Egreso();
                            // Llenar el id categoria cuando este en esdo modificar
                            currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
                            currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
                            currentSaveObject.idMedioPago = 1;// efectivo el unico modo 
                            currentSaveObject.idMoneda = Convert.ToInt32(currentPedido.idMoneda);
                            currentSaveObject.medioPago = "EFECTIVO";
                            currentSaveObject.moneda = currentPedido.moneda;


                            currentSaveObject.monto = ade;//  monto de devolucion
                            currentSaveObject.motivo = "DEVOLUCION  DE DINERO";
                            currentSaveObject.observacion = "Devolucion de dinero por el pedido " + currentPedido.serie + "-" + currentPedido.correlativo;
                            string nroOperacion = "";
                            try
                            {

                                CorrelativoData response3 = await cajaModel.correlativoSerie<CorrelativoData>(ConfigModel.asignacionPersonal.idCaja, 0);
                                nroOperacion = String.Format("{0} - {1}", response3.serie, response3.correlativoActual);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message, "nro Operacion egreso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            }
                            currentSaveObject.numeroOperacion = nroOperacion;
                            DateTime thisDay = DateTime.Today;
                            currentSaveObject.fechaPago = thisDay.ToString("yyyy-MM-dd HH':'mm':'ss");
                            if (toDouble(ade) > 0)
                            {
                                Response response2 = await egresoModel.guardar(currentSaveObject);
                            }

                            MessageBox.Show(response.msj + "  correctamente", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {

                            MessageBox.Show("No exite suficiente dinero en caja", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;

                        }

                    }


                }
                else
                {

                    if (currentPedido.estado == 9)
                    {

                        // Pregunta de seguridad de eliminacion
                        DialogResult dialog = MessageBox.Show("¿Está seguro de eliminar este registro?", "Eliminar",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {

                            // ver si caja esta abierta y hay dinero suficiente
                            bool cumple = false;
                            if (ConfigModel.cajaIniciada && ConfigModel.cajaSesion != null)
                            {
                                foreach (Entidad.Configuracion.Moneda M in await cajaModel.cierreCajaIngresoMenosEgreso(1, ConfigModel.cajaSesion.idCajaSesion))
                                {
                                    // ver si hay suficiente dinero
                                    double adelanto = 0;
                                    list = await cotizacionModel.Adelanto(currentPedido.idCotizacion);
                                    if (list.Count > 0)
                                    {

                                        adelanto = toDouble(list[0].adelantos);
                                    }
                                    if (currentPedido.idMoneda == M.idMoneda)
                                    {
                                        if (adelanto <= M.total)
                                        {
                                            cumple = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se inicio la caja", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }



                            if (cumple)
                            {
                                string ade = "0";
                                try
                                {

                                    list = await cotizacionModel.Adelanto(currentPedido.idCotizacion);
                                    if (list.Count > 0)
                                    {

                                        ade = list[0].adelantos;
                                    }


                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR: " + ex.Message, "cargar adelanto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                }



                                Response response = await cotizacionModel.eliminarPedido(currentPedido); // Eliminando con el webservice correspondiente
                                                                                                         // realizar el egreso
                                currentSaveObject = new SaveObjectEgreso();
                                //currentEgreso = new Egreso();
                                // Llenar el id categoria cuando este en esdo modificar
                                currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
                                currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
                                currentSaveObject.idMedioPago = 1;// efectivo el unico modo 
                                currentSaveObject.idMoneda = Convert.ToInt32(currentPedido.idMoneda);
                                currentSaveObject.medioPago = "EFECTIVO";
                                currentSaveObject.moneda = currentPedido.moneda;


                                currentSaveObject.monto = ade;//  monto de devolucion
                                currentSaveObject.motivo = "DEVOLUCION  DE DINERO";
                                currentSaveObject.observacion = "Devolucion de dinero por el pedido " + currentPedido.serie + "-" + currentPedido.correlativo;
                                string nroOperacion = "";
                                try
                                {

                                    CorrelativoData response3 = await cajaModel.correlativoSerie<CorrelativoData>(ConfigModel.asignacionPersonal.idCaja, 0);
                                    nroOperacion = String.Format("{0} - {1}", response3.serie, response3.correlativoActual);

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR: " + ex.Message, "nro Operacion egreso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                }
                                currentSaveObject.numeroOperacion = nroOperacion;
                                DateTime thisDay = DateTime.Today;
                                currentSaveObject.fechaPago = thisDay.ToString("yyyy-MM-dd HH':'mm':'ss");
                                if (toDouble(ade) > 0)
                                {
                                    Response response2 = await egresoModel.guardar(currentSaveObject);
                                }

                                MessageBox.Show(response.msj + "  correctamente", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            else
                            {

                                MessageBox.Show("No exite suficiente dinero en caja", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;

                            }
                        }


                    }
                }

                
                cargarRegistros(); // recargando el datagridview
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

        struct CorrelativoData
        {
            public string correlativoActual { get; set; }
            public string serie { get; set; }
        }
        private  async void crearObjetoEgreso()
        {
            currentSaveObject = new SaveObjectEgreso();
            //currentEgreso = new Egreso();
            // Llenar el id categoria cuando este en esdo modificar
            currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
            currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
            currentSaveObject.idMedioPago = 1;// efectivo el unico modo 
            currentSaveObject.idMoneda = Convert.ToInt32(currentPedido.idMoneda);
            currentSaveObject.medioPago ="EFECTIVO";
            currentSaveObject.moneda = currentPedido.moneda;
            string ade = "0";
            try
            {

                list = await cotizacionModel.Adelanto(currentCotizacion.idCotizacion);
                if (list.Count > 0)
                {

                    ade = list[0].adelantos;
                }
                

            }    
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "cargar adelanto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            
            currentSaveObject.monto = ade;//  monto de devolucion
            currentSaveObject.motivo ="DEVOLUCION  DE DINERO";
            currentSaveObject.observacion = "Devolucion de dinero por el pedido " + currentPedido.serie+"-"+currentPedido.correlativo;
            string nroOperacion = "";
            try
            {

                CorrelativoData response = await cajaModel.correlativoSerie<CorrelativoData>(ConfigModel.asignacionPersonal.idCaja, 0);
                nroOperacion = String.Format("{0} - {1}", response.serie, response.correlativoActual);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: "+ ex.Message, "nro Operacion egreso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }            
            currentSaveObject.numeroOperacion = nroOperacion;
            DateTime thisDay = DateTime.Today;
            currentSaveObject.fechaPago = thisDay.ToString("yyyy-MM-dd HH':'mm':'ss");
        }

        private void cbxProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxProveedores.SelectedIndex == -1) return;
            if ((int)cbxProveedores.SelectedValue == 0)
            {
                //btnRealizarOrdenCompra.Text ="Seleccione un proveedor para hacer la orden de pedido.";
            }
            else
            {
                Proveedor proveedor = listProveedorescombo.Find(X => X.idProveedor == (int)cbxProveedores.SelectedValue);
                //btnRealizarOrdenCompra.Text = proveedor.razonSocial;
            }
            cargarRegistros();
        }

        private async void cbxSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbxSucursales.SelectedIndex == -1) return;
            loadState(true);
            try
            {
                //Si el personal logeado es gerente o administrador debe listar a todos los personales
                if (ConfigModel.asignacionPersonal.idPuntoGerencia != 0 || ConfigModel.asignacionPersonal.idPuntoAdministracion != 0)
                {
                    //personalBindingSource.DataSource = await personalModel.listarPersonalAlmacen(ConfigModel.sucursal.idSucursal);
                    personalBindingSource.DataSource = await personalModel.listarPersonalVenta((int)cbxSucursales.SelectedValue);
                }
                else
                {
                    List<Personal> listaPersonal = new List<Personal>();
                    Personal personal = new Personal();
                    personal.idPersonal = PersonalModel.personal.idPersonal;
                    personal.nombres = PersonalModel.personal.nombres;
                    listaPersonal.Add(personal);
                    personalBindingSource.DataSource = listaPersonal;
                    cbxPersonales.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                cargarRegistros();
                loadState(false);
            }
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dataGridView.Sort(dataGridView.Columns[e.ColumnIndex], ListSortDirection.Ascending);

            
                dataGridView.Sort(new RowComparer(SortOrder.Ascending));
           
           
        }
        private bool click = false;
        private void dataGridView_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            // columnSort.Add(name, false);
            //correlativo
            string name =  dataGridView.Columns[e.ColumnIndex].Name;

            switch (name){

                case "serie":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.serie).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.serie).ToList();

                    }
                    break;
                case "correlativo":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.correlativo).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.correlativo).ToList();

                    }
                    break;
                case "descripcion":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.descripcion).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.descripcion).ToList();

                    }
                    break;
                case "cantidad":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.cantidad).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.cantidad).ToList();

                    }
                    break;
                case "nombreCliente":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.nombreCliente).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.nombreCliente).ToList();

                    }
                    break;
                case "pc":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.pc).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.pc).ToList();

                    }
                    break;
                case "pc1":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.pc1).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.pc1).ToList();
                    }
                    break;
                case "pc2":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.pc2).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.pc2).ToList();

                    }
                    break;
                case "pc3":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.pc3).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.pc3).ToList();

                    }
                    break;
                case "cant":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.cant).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.cant).ToList();

                    }
                    break;
                case "cant1":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.cant1).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.cant1).ToList();

                    }
                    break;
                case "cant2":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.cant2).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.cant2).ToList();

                    }
                    break;
                case "cant3":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.cant3).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.cant3).ToList();

                    }
                    break;
                case "precioVenta":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = detallePedidos.OrderBy(s => s.precioVenta).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = detallePedidos.OrderByDescending(s => s.precioVenta).ToList();

                    }
                    break;
                    //descripcion

            }

            decorationDataGridView();
        }        

        private void txtfiltrado_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView.Rows.Count == 0 || dataGridView.CurrentRow == null)
                {
                    MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview

                int idDetallePedido = Convert.ToInt32(dataGridView.Rows[index].Cells["idDetalleCotizacion"].Value);
               
                DetalleV detalle = detallePedidos.Find(X => X.idDetalleCotizacion == idDetallePedido );

              

                    Forminfo detalleStock = new Forminfo(detalle);
                    detalleStock.ShowDialog();
                formPrincipal.cargarDatosAsideRight();

            }
        }

        private void cbxPersonales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPersonales.SelectedIndex == -1) return;
            cargarRegistros();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Modificar registros
            modificarPedidosVarios();
        }

        private async void modificarPedidosVarios()
        {
            loadState(true);
            try
            {
                List<DetalleV> listaPedidos = new List<DetalleV>();
                //Recorrer los registro y guardados
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    DetalleV pedidoAgregado = new DetalleV();
                    pedidoAgregado.idDetalleCotizacion = int.Parse(fila.Cells["idDetalleCotizacion"].Value.ToString());
                    pedidoAgregado.cant1 = double.Parse(fila.Cells["cant1"].Value.ToString());
                    pedidoAgregado.pc1 = double.Parse(fila.Cells["pc1"].Value.ToString());
                    listaPedidos.Add(pedidoAgregado);
                }
                //Enviar los detalles de cotizacion 
                Response respuesta = await cotizacionModel.actualizarPedidoVarios(listaPedidos);
                MessageBox.Show(respuesta.msj, "Actualiar Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Error: " + ex.Message, "Actualizar Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
                cargarRegistros();
            }
            
        }

        private void btnRealizarCompra_Click(object sender, EventArgs e)
        {
            if ((int)cbxProveedores.SelectedValue == 0)
            {
                MessageBox.Show("Ningun proveedor seleccionado, seleccione uno", "Realizar Compra", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbxProveedores.Focus();
                return;
            }
            seleccionarPedidos();
            if (pedidoSeleccionados.Count == 0)
            {
                seleccionarPedidosOrden();
            }
            if (pedidoSeleccionados.Count == 0)
            {
                MessageBox.Show("No hay pedidos con estado \"Iniciado\"", "Realizar Compra", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            Proveedor proveedor = listProveedoresEscogidos.Find(X => X.idProveedor == (int)cbxProveedores.SelectedValue);
            FormCompraPedido formCompraPedido = new FormCompraPedido(pedidoSeleccionados,proveedor);
            formCompraPedido.ShowDialog();
            cargarRegistros();
        }
    }



    class SaveObjectEgreso
    {
        public int idEgreso { get; set; }
        public string numeroOperacion { get; set; }
        public string fecha { get; set; }
        public string fechaPago { get; set; }
        public string monto { get; set; }
        public string motivo { get; set; }
        public string observacion { get; set; }
        public string moneda { get; set; }
        public int estado { get; set; }
        public int idMoneda { get; set; }
        public int idCaja { get; set; }
        public int idCajaSesion { get; set; }
        public int idDetallePago { get; set; }
        public int idMedioPago { get; set; }
        public string medioPago { get; set; }
        public string personal { get; set; }
        public string esDeCompra { get; set; }
    }
}
