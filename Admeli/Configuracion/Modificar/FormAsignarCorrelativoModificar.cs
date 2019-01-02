using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
using Modelo;

namespace Admeli.Configuracion.Modificar
{
    public partial class FormAsignarCorrelativoModificar : Form
    {
        private DocCorrelativo currentDocCorrelativo;

        private PuntoVentaModel puntoVentaModel = new PuntoVentaModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private TipoDocumentoModel tipoDocumentoModel = new TipoDocumentoModel();
        private DocCorrelativoModel docCorrelativoModel = new DocCorrelativoModel();
        private CajaModel cajaModel = new CajaModel();
        private CompraModel compraModel = new CompraModel();
        private AlmacenModel almacenModel = new AlmacenModel();


        private CajaCorrelativo cajaCorrelativo { get; set; }
        private VentaCorrelativo ventaCorrelativo { get; set; }
        private CajaCorrelativoM cajaCorrelativoM { get; set; }


        private SucursalCorrelativo sucursalCorrelativo { get; set; }

        private AlmacenCorrelativo almacenCorrelativo { get; set; }
        private string area { get; set; }
        #region ========================= Constructor =========================
        public FormAsignarCorrelativoModificar()
        {
            InitializeComponent();
        }

        public FormAsignarCorrelativoModificar(DocCorrelativo currentDocCorrelativo)
        {
            InitializeComponent();
            this.currentDocCorrelativo = currentDocCorrelativo;
            //VENTAS  CAJA COMPRAS ALMACEN
            area = currentDocCorrelativo.area;

        }

        #endregion

        #region ================================== Root Load ==================================
        private void FormAsignarCorrelativoModificar_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {

            switch (area)
                
                
            {
                case "VENTAS":

                    this.cbxPuntoVenta.DataSource = this.puntoDeVentaBindingSource;
                    this.cbxPuntoVenta.DisplayMember = "nombre";
                    this.cbxPuntoVenta.ValueMember = "idPuntoVenta";
                    // tres servivios tipo documentoventas 

                    this.cargarSucursalesVentas(1);
                  

                    break;
                case "CAJA":

                    //2  servicios sucursal  cajas/suc/1
                    this.cbxPuntoVenta.DataSource = this.cajaCorrelativoBindingSource;
                    this.cbxPuntoVenta.DisplayMember = "nombre";
                    this.cbxPuntoVenta.ValueMember = "idCaja";

                    this.cargarSucursalesVentas(2);
                    

                    break;
                case "COMPRAS":


                    // 2 servivios sucursal tipodocumentocompras/compras 
                    this.cargarSucursalesVentas(3);
                   

                    break;
                case "ALMACÉN":
                    this.cbxPuntoVenta.DataSource = this.almacenBindingSource;
                    this.cbxPuntoVenta.DisplayMember = "nombre";
                    this.cbxPuntoVenta.ValueMember = "idAlmacen";
                    //3 servicios sucursal  almacen/suc/1 tipodocumentoalmacen/almacen
                    this.cargarSucursalesVentas(4);
                  
                    break;

            }

        }
        #endregion


        #region ================================== Loads ==================================
        private async void cargarSucursalesVentas(int tipo)
        {
            try
            {

                sucursalBindingSource.DataSource = await sucursalModel.sucursales();

                RootObject<TipoDocumento> rootData = await tipoDocumentoModel.tipodocumentos(1,10);
                tipoDocumentoBindingSource.DataSource = rootData.datos;
              
                int sucursalID = currentDocCorrelativo.idSucursal;                
                switch (tipo)
                {
                    case 1:
                        puntoDeVentaBindingSource.DataSource = await puntoVentaModel.puntoventas(sucursalID);                      
                        lbArea.Text = area;
                        if (area == "")
                        {

                            plArea.Visible = false;

                        }
                        cbxPuntoVenta.SelectedValue = currentDocCorrelativo.idOperacion;
                        cbxSucursal.SelectedValue = currentDocCorrelativo.idSucursal;
                        cbxTipoDocumento.SelectedValue = currentDocCorrelativo.idDocumento;
                        textSerie.Text = currentDocCorrelativo.serie;
                        textCorrelativoSiguiente.Text = currentDocCorrelativo.correlativoActual;
                        cbxArea.Text = this.area;
                        break;
                    case 2:
                        cbxTipoDocumento.DataSource = null;
                        cbxTipoDocumento.Items.Add("EGRESO");
                        cbxTipoDocumento.Items.Add("INGRESO");
                        cajaCorrelativoBindingSource.DataSource = await cajaModel.listarCajasByIdSucursal(sucursalID);
                        lbArea.Text = area;
                        if (area == "")
                        {

                            plArea.Visible = false;

                        }

                        cbxPuntoVenta .SelectedValue = currentDocCorrelativo.idOperacion;
                        cbxSucursal.SelectedValue = currentDocCorrelativo.idSucursal;
                        cbxTipoDocumento.Text = currentDocCorrelativo.nombreLabel;
                        textSerie.Text = currentDocCorrelativo.serie;
                        textCorrelativoSiguiente.Text = currentDocCorrelativo.correlativoActual;
                        cbxArea.Text = this.area;
                        break;
                    case 3:


                        // 2 servivios sucursal tipodocumentocompras/compras 
                        lbArea.Text = area;                     
                        plArea.Visible = false;                    
                        cbxSucursal.SelectedValue = currentDocCorrelativo.idSucursal;
                        cbxTipoDocumento.SelectedValue = currentDocCorrelativo.idDocumento;
                        textSerie.Text = currentDocCorrelativo.serie;
                        textCorrelativoSiguiente.Text = currentDocCorrelativo.correlativoActual;
                        cbxArea.Text = this.area;

                        break;
                    case 4:

                        //3 servicios sucursal  almacen/suc/1 tipodocumentoalmacen/almacen

                        almacenBindingSource.DataSource = await almacenModel.listarAlmacenPorIdSucursal(sucursalID);

                        if (area == "")
                        {

                            plArea.Visible = false;

                        }
                        cbxPuntoVenta.SelectedValue = currentDocCorrelativo.idOperacion;
                        cbxSucursal.SelectedValue = currentDocCorrelativo.idSucursal;
                        cbxTipoDocumento.SelectedValue = currentDocCorrelativo.idDocumento;
                        textSerie.Text = currentDocCorrelativo.serie;
                        textCorrelativoSiguiente.Text = currentDocCorrelativo.correlativoActual;
                        cbxArea.Text = this.area;
                        

                        break;




                }





                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message, "Load", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       

       

  

        #endregion

        #region ============================ Decoration ============================
        private void FormAsignarCorrelativoModificar_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel2, 157, 157, 157);
            drawShape.lineBorder(panel4, 157, 157, 157);
            drawShape.lineBorder(plArea, 157, 157, 157);
            drawShape.lineBorder(panel6, 157, 157, 157);
        }
        #endregion

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarSucursal();
        }

