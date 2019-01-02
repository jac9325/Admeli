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
using Entidad.Configuracion;
using Bunifu.Framework.UI;
using Entidad;
using System.Globalization;
using Admeli.Componentes;

namespace Admeli.Configuracion
{
    public partial class UCTipoCambio : UserControl
    {
        private MonedaModel monedaModel = new MonedaModel();

        private FormPrincipal formPrincipal;
        public bool lisenerKeyEvents { get; set; }
        private List<Moneda> monedas { get; set; }
        private Moneda monedaPorDefecto { get; set; }

        private List<SaveObject> saveObjects { get; set; }

        public UCTipoCambio()
        {
            InitializeComponent();
            lisenerKeyEvents = true; // Active lisener key events
        }

        public UCTipoCambio(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            lisenerKeyEvents = true; // Active lisener key events
            
        }

        #region ========================== Root Load ==========================
        private void UCTipoCambio_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarMonedas();
            }
            lisenerKeyEvents = true; // Active lisener key events
        }
        #endregion

        #region ======================== Loads ========================
        private async void cargarMonedas()
        {
            try
            {
                loadState(true); // cambiando de estado
                monedas = await monedaModel.monedas(); // Cargando las monedas
                int y = 170; // POsition initial elements (TextBox AND Label)

                // Buscando la moneda por defecto
                foreach (Moneda money in monedas)
                {
                    if (money.porDefecto)
                    {
                        textNombre.Text = String.Format("{0} {1}", money.nombres, money.apellidos);
                        dtpFechaIngreso.Value = (money.fechaCreacion == null) ? DateTime.Now : money.fechaCreacion.date;
                        lblMonedaPorDefecto.Text = String.Format("Moneda por defecto: {0}", money.moneda);
                        monedaPorDefecto = money;
                    }
                }

                // Creando los campos para cada moneda
                foreach (Moneda money in monedas)
                {
                    if (!money.porDefecto)
                    {
                        crearElementosMoneda(money, y);
                        y += 60;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upps! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }

        } 
        #endregion

        #region ============================= Crear Elementos =============================
        private void crearElementosMoneda(Moneda moneda, int y)
        {

            Label lblMonedaLabel = new Label()
            {
                // creando un nuevo label
                AutoSize = true,
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = System.Drawing.Color.DodgerBlue,
                Margin = new System.Windows.Forms.Padding(2, 0, 2, 0),
                Size = new System.Drawing.Size(52, 14),

                // Modificar al gusto del cliente
                Name = "lbl" + moneda.idMoneda,
                Text = String.Format("1 {0} es igual a ... {1} ↓", monedaPorDefecto.moneda.ToUpper(), moneda.moneda.ToUpper()),
                Location = new System.Drawing.Point(25, (y + 8)),
                TabIndex = 93,
            };

            BunifuMetroTextbox textMoneda1 = new BunifuMetroTextbox()
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
                Margin = new System.Windows.Forms.Padding(4),
                Padding = new System.Windows.Forms.Padding(5, 18, 5, 0),
                Size = new System.Drawing.Size(350, 50),
                TextAlign = System.Windows.Forms.HorizontalAlignment.Left,

                // al gusto del cliente
                Location = new System.Drawing.Point(18, y),
                Name = moneda.idMoneda.ToString(),
                TabIndex = 91,
                Text = string.Format(CultureInfo.GetCultureInfo("en-US"), "{0:n" + ConfigModel.configuracionGeneral.numeroDecimales + "}", moneda.tipoCambio)

            //OnValueChanged += new System.EventHandler(this.bunifuMetroTextbox1_OnValueChanged);
        };

           

            textMoneda1.KeyPress += new KeyPressEventHandler(this.textMoneda1_OnValueChanged);


            // Agreganto los dos elementos
            this.panelContainer.Controls.Add(lblMonedaLabel);
            this.panelContainer.Controls.Add(textMoneda1);
            Control[] textMoneda3  = this.panelContainer.Controls.Find("1002",false);

            //Se comentó, porque genra error y no sabe para que es usado
            //if (textMoneda3 != null)
            //{
            //    BunifuMetroTextbox textMoneda14 = textMoneda3[0] as BunifuMetroTextbox;
            //}
        }

        private void textMoneda1_OnValueChanged(object sender, KeyPressEventArgs e)
        {

            BunifuMetroTextbox textMoneda1 = sender as BunifuMetroTextbox;
            Validator.isDecimal(e, textMoneda1.Text);
        }
        #endregion

        #region ================================ SAVE AND UPDATE ================================
        private async void guardar()
        {
            try
            {
                loadState(true);
                foreach (SaveObject item in saveObjects)
                {
                    Response response = await monedaModel.tipoCambioGuardar(item);
                }
                MessageBox.Show("Se guardo correctamente", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.reLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
        }

        private void crearObjeto()
        {
            saveObjects = new List<SaveObject>();
            foreach (Moneda moneda in monedas)
            {
                SaveObject save = new SaveObject();
                save.cambioCompra = moneda.tipoCambio;
                save.cambioVenta = moneda.tipoCambio;
                save.estado = moneda.estado;
                save.idMoneda = monedaPorDefecto.idMoneda;
                save.idMonedaCambio = moneda.idMoneda;
                save.idPersonal = PersonalModel.personal.idPersonal;
                saveObjects.Add(save);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            crearObjeto();
            guardar();
        }

        struct SaveObject
        {
            public double cambioCompra { get; set; }
            public double cambioVenta { get; set; }
            public int estado { get; set; }
            public int idMoneda { get; set; }
            public int idMonedaCambio { get; set; }
            public int idPersonal { get; set; }
        }

        #endregion

        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
        }

        private void textNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, textNombre.Text);
        }
    }
}
