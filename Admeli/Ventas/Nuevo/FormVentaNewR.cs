using Admeli.Componentes;
using Admeli.Productos.Nuevo;
using Admeli.Properties;
using Admeli.Ventas.buscar;
using Admeli.Ventas.Buscar;
using Admeli.Ventas.Nuevo.Adelantos;
using Entidad;
using Entidad.Configuracion;
using Entidad.Location;
using Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Ventas.Nuevo
{
    public partial class FormVentaNewR : Form
    {

        // Variables para verificar stock
        List<List<object>> dato { get; set; }
        VerificarStock verificarStock { get; set; }
        AbasteceV abastece { get; set; }
        AbasteceReceive abasteceReceive { get; set; }
        //variables para realizar  un venta
        private Cobrov cobrov { get; set; }
        private Ventav ventav { get; set; }
        private CobroVentaV cobroVentaV { get; set; }
        private List<DatosNotaSalidaVenta> datosNotaSalidaVenta { get; set; }
        private NotasalidaVenta notasalidaVenta { get; set; }
        private VentaTotal ventaTotal { get; set; }
        SaveObjectCobroDetalle currentSaveObject = new SaveObjectCobroDetalle();// para poder hacer adelantos de venta
        private DatosVentaAbastece datosVentaAbastece { get; set; }
        List<List<object>> datoaAbastece { get; set; }

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
        private VentaModel ventaModel = new VentaModel();
        private AlmacenModel almacenModel = new AlmacenModel();
        private ConfigModel configModel = new ConfigModel();
        private CobroModel cobroModel = new CobroModel();
        /// Sus datos se cargan al abrir el formulario
        private List<Moneda> monedas { get; set; }
        private List<TipoDocumento> tipoDocumentos { get; set; }
        private FechaSistema fechaSistema { get; set; }
        private List<MedioPago> medioPagos { get; set; }
        private List<DocumentoIdentificacion> documentoIdentificacion { get; set; }
        private List<Cliente> listClientes { get; set; }
        private List<Impuesto> listImpuesto { get; set; }
        private List<ImpuestoDocumento> listIDocumento { get; set; }
        private List<ProductoVenta> listProductos { get; set; }
        private List<AlternativaCombinacion> listAltenativas { get; set; }
        private DescuentoProductoReceive descuentoProducto { get; set; }
        private List<DetalleV> detalleVentas { get; set; }

        private List<ImpuestoProducto> listImpuestosProducto { get; set; }
        private List<AlmacenCompra> listAlmecenes { get; set; }
        List<AlternativaCombinacion> alternativaCombinacion { get; set; }
        public UbicacionGeografica CurrentUbicacionGeografica;
        /// Llenan los datos en las interacciones en el formulario 
        private AlmacenCompra almacenVenta { get; set; }
        private Presentacion currentPresentacion { get; set; }
        private CorrelativoCotizacion correlativoCotizacion { get; set; }
        private ProductoVenta currentProducto { get; set; }
        private ImpuestoProducto impuestoProducto { get; set; }
        private Cliente CurrentCliente { get; set; }
        public CotizacionBuscar currentCotizacion { get; set; }
        private Venta currentVenta { get; set; }
        private DetalleV currentdetalleV { get; set; }
        private double stockPresentacion { get; set; }


        private int cbxControlProductod { get; set; }

        public int nroNuevo = 0;
        private bool nuevo { get; set; }
        private bool enModificar { get; set; }
        private bool seleccionado { get; set; }
        int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        string formato { get; set; }
        private double subTotal = 0;
        private double Descuento = 0;
        private double impuesto = 0;
        private double total = 0;

        private int tab = 0;
        private bool faltaCliente = false;
        private bool faltaProducto = false;


        private bool lisenerKeyEvents = false;



        // variables para poder imprimir la cotizacion

        private int numberOfItemsPerPage = 0;
        private int numberOfItemsPrintedSoFar = 0;
        List<FormatoDocumento> listformato;
        // cambio de monedas
        private Moneda monedaActual { get; set; }
        private double valorDeCambio = 1;
        private double MontoAdelanto = 0;
        private bool esPedido = false;
        private int cobroPedido = 0;
        private double MontoPedidoTotal = 0;
        private int idPedido = 0;
        private bool actulizar = false;
        private bool actulizarcliente = false;
    
        private bool cargoAlternativas = false;
        private bool esVentaRapida = false;
        #region========================== constructor=========================
        public FormVentaNewR()
        {
            InitializeComponent();
            this.nuevo = true;
            cargarFechaSistema();


            formato = "{0:n" + nroDecimales + "}";
            if (!ConfigModel.cajaIniciada)
            {
                chbxPagarCompra.Checked = false;
                chbxPagarCompra.Enabled = false;
                verAdelanto();
            }
            chbxGuiaRemision.Checked = false;
            chbxGuiaRemision.Enabled = false;

            cargarResultadosIniciales();
            darFormatoDecimales();
            this.cbxCodigoProducto.Focus();
            this.cbxCodigoProducto.Select();
            plinfoAdelanto.Visible = false;
        
        }

        #region============= metods de apoyo en formato de decimales

        private void cargarResultadosIniciales()
        {

            lbSubtotal.Text = "s/" + ". " + darformato(0);
            lbDescuentoVenta.Text = "s/" + ". " + darformato(0);
            lbImpuesto.Text = "s/" + ". " + darformato(0);
            lbTotal.Text = "s/" + ". " + darformato(0);

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

            if (texto == "" || texto == null)
            {
                return 0;
            }
            return double.Parse(texto, CultureInfo.GetCultureInfo("en-US")); ;
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
        private void deshabilitarControles()
        {
            cbxNombreDocumento.Enabled = false;
            txtSerie.Enabled = false;
            txtCorrelativo.Enabled = false;
           
            cbxTipoDocumento.Enabled = false;
            txtDocumentoCliente.Enabled = false;
            cbxNombreRazonCliente.Enabled = false;
            cbxTipoMoneda.Enabled = false;

            if (currentCotizacion == null)
            {
                chbxEditar.Enabled = false;
                dtpFechaPago.Enabled = false;
                dtpFechaEmision.Enabled = false;

                btnActulizar.Enabled = false;
                btnAgregar.Enabled = false;
                btnBuscarCliente.Enabled = false;
                btnVenta.Enabled = false;
                btnModificar.Enabled = false;
                btnNuevoProducto.Enabled = false;
                btnImportarCotizacion.Visible = false;

            }
           
        }

        #endregion============================

        public FormVentaNewR(Venta currentVenta)
        {

            InitializeComponent();

            this.nuevo = false;
            formato = "{0:n" + nroDecimales + "}";
            this.currentVenta = currentVenta;

            if (ConfigModel.cajaSesion == null)
            {
                chbxPagarCompra.Checked = false;
                chbxPagarCompra.Enabled = false;
                verAdelanto();

            }
            chbxGuiaRemision.Checked = false;
            chbxGuiaRemision.Enabled = false;
            deshabilitarControles();
            darFormatoDecimales();
            this.cbxCodigoProducto.Select();
            plinfoAdelanto.Visible = false;
        }
        public FormVentaNewR(CotizacionBuscar currentCotizacion)
        {

            InitializeComponent();

            this.nuevo = true;
            formato = "{0:n" + nroDecimales + "}";

            esPedido = true;
            if (ConfigModel.cajaSesion == null)
            {
                chbxPagarCompra.Checked = false;
                chbxPagarCompra.Enabled = false;
                verAdelanto();
            }
            this.currentCotizacion = currentCotizacion;
            plCotizacion.Visible = false;// para evitar 
            deshabilitarControles();
            darFormatoDecimales();
        }

        #endregion========================== constructor=========================
        #region ================================ Root Load ================================
        private void FormCompraNew_Load(object sender, EventArgs e)
        {
            if (nuevo == true)
            {
                this.reLoad();
            }
            else
            {
                this.reLoad();

                cargarVentas();
                cargarCorrelativo();
                btnVenta.Text = "Modificar";
                
            }

            AddButtonColumn();

            btnModificar.Enabled = false;
            btnVenta.Enabled = true;
            if (ConfigModel.currentIdAlmacen == 0)
            {

                chbxNotaEntrada.Enabled = false;
                chbxNotaEntrada.Checked = false;
            }

            this.ParentChanged += ParentChange; // Evetno que se dispara cuando el padre cambia // Este eveto se usa para desactivar lisener key events de este modulo
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }
            this.cbxCodigoProducto.Focus();
        }
        public void reLoad()
        {
            cargarAlternattivas();
            cargarTipoComprobantes();
            cargarMonedas();
            cargarFechaSistema();
            cargarProductos();
            cargartiposDocumentos();
            
            cargarImpuesto();
            cargarObjetos();
            cargarAlmacen();
            cargarClientes();
            lisenerKeyEvents = true;

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
        private  async void TopLevelControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!lisenerKeyEvents) return;
            switch (e.KeyCode)
            {
                //
                case Keys.F2: // productos
                    cbxCodigoProducto.Focus();
                    break;
                case Keys.F3:
                    cbxTipoDocumento.Focus();
                    break;
                case Keys.F4:
                    cbxNombreDocumento.Focus();
                    break;
                case Keys.F5:
                    ImportarCotizacion();
                    break;
                case Keys.F6:
                    statuschbxActivar();
                    break;
                case Keys.F7:
                    buscarProducto();
                    break;
                case Keys.F8:
                    if (esPedido)
                    {
                        await cargarAldelanto();
                    }                  
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
                default:
                    if (e.Control && e.KeyValue == 13)
                    {
                        hacerVenta();
                    }

                    break;
            }




        }
        #endregion

        #region ============================== Load ==============================

        private void cargarFormatoDocumento(int idTipoDocumento)
        {
            try
            {

                TipoDocumento tipoDocumento = ConfigModel.tipoDocumento.Find(X => X.idTipoDocumento == idTipoDocumento);// cotizacion
                listformato = JsonConvert.DeserializeObject<List<FormatoDocumento>>(tipoDocumento.formatoDocumento);

                if (listformato == null) return;
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
            catch (Exception ex) {

                MessageBox.Show("Error: " + ex.Message, "Cargar Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }



        }
        private void cargarCorrelativo()
        {

            try
            {
                cbxNombreDocumento.SelectedValue = currentVenta.idTipoDocumento;

                txtSerie.Text = currentVenta.serie;
                txtCorrelativo.Text = currentVenta.correlativo;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Cargar Correlativo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        private async void cargarAlmacen()
        {
            try
            {
                listAlmecenes = await almacenModel.almacenesCompra(ConfigModel.sucursal.idSucursal, PersonalModel.personal.idPersonal);
                // 
                AlmacenCompra almacen = new AlmacenCompra();
                almacen.idAlmacen = ConfigModel.currentIdAlmacen;
                almacen.nombre = "ninguno";
                almacenVenta = ConfigModel.currentIdAlmacen == 0 ? almacen : listAlmecenes[0];
            }

            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Cargar Almacen", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }



        }
        private async void cargarVentas()
        {

            try
            {
                detalleVentas = await ventaModel.listarDetalleventas(currentVenta.idVenta);
            }

            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Cargar Detalle de ventas", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            try
            {
                double impuesto = 0;
                foreach (DetalleV V in detalleVentas)
                {

                    double Im = (double)V.valor;
                    impuesto += Im;


                    decimal precioUnitario = V.precioUnitario;
                    double cantidad = toDouble(V.cantidad);
                    double cantidad1 = toDouble(V.cantidadUnitaria);
                    V.existeStock = 1;
                    V.precioUnitario = precioUnitario;
                    V.cantidad = darformato(cantidad);
                    V.cantidadUnitaria = darformato(cantidad1);

                    decimal total1 = V.total;
                    V.total = total1;
                    decimal total = precioUnitario * (decimal)cantidad +(decimal) Im;
                    V.totalGeneral = total;
                    V.precioVenta = total / (decimal)cantidad;

                    decimal precioVenta = V.precioVenta;
                    decimal d = 1 - V.descuento / 100;



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

                MessageBox.Show("Error: " + ex.Message, "recalculando", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {
                loadState(false);
            }


            detalleVBindingSource.DataSource = detalleVentas;
            limpiarCamposProducto();

        }
        private async void cargarCotizacion()
        {

            if (currentCotizacion.tipo != "Pedido")
            {
                try
                {
                    detalleVentas = await cotizacionModel.detalleCotizacion(currentCotizacion.idCotizacion);

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error: " + ex.Message, "Cargar Detalle de Cotizacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }


            }
            else
            {
                try
                {
                    detalleVentas = await cotizacionModel.detallepedidos(currentCotizacion.idCotizacion);

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error: " + ex.Message, "Cargar Detalle de Cotizacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }

            try
            {

                double impuesto = 0;
                foreach (DetalleV V in detalleVentas)
                {

                    double Im = (double)V.valor;
                    impuesto += Im;

                    V.precioEnvio = 0;
                    decimal precioUnitario = V.precioUnitario;
                    double cantidad = toDouble(V.cantidad);
                    double cantidad1 = toDouble(V.cantidadUnitaria);

                    V.precioUnitario = precioUnitario;
                    V.cantidad = darformato(cantidad);
                    V.cantidadUnitaria = darformato(cantidad1);

                    decimal total1 = V.total;
                    V.total = total1;
                    decimal total = precioUnitario * (decimal)cantidad + (decimal)Im;
                    V.totalGeneral = total;
                    V.precioVenta = total / (decimal)cantidad;

                    decimal precioVenta = V.precioVenta;
                    decimal d = 1 -V.descuento / 100;



                    V.precioVentaReal = precioVenta / d;
                    listImpuestosProducto = await impuestoModel.impuestoProducto(V.idPresentacion, ConfigModel.sucursal.idSucursal);



                    List<StockReceive> stockReceive = await stockModel.getStockProductoCombinacion(V.idProducto, V.idCombinacionAlternativa, ConfigModel.sucursal.idSucursal, PersonalModel.personal.idPersonal);

                    double stockTotal = stockReceive[0].stock_total;
                    V.existeStock = cantidad - stockTotal > 0 ? 0 : 1;


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

                MessageBox.Show("Error: " + ex.Message, "recalcular Cotizacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {
                loadState(false);
            }
            detalleVBindingSource.DataSource = null;
            detalleVBindingSource.DataSource = detalleVentas;
            dgvDetalleOrdenCompra.DataSource = detalleVBindingSource;
            if (currentCotizacion.tipo == "Pedido")
            {
                calcularDescuento();
                calculoSubtotal();
            }


        }
        private void cargarObjetos() {

            cobrov = new Cobrov();
            ventav = new Ventav();
            cobroVentaV = new CobroVentaV();
            datosNotaSalidaVenta = new List<DatosNotaSalidaVenta>();
            notasalidaVenta = new NotasalidaVenta();
            ventaTotal = new VentaTotal();
            dato = new List<List<object>>();
            verificarStock = new VerificarStock();
            abastece = new AbasteceV();
            datosVentaAbastece = new DatosVentaAbastece();
            datoaAbastece = new List<List<object>>();

        }
        private void limpiarObjetos()
        {

            dato.Clear();

            datoaAbastece.Clear();
            datosNotaSalidaVenta.Clear();

        }
        private  async void cargarProductos()
        {
            try
            {
                if (FormPrincipal.productos==null ||actulizar)
                {
                   
                    FormPrincipal.productos= await productoModel.productos(ConfigModel.sucursal.idSucursal, PersonalModel.personal.idPersonal, ConfigModel.currentIdAlmacen);// ver como funciona
                    listProductos = FormPrincipal.productos;
                    productoVentaBindingSource.DataSource = listProductos;   
                    cbxCodigoProducto.SelectedIndex = -1;
                    cbxDescripcion.SelectedIndex = -1;
                    if (nuevo)
                    {
                        loadState(false);
                        this.cbxCodigoProducto.Focus();
                        this.cbxCodigoProducto.Select();
                    }
                   
                    if (actulizar)
                    {
                        loadState(false);
                        actulizar = false;
                        this.cbxCodigoProducto.Focus();
                        this.cbxCodigoProducto.Select();
                    }
                    dgvDetalleOrdenCompra.Refresh();
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
                MessageBox.Show("Error: " + ex.Message, "Listar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
        private async Task cargarClientes()
        {
            loadState(true);
            try
            {
                if (FormPrincipal.clientes == null || actulizarcliente)
                {
                    
                    actulizarcliente = false;
                    FormPrincipal.clientes = await clienteModel.ListarClientesActivos();
                    listClientes = FormPrincipal.clientes;
                    clienteBindingSource.DataSource = listClientes;

                }
                else
                {

                    listClientes = FormPrincipal.clientes;
                    clienteBindingSource.DataSource = listClientes;
                }
                
                if (!nuevo)
                {

                    Cliente cliente = listClientes.Find(X => X.idCliente == currentVenta.idCliente);
                    cbxNombreRazonCliente.SelectedValue = currentVenta.idCliente;
                    cbxTipoDocumento.SelectedValue = cliente.idDocumento;
                }
                else
                {
                    if(currentCotizacion != null)
                    {
                        cbxNombreRazonCliente.SelectedValue = currentCotizacion.idCliente;
                    }
                    else
                    {
                        cbxNombreRazonCliente.SelectedIndex = -1;
                    }

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadState(false);

        }
        private async void cargarAlternattivas()
        {
            try
            {

            listAltenativas=await  alternativaModel.cAlternativaActivas();
            }
            catch(Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Listar Alternativas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          
           
        }
        private async void cargartiposDocumentos()
        {
            try
            {
                documentoIdentificacion = await documentoIdentificacionModel.docIdentificacion();
                documentoIdentificacionBindingSource.DataSource = documentoIdentificacion;

                DocumentoIdentificacion identificacion = documentoIdentificacion.Find(X => X.tipoDocumento == "Jurídico");

                if (identificacion != null)
                {
                    cbxTipoDocumento.SelectedValue = documentoIdentificacion.Find(X => X.tipoDocumento == "Jurídico").idDocumento;
                }
                cbxTipoDocumento_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Tipos de Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void cargarImpuesto(int tipo = 3)
        {
            try
            {
                // variables necesarios para el calculo del impuesto de la venta
                listImpuesto = await impuestoModel.listarImpuesto();
                listIDocumento = await impuestoModel.impuestoTipoDoc(ConfigModel.sucursal.idSucursal, tipo); // tipo documento 4 


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Impuesto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (FormPrincipal.productos != null)
                {
                    loadState(false);
                    this.cbxCodigoProducto.Focus();
                    this.cbxCodigoProducto.Select();
                }
                    
            }

        }
        private async void cargarTipoComprobantes()
        {
            try
            {
                tipoDocumentos = await tipoDocumentoModel.tipoDocumentoVentas();
                cbxNombreDocumento.DataSource = tipoDocumentos;
                if (!nuevo)
                {
                    cbxNombreDocumento.SelectedValue = currentVenta.idTipoDocumento;

                }
                else
                {
                    List<Venta_correlativo> list = await ventaModel.listarNroDocumentoVenta(3, ConfigModel.asignacionPersonal.idPuntoVenta);
                    txtCorrelativo.Text = list[0].correlativoActual;
                    txtSerie.Text = list[0].serie;
                }
                cargarFormatoDocumento((int)cbxNombreDocumento.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void cargarMonedas()
        {
            loadState(true);
            try
            {
                monedas = await monedaModel.monedas();
                monedaBindingSource.DataSource = monedas;

                //monedas/estado/1

                
                if (currentCotizacion == null)
                {
                    monedaActual = monedas.Find(X => X.porDefecto == true);
                    cbxTipoMoneda.SelectedValue = monedaActual.idMoneda;
                }
                else
                {
                    monedaActual = monedas.Find(X => X.moneda == currentCotizacion.moneda);
                    cbxTipoMoneda.SelectedValue = monedaActual.idMoneda;
                }
                
                if (!nuevo)
                {

                    cbxTipoMoneda.Text = currentVenta.moneda;
                    Moneda moneda = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);
                    cbxTipoMoneda.Text = currentVenta.moneda;
                    txtObservaciones.Text = currentVenta.observacion;
                    this.Descuento = toDouble(currentVenta.descuento);

                    if (Descuento != 0)
                        lbDescuentoVenta.Text = moneda.simbolo + ". " + darformato(Descuento);
                    else
                    {
                        lbDescuentoVenta.Visible = false;
                        label4.Visible = false;


                    }
                    this.total = (double)currentVenta.total;
                    lbTotal.Text = moneda.simbolo + ". " + darformato(total);

                    this.subTotal = toDouble(currentVenta.subTotal);
                    lbSubtotal.Text = moneda.simbolo + ". " + darformato(subTotal);
                    double impuesto = total - subTotal;
                    lbImpuesto.Text = moneda.simbolo + ". " + darformato(impuesto);
                    valorDeCambio = 1;

                }
                if (currentCotizacion != null)
                {
                    cargarPedido();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Moneda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void cargarPedidoCotizacion()
        {
            try
            {
                currentCotizacion = await cotizacionModel.pedidoSeleccionado(this.idPedido);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "cargar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        private async void cargarFechaSistema()
        {
            loadState(true);
            try
            {
                if (!nuevo)
                {
                    dtpFechaEmision.Value = currentVenta.fechaVenta.date;
                    dtpFechaPago.Value = currentVenta.fechaPago.date;

                }
                else
                {
                    fechaSistema = await fechaModel.fechaSistema();
                    dtpFechaEmision.Value = fechaSistema.fecha;
                    dtpFechaPago.Value = fechaSistema.fecha;

                }

                //dtpFechaPago.Value = fechaSistema.fecha;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar fechas del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void AddButtonColumn()
        {

            try
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

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Agregando Buttones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
        // para el cbx de descripcion
        #endregion
        #region =========================== Estados ===========================

        public void appLoadState(bool state)
        {
            if (state)
            {
                panelCargar.Visible = state;
                progrestatus.Visible = state;
                progrestatus.Style = ProgressBarStyle.Marquee;
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                progrestatus.Style = ProgressBarStyle.Blocks;
                progrestatus.Visible = state;
                panelCargar.Visible = state;
                Cursor = Cursors.Default;
            }
        }
        private void loadState(bool state)
        {


            appLoadState(state);

            panelProductos.Enabled = !state;

            panelInfo.Enabled = !state;



            panelInfo.Enabled = !state;
            panelDatos.Enabled = !state;
            panelFooter.Enabled = !state;
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
        private DetalleV buscarElemento(int idPresentacion, int idCombinacion)
        {

            if (detalleVentas == null)
            {
                detalleVentas = new List<DetalleV>();
            }
            try
            {
                return detalleVentas.Find(x => x.idPresentacion == idPresentacion && x.idCombinacionAlternativa == idCombinacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "buscar Elemento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
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
                this.subTotal = (double)subTotalLocal;

                lbSubtotal.Text = moneda.simbolo + ". " + darformato(subTotalLocal);
                // determinar impuesto de cada producto
                decimal impuestoTotal = TotalLocal - subTotalLocal;

                // arreglar esto esta mal la logica ya que el impuesto es procentual

                this.impuesto = (double)impuestoTotal;
                lbImpuesto.Text = moneda.simbolo + ". " + darformato(impuestoTotal);

                // determinar impuesto de cada producto
                this.total = (double)TotalLocal;
                lbSaldo.Text = darformato(total);
                lbUsado.Text = darformato(0);
                lbTotal.Text = moneda.simbolo + ". " + darformato(TotalLocal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "calcular subtotal", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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




        private void cargarProductoDetalle()
        {
            if (cbxCodigoProducto.SelectedIndex == -1) return;
            try
            {
                /// Buscando el producto seleccionado
                int idProducto = Convert.ToInt32(cbxCodigoProducto.SelectedValue);


                currentProducto = listProductos.Find(x => x.idProducto == idProducto);
                cbxDescripcion.SelectedValue = currentProducto.idPresentacion;



                if (currentProducto.precioVenta == null)
                {

                    currentProducto = null;
                    MessageBox.Show("Error:  producto seleccionado no tiene precio de venta, no se puede seleccionar", "Buscar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }

                // Llenar los campos del producto escogido.............!!!!!

                if (!enModificar)
                {
                    txtCantidad.Text = "1";
                    txtDescuento.Text = "0";

                }

                /// Cargando presentaciones

                /// Cargando alternativas del producto
                cargarAlternativasProducto();
                determinarDescuentoEImpuesto();
                determinarStock(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cargarPresentacionesProducto()
        {
            try
            {
                // arreglar esto  no es necesrio ir a un servicio
                if (cbxCodigoProducto.SelectedIndex == -1) return; /// validacion   
                int idProducto = (int)cbxCodigoProducto.SelectedValue;
                ProductoVenta productoVenta = listProductos.Find(X => X.idProducto == idProducto);
                /// Cargar las precentacione
                cbxDescripcion.SelectedValue = productoVenta.idPresentacion;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar Presentacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        private void calcularPrecioUnitarioProducto()
        {
            if (cbxCodigoProducto.SelectedIndex == -1) return; /// Validación
            try
            {
                if (cbxDescripcion.SelectedIndex == -1)
                {
                    txtPrecioUnitario.Text = darformato(currentProducto.precioVenta);
                }
                else
                {
                    // Realizando el calculo
                    if (!seleccionado)
                    {
                        double precioCompra = toDouble(currentProducto.precioVenta);

                        double cantidadUnitario = toDouble(currentProducto.cantidadUnitaria);
                        double precioUnidatio = precioCompra * cantidadUnitario * valorDeCambio;

                        // Imprimiendo valor
                        txtPrecioUnitario.Text = darformato(precioUnidatio);

                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Calcular precio unitario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private  void cargarAlternativasProducto()
        {
            if (cbxCodigoProducto.SelectedIndex == -1) return; /// validacion
                                                               /// cargando las alternativas del producto



            try
            {
               
                alternativaCombinacion = listAltenativas.Where(X => X.idPresentacion == currentProducto.idPresentacion).ToList(); ;

                if (alternativaCombinacion.Count == 0)
                {
                    AlternativaCombinacion aux = new AlternativaCombinacion();
                    aux.codigoSku = "0000000";
                    aux.nombreCombinacion = "ninguno";
                    aux.precio = "0";
                    aux.idCombinacionAlternativa = 0;
                    alternativaCombinacion.Add(aux);
                }
    

                alternativaCombinacionBindingSource.DataSource = alternativaCombinacion;
                cbxVariacion.SelectedIndex = -1;
                if (!seleccionado)
                    cbxVariacion.SelectedValue = alternativaCombinacion[0].idCombinacionAlternativa;
                else
                {

                    cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;
                    txtCantidad.Text = currentdetalleV.cantidad;
                    txtDescuento.Text =darformato( currentdetalleV.descuento);
                    txtPrecioUnitario.Text = darformato( currentdetalleV.precioVenta);
                    txtTotalProducto.Text = darformato( currentdetalleV.total);

                }
                if (!nuevo)
                {
                    if (cbxVariacion.SelectedIndex != -1 && currentdetalleV != null)
                        cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;

                }


            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar fechas del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {
                cargoAlternativas = true;
            }





            if (alternativaCombinacion[0].idCombinacionAlternativa <= 0)
                calcularPrecioUnitarioProducto();

            calcularTotal();
            decorationDataGridView();
        }


        // el calculo se hace entre las fechas  entre fechas
        public async void determinarDescuentoEImpuesto()
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
                string dateVecimiento = String.Format("{0:u}", dtpFechaPago.Value);
                dateVecimiento = dateVecimiento.Substring(0, dateVecimiento.Length - 1);
                descuentoProductoSubmit.fechaInicio = dateEmision;
                descuentoProductoSubmit.fechaFin = dateVecimiento;


                descuentoProducto = await descuentoModel.descuentoTotalALaFecha(descuentoProductoSubmit);

                txtDescuento.Text = darformato(descuentoProducto.descuento);

                calcularTotal();

                determinarDescuento();
                // para el descuento en grupo


            }
        }

        public async void determinarDescuento()
        {


            try {
                string dateEmision = String.Format("{0:u}", dtpFechaEmision.Value);
                dateEmision = dateEmision.Substring(0, dateEmision.Length - 1);
                string dateVecimiento = String.Format("{0:u}", dtpFechaPago.Value);
                dateVecimiento = dateVecimiento.Substring(0, dateVecimiento.Length - 1);


                if (detalleVentas != null)
                    if (detalleVentas.Count != 0)
                    {
                        //primero traemos los descuento correspondientes
                        DescuentoSubmit descuentoSubmit = new DescuentoSubmit();
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

                        List<DescuentoReceive> descuentoReceive = await descuentoModel.descuentoTotalALaFechaGrupo(descuentoSubmit);
                        if(descuentoReceive.Count!= detalleVentas.Count)
                        {
                            return;
                        }
                        int i = 0;
                        foreach (DetalleV V in detalleVentas)
                        {
                            double descuento = descuentoReceive[i++].descuento;

                            V.descuento = (decimal)descuento;
                            // nuevo Precio unitario
                            double precioUnitario =(double)V.precioVentaReal;
                            double precioUnitarioDescuento = precioUnitario - (descuento / 100) * precioUnitario;
                            V.precioVenta = (decimal)precioUnitarioDescuento;

                            double precioUnitarioI1 = precioUnitarioDescuento;

                            double porcentual = toDouble(V.Porcentual);
                            double efectivo = toDouble(V.Efectivo);
                            if (porcentual != 0)
                            {
                                double datoaux = (porcentual / 100) + 1;
                                precioUnitarioI1 = precioUnitarioDescuento / datoaux;
                            }
                            double precioUnitarioImpuesto = precioUnitarioI1 - efectivo;
                            V.precioUnitario = (decimal)precioUnitarioImpuesto;
                            V.total = (decimal)(precioUnitarioImpuesto * toDouble(V.cantidad));// utilizar para sacar el subtotal
                            V.totalGeneral = (decimal)(precioUnitarioDescuento * toDouble(V.cantidad));


                        }

                        i = 0;
                        detalleVBindingSource.DataSource = null;
                        detalleVBindingSource.DataSource = detalleVentas;


                        // Calculo de totales y subtotales
                        calculoSubtotal();
                        descuentoTotal();

                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Determinar descuento", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {

                decorationDataGridView();
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
                    decimal descuentoV = total - (V.totalGeneral);
                    descuentoTotal += descuentoV;

                }
                this.Descuento = (double)descuentoTotal;

                if (Descuento <= 0)
                {


                    label4.Visible = false;
                    lbDescuentoVenta.Visible = false;
                }
                else
                {


                    label4.Visible = true;
                    lbDescuentoVenta.Visible = true;
                }
                Moneda moneda = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);


                lbDescuentoVenta.Text = moneda.simbolo + ". " + darformato(descuentoTotal);

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "determinar Descuento", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        public async void determinarStock(double cantidad)
        {


            if (cbxVariacion.SelectedIndex == -1) return;
            try
            {
                int idPersonal = PersonalModel.personal.idPersonal;
                if (ConfigModel.currentIdAlmacen == 0)
                {

                    idPersonal = 0;

                }

                List<StockReceive> stockReceive = await stockModel.getStockProductoCombinacion((int)cbxCodigoProducto.SelectedValue, cbxVariacion.SelectedValue == null ? 0 : (int)cbxVariacion.SelectedValue, ConfigModel.sucursal.idSucursal, idPersonal);
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
                MessageBox.Show("Error: " + ex.Message, "determinar Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            finally
            {

                loadState(false);
            }



        }

        private void cargarDescripcionDetalle()
        {

            if (cbxDescripcion.SelectedIndex == -1) return;
            try
            {
                /// Buscando el producto seleccionado
                int idPresentacion = Convert.ToInt32(cbxDescripcion.SelectedValue);

                if (currentProducto == null)
                    currentProducto = listProductos.Find(x => x.idPresentacion == idPresentacion);
                cbxCodigoProducto.SelectedValue = currentProducto.idProducto;
                // Llenar los campos del producto escogido.............!!!!!
                if (!enModificar)
                {
                    txtCantidad.Text = "1";
                    txtDescuento.Text = "0";

                }
                /// Cargando presentaciones           

                /// Cargando alternativas del producto

                determinarDescuentoEImpuesto();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar desc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void calcularPrecioUnitarioDescripcion()
        {
            if (cbxDescripcion.SelectedIndex == -1) return; /// Validación
            try
            {
                if (cbxDescripcion.SelectedIndex == -1)
                {
                    txtPrecioUnitario.Text = darformato(currentProducto.precioVenta);
                }
                else
                {


                    // Realizando el calculo
                    double precioCompra = toDouble(currentProducto.precioVenta);

                    double cantidadUnitario = toDouble(currentProducto.cantidadUnitaria);
                    double precioUnidatio = precioCompra * cantidadUnitario * valorDeCambio;

                    // Imprimiendo valor
                    txtPrecioUnitario.Text = darformato(precioUnidatio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Calcular total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarAlternativasdescripcion()
        {
            try
            {
                /// cargando las alternativas del producto
                alternativaCombinacion = await alternativaModel.cAlternativa31(currentProducto.idPresentacion);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "determinar Alternativas", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                this.Descuento =(double) descuentoTotal;

                lbDescuentoVenta.Text = moneda.simbolo + ". " + darformato(descuentoTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "calcular Descuento", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        #endregion


        #region====================  Eventos===========================================

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
                    {
                        seleccionado = false;

                        btnAgregar.Enabled = true;
                        btnModificar.Enabled = false;
                        enModificar = false;

                        cbxCodigoProducto.Enabled = true;
                        cbxDescripcion.Enabled = true;

                        limpiarCamposProducto();
                    }


            }
        }

        private void dgvDetalleCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            limpiarCamposProducto();
            // Verificando la existencia de datos en el datagridview
            if (dgvDetalleOrdenCompra.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {

                cbxCodigoProducto.Enabled = false;
                enModificar = true;
                int index = dgvDetalleOrdenCompra.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idPresentacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[0].Value);
                int idCombinacion = Convert.ToInt32(dgvDetalleOrdenCompra.Rows[index].Cells[1].Value);
                // obteniedo el idRegistro del datagridview
                currentdetalleV = buscarElemento(idPresentacion, idCombinacion);
                // obteniedo el idRegistro del datagridview
                txtCantidad.Text = darformato(toDouble(currentdetalleV.cantidad));
                cbxCodigoProducto.SelectedValue = currentdetalleV.idProducto;
                cbxDescripcion.Text = currentdetalleV.descripcion;
                txtCantidad.Text = darformato(toDouble(currentdetalleV.cantidad));

                cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;
                txtPrecioUnitario.Text = darformato(currentdetalleV.precioVentaReal);
                txtDescuento.Text = darformato(currentdetalleV.descuento);
                txtTotalProducto.Text = darformato(currentdetalleV.totalGeneral);
                btnAgregar.Enabled = false;
                btnModificar.Enabled = true;

                cbxDescripcion.Enabled = false;
                seleccionado = true;
                cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;
               



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            currentdetalleV.idCombinacionAlternativa = (int)cbxVariacion.SelectedValue;
            currentdetalleV.nombreCombinacion = cbxVariacion.Text;
            currentdetalleV.cantidad = txtCantidad.Text.Trim();
            currentdetalleV.cantidadUnitaria = txtCantidad.Text.Trim();
            double descuento = toDouble(txtDescuento.Text.Trim());
            currentdetalleV.descuento = (decimal)descuento;
            double precioUnitario = toDouble(txtPrecioUnitario.Text.Trim());
            currentdetalleV.precioVentaReal = (decimal) precioUnitario;
            double precioUnitarioDescuento = precioUnitario - (descuento / 100) * precioUnitario;
            currentdetalleV.precioVenta = (decimal)(precioUnitarioDescuento);

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
            currentdetalleV.precioUnitario = (decimal)(precioUnitarioImpuesto);
            currentdetalleV.total = (decimal)(precioUnitarioImpuesto * toDouble(currentdetalleV.cantidad));// utilizar para sacar el subtotal
            currentdetalleV.totalGeneral = (decimal)(precioUnitarioDescuento * toDouble(currentdetalleV.cantidad));//utilizar para sacar el suTotal 


            detalleVBindingSource.DataSource = null;
            detalleVBindingSource.DataSource = detalleVentas;
            dgvDetalleOrdenCompra.Refresh();
            calculoSubtotal();
            descuentoTotal();
            btnAgregar.Enabled = true;
            btnModificar.Enabled = false;
            cbxCodigoProducto.Enabled = true;
            cbxDescripcion.Enabled = true;
            limpiarCamposProducto();
            enModificar = false;
            seleccionado = false;
            limpiarCamposProducto();
            decorationDataGridView();
        }

        public async void agregar()
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
            loadState(true);
            try
            {

                bool seleccionado = false;
                if (!cargoAlternativas)
                {
                    try
                    {
                        alternativaCombinacion = await alternativaModel.cAlternativa31(currentProducto.idPresentacion);
                        alternativaCombinacionBindingSource.DataSource = alternativaCombinacion;
                        cbxVariacion.SelectedIndex = -1;
                        if (!seleccionado)
                            cbxVariacion.SelectedValue = alternativaCombinacion[0].idCombinacionAlternativa;
                        else
                        {

                            cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;
                            txtCantidad.Text = currentdetalleV.cantidad;
                            txtDescuento.Text = darformato(currentdetalleV.descuento);
                            txtPrecioUnitario.Text = darformato(currentdetalleV.precioVenta);
                            txtTotalProducto.Text = darformato(currentdetalleV.total);

                        }
                        if (!nuevo)
                        {
                            if (cbxVariacion.SelectedIndex != -1 && currentdetalleV != null)
                                cbxVariacion.SelectedValue = currentdetalleV.idCombinacionAlternativa;

                        }


                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "cargar fechas del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    finally
                    {
                        cargoAlternativas = true;
                    }





                    if (alternativaCombinacion[0].idCombinacionAlternativa <= 0)
                        calcularPrecioUnitarioProducto();

                    calcularTotal();
                    decorationDataGridView();


                }
                cargoAlternativas = false;
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
                        return;

                    }



                    // Creando la lista
                    detalleV.cantidad = darformato(txtCantidad.Text.Trim());//1
                                                                            //determinamos el stock
                    determinarStock(0);
                    /// Busqueda presentacion
                    detalleV.idDetalleVenta = 0;
                    detalleV.idVenta = 0;
                    detalleV.cantidadUnitaria = darformato(txtCantidad.Text.Trim());
                    detalleV.codigoProducto = cbxCodigoProducto.Text.Trim();
                    detalleV.descripcion = cbxDescripcion.Text;
                    detalleV.stockALmacenLocal = currentProducto.stock;
                    double descuento = toDouble(txtDescuento.Text.Trim());
                    detalleV.descuento = (decimal)(descuento);
                    double precioUnitario = toDouble(txtPrecioUnitario.Text.Trim());

                    detalleV.precioVentaReal = (decimal)(precioUnitario);
                    decimal precioUnitarioDescuento = (decimal)(precioUnitario - (descuento / 100) * precioUnitario);
                    detalleV.precioVenta = (decimal)(precioUnitarioDescuento);

                    // si es que exite impuesto al producto 

                    // impuestoProducto
                    listImpuestosProducto = await impuestoModel.impuestoProducto(currentProducto.idPresentacion, ConfigModel.sucursal.idSucursal);


                    //listImpuesto
                    // calculamos lo impuesto posibles del producto
                    double porcentual = 0;
                    double efectivo = 0;

                    int i = 0;
                    foreach (ImpuestoProducto I in listImpuestosProducto)
                    {
                        if (listIDocumento.Count > 0)
                            if (I.idImpuesto == listIDocumento[i++].idImpuesto)
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

                    }

                    detalleV.Porcentual = darformato(porcentual);
                    detalleV.Efectivo = darformato(efectivo);

                    decimal precioUnitarioI1 = precioUnitarioDescuento;
                    if (porcentual != 0)
                    {
                        double datoaux = (porcentual / 100) + 1;
                        precioUnitarioI1 = precioUnitarioDescuento /(decimal) datoaux;
                    }
                    decimal precioUnitarioImpuesto = precioUnitarioI1 - (decimal)efectivo;
                    detalleV.precioUnitario = precioUnitarioImpuesto;
                    detalleV.total = (precioUnitarioImpuesto * (decimal)toDouble(detalleV.cantidad));// utilizar para sacar el subtotal
                    detalleV.totalGeneral = (decimal)(precioUnitarioDescuento * (decimal)toDouble(detalleV.cantidad));//utilizar para sacar el suTotal
                    detalleV.valor = detalleV.totalGeneral - detalleV.total;
                    // fin cde calculo de necesarios par detalles productos



                    detalleV.estado = 1;//5
                    detalleV.idCombinacionAlternativa = Convert.ToInt32(cbxVariacion.SelectedValue);//7
                    detalleV.idPresentacion = currentProducto.idPresentacion;
                    detalleV.idProducto = currentProducto.idProducto;
                    detalleV.idSucursal = ConfigModel.sucursal.idSucursal;
                    detalleV.nombreCombinacion = cbxVariacion.Text;
                    detalleV.nombreMarca = currentProducto.nombreMarca;
                    detalleV.nombrePresentacion = currentProducto.nombreProducto;
                    detalleV.nro = 1;
                    // determinar el impuesto                 

                    detalleV.eliminar = "";
                    detalleV.existeStock = (stockPresentacion > 0 && stockPresentacion >= Convert.ToInt32(toDouble(txtCantidad.Text.Trim()))) ? 1 : 0;

                    detalleV.nombreMarca = currentProducto.nombreMarca;
                    detalleV.nombrePresentacion = currentProducto.nombreProducto;
                    detalleV.precioEnvio = 0;
                    detalleV.ventaVarianteSinStock = currentProducto.ventaVarianteSinStock;
                    // agrgando un nuevo item a la lista
                    detalleVentas.Add(detalleV);

                    // calcular los descuentos


                    // Refrescando la tabla
                    detalleVBindingSource.DataSource = null;
                    detalleVBindingSource.DataSource = detalleVentas;
                    dgvDetalleOrdenCompra.Refresh();
                   
                    // Calculo de totales y subtotales
                    calculoSubtotal();
                    dgvDetalleOrdenCompra.Refresh();
                    descuentoTotal();
                    dgvDetalleOrdenCompra.DataSource = dgvDetalleOrdenCompra;
                    limpiarCamposProducto();
                    dgvDetalleOrdenCompra.DataSource = detalleVBindingSource;
                    decorationDataGridView();


                    this.cbxCodigoProducto.Focus();
                }
                else
                {

                    MessageBox.Show("Error: elemento no seleccionado", "agregar Elemento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tab = 1;
                    this.cbxCodigoProducto.Focus();

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "agregar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
                if (tab == 0)
                {
                    this.cbxCodigoProducto.Focus();

                } else
                {
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
            determinarDescuentoEImpuesto();
        }

        private void txtPrecioUnitario_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }

        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            calcularTotal();
        }





        private void decorationDataGridView()
        {

            if (dgvDetalleOrdenCompra.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dgvDetalleOrdenCompra.Rows)
            {
                int idPresentacion = Convert.ToInt32(row.Cells[0].Value); // obteniedo el idCategoria del datagridview
                int idCombinacion = Convert.ToInt32(row.Cells[1].Value);
                DetalleV aux = detalleVentas.Find(x => x.idPresentacion == idPresentacion && x.idCombinacionAlternativa == idCombinacion); // Buscando la categoria en las lista de categorias
                if (aux.existeStock == 0)
                {
                    dgvDetalleOrdenCompra.ClearSelection();
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 224);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(250, 5, 73);
                }
            }
        }


        private void limpiarCamposProducto()
        {
            cbxCodigoProducto.SelectedIndex = -1;
            cbxDescripcion.SelectedIndex = -1;
            cbxDescripcion.Text = "";
            cbxVariacion.SelectedIndex = -1;
            txtCantidad.Text = "";
            txtDescuento.Text = "";
            txtPrecioUnitario.Text = "";
            txtTotalProducto.Text = "";

            currentdetalleV = null;
            currentProducto = null;
            this.cbxCodigoProducto.Focus();
        }

        private async void btnComprar_Click(object sender, EventArgs e)
        {

            cobrov.cantidadCuotas = 1;
            cobrov.estado = 1;
            cobrov.estadoCobro = 1;
            cobrov.idCobro = 0;
            cobrov.idMoneda = (int)cbxTipoMoneda.SelectedValue;
            cobrov.interes = 0;
            cobrov.montoPagar = 0;

            cobroVentaV.idCaja = FormPrincipal.asignacion.idCaja;
            cobroVentaV.idCajaSesion = ConfigModel.cajaSesion != null ? ConfigModel.cajaSesion.idCajaSesion : 0;
            cobroVentaV.idMedioPago = 1;
            cobroVentaV.idMoneda = (int)cbxTipoMoneda.SelectedValue;
            cobroVentaV.moneda = cbxTipoMoneda.Text;
            cobroVentaV.pagarVenta = chbxPagarCompra.Checked ? 1 : 0;


            foreach (DetalleV V in detalleVentas)
            {
                DatosNotaSalidaVenta aux = new DatosNotaSalidaVenta();
                aux.cantidad = toEntero(V.cantidad);
                aux.descripcion = V.descripcion;
                aux.idAlmacen = almacenVenta.idAlmacen;
                aux.idCombinacionAlternativa = V.idCombinacionAlternativa;
                aux.idProducto = V.idProducto;


                List<object> list = new List<object>();
                list.Add(V.idProducto);
                list.Add(V.idCombinacionAlternativa);
                list.Add((int)(toDouble(V.cantidad)));
                list.Add(V.ventaVarianteSinStock);

                dato.Add(list);


                datosNotaSalidaVenta.Add(aux);
            }

            notasalidaVenta.datosNotaSalida = datosNotaSalidaVenta;
            notasalidaVenta.generarNotaSalida = chbxNotaEntrada.Checked ? 1 : 0;
            notasalidaVenta.idPersonal = PersonalModel.personal.idPersonal;
            notasalidaVenta.idTipoDocumento = 8; // de nota de salida

            ventav.correlativo = txtCorrelativo.Text.Trim();
            ventav.descuento = darformato(this.Descuento).Replace(",", "");
            ventav.direccion = txtDireccionCliente.Text;
            ventav.documentoIdentificacion = cbxTipoDocumento.Text;
            ventav.editar = chbxEditar.Checked;
            ventav.estado = 1;

            string fechaVenta = String.Format("{0:u}", dtpFechaEmision.Value);
            fechaVenta = fechaVenta.Substring(0, fechaVenta.Length - 1);
            string fechaPago = String.Format("{0:u}", dtpFechaPago.Value);
            fechaPago = fechaPago.Substring(0, fechaPago.Length - 1);
            ventav.fechaPago = fechaPago;
            ventav.fechaVenta = fechaVenta;
            ventav.formaPago = "EFECTIVO";
            ventav.idAsignarPuntoVenta = FormPrincipal.asignacion.idAsignarPuntoVenta;
            ventav.idCliente = CurrentCliente.idCliente;
            ventav.idDocumentoIdentificacion = (int)cbxTipoDocumento.SelectedValue;
            ventav.idPuntoVenta = FormPrincipal.asignacion.idPuntoVenta;
            ventav.idTipoDocumento = (int)cbxNombreDocumento.SelectedValue;
            ventav.idVenta = 0;
            ventav.moneda = cbxTipoMoneda.Text;
            ventav.nombreCliente = cbxNombreRazonCliente.Text;
            ventav.observacion = txtObservaciones.Text;
            ventav.rucDni = txtDocumentoCliente.Text;
            ventav.serie = txtSerie.Text;
            ventav.subTotal = darformato(this.subTotal).Replace(",", "");
            ventav.tipoCambio = 1;
            ventav.tipoVenta = "Con producto";
            ventav.total = darformato(this.total).Replace(",", "");



            // datos para comprobara stock

            verificarStock.dato = dato;
            verificarStock.idPersonal = PersonalModel.personal.idPersonal;
            verificarStock.idSucursal = ConfigModel.sucursal.idSucursal;
            verificarStock.idVenta = 0;

            abastece.dato = dato;
            abastece.idAlmacen = almacenVenta.idAlmacen;
            abastece.idVenta = 0;
            List<verificarStockReceive> verificarStockReceive = await stockModel.verificarstockproductossucursal(verificarStock);

            abasteceReceive = await stockModel.Abastece(abastece);

            if (abasteceReceive.abastece == 0)
            {
                MessageBox.Show("no exite suficiente stock para hacer esta venta", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DialogResult dialog = MessageBox.Show("¿Desea guardar y a se podra modificar", "Venta",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialog == DialogResult.No)
            {

                this.Close();
                return;
            }
            ventaTotal.cobro = cobrov;
            ventaTotal.cobroventa = cobroVentaV;
            ventaTotal.detalle = detalleVentas;
            ventaTotal.notasalida = notasalidaVenta;
            ventaTotal.venta = ventav;

            ResponseVenta response = await ventaModel.guardar(ventaTotal);

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
        private void cbxProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxNombreRazonCliente.SelectedIndex == -1) return;

            CurrentCliente = listClientes.Find(X => X.idCliente == (int)cbxNombreRazonCliente.SelectedValue);

            datosClientes();

            determinarDescuento();


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
            actulizar = true;
            actulizarcliente = true;
            loadState(true);
            cargarClientes();
            cargarProductos();
            

            btnAgregar.Enabled = true;
            btnModificar.Enabled = false;
            enModificar = false;
            cbxCodigoProducto.Enabled = true;
            cbxDescripcion.Enabled = true;
            seleccionado = false;
            limpiarCamposProducto();

            if (currentCotizacion != null)
            {
                cargarCotizacion();
            }
        }

        private void btnNuevoProducto_Click(object sender, EventArgs e)
        {
            actulizar = true;
            loadState(true);
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

        private async void txtDni_TextChanged(object sender, EventArgs e)
        {
            String aux = txtDocumentoCliente.Text;

            int nroCaracteres = aux.Length;
            bool exiteCliente = false;


            if (nroCaracteres == txtDocumentoCliente.MaxLength)
            {
                try
                {
                    CurrentCliente = listClientes.Find(X => X.numeroDocumento == aux);
                    if (CurrentCliente != null)
                    {
                        exiteCliente = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Buscar Cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (exiteCliente)
                {
                    //Si existe el cliente, llenamos los datos correspondientes
                    datosClientes();
                }
                else
                {
                    //Si no existe el cliente, lo buscamos en la base de Datos (por seguridad de concurrencia)
                    //El servicio pide como parametro un cliente (corregir) y retorna una lista de Clientes
                    Cliente clienteBuscar = new Cliente();
                    clienteBuscar.numeroDocumento = aux;
                    List<Cliente> listaClienteBuscados= await clienteModel.BuscarClienteDocumento(clienteBuscar);
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
                        //No existe el cliente en la base de datos
                        //llenamos los datos en FormproveerdorNuevo
                        FormClienteNuevo formClienteNuevo = new FormClienteNuevo(aux, (int)cbxTipoDocumento.SelectedValue);
                        formClienteNuevo.ShowDialog();

                        Response response = formClienteNuevo.rest;
                        if (response != null)
                        {
                            if (response.id > 0)
                            {
                                listClientes = await clienteModel.ListarClientesActivos();
                                clienteBindingSource.DataSource = listClientes;
                                CurrentCliente = listClientes.Find(X => X.idCliente == response.id);
                                datosClientes();
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
                    
                     
                }
            }

        }



        private void datosClientes()
        {
            if (CurrentCliente != null)
            {

                txtDocumentoCliente.removePlaceHolder();
                txtDocumentoCliente.Text = CurrentCliente.numeroDocumento;
                txtDireccionCliente.Text = CurrentCliente.direccion;
                cbxNombreRazonCliente.Text = CurrentCliente.nombreCliente;
                cbxTipoDocumento.SelectedValue = CurrentCliente.idDocumento;

            }
            else
            {
                int i = 0; 
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
            Buscarcliente buscarCliente = new Buscarcliente();
            buscarCliente.ShowDialog();
            if (buscarCliente.currentCliente != null)
            {
                establecerListaCliente(buscarCliente.clientes);
                this.CurrentCliente = buscarCliente.currentCliente;
                //txtDocumentoCliente.Text = CurrentCliente.numeroDocumento;
                datosClientes();
            }
            else
            {
                if (this.CurrentCliente == null)
                {
                    //limpiarCamposCliente();
                    limpiarClienteNoDNI();
                }
                else
                {
                    datosClientes();
                }
            }
            //determinarDescuento();
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



        private async void cbxTipoComprobante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxNombreDocumento.SelectedIndex == -1) return;

            if (tipoDocumentos == null || documentoIdentificacion == null) return;
            if (nuevo)
            {
                int idTipoDocumento = (int)cbxNombreDocumento.SelectedValue;
                TipoDocumento tipoDocumento = tipoDocumentos.Find(X => X.idTipoDocumento == idTipoDocumento);

                if (tipoDocumento.tipoCliente == 2)
                {

                    try
                    {

                        DocumentoIdentificacion dd = documentoIdentificacion.Find(X => X.tipoDocumento == "Jurídico");
                        cbxTipoDocumento.SelectedValue = dd.idDocumento;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "buscar Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                    }


                }
                else
                {
                    if (tipoDocumento.tipoCliente == 1)
                    {

                        DocumentoIdentificacion dd = documentoIdentificacion.Find(X => X.tipoDocumento == "Natural");
                        cbxTipoDocumento.SelectedValue = dd.idDocumento;


                    }


                }

                cargarImpuesto((int)cbxNombreDocumento.SelectedValue);
                List<Venta_correlativo> list = await ventaModel.listarNroDocumentoVenta(idTipoDocumento, ConfigModel.asignacionPersonal.idPuntoVenta);
                txtCorrelativo.Text = list[0].correlativoActual;
                txtSerie.Text = list[0].serie;



            }
            cargarFormatoDocumento((int)cbxNombreDocumento.SelectedValue);

            loadState(true);
            try
            {
                listIDocumento = await impuestoModel.impuestoTipoDoc(ConfigModel.sucursal.idSucursal, (int)cbxNombreDocumento.SelectedValue);
                if (detalleVentas != null)
                {

                    foreach (DetalleV dv in detalleVentas)
                    {
                        decimal precioUnitario = dv.precioVentaReal;
                        decimal descuento = dv.descuento;


                        decimal precioUnitarioDescuento = precioUnitario - (descuento / 100) * precioUnitario;
                        dv.precioVenta = precioUnitarioDescuento;

                        listImpuestosProducto = await impuestoModel.impuestoProducto(dv.idPresentacion, ConfigModel.sucursal.idSucursal);


                        //listImpuesto
                        // calculamos lo impuesto posibles del producto
                        double porcentual = 0;
                        double efectivo = 0;

                        int i = 0;
                        foreach (ImpuestoProducto I in listImpuestosProducto)
                        {
                            if (listIDocumento.Count > 0)
                                if (I.idImpuesto == listIDocumento[i++].idImpuesto)
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

                        }

                        dv.Porcentual = darformato(porcentual);
                        dv.Efectivo = darformato(efectivo);

                        decimal precioUnitarioI1 = precioUnitarioDescuento;
                        if (porcentual != 0)
                        {
                            double datoaux = (porcentual / 100) + 1;
                            precioUnitarioI1 = precioUnitarioDescuento / (decimal)datoaux;
                        }
                        decimal precioUnitarioImpuesto = precioUnitarioI1 - (decimal)efectivo;
                        dv.precioUnitario = precioUnitarioImpuesto;
                        dv.total = precioUnitarioImpuesto * (decimal)toDouble(dv.cantidad);// utilizar para sacar el subtotal
                        dv.totalGeneral = precioUnitarioDescuento * (decimal)toDouble(dv.cantidad);//utilizar para sacar el suTotal
                        dv.valor =dv.totalGeneral - dv.total;


                    }

                    dgvDetalleOrdenCompra.DataSource = null;
                    dgvDetalleOrdenCompra.DataSource = detalleVentas;

                    calculoSubtotal();
                    decorationDataGridView();

                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "buscar cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }
            finally
            {
                if (currentCotizacion != null)
                {

                    int idTipoDocumento = (int)cbxTipoDocumento.SelectedValue;
                    DocumentoIdentificacion tipoDocumento = documentoIdentificacion.Find(X => X.idDocumento == idTipoDocumento);

                    if (txtDocumentoCliente.Text.Length != tipoDocumento.numeroDigitos)
                    {
                        cbxNombreRazonCliente.SelectedIndex = -1;
                        cbxNombreRazonCliente.SelectedValue = currentCotizacion.idCliente;
                    }
                }
                loadState(false);

            }
            
            
        }

        private void btnImportarCotizacion_Click(object sender, EventArgs e)
        {

            ImportarCotizacion();

        }

        public void ImportarCotizacion()
        {
            loadState(true);
            FormBuscarCotizacion formBuscarCotizacion = new FormBuscarCotizacion();
            formBuscarCotizacion.ShowDialog();


            currentCotizacion = formBuscarCotizacion.currentCotizacion;


            if (currentCotizacion != null)
            {

                cargarDatosCotizacion();

            }

        }


        private async  Task cargarAldelanto()
        {

            if (currentCotizacion != null)
            {
                if (currentCotizacion.tipo == "Pedido")
                {


                    // hacer de esta algo manual que se pueda ver o al momento de hacer la venta 
                    try
                    {

                        List<AdelantoCotizacion> list = await cotizacionModel.AdelantoSinUso(currentCotizacion.idCotizacion);
                        MontoPedidoTotal = toDouble(currentCotizacion.total);
                        if (list.Count > 0)
                        {
                            double monto = list[0].saldo;
                            if (monto > 0)
                            {
                                Moneda moneda = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);
                                FormUsarAdelanto usarAdelanto = new FormUsarAdelanto(monto, this.total);
                                usarAdelanto.ShowDialog();
                                if (usarAdelanto.guardar)
                                {
                                    MontoAdelanto = usarAdelanto.adelanto;
                                    cobroPedido = list[0].idCobro;
                                    double saldo = this.total - MontoAdelanto;
                                    lbUsado.Text = moneda.simbolo + ". " + darformato(MontoAdelanto);
                                    lbSaldo.Text = moneda.simbolo + ". " + darformato(saldo);
                                }
                                else
                                {
                                    MontoAdelanto = 0;
                                }
                            }
                            

                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Error: " + ex.Message, "cargar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }



                }



            }
        }
        public  void cargarPedido ()
        {

           
            if (currentCotizacion != null)
            {
               

                cargarDatosCotizacion();

            }

        }

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
                        MessageBox.Show("Error: Producto seleccionado no tiene precio de venta, no se puede seleccionar", "Buscar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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



        private void cargarDatosCotizacion()
        {
            if(currentCotizacion==null)
                cbxNombreRazonCliente.SelectedValue = currentCotizacion.idCliente;
            cbxTipoMoneda.Text = currentCotizacion.moneda;
          
            cargarCotizacion();

            // resultados
            Moneda moneda = monedas.Find(X => X.moneda == cbxTipoMoneda.Text);
            cbxTipoMoneda.Text = currentCotizacion.moneda;          
            this.Descuento = toDouble(currentCotizacion.descuento);

            lbDescuentoVenta.Text = moneda.simbolo + ". " + darformato(Descuento);

            this.total = toDouble(currentCotizacion.total);
            lbTotal.Text = moneda.simbolo + ". " + darformato(total);

            this.subTotal = toDouble(currentCotizacion.subTotal);
            lbSubtotal.Text = moneda.simbolo + ". " + darformato(subTotal);
            double impuesto = total - subTotal;
            lbImpuesto.Text = moneda.simbolo + ". " + darformato(impuesto);
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
                    if (tipoDocumentos == null || listClientes ==null) return; 

                    TipoDocumento tipoDocumento2 = tipoDocumentos.Find(X => X.tipoCliente == 2);
                    cbxNombreDocumento.SelectedValue = tipoDocumento2.idTipoDocumento;
                    if (cbxNombreRazonCliente.SelectedIndex == -1)
                    {  txtDni_TextChanged(null, null); return;  }
                    Cliente cliente = listClientes.Find(X => X.idDocumento == tipoDocumento.idDocumento && X.idCliente == (int)cbxNombreRazonCliente.SelectedValue);

                    if (cliente == null)
                    {
                        limpiarCamposCliente();
                    }
                }
                else
                {

                    if (tipoDocumentos == null || listClientes == null) return;

                    TipoDocumento tipoDocumento2 = tipoDocumentos.Find(X => X.tipoCliente == 1);
                    cbxNombreDocumento.SelectedValue = tipoDocumento2.idTipoDocumento;
                    if (cbxNombreRazonCliente.SelectedIndex == -1)
                    { txtDni_TextChanged(null, null); return; }
                    Cliente cliente = listClientes.Find(X => X.idDocumento == tipoDocumento.idDocumento && X.idCliente == (int)cbxNombreRazonCliente.SelectedValue);
                    if (cliente == null)
                    {

                        limpiarCamposCliente();
                    }
                }
            }

            // solo para recargar  si no hay cliente seleccionado

            if (currentCotizacion != null)
            {
               
                    int idTipoDocumento = (int)cbxTipoDocumento.SelectedValue;
                    DocumentoIdentificacion tipoDocumento = documentoIdentificacion.Find(X => X.idDocumento == idTipoDocumento);

                    if (txtDocumentoCliente.Text.Length != tipoDocumento.numeroDigitos)
                    {
                            cbxNombreRazonCliente.SelectedIndex = -1;
                            cbxNombreRazonCliente.SelectedValue = currentCotizacion.idCliente;
                    }
                       
                  

               

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

        private void cbxVariacion_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cbxVariacion.SelectedIndex == -1) return;
            if (cbxCodigoProducto.SelectedIndex == -1) return;
            AlternativaCombinacion alternativa = alternativaCombinacion.Find(X => X.idCombinacionAlternativa == (int)cbxVariacion.SelectedValue);
            currentProducto = listProductos.Find(X => X.idProducto == (int)cbxCodigoProducto.SelectedValue);
            double precioUnitario =toDouble( currentProducto.precioVenta)+ toDouble(alternativa.precio);
            txtPrecioUnitario.Text = darformato(precioUnitario*valorDeCambio);
            determinarStock(0);
        }

        private  void btnVenta_Click(object sender, EventArgs e)
        {
           hacerVenta();
        }


        public async  void hacerVenta()
        {
            
            loadState(true);

            try
            {
                await configModel.loadCajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);

            }
            catch(Exception ex)
            {
                
            }
           
            int i = 0;
            if (detalleVentas == null)
            {
                detalleVentas = new List<DetalleV>();
            }
            
            try
            {

                limpiarObjetos();
                if (CurrentCliente == null)
                {

                    MessageBox.Show("Error: " + " cliente no seleccionado", "cliente ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    loadState(false);
                    faltaCliente = true;
                    cbxTipoDocumento.Select();
                    cbxTipoDocumento.Focus();

                    return;

                }
                if (detalleVentas.Count == 0)
                {
                    MessageBox.Show("Error: " + " Productos no seleccionados", "Productos ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    loadState(false);
                    faltaProducto = true;
                    cbxCodigoProducto.Focus();
                    return;

                }
                if (currentCotizacion != null)
                {
                    if (MontoAdelanto == 0)
                    {
                        await cargarAldelanto();
                    }
                }
                i++;
                cobrov.cantidadCuotas = 1;
                cobrov.estado = 1;
                cobrov.estadoCobro = 1;
                cobrov.idCobro = esPedido? cobroPedido:0;
                cobrov.idMoneda = (int)cbxTipoMoneda.SelectedValue;
                i++;
                cobrov.interes = 0;
                cobrov.montoPagar = esPedido ? this.MontoPedidoTotal : 0;
                i++;

                cobroVentaV.idCaja = ConfigModel.asignacionPersonal.idCaja;
                i++;
                cobroVentaV.idCajaSesion = ConfigModel.cajaIniciada  ? ConfigModel.cajaSesion.idCajaSesion : 0;
                i++;
                cobroVentaV.idMedioPago = 1;// efectivo
                cobroVentaV.idMoneda = (int)cbxTipoMoneda.SelectedValue;
                cobroVentaV.moneda = cbxTipoMoneda.Text;

                if (!ConfigModel.cajaIniciada)
                {
                    chbxPagarCompra.Checked = false;
                }
                cobroVentaV.pagarVenta = chbxPagarCompra.Checked ? 1 : 0;
                cobroVentaV.montoPagarPedido = chbxPagarCompra.Checked ? total-this.MontoAdelanto : 0;

                foreach (DetalleV V in detalleVentas)
                {

                    List<object> list = new List<object>();
                    List<object> listAbastece = new List<object>();                   
                    list.Add(V.idProducto);
                    list.Add(V.idCombinacionAlternativa);
                    list.Add((int)(toDouble(V.cantidad)));
                    list.Add(V.ventaVarianteSinStock);

                    listAbastece.Add(V.idProducto);
                    listAbastece.Add(V.idCombinacionAlternativa);
                    listAbastece.Add(V.idPresentacion);
                    listAbastece.Add((int)(toDouble(V.cantidad)));
                    listAbastece.Add(V.ventaVarianteSinStock);



                    dato.Add(list);

                    datoaAbastece.Add(listAbastece);                  
                    DatosNotaSalidaVenta aux = new DatosNotaSalidaVenta();
                    aux.descripcion = V.descripcion;
                    aux.idAlmacen = ConfigModel.currentIdAlmacen;
                    aux.idCombinacionAlternativa = V.idCombinacionAlternativa;
                    aux.idProducto = V.idProducto;
                    aux.idPresentacion = V.idPresentacion;
                    aux.cantidad = (int)(toDouble(V.cantidad));
                    datosNotaSalidaVenta.Add(aux);
                    
                }
                i++;
                //FormAsignarDetalleVenta formAsignar = new FormAsignarDetalleVenta(datosNotaSalidaVenta);


       

                ventav.correlativo = txtCorrelativo.Text.Trim();
                ventav.descuento = darformato(this.Descuento).Replace(",", "");
                ventav.direccion = txtDireccionCliente.Text;
                ventav.documentoIdentificacion = cbxTipoDocumento.Text;
                ventav.editar = chbxEditar.Checked;
                ventav.estado = 1;

                string fechaVenta = String.Format("{0:u}", dtpFechaEmision.Value);
                fechaVenta = fechaVenta.Substring(0, fechaVenta.Length - 1);
                string fechaPago = String.Format("{0:u}", dtpFechaPago.Value);
                fechaPago = fechaPago.Substring(0, fechaPago.Length - 1);
                ventav.fechaPago = fechaPago;
                ventav.fechaVenta = fechaVenta;
                ventav.formaPago = "EFECTIVO";
                ventav.idAsignarPuntoVenta = FormPrincipal.asignacion.idAsignarPuntoVenta;
                ventav.idCliente = CurrentCliente.idCliente;
                ventav.idDocumentoIdentificacion = (int)cbxTipoDocumento.SelectedValue;
                ventav.idPuntoVenta = FormPrincipal.asignacion.idPuntoVenta;
                ventav.idTipoDocumento = (int)cbxNombreDocumento.SelectedValue;
                ventav.idVenta = 0;
                ventav.moneda = cbxTipoMoneda.Text;
                ventav.nombreCliente = cbxNombreRazonCliente.Text;
                ventav.observacion = txtObservaciones.Text;
                ventav.rucDni = txtDocumentoCliente.Text;
                ventav.serie = txtSerie.Text;
                ventav.subTotal = darformato(this.subTotal).Replace(",", "");
                ventav.tipoCambio = 1;
                ventav.tipoVenta = "Con producto";
                ventav.total = darformato(this.total).Replace(",", "");
                // datos para comprobara stock

                verificarStock.dato = dato;
                int idPersonal = PersonalModel.personal.idPersonal;
                if (ConfigModel.currentIdAlmacen == 0)
                {
                    idPersonal = 0;

                }
                i++;
                verificarStock.idPersonal = idPersonal;
                verificarStock.idSucursal = ConfigModel.sucursal.idSucursal;
                verificarStock.idVenta = 0;

                abastece.dato = dato;
                abastece.idAlmacen = almacenVenta.idAlmacen;
                abastece.idVenta = 0;
                List<verificarStockReceive> verificarStockReceive = await stockModel.verificarstockproductossucursal(verificarStock);

                i++;
                if (chbxNotaEntrada.Checked)
                {

                    abasteceReceive = await stockModel.Abastece(abastece);
                    dato.Clear();
                    if (abasteceReceive.abastece == 0)
                    {


                        if (listAlmecenes.Count > 1)
                        {
                            datosVentaAbastece.idAlmacen = ConfigModel.currentIdAlmacen;
                            datosVentaAbastece.idPersonal = PersonalModel.personal.idPersonal;
                            datosVentaAbastece.idSucursal = ConfigModel.sucursal.idSucursal;
                            datosVentaAbastece.idVenta = 0;
                            datosVentaAbastece.dato = datoaAbastece;

                            List<DatosNotaSalidaVenta> listNota = await ventaModel.verificarabastecealmacenventa(datosVentaAbastece);
                            int cantidadRestante = 0;
                            bool hayStock = true;
                            foreach (DetalleV V in detalleVentas)
                            {
                                List<DatosNotaSalidaVenta> listAux = listNota.Where(X => X.idPresentacion == V.idPresentacion && X.idCombinacionAlternativa == V.idCombinacionAlternativa).ToList();
                                DatosNotaSalidaVenta notaPrincipal = listNota.Find(X => X.idPresentacion == V.idPresentacion && X.idCombinacionAlternativa == V.idCombinacionAlternativa && X.idAlmacen == ConfigModel.currentIdAlmacen);

                                if (listAux.Count == 0) continue;
                                int cantidadAlmacen = (int)notaPrincipal.stock;
                                int cantidad =(int)toDouble(V.cantidad);
                                cantidadRestante = cantidad - cantidadAlmacen;
                                bool saltar = false;

                                //ver si no exite stock en ninguna almacen

                                foreach (DatosNotaSalidaVenta nota in listAux)
                                {
                                    if (nota.cantidad == 0 && nota.stock == 0 && nota.stockTotal == 0)
                                        hayStock = false;
                                    else
                                    {
                                        hayStock = true;
                                    }

                                    nota.descripcion = V.descripcion;
                                    if (nota.idAlmacen != ConfigModel.currentIdAlmacen)
                                    {
                                        cantidadRestante -= (int)nota.stock;
                                        if (cantidadRestante < 0)
                                        {
                                            if (!saltar)
                                            {
                                                nota.stockGuardar = Math.Abs(cantidadRestante);
                                                cantidadRestante += (int)nota.stock;
                                                nota.stock = (cantidadRestante);
                                                cantidadRestante = 0;
                                                saltar = true;
                                            }
                                            else
                                            {
                                                nota.stock = 0;
                                                nota.stockGuardar = (int)nota.stockTotal;
                                            }

                                        }
                                        if (cantidadRestante == 0)
                                        {
                                            saltar = true;

                                        }
                                        nota.cantidadVentaRestante = 0;

                                    }
                                    nota.cantidadVenta = Decimal.Parse(V.cantidad);
                                }

                            }

                            if(hayStock == false)
                            {
                                MessageBox.Show("No exite stock Suficiente en los Almacenes Asignados", "Verificar stock", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;

                            }
                            FormAsignarDetalleVenta formAsignar = new FormAsignarDetalleVenta(listNota);
                            formAsignar.ShowDialog();
                            if (formAsignar.salir)
                            {
                                loadState(false);
                                return;

                            }

                            listNota = formAsignar.list;

                            List<DatosNotaSalidaVenta> listaux = listNota.GroupBy(test => test.idPresentacion)
                                                                                                           .Select(grp => grp.First())
                                                                                                           .ToList();


                            foreach (DatosNotaSalidaVenta ListA in listaux)
                            {

                                int idPresentacion = ListA.idPresentacion;
                                int idCombinacion = ListA.idCombinacionAlternativa;
                                DatosNotaSalidaVenta aux = datosNotaSalidaVenta.Find(X => X.idPresentacion == idPresentacion && X.idCombinacionAlternativa == idCombinacion);
                                if (aux != null)
                                {

                                    datosNotaSalidaVenta.Remove(aux);

                                }

                            }
                            datosNotaSalidaVenta.AddRange(listNota);


                        }
                        else
                        {
                            if (listAlmecenes.Count == 1)
                            {
                                MessageBox.Show("No exite stock Suficiente en el Almacen", "Verificar stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;

                            }

                        } 
                       
                    }
                }
                i++;

                DialogResult dialog = MessageBox.Show("¿Desea guardar, ya no se podra modificar?", "Venta",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dialog == DialogResult.No)
                {

                    this.Close();
                    return;
                }


                if (currentCotizacion != null)
                {
                    if (currentCotizacion.tipo == "Pedido")
                    {

                        try
                        {
                            Response res = await cotizacionModel.AdelantoCambiarEstado(currentCotizacion.idCotizacion,this.MontoAdelanto);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERRROR: " + ex.Message, "Cambiar estado de adelanto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                        //nota de salida

                notasalidaVenta.datosNotaSalida = datosNotaSalidaVenta;
                notasalidaVenta.generarNotaSalida = chbxNotaEntrada.Checked ? 1 : 0;
                notasalidaVenta.idPersonal = PersonalModel.personal.idPersonal;
                notasalidaVenta.idTipoDocumento = 8; // de nota de salida

                ventaTotal.cobro = cobrov;
                ventaTotal.cobroventa = cobroVentaV;
                ventaTotal.detalle = detalleVentas;
                ventaTotal.notasalida = notasalidaVenta;
                ventaTotal.venta = ventav;

                ResponseVenta response = await ventaModel.guardar(ventaTotal);

                if (response.id > 0)
                {
                    List<cobroVenta> cobroVentas = await cobroModel.CobroVenta(response.id);
                    // ver si se puede hacer el adelanto de venta
                    if (!chbxPagarCompra.Checked && ConfigModel.cajaIniciada)
                    {
                        double total = toDouble(txtTotalAdelantos.Text.Trim());
                        if (total > 0)
                        {
                            crearObjetoCobroDetalle(cobroVentas[0].idCobro);
                            Response responsecobro = await cobroModel.guardarCobroDetalle(currentSaveObject);

                        }
                            
                    }
                    actulizar = true;
                    cargarProductos();
                    if (nuevo)
                    {
                        MessageBox.Show(response.msj+ "  Satisfatoriamente", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                    }
                    else
                    {
                        MessageBox.Show(response.msj+" Satisfariamente", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }

                    DialogResult dialog2 = MessageBox.Show("¿Desea salir?", "Venta",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialog2 == DialogResult.Yes)
                    {

                        this.Close();

                    }
                    else
                    {

                   
                        // limpiar y recargarProductos

                        CurrentCliente = null;
                        limpiarCamposCliente();
                        cbxNombreDocumento.SelectedIndex = -1;
                        cbxNombreDocumento.SelectedValue = 3;// factura por defecto
                        Moneda moneda = monedas.Find(X => X.porDefecto);
                        cbxTipoMoneda.SelectedValue = moneda.idMoneda;
                        detalleVentas.Clear();
                        detalleVBindingSource.DataSource = null;
                        detalleVBindingSource.DataSource = detalleVentas;
                        dgvDetalleOrdenCompra.DataSource = detalleVBindingSource;
                    
                        dgvDetalleOrdenCompra.Refresh();
                    
                        calcularDescuento();
                        calculoSubtotal();


                    }



                }
                else
                {
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                  
                }


               

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message+" "+i, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            finally
            {
                
                loadState(false);
            }
            

        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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
                            if (((this.Controls.Find("txt" + doc.value, true).First() as TextBox) != null))
                            {
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

                        e.Graphics.DrawString(doc.value, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(XI, Y));
                        dictionary.Add(doc.value, new Point(XI, Y));                     
                        XI += X + (int)(doc.w);
                        break;
                    case "Img":

                        Image image = Resources.logo1;

                        e.Graphics.DrawImage(image, doc.x, doc.y, (int)doc.w, (int)doc.h);

                        break;

                }


            }

            KeyValuePair<string, Point> primero  = dictionary.First();
            Point point = primero.Value;
            int YI = point.Y + 30;
            Point point1 = new Point();

            if (detalleVentas == null) detalleVentas = new List<DetalleV>();



            for (int i = numberOfItemsPrintedSoFar; i < detalleVentas.Count; i++)
            {
                numberOfItemsPerPage++;

                if (numberOfItemsPerPage <= 20)
                {
                    numberOfItemsPrintedSoFar++;

                    if (numberOfItemsPrintedSoFar <= detalleVentas.Count)
                    {

                        if (dictionary.ContainsKey("codigoProducto"))
                        {

                            point1 = dictionary["codigoProducto"];
                            e.Graphics.DrawString(detalleVentas[i].codigoProducto, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }

                        if (dictionary.ContainsKey("nombreCombinacion"))
                        {
                            point1 = dictionary["nombreCombinacion"];
                            e.Graphics.DrawString(detalleVentas[i].nombreCombinacion, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));

                        }
                        if (dictionary.ContainsKey("cantidad"))
                        {
                            point1 = dictionary["cantidad"];
                            e.Graphics.DrawString(detalleVentas[i].cantidad, new Font("Arial",8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }

                        if (dictionary.ContainsKey("nombrePresentacion"))
                        {
                            point1 = dictionary["nombrePresentacion"];
                            e.Graphics.DrawString(detalleVentas[i].nombrePresentacion, new Font("Arial",8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }
                        if (dictionary.ContainsKey("descripcion"))
                        {
                            point1 = dictionary["descripcion"];
                            e.Graphics.DrawString(detalleVentas[i].descripcion, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));

                        }
                        if (dictionary.ContainsKey("nombreMarca"))
                        {
                            point1 = dictionary["nombreMarca"];
                            e.Graphics.DrawString(detalleVentas[i].nombreMarca, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }
                        if (dictionary.ContainsKey("precioUnitario"))
                        {
                            point1 = dictionary["precioUnitario"];
                            e.Graphics.DrawString(darformato( detalleVentas[i].precioUnitario), new Font("Arial",8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }

                        if (dictionary.ContainsKey("total"))
                        {
                            point1 = dictionary["total"];


                            e.Graphics.DrawString(darformato( detalleVentas[i].total), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));
                        }
                        if (dictionary.ContainsKey("precioVenta"))
                        {

                            point1 = dictionary["precioVenta"];
                            e.Graphics.DrawString(darformato( detalleVentas[i].precioVenta), new Font("Arial",8, FontStyle.Regular), Brushes.Black, new Point(point1.X, YI));

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
                                    e.Graphics.DrawString( textBox.Text, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(doc.x , doc.y));
                            }

                        break;
                }
            }

            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {

            if(listformato== null)
            {
                MessageBox.Show("no exite un formato, para este tipo de documento", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            FormatoDocumento doc = listformato.Last();
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("tamaño pagina", (int)doc.w, (int)doc.h);

            // pre visualizacion
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            printDocument1.Print();

        }


        #endregion========================================================

        private void cbxCodigoProducto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
                if (esVentaRapida)
                    {

                        this.cbxCodigoProducto.Focus();
                    }

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
        }

       

        private void txtCantidad_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void cbxVariacion_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtPrecioUnitario_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtDescuento_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtTotalProducto_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {

                this.SelectNextControl((Control)sender, true, true, true, true);
                this.btnAgregar.Focus();
            }
        }

        private void btnAgregar_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        

        private void btnAgregar_TabIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dgvDetalleOrdenCompra_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.cbxCodigoProducto.Focus();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbxTipoDocumento_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            if (faltaCliente)
            {

                faltaCliente = false;
            }


        }

        private void txtDocumentoCliente_KeyUp(object sender, KeyEventArgs e)
        {            
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void cbxNombreRazonCliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtDireccionCliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void cbxNombreDocumento_ForeColorChanged(object sender, EventArgs e)
        {

        }

        private void cbxNombreDocumento_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void chbxEditar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtCorrelativo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void cbxTipoMoneda_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void dtpFechaEmision_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void dtpFechaPago_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void btnImportarCotizacion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txtObservaciones_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }


        public decimal cambiarValor( decimal valor, double valorCambio)
        {

            return  valor * (decimal)valorCambio;
        }
        
        private async void cbxTipoMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTipoMoneda.SelectedIndex == -1)
                return;

          
                Moneda monedaCambio = monedas.Find(X => X.idMoneda == (int)cbxTipoMoneda.SelectedValue);

            if (monedaActual.idMoneda == monedaCambio.idMoneda)
                return;
                CambioMoneda cambio = new CambioMoneda();
                cambio.idMonedaActual = monedaActual.idMoneda;
                cambio.idMonedaCambio = monedaCambio.idMoneda;
                


               ValorcambioMoneda valorcambioMoneda=await monedaModel.cambiarMoneda(cambio);

                valorDeCambio = toDouble(valorcambioMoneda.cambioMonedaCambio) / toDouble(valorcambioMoneda.cambioMonedaActual);

                if (nuevo)
                {
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

                        txtPrecioUnitario.Text =  darformato( cambiarValor( (decimal)  toDouble(txtPrecioUnitario.Text), valorDeCambio));


                    }
                }
                monedaActual = monedaCambio;


            
         

            //Moneda moneda=monedas.Find(X.)


            //cambio 

            //cambiarMoneda(Valorcambio param)


            // cambiar de moneda
            //post//valormonedas

        }

        private void txtDocumentoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void cbxCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && esVentaRapida)
            {
                esVentaRapida = true; //definida por el usuario
            ComboBox comboBox = (ComboBox)sender;
            string text = comboBox.Text;

            AlternativaCombinacion alternativaCombinacions = listAltenativas.Find(X => X.codigoSku == text);
                // no tiene combinacionaciones 
                if (alternativaCombinacions == null)
                {
                    ProductoVenta productoaux = listProductos.Find(X => X.codigoProducto == text);
                    // nose encontro el producto 
                    if (productoaux == null)
                    {

                        return;
                    }
                    else
                    {
                          //verificamos si el producot esta agregado al detallev
                          DetalleV aux = buscarElemento( Convert.ToInt32(productoaux.idPresentacion), 0);

                        if (aux != null)
                        {
                            aux.cantidad = darformato(toDouble( aux.cantidad)+1);
                            aux.cantidadUnitaria = darformato(toDouble(aux.cantidadUnitaria) + 1);
                            
                            // Refrescando la tabla
                            detalleVBindingSource.DataSource = null;
                            detalleVBindingSource.DataSource = detalleVentas;
                            dgvDetalleOrdenCompra.Refresh();

                            // Calculo de totales y subtotales
                            calculoSubtotal();

                            descuentoTotal();

                            limpiarCamposProducto();
                            decorationDataGridView();


                            this.cbxCodigoProducto.Focus();
                        }
                        else
                        {

                            agregar();
                        }


                    }

                }
                else
                {
                    cbxDescripcion.SelectedValue = alternativaCombinacions.idPresentacion;
                    cbxVariacion.SelectedValue = alternativaCombinacions.idCombinacionAlternativa;

                    DetalleV aux = buscarElemento(alternativaCombinacions.idPresentacion, alternativaCombinacions.idCombinacionAlternativa);
                    if (aux != null)
                    {
                        aux.cantidad = darformato(toDouble(aux.cantidad) + 1);
                        aux.cantidadUnitaria = darformato(toDouble(aux.cantidadUnitaria) + 1);
                        // Refrescando la tabla
                        detalleVBindingSource.DataSource = null;
                        detalleVBindingSource.DataSource = detalleVentas;
                        dgvDetalleOrdenCompra.Refresh();

                        // Calculo de totales y subtotales
                        calculoSubtotal();

                        descuentoTotal();

                        limpiarCamposProducto();
                        decorationDataGridView();


                        this.cbxCodigoProducto.Focus();
                    }
                    else
                    {

                        agregar();
                    }
                }

            }
        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {
            statuschbxActivar();
        }

        private void statuschbxActivar()
        {
            if (chbxActivar.Checked)
            {
                esVentaRapida = true;
            }
            else
            {
                esVentaRapida = false;
            }

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chbxPagarCompra_OnChange(object sender, EventArgs e)
        {

            verAdelanto();


        }

        private void verAdelanto()
        {
            if (chbxPagarCompra.Checked)
            {

                plAdelanto.Visible = false;
                pictureBox1.Visible = true;
            }
            else
            {

                if (ConfigModel.cajaIniciada)
                {
                    plAdelanto.Visible = true;
                }
                else
                {
                    plAdelanto.Visible = false;
                }
                
                pictureBox1.Visible = false;
            }
        }

        private void btnAdelanto_Click(object sender, EventArgs e)
        {

            DialogResult dialog = MessageBox.Show("¿Está seguro de hacer adelanto?", "Agregrar Adelanto",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialog == DialogResult.No) return;
            double adelantoaux = toDouble(txtAdelanto.Text.Trim());
            double adelantosA= toDouble(txtTotalAdelantos.Text.Trim());
            if (adelantoaux + adelantosA > total)
            {

                double resto = total - adelantosA;
                txtAdelanto.Text = darformato(resto);
                txtAdelanto.Focus();
              
                MessageBox.Show("La suma de los  adelantos excenden al total  ", "Comprobar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAdelanto.Focus();


            }
            else
            {
               
                adelantosA += toDouble(txtAdelanto.Text);
                // actualizamos los adelantos en uc 
                txtAdelanto.Text = darformato(0);
                txtTotalAdelantos.Text = darformato(adelantosA);
                
                MessageBox.Show("El Adelanto Actual es de: " + adelantosA, "Comprobar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAdelanto.Focus();
            }
        }
        private  void crearObjetoCobroDetalle(int idCobro)
        {
           
            currentSaveObject = new SaveObjectCobroDetalle();
            currentSaveObject.estado = 1;
            currentSaveObject.fechaPago = dtpFechaPago.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
            currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
            currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
            currentSaveObject.idCobro = idCobro;// recuperar
            currentSaveObject.idMedioPago =1;
            currentSaveObject.idMoneda = Convert.ToInt32(cbxTipoMoneda.SelectedValue);
            currentSaveObject.medioPago = "EFECTIVO";
            currentSaveObject.moneda = cbxTipoMoneda.Text;
            currentSaveObject.monto = darformatoGuardar( txtTotalAdelantos.Text);//
            currentSaveObject.montoInteres ="0";
            currentSaveObject.motivo = "PAGO VENTA";
            currentSaveObject.numeroOperacion = " ";
            currentSaveObject.observacion ="Adelanto de Venta";
            // currentSaveObject
            currentSaveObject.idMoneda = Convert.ToInt32(cbxTipoMoneda.SelectedValue);

        }

        private void txtObservaciones_TextChanged(object sender, EventArgs e)
        {
            
        }
    }



    class SaveObjectCobroDetalle
    {
        // public int idIngreso { get; set; }
        public int estado { get; set; }
        public string fechaPago { get; set; }
        public int idCaja { get; set; }
        public int idCajaSesion { get; set; }
        public int idCobro { get; set; }
        public int idMedioPago { get; set; }
        public int idMoneda { get; set; }
        public string medioPago { get; set; }
        public string moneda { get; set; }
        public string monto { get; set; }
        public string montoInteres { get; set; }
        public string motivo { get; set; }
        public string numeroOperacion { get; set; }
        public string observacion { get; set; }
    }
    
}
