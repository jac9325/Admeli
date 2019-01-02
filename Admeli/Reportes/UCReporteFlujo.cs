using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Modelo;
using Entidad;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using Entidad.Configuracion;

namespace Admeli.Reportes
{
    public partial class UCReporteFlujo : UserControl
    {

        private ReporteModel reporteModel = new ReporteModel();
        private MonedaModel monedaModel = new MonedaModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private FechaModel fechaModel = new FechaModel();
        private Precio currentPrecio { get; set; }
        private Stock currentStock { get; set; }
        private FormPrincipal formPrincipal { get; set; }
        private List<FlujoDia> ListPaneles { get; set; }
        private List<Moneda> lisMoneda { get; set; }

        public bool lisenerKeyEvents { get; internal set; }
        int nroDecimales = 2;
        string formato { get; set; }

        #region ================================ Constructor ================================
        public UCReporteFlujo()
        {
            InitializeComponent();
            formato = "{0:n" + nroDecimales + "}";
        }

        public UCReporteFlujo(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            this.reLoad();
            formato = "{0:n" + nroDecimales + "}";
        }
        private string darformato(object dato)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }

        #endregion

        #region =========================== Root Load ===========================
        private void UCStockPD_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }



        internal void reLoad()
        {
            plDivisor.Controls.Clear();
            plDivisor.RowStyles.Clear();
            cargarFechas();
            cargarSucursales();
            cargarMonedas();
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

        }
        #endregion

        #region =========================== Loads ===========================

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
            finally
            {

                loadState(false);
            }
        }
        private async void cargarFechas()
        {
            try
            {
                //Cargar la fecha del sistema
                FechaSistema fechaSistema = await fechaModel.fechaSistema();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            tabDetalle.Enabled = !state;
            tabGrafico.Enabled = !state;
        }
        #endregion

        #region ============================== Paint decoration ==============================
        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panelHeader);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
                                                            DrawShape drawShape = new DrawShape();
           
            drawShape.bottomLine(panel2);
          
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panel5);
           
        }
        #endregion
        private void btnActualizarStock_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarSalir_Click(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }

        private async void cargarRegistros()
        {
            string fechaI = String.Format("{0:u}", dtpFInicio2.Value);
            fechaI = fechaI.Substring(0, fechaI.Length - 1);
            string[] fechas = fechaI.Split(' ');
            fechaI = fechas[0];
            string fechaF = String.Format("{0:u}", dtpFFinal2.Value);
            fechaF = fechaF.Substring(0, fechaF.Length - 1);
            string[] fechasf = fechaF.Split(' ');
            fechaF = fechasf[0];
            int idSucursal = (int)cbxSucursal2.SelectedValue;
            int idMoneda = (int)cbxMoneda2.SelectedValue;
            try
            {
                List<ReporteIE> objetoImpuesto = await reporteModel.reporteIngresoMenosEngreso<List<ReporteIE>>(idSucursal, fechaI, fechaF, idMoneda);
                // cargar el char el grafico
                int nro = objetoImpuesto.Count;
                List<  double> valoresingreso = new List<double>();
                List<double> valoresegreso = new List<double>();
                List<double> valoressobrante = new List<double>();
                List<string> fechas1 = new List<string>();
                int j = 0;
                for (int i = 0; i < objetoImpuesto.Count; i++)
                {
                    if (objetoImpuesto[i].sobrante == 0)
                    {
                        continue;
                    }
                    valoresingreso.Add(objetoImpuesto[i].ingreso);
                    valoresegreso.Add(objetoImpuesto[i].egreso);
                    valoressobrante.Add(objetoImpuesto[i].sobrante);
                    fechas1.Add(objetoImpuesto[i].fecha.date.Month + "/" + objetoImpuesto[i].fecha.date.Day);
                    j++;
                }
                dibujarGrafico(valoresingreso.ToArray(), valoresegreso.ToArray(), valoressobrante.ToArray(), fechas1.ToArray());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Filtrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void dibujarGrafico(double[] valoresingreso, double[] valoresegreso, double[] valoressobrante, string[] fechas)
        {

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea());

            //Los vectores con datos
            string[] series = { "Ingresos", "Egresos", "Sobrante" };

            for (int i = 0; i < series.Length; i++)
            {
                //Titulos
                Series serie = chart1.Series.Add(series[i]);
                //Cantidades


            }


            for (int j = 0; j < valoresingreso.Length; j++)
            {
                this.chart1.Series["Ingresos"].Points.AddXY(fechas[j], valoresingreso[j]);
                this.chart1.Series["Ingresos"].Points[j].Label = darformato(valoresingreso[j]);
                this.chart1.Series["Egresos"].Points.AddXY(fechas[j], valoresegreso[j]);
                this.chart1.Series["Egresos"].Points[j].Label = darformato(valoresegreso[j]);
                this.chart1.Series["Sobrante"].Points.AddXY(fechas[j], valoressobrante[j]);
                this.chart1.Series["Sobrante"].Points[j].Label = darformato(valoressobrante[j]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargarRegistros1();
        }
        private async void cargarRegistros1()
        {

            try
            {

                string fechaI = String.Format("{0:u}", dtpFInicio1.Value);
                fechaI = fechaI.Substring(0, fechaI.Length - 1);
                string[] fechas = fechaI.Split(' ');
                fechaI = fechas[0];
                string fechaF = String.Format("{0:u}", dtpFFinal1.Value);
                fechaF = fechaF.Substring(0, fechaF.Length - 1);
                string[] fechasf = fechaF.Split(' ');
                fechaF = fechasf[0];
                int idSucursal = (int)cbxSucursal1.SelectedValue;
                int idMoneda = (int)cbxMoneda1.SelectedValue;

                plDivisor.Controls.Clear();
                plDivisor.RowStyles.Clear();
                List<ReporteIE> objetoImpuesto = await reporteModel.reporteIngresoMenosEngreso<List<ReporteIE>>(idSucursal, fechaI, fechaF, idMoneda);
                ListPaneles = new List<FlujoDia>();


                // cargar el char el grafico
                int nro = objetoImpuesto.Count;
             
                int j = 0;
                int dia = (int)objetoImpuesto[0].fecha.date.DayOfWeek;
                int column = dia;// 
                int row = 0;
                CultureInfo ci = new CultureInfo("Es-Es");
                Console.WriteLine(ci.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek));
                for (int i = 0; i < objetoImpuesto.Count; i++)
                { 
                    FlujoDia flujoDia = new FlujoDia();

                    //
                    flujoDia.lbTitulo.Text = objetoImpuesto[i].fecha.date.Day +" "+ ci.DateTimeFormat.GetDayName(objetoImpuesto[i].fecha.date.DayOfWeek);
                    flujoDia.lbIngreso.Text = "Ingreso: " + darformato( objetoImpuesto[i].ingreso);
                    flujoDia.lbEgreso.Text = "Egreso: " + darformato(objetoImpuesto[i].egreso); 
                    flujoDia.lbDiferencia.Text = "Sobrante: " + darformato(objetoImpuesto[i].sobrante) ;
                    this.plDivisor.Controls.Add(flujoDia.ContenedorDia, column, row);
                    column++;
                    if (column == 7)
                    {
                        column = 0;
                        row ++;

                    }
            
                }
              

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Filtrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cargarRegistros1();

        }

        private void dtpFInicio1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateInicial = dtpFInicio1.Value;
            DateTime dateFinal = dtpFFinal1.Value;
            int comparar = dateInicial.CompareTo(dateFinal);// -1 menor 0 igual 1 mayor
            if (comparar > 0)
            {
                dtpFInicio1.Value = dateFinal;

            }
        }

        private void dtpFFinal1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateInicial = dtpFInicio1.Value;
            DateTime dateFinal = dtpFFinal1.Value;
            int comparar = dateInicial.CompareTo(dateFinal);// -1 menor 0 igual 1 mayor
            if (comparar > 0)
            {
                dtpFFinal1.Value = dateInicial;

            }
        }

        private void dtpFInicio2_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateInicial = dtpFInicio2.Value;
            DateTime dateFinal = dtpFFinal2.Value;
            int comparar = dateInicial.CompareTo(dateFinal);// -1 menor 0 igual 1 mayor
            if (comparar > 0)
            {
                dtpFInicio2.Value = dateFinal;

            }
        }

        private void dtpFFinal2_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateInicial = dtpFInicio2.Value;
            DateTime dateFinal = dtpFFinal2.Value;
            int comparar = dateInicial.CompareTo(dateFinal);// -1 menor 0 igual 1 mayor
            if (comparar > 0)
            {
                dtpFFinal2.Value = dateInicial;
            }
        }

       
    }
}
