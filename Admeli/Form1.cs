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
using System.Windows.Forms.DataVisualization.Charting;

namespace Admeli
{
    public partial class Form1 : Form
    {


        private Bunifu.Framework.UI.BunifuMetroTextbox txtprueba;

        ReporteModel reporteModel = new ReporteModel();
        List<Venta> ventas = new List<Venta>();
        int nroDecimales = 2;
        string formato { get; set; }
        public Form1()
        {

            InitializeComponent();
            //cargarRegistros();
            // 
            // bunifuMetroTextbox1
            // 

            formato = "{0:n" + nroDecimales + "}";


        }
        private string darformato(object dato)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }
        private void bunifuCustomTextbox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //textBox1.Focus();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem dd = sender as ToolStripMenuItem;

            ContextMenuStrip  contextMenuStrip  =(ContextMenuStrip)   dd.Owner;
            Control control = contextMenuStrip.SourceControl;
            this.Controls.Remove(control);
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            fontDialog.ShowApply = true;
            fontDialog.ShowEffects = true;
            fontDialog.ShowHelp = true;
            fontDialog.MinSize = 7;
            fontDialog.MaxSize = 40;


            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ToolStripMenuItem dd = sender as ToolStripMenuItem;

                ContextMenuStrip contextMenuStrip = (ContextMenuStrip)dd.Owner;
                Control control = contextMenuStrip.SourceControl;
                control.Font = fontDialog.Font;
                control.ForeColor = fontDialog.Color;

            }


            Color myColor = Color.FromArgb(255, 181, 178);

            string hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            
            

        }

        private void dataGridView1_AllowUserToOrderColumnsChanged(object sender, EventArgs e)
        {
           
         }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
          
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_Move(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
          
        }

        private void dataGridView1_DragLeave(object sender, EventArgs e)
        {
          
         
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
           
           
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private  void Form1_Load(object sender, EventArgs e)
        {
           cargarRegistros();
        }
        private async void cargarRegistros()
        {

            try
            {              
                List<ReporteIE> objetoImpuesto = await reporteModel.reporteIngresoMenosEngreso<List<ReporteIE>>(1, "2018-08-1", "2018-08-30", 1);
                // cargar el char el grafico
                int nro = objetoImpuesto.Count;
                double[] valoresingreso = new double[nro];
                double[] valoresegreso = new double[nro];
                double[] valoressobrante = new double[nro];
                string[] fechas = new string[nro];
                int j = 0;
                for (int i = 0; i < objetoImpuesto.Count; i++)
                {
                    valoresingreso[j] = objetoImpuesto[i].ingreso;
                    valoresegreso[j] = objetoImpuesto[i].egreso;
                    valoressobrante[j] = objetoImpuesto[i].sobrante;
                    fechas[j] = objetoImpuesto[i].fecha.date.Month+"/"+ objetoImpuesto[i].fecha.date.Day;
                    j++;
                }
                dibujarGrafico(valoresingreso, valoresegreso, valoressobrante, fechas);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Filtrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dibujarGrafico(double[] valoresingreso, double[] valoresegreso, double[] valoressobrante,string[] fechas) 
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

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
