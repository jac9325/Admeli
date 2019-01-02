using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
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

namespace Admeli.Ventas.Nuevo.detalle
{

    public partial class Forminfo : Form
    {


        // parver la moneda
        MonedaModel monedaModel = new MonedaModel();
        private CotizacionModel cotizacionModel = new CotizacionModel();
        private double total { get; set; }
        public bool omitir { get; set; }
        public bool guardar { get; set; }
        public double adelanto { get; set; }
        public double porcentaje { get; set; }
        int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        string formato { get; set; }
        private double adelantosA { get; set; }
        DetalleV detalles { get; set; }
        private bool lisenerKeyEvents = true;
        public Forminfo( DetalleV detalles)
        {
            InitializeComponent();
            formato = "{0:n" + nroDecimales + "}";
            porcentaje = 10;
            omitir = false;
            guardar = false;
            adelanto = 0;
            this.total = total;
            txtCelular.Text = darformato(adelanto);
            this.detalles = detalles;
            if (detalles.estadoPedido == 7)
            {
                
                btnVenta.Enabled = true;
            }
            else
            {
                
                btnVenta.Enabled = false;
            }
        }

        private void FormMotivo_Load(object sender, EventArgs e)
        {

            this.ParentChanged += ParentChange;
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }

            cargarDetallePedido();
            cargarMoneda();
        }
        public async  void cargarMoneda()
        {

            List <Moneda>    monedas = await monedaModel.monedas();
            Moneda moneda = monedas.Find(X=>X.idMoneda== detalles.idMoneda);
            txtMoneda.Text = moneda.moneda;
            txtPrecioVenta.Text =moneda.simbolo+". " +darformato( detalles.precioVenta);
            txtResumen.Text = detalles.resumenAdelanto;

        }
        private void cargarDetallePedido()
        {

            txtCelular.Text = detalles.celular;
            txtNombre.Text = detalles.nombreCliente;
          
        }
        

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
                case Keys.Escape: // productos
                    cerrar();
                    break;
            }
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

            if (texto == "")
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
        private  void btnGuardar_Click(object sender, EventArgs e)
        {
            guardarAdelanto();
        }

        private void guardarAdelanto()
        {

            bloquear(true);
            try
            {
                double adelantoaux = toDouble(txtCelular.Text.Trim());
                if (adelantoaux + adelantosA > total)
                {

                    double resto = total - adelantosA;
                    txtCelular.Text = darformato(resto);
                    MessageBox.Show("La suma de los  adelantos excenden al total  ", "Comprobar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    bloquear(false);
                    txtCelular.Focus();
                  
                }
                else
                {
                    guardar = true;
                    adelanto = toDouble(txtCelular.Text);
                    this.Close();

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "btnGuardar Response", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bloquear(false);

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            cerrar();
        }
        private void cerrar()
        {

            omitir = false;
            this.Close();
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

        private void txtAdelanto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, txtCelular.Text);
        }

        private void txtAdelanto_OnValueChanged(object sender, EventArgs e)
        {
           
        }

        private void txtAdelanto_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                guardarAdelanto();

            }
        }

        private async void btnVenta_Click(object sender, EventArgs e)
        {
            CotizacionBuscar currentCotizacion = null;
 try
            {
                currentCotizacion = await cotizacionModel.pedidoSeleccionado(detalles.idCotizacion);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "cargar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            FormVentaNewR venta = new FormVentaNewR(currentCotizacion);
           
            
            venta.ShowDialog();
            
        }
    }
}
