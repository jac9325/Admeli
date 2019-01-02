using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidad;
using Modelo;
using Admeli.Componentes;

namespace Admeli.Reportes
{
    public partial class UCReporteExisteciaProductos : UserControl
    {
        private Paginacion paginacion;
        private FormPrincipal formPrincipal;
        private CategoriaModel categoriaModel = new CategoriaModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private AlmacenModel almacenModel = new AlmacenModel();
        private ProductoModel productoModel = new ProductoModel();
        private ReporteModel reporteModel = new ReporteModel();
        private List<Producto> productos { get; set; }
        private List<Sucursal> listSucursal { get; set; }
        private List<Sucursal> listSucCargar { get; set; }
        private List<Almacen> listAlmacen { get; set; }
        private List<Almacen> listAlmCargar { get; set; }
        List<ObjectReporteProducto> listaProductos { get; set; }
        string formato { get; set; }
        int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        private int numberOfItemsPerPage = 0;
        private int numberOfItemsPrintedSoFar = 0;
        public UCReporteExisteciaProductos()
        {
            InitializeComponent();
        }
        private string darformato(object dato)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }
        public UCReporteExisteciaProductos(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            formato = "{0:n" + this.nroDecimales + "}";
            this.formPrincipal = formPrincipal;
            lblSpeedPages.Text = ConfigModel.configuracionGeneral.itemPorPagina.ToString();     // carganto los items por página
            paginacion = new Paginacion(Convert.ToInt32(lblCurrentPage.Text), Convert.ToInt32(lblSpeedPages.Text));
            this.reload();
            btnFiltrar.Enabled = true;
            btnExcel.Enabled = true;
            btnImprimir.Enabled = true;
        }








        public void reload()
        {
            cargarCategorias();
            cargarSucursales();
            cargarAlmacenes();
        }