        private async void guardarSucursal()
        {
            bloquear(true);
            Response response=null;
            if (!validarCampos()) { bloquear(false); return; }
            try
            {

                
                switch (area)


                {
                    case "VENTAS":
;
                        crearObjetoSucursal();
                       response = await docCorrelativoModel.modificarVentaCorrelativo(ventaCorrelativo);

                        break;
                    case "CAJA":

                        crearObjetoCaja();
                        response = await docCorrelativoModel.modificarCajaCorrelativo(cajaCorrelativoM);


                        break;
                    case "COMPRAS":
                        crearObjetoCompra();
                        response = await docCorrelativoModel.modificarSucursalCorrelativo(sucursalCorrelativo);

                        break;
                    case "ALMACÉN":
                        crearObjetoAlmacen();
                        response = await docCorrelativoModel.modificarAlmacenCorrelativo(almacenCorrelativo);

                        break;

                }

            
                MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bloquear(false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bloquear(false);
            }
        }

        private void crearObjetoSucursal()
        {

            ventaCorrelativo = new VentaCorrelativo();
            ventaCorrelativo.serie = textSerie.Text;
            ventaCorrelativo.correlativoActual = textCorrelativoSiguiente.Text;
            ventaCorrelativo.estado = Convert.ToInt32(chkActivoSucursal.Checked);
            ventaCorrelativo.idVentaCorrelativo = currentDocCorrelativo.idCorrelativo;           
        }

        private void crearObjetoCaja()
        {

            cajaCorrelativoM = new CajaCorrelativoM();
            cajaCorrelativoM.serie = textSerie.Text;
            cajaCorrelativoM.correlativoActual = textCorrelativoSiguiente.Text;
            cajaCorrelativoM.estado = Convert.ToInt32(chkActivoSucursal.Checked);
            cajaCorrelativoM.idCajaCorrelativo = currentDocCorrelativo.idCorrelativo;
        }
        private void crearObjetoCompra()
        {

            sucursalCorrelativo = new SucursalCorrelativo();
            sucursalCorrelativo.serie = textSerie.Text;
            sucursalCorrelativo.correlativoActual = textCorrelativoSiguiente.Text;
            sucursalCorrelativo.estado = Convert.ToInt32(chkActivoSucursal.Checked);
            sucursalCorrelativo.idSucursalCorrelativo = currentDocCorrelativo.idCorrelativo;
        }
        private void crearObjetoAlmacen()
        {

            almacenCorrelativo = new AlmacenCorrelativo();
            almacenCorrelativo.serie = textSerie.Text;
            almacenCorrelativo.correlativoActual = textCorrelativoSiguiente.Text;
            almacenCorrelativo.estado = Convert.ToInt32(chkActivoSucursal.Checked);
            almacenCorrelativo.idAlmacenCorrelativo = currentDocCorrelativo.idCorrelativo;
        }

        private bool validarCampos()
        {
            if (textSerie.Text == "")
            {
                errorProvider1.SetError(textSerie, "Este campo esta bacía");
                textSerie.Focus();
                return false;
            }
            errorProvider1.Clear();

            if (textCorrelativoSiguiente.Text == "")
            {
                errorProvider1.SetError(textCorrelativoSiguiente, "Este campo esta bacía");
                textCorrelativoSiguiente.Focus();
                return false;
            }
            errorProvider1.Clear();
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        public void bloquear(bool state)
        {
            if (state)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
            this.Enabled = !state;
        }
    }
}
