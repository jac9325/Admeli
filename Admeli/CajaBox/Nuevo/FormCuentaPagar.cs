using Entidad;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.CajaBox.Nuevo
{
    public partial class FormCuentaPagar : Form
    {
        private DatoCuentaPagar currentCuentaPagar;
        private Pago currentPago;
        private DetallePago currentDetallePago;
        private PagoModel pagoModel = new PagoModel();
        private ConfigModel configModel = new ConfigModel();
        private List<Pago> listaPagos = new List<Pago>();
        private List<DetallePago> listaDetallePagos = new List<DetallePago>();
        private int currentIdProveedor;
        int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        string formato { get; set; }
        public FormCuentaPagar()
        {
            InitializeComponent();
        }
        private string darformato(object dato)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }
        private void darFormatoDecimales()
        {
            try
            {
                dgvPagos.Columns["valorTotalDataGridViewTextBoxColumn"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
                dgvPagos.Columns["saldoDataGridViewTextBoxColumn"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
                dgvDetallePago.Columns["importeDataGridViewTextBoxColumn"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
                
                dgvPagos.Columns["valorTotalDataGridViewTextBoxColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPagos.Columns["saldoDataGridViewTextBoxColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDetallePago.Columns["importeDataGridViewTextBoxColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
               
            }
            catch
            {
                MessageBox.Show("Error al dar formato a los DataGrid", "Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public FormCuentaPagar(DatoCuentaPagar currentCuentaPagar)
        {
            InitializeComponent();
            formato = "{0:n" + nroDecimales + "}";
            darFormatoDecimales();
            this.currentCuentaPagar = currentCuentaPagar;
            this.currentIdProveedor = currentCuentaPagar.idProveedor;
            this.lbNombreProveedor.Text = currentCuentaPagar.razonSocial;
            this.chkMostrarTodos.Checked = false;
            this.reLoad();
        }

        private void reLoad()
        {
            loadState(false);
            cargarCuentasPagar();

            //Calcular totales y saldo pendientes

            loadState(true);
        }




        private async void cargarCuentasPagar()
        {
            try
            {
                //Cargar Las Cuentas por Pagar del Proveedor
                if (chkMostrarTodos.Checked)
                {
                    listaPagos = await pagoModel.pagosProveedor(currentIdProveedor, ConfigModel.sucursal.idSucursal, 1);
                }
                else
                {
                    listaPagos = await pagoModel.pagosProveedor(currentIdProveedor, ConfigModel.sucursal.idSucursal, 0);
                }
                pagoBindingSource.DataSource = listaPagos;
                dgvPagos.Refresh();

                if (listaPagos.Count == 0)
                {
                    detallePagoBindingSource.DataSource = null;
                    dgvDetallePago.Refresh();
                    return;
                };

                executeDetalles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: No se pudo cargar las Cuentas por Pagar" + ex.Message, "Cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void executeDetalles()
        {
            // Verificando la existencia de datos en el datagridview
            if (dgvPagos.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Detalles", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dgvPagos.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idPago = Convert.ToInt32(dgvPagos.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview
            currentPago = listaPagos.Find(x => x.idPago == idPago); //Buscando el registro especifico en la lista de registros

            //Mostrar Detalles del Cobro
            listaDetallePagos = await pagoModel.detallePago(currentPago.idPago);

            // Mostrando el formulario de modificacion
            detallePagoBindingSource.DataSource = listaDetallePagos;
            dgvDetallePago.Refresh();

            //Colocar el total para cobrar
            lbTotal.Text =darformato( currentPago.valorTotal);
            lbSaldo.Text = darformato( currentPago.saldo);
        }

        private void loadState(bool state)
        {
            if (!state)
            {
                progressbActual.Visible = true;
                progressbActual.Style = ProgressBarStyle.Marquee;

            }
            else
            {
                progressbActual.Visible = false;
                progressbActual.Style = ProgressBarStyle.Blocks;

            }
            panelBody.Enabled = state;
            panelFooder.Enabled = state;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminarDetallePago_Click(object sender, EventArgs e)
        {
            //Verificar que se haya seleccionado un registro de CobroDetalle
            if (dgvDetallePago.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Verificar que se inicio caja
            if (!ConfigModel.cajaIniciada)
            {
                MessageBox.Show("Ud. no inició caja", "Anular", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Anular CobroDetalle
            anularPagoDetalle();
            reLoad();
        }

        private async void anularPagoDetalle()
        {
            try
            {
                //Verificar que la caja con la que se realizo el cobro sea la misma que la actual
                int index = dgvDetallePago.CurrentRow.Index;
                int idDetallePago = Convert.ToInt32(dgvDetallePago.Rows[index].Cells[0].Value);
                currentDetallePago = listaDetallePagos.Find(x => x.idDetallePago == idDetallePago);
                currentDetallePago.idPago = currentPago.idPago;
                if (currentDetallePago.idCajaSesion != ConfigModel.cajaSesion.idCajaSesion)
                {
                    MessageBox.Show("Error: Este egreso lo realizó con otra caja y no podrá ser anulado", "Anular", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //Verificar que haya dinero suficiente para hacer el procedimiento
                //http://localhost:8085/ad_meli/xcore/services.php/cierrecajaingresomenosegreso/mediopago/1/cajasesion/11                   

                //List<Moneda> monedas = await cajaModel.cierreCajaIngresoMenosEgreso(mediosDePagos[0].idMedioPago, ConfigModel.cajaSesion.idCajaSesion);
                //if (monedas[0].total < double.Parse(textMonto.Text))
                //{
                //    MessageBox.Show("No Hay dinero suficiente en la caja", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                //Anular Cobro Detalle
                //No se puede anular si es solo un cobrodetalle
                Response response = await pagoModel.anularPagoDetalle(currentDetallePago);

                MessageBox.Show(response.msj, "Anular", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Anular", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkMostrarTodos_OnChange(object sender, EventArgs e)
        {
            reLoad();
        }

        private void dgvPagos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            executeDetalles();
        }
        private async void loadCajaSesion()
        {
            await configModel.loadCajaSesion(ConfigModel.asignacionPersonal.idAsignarCaja);


        }
        private void btnNuevoDetallePago_Click(object sender, EventArgs e)
        {
            loadCajaSesion();
            //Verificar que se haya seleccionado un regitro
            if (dgvPagos.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Nuevo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Verificar que se inicio caja
            if (!ConfigModel.cajaIniciada)
            {
                MessageBox.Show("Ud. no inició caja", "Nuevo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Nuevo Detalle Cobro
            FormDetallePagoNuevo formDetallePagoNuevo = new FormDetallePagoNuevo(currentPago);
            formDetallePagoNuevo.ShowDialog();
            reLoad();
        }

        private void toolStripCrud_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
