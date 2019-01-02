using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Compras.Nuevo.Detalle;
using Entidad;

namespace Admeli.Compras.Nuevo
{
    public partial class FormProveedorNuevo : Form
    {
        private UCProveedorContacto uCProveedorContacto;
        public UCProveedorGeneral uCProveedorGeneral;

        internal int currentIDProveedor { get; set; }
        internal bool nuevo { get; set; }
        internal bool enCompras { get; set; }

        internal Proveedor currentProveedor;

        public  string nroDocumento { get; set; }
        
        public FormProveedorNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }
        public FormProveedorNuevo( string nroDocumento)
        {
            InitializeComponent();
            this.nuevo = true;
            this.nroDocumento = nroDocumento;
            this.enCompras = true;
            
        }

        public FormProveedorNuevo(Proveedor currentProveedor)
        {
            InitializeComponent();
            this.currentProveedor = currentProveedor;
            this.currentIDProveedor = currentProveedor.idProveedor;
            this.nuevo = false;
        }

        #region ==================== Estados =====================
        internal void loadStateApp(bool state)
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

        private void togglePanelMain(string panelName)
        {
            limpiarControles();
            btnColor();
            switch (panelName)
            {
                case "contacto":
                    if (uCProveedorContacto == null)
                    {
                        if (enCompras)
                        {

                            this.uCProveedorContacto = new UCProveedorContacto(this,nroDocumento);
                            this.panelMainNP.Controls.Add(uCProveedorContacto);
                            this.uCProveedorContacto.Dock = System.Windows.Forms.DockStyle.Fill;
                            this.uCProveedorContacto.Location = new System.Drawing.Point(0, 0);
                            this.uCProveedorContacto.Name = "uCProveedorContacto";
                            this.uCProveedorContacto.Size = new System.Drawing.Size(250, 776);
                            this.uCProveedorContacto.TabIndex = 0;
                        }
                        else
                        {
                            this.uCProveedorContacto = new UCProveedorContacto(this);
                            this.panelMainNP.Controls.Add(uCProveedorContacto);
                            this.uCProveedorContacto.Dock = System.Windows.Forms.DockStyle.Fill;
                            this.uCProveedorContacto.Location = new System.Drawing.Point(0, 0);
                            this.uCProveedorContacto.Name = "uCProveedorContacto";
                            this.uCProveedorContacto.Size = new System.Drawing.Size(250, 776);
                            this.uCProveedorContacto.TabIndex = 0;

                        }
                      
                    }
                    else
                    {
                        this.panelMainNP.Controls.Add(uCProveedorContacto);
                        this.uCProveedorContacto.reLoad();
                    }
                    break;
                case "general":
                    if (uCProveedorGeneral == null)
                    {

                        if (enCompras)
                        {
                            this.uCProveedorGeneral = new UCProveedorGeneral(this,nroDocumento);
                            this.panelMainNP.Controls.Add(uCProveedorGeneral);
                            this.uCProveedorGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
                            this.uCProveedorGeneral.Location = new System.Drawing.Point(0, 0);
                            this.uCProveedorGeneral.Name = "uCProveedorGeneral";
                            this.uCProveedorGeneral.Size = new System.Drawing.Size(250, 776);
                            this.uCProveedorGeneral.TabIndex = 0;


                        }
                        else
                        {
                            this.uCProveedorGeneral = new UCProveedorGeneral(this);
                            this.panelMainNP.Controls.Add(uCProveedorGeneral);
                            this.uCProveedorGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
                            this.uCProveedorGeneral.Location = new System.Drawing.Point(0, 0);
                            this.uCProveedorGeneral.Name = "uCProveedorGeneral";
                            this.uCProveedorGeneral.Size = new System.Drawing.Size(250, 776);
                            this.uCProveedorGeneral.TabIndex = 0;

                        }
                        
                    }
                    else
                    {
                        this.panelMainNP.Controls.Add(uCProveedorGeneral);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnColor()
        {
            btnGenerales.BackColor = Color.FromArgb(230, 231, 232);
            btnContacto.BackColor = Color.FromArgb(230, 231, 232);
        }

        private void limpiarControles()
        {
            this.panelMainNP.Controls.Clear();
        }

        private void btnGenerales_Click(object sender, EventArgs e)
        {
            togglePanelMain("general");
            btnGenerales.BackColor = Color.White;
        }

        private void btnContacto_Click(object sender, EventArgs e)
        {
            togglePanelMain("contacto");
            btnContacto.BackColor = Color.White;
        }

        private void FormProveedorNuevo_Load(object sender, EventArgs e)
        {
            togglePanelMain("general");
            btnGenerales.BackColor = Color.White;
            btnContacto.Enabled = (nuevo) ? false : true;
        }
    }
}
