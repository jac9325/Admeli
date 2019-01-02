using Admeli.Componentes;
using Entidad;
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
    public partial class FormDetalleCobroNuevo : Form
    {
        SaveObjectCobroDetalle currentSaveObject = new SaveObjectCobroDetalle();
        private MonedaModel monedaModel = new MonedaModel();
        private CobroModel cobroModel = new CobroModel();
        private MedioPagoModel medioPagoModel = new MedioPagoModel();
        private ConfigModel configModel = new ConfigModel();
        private CajaModel cajaModel = new CajaModel();
        private List<MedioPago> mediosDePagos { get; set; }
        private bool nuevo { get; set; }

        private Cobro currentCobro = new Cobro();
        public FormDetalleCobroNuevo()
        {
            InitializeComponent();
        }
        public FormDetalleCobroNuevo(Cobro currentCobro)
        {
            InitializeComponent();
            this.nuevo = true;
            this.currentCobro = currentCobro;
            this.reLoad();
        }

        private void reLoad()
        {
            this.verificarCaja();
            this.fechaSistema();
            this.cargarMonedas();
            this.cargarMediosPago();
        }
        
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

        private async void fechaSistema()
        {
            try
            {
                //No existe un Modelo para fechaSistema, por ello se hace directmente
                //http://localhost:8085/admeli/xcore/services.php/fechasystema
                Modelo.Recursos.WebService webService = new Modelo.Recursos.WebService();
                FechaSistema fecha = await webService.GET<FechaSistema>("fechasystema");
                dtpFechaPago.Value = fecha.fecha;
                dtpFechaCalendario.Value = fecha.fecha;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void cargarMonedas()
        {
            try
            {
                monedaBindingSource.DataSource = await monedaModel.monedas();
                cbxMoneda.SelectedValue = currentCobro.idMoneda;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Error: " + ex.Message, "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        struct CorrelativoData
        {
            public string correlativoActual { get; set; }
            public string serie { get; set; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            guardarCobroDetalle();
        }
        private async void guardarCobroDetalle()
        {
            bloquear(true);
            await configModel.loadCajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);

            //validar los campos
            if (!validarCampos()) { bloquear(false); return; }
            try
            {
                //Verificar Caja Asignada y recuperar idCajaSesion
                //currentCajaSesion=await cajaModel.cajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);
                //Registrar CobroDetalle
                crearObjetoCobroDetalle();
                if (nuevo)
                {

                    Response response = await cobroModel.guardarCobroDetalle(currentSaveObject);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //No se puede modificar
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                bloquear(false);

            }

            
        }
        private void crearObjetoCobroDetalle()
        {
            currentSaveObject = new SaveObjectCobroDetalle();
            currentSaveObject.estado = Convert.ToInt32(chkActivo.Checked);
            currentSaveObject.fechaPago = dtpFechaPago.Value.ToString("yyyy-MM-dd HH':'mm':'ss");
            currentSaveObject.idCaja = ConfigModel.asignacionPersonal.idCaja;
            currentSaveObject.idCajaSesion = ConfigModel.cajaSesion.idCajaSesion;
            currentSaveObject.idCobro = currentCobro.idCobro;
            currentSaveObject.idMedioPago = mediosDePagos[0].idMedioPago;
            currentSaveObject.idMoneda = Convert.ToInt32(cbxMoneda.SelectedValue);
            currentSaveObject.medioPago = mediosDePagos[0].nombre;
            currentSaveObject.moneda = cbxMoneda.Text;
            currentSaveObject.monto = textMonto.Text;
            currentSaveObject.montoInteres = textMontoInteres.Text;
            currentSaveObject.motivo = "PAGO VENTA";
            currentSaveObject.numeroOperacion = " ";
            currentSaveObject.observacion = textObservacion.Text;
            // currentSaveObject
            currentSaveObject.idMoneda = Convert.ToInt32(cbxMoneda.SelectedValue);

        }

        private bool validarCampos()
        {
            //Validar Monto
            if(textMonto.Text.Trim() == "")
            {
                errorProvider1.SetError(textMonto, "Este campo está vacío");
                Validator.textboxValidateColor(textMonto, 0);
                textMonto.Focus();
                return false;
            }            
            errorProvider1.Clear();
            Validator.textboxValidateColor(textMonto, 1);
            //Moneda
            if(cbxMoneda.SelectedIndex < 0)
            {
                errorProvider1.SetError(cbxMoneda, "Este campo está vacío, vuevla a cargar el formulario");
                return false;
            }
            //Fecha Pago
            if (dtpFechaPago.Value == null)
            {
                errorProvider1.SetError(dtpFechaPago, "Este campo está vacío");
                return false;
            }
            //Fecha Calendario
            if (dtpFechaCalendario.Value == null)
            {
                errorProvider1.SetError(dtpFechaCalendario, "Este campo está vacío, vuevla a cargar el formulario");
                return false;
            }
            // Toda las validaciones correctas
            return true;
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

        private void textMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, textMonto.Text);
        }

        private void bunifuMetroTextbox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, textMonto.Text);
        }

        private void panelMoneda_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelMoneda, 157, 157, 157);
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
