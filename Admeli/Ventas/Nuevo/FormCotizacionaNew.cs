using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Admeli.Compras.buscar;
using Admeli.Compras.Buscar;
using Admeli.Productos.Nuevo;
using Admeli.Properties;
using Admeli.Ventas.buscar;
using Admeli.Ventas.Buscar;
using Admeli.Ventas.Nuevo.detalle;
using Entidad;
using Entidad.Configuracion;
using Entidad.Location;
using Modelo;
using Newtonsoft.Json;


namespace Admeli.Ventas.Nuevo
{
    public partial class FormCotizacionaNew : Form
    {

       

     


        //variables para realizar  un orden de compra ordenCompra

        CotizacionG cotizacionG { get; set; }
        TotalCotizacion totalCotizacion { get; set; }
        //webservice utilizados     
        private TipoDocumentoModel tipoDocumentoModel = new TipoDocumentoModel();
        private DocCorrelativoModel docCorrelativoModel = new DocCorrelativoModel();
        private AlternativaModel alternativaModel = new AlternativaModel();
        private FechaModel fechaModel = new FechaModel();
        private MedioPagoModel medioPagoModel = new MedioPagoModel();
        private LocationModel locationModel = new LocationModel();
        private MonedaModel monedaModel = new MonedaModel();
        private DocumentoIdentificacionModel documentoIdentificacionModel = new DocumentoIdentificacionModel();
        private ClienteModel clienteModel = new ClienteModel();
        private CotizacionModel cotizacionModel = new CotizacionModel();
        private ImpuestoModel impuestoModel = new ImpuestoModel();
        private ProductoModel productoModel = new ProductoModel();
        private PresentacionModel presentacionModel = new PresentacionModel();
        private DescuentoModel descuentoModel = new DescuentoModel();
        private StockModel stockModel = new StockModel();
        private CajaModel cajaModel = new CajaModel();
        private IngresoModel ingresoModel = new IngresoModel();
        private ConfigModel configModel = new ConfigModel();
        /// Sus datos se cargan al abrir el formulario
        private List<Moneda> monedas { get; set; }
        private List<TipoDocumento> tipoDocumentos { get; set; }
        private List<AdelantoCotizacion> list { get; set; }
        private FechaSistema fechaSistema { get; set; }
        private List<MedioPago> medioPagos { get; set; }
        private List<DocumentoIdentificacion> documentoIdentificacion { get; set; }
        private List<Cliente> listClientes { get; set; }
        private List<Impuesto> listImpuesto { get; set; }
        private List<ImpuestoDocumento> listIDocumento { get; set; }
        private List<ProductoVenta> listProductos { get; set; }
        private DescuentoProductoReceive descuentoProducto { get; set; }
        private List<DetalleV> detalleVentas { get; set; }
      
        private List<ImpuestoProducto> listImpuestosProducto { get; set; }
        List<DescuentoReceive> descuentoReceive { get; set; }
        List<AlternativaCombinacion> alternativaCombinacion { get; set; }


        public UbicacionGeografica CurrentUbicacionGeografica;
        /// Llenan los datos en las interacciones en el formulario                
      
        private CorrelativoCotizacion correlativoCotizacion { get; set; }
        private ProductoVenta currentProducto { get; set; }
        private ImpuestoProducto impuestoProducto { get; set; }
        private Cliente CurrentCliente { get; set; }
        private SaveObject currentSaveObject { get; set; }
        DescuentoSubmit descuentoSubmit { get; set; }
        private DetalleV currentdetalleV { get; set; }


        private Cotizacion currentCotizacion { get; set; }
        public bool lisenerKeyEvents { get; set; }

        private double stockPresentacion { get; set; }
        bool enModificar = false;
        public int nroNuevo = 0;
        private bool nuevo { get; set; }
        int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        string formato { get; set; }
        private double SubTotal = 0;
        private double Descuento = 0;
        private double impuesto = 0;
        private double total = 0;

        private int tab = 0;
        private bool faltaCliente=false;
        private bool faltaProducto = false;
        // variables para poder imprimir la cotizacion

        private int numberOfItemsPerPage = 0;
        private int numberOfItemsPrintedSoFar = 0;

        List<FormatoDocumento> listformato;
        private bool seleccionado;

        private double valorDeCambio = 1;
        private Moneda monedaActual { get; set; }


        // datos para hacer adelantos
        private string NOperacion { get; set; }
        private double Adelanto { get; set; }

        private bool esPedido { get; set; }

        private int idIngreso { get; set; }

        private double adelantosA { get; set; }
        private double adelanto = 0;// variable para acumular los adelantos actuales
        private string resumen_actual = "";// para ver el adelanto total actual
        private string resumen_global = "";// para el resumen global del pedido

        private bool clickbtnAdelant = false;
        private bool ordenPedido = false;// para hacer una orden de pedido "el cliente es la misma empres"
        private bool actualizar = false;
        private bool actulizarcliente = false;
       
        #region ================================ Construtor ================================

        public FormCotizacionaNew(bool esPedido=false)
        {
            InitializeComponent();
            this.nuevo = true;
            cargarFechaSistema();
            formato = "{0:n" + nroDecimales + "}";
            cargarResultadosIniciales();
            this.esPedido = esPedido;
            if (esPedido)
            {
                cargarFormPedido();
                plAdelanto.Visible = true;
            }
            darFormatoDecimales();
        }


        #region============= metods de apoyo en formato de decimales

        private void cargarResultadosIniciales()
        {


            lbSubtotal.Text = "s/" + ". " + darformato(0);
            lbDescuentoVentas.Text = "s/" + ". " + darformato(0);
            lbImpuesto.Text = "s/" + ". " + darformato(0);
            lbTotal.Text = "s/" + ". " + darformato(0);

        }
        private void cargarFormPedido()
        {
            this.Text = "Pedido";
            label16.Text = "DATOS PEDIDO (F4)";
            pLTIPO.Visible = false;
            plFecha.Visible = false;
        }


        private string darformato(object dato)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }
        private string darformatoGuardar(object dato)
        {

            string var1 = string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
            var1 = var1.Replace(",", "");
            return var1;
        }

