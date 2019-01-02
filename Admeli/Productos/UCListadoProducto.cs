﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Admeli.Productos.Nuevo;
using Modelo;
using Entidad;
using Newtonsoft.Json;
using Admeli.Productos.Importar;
using Entidad.Configuracion;
using System.Globalization;
using Admeli.Productos.Detalle;
using Admeli.Properties;
using System.IO;
using System.Reflection;

namespace Admeli.Productos
{
    
    public partial class UCListadoProducto : UserControl
    {

        private ProductoModel productoModel = new ProductoModel();
        private CategoriaModel categoriaModel = new CategoriaModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private AlmacenModel almacenModel = new AlmacenModel();
        private LocationModel locationModel = new LocationModel();
        private MonedaModel monedaModel = new MonedaModel();
        private PuntoModel puntoModel = new PuntoModel();

       
        private PuntoAdministracion puntoAdministracion { get; set; }
        private PuntoCompra puntoCompra { get; set; }
        private List<PuntoDeVenta> puntosDeVenta { get; set; }
        private List<Caja> cajas { get; set; }
        private PuntoGerencia puntoGerencia { get; set; }




        private Paginacion paginacion;
        private FormPrincipal formPrincipal;

        public bool lisenerKeyEvents { get; set; }
        private List<CombinacionStock> combinaciones { get; set; }
        private List<CombinacionStock> combinacionesProducto { get; set; }
        private List<Producto> productos { get; set; }
        private List<Sucursal> listSuc { get; set; }
        private List<Sucursal> listSucCargar { get; set; }
        private List<Almacen> listAlm { get; set; }
        private List<Almacen> listAlmCargar { get; set; }

        private List<Moneda> listMoneda { get; set; }

        private Moneda monedaActual { get; set; }
        private Producto currentProducto { get; set; }
        private decimal valorDeCambio = 1;
        private int index = 0;
        private Assembly _assembly;
        private StreamReader _textStreamReader;
        private bool hayConfiguracion = false;
        private int contador = 0;
        Moneda monedaConfiguracion { get; set; }
        private Dictionary<string, bool> columnSort = new Dictionary<string, bool>();

        #region ============================== Constructor ==============================
        public UCListadoProducto()
        {
            InitializeComponent();

            lblSpeedPages.Text = ConfigModel.configuracionGeneral.itemPorPagina.ToString();     // carganto los items por página
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));

