using Admeli.Configuracion.Nuevo;
using Bunifu.Framework.UI;
using Entidad;
using Entidad.Configuracion;
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

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class FormPrecioDetalle : Form
    {
        private MonedaModel monedaModel = new MonedaModel();
        private SucursalModel sucursalModel = new SucursalModel();
        private ImpuestoModel impuestoModel = new ImpuestoModel();
        private PrecioModel precioModel = new PrecioModel();
        private Decimal precioCompra;
        private double sumaImpuestosProducto=0;
        private Precio currentPrecio;
        private int currentidProducto;

        public FormPrecioDetalle()
        {
            InitializeComponent();
        }

        public FormPrecioDetalle(Precio currentPrecio)
        {
            InitializeComponent();
            this.currentPrecio = currentPrecio;
        }

        public FormPrecioDetalle(Precio currentPrecio, Decimal precioCompra,int idProducto)
        {
            InitializeComponent();
            this.currentPrecio = currentPrecio;
            this.precioCompra = precioCompra;
            this.currentidProducto = idProducto;
            this.reLoad();
        }

        private void cargarDatosModificar()
        {
            textPrecioCompra.Text = precioCompra.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            textPrecioVenta.Text = currentPrecio.precioVenta.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            textPrecioCompetencia.Text = currentPrecio.precioCompetencia.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            textPrecioUtilidad.Text = currentPrecio.utilidad.ToString(ConfigModel.configuracionGeneral.formatoDecimales);
            cbxMoneda.SelectedValue = currentPrecio.idMoneda;
            cbxSucursal.SelectedValue = currentPrecio.idSucursal;
        }

        private void FormPrecioDetalle_Load(object sender, EventArgs e)
        {
        }

        private void reLoad()
        {
            this.cargarImpuestos();
            this.cargarMonedas();
            this.cargarSucursales();
        }

        private async void cargarSucursales()
        {
            sucursalBindingSource.DataSource = await sucursalModel.sucursales();
        }

        private async void cargarMonedas()
        {
            monedaBindingSource.DataSource = await monedaModel.monedas();
            Moneda moneda = await monedaModel.monedaPorDefecto();
            cbxMoneda.SelectedValue = moneda.idMoneda;

            // mostrando los datos de modificar
            cargarDatosModificar();
        }

        private async void cargarImpuestos()
        {
            try
            {
                List<Impuesto> impuestos = await impuestoModel.impuestoProductoSucursal(currentidProducto, currentPrecio.idSucursal);
                int columnas = 1; // Indicar numero de columnas de la grilla
                int x = 13;
                int y = 13;

                // ===== Algoritmo para crear una grilla
                int items = impuestos.Count % columnas;      // Detectar cuandos elementos hay en la ultima fila de la grilla
                int rowComplete = Convert.ToInt32(Math.Floor((decimal)(impuestos.Count / columnas))) * columnas; // detectar cuantos fila esta lleno de  registros
                for (int i = 0; i < impuestos.Count; i++) // for para las filas
                {
                    sumaImpuestosProducto += (impuestos[i].valorImpuesto/100)*double.Parse(precioCompra.ToString());
                    for (int j = 0; j < columnas; j++) // For para las columnas
                    {
                        if (i > rowComplete) // validacion
                        {
                            if (items == j) break; // salir de este for
                        }
                        this.createElement(panelImpuestos.Controls, x, y, impuestos[i].siglasImpuesto, impuestos[i].valorImpuesto.ToString());
                        i = (columnas - 1 == j) ? i : i + 1; // indice de registros aumento
                        x += 170; // cordenada x aumentado
                    }
                    y += 60; // cordenada y
                    x = 13; // cordenada x regresando al valor original
                }
                textPrecioConImpuesto.Text = (sumaImpuestosProducto+ double.Parse(precioCompra.ToString())).ToString();
                // =====
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Carga Impuestos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void createElement(Control.ControlCollection controls, int x, int y, string labelValue, string textBoxValue, string key = "", int whidt = 288, int height = 35, int gap = 10)
        {
            Label titlefield = new Label()
            {
                AutoSize = true,
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = System.Drawing.Color.DimGray,
                Location = new System.Drawing.Point(x,y),
                Margin = new System.Windows.Forms.Padding(2, 0, 2, 0),
                Name = "label1111",
                Size = new System.Drawing.Size(127, 16),
                TabIndex = 8,
                Text = labelValue
            };

            BunifuMetroTextbox textBoxBF1 = new BunifuMetroTextbox()
            {
                BackColor = System.Drawing.Color.White,
                BorderColorFocused = System.Drawing.Color.DodgerBlue,
                BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(157))))),
                BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(157))))),
                BorderThickness = 1,
                Cursor = System.Windows.Forms.Cursors.IBeam,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))),
                isPassword = false,
                Location = new System.Drawing.Point(x, y + 16),
                Margin = new System.Windows.Forms.Padding(4),
                Name = "textBox1111",
                Padding = new System.Windows.Forms.Padding(6, 0, 6, 0),
                Size = new System.Drawing.Size(whidt, height),
                TabIndex = 9,
                TextAlign = System.Windows.Forms.HorizontalAlignment.Left,
                Text = textBoxValue,
                Enabled = false
            };
            controls.Add(titlefield);
            controls.Add(textBoxBF1);
        }

        private void btnAddMoneda_Click(object sender, EventArgs e)
        {
            FormMonedaNuevo formMoneda = new FormMonedaNuevo();
            formMoneda.ShowDialog();
            this.cargarMonedas();
        }

        private void btnAddSucursal_Click(object sender, EventArgs e)
        {
            FormSucursalNuevo formSucursalNuevo = new FormSucursalNuevo();
            formSucursalNuevo.ShowDialog();
            this.cargarSucursales();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ejecutarGuardar();
        }

        private void cargarValores()
        {
            currentPrecio.idProducto = this.currentidProducto;
            currentPrecio.utilidad = Decimal.Parse(textPrecioUtilidad.Text);
            currentPrecio.precioVenta = Decimal.Parse(textPrecioVenta.Text);
            currentPrecio.precioCompetencia = Decimal.Parse(textPrecioCompetencia.Text);
        }

        private async void ejecutarGuardar()
        {
            try
            {
                //Crear objeto Precio
                cargarValores();
                //Guardamos el PrecioProducto
                Response response = await precioModel.precioModificar(currentPrecio);
                MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textPrecioUtilidad_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(textPrecioUtilidad.Text))
            {
                textPrecioVenta.Text = (double.Parse(textPrecioConImpuesto.Text)).ToString();
            }
            else
            {
                textPrecioVenta.Text = (double.Parse(textPrecioConImpuesto.Text) + (double.Parse(textPrecioConImpuesto.Text) * double.Parse(textPrecioUtilidad.Text) / 100)).ToString();
            }
        }

        private void textPrecioVenta_KeyUp(object sender, KeyEventArgs e)
        {
            if (double.Parse(textPrecioConImpuesto.Text) > 0)
            {

                if (string.IsNullOrEmpty(textPrecioVenta.Text))
                {
                    textPrecioUtilidad.Text = (((0 - double.Parse(textPrecioConImpuesto.Text)) / double.Parse(textPrecioConImpuesto.Text)) * 100).ToString();
                }
                else
                {
                    textPrecioUtilidad.Text = (((double.Parse(textPrecioVenta.Text) - double.Parse(textPrecioConImpuesto.Text)) / double.Parse(textPrecioConImpuesto.Text)) * 100).ToString();
                }
            }
        }
    }

}
