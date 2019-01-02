using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
using Entidad.Location;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Configuracion.Nuevo
{
    public partial class FormSucursalNuevo : Form
    {
        private LocationModel locationModel = new LocationModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private PuntoModel puntoModel = new PuntoModel();

        private List<LabelUbicacion> labelUbicaciones { get; set; }
        private UbicacionGeografica ubicacionGeografica { get; set; }

        private PuntoAdministracion puntoAdministracion { get; set; }
        private PuntoCompra puntoCompra { get; set; }
        private List<PuntoDeVenta> puntosDeVenta { get; set; }
        private List<Caja> cajas { get; set; }
        private PuntoGerencia puntoGerencia { get; set; }

        private int currentIDSucursal { get; set; }
        private bool nuevo { get; set; }
        private Sucursal currentSucursal { get; set; }

        #region ========================= Constructor =========================
        public FormSucursalNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormSucursalNuevo(Sucursal currentSucursal)
        {
            InitializeComponent();
            this.currentSucursal = currentSucursal;
            this.currentIDSucursal = currentSucursal.idSucursal;
            this.mostrarDatosModificar();
            this.nuevo = false;
        }
        #endregion

        #region ========================= Paint =========================
        private void panelFooter_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.topLine(panelFooter);
        }

        private void FormSucursalNuevo_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelLevelPais, 157, 157, 157);
            drawShape.lineBorder(panelLevel1, 157, 157, 157);
            drawShape.lineBorder(panelLevel2, 157, 157, 157);
            drawShape.lineBorder(panelLevel3, 157, 157, 157);
        }
        #endregion

        #region =============================== Root Load ===============================
        private void FormSucursalNuevo_Load(object sender, EventArgs e)
        {
            reLoad();
        }

        private async void reLoad()
        {
            try
            {
                await cargarPaises();
                crearNivelesPais();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "reload Cargar Paises", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region ========================== Load ==========================
        private void mostrarDatosModificar()
        {
            textNombreSucursal.Text = currentSucursal.nombre;
            textDirecionSucursal.Text = currentSucursal.direccion;
            chkPrincipalSucursal.Checked = currentSucursal.principal;
            chkActivoSucursal.Checked = Convert.ToBoolean(currentSucursal.estado);

            loadPuntoAdministracion();
            loadPuntoCompra();
            loadPuntoVenta();
            loadCajas();
            loadPuntoGerencia();
        }



        private async void loadPuntoAdministracion()
        {
            puntoAdministracion = await puntoModel.puntoAdministracion(currentSucursal.idSucursal);
            chkAdministracionSucursal.Checked = puntoAdministracion == null ? false: Convert.ToBoolean(puntoAdministracion.estado);
        }
        private async void loadPuntoCompra()
        {
            puntoCompra = await puntoModel.puntoCompra(currentSucursal.idSucursal);
            chkCompraSucursal.Checked = puntoCompra == null ? false: Convert.ToBoolean(puntoCompra.estado);
        }
        private async void loadPuntoVenta()
        {
            puntosDeVenta = await puntoModel.puntoVentas(currentSucursal.idSucursal);
            chkVentaSucursal.Checked = puntosDeVenta == null ? false:Convert.ToBoolean((puntosDeVenta.Count > 0) ? puntosDeVenta[0].estado : 0);
        }
        private async void loadCajas()
        {
            cajas = await puntoModel.cajas(currentSucursal.idSucursal);
           
            chkCajaSucursal.Checked = cajas == null ?  false:Convert.ToBoolean((cajas.Count > 0) ? cajas[0].estado:0);
        }
        private async void loadPuntoGerencia()
        {
            puntoGerencia = await puntoModel.puntoGerencia(currentSucursal.idSucursal);
            chkGerenciaSucursal.Checked = puntoGerencia == null ? false: Convert.ToBoolean(puntoGerencia.estado);
        }

        private async Task cargarPaises()
        {
            try
            {
                // cargando los paises
                paisBindingSource.DataSource = await locationModel.paises();

                // cargando la ubicacion geografica por defecto
                if (nuevo)
                {
                    ubicacionGeografica = await locationModel.ubigeoActual(ConfigModel.sucursal.idUbicacionGeografica);
                }
                else
                {
                    ubicacionGeografica = await locationModel.ubigeoActual(currentSucursal.idUbicacionGeografica);
                }
                cbxPaises.SelectedValue = ubicacionGeografica.idPais;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Paises", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } 
        #endregion

        #region ================== Formando los niveles de cada pais ==================
        private async void crearNivelesPais()
        {
            try
            {
                loadStateApp(true);
                labelUbicaciones = await locationModel.labelUbicacion(Convert.ToInt32(cbxPaises.SelectedValue));
                ocultarNiveles(); // Ocultando todo los niveles

                // Mostrando los niveles uno por uno
                if (labelUbicaciones.Count >= 1)
                {
                    lblNivel1.Text = labelUbicaciones[0].denominacion;
                    panelLevel1.Visible = true;
                }

                if (labelUbicaciones.Count >= 2)
                {
                    lblNivel2.Text = labelUbicaciones[1].denominacion;
                    panelLevel2.Visible = true;
                }

                if (labelUbicaciones.Count >= 3)
                {
                    panelLevel3.Visible = true;
                    lblNivel3.Text = labelUbicaciones[2].denominacion;
                }

                // Cargar el primer nivel de la localizacion
                cargarNivel1();

            }
            catch (Exception)
            {
                // MessageBox.Show(ex.Message);
            }
            finally
            {
                loadStateApp(false);
            }
        }

        private void ocultarNiveles()
        {
            panelLevel1.Visible = false;
            panelLevel2.Visible = false;
            panelLevel3.Visible = false;

            cbxNivel1.SelectedIndex = -1;
            cbxNivel2.SelectedIndex = -1;
            cbxNivel3.SelectedIndex = -1;
        }

        private async void cargarNivel1()
        {
            try
            {
                // No cargar directo al comobobox esto causara que el evento SelectedIndexChange de forma automatica
                if (labelUbicaciones.Count < 1) return;
                loadStateApp(true);        
                nivel1BindingSource.DataSource = await locationModel.nivel1(Convert.ToInt32(cbxPaises.SelectedValue));
                // seleccionando el valor por defecto
                if (ubicacionGeografica.idNivel1 > 0)
                {
                    cbxNivel1.SelectedValue = ubicacionGeografica.idNivel1;
                }
                else
                {
                    cbxNivel1.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upps! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally{
                loadStateApp(false);
                desactivarNivelDesde(2);
            }
        }

        private async void cargarNivel2()
        {
            try
            {
                if (labelUbicaciones.Count < 2) return;
                loadStateApp(true);
                nivel2BindingSource.DataSource = await locationModel.nivel2(Convert.ToInt32(cbxNivel1.SelectedValue));
                if (ubicacionGeografica.idNivel2 > 0)
                {
                    cbxNivel2.SelectedValue = ubicacionGeografica.idNivel2;
                }
                else
                {
                    cbxNivel2.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upps! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally{
                desactivarNivelDesde(3);
                loadStateApp(false);
            }
        }

        private async void cargarNivel3()
        {
            try
            {
                if (labelUbicaciones.Count < 3) return;
                loadStateApp(true);
                nivel3BindingSource.DataSource = await locationModel.nivel3(Convert.ToInt32(cbxNivel2.SelectedValue));
                if (ubicacionGeografica.idNivel3 > 0)
                {
                    cbxNivel3.SelectedValue = ubicacionGeografica.idNivel3;
                }
                else
                {
                    cbxNivel3.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upps! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadStateApp(false);
                desactivarNivelDesde(4);
            }
        }

        private void desactivarNivelDesde(int n)
        {
            cbxNivel1.Enabled = true;
            cbxNivel2.Enabled = true;
            cbxNivel3.Enabled = true;

            if (n < 2) cbxNivel1.Enabled = false;
            if (n < 3) cbxNivel2.Enabled = false;
            if (n < 4) cbxNivel3.Enabled = false;
        }
        #endregion

        #region ==================== Estados =====================
        private void loadStateApp(bool state)
        {
            if (state)
            {
                progressBarApp.Style = ProgressBarStyle.Marquee;
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                progressBarApp.Style = ProgressBarStyle.Blocks;
                Cursor.Current = Cursors.Default;
            }
        } 
        #endregion

        #region ====================== Niveles localizacion eventos =====================
        private void cbxPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            crearNivelesPais();
        }

        private void cbxNivel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarNivel2();
        }

        private void cbxNivel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarNivel3();
        } 
        #endregion

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarSucursal();
        } 

        private async void guardarSucursal()
        {
            Bloqueo.bloquear(this, true);
            if (!validarCampos()) { Bloqueo.bloquear(this, false); return; }
            try
            {
                // Validacion
                int sucursalID = (nuevo) ? 0 : currentIDSucursal;
                Response ress = await sucursalModel.existeSucursal(textNombreSucursal.Text, sucursalID);
                if (ress.id == 0)
                {
                    errorProvider1.SetError(textNombreSucursal, ress.msj);
                    Validator.textboxValidateColor(textNombreSucursal, 0);
                    return;
                }
                errorProvider1.Clear();
                Validator.textboxValidateColor(textNombreSucursal, 1);

                // Procediendo con el guardado
                crearObjetoSucursal();
                if (nuevo)
                {
                    Response response = await sucursalModel.guardarTotal(ubicacionGeografica, currentSucursal);
                    currentSucursal.idSucursal = response.id;
                   
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await sucursalModel.modificar(ubicacionGeografica, currentSucursal);
                    cargarEstados();
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Bloqueo.bloquear(this, false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Bloqueo.bloquear(this, false);
            }
        }

        private void crearObjetoSucursal()
        {
            currentSucursal = new Sucursal();

            if (!nuevo) currentSucursal.idSucursal = currentIDSucursal; // Llenar el id categoria cuando este en esdo modificar

            currentSucursal.nombre = textNombreSucursal.Text;
            currentSucursal.principal = chkPrincipalSucursal.Checked;
            currentSucursal.estado = Convert.ToInt32(chkActivoSucursal.Checked);
            currentSucursal.direccion = textDirecionSucursal.Text;
            currentSucursal.tieneRegistros = "0";

            // Cargando estados
            currentSucursal.estados += "";
            currentSucursal.estados += Convert.ToInt32(chkGerenciaSucursal.Checked) + ",";
            currentSucursal.estados += Convert.ToInt32(chkAdministracionSucursal.Checked) + ",";
            currentSucursal.estados += Convert.ToInt32(chkCajaSucursal.Checked) + ",";
            currentSucursal.estados += Convert.ToInt32(chkVentaSucursal.Checked) + ",";
            currentSucursal.estados += Convert.ToInt32(chkCompraSucursal.Checked).ToString();

            // Ubicacion geografica
            ubicacionGeografica.idPais = (cbxPaises.SelectedIndex == -1) ? ubicacionGeografica.idPais : Convert.ToInt32(cbxPaises.SelectedValue);
            ubicacionGeografica.idNivel1 = (cbxNivel1.SelectedIndex == -1) ? ubicacionGeografica.idNivel1 : Convert.ToInt32(cbxNivel1.SelectedValue);
            ubicacionGeografica.idNivel2 = (cbxNivel2.SelectedIndex == -1) ? ubicacionGeografica.idNivel2 : Convert.ToInt32(cbxNivel2.SelectedValue);
            ubicacionGeografica.idNivel3 = (cbxNivel3.SelectedIndex == -1) ? ubicacionGeografica.idNivel3 : Convert.ToInt32(cbxNivel3.SelectedValue);
        }

        private async void cargarEstados()
        {
            //caja puntoventa/modificar
            cajaS cajaS = new cajaS();
            cajaS.estado =Convert.ToInt32( chkCajaSucursal.Checked);
            cajaS.idCaja =cajas.Count>0?  cajas[0].idCaja:0;
            cajaS.idSucursal = currentSucursal.idSucursal;
            await sucursalModel.modificarCaja(cajaS);
            //puntoadministracion
            AdministracionS administracionS = new AdministracionS();
            administracionS.estado= Convert.ToInt32(chkAdministracionSucursal.Checked);
            administracionS.idPuntoAdministracion = puntoAdministracion.idPuntoAdministracion;
            administracionS.idSucursal= currentSucursal.idSucursal;
            await sucursalModel.modificarAdministracion(administracionS);
            //puntogerencia
            GerenciaS gerenciaS = new GerenciaS();
            gerenciaS.estado= Convert.ToInt32(chkGerenciaSucursal.Checked);
            gerenciaS.idPuntoGerencia = puntoGerencia.idPuntoGerencia;
            gerenciaS.idSucursal= currentSucursal.idSucursal;
            await sucursalModel.modificarGerencia(gerenciaS);
            //puntocompra
            CompraS compraS = new CompraS();
            compraS.estado= Convert.ToInt32(chkCompraSucursal.Checked);
            compraS.idPuntoCompra = puntoCompra.idPuntoCompra;
            compraS.idSucursal = currentSucursal.idSucursal;
            await sucursalModel.modificarpuntocompra(compraS);
            //puntoVenta
            VentaS ventaS = new VentaS();
            ventaS.estado= Convert.ToInt32(chkVentaSucursal.Checked);
            ventaS.idPuntoVenta = puntosDeVenta[0].idPuntoVenta;
            ventaS.idSucursal= currentSucursal.idSucursal;
            await sucursalModel.modificarpuntoventa(ventaS);
        }

        private bool validarCampos()
        {
            if (textNombreSucursal.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreSucursal, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreSucursal, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombreSucursal, 1);

            switch (labelUbicaciones.Count)
            {
                case 1:
                    if (cbxNivel1.SelectedIndex == -1)
                    {
                        errorProvider1.SetError(cbxNivel1, "No se seleccionó ningún elemento");
                        cbxNivel1.Focus();
                        return false;
                    }
                    errorProvider1.Clear();
                    break;
                case 2:
                    if (cbxNivel2.SelectedIndex == -1)
                    {
                        errorProvider1.SetError(cbxNivel2, "No se seleccionó ningún elemento");
                        cbxNivel2.Focus();
                        return false;
                    }
                    errorProvider1.Clear();
                    break;
                case 3:
                    if (cbxNivel3.SelectedIndex == -1)
                    {
                        errorProvider1.SetError(cbxNivel3, "No se seleccionó ningún elemento");
                        cbxNivel3.Focus();
                        return false;
                    }
                    errorProvider1.Clear();
                    break;
                default:
                    break;
            }

            if (textDirecionSucursal.Text.Trim() == "")
            {
                errorProvider1.SetError(textDirecionSucursal, "Campo obligatorio");
                Validator.textboxValidateColor(textDirecionSucursal, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textDirecionSucursal, 1);

            return true;
        }

        #endregion

        #region ============================ Validacion Timepo Real ============================
        private async void textNombreSucursal_Validated(object sender, EventArgs e)
        {
            if (textNombreSucursal.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreSucursal, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreSucursal, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombreSucursal, 1);

            try
            {
                int sucursalID = (nuevo) ? 0 : currentIDSucursal;
                Response response = await sucursalModel.existeSucursal(textNombreSucursal.Text, sucursalID);
                if (response.id == 0)
                {
                    errorProvider1.SetError(textNombreSucursal, response.msj);
                    Validator.textboxValidateColor(textNombreSucursal, 0);
                    return;
                }
                errorProvider1.Clear();
                Validator.textboxValidateColor(textNombreSucursal, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textDirecionSucursal_Validated(object sender, EventArgs e)
        {
            if (textDirecionSucursal.Text.Trim() == "")
            {
                errorProvider1.SetError(textDirecionSucursal, "Campo obligatorio");
                Validator.textboxValidateColor(textDirecionSucursal, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textDirecionSucursal, 1);
        }
        #endregion

        private void FormSucursalNuevo_Shown(object sender, EventArgs e)
        {
            textNombreSucursal.Focus();
        }

        #region ======================================= WORKERS =======================================
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        } 
        #endregion
    }


   
}
