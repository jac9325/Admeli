using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Ventas;

namespace Admeli.Navigation.SubMenu
{
    public partial class UCVentasNav : UserControl
    {
        private UCClientes uCClientes;
        private UCCotizacionCliente uCCotizacionCliente;       
        private UCVentas uCVentas;
        private UCDescuentosOfertas uCDescuentosOfertas;
        private UCPedidos uCPedidos;
        private FormPrincipal formPrincipal;
        private UCTiendaRoot uCTiendaRoot;

        public UCVentasNav()
        {
            InitializeComponent();
        }
        
        public UCVentasNav(FormPrincipal formPrincipal, UCTiendaRoot uCTiendaRoot)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            this.uCTiendaRoot = uCTiendaRoot;
        }

        private void togglePanelMain(string panelName)
        {
            limpiarControles();
            switch (panelName)
            {
                case "clientes":
                    if (uCClientes == null)
                    {
                        this.uCClientes = new Admeli.Ventas.UCClientes(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCClientes);
                        this.uCClientes.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCClientes.Location = new System.Drawing.Point(0, 0);
                        this.uCClientes.Name = "uCClientes";
                        this.uCClientes.Size = new System.Drawing.Size(250, 776);
                        this.uCClientes.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCClientes);
                        this.uCClientes.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Venta - Clientes"; /// Titulo en el encabezado
                    break;
                case "contizacionCliente":
                    if (uCCotizacionCliente == null)
                    {
                        this.uCCotizacionCliente = new Admeli.Ventas.UCCotizacionCliente(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCCotizacionCliente);
                        this.uCCotizacionCliente.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCCotizacionCliente.Location = new System.Drawing.Point(0, 0);
                        this.uCCotizacionCliente.Name = "uCCotizacionCliente";
                        this.uCCotizacionCliente.Size = new System.Drawing.Size(250, 776);
                        this.uCCotizacionCliente.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCCotizacionCliente);
                        this.uCCotizacionCliente.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Venta - Contizaciones cliente"; /// Titulo en el encabezado
                    break;
               
                case "ventas":
                    if (uCVentas == null)
                    {
                            this.uCVentas = new Admeli.Ventas.UCVentas(this.formPrincipal);
                            this.formPrincipal.panelMain.Controls.Add(uCVentas);
                            this.uCVentas.Dock = System.Windows.Forms.DockStyle.Fill;
                            this.uCVentas.Location = new System.Drawing.Point(0, 0);
                            this.uCVentas.Name = "uCVentas";
                            this.uCVentas.Size = new System.Drawing.Size(250, 776);
                            this.uCVentas.TabIndex = 0;
                        }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCVentas);
                        this.uCVentas.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Venta - Ventas"; /// Titulo en el encabezado
                    break;
                case "ventaTouch":
                    FormVentaTouch ventaTouch = new FormVentaTouch();
                    ventaTouch.ShowDialog();
                    break;
                case "descuentoOferta":
                    if(uCDescuentosOfertas == null)
                    {
                        this.uCDescuentosOfertas = new Admeli.Ventas.UCDescuentosOfertas(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCDescuentosOfertas);
                        this.uCDescuentosOfertas.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCDescuentosOfertas.Location = new System.Drawing.Point(0, 0);
                        this.uCDescuentosOfertas.Name = "UCDescuentosOfertas";
                        this.uCDescuentosOfertas.Size = new System.Drawing.Size(250, 776);
                        this.uCDescuentosOfertas.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCDescuentosOfertas);
                        //this.uCDescuentosOfertas.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Descuentos - Ofertas"; /// Titulo en el encabezado
                    break;
                case "Pedidos":
                    if (uCPedidos == null)
                    {
                        this.uCPedidos = new Admeli.Ventas.UCPedidos(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCPedidos);
                        this.uCPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCPedidos.Location = new System.Drawing.Point(0, 0);
                        this.uCPedidos.Name = "UCDescuentosOfertas";
                        this.uCPedidos.Size = new System.Drawing.Size(250, 776);
                        this.uCPedidos.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCPedidos);
                        //this.uCDescuentosOfertas.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Pedidos"; /// Titulo en el encabezado
                    break;
                default:
                    break;
            }
        }

        private void limpiarControles()
        {
            this.formPrincipal.panelMain.Controls.Clear();
            this.formPrincipal.hideMenuRight(); /// Ocultar el menu derecho del formulario principal
        }

        private void btnColor()
        {
            /// Reset Color buttons
            btnCotizacionCliente.Textcolor = Color.FromArgb(139, 138, 141);
            btnVentas.Textcolor = Color.FromArgb(139, 138, 141);
            btnClientes.Textcolor = Color.FromArgb(139, 138, 141);
            btnDescuentoOferta.Textcolor = Color.FromArgb(139, 138, 141);
        }

        private void btnCotizacionCliente_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset Color
            togglePanelMain("contizacionCliente");
            btnCotizacionCliente.Textcolor = Color.White; /// COlor Active
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset Color
            togglePanelMain("ventas");
            btnVentas.Textcolor = Color.White; /// COlor Active
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset Color
            togglePanelMain("clientes");
            btnClientes.Textcolor = Color.White; /// COlor Active
        }

        private void btnDescuentoOferta_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset Color
            togglePanelMain("descuentoOferta");
            btnDescuentoOferta.Textcolor = Color.White; /// COlor Active
        }

        private void btnVentaTocuh_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset Color
            togglePanelMain("ventaTouch");
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset Color
            togglePanelMain("Pedidos");
        }
    }
}
