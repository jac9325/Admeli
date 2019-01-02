using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modelo;
using Entidad;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using Entidad.Configuracion;

namespace Admeli.Reportes
{
    public partial class UCReporteImpuestos : UserControl
    {
        private FormPrincipal formPrincipal;
        private SucursalModel sucursalModel = new SucursalModel();
        private FechaModel fechaModel = new FechaModel();
        private ReporteModel reporteModel = new ReporteModel();
        private MonedaModel monedaModel = new MonedaModel();
        private int maxAnio = 2018;
        private int maxMes = 04;
        private string formato =  "";
        private int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        private List<Moneda> lisMoneda { get; set; }
        public UCReporteImpuestos()
        {
            InitializeComponent();
        }

        public UCReporteImpuestos(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            this.reLoad();
            formato = "{0:n" + nroDecimales + "}";
        }

        private void UCReporteImpuestos_Load(object sender, EventArgs e)
        {
            darFormatoDecimales();
            this.reLoad();
        }

        private void darFormatoDecimales()
        {
            dgvImpuestos.Columns["valorImpuesto"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvImpuestos.Columns["impuesto"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvImpuestos.Columns["utilidadProductos"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvImpuestos.Columns["utilidadFiltrada"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvImpuestos.Columns["valorImpuesto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvImpuestos.Columns["impuesto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvImpuestos.Columns["utilidadProductos"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvImpuestos.Columns["utilidadFiltrada"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        public void reLoad()
        {
            loadState(true);
            cargarFechas();
            cargarSucursales();
            cargarMonedas();
            loadState(false);
        }
        private async void cargarMonedas()
        {
            loadState(true);
            try
            {

                lisMoneda = await monedaModel.monedas();
                monedaBindingSource.DataSource = lisMoneda;
                //monedas/estado/1
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async void cargarFechas()
        {
            try
            {
                //Cargar la fecha del sistema
                FechaSistema fechaSistema = await fechaModel.fechaSistema();
                maxAnio = fechaSistema.fecha.Year;
                maxMes = fechaSistema.fecha.Month;
                limitarComboAnio(maxAnio);
                limitarComboMes(maxMes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void limitarComboMes(int limiteMes)
        {
            cbxMes.Items.Clear();
            cbxMes.Items.Add("Todo");
            for (int i = 1; i <= limiteMes; i++)
            {
                cbxMes.Items.Add(i);
            }
            cbxMes.SelectedItem = limiteMes;
        }
        private void limitarComboAnio(int limiteAnio)
        {
            cbxAnio.Items.Clear();
            cbxAnio.Items.Add("Todo");
            for (int i = 2000; i <= limiteAnio; i++)
            {
                cbxAnio.Items.Add(i);
            }
            cbxAnio.SelectedItem = limiteAnio;
        }

        private async void cargarSucursales()
        {

            try
            {
                // Cargando las categorias desde el webservice
                sucursalBindingSource.DataSource = await sucursalModel.listarSucursalesActivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Categorias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelBody.Enabled = !state;
            dgvImpuestos.Enabled = !state;
        }

        private void dibujarGrafico(decimal compras,decimal ventas,decimal ingresos, decimal egresos)
        {
            
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea());

            //Los vectores con datos
            string[] series = { "Compras", "Ventas", "Ingresos","Egresos" };
            double[] puntos = { Convert.ToDouble(compras), Convert.ToDouble(ventas), Convert.ToDouble(ingresos), Convert.ToDouble(egresos) };
            //double[] puntos = { 3000.5, 90, 30, 0 };
            //Cambiar la combinacion
            //chart1.Palette = ChartColorPalette.Pastel;

            for(int i = 0; i < series.Length; i++)
            {
                //Titulos
                Series serie = chart1.Series.Add(series[i]);
                //Cantidades
                serie.Label = puntos[i].ToString();
                serie.Points.Add(puntos[i]);
            }
        }
        private void panelBody_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnConsuitar_Click(object sender, EventArgs e)
        {
            consultar();
            
        }
        private string darformato(object dato)
        {

            return string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }
        private async void consultar()
        {
            
            try
            {
                string stringMes = cbxMes.SelectedItem.ToString();
                if (stringMes == "Todo") { stringMes = "00"; }
                if (int.Parse(stringMes) < 10) { stringMes = "0" + stringMes; }

                int idMoneda = (int)cbxMoneda.SelectedValue;
                List<ImpuestoIngresoEgreso> objetoImpuesto = await reporteModel.comprasSucursal<List<ImpuestoIngresoEgreso>>(cbxSucursal.SelectedValue.ToString(), stringMes, cbxAnio.SelectedItem.ToString(), idMoneda);
                if (objetoImpuesto.Count > 0)
                {
                    textTotalCompras.Text = darformato( objetoImpuesto[0].totalCompras);
                    textTotalVentas.Text = darformato(objetoImpuesto[0].totalVentas);
                    textTotalIngresos.Text = darformato(objetoImpuesto[0].ingresoNeto);
                    textTotalEgresos.Text = darformato(objetoImpuesto[0].egresoNeto);
                    dibujarGrafico(objetoImpuesto[0].totalCompras, objetoImpuesto[0].totalVentas, objetoImpuesto[0].ingresoNeto, objetoImpuesto[0].egresoNeto);
                }
                impuestoIngresoEgresoBindingSource.DataSource = objetoImpuesto;
                dgvImpuestos.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Filtrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void cbxAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAnio.SelectedItem.ToString() == "Todo")
            {
                limitarComboMes(12);
                return;
            }
            if (int.Parse(cbxAnio.SelectedItem.ToString()) != maxAnio)
            {
                limitarComboMes(12);
            }
            else
            {
                limitarComboMes(maxMes);
            }
        }

        
    }

    public class ObjectMes{
        public string nombre { get; set; }
        public string numero { get; set; }
    }

}