            visualizar();
            if (ConfigModel.asignacionPersonal.idPuntoCompra == 0)
            {
                btnModificar.Enabled = false;
            }

        }

        public UCListadoProducto(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            lblSpeedPages.Text = ConfigModel.configuracionGeneral.itemPorPagina.ToString();     // carganto los items por página
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));
            visualizar();
            if (!formPrincipal.hayAlmacen)
            {
                panelCrud.Visible = false;
                flowLayoutPanel1.Visible = false;
                button1.Visible = false;
                button4.Visible = false;
            }
            InicializarColumnasSort();
        
        }

        private void InicializarColumnasSort()
        {
            foreach (DataGridViewColumn columns in dataGridView.Columns)
            {
                string name = columns.Name;
                columnSort.Add(name, false);
            }
        }

        private void visualizar()
        {
            loadPuntoCompra();
            loadPuntoVenta();
        }
        private double toDouble(string texto)
        {

            if (texto == "")
            {
                return 0;
            }
            return double.Parse(texto, CultureInfo.GetCultureInfo("en-US")); ;
        }

        private async void loadPuntoCompra()
        {

            AsignacionPersonal asignacionPersonal = ConfigModel.asignacionPersonal;
            puntoCompra = await puntoModel.puntoCompra(ConfigModel.sucursal.idSucursal);
            if (asignacionPersonal.idPuntoCompra == 0)
            {
                dataGridView.Columns["precioCompra"].Visible = false;


            }

        }
        private async void loadPuntoVenta()
        {
            puntosDeVenta = await puntoModel.puntoVentas(ConfigModel.sucursal.idSucursal);
            if (ConfigModel.asignacionPersonal.idPuntoVenta == 0)
            {

                dataGridView.Columns["precioVenta"].Visible = false;
            }

        }
        #endregion

        #region ============================== Root Load ==============================
        private void UCListadoProducto_Load(object sender, EventArgs e)
        {

            try
            {
                
                if(File.Exists("configuracion.txt"))
                {

                    FileStream stream = new FileStream("configuracion.txt", FileMode.Open, FileAccess.Read);
                    _textStreamReader = new StreamReader(stream);
                    string sLine = "";
                    List<string> arrText = new List<string>();

                    while (sLine != null)
                    {
                        sLine = _textStreamReader.ReadLine();
                        if (sLine != null)
                            arrText.Add(sLine);
                    }
                    
                    _textStreamReader.Close();
                    string monedaS = "";
                    foreach(string dato in arrText)
                    {
                        monedaS += dato;

                    }
                    monedaConfiguracion = JsonConvert.DeserializeObject<Moneda>(monedaS);
                    if (monedaConfiguracion != null)
                    {
                        hayConfiguracion = true;
                        
                    }
                    else
                    {
                        hayConfiguracion = false;

                    }

                }
                else
                {
                    hayConfiguracion = false;
                }

                //Stream idPre = _assembly.GetManifestResourceStream("Admeli.configuracion.txt");

               
               






            }
            catch(Exception ex )
            {
                MessageBox.Show("Error: " + ex.Message, "cargar load", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           


            this.darFormatoDecimales();
            this.reLoad();

            // Preparando para los eventos de teclado
            this.ParentChanged += ParentChange; // Evetno que se dispara cuando el padre cambia // Este eveto se usa para desactivar lisener key events de este modulo
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }
        }

        private void darFormatoDecimales()
        {
            dataGridView.Columns["precioCompra"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridView.Columns["precioVenta"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridView.Columns["stock"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dataGridView.Columns["precioCompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns["precioVenta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;            
        }

        internal void reLoad(bool refreshData = true)
        {
            cargarValorCambio();
            if (refreshData)

            {
                refresh();
                cargarCategorias();
               

            }
            lisenerKeyEvents = true; // Active lisener key events
        }


        internal void refresh(bool refreshData = true)
        {

            cargarValorCambio();
            if (refreshData)
            {

                cargarMonedas();
                cargarSucursales();
                cargarAlmacenes();
                cargarStock();

            }
            lisenerKeyEvents = true; // Active lisener key events
           
        }
        
        #endregion

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelContainer);
            drawShape.lineBorder(panel7);
            drawShape.lineBorder(panelsucursal);
            drawShape.lineBorder(panelAlmacen);
            drawShape.lineBorder(panelMoneda);

        }

        #region ======================== KEYBOARD ========================
        // Evento que se dispara cuando el padre cambia
        private void ParentChange(object sender, EventArgs e)
        {
           

            if (lisenerKeyEvents) {

                try
                {

                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter("configuracion.txt");
                    string monedaS = JsonConvert.SerializeObject(monedaActual); 
                    //Write a line of text
                    sw.WriteLine(monedaS);               
                    //Close the file
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }

                
            }
            else
            {


            }
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
                default:
                    break;
            }
        }
        #endregion

        #region =========================== Decoration ===========================
        private void decorationDataGridView()
        {
            if (dataGridView.Rows.Count == 0) return;

            try
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                 {
                int idProducto = Convert.ToInt32(row.Cells["idProducto"].Value); // obteniedo el idCategoria del datagridview
                int idAlmacen = Convert.ToInt32(row.Cells["idAlmacen"].Value);
                //Prodcutos con combinacion
                combinacionesProducto = combinaciones.Where(X => X.idPresentacion== idProducto && X.idAlmacen == idAlmacen).ToList();
                if (combinacionesProducto.Count > 0)
                {
                    dataGridView.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(122, 255, 105);
                    row.DefaultCellStyle.ForeColor = Color.Green;
                }
                //Productos desactivados
                currentProducto = productos.Find(x => x.idProducto == idProducto);
                if (currentProducto.estado == false)
                {
                    dataGridView.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
                if (currentProducto.categorias == 0)
                {

                     dataGridView.ClearSelection();
                     row.DefaultCellStyle.BackColor = Color.FromArgb(252, 246, 243);
                     row.DefaultCellStyle.ForeColor = Color.FromArgb(206, 64, 54);

                 }

            }


            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Decoracion", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }
        #endregion

        #region ======================= Loads =======================

        private async void cargarMonedas()
        {
            loadState(true);
            try
            {

                listMoneda = await monedaModel.monedas();
                monedaBindingSource.DataSource = listMoneda;
                //monedas/estado/1
                if (monedaActual == null)
                {

                    monedaActual = listMoneda.Find(X => X.porDefecto == true);
                    cbxMoneda.SelectedValue = monedaActual.idMoneda;
                    if (hayConfiguracion)
                    {
                        cbxMoneda.SelectedValue = monedaConfiguracion.idMoneda;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void cargarSucursales()
        {
            // Cargando el combobox de personales
            loadState(true);
            try
            {
               
                listSucCargar = new List<Sucursal>();
                listSuc = ConfigModel.listSucursales;
                Sucursal sucursal = new Sucursal();
                sucursal.idSucursal = 0;
                sucursal.nombre = "Todas";
                listSucCargar.Add(sucursal);
                listSucCargar.AddRange(listSuc);
                sucursalBindingSource.DataSource = listSucCargar;
                cbxSucursales.SelectedValue = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar sucursal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarAlmacenes()
        {
            // Cargando el combobox de personales
            loadState(true);
            try
            {
                listAlm = new List<Almacen>();
                listAlmCargar = new List<Almacen>();
                listAlm =  await almacenModel.almacenesAsignados(0, PersonalModel.personal.idPersonal);//  para todos las asignadas al personal
                
                Almacen almacen = new Almacen();
                almacen.idAlmacen = 0;
                almacen.nombre = "Todas";
                listAlmCargar.Add(almacen);
                listAlmCargar.AddRange(listAlm);
                almacenBindingSource.DataSource = listAlmCargar;
               
                cbxSucursales.SelectedIndex = -1;
                cbxSucursales.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarCategorias()
        {
            loadState(true);
            try
            {
                // Cargando las categorias desde el webservice
                List<Categoria> categoriaList = await categoriaModel.categoriasTodo();
                Categoria lastCategori = categoriaList.Last();
                categoriaList.Remove(lastCategori);

                // Cargando
                treeViewCategoria.Nodes.Clear(); // limpiando
                treeViewCategoria.Nodes.Add(lastCategori.idCategoria.ToString(), lastCategori.nombreCategoria); // Cargando categoria raiz

                treeViewCategoria.Nodes[0].Checked = true;
                List<TreeNode> listNode = new List<TreeNode>();

                foreach (Categoria categoria in categoriaList)
                {
                    TreeNode aux = new TreeNode(categoria.nombreCategoria);
                    aux.Name = categoria.idCategoria.ToString();
                    listNode.Add(aux);

                }
                treeviewVista(categoriaList, treeViewCategoria.Nodes[0], listNode);



                // Estableciendo valores por defecto
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadState(false);
        }

        #region ======================= metodos para treeview =======================
        private Categoria primerElemento(List<Categoria> categoriaList)
        {

            Categoria lastCategori = categoriaList[0];//raiz
            categoriaList.Remove(lastCategori);
            return lastCategori;
        }
        private bool hijos(List<Categoria> categoriaList, Categoria padre)
        {
            bool tieneHijos = false;

            foreach (Categoria categoria in categoriaList)
            {
                if (categoria.padre == padre.nombreCategoria)
                {
                    tieneHijos = true;
                    break;

                }

            }
            return tieneHijos;
        }
        private TreeNode buscar(Categoria categoria, List<TreeNode> listNode)
        {
            TreeNode aux = new TreeNode();

            foreach (TreeNode node in listNode)
            {
                if (categoria.nombreCategoria == node.Text)
                {

                    aux = node;
                    break;
                }

            }

            return aux;

        }
        private TreeNode buscarPadre(Categoria categoria, List<TreeNode> listNode)
        {
            TreeNode aux = new TreeNode();

            foreach (TreeNode node in listNode)
            {
                if (categoria.padre == node.Text)
                {

                    aux = node;
                    break;
                }

            }

            return aux;

        }
        private void treeviewVista(List<Categoria> categoriaList, TreeNode padreNode, List<TreeNode> listNode)
        {
            if (categoriaList.Count == 0)
            {
                return;

            }
            else
            {
                Categoria aux = primerElemento(categoriaList);
                TreeNode auxTreeNode = buscar(aux, listNode);
                TreeNode nodePadre = buscarPadre(aux, listNode);
                if (hijos(categoriaList, aux))
                {
                    auxTreeNode.ImageIndex = 1;
                }

                if (nodePadre.Text != "")
                {

                    nodePadre.Nodes.Add(auxTreeNode);
                    treeviewVista(categoriaList, padreNode, listNode);
                }
                else
                {

                    padreNode.Nodes.Add(auxTreeNode);

                    treeviewVista(categoriaList, padreNode, listNode);

                }



            }



        }
        #endregion =============================

        private async void cargarRegistros()
        {
            loadState(true);
            try
            {
                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);
                Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;

                //RootObject<Producto> productosRoot = await productoModel.productosPorCategoria(sendList, paginacion.currentPage, paginacion.speed);
                RootObject<Producto,CombinacionStock> productos_combinacion = await productoModel.productosPorCategoria(sendList, paginacion.currentPage, paginacion.speed);

                // actualizando datos de páginacón
                paginacion.itemsCount = productos_combinacion.nro_registros;
                paginacion.reload();

                // Ingresando
                
                this.productos = productos_combinacion.datos;
                cambiarPVPC();
                this.combinaciones = productos_combinacion.combinacion;
                productoBindingSource.DataSource = null;
                productoBindingSource.DataSource = productos;
                dataGridView.DataSource = productoBindingSource;
                dataGridView.Refresh();

                // Mostrando la paginacion
                mostrarPaginado();

                // Formato de celdas
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                decorationDataGridView();
                loadState(false);
               
                //currentProducto = productos.Find(x => x.idProducto == 808);
            }
        }

        private async void cargarRegistrosBuscar()
        {
            loadState(true);
            try
            {
                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);
                Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;
                //RootObject<Producto> productos = await productoModel.productosPorCategoriaBuscar(sendList, textBuscar.Text, paginacion.currentPage, paginacion.speed);
                RootObject<Producto,CombinacionStock> productos_combinacion= await productoModel.productosPorCategoriaBuscar(sendList, textBuscar.Text.Trim(), paginacion.currentPage, paginacion.speed);                

                // actualizando datos de páginacón
                paginacion.itemsCount = productos_combinacion.nro_registros;
                paginacion.reload();
                this.productos = productos_combinacion.datos;
                this.combinaciones = productos_combinacion.combinacion;
                // Ingresando
                cambiarPVPC();
                productoBindingSource.DataSource = null;
                productoBindingSource.DataSource = productos;
                dataGridView.DataSource = productoBindingSource;
                dataGridView.Refresh();
                mostrarPaginado();
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

        private async void cargarRegistrosStock()
        {
            loadState(true);
            try
            {
                
                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);
                Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;

                int idAlmacen = cbxAlmacenes.SelectedIndex == -1 ? 0 : Convert.ToInt32(cbxAlmacenes.SelectedValue);
                int idSucursal = cbxSucursales.SelectedIndex == -1 ? 0 : Convert.ToInt32(cbxSucursales.SelectedValue);


                //RootObject<Producto> productos = await productoModel.productosStock(sendList, textBuscar.Text, idAlmacen, idSucursal, paginacion.currentPage, paginacion.speed);
                RootObject<Producto, CombinacionStock> productos_combinacion = await productoModel.productosStock(sendList, textBuscar.Text, idAlmacen, idSucursal, paginacion.currentPage, paginacion.speed);
                // actualizando datos de páginacón
                paginacion.itemsCount = productos_combinacion.nro_registros;
                paginacion.reload();

                // Ingresando
                
                this.productos = productos_combinacion.datos;
                cambiarPVPC();
                this.combinaciones = productos_combinacion.combinacion;
                productoBindingSource.DataSource = null;
                productoBindingSource.DataSource = productos;
                dataGridView.DataSource = productoBindingSource;
                dataGridView.Refresh();
                mostrarPaginado();
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

        private async void cargarRegistrosStockLike()
        {
            loadState(true);
            try
            {
                Dictionary<string, int> list = new Dictionary<string, int>();
                list.Add("id0", 0);
                Dictionary<string, int> sendList = (ConfigModel.currentProductoCategory.Count == 0) ? list : ConfigModel.currentProductoCategory;

                int idAlmacen = cbxAlmacenes.SelectedIndex==-1?0:    Convert.ToInt32(cbxAlmacenes.SelectedValue);
                int idSucursal = cbxSucursales.SelectedIndex ==-1 ? 0 : Convert.ToInt32(cbxSucursales.SelectedValue);
                //RootObjectData productos=await productoModel.productoDatos;
                RootObject<Producto,CombinacionStock> productos_combinacion = await productoModel.productosStockLike(sendList, textBuscar.Text.Trim(), idAlmacen, idSucursal, paginacion.currentPage, paginacion.speed);
                //RootObject<Producto> productos = await productoModel.productosStockLike(sendList, textBuscar.Text, idAlmacen, idSucursal, paginacion.currentPage, paginacion.speed);

                // actualizando datos de páginacón
                paginacion.itemsCount = productos_combinacion.nro_registros;
                paginacion.reload();

                // Ingresando
               
                this.productos = productos_combinacion.datos;
                cambiarPVPC();
                this.combinaciones = productos_combinacion.combinacion;
                productoBindingSource.DataSource = null;
                productoBindingSource.DataSource = productos;
                dataGridView.DataSource = productoBindingSource;
                dataGridView.Refresh();
                mostrarPaginado();
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

        private void cambiarPVPC()
        {
            if (productos != null)
            {

                if (productos.Count > 0)
                {

                    foreach (Producto v in productos)
                    {
                        v.precioCompra = cambiarValor(v.precioCompra, valorDeCambio);
                        v.precioVenta = cambiarValor(v.precioVenta, valorDeCambio);
                      

                   }
                
                }



            }


        }
        #endregion

        #region =========================== Estados ===========================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelNavigation.Enabled = !state;
            panelCrud.Enabled = !state;
            panelTools.Enabled = !state;
            dataGridView.Enabled = !state;
            panel4.Enabled= !state;
        }

        private void stateCombobox(bool state)
        {
            cbxAlmacenes.Enabled = state;
            cbxSucursales.Enabled = state;
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
                contador = 0;
                cargarStock();
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (lblCurrentPage.Text != "1")
            {
                paginacion.firstPage();
                contador = 0;
                cargarStock ();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lblPageCount.Text == "0") return;
            if (lblPageCount.Text != lblCurrentPage.Text)
            {
                paginacion.nextPage();
                contador = 0;
                cargarStock();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (lblPageCount.Text == "0") return;
            if (lblPageCount.Text != lblCurrentPage.Text)
            {
                paginacion.lastPage();
                cargarStock();
            }
        }

        private void lblSpeedPages_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paginacion.speed = Convert.ToInt32(lblSpeedPages.Text);
                paginacion.currentPage = 1;
                contador = 0;
                cargarStock();
            }
        }

        private void lblCurrentPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paginacion.reloadPage(Convert.ToInt32(lblCurrentPage.Text));
                contador = 0;
                cargarStock();
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
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ConfigModel.asignacionPersonal.idPuntoCompra != 0)
                executeModificar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            contador = 0;
            cargarStock();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            executeNuevo();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(ConfigModel.asignacionPersonal.idPuntoCompra !=0)

                 executeModificar();
            
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            executeEliminar();
        }

        private void textBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter )
            {
                paginacion.currentPage = 1;
                if (textBuscar.Text.Trim() != "")
                {
                    if (chkVerStock.Checked)
                    {
                        cargarRegistrosStockLike();
                    }
                    else
                    {
                        cargarRegistrosBuscar();
                    }
                }
                else
                {
                    if (chkVerStock.Checked)
                    {
                        cargarRegistrosStock();
                    }
                    else
                    {
                        cargarRegistros();
                    }
                }
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (textBuscar.Text != "")
            {
                cargarRegistrosBuscar();
            }
        }

        private void executeNuevo()
        {
            FormProductoNuevo formProducto = new FormProductoNuevo();
            formProducto.ShowDialog();
            cargarRegistros();
        }

        private  async void executeModificar()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idProducto = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

             // Buscando la registro especifico en la lista de registros
            currentProducto = await productoModel.productoDatos(idProducto);
            // Mostrando el formulario de modificacion
            FormProductoNuevo formProducto = new FormProductoNuevo(currentProducto);
            formProducto.ShowDialog();
        }

        private async void executeEliminar()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                loadState(true); // cambiando el estado
                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview

                int idProducto = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview
                currentProducto = productos.Find(x => x.idProducto == idProducto); // Buscando la registro especifico en la lista de registros

                if (currentProducto.enUso == true)
                {
                    // Pregunta de seguridad de eliminacion
                    DialogResult dialog = MessageBox.Show("¿Está seguro de inhabilitar este registro?", "Inhabilitar",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialog == DialogResult.No) { return; }
                    currentProducto = new Producto(); //creando una instancia del objeto categoria
                    currentProducto.idProducto = idProducto;
                    Response response = await productoModel.inhabilitar(currentProducto); // Eliminando con el webservice correspondiente
                    MessageBox.Show(response.msj, "Inhabilitar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Pregunta de seguridad de eliminacion
                    DialogResult dialog = MessageBox.Show("¿Está seguro de eliminar este registro?", "Eliminar",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialog == DialogResult.No) { return; }
                    currentProducto = new Producto(); //creando una instancia del objeto categoria
                    currentProducto.idProducto = idProducto;
                    Response response = await productoModel.eliminar(currentProducto); // Eliminando con el webservice correspondiente
                    MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /*
        private async void executeAnular()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Desactivar o anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idProducto = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

                currentProducto = productos.Find(x => x.idProducto == idProducto); // Buscando la registro especifico en la lista de registros
                currentProducto.estado = 0;

                // Procediendo con las desactivacion
                Response response = await productoModel.modificar(currentProducto);
                MessageBox.Show(response.msj, "Desactivar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarRegistros(); // recargando los registros en el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        */
        private void btnNuevoCategoria_Click(object sender, EventArgs e)
        {
            FormCategoriaNuevo formCategoria = new FormCategoriaNuevo();
            formCategoria.ShowDialog();
            cargarCategorias();
        }
        private void btnActualizarCategoria_Click(object sender, EventArgs e)
        {
            cargarCategorias();
        }

        // Nueva ventana para importar los productos desde un archivo excel
        private void btnImportar_Click(object sender, EventArgs e)
        {
            FormImportarProduto formImportar = new FormImportarProduto();
            formImportar.ShowDialog();
            this.reLoad();
        }

        #endregion

        #region ======================== Treeview control checked ========================

        private void recuperarCatSeleccionado()
        {
            foreach (var item in ConfigModel.currentProductoCategory)
            {

                int nodeIndex = treeViewCategoria.Nodes[0].Nodes.IndexOfKey(item.Value.ToString());
            }
        }


        private int itemNumber { get; set; }

        private void treeViewCategoria_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // OBteniendo los datos seleccionados del treeView y almacenando en un diccionary
            TreeNode mainNode = treeViewCategoria.Nodes[0];
            itemNumber = 0;
            ConfigModel.currentProductoCategory.Clear();
            getRecursiveNodes(mainNode);
            paginacion.currentPage = 1;
            // cargando los registros

            if (ConfigModel.currentProductoCategory.Count == 0)
            {
                contador = 0;
            }
            contador = 0;
            cargarStock();
        }
        public void getRecursiveNodes(TreeNode parentNode)
        {
            if (parentNode.Checked )
            {
                ConfigModel.currentProductoCategory.Add("id" + itemNumber.ToString(), Convert.ToInt32(parentNode.Name));
                itemNumber++;
            }
            
            foreach (TreeNode subNode in parentNode.Nodes)
            {
                getRecursiveNodes(subNode);
            }
        }
        #endregion

        #region ======================= Control de isChecked en el treeview =======================
        private void CheckTreeViewNode(TreeNode node, Boolean isChecked)
        {
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = isChecked;

                if (item.Nodes.Count > 0)
                {
                    this.CheckTreeViewNode(item, isChecked);
                }
            }
        }

        private void treeViewCategoria_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckTreeViewNode(e.Node, e.Node.Checked);
        }
        #endregion


        private void cargarStock()
        {

            if (contador > 0)
            {
                return;

            }
               
            
            cbxAlmacenes.Enabled = chkVerStock.Checked;
            cbxSucursales.Enabled = chkVerStock.Checked;
            if (chkVerStock.Checked)
            {
                // activar las columnas  var ver stock
                dataGridView.Columns[0].Visible = true;
                dataGridView.Columns[1].Visible = true;
                dataGridView.Columns[2].Visible = true;
                dataGridView.Columns[3].Visible = false;

                if(ConfigModel.asignacionPersonal.idPuntoCompra!=0)
                    dataGridView.Columns[4].Visible = true;
                else
                {
                    dataGridView.Columns[4].Visible = false;
                }
                dataGridView.Columns[5].Visible = false;
                dataGridView.Columns[6].Visible = false;
                if (ConfigModel.asignacionPersonal.idPuntoVenta != 0)
                    dataGridView.Columns[7].Visible = true;
                else
                {
                    dataGridView.Columns[7].Visible = false;
                }
            
                dataGridView.Columns[8].Visible = true;
                dataGridView.Columns[9].Visible = false;
                dataGridView.Columns[10].Visible = false;
                dataGridView.Columns["nombreAlmacen"].Visible = true;
                
                if (textBuscar.Text != "")
                {
                    contador++;
                    cargarRegistrosStockLike();
                }
                else
                {
                    contador++;
                    cargarRegistrosStock();
                }
                
            }
            else
            {
                dataGridView.Columns[0].Visible = true;
                dataGridView.Columns[1].Visible = true;
                dataGridView.Columns[2].Visible = true;
                dataGridView.Columns[3].Visible = false;
                if (ConfigModel.asignacionPersonal.idPuntoCompra != 0)
                    dataGridView.Columns[4].Visible = true;
                else
                {
                    dataGridView.Columns[4].Visible = false;
                }
                dataGridView.Columns[5].Visible = true;
                if (ConfigModel.asignacionPersonal.idPuntoVenta != 0)
                    dataGridView.Columns[7].Visible = true;
                else
                {
                    dataGridView.Columns[7].Visible = false;
                }
                dataGridView.Columns[7].Visible = false;
                dataGridView.Columns[8].Visible = false;
                dataGridView.Columns[9].Visible = false;
                dataGridView.Columns[10].Visible = false;
                dataGridView.Columns["nombreAlmacen"].Visible = true;

                if (textBuscar.Text != "")
                {
                    contador++;
                    cargarRegistrosBuscar();
                }
                else
                {
                    contador++;
                    cargarRegistros();
                }   
            }
        }

        private void chkActivoAlmacen_OnChange(object sender, EventArgs e)
        {
            paginacion.currentPage = 1;
            contador = 0;
            cargarStock();
        }

        private void cbxSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSucursales.SelectedIndex == -1)
                return;

            if ((int)cbxSucursales.SelectedValue == 0)
            {


                almacenBindingSource.DataSource = listAlmCargar;
                cbxAlmacenes.SelectedIndex = -1;
                cbxAlmacenes.SelectedIndex = 0;

            }
            else
            {
                List<Almacen> listA = new List<Almacen>();
                Almacen almacen = new Almacen();
                almacen.idAlmacen = 0;
                almacen.nombre = "Todas";
                listA.Add(almacen);


                List<Almacen> list = listAlm.Where(X => X.idSucursal == (int)cbxSucursales.SelectedValue).ToList();
                listA.AddRange(list);
                almacenBindingSource.DataSource = listA;
                cbxAlmacenes.SelectedIndex = -1;
                cbxAlmacenes.SelectedIndex = 0;

            }
        }

        private void cbxAlmacenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            paginacion.currentPage = 1;
            contador = 0;
            cargarStock();
        }

        private void btnImportar_Click_1(object sender, EventArgs e)
        {
            FormImportarProduto formImportarProduto = new FormImportarProduto();
            formImportarProduto.ShowDialog();
        }

        private void textBuscar_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBuscar_OnValueChanged_1(object sender, EventArgs e)
        {

        }

        private void textBuscar_KeyUp_1(object sender, KeyEventArgs e)
        {

        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Cambiar el nombre del btnEliminar
            cambiarNombreEliminar();

        }
        private void cambiarNombreEliminar()
        {
            if (dataGridView.Rows.Count == 0)
            {
                return;
            }
            int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idProducto = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview
            currentProducto = productos.Find(x => x.idProducto == idProducto); // Buscando la registro especifico en la lista de registros

            if (currentProducto.enUso == true)
            {
                btnEliminar.Text = " Desactivar (F6)";
            }
            else
            {
                btnEliminar.Text = " Eliminar (F6)";
            }
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

                index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview

                int idPresentacion = Convert.ToInt32(dataGridView.Rows[index].Cells["idProducto"].Value);
                int idAlmacen = Convert.ToInt32(dataGridView.Rows[index].Cells["idAlmacen"].Value);

                Producto producto = productos.Find(X => X.idProducto== idPresentacion && X.idAlmacen == idAlmacen);

                combinacionesProducto = combinaciones.Where(X => X.idPresentacion == idPresentacion && X.idAlmacen == idAlmacen).ToList();
                if (combinacionesProducto.Count > 0)
                {
                  
                    FormDetalleStock detalleStock = new FormDetalleStock(combinacionesProducto, producto);
                    detalleStock.ShowDialog();
                }
               
            }
        }


        public decimal cambiarValor(decimal valor, decimal valorCambio)
        {

            return valor * valorCambio;
        }

        private   void cbxMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarValorCambio();
        }

        private async void cargarValorCambio()
        {
            if (cbxMoneda.SelectedIndex == -1)
                return;


            loadState(true);
            try
            {


                Moneda monedaCambio = listMoneda.Find(X => X.idMoneda == (int)cbxMoneda.SelectedValue);
                if (monedaActual.idMoneda != monedaCambio.idMoneda)
                {

                    CambioMoneda cambio = new CambioMoneda();
                    cambio.idMonedaActual = monedaActual.idMoneda;
                    cambio.idMonedaCambio = monedaCambio.idMoneda;




                    ValorcambioMoneda valorcambioMoneda = await monedaModel.cambiarMoneda(cambio);

                    valorDeCambio = (decimal)(toDouble(valorcambioMoneda.cambioMonedaCambio) / toDouble(valorcambioMoneda.cambioMonedaActual));

                    if (productos != null)
                    {

                        if (productos.Count > 0)
                        {

                            foreach (Producto v in productos)
                            {
                                v.precioCompra = cambiarValor(v.precioCompra, valorDeCambio);
                                v.precioVenta = cambiarValor(v.precioVenta, valorDeCambio);


                            }

                            dataGridView.DataSource = null;
                            dataGridView.DataSource = productos;


                            decorationDataGridView();




                        }
                    }
                    monedaActual = monedaCambio;
                    if (monedaActual.porDefecto)
                    {
                        valorDeCambio = 1;
                    }


                }
                else
                {

                    if (monedaActual.porDefecto)
                        valorDeCambio = 1;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message, "cambio de moneda", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


            }
            finally
            {
                loadState(false);

            }
        }

        private void panelTools_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelContainer);
            drawShape.lineBorder(panel7);
            drawShape.lineBorder(panelsucursal);
            drawShape.lineBorder(panelAlmacen);
            drawShape.lineBorder(panelMoneda);
        }

        private void panelMoneda_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // columnSort.Add(name, false);
            //correlativo
            string name = dataGridView.Columns[e.ColumnIndex].Name;

            switch (name)
            {

                case "idProducto":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.idProducto).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.idProducto).ToList();

                    }
                    break;
                case "nombreProducto":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.nombreProducto).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.nombreProducto).ToList();

                    }
                    break;
                case "codigoProducto":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.codigoProducto).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.codigoProducto).ToList();

                    }
                    break;
                case "precioCompra":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.precioCompra).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.precioCompra).ToList();

                    }
                    break;
                case "stockFinanciero":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.stockFinanciero).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.stockFinanciero).ToList();

                    }
                    break;
               
                case "precioVenta":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.precioVenta).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.precioVenta).ToList();

                    }
                    break;
                case "stock":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.stock).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.stock).ToList();

                    }
                    break;
                case "nombreAlmacen":

                    if (columnSort[name])
                    {
                        columnSort[name] = false;
                        dataGridView.DataSource = productos.OrderBy(s => s.nombreAlmacen).ToList();

                    }
                    else
                    {
                        columnSort[name] = true;
                        dataGridView.DataSource = productos.OrderByDescending(s => s.nombreAlmacen).ToList();

                    }
                    break;

                    //descripcion

            }

            decorationDataGridView();
        }
    }
}
