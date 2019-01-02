using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Compras;

namespace Admeli.Navigation.SubMenu
{
    public partial class UCComprasNav : UserControl
    {
        private UCCompras uCCompras;      
        private UCOrdenCompraProveedor uCOrdenCompraProveedor;
        private UCProveedores uCProveedores;

        private FormPrincipal formPrincipal;
        private UCTiendaRoot uCTiendaRoot;

        public UCComprasNav()
        {
            InitializeComponent();
        }
        
        public UCComprasNav(FormPrincipal formPrincipal, UCTiendaRoot uCTiendaRoot)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            this.uCTiendaRoot = uCTiendaRoot;
        }

        private void togglePanelMain(string panelName)
        {
            this.limpiarControles();
            switch (panelName)
            {
                case "compras":
                    if (uCCompras == null)
                    {
                        this.uCCompras = new Admeli.Compras.UCCompras(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCCompras);
                        this.uCCompras.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCCompras.Location = new System.Drawing.Point(0, 0);
                        this.uCCompras.Name = "uCCompras";
                        this.uCCompras.Size = new System.Drawing.Size(250, 776);
                        this.uCCompras.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCCompras);
                        this.uCCompras.reLoad();
                    }
                    this.formPrincipal.lblTitlePage.Text = "Compra - Compras";
                    break;
            
                case "ordenCompraProveedor":
                    if (uCOrdenCompraProveedor == null)
                    {
                        this.uCOrdenCompraProveedor = new Admeli.Compras.UCOrdenCompraProveedor(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCOrdenCompraProveedor);
                        this.uCOrdenCompraProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCOrdenCompraProveedor.Location = new System.Drawing.Point(0, 0);
                        this.uCOrdenCompraProveedor.Name = "uCOrdenCompraProveedor";
                        this.uCOrdenCompraProveedor.Size = new System.Drawing.Size(250, 776);
                        this.uCOrdenCompraProveedor.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCOrdenCompraProveedor);
                        this.uCOrdenCompraProveedor.reLoad();
                    }
                    this.formPrincipal.lblTitlePage.Text = "Compra - Orden de compra proveedor";
                    break;
                case "proveedores":
                    if (uCProveedores == null)
                    {
                        this.uCProveedores = new Admeli.Compras.UCProveedores(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCProveedores);
                        this.uCProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCProveedores.Location = new System.Drawing.Point(0, 0);
                        this.uCProveedores.Name = "uCProveedores";
                        this.uCProveedores.Size = new System.Drawing.Size(250, 776);
                        this.uCProveedores.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCProveedores);
                        this.uCProveedores.reLoad();
                    }
                    this.formPrincipal.lblTitlePage.Text = "Compra - Proveedores";
                    break;
                default:
                    break;
            }
        }

        private void limpiarControles()
        {
            this.formPrincipal.panelMain.Controls.Clear();
          
            
            
            
            
            /// Ocultar el menu derecho del formulario principal
        }

        private void btnColor()
        {
            /// Reset Color buttons
            btnOrdenCompra.Textcolor = Color.FromArgb(139, 138, 141);
            btnCompra.Textcolor = Color.FromArgb(139, 138, 141);
            btnProveedor.Textcolor = Color.FromArgb(139, 138, 141);
        }

        private void btnOrdenCompra_Click(object sender, EventArgs e)
        {
            btnColor(); /// Color reset
            togglePanelMain("ordenCompraProveedor");
            btnOrdenCompra.Textcolor = Color.White; /// Color
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            btnColor(); /// Color reset
            togglePanelMain("compras");
            btnCompra.Textcolor = Color.White; /// Color
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            btnColor(); /// Color reset
            togglePanelMain("proveedores");
            btnProveedor.Textcolor = Color.White; /// Color
        }


    }
}