        private double toDouble(string texto)
        {
            if (texto == "" || texto==null)
            {

                return 0;
            }
            return double.Parse(texto, CultureInfo.GetCultureInfo("en-US")); 
        }
        private int toEntero(string texto)
        {
            if (texto == "")
            {

                return 0;
            }
            return Int32.Parse(texto, CultureInfo.GetCultureInfo("en-US")); ;
        }
        private void darFormatoDecimales()
        {
            dgvDetalleOrdenCompra.Columns["precioUnitario"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvDetalleOrdenCompra.Columns["totalDataGridViewTextBoxColumn"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvDetalleOrdenCompra.Columns["precioUnitario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetalleOrdenCompra.Columns["totalDataGridViewTextBoxColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        #endregion============================

        public FormCotizacionaNew(Cotizacion currentCotizacion, bool esPedido = false)
        {

            InitializeComponent();
            this.currentCotizacion = currentCotizacion;
            this.nuevo = false;
            formato = "{0:n" + nroDecimales + "}";
            this.esPedido = esPedido;
            if (esPedido)
            {
                cargarFormPedido();
                plAdelanto.Visible = true;
            }
            darFormatoDecimales();

        }

        #endregion ================================ Construtor ================================

        #region ================================ Root Load ================================
        private void FormCompraNew_Load(object sender, EventArgs e)
        {                     
            this.reLoad();
            AddButtonColumn();
            btnModificar.Enabled = false;

            this.ParentChanged += ParentChange; // Evetno que se dispara cuando el padre cambia // Este eveto se usa para desactivar lisener key events de este modulo
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }
            if (esPedido)
            {
                label5.Visible = false;
                dtpFechaVecimiento.Visible = false;
            }
            //Si no inició caja, no podrá hacer ningún tipo de Ingreso ni adelanto
            if (!ConfigModel.cajaIniciada || ConfigModel.cajaSesion == null)
            {
                plAdelanto.Enabled = false;
            }
        }
        private async  void  reLoad()
        {
            loadState(true);
            cargarAdelantos();
            cargarObjetos();
            cargarFormatoDocumento();
           
            await cargarMonedas();
            await cargartiposDocumentos();         
            cargarCorrelactivo();
            cargarFechaSistema();        
            cargarImpuesto();
            cargarCorrelativoCaja();
            cargarProductos();
            await cargarClientes();
           
            
            lisenerKeyEvents = true;
            if (!nuevo)
            {
                cargarCotizacion();
            }
           
        }
        #endregion
        #region ======================== KEYBOARD ========================
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
                case Keys.F2: // productos
                    cbxCodigoProducto.Focus();
                    break;
                case Keys.F3:
                    cbxTipoDocumento.Focus();
                    break;
                case Keys.F4:
                    lbEditar.ForeColor = Color.Red;
                    chbxEditar.Focus();
                    break;
                case Keys.F5:
                    txtAdelanto.Focus();
                    break;
                case Keys.F7:
                    buscarProducto();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
                default:
                    if (e.Control &&  e.KeyValue ==13)
                    {
                        HacerCotizacion();
                    }

                    break;
            }

           


        }
        #endregion


        #region ============================== Load ==============================
        struct CorrelativoData
        {
            public string correlativoActual { get; set; }
            public string serie { get; set; }
        }

        private async void cargarCorrelativoCaja()
        {
            try
            {
                if (ConfigModel.cajaIniciada)
                {
                    CorrelativoData response = await cajaModel.correlativoSerie<CorrelativoData>(ConfigModel.asignacionPersonal.idCaja, 1);
                    NOperacion = String.Format("{0} - {1}", response.serie, response.correlativoActual);
                }
                else
                {
                    NOperacion = "";
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar correlativo Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void cargarFormatoDocumento()
        {

            loadState(true);
            try
            {
                TipoDocumento tipoDocumento = ConfigModel.tipoDocumento.Find(X => X.idTipoDocumento == 2);// cotizacion
                listformato = JsonConvert.DeserializeObject<List<FormatoDocumento>>(tipoDocumento.formatoDocumento);
                foreach (FormatoDocumento f in listformato)
                {

                    string textoNormalizado = f.value.Normalize(NormalizationForm.FormD);
                    //coincide todo lo que no sean letras y números ascii o espacio
                    //y lo reemplazamos por una cadena vacía.
                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                    f.value = reg.Replace(textoNormalizado, "");
                    f.value = f.value.Replace(" ", "");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }          
        }

        private async void cargarCotizacion()
        {
            try
            {
                detalleVentas = await cotizacionModel.detalleCotizacion(currentCotizacion.idCotizacion);      
                if (esPedido)
                {
                    list = await cotizacionModel.AdelantoSinUso(currentCotizacion.idCotizacion);
                    cargarAdelantos();
                }

                txtObservaciones.Text = currentCotizacion.observacion;
                dtpFechaEmision.Value = currentCotizacion.fechaEmision.date;
                dtpFechaVecimiento.Value = currentCotizacion.fechaVencimiento.date;
                double impuesto = 0;
                foreach (DetalleV V in detalleVentas)
                {
                    V.existeStock = 1;
                    double Im = (double)V.valor;
                    impuesto += Im;
                    decimal precioUnitario = V.precioUnitario;
                    decimal cantidad =(decimal) toDouble(V.cantidad);
                    decimal cantidad1 = (decimal)toDouble(V.cantidadUnitaria);

                    V.precioUnitario =precioUnitario;
                    V.cantidad = darformato(cantidad);
                    V.cantidadUnitaria = darformato(cantidad1);

                    decimal total1 = V.total;
                    V.total = total1;
                    decimal total = precioUnitario * cantidad + (decimal)Im;
                    V.totalGeneral = total;
                    V.precioVenta=total / cantidad;

                    decimal precioVenta = V.precioVenta;
                    decimal d = 1 - V.descuento/ 100;



                    V.precioVentaReal = precioVenta / d;
                    listImpuestosProducto = await impuestoModel.impuestoProducto(V.idPresentacion, ConfigModel.sucursal.idSucursal);


                    // calculamos lo impuesto posibles del producto
                    double porcentual = 0;
                    double efectivo = 0;
                    foreach (ImpuestoProducto I in listImpuestosProducto)
                    {


                        if (I.porcentual)
                        {
                            porcentual += toDouble(I.valorImpuesto);
                        }
                        else
                        {
                            efectivo += toDouble(I.valorImpuesto);
                        }
                    }
                    V.Porcentual = darformato(porcentual);
                    V.Efectivo = darformato(efectivo);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Cotizacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
          
            detalleVBindingSource.DataSource = detalleVentas;
            calculoSubtotal();
            calcularDescuento();

        }

        public  void cargarAdelantos()
        {
            if (list == null || list.Count()==0)
            {
                txtTotalAdelantos.Text = darformato(0);
                this.adelantosA = 0;
            }
            else
            {
                this.adelantosA = toDouble(list[0].adelantos);
                txtTotalAdelantos.Text = darformato(this.adelantosA);
            }
            txtAdelanto.Text = darformato(0);
        }
        private void cargarObjetos() {
            loadState(true);
            cotizacionG = new CotizacionG();
            totalCotizacion = new TotalCotizacion();
        }

        private async void  cargarProductos()
        {
           
            try
            {
                if (FormPrincipal.productos == null || actualizar )
                {

                    FormPrincipal.productos = await productoModel.productos(ConfigModel.sucursal.idSucursal, PersonalModel.personal.idPersonal, ConfigModel.currentIdAlmacen);// ver como funciona
                    listProductos = FormPrincipal.productos;
                    productoVentaBindingSource.DataSource = listProductos;
                    cbxCodigoProducto.SelectedIndex = -1;
                    cbxDescripcion.SelectedIndex = -1;
                    if (nuevo)
                    {
                        loadState(false);
                    }
                    if (actualizar)
                    {
                        loadState(false);
                        actualizar = true;
                    }
                   
                }
                else
                {
                    listProductos = FormPrincipal.productos;
                    productoVentaBindingSource.DataSource = listProductos;
                    cbxCodigoProducto.SelectedIndex = -1;
                    cbxDescripcion.SelectedIndex = -1;


                    loadState(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Rroductos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        private async Task cargarClientes()
        {
            
            try
            {

                if (FormPrincipal.clientes == null || actulizarcliente)
                {
                    FormPrincipal.clientes = await clienteModel.ListarClientesActivos();
                    listClientes = FormPrincipal.clientes;
                    clienteBindingSource.DataSource = listClientes;
                }
                else
                {
                    loadState(true);
                    listClientes = FormPrincipal.clientes;
                    clienteBindingSource.DataSource = listClientes;
                    //Elegir el cliente eventual
                    cbxNombreRazonCliente.SelectedIndex = 0;
                    cbxProveedor_SelectedIndexChanged(null, null);
                }
                if (!nuevo)
                {
                    cbxNombreRazonCliente.SelectedValue = currentCotizacion.idCliente;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private async Task cargartiposDocumentos()
        {
           
            try
            {

                documentoIdentificacion = await documentoIdentificacionModel.docIdentificacion();
                documentoIdentificacionBindingSource.DataSource = documentoIdentificacion;

                cbxTipoDocumento_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async  void cargarImpuesto()
        {
          
            try
            {
                // variables necesarios para el calculo del impuesto de la venta
                listImpuesto = await impuestoModel.listarImpuesto();

                listIDocumento = await impuestoModel.impuestoTipoDoc(ConfigModel.sucursal.idSucursal, 4); // tipo documento 4 


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Impuesto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (nuevo && FormPrincipal.productos!=null)
                {
                    loadState(false);
                }
                
               
            }

        }
        private async void cargarCorrelactivo()
        {
           
            if (nuevo)
            {

                try
                {
                    List<CorrelativoCotizacion> list = await cotizacionModel.Correlativo(ConfigModel.sucursal.idSucursal);
                    correlativoCotizacion = list[0];
                    txtNombreDocumento.Text = "COTIZACION";
                    txtSerie.Text = correlativoCotizacion.serie;
                    txtCorrelativo.Text = correlativoCotizacion.correlativoActual;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Listar Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                txtNombreDocumento.Text = "COTIZACION";
                txtSerie.Text = currentCotizacion.serie;
                txtCorrelativo.Text = currentCotizacion.correlativo;

            }


        }
        private async  Task  cargarMonedas()
        {

            
            try
            {
                monedas = await monedaModel.monedas();
                monedaBindingSource.DataSource = monedas;

                //monedas/estado/1
                cbxTipoMoneda.SelectedIndex = -1;
               

                if (!nuevo)
                {
                    
                    monedaActual = monedas.Find(X => X.idMoneda == (int)currentCotizacion.idMoneda);
                    cbxTipoMoneda.SelectedValue = currentCotizacion.idMoneda;
                    txtObservaciones.Text = currentCotizacion.observacion;
                    this.Descuento = toDouble(currentCotizacion.descuento);

                    lbDescuentoVentas.Text = monedaActual.simbolo + ". " + darformato(Descuento);

                    this.total = toDouble(currentCotizacion.total);
                    lbTotal.Text = monedaActual.simbolo + ". " + darformato(total);

                    this.SubTotal = toDouble(currentCotizacion.subTotal);
                    lbSubtotal.Text = monedaActual.simbolo + ". " + darformato(SubTotal);
                    double impuesto = total - SubTotal;
                    lbImpuesto.Text = monedaActual.simbolo + ". " + darformato(impuesto);

                }
                else
                {
                    monedaActual = monedas.Find(X => X.porDefecto == true);
                    cbxTipoMoneda.SelectedValue = monedaActual.idMoneda;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Monedas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void  cargarFechaSistema()
        {
           
            try
            {
                if (!nuevo)
                {
                    dtpFechaEmision.Value = currentCotizacion.fechaEmision.date;
                    dtpFechaVecimiento.Value = currentCotizacion.fechaVencimiento.date;

                }
                else
                {
                    fechaSistema = await fechaModel.fechaSistema();
                    dtpFechaEmision.Value = fechaSistema.fecha;
                    dtpFechaVecimiento.Value = fechaSistema.fecha;

                }

                //dtpFechaPago.Value = fechaSistema.fecha;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddButtonColumn()
        {
            DataGridViewButtonColumn buttons = new DataGridViewButtonColumn();
            {
                buttons.HeaderText = "Acciones";
                buttons.Text = "X";
                buttons.UseColumnTextForButtonValue = true;
                buttons.FillWeight = 20;
                //buttons.AutoSizeMode =
                //   DataGridViewAutoSizeColumnMode.AllCells;
                buttons.FlatStyle = FlatStyle.Popup;
                buttons.CellTemplate.Style.BackColor = Color.Red;
                buttons.CellTemplate.Style.ForeColor = Color.White;

                buttons.Name = "acciones";
            }

            dgvDetalleOrdenCompra.Columns.Add(buttons);

        }


        private void cargarProductoDetalle()
        {
            if (cbxCodigoProducto.SelectedIndex == -1) return;
            try
            {
                /// Buscando el producto seleccionado
                int idProducto = Convert.ToInt32(cbxCodigoProducto.SelectedValue);
                currentProducto = listProductos.Find(x => x.idProducto == idProducto);
                cbxDescripcion.SelectedValue = currentProducto.idPresentacion;
                // Llenar los campos del producto escogido.............!!!!!

                if (!enModificar)
                {

                    txtCantidad.Text = "1";
                    txtDescuento.Text = "0";
                }

                /// Cargando presentaciones
                //cargarPresentacionesProducto();
                /// Cargando alternativas del producto
              //  cargarAlternativasProducto();
                //determinarDescuentoEImpuesto();
                determinarStock(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
               
                cbxCodigoProducto.Focus();
            }
        }

        private void cargarPresentacionesProducto()
        {

            // arreglar esto  no es necesrio ir a un servicio
            if (cbxCodigoProducto.SelectedIndex == -1) return; /// validacion   
            int idProducto = (int)cbxCodigoProducto.SelectedValue;
            ProductoVenta productoVenta = listProductos.Find(X => X.idProducto == idProducto);
            /// Cargar las precentacione
            cbxDescripcion.SelectedValue = productoVenta.idPresentacion;
           

        }
        private void calcularPrecioUnitarioProducto()
        {
            if (cbxCodigoProducto.SelectedIndex == -1) return; /// Validación
            try
            {
                if (cbxDescripcion.SelectedIndex == -1)
                {
                    txtPrecioUnitario.Text =darformato( currentProducto.precioVenta);
                }
                else
                {
                    // Buscar presentacion elegida
                   
                    // Realizando el calculo
                    double precioCompra = toDouble(currentProducto.precioVenta);
                    double cantidadUnitario = toDouble(currentProducto.cantidadUnitaria);
                    double precioUnidatio = precioCompra * cantidadUnitario;

                    // Imprimiendo valor
                    txtPrecioUnitario.Text = darformato(precioUnidatio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Calcular total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarAlternativasProducto()
        {
            if (cbxCodigoProducto.SelectedIndex == -1) return; /// validacion
                                                               /// cargando las alternativas del producto
            try
            {

                alternativaCombinacion = await alternativaModel.cAlternativa31(Convert.ToInt32(cbxCodigoProducto.SelectedValue));
                alternativaCombinacionBindingSource.DataSource = alternativaCombinacion;
                cbxVariacion.SelectedIndex = -1;
                if (!seleccionado)
                    cbxVariacion.SelectedValue = alternativaCombinacion[0].idCombinacionAlternativa;
                else
                {

                    cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;

                }
                if (!nuevo)
                {
                    if (cbxVariacion.SelectedIndex != -1 && currentdetalleV != null)
                        cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;

                }


                if (alternativaCombinacion[0].idCombinacionAlternativa <= 0)
                    calcularPrecioUnitarioProducto();
                calcularTotal();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar alternativas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        
        }




        #endregion

        #region =========================== Estados ===========================

        public void appLoadState(bool state)
        {
            if (state)
            {
                panel1.Visible = state;
                progressBarVenta.Visible = state;
               
                progressBarVenta.Style = ProgressBarStyle.Marquee;
            }
            else
            {
               
                progressBarVenta.Style = ProgressBarStyle.Blocks;
                progressBarVenta.Visible = state;
                panel1.Visible = state;
            }
        }
        private void loadState(bool state)
        {


            appLoadState(state);
          
            panelProducto.Enabled = !state;

            panelInfo.Enabled = !state;
            panelDatos.Enabled = !state;
            panelFooter.Enabled= !state;
            lisenerKeyEvents = !state;

            if (state)
            {

                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;

            }
        }
        #endregion


        #region=========== METODOS DE APOYO EN EL CALCULO

        public void buscarProducto()
        {
            try
            {
                if (listProductos == null) return;
                FormBuscarProducto formBuscarProducto = new FormBuscarProducto(listProductos);
                formBuscarProducto.ShowDialog();

                if (formBuscarProducto.currentProducto != null)
                {


                    currentProducto = formBuscarProducto.currentProducto;
                    if (currentProducto.precioVenta == null)
                    {

                        currentProducto = null;
                        MessageBox.Show("Error:  producto seleccionado no tiene precio de venta, no se puede seleccionar", "Buscar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;

                    }
                    else
                    {
                        limpiarCamposProducto();
                        cbxCodigoProducto.SelectedValue = formBuscarProducto.currentProducto.idProducto;

                    }
                 

                }



            

            }
            catch (Exception ex)

            {
                MessageBox.Show("Error: " + ex.Message, "Productos ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        private void calculoSubtotal()
        {


            try
            {

                if (cbxTipoMoneda.SelectedValue == null)
                    return;

                decimal subTotalLocal = 0;
                decimal TotalLocal = 0;
                foreach (DetalleV item in detalleVentas)
                {
                    if (item.estado == 1)
                        subTotalLocal += item.total;
                    TotalLocal += item.totalGeneral;
                }


                Moneda moneda = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);
                this.SubTotal =(double) subTotalLocal;

                lbSubtotal.Text = moneda.simbolo + ". " + darformato(subTotalLocal);
                // determinar impuesto de cada producto
                decimal impuestoTotal = TotalLocal - subTotalLocal;

                // arreglar esto esta mal la logica ya que el impuesto es procentual

                this.impuesto = (double)impuestoTotal;
                lbImpuesto.Text = moneda.simbolo + ". " + darformato(impuestoTotal);

                // determinar impuesto de cada producto
                this.total = (double)TotalLocal;
                lbTotal.Text = moneda.simbolo + ". " + darformato(TotalLocal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Calcular Subtota", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        /// <summary>
        /// Calcular Total
        /// </summary>
        private void calcularTotal()
        {
            try
            {
                if (txtCantidad.Text.Trim() == "") txtTotalProducto.Text = "0";
                if (txtPrecioUnitario.Text.Trim() == "" || txtCantidad.Text.Trim() == "" || txtDescuento.Text.Trim() == "") return; /// Validación
                double precioUnidario = toDouble(txtPrecioUnitario.Text);
                double cantidad = toDouble(txtCantidad.Text);
                double descuento = toDouble(txtDescuento.Text);
                double totalConDescuento = (precioUnidario * cantidad) - (descuento / 100) * (precioUnidario * cantidad);
                txtTotalProducto.Text = darformato(totalConDescuento);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Calcular total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        public async void determinarDescuentoEImpuesto()
        {

            try
            {

                if (txtCantidad.Text != "" && cbxCodigoProducto.SelectedValue != null)
                {
                    DescuentoProductoSubmit descuentoProductoSubmit = new DescuentoProductoSubmit();

                    descuentoProductoSubmit.cantidad = toDouble(txtCantidad.Text);
                    descuentoProductoSubmit.cantidades = "";
                    descuentoProductoSubmit.idGrupoCliente = CurrentCliente != null ? CurrentCliente.idGrupoCliente : 1;
                    descuentoProductoSubmit.idProducto = (int)cbxCodigoProducto.SelectedValue;
                    descuentoProductoSubmit.idProductos = "";
                    descuentoProductoSubmit.idSucursal = ConfigModel.sucursal.idSucursal;
                    string dateEmision = String.Format("{0:u}", dtpFechaEmision.Value);
                    dateEmision = dateEmision.Substring(0, dateEmision.Length - 1);
                    string dateVecimiento = String.Format("{0:u}", dtpFechaVecimiento.Value);
                    dateVecimiento = dateVecimiento.Substring(0, dateVecimiento.Length - 1);
                    descuentoProductoSubmit.fechaInicio = dateEmision;
                    descuentoProductoSubmit.fechaFin = dateVecimiento;


                    descuentoProducto = await descuentoModel.descuentototalentrefechas(descuentoProductoSubmit);

                    txtDescuento.Text = darformato(descuentoProducto.descuento);

                    calcularTotal();

                    determinarDescuento();
                    // para el descuento en grupo


                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Determinar Descuento I", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          

        }

        public async void determinarDescuento()
        {

            
            try
            {
                string dateEmision = String.Format("{0:u}", dtpFechaEmision.Value);
                dateEmision = dateEmision.Substring(0, dateEmision.Length - 1);
                string dateVecimiento = String.Format("{0:u}", dtpFechaVecimiento.Value);
                dateVecimiento = dateVecimiento.Substring(0, dateVecimiento.Length - 1);


                if (detalleVentas != null)
                    if (detalleVentas.Count != 0)
                    {
                        //primero traemos los descuento correspondientes
                        descuentoSubmit = new DescuentoSubmit();
                        string cantidades = "";
                        string idProductos = "";
                        foreach (DetalleV V in detalleVentas)
                        {
                            cantidades += V.cantidad + ",";
                            idProductos += V.idProducto + ",";
                        }

                        descuentoSubmit.idProductos = idProductos.Substring(0, idProductos.Length - 1);
                        descuentoSubmit.cantidades = cantidades.Substring(0, cantidades.Length - 1);
                        descuentoSubmit.idGrupoCliente = CurrentCliente != null ? CurrentCliente.idGrupoCliente : 1;
                        descuentoSubmit.idSucursal = ConfigModel.sucursal.idSucursal;
                        descuentoSubmit.fechaInicio = dateEmision;
                        descuentoSubmit.fechaFin = dateVecimiento;

                        descuentoReceive = await descuentoModel.descuentototalentrefechasgrupo(descuentoSubmit);




                        int i = 0;

                        foreach (DetalleV V in detalleVentas)
                        {
                            double descuento = descuentoReceive[i++].descuento;

                            V.descuento = (decimal)descuento;
                            // nuevo Precio unitario
                            decimal precioUnitario = V.precioVentaReal;
                            decimal precioUnitarioDescuento = precioUnitario - ((decimal)descuento / 100) * precioUnitario;
                            V.precioVenta = precioUnitarioDescuento;

                            decimal precioUnitarioI1 = precioUnitarioDescuento;

                            double porcentual = toDouble(V.Porcentual);
                            double efectivo = toDouble(V.Efectivo);
                            if (porcentual != 0)
                            {
                                decimal datoaux = ((decimal)porcentual / 100) + 1;
                                precioUnitarioI1 = precioUnitarioDescuento / datoaux;
                            }
                            decimal precioUnitarioImpuesto = precioUnitarioI1 - (decimal)efectivo;
                            V.precioUnitario = precioUnitarioImpuesto;
                            V.total = precioUnitarioImpuesto *(decimal) toDouble(V.cantidad);// utilizar para sacar el subtotal
                            V.totalGeneral = precioUnitarioDescuento * (decimal)toDouble(V.cantidad);


                        }



                        detalleVBindingSource.DataSource = null;
                        detalleVBindingSource.DataSource = detalleVentas;




                        // Calculo de totales y subtotales
                        calculoSubtotal();
                        descuentoTotal();

                    }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "determinar Descuento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {

              
            }



        }

        private void descuentoTotal()
        {
            try
            {
                if (cbxTipoMoneda.SelectedValue == null)
                    return;
                decimal descuentoTotal = 0;
                // calcular el descuento total
                foreach (DetalleV V in detalleVentas)
                {
                    // calculamos el decuento para cada elemento
                    decimal precioReal = V.precioVentaReal;
                    double cantidad = toDouble(V.cantidad);
                    decimal total = precioReal * (decimal)cantidad;
                    decimal descuentoV = total - V.totalGeneral;
                    descuentoTotal += descuentoV;

                }



                this.Descuento = (double)descuentoTotal;


                if(Descuento>0)
                {

                    lbDescuentoVentas.Visible = true;
                    label4.Visible = true;
                }
                else
                {

                    lbDescuentoVentas.Visible = false;
                    label4.Visible = false;
                }

            
                Moneda moneda = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);

                // ver algun criterio para ver si es cero que no se muestre por los decimales que tenga
                lbDescuentoVentas.Text = moneda.simbolo + ". " + darformato(descuentoTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "determinar Descuento total" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        public async void determinarStock(double cantidad)
        {

            if (cbxCodigoProducto.SelectedIndex == -1 || cbxVariacion.SelectedIndex == -1) return;
            loadState(true);
            try
            {


                // determinamos el stock del producto seleccionado
                List<StockReceive> stockReceive = await stockModel.getStockProductoCombinacion((int)cbxCodigoProducto.SelectedValue, cbxVariacion.SelectedValue == null ? 0 : (int)cbxVariacion.SelectedValue, ConfigModel.sucursal.idSucursal, PersonalModel.personal.idPersonal);

                if (stockReceive.Count == 0)
                    return;
                double stockTotal = stockReceive[0].stock_total;

                double stockDetalle = 0;
                // si exite en el producto en lista detalle
                if (detalleVentas != null && (cbxDescripcion.SelectedIndex != -1))
                {
                    foreach (DetalleV V in detalleVentas)
                    {
                        if (V.idPresentacion == currentProducto.idPresentacion && V.idCombinacionAlternativa == (int)cbxVariacion.SelectedValue)
                        {
                            stockDetalle = toDouble(V.cantidad);
                        }

                    }
                }
                stockPresentacion = stockTotal - stockDetalle;
                if (stockPresentacion > 0)
                {
                    lbStock1.Text = "/" + stockTotal.ToString();
                    lbStock1.ForeColor = Color.Green;
                }
                else
                {
                    lbStock1.Text = "no exite stock suficiente";
                    lbStock1.ForeColor = Color.Red;

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "determinar stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                
               
                    loadState(false);
                    cbxCodigoProducto.Focus();
                    decorationDataGridView();
                                                           
            }


        }
        private void cargarDescripcionDetalle()
        {

            if (cbxDescripcion.SelectedIndex == -1) return;
            try
            {
                /// Buscando el producto seleccionado
                int idPresentacion = Convert.ToInt32(cbxDescripcion.SelectedValue);
                currentProducto= listProductos.Find(x => x.idPresentacion == idPresentacion);
                cbxCodigoProducto.SelectedValue = currentProducto.idProducto;
                // Llenar los campos del producto escogido.............!!!!!

                if (!enModificar) {
                    txtCantidad.Text = "1";
                    txtDescuento.Text = "0";
                }

                /// Cargando presentaciones           

                /// Cargando alternativas del producto
                cargarAlternativasdescripcion();
                determinarDescuentoEImpuesto();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void calcularPrecioUnitarioDescripcion()
        {
            if (cbxDescripcion.SelectedIndex == -1) return; /// Validación
            try
            {
                if (cbxDescripcion.SelectedIndex == -1)
                {
                    txtPrecioUnitario.Text =darformato( currentProducto.precioVenta);
                }
                else
                {
                    // Buscar presentacion elegida

                    if (!btnModificar.Enabled)
                    {
                        double precioCompra = toDouble( currentProducto.precioVenta);
                        double cantidadUnitario = toDouble(currentProducto.cantidadUnitaria);
                        double precioUnidatio = precioCompra * cantidadUnitario*valorDeCambio;
                        txtPrecioUnitario.Text = darformato(precioUnidatio);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Calcular precio unitario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarAlternativasdescripcion()
        {

            loadState(true);
            try
            {

                if (cbxDescripcion.SelectedIndex == -1) return; /// validacion
                                                                /// cargando las alternativas del producto
                alternativaCombinacion = await alternativaModel.cAlternativa31(currentProducto.idPresentacion);
                alternativaCombinacionBindingSource.DataSource = alternativaCombinacion;
                cbxVariacion.SelectedIndex = -1;
                if (!seleccionado)
                {
                    cbxVariacion.SelectedValue = alternativaCombinacion[0].idCombinacionAlternativa;
                    

                }
                    
                else
                {
                    txtCantidad.Text = currentdetalleV.cantidad;
                    txtDescuento.Text =darformato( currentdetalleV.descuento);
                    txtPrecioUnitario.Text = darformato( currentdetalleV.precioVentaReal);
                    txtTotalProducto.Text = darformato( currentdetalleV.totalGeneral);
                    cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;

                }
                if (!nuevo)
                {
                    if (cbxVariacion.SelectedIndex != -1 && currentdetalleV != null)
                        cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, " cargar Alternativa ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
               
            }

            if (alternativaCombinacion[0].idCombinacionAlternativa <= 0)
                calcularPrecioUnitarioDescripcion();
            calcularTotal();
        }

        private void calcularDescuento()
        {
            try
            {
                if (cbxTipoMoneda.SelectedValue == null)
                    return;

                decimal descuentoTotal = 0;
                Moneda moneda = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);

                // calcular el descuento total
                foreach (DetalleV V in detalleVentas)
                {
                    // calculamos el decuento para cada elemento

                    if (V.estado == 1)
                    {
                        decimal total = V.precioUnitario * (decimal)toDouble(V.cantidad);
                        decimal descuentoC = total -V.total;
                        descuentoTotal += descuentoC;
                    }

                }
                this.Descuento = (double)descuentoTotal;

                lbDescuentoVentas.Text = moneda.simbolo + ". " + darformato(descuentoTotal);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "calcular Descuento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private bool exitePresentacion(int idPresentacion)
        {
            foreach (DetalleV C in detalleVentas)
            {
                if (C.idPresentacion == idPresentacion)
                    return true;
            }

            return false;

        }

        private void decorationDataGridView()
        {
            if (dgvDetalleOrdenCompra.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dgvDetalleOrdenCompra.Rows)
            {
                int idPresentacion = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview
                int idCombinacion = Convert.ToInt32(row.Cells[1].Value);
                DetalleV aux = detalleVentas.Find(x => x.idPresentacion == idPresentacion && x.idCombinacionAlternativa == idCombinacion); // Buscando la categoria en las lista de categorias
                if (aux.existeStock == 0 )
                {
                    dgvDetalleOrdenCompra.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }

        private void datosClientes()
        {
            try
            {
                if (CurrentCliente != null)
                {
                    txtDocumentoCliente.removePlaceHolder();
                    txtDocumentoCliente.Text = CurrentCliente.numeroDocumento;
                    txtDireccionCliente.Text = CurrentCliente.direccion;
                    cbxNombreRazonCliente.Text = CurrentCliente.nombreCliente;
                    cbxTipoDocumento.SelectedValue = CurrentCliente.idDocumento;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Datos Clientes ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public void establecerListaCliente(List<Cliente> listaCl)
        {
            actulizarcliente = false;
            FormPrincipal.clientes = listaCl;
            listClientes = FormPrincipal.clientes;
            clienteBindingSource.DataSource = listClientes;
        }

        private void executeBuscarCliente()
        {
            try
            {

                Buscarcliente buscarCliente = new Buscarcliente();
                buscarCliente.ShowDialog();
                if (buscarCliente.currentCliente != null)
                {
                    establecerListaCliente(buscarCliente.clientes);
                    this.CurrentCliente = buscarCliente.currentCliente;
                    datosClientes();
                    if (CurrentCliente.numeroDocumento == ConfigModel.datosGenerales.ruc)
                        {
                            ordenPedido = true;
                        }
                   else
                        {
                            ordenPedido = false;
                        }
                }
                else
                {
                    if (CurrentCliente == null)
                    {

                        //limpiarCamposCliente();
                        limpiarClienteNoDNI();

                    }
                    else
                    {
                        datosClientes();
                    }
                }
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Datos Clientes ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void limpiarCamposProducto()
        {
            cbxCodigoProducto.SelectedIndex = -1;
            cbxDescripcion.SelectedIndex = -1;
            cbxVariacion.SelectedIndex = -1;
            txtCantidad.Text = "";
            txtDescuento.Text = "";
            txtPrecioUnitario.Text = "";
            txtTotalProducto.Text = "";
            currentdetalleV = null;
            currentProducto = null;
        }




        private DetalleV buscarElemento(int idPresentacion, int idCombinacion)
        {
            return detalleVentas.Find(x => x.idPresentacion == idPresentacion && x.idCombinacionAlternativa == idCombinacion);
        }
        #endregion  
        #region=========================== eventos======================================  


        // comenzando eventos

        private void cbxCodigoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarProductoDetalle();
        }

        private void cbxDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {

            cargarDescripcionDetalle();
        }
        // validaciones
        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, txtPrecioUnitario.Text);
        }

        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, txtDescuento.Text);
        }

        private void txtTotalProducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, txtTotalProducto.Text);
        }

        private void dgvDetalleCompra_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                DetalleV aux = null;
                int y = e.ColumnIndex;

                if (dgvDetalleOrdenCompra.Columns[y].Name == "acciones")
                {
                    if (dgvDetalleOrdenCompra.Rows.Count == 0)
                    {
                        MessageBox.Show("No hay un registro seleccionado", "eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (nuevo)
                    {
                        int index = dgvDetalleOrdenCompra.CurrentRow.Index; // Identificando la fila actual del datagridview
                        int idPresentacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[0].Value);
                        int idCombinacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[1].Value);
                        // obteniedo el idRegistro del datagridview
                        aux = buscarElemento(idPresentacion, idCombinacion);
                        dgvDetalleOrdenCompra.Rows.RemoveAt(index);

                        detalleVentas.Remove(aux);

                        calculoSubtotal();
                        calcularDescuento();
                    }
                    else
                    {
                        int index = dgvDetalleOrdenCompra.CurrentRow.Index;
                        int idPresentacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[0].Value);
                        int idCombinacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[1].Value);
                        // obteniedo el idRegistro del datagridview
                        aux = buscarElemento(idPresentacion, idCombinacion);
                        aux.estado = 9;

                        dgvDetalleOrdenCompra.ClearSelection();
                        dgvDetalleOrdenCompra.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                        dgvDetalleOrdenCompra.Rows[index].DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);

                        decorationDataGridView();
                        calculoSubtotal();
                        calcularDescuento();


                    }



                    if (currentdetalleV != null)
                        if (currentdetalleV.idPresentacion == aux.idPresentacion && currentdetalleV.idCombinacionAlternativa == aux.idCombinacionAlternativa)
                        { seleccionado = false;

                            btnAgregar.Enabled = true;
                            btnModificar.Enabled = false;
                            enModificar = false;

                            cbxCodigoProducto.Enabled = true;
                            cbxDescripcion.Enabled = true;

                            limpiarCamposProducto();
                        }

                }


            }



            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminiar Producto ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void dgvDetalleCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = 0;
            try
            {
                // Verificando la existencia de datos en el datagridview
                if (dgvDetalleOrdenCompra.Rows.Count == 0)
                {
                    MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                enModificar = true;
                index = dgvDetalleOrdenCompra.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idPresentacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[0].Value);
                int idCombinacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[1].Value);


                // obteniedo el idRegistro del datagridview
                currentdetalleV = buscarElemento(idPresentacion, idCombinacion); // Buscando la registro especifico en la lista de registros

                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;
                cbxCodigoProducto.Enabled = false;
                cbxDescripcion.Enabled = false;
                seleccionado = true;
                txtCantidad.Text = darformato(toDouble(currentdetalleV.cantidad));
                cbxCodigoProducto.Text = currentdetalleV.codigoProducto;
                cbxDescripcion.Text = currentdetalleV.descripcion;
                cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;
                txtCantidad.Text = darformato(toDouble(currentdetalleV.cantidad));

                cbxVariacion.Text = currentdetalleV.nombreCombinacion;
                txtPrecioUnitario.Text = darformato(currentdetalleV.precioVentaReal);
                txtDescuento.Text = darformato(currentdetalleV.descuento);
                txtTotalProducto.Text = darformato(currentdetalleV.totalGeneral);
                
                cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "seleccionar producto ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            finally
            {
                if (!dgvDetalleOrdenCompra.Rows[index].Selected)
                    dgvDetalleOrdenCompra.Rows[index].Selected = true;

            }

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            try
            {




                currentdetalleV.idCombinacionAlternativa = (int)cbxVariacion.SelectedValue;
                currentdetalleV.nombreCombinacion = cbxVariacion.Text;
                currentdetalleV.cantidad = txtCantidad.Text.Trim();
                currentdetalleV.cantidadUnitaria = txtCantidad.Text.Trim();
                double descuento = toDouble(txtDescuento.Text.Trim());
                currentdetalleV.descuento = (decimal)descuento;
                double precioUnitario = toDouble(txtPrecioUnitario.Text.Trim());

                currentdetalleV.precioVentaReal = (decimal)precioUnitario;
                double precioUnitarioDescuento = precioUnitario - (descuento / 100) * precioUnitario;
                currentdetalleV.precioVenta =(decimal) precioUnitarioDescuento;

                // si es que exite impuesto al producto 

                // impuestoProducto
                // calculamos lo impuesto posibles del producto
                double porcentual = toDouble(currentdetalleV.Porcentual);
                double efectivo = toDouble(currentdetalleV.Efectivo);
                double precioUnitarioI1 = precioUnitarioDescuento;
                if (porcentual != 0)
                {
                    double datoaux = (porcentual / 100) + 1;
                    precioUnitarioI1 = precioUnitarioDescuento / datoaux;
                }
                double precioUnitarioImpuesto = precioUnitarioI1 - efectivo;
                currentdetalleV.precioUnitario = (decimal)precioUnitarioImpuesto;
                currentdetalleV.total = (decimal)precioUnitarioImpuesto *(decimal) toDouble(currentdetalleV.cantidad);// utilizar para sacar el subtotal
                currentdetalleV.totalGeneral = (decimal)precioUnitarioDescuento * (decimal)toDouble(currentdetalleV.cantidad);//utilizar para sacar el suTotal 

                // modificar
                detalleVBindingSource.DataSource = null;
                detalleVBindingSource.DataSource = detalleVentas;
                dgvDetalleOrdenCompra.Refresh();
                calculoSubtotal();
                descuentoTotal();
                btnAgregar.Enabled = true;
                btnModificar.Enabled = false;
                enModificar = false;
                cbxCodigoProducto.Enabled = true;
                cbxDescripcion.Enabled = true;
                seleccionado = false;
                limpiarCamposProducto();
                decorationDataGridView();
            }



            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminiar Producto ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        public async void agregar()
        {
            loadState(true);
            try
            {
                if (txtPrecioUnitario.Text == "")
                {
                    txtPrecioUnitario.Text = "0";
                }
                if (txtDescuento.Text == "")
                {

                    txtDescuento.Text = "0";
                }
                if (txtCantidad.Text == "")
                {
                    txtCantidad.Text = "0";
                }

                bool seleccionado = false;
                if (cbxCodigoProducto.SelectedValue != null)
                    seleccionado = true;
                
                // if(idProducto)


                if (seleccionado)
                {
                    if (detalleVentas == null) detalleVentas = new List<DetalleV>();
                    DetalleV detalleV = new DetalleV();

                    DetalleV aux = buscarElemento(currentProducto.idPresentacion, (int)cbxVariacion.SelectedValue);
                    if (aux != null)
                    {

                        MessageBox.Show("Este dato ya fue agregado", "presentacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tab = 1;
                        return;

                    }
                    if (toDouble(txtCantidad.Text.Trim())<1)
                    {

                        MessageBox.Show("cantidad igual "+ toDouble(txtCantidad.Text.Trim()), "presentacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tab = 1;
                        return;

                    }


                    // Creando la lista
                    detalleV.cantidad = darformato(toDouble(txtCantidad.Text.Trim()));//1

                    //determinamos el stock
                    //determinarStock(0);
                    /// Busqueda presentacion
                   
                    detalleV.idDetalleCotizacion = 0;
                    detalleV.idCotizacion = currentCotizacion != null ? currentCotizacion.idCotizacion : 0;// depende luego 
                    detalleV.cantidadUnitaria = darformato(txtCantidad.Text.Trim());
                    detalleV.codigoProducto = cbxCodigoProducto.Text.Trim();
                    detalleV.descripcion = cbxDescripcion.Text;

                    double descuento = toDouble(txtDescuento.Text.Trim());
                    detalleV.descuento = (decimal)descuento;
                    double precioUnitario = toDouble(txtPrecioUnitario.Text.Trim());

                    detalleV.precioVentaReal =(decimal)precioUnitario;
                    double precioUnitarioDescuento = precioUnitario - (descuento / 100) * precioUnitario;
                    detalleV.precioVenta = (decimal)precioUnitarioDescuento;

                    // si es que exite impuesto al producto 

                    // impuestoProducto
                    listImpuestosProducto = await impuestoModel.impuestoProducto(currentProducto.idPresentacion, ConfigModel.sucursal.idSucursal);
                    // calculamos lo impuesto posibles del producto
                    double porcentual = 0;
                    double efectivo = 0;
                    foreach (ImpuestoProducto I in listImpuestosProducto)
                    {
                        if (I.porcentual)
                        {
                            porcentual += toDouble(I.valorImpuesto);
                        }
                        else
                        {
                            efectivo += toDouble(I.valorImpuesto);
                        }
                    }
                    detalleV.Porcentual = darformato(porcentual);
                    detalleV.Efectivo = darformato(efectivo);

                    double precioUnitarioI1 = precioUnitarioDescuento;
                    if (porcentual != 0)
                    {
                        double datoaux = (porcentual / 100) + 1;
                        precioUnitarioI1 = precioUnitarioDescuento / datoaux;
                    }
                     double precioUnitarioImpuesto = precioUnitarioI1 - efectivo;
                    detalleV.precioUnitario =(decimal)precioUnitarioImpuesto;
                    detalleV.total = (decimal)precioUnitarioImpuesto * (decimal)toDouble(detalleV.cantidad);// utilizar para sacar el subtotal
                    detalleV.totalGeneral =(decimal)(precioUnitarioDescuento * toDouble(detalleV.cantidad));//utilizar para sacar el suTotal
                    detalleV.valor =detalleV.totalGeneral- detalleV.total;
                    // fin cde calculo de necesarios par detalles productos



                    detalleV.estado = 1;//5
                    detalleV.idCombinacionAlternativa = Convert.ToInt32(cbxVariacion.SelectedValue);//7
                    detalleV.idPresentacion =currentProducto.idPresentacion;
                    detalleV.idProducto = Convert.ToInt32(cbxCodigoProducto.SelectedValue);
                    detalleV.idSucursal = ConfigModel.sucursal.idSucursal;
                    detalleV.nombreCombinacion = cbxVariacion.Text;
                    detalleV.nombreMarca = currentProducto.nombreMarca;
                    detalleV.nombrePresentacion = currentProducto.nombreProducto;
                    detalleV.nro = 1;
                    // determinar el impuesto                 

                    detalleV.eliminar = "";


                    detalleV.existeStock = (stockPresentacion > 0 && stockPresentacion >= Convert.ToInt32(toDouble(txtCantidad.Text.Trim()))) ? 1 : 0;
                    ProductoVenta aux1 = listProductos.Find(x => x.idProducto == (int)cbxCodigoProducto.SelectedValue);
                    detalleV.nombreMarca = aux1.nombreMarca;
                    detalleV.nombrePresentacion = currentProducto.nombreProducto;
                    detalleV.precioEnvio = 0;
                    detalleV.ventaVarianteSinStock = aux1.ventaVarianteSinStock;
                    // agrgando un nuevo item a la lista
                    detalleVentas.Add(detalleV);

                    // calcular los descuentos

                    // agregrar
                    // Refrescando la tabla
                    detalleVBindingSource.DataSource = null;
                    detalleVBindingSource.DataSource = detalleVentas;
                    dgvDetalleOrdenCompra.Refresh();

                    // Calculo de totales y subtotales
                    calculoSubtotal();

                    descuentoTotal();

                    limpiarCamposProducto();
                    decorationDataGridView();

                }
                else
                {

                    MessageBox.Show("Error: elemento no seleccionado", "agregar Elemento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tab = 1;
                }

            }



            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "agregar Producto ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {

               
                if (tab == 0)
                {
                    loadState(false);
                    this.cbxCodigoProducto.Focus();
                  
                }
                else
                {
                    loadState(false);
                    this.dgvDetalleOrdenCompra.Focus();
                    tab = 0;
                }
              
            }


        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            agregar();

        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            loadState(true);
            try
            {
                determinarDescuentoEImpuesto();

            }
            catch(Exception ex)
            {

            }
            finally
            {

                loadState(false);
                txtCantidad.Focus();
            }
            
        }

        private void txtPrecioUnitario_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }



        private void cbxProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxNombreRazonCliente.SelectedIndex == -1) return;
            try
            {
               

                CurrentCliente = listClientes.Find(X => X.idCliente == (int)cbxNombreRazonCliente.SelectedValue);
                datosClientes();

                determinarDescuento();

                if (CurrentCliente.numeroDocumento == ConfigModel.datosGenerales.ruc)
                {
                    ordenPedido = true;

                }
                else
                {
                    ordenPedido = false;

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar Cotización ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            

            


            
        }
        private void btnModificar_EnabledChanged(object sender, EventArgs e)
        {
            if (btnModificar.Enabled)
                this.btnModificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            else
                btnModificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(88)))), ((int)(((byte)(152)))));
        }

        private void btnAgregar_EnabledChanged(object sender, EventArgs e)
        {

            if (btnModificar.Enabled)
                this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            else

                btnAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(139)))), ((int)(((byte)(23)))));

        }

        private void btnActulizar_Click(object sender, EventArgs e)
        {
            actualizar = true;
            loadState(true);
            actulizarcliente = false;
            cargarClientes();
            cargarProductos();          
            btnAgregar.Enabled = true;
            btnModificar.Enabled = false;
            enModificar = false;
            cbxCodigoProducto.Enabled = true;
            cbxDescripcion.Enabled = true;
            seleccionado = false;
            limpiarCamposProducto();
            if (!nuevo)
            {
                cargarCotizacion();

            }
            
        }

        private  void btnNuevoProducto_Click(object sender, EventArgs e)
        {
            actualizar = true;
            loadState(true);

            try
            {
                FormProductoNuevo formProductoNuevo = new FormProductoNuevo();
                formProductoNuevo.ShowDialog();
                cargarProductos();
               
                btnAgregar.Enabled = true;
                btnModificar.Enabled = false;
                enModificar = false;
                cbxCodigoProducto.Enabled = true;
                cbxDescripcion.Enabled = true;
                seleccionado = false;
                limpiarCamposProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Nuevo Producto ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private async void txtDni_TextChanged(object sender, EventArgs e)
        {
            String aux = txtDocumentoCliente.Text;

            int nroCaracteres = aux.Length;
            bool exiteProveedor = false;
            if (nroCaracteres == txtDocumentoCliente.MaxLength)
            {
                try
                {
                    CurrentCliente = listClientes.Find(X => X.numeroDocumento == aux);
                    if (CurrentCliente != null)
                    {
                        exiteProveedor = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Buscar Cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (exiteProveedor)
                {
                    // llenamos los dato con el current proveerdor
                    datosClientes();
                }
                else
                {
                    try
                    {
                        loadState(true);
                        //Si no existe el cliente, lo buscamos en la base de Datos (por seguridad de concurrencia)
                        //El servicio pide como parametro un cliente (corregir) y retorna una lista de Clientes
                        Cliente clienteBuscar = new Cliente();
                        clienteBuscar.numeroDocumento = aux;
                        List<Cliente> listaClienteBuscados = await clienteModel.BuscarClienteDocumento(clienteBuscar);
                        if (listaClienteBuscados.Count > 0)
                        {
                            //Existe almenos un cliente en la base de datos
                            actulizarcliente = true;
                            await cargarClientes();
                            CurrentCliente = listaClienteBuscados[0];
                            datosClientes();
                        }
                        else
                        {
                            //llenamos los datos en FormproveerdorNuevo
                            FormClienteNuevo formClienteNuevo = new FormClienteNuevo(aux, (int)cbxTipoDocumento.SelectedValue);
                            formClienteNuevo.ShowDialog();

                            Response response = formClienteNuevo.rest;
                            if (response != null)
                            {
                                if (response.id > 0)
                                {
                                    listClientes = await clienteModel.ListarClientesActivos();

                                    loadState(false);
                                    clienteBindingSource.DataSource = listClientes;
                                    CurrentCliente = listClientes.Find(X => X.idCliente == response.id);
                                    datosClientes();
                                    if (CurrentCliente.numeroDocumento == ConfigModel.datosGenerales.ruc)
                                    {
                                        ordenPedido = true;
                                    }
                                    else
                                    {
                                        ordenPedido = false;
                                    }
                                }
                            }
                            else
                            {
                                if (CurrentCliente == null)
                                {
                                    //limpiarCamposCliente();
                                    limpiarClienteNoDNI();
                                }
                                else
                                {
                                    datosClientes();
                                }
                            }
                        }

                        loadState(false);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Cargar Nuevo Cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                }
            }
            if (aux == "1" &&(int)cbxTipoDocumento.SelectedValue==1)
            {
                CurrentCliente = listClientes.Find(X => X.numeroDocumento == aux);
                if (CurrentCliente != null)
                {
                    exiteProveedor = true;
                }
                if (exiteProveedor)
                {
                    // llenamos los dato con el current proveerdor
                    datosClientes();
                }

            }

        }
        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            executeBuscarCliente();
        }

        private void chbxNotaEntrada_OnChange(object sender, EventArgs e)
        {
            if (!txtCorrelativo.Enabled)
                txtCorrelativo.Enabled = true;
            else
            {
                txtCorrelativo.Enabled = false;
            }
        }


        #endregion=========================== eventos======================================  

        private void cbxVariacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxVariacion.SelectedIndex == -1 || cbxCodigoProducto.SelectedIndex==-1) return;


            AlternativaCombinacion alternativa = alternativaCombinacion.Find(X => X.idCombinacionAlternativa == (int)cbxVariacion.SelectedValue);
            currentProducto = listProductos.Find(X => X.idProducto == (int)cbxCodigoProducto.SelectedValue );
            double precioUnitario =toDouble(  currentProducto.precioVenta) + toDouble(alternativa.precio);
            if (!btnModificar.Enabled)
            {
                txtPrecioUnitario.Text = darformato(precioUnitario);
            }
            

            determinarStock(0);
            



        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

      
        private  void btnCotizacion_Click_1(object sender, EventArgs e)
        {
            HacerCotizacion();
        }

        public async  void HacerCotizacion()
        {
            btnCotizacion.Focus();
            btnCotizacion.Select();
            loadState(true);
            try
            {
                await configModel.loadCajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);

            }
            catch (Exception ex)
            {

            }
            if (detalleVentas == null)
            {
                detalleVentas = new List<DetalleV>();
            }
            int I = 0;
            
            try
            {

                if(CurrentCliente == null)
                {
                    
                    MessageBox.Show("Error: " + " Cliente no seleccionado", "Cliente ",0, MessageBoxIcon.Exclamation);
                    loadState(false);
                    faltaCliente = true;
                    cbxTipoDocumento.Select();
                    cbxTipoDocumento.Focus();
                    
                    return;

                }
                if (detalleVentas.Count == 0)
                {
                  
                    MessageBox.Show("Error: " + " Productos no seleccionados", "Productos ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    loadState(false);
                    faltaProducto = true;
                    cbxCodigoProducto.Focus();
                    return;

                }
                // vemos si desea hacer un adelanto
                if (esPedido)
                {
                    Adelanto = 0;
                    if(ConfigModel.cajaIniciada && ConfigModel.cajaSesion != null)
                    {                      
                        crearObjetoIngreso();
                    }                  
                    double totalAde = 0;
                    if (list != null)
                        if (list.Count > 0)
                        {

                            totalAde = this.adelanto + toDouble(list[0].adelantos);
                        }
                        else
                        {
                            totalAde = this.adelanto;
                        }
                    else
                    {
                        totalAde = this.adelanto;
                    }

                    if (totalAde > 0)
                    {
                        resumen_actual="se hizo un ultimo adelanto de: " + this.adelanto+ "\n";
                        int nro = this.detalleVentas.Count();
                        string entre = "";
                        if(nro > 1)
                        {
                            entre = "/" + nro;
                        }
                        resumen_global = "se hizo un adelanto Total de: " + totalAde+ entre;
                        resumen_actual += resumen_global;
                    }
                    else
                    {
                        MessageBox.Show("El total de adelantos es 0, " +
                            "agrege un adelanto", "Adelanto ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        loadState(false);
                        txtAdelanto.Focus();  
                        return;
                    }

                    // hacer el adelanto

                }

                cotizacionG.correlativo = txtCorrelativo.Text.Trim();
                cotizacionG.descuento = (decimal)this.Descuento;
                cotizacionG.direccion = txtDireccionCliente.Text.Trim();
                cotizacionG.documentoIdentificacion = cbxTipoDocumento.Text;
                cotizacionG.editar = currentCotizacion != null ? false : chbxEditar.Checked;
                cotizacionG.estado = 1;// solo es cotizacion comun sin adelanto
                cotizacionG.tipo = esPedido ? "Pedido" : "Cotizacion";

                if (ordenPedido)
                {
                    cotizacionG.tipo = "ordenPedido";
                }
                string fechaEmision = String.Format("{0:u}", dtpFechaEmision.Value);
                fechaEmision = fechaEmision.Substring(0, fechaEmision.Length - 1);
                string fechaVencimiento = String.Format("{0:u}", dtpFechaVecimiento.Value);
                fechaVencimiento = fechaVencimiento.Substring(0, fechaVencimiento.Length - 1);


                cotizacionG.fechaEmision = fechaEmision;
                
                cotizacionG.fechaVencimiento = fechaVencimiento;// cambiar en pedido para q no se vaya a perder

                Cliente cliente = listClientes.Find(X => X.idCliente == (int)cbxNombreRazonCliente.SelectedValue);

                cotizacionG.idCliente = cliente.idCliente;
                cotizacionG.idCotizacion = currentCotizacion != null ? currentCotizacion.idCotizacion : 0; // ver en modificar
                cotizacionG.idDocumentoIdentificacion = (int)cbxTipoDocumento.SelectedValue;
                cotizacionG.idGrupoCliente = cliente.idGrupoCliente;
                cotizacionG.idMoneda = (cbxTipoMoneda.SelectedItem as Moneda).idMoneda;
                cotizacionG.idPersonal = PersonalModel.personal.idPersonal;
                cotizacionG.idSucursal = ConfigModel.sucursal.idSucursal;
                cotizacionG.idTipoDocumento = 2; // COTIZACION
                cotizacionG.moneda = (cbxTipoMoneda.SelectedItem as Moneda).moneda;
                cotizacionG.nombreCliente = cliente.nombreCliente;
                cotizacionG.observacion = txtObservaciones.Text.Trim();
                cotizacionG.resumenAdelanto = resumen_actual;
                cotizacionG.personal = PersonalModel.personal.nombres;
                cotizacionG.rucDni = cliente.numeroDocumento;
                cotizacionG.serie = txtSerie.Text.Trim();
                cotizacionG.subTotal = (decimal)SubTotal;
                cotizacionG.tipoCambio = 1;
                cotizacionG.total = (decimal)total;

                foreach (DetalleV V in detalleVentas)
                {
                    if (nuevo)
                    {
                        V.estadoPedido = esPedido ? 0 : -1;
                    } 
                }

                totalCotizacion.cotizacion = cotizacionG;
                totalCotizacion.detalle = detalleVentas;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Asignar Cotización", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
            Response response = null;
            try
            {  
                response = await cotizacionModel.guardar(totalCotizacion);// primero  creamos la cotizacion 
                if (response.id == 0)
                {
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (esPedido)
                {
                    I++;
                    Response responseCaja = null;
                    if (adelanto>0)
                    {
                        if (ConfigModel.cajaIniciada && ConfigModel.cajaSesion != null)
                        {
                            if (nuevo)
                            {
                                responseCaja = await ingresoModel.guardarEnUno(currentSaveObject);
                                Adelanto objadelanto = new Adelanto();
                                objadelanto.idCotizacion = response.id;
                                objadelanto.idIngreso = responseCaja.id;
                                Response responseAdelanto = await ingresoModel.guardarAdelanto(objadelanto);
                            }
                            else// por si desea hacer otro adelanto
                            {
                                responseCaja = await ingresoModel.AgregraAdelantos(currentSaveObject);
                                Adelanto objadelanto = new Adelanto();
                                objadelanto.idCotizacion = response.id;
                                objadelanto.idIngreso = responseCaja.id;
                                Response responseAdelanto = await ingresoModel.guardarAdelanto(objadelanto);
                            }
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar Cotización ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally {
                loadState(false);
            }
            if (response != null)
            {
                if (response.id > 0)
                {
                    if (nuevo)
                    {
                        MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("No se pudo guardar la cotización", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            loadState(false);
        }



        private void crearObjetoIngreso()
        {
            currentSaveObject = new SaveObject();
            currentSaveObject.estado = 1;
            currentSaveObject.fechaPago = dtpFechaEmision.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
            currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
            currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
            currentSaveObject.idMedioPago = 1;// efectivo la unica forma de pago
            currentSaveObject.idMoneda = Convert.ToInt32(cbxTipoMoneda.SelectedValue);
            currentSaveObject.medioPago = "";
            currentSaveObject.moneda = cbxTipoMoneda.Text;
            currentSaveObject.monto = darformato(adelanto);
            currentSaveObject.motivo = "Adelanto";
            currentSaveObject.numeroOperacion =NOperacion;
            currentSaveObject.observacion = "Adelanto por Cotizacion";
            currentSaveObject.personal = PersonalModel.personal.nombres;

            if (esPedido)
            {

                if (list != null)
                    if (list.Count > 0)
                    {
                        idIngreso = list[0].idCobro;
                        currentSaveObject.idCobro = idIngreso;
                        double Total = toDouble(list[0].adelantos) + adelanto;

                        if (Total > this.total)
                        {
                            MessageBox.Show("EL adelanto excede el total de pago   ", "Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            adelanto = 0;
                            
                        }
                        else
                        {
                            currentSaveObject.monto =darformato(adelanto);
                        }
                        
                    }

            }                              
        }

        // para graficar lo que va imprimir


        private void btnImprimir_Click(object sender, EventArgs e)
        {

            FormatoDocumento doc=  listformato.Last();
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("tamaño pagina",(int) doc.w,(int) doc.h);
          
            // pre visualizacion
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int X = 0;
            int Y = 0;
            int XI = 0;


            Dictionary<string, Point> dictionary = new Dictionary<string, Point>(); 
            foreach (FormatoDocumento doc in listformato)
            {


                string tipo = doc.tipo;

                switch (tipo)
                {
                    case "Label":

                        int v = 0;
                        if (this.Controls.Find("txt" + doc.value, true).Count() > 0)
                            if (((this.Controls.Find("txt" + doc.value, true).First() as TextBox) != null)) {
                                TextBox textBox = this.Controls.Find("txt" + doc.value, true).First() as TextBox;
                                e.Graphics.DrawString(textBox.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));
                                v++;
                            }
                        if (this.Controls.Find("cbx" + doc.value, true).Count() > 0)
                            if (((this.Controls.Find("cbx" + doc.value, true).First() as ComboBox) != null))
                            {
                                ComboBox textBox = this.Controls.Find("cbx" + doc.value, true).First() as ComboBox;
                                e.Graphics.DrawString(textBox.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));
                                v++;
                            }
                        if (this.Controls.Find("dtp" + doc.value, true).Count() > 0)
                            if (((this.Controls.Find("dtp" + doc.value, true).First() as DateTimePicker) != null))
                            {
                                DateTimePicker textBox = this.Controls.Find("dtp" + doc.value, true).First() as DateTimePicker;
                                e.Graphics.DrawString(textBox.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));
                                v++;
                            }
                      

                        if (v == 0)
                        {

                            switch (doc.value)
                            {
                                case "SerieCorrelativo":

                                    e.Graphics.DrawString(txtSerie.Text + "-" + txtCorrelativo.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));


                                    break;
                                case "DescripcionEmpresa":

                                    e.Graphics.DrawString(ConfigModel.datosGenerales.razonSocial, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));


                                    break;

                                case "DireccionEmpresa":

                                    e.Graphics.DrawString(ConfigModel.datosGenerales.direccion, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));


                                    break;
                                case "DocumentoEmpresa":

                                    e.Graphics.DrawString(ConfigModel.datosGenerales.ruc, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));


                                    break;
                                case "NombreEmpresa":

                                    e.Graphics.DrawString(ConfigModel.datosGenerales.razonSocial, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));


                                    break;




                            }




                        }

                        break;
                    case "ListGrid":
                        X = (int)doc.x;
                        Y = (int)doc.y;
                        XI = X;
                        break;
                    case "ListGridField":

                        e.Graphics.DrawString("", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(XI, Y));
                        dictionary.Add(doc.value, new Point(XI, Y));

                        //int YI = Y+30;
                        //foreach(DetalleV V in  detalleVentas)
                        //{
                        //    e.Graphics.DrawString(V.cantidad, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(XI, YI));
                        //    YI += 30;
                        //}
                        XI += X + (int)(doc.w);




                        break;
                    case "Img":

                        Image image = Resources.logo1;

                        e.Graphics.DrawImage(image, doc.x, doc.y,(int) doc.w, (int)doc.h);
                       
                        break;

                }


            }

            KeyValuePair<string, Point> primero = dictionary.First();
            Point point = primero.Value;
            int YI = point.Y + 30;
            Point point1 = new Point();

            if (detalleVentas == null) detalleVentas = new List<DetalleV>();
      


            for (int i = numberOfItemsPrintedSoFar; i < detalleVentas.Count; i++)
            {
                numberOfItemsPerPage++;

                if (numberOfItemsPerPage <= 30)
                {
                    numberOfItemsPrintedSoFar++;

                    if (numberOfItemsPrintedSoFar <= detalleVentas.Count)
                    {

                        if (dictionary.ContainsKey("codigoProducto"))
                        {

                            point1 = dictionary["codigoProducto"];
                            e.Graphics.DrawString(detalleVentas[i].codigoProducto, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }

                        if (dictionary.ContainsKey("nombreCombinacion"))
                        {
                            point1 = dictionary["nombreCombinacion"];
                            e.Graphics.DrawString(detalleVentas[i].nombreCombinacion, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));

                        }
                        if (dictionary.ContainsKey("cantidad"))
                        {
                            point1 = dictionary["cantidad"];
                            e.Graphics.DrawString(detalleVentas[i].cantidad, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }

                        if (dictionary.ContainsKey("nombrePresentacion"))
                        {
                            point1 = dictionary["nombrePresentacion"];
                            e.Graphics.DrawString(detalleVentas[i].nombrePresentacion, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }
                        if (dictionary.ContainsKey("descripcion"))
                        {
                            point1 = dictionary["descripcion"];
                            e.Graphics.DrawString(detalleVentas[i].descripcion, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));

                        }
                        if (dictionary.ContainsKey("nombreMarca"))
                        {
                            point1 = dictionary["nombreMarca"];
                            e.Graphics.DrawString(detalleVentas[i].nombreMarca, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }
                        if (dictionary.ContainsKey("precioUnitario"))
                        {
                            point1 = dictionary["precioUnitario"];
                            e.Graphics.DrawString(darformato( detalleVentas[i].precioUnitario), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }

                        if (dictionary.ContainsKey("total"))
                        {
                            point1 = dictionary["total"];


                            e.Graphics.DrawString(darformato(  detalleVentas[i].total), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }
                        if (dictionary.ContainsKey("precioVenta"))
                        {

                            point1 = dictionary["precioVenta"];
                            e.Graphics.DrawString(darformato( detalleVentas[i].precioVenta), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));

                        }
                        YI += 30;



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



            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;

            foreach (FormatoDocumento doc in listformato)
            {


                string tipo = doc.tipo;

                switch (tipo)
                {
                    case "Label":

                      
                        if (this.Controls.Find("lb" + doc.value, true).Count() > 0)
                            if (((this.Controls.Find("lb" + doc.value, true).First() as Label) != null))
                            {
                                Label textBox = this.Controls.Find("lb" + doc.value, true).First() as Label;
                                if (doc.value == "Total")
                                {
                                    e.Graphics.DrawString( textBox.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));


                                }
                                else
                                    e.Graphics.DrawString(textBox.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x, doc.y));                                                  
                        }

                        break;                
                }
            }

            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;



        }

        private void lbStock_Click(object sender, EventArgs e)
        {

        }

        private void cbxCodigoProducto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (faltaProducto)
            {
                faltaProducto = false;
            }
        }

        private void cbxDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }

        private void cbxVariacion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }

        private void txtCantidad_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }

        private void txtPrecioUnitario_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }

        private void txtDescuento_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }

        private void txtTotalProducto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                this.btnAgregar.Focus();
            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }

        private void dgvDetalleOrdenCompra_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {

                this.cbxCodigoProducto.Focus();

            }
            if (e.Control)
            {
                btnCotizacion.Select();



            }
        }
        private void limpiarCamposCliente()
        {
            txtDocumentoCliente.Clear();
            cbxNombreRazonCliente.SelectedIndex = -1;
            txtDireccionCliente.Clear();
        }
        private void limpiarClienteNoDNI()
        {
            cbxNombreRazonCliente.SelectedIndex = -1;
            txtDireccionCliente.Clear();
        }
        private void cbxTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if (nuevo)
            {
                if (cbxTipoDocumento.SelectedIndex == -1) return;
                int idTipoDocumento = (int)cbxTipoDocumento.SelectedValue;
                DocumentoIdentificacion tipoDocumento = documentoIdentificacion.Find(X => X.idDocumento == idTipoDocumento);
                txtDocumentoCliente.MaxLength = tipoDocumento.numeroDigitos;
                if (tipoDocumento.tipoDocumento == "Jurídico")
                {
                    if (listClientes == null ) return;
                    if (cbxNombreRazonCliente.SelectedIndex == -1)
                    { txtDni_TextChanged(null, null); return; }

                    Cliente cliente = listClientes.Find(X => X.idDocumento == tipoDocumento.idDocumento  &&  X.idCliente==(int)cbxNombreRazonCliente.SelectedValue);// && falta lo de 
                    if (cliente == null)
                    {

                        limpiarCamposCliente();                       
                        ordenPedido = false;                    
                    }

                }

                else
                {
                    if ( listClientes == null ) return;
                    if (cbxNombreRazonCliente.SelectedIndex == -1)
                    { txtDni_TextChanged(null, null); return; }
                    Cliente cliente = listClientes.Find(X => X.idDocumento == tipoDocumento.idDocumento && X.idCliente == (int)cbxNombreRazonCliente.SelectedValue);
                    if (cliente == null)
                    {
                        limpiarCamposCliente();
                        ordenPedido = false;
                    }
                }



            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxDescripcion_TextChanged(object sender, EventArgs e)
        {
           
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnCotizacion_Enter(object sender, EventArgs e)
        {
            
        }

        private void cbxTipoDocumento_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
               
            }
            else
            {
                faltaCliente = false;

            }
            //if()
        }

        private void txtDocumentoCliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void panel7_Enter(object sender, EventArgs e)
        {

        }

        private void txtSerie_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        

        private void cbxNombreRazonCliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                txtDireccionCliente.Focus();
            }
            if(e.KeyCode== Keys.Right)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);

            }
        }


        // ver en detalle esta parte para un mejor buscador
        private void btnBuscarCliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        

        private void txtDireccionCliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                lbEditar.ForeColor = Color.Red;
            }
        }

        private void chbxEditar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                lbEditar.ForeColor = Color.Black;
            }
            if (e.KeyCode == Keys.Add)
            {
                chbxEditar.Checked = true;
                txtCorrelativo.Enabled = true;
            }
            if (e.KeyCode == Keys.Subtract)
            {
                chbxEditar.Checked = false;
                txtCorrelativo.Enabled = false;
            }
       
        
        }

        private void txtCorrelativo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);

            }
        }

        private void cbxTipoMoneda_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);

            }
        }

        private void dtpFechaEmision_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpFechaEmision_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);

            }
        }

        private void dtpFechaVecimiento_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);

            }
        }
        public decimal cambiarValor(decimal valor, double valorCambio)
        {

            return valor* (decimal)valorCambio;
        }



        private  async void cbxTipoMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTipoMoneda.SelectedIndex == -1)
                return;
           
            Moneda monedaCambio = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);

            CambioMoneda cambio = new CambioMoneda();
            cambio.idMonedaActual = monedaActual.idMoneda;
            cambio.idMonedaCambio = monedaCambio.idMoneda;

            if(monedaActual.idMoneda!= monedaCambio.idMoneda)
            {
                ValorcambioMoneda valorcambioMoneda = await monedaModel.cambiarMoneda(cambio);
                valorDeCambio = toDouble(valorcambioMoneda.cambioMonedaCambio) / toDouble(valorcambioMoneda.cambioMonedaActual);
            }
            else
            {
                valorDeCambio = 1;
            }
           
            if (detalleVentas != null)
            {

                if (detalleVentas.Count > 0)
                {

                    foreach (DetalleV v in detalleVentas)
                    {
                        v.precioEnvio = cambiarValor(v.precioEnvio, valorDeCambio);
                        v.precioUnitario = cambiarValor(v.precioUnitario, valorDeCambio);
                        v.precioVenta = cambiarValor(v.precioVenta, valorDeCambio);
                        v.precioVentaReal = cambiarValor(v.precioVentaReal, valorDeCambio);
                        v.total = cambiarValor(v.total, valorDeCambio);
                        v.totalGeneral = cambiarValor(v.totalGeneral, valorDeCambio);
                        v.descuento = cambiarValor(v.descuento, valorDeCambio);

                    }

                    detalleVBindingSource.DataSource = null;
                    detalleVBindingSource.DataSource = detalleVentas;
                    calculoSubtotal();

                    descuentoTotal();


                    decorationDataGridView();



                }
            }
            if (cbxCodigoProducto.SelectedIndex != -1)
            {

                txtPrecioUnitario.Text =darformato(  cambiarValor((decimal) toDouble( txtPrecioUnitario.Text), valorDeCambio));


            }

            monedaActual = monedaCambio;



        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtDocumentoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void bunifuMetroTextbox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void txtAdelanto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e,txtAdelanto.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Confirmamos si se hará el adelanto
            DialogResult dialog = MessageBox.Show("¿Está seguro de hacer el adelanto?", "Agregar Adelanto",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialog == DialogResult.No) return;

            //Obtenemos valor del nuevo el adelanto 
            double adelantoaux = toDouble(txtAdelanto.Text.Trim());
            //Comprobamos que la suma de adelantos no sea mayor al costo total
            if (adelantoaux + adelantosA > total)
            {
                double resto = total - adelantosA;
                txtAdelanto.Text = darformato(resto);
                txtAdelanto.Focus();
                clickbtnAdelant = true;
                MessageBox.Show("La suma de los  adelantos excenden al total.", "Comprobar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Warning);             
                txtAdelanto.Focus();   
            }
            else
            {               
                adelanto += toDouble(txtAdelanto.Text);
                adelantosA += toDouble(txtAdelanto.Text);
                // actualizamos los adelantos en uc 
                txtAdelanto.Text = darformato(0);
                txtTotalAdelantos.Text = darformato( adelantosA);
                clickbtnAdelant = true;
                MessageBox.Show("El adelanto actual es de: "+ adelanto, "Comprobar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAdelanto.Focus();
            }
           
        }

        private void txtAdelanto_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAdelanto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter )
            {
                if (clickbtnAdelant)
                {
                    clickbtnAdelant = false;

                }
                else
                {
                    btnAdelanto.Focus();
                    btnAdelanto.Select();

                }
               
            }
            
        }
    }

    class SaveObject
    {
        // public int idIngreso { get; set; }
        public int estado { get; set; }
        public string fechaPago { get; set; }
        public int idCaja { get; set; }
        public int idCajaSesion { get; set; }
        public int idMedioPago { get; set; }
        public int idCobro { get; set; }
        public int idMoneda { get; set; }
        public string medioPago { get; set; }
        public string moneda { get; set; }
        public string monto { get; set; }
        public string motivo { get; set; }
        public string numeroOperacion { get; set; }
        public string observacion { get; set; }
        public string personal { get; set; }
        public int idIngreso { get; set; }
    }

    

}
