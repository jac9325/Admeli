using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
using Entidad.Util;
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

namespace Admeli.CajaBox.Nuevo
{
    public partial class FormEgresoNuevo : Form
    {
        private MonedaModel monedaModel = new MonedaModel();
        private EgresoModel egresoModel = new EgresoModel();
        private MedioPagoModel medioPagoModel = new MedioPagoModel();
        private CajaModel cajaModel = new CajaModel();
        private ConfigModel configModel = new ConfigModel();
        private int currentIDEgreso { get; set; }
        private bool nuevo { get; set; }
        private Egreso currentEgreso { get; set; }
        private SaveObjectEgreso currentSaveObject { get; set; }
        private List<MedioPago> mediosDePagos { get; set; }

        private List<CajaSesion> currentCajaSesion { get; set; }


        public FormEgresoNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
            dtpFechaPago.Value = DateTime.Now;
        }

        public FormEgresoNuevo(Egreso currentEgreso)
        {
            this.currentEgreso = currentEgreso;
            this.nuevo = false;
        }

        #region ========================= Root Load =========================
        private void FormEgresoNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            this.cargarMonedas();
            this.cargarMediosPago();
            this.cargarCorrelativo();

            // Verificacion del estado de caja
            this.verificarCaja();
        } 
        #endregion

        #region ========================= Loads =========================
        private async void cargarMonedas()
        {
            try
            {
                monedaBindingSource.DataSource = await monedaModel.monedas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void cargarMediosPago()
        {
            try
            {
                mediosDePagos = await medioPagoModel.medioPagos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        struct CorrelativoData
        {
            public string correlativoActual { get; set; }
            public string serie { get; set; }
        }

        private async void cargarCorrelativo()
        {
            try
            {
                CorrelativoData response = await cajaModel.correlativoSerie<CorrelativoData>(ConfigModel.asignacionPersonal.idCaja, 0);
                textNOperacion.Text = String.Format("{0} - {1}", response.serie, response.correlativoActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region ================================= Validator ====================================
        private void verificarCaja()
        {
            if (ConfigModel.cajaIniciada)
            {
                lblCajaEstado.Visible = false;
            }
            else
            {
                Validator.labelAlert(lblCajaEstado, 0, "No se inició la caja");
                lblCajaEstado.Visible = true;
                btnAceptar.Enabled = false;
            }
        }
        #endregion

        #region ========================== SAVE AND UPDATE ===========================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //guardarSucursal();
            guardarEgreso();
        }
        private async void guardarEgreso()
        {
            bloquear(true);
            await configModel.loadCajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);
            //validar los campos
            if (!validarCampos()) { bloquear(false); return; }
            try
            {
                //Verificar Caja Asignada y recuperar idCajaSesion
                //currentCajaSesion=await cajaModel.cajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);
                //Verificar que exista dinero para egresar

                //cbxMoneda.SelectedValue.ToString()
                
                List<Moneda> monedas = await cajaModel.cierreCajaIngresoMenosEgreso(mediosDePagos[0].idMedioPago, ConfigModel.cajaSesion.idCajaSesion);
                int idMoneda = Convert.ToInt32(cbxMoneda.SelectedValue.ToString());
                Moneda moneda = monedas.Find(x => x.idMoneda == idMoneda);

                if (moneda.total < double.Parse(textMonto.Text))
                {
                    MessageBox.Show("No Hay dinero suficiente en la caja", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bloquear(false);
                    return;
                }
                //Registrar Egreso
                crearObjetoEgreso();
                if (nuevo)
                {
                    Response response = await egresoModel.guardar(currentSaveObject);
                    if (response.id == -1)
                    {
                        loadCajaSesion();
                        MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else
                    {
                        MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                   
                }
                else
                {
                    Response response = await egresoModel.modificar(currentEgreso);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                bloquear(false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bloquear(false);
            }
            
        }

        private async void loadCajaSesion()
        {
            await configModel.loadCajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);

            if (!ConfigModel.cajaIniciada)
            {
                Validator.labelAlert(lblCajaEstado, 0, "No se inició la caja");

            }


        }

        private async void guardarSucursal()
        {
            if (!validarCampos()) return;
            try
            {
                // Verificacion
                List<Moneda> monedas = await cajaModel.cierreCajaIngresoMenosEgreso(mediosDePagos[0].idMedioPago, ConfigModel.cajaSesion.idCajaSesion);
                MessageBox.Show("Falta logica de programacion");

                // Guardar
                if (nuevo)
                {
                    crearObjetoEgreso();
                    Response response = await egresoModel.guardar(currentEgreso);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await egresoModel.modificar(currentEgreso);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void crearObjetoEgreso()
        {
            currentSaveObject = new SaveObjectEgreso();
            //currentEgreso = new Egreso();
            if (!nuevo) currentSaveObject.idEgreso = currentIDEgreso; // Llenar el id categoria cuando este en esdo modificar
            currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
            currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
            currentSaveObject.idMedioPago = mediosDePagos[0].idMedioPago;
            currentSaveObject.idMoneda = Convert.ToInt32(cbxMoneda.SelectedValue);
            currentSaveObject.medioPago = mediosDePagos[0].nombre;
            currentSaveObject.moneda = cbxMoneda.Text;
            currentSaveObject.monto = textMonto.Text;
            currentSaveObject.motivo = textMotivo.Text;
            currentSaveObject.observacion = textObcervacion.Text;
            currentSaveObject.numeroOperacion = textNOperacion.Text;
            currentSaveObject.fechaPago = dtpFechaPago.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
        }

        private bool validarCampos()
        {
            //Validar Nro Operacion
            if (textNOperacion.Text == "")
            {
                errorProvider1.SetError(textNOperacion, "Este campo está vacío");
                textNOperacion.Focus();
                return false;
            }
            errorProvider1.Clear();
            // validacion Monto
            if (textMonto.Text.Trim() == "")
            {
                errorProvider1.SetError(textMonto, "Campo obligatorio");
                Validator.textboxValidateColor(textMonto, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textMonto, 1);
            // Toda las validaciones correctas
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ========================= Decoration and value defaults =========================
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel2, 157, 157, 157);
        }
        #endregion

        #region ============================ Validacion timpo real ============================
        private void textMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e,textMonto.Text);
        }
        private void textMonto_Validated(object sender, EventArgs e)
        {
            if (textMonto.Text.Trim() == "")
            {
                errorProvider1.SetError(textMonto, "Campo obligatorio");
                Validator.textboxValidateColor(textMonto, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textMonto, 1);
        }

        private void textMotivo_Validated(object sender, EventArgs e)
        {
        }
        #endregion

        private void FormEgresoNuevo_Shown(object sender, EventArgs e)
        {
            textMonto.Focus();
        }
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