        private async void cargarCategorias()
        {
            loadState(true);
            try
            {
                // Cargando las categorias desde el webservice
                categoriaBindingSource.DataSource = await categoriaModel.categoriasTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Categorias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadState(false);
        }
        private  void cargarSucursales()
        {

            // Cargando el combobox de sucursales
       
            try
            {

                listSucCargar = new List<Sucursal>();
                listSucursal = ConfigModel.listSucursales;
                Sucursal sucursal = new Sucursal();
                sucursal.idSucursal = 0;
                sucursal.nombre = "Todas";
                listSucCargar.Add(sucursal);
                listSucCargar.AddRange(listSucursal);
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
         
           
            try
            {
                
                listAlmCargar = new List<Almacen>();
                listAlmacen = await almacenModel.almacenes();//  para todos las asignadas al personal

                Almacen almacen = new Almacen();
                almacen.idAlmacen = 0;
                almacen.nombre = "Todas";
                listAlmCargar.Add(almacen);
                listAlmCargar.AddRange(listAlmacen);
                almacenBindingSource.DataSource = listAlmCargar;

                cbxAlmacenes.SelectedIndex = -1;
                cbxAlmacenes.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarRegistros()
        {
            loadState(true);
            try
            {
                string textoBuscado = textBuscar.Text;
                if (string.IsNullOrEmpty(textoBuscado.Trim())) { textoBuscado = "todos"; }
                RootObjectReporte<ObjectReporteProducto, TotalExitencia> rootObject = await reporteModel.existenciaProductos<RootObjectReporte<ObjectReporteProducto, TotalExitencia>>(textoBuscado, int.Parse(cbxCategorias.SelectedValue.ToString()), int.Parse(cbxSucursales.SelectedValue.ToString()), int.Parse(cbxAlmacenes.SelectedValue.ToString()), paginacion.currentPage, paginacion.speed);
                listaProductos = rootObject.datos;
                objectReporteProductoBindingSource.DataSource = listaProductos;
                TotalExitencia total = rootObject.total[0];
                lbVendidos.Text = darformato(total.valorTotal);
                lbUtilidad.Text = darformato(total.valorVenta);
                // actualizando datos de páginacón
                paginacion.itemsCount = rootObject.nro_registros;
                paginacion.reload();

              
                dgvProductos.Refresh();

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
                loadState(false);
            }
        }

        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelNavigation.Enabled = !state;
            panelTools.Enabled = !state;
            panelTools.Enabled = !state;
            dgvProductos.Enabled = !state;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            //Recuperar Productos
            //http://localhost:8085/admeli/~admeli/reporte/srch-ptrn-xstnc/nombre/xpe/categoria/0/sucursal/0/almacen/0/pagina/0
            paginacion.currentPage = 1;
          
            if (chbxStockVentas.Checked)
            {
                buscarProductoStock();
            }
            else
            {

                 buscarProductos();
            }

        }

        private async void buscarProductoStock()
        {
            try
            {
                string fechaI = String.Format("{0:u}", dtpFInicial.Value);
                fechaI = fechaI.Substring(0, fechaI.Length - 1);
                string [] fechas=   fechaI.Split(' ');
                fechaI = fechas[0];
                string fechaF = String.Format("{0:u}", dtpFFinal.Value);
                fechaF = fechaF.Substring(0, fechaF.Length - 1);
                string[] fechasf = fechaF.Split(' ');
                fechaF = fechasf[0];
                string textoBuscado = textBuscar.Text;
                if (string.IsNullOrEmpty(textoBuscado.Trim())) { textoBuscado = "todos"; }

                RootObjectReporte<ObjectReporteProducto, Total> rootObject = await reporteModel.existenciaProductosStock<RootObjectReporte<ObjectReporteProducto,Total>>(textoBuscado, int.Parse(cbxCategorias.SelectedValue.ToString()), int.Parse(cbxSucursales.SelectedValue.ToString()), int.Parse(cbxAlmacenes.SelectedValue.ToString()), paginacion.currentPage, paginacion.speed, fechaI, fechaF);
                 listaProductos = rootObject.datos;
                objectReporteProductoBindingSource.DataSource = listaProductos;

                Total total = rootObject.total[0];

                lbVendidos.Text = darformato(total.totalVendidos);
                lbUtilidad.Text = darformato(total.totalUtilidad);
                // cargar totales

                

                // actualizando datos de páginacón
                paginacion.itemsCount = rootObject.nro_registros;
                paginacion.reload();


                dgvProductos.Refresh();

                // Mostrando la paginacion
                mostrarPaginado();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Filtrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void buscarProductos()
        {
            try
            {
                string textoBuscado = textBuscar.Text;
                if (string.IsNullOrEmpty(textoBuscado.Trim())) { textoBuscado = "todos"; }

                RootObjectReporte<ObjectReporteProducto, TotalExitencia> rootObject = await reporteModel.existenciaProductos<RootObjectReporte<ObjectReporteProducto, TotalExitencia>>(textoBuscado, int.Parse(cbxCategorias.SelectedValue.ToString()), int.Parse(cbxSucursales.SelectedValue.ToString()), int.Parse(cbxAlmacenes.SelectedValue.ToString()), paginacion.currentPage, paginacion.speed);
                 listaProductos = rootObject.datos;
                objectReporteProductoBindingSource.DataSource = listaProductos;

                TotalExitencia total = rootObject.total[0];
                lbVendidos.Text = darformato(total.valorTotal);
                lbUtilidad.Text = darformato(total.valorVenta);
                // actualizando datos de páginacón
                paginacion.itemsCount = rootObject.nro_registros;
                paginacion.reload();


                dgvProductos.Refresh();

                // Mostrando la paginacion
                mostrarPaginado();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Filtrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            elegirCamposExportar formElegirCamposExportar = new elegirCamposExportar(dgvProductos);
            formElegirCamposExportar.ShowDialog();
        }

        private void textBuscar_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void lblCurrentPage_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void lblPageCount_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            printDocument1.DefaultPageSettings.Landscape = true;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
           
        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {
            if (lblPageCount.Text == "0") return;
            if (lblPageCount.Text != lblCurrentPage.Text)
            {
                paginacion.nextPage();
                if (chbxStockVentas.Checked)
                {
                    buscarProductoStock();
                }
                else
                {

                    cargarRegistros();
                }
            }
        }

        private void btnLast_Click_1(object sender, EventArgs e)
        {
            if (lblPageCount.Text == "0") return;
            if (lblPageCount.Text != lblCurrentPage.Text)
            {
                paginacion.lastPage();
                if (chbxStockVentas.Checked)
                {
                    buscarProductoStock();
                }
                else
                {

                    cargarRegistros();
                }
            }
        }

        private void btnPrevious_Click_1(object sender, EventArgs e)
        {
            if (lblCurrentPage.Text != "1")
            {
                paginacion.previousPage();
                if (chbxStockVentas.Checked)
                {
                    buscarProductoStock();
                }
                else
                {

                    cargarRegistros();
                }
            }
        }

        private void btnFirst_Click_1(object sender, EventArgs e)
        {
            if (lblCurrentPage.Text != "1")
            {
                paginacion.firstPage();

                if (chbxStockVentas.Checked)
                {
                    buscarProductoStock();
                }
                else
                {

                    cargarRegistros();
                }
            }
        }

        private void lblCurrentPage_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paginacion.reloadPage(Convert.ToInt32(lblCurrentPage.Text));
                cargarRegistros();
            }
        }

        private void UCReporteExisteciaProductos_Load(object sender, EventArgs e)
        {
            darFormatoDecimales();
            
        }

        private void darFormatoDecimales()
        {
            dgvProductos.Columns["precioCompra"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvProductos.Columns["precioVenta"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvProductos.Columns["stock"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvProductos.Columns["valor"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvProductos.Columns["utilidad"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvProductos.Columns["vendidos"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;

           


            dgvProductos.Columns["precioCompra"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductos.Columns["precioVenta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductos.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductos.Columns["valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductos.Columns["utilidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProductos.Columns["vendidos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateInicial = dtpFInicial.Value;
            DateTime dateFinal = dtpFFinal.Value;
            int comparar=dateInicial.CompareTo(dateFinal);// -1 menor 0 igual 1 mayor
            if(comparar > 0)
            {
                dtpFInicial.Value = dateFinal;

            }
        }

        private void dtpFFinal_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateInicial = dtpFInicial.Value;
            DateTime dateFinal = dtpFFinal.Value;
            int comparar = dateInicial.CompareTo(dateFinal);// -1 menor 0 igual 1 mayor
            if (comparar > 0)
            {
                dtpFFinal.Value = dateInicial;

            }
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


                List<Almacen> list = listAlmacen.Where(X => X.idSucursal == (int)cbxSucursales.SelectedValue).ToList();
                listA.AddRange(list);
                almacenBindingSource.DataSource = listA;
                cbxAlmacenes.SelectedIndex = -1;
                cbxAlmacenes.SelectedIndex = 0;

            }
        }

        private void chbxStockVentas_OnChange(object sender, EventArgs e)
        {

            paginacion.currentPage = 1;
            if (chbxStockVentas.Checked)
            {
                dgvProductos.Columns["utilidad"].Visible = true;
                dgvProductos.Columns["vendidos"].Visible = true;
                dgvProductos.Columns["valor"].Visible = false;
                label12.Text = "Cantidad Vendidos";
                label11.Text = "Total de Utilidad";

                plFfinal.Visible = true;
                plFInicia.Visible = true;
             


                buscarProductoStock();
            }
            else
            {
                dgvProductos.Columns["utilidad"].Visible = false;
                dgvProductos.Columns["vendidos"].Visible = false;
                dgvProductos.Columns["valor"].Visible = true;
                plFfinal.Visible = false;
                plFInicia.Visible = false;
               
                cargarRegistros();
                label12.Text = "Valor de Compra";
                label11.Text = "Valor de Venta";
            }
           

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblSpeedPages_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void lblSpeedPages_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                paginacion.speed =Int32.Parse( lblSpeedPages.Text);

                paginacion.currentPage = 1;
                if (chbxStockVentas.Checked)
                {                   
                    buscarProductoStock();
                }
                else
                {
                    cargarRegistros();
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (!chbxStockVentas.Checked)
            {
                        e.Graphics.DrawString("nombre", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(30, 20));
                        e.Graphics.DrawString("Sucursal", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(480,20));
                        e.Graphics.DrawString("Almacén", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(610, 20));
                        e.Graphics.DrawString("P.Compra", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(820, 20));
                        e.Graphics.DrawString("Stock", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(920, 20));
                        e.Graphics.DrawString("Valor", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(1020, 20));
                        e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, 35));
            
                        int yPos = 50;

                        for (int i = numberOfItemsPrintedSoFar; i < listaProductos.Count; i++)
                        {
                            numberOfItemsPerPage++;

                            if (numberOfItemsPerPage <= 25)
                            {
                                numberOfItemsPrintedSoFar++;

                                if (numberOfItemsPrintedSoFar <= listaProductos.Count)
                                {
                                    e.Graphics.DrawString(listaProductos[i].nombreProducto, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(30, yPos));
                                    e.Graphics.DrawString(listaProductos[i].nombreSucursal, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(480, yPos));
                                    e.Graphics.DrawString(listaProductos[i].nombreAlmacen, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(610, yPos));
                                    e.Graphics.DrawString( darformato( listaProductos[i].precioCompra), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(820, yPos));
                                    e.Graphics.DrawString( darformato( listaProductos[i].stock), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(920, yPos));
                                    e.Graphics.DrawString( darformato( listaProductos[i].valor), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(1020, yPos));

                                    yPos += 30;
                                }
                                else
                                {
                                    e.HasMorePages = false;
                                }
                            }
                            else
                            {
                                numberOfItemsPerPage = 0;
                                e.HasMorePages = true;
                                return;
                            }
                        }

                        // reset the variables
                        numberOfItemsPerPage = 0;
                        numberOfItemsPrintedSoFar = 0;

            }
            else
            {
                e.Graphics.DrawString("nombre", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(30, 20));
                e.Graphics.DrawString("Sucursal", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(480, 20));
                e.Graphics.DrawString("Almacén", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(610, 20));
                e.Graphics.DrawString("P.Venta", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(820, 20));
                e.Graphics.DrawString("Vendidos", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(920, 20));
                e.Graphics.DrawString("utilidad", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(1020, 20));
                e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, 35));

                int yPos = 50;

                for (int i = numberOfItemsPrintedSoFar; i < listaProductos.Count; i++)
                {
                    numberOfItemsPerPage++;

                    if (numberOfItemsPerPage <= 25)
                    {
                        numberOfItemsPrintedSoFar++;

                        if (numberOfItemsPrintedSoFar <= listaProductos.Count)
                        {
                            e.Graphics.DrawString(listaProductos[i].nombreProducto, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(30, yPos));
                            e.Graphics.DrawString(listaProductos[i].nombreSucursal, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(480, yPos));
                            e.Graphics.DrawString(listaProductos[i].nombreAlmacen, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(610, yPos));
                            e.Graphics.DrawString(darformato(listaProductos[i].precioVenta), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(820, yPos));
                            e.Graphics.DrawString(darformato(listaProductos[i].vendidos), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(920, yPos));
                            e.Graphics.DrawString(darformato(listaProductos[i].utilidad), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(1020, yPos));

                            yPos += 30;
                        }
                        else
                        {
                            e.HasMorePages = false;
                        }
                    }
                    else
                    {
                        numberOfItemsPerPage = 0;
                        e.HasMorePages = true;
                        return;
                    }
                }

                // reset the variables
                numberOfItemsPerPage = 0;
                numberOfItemsPrintedSoFar = 0;

            }
          
           
        }
    }

    

}