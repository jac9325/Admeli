using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Reportes;

namespace Admeli.Navigation.SubMenu
{
    public partial class UCReporteNav : UserControl
    {
        private UCTiendaRoot uCTiendaRoot;
        private FormPrincipal formPrincipal;

        public UCReporteExisteciaProductos uCReporteExisteciaProductos;
        public UCReporteIngresoVentas uCReporteIngresoVentas;
        public UCReporteImpuestos uCReporteImpuestos;
        public UCReporteFlujo uCReporteFlujo;

        public UCReporteNav()
        {
            InitializeComponent();
        }

        public UCReporteNav(FormPrincipal formPrincipal, UCTiendaRoot uCTiendaRoot)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            this.uCTiendaRoot = uCTiendaRoot;
        }
        public void togglePanelMain(string panelName)
        {
            limpiarControles();
            switch (panelName)
            {
                case "reporteProductos":
                    if ( uCReporteExisteciaProductos== null)
                    {
                        this.uCReporteExisteciaProductos = new Admeli.Reportes.UCReporteExisteciaProductos(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCReporteExisteciaProductos);
                        this.uCReporteExisteciaProductos.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCReporteExisteciaProductos.Location = new System.Drawing.Point(0, 0);
                        this.uCReporteExisteciaProductos.Name = "uCReporteExisteciaProductos";
                        this.uCReporteExisteciaProductos.Size = new System.Drawing.Size(250, 776);
                        this.uCReporteExisteciaProductos.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCReporteExisteciaProductos);
                        this.uCReporteExisteciaProductos.reload();
                    }
                    formPrincipal.lblTitlePage.Text = "Reporte - Existencia de Poductos"; /// Titulo en el encabezado
                    break;
                case "reporteFlujo":
                    if (uCReporteFlujo == null)
                    {
                        this.uCReporteFlujo = new Admeli.Reportes.UCReporteFlujo(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCReporteFlujo);
                        this.uCReporteFlujo.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCReporteFlujo.Location = new System.Drawing.Point(0, 0);
                        this.uCReporteFlujo.Name = "uCReporteIngresoVentas";
                        this.uCReporteFlujo.Size = new System.Drawing.Size(250, 776);
                        this.uCReporteFlujo.TabIndex = 0;
                        
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCReporteFlujo);
                        this.uCReporteFlujo.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Reporte - Flujo de Efectivo"; /// Titulo en el encabezado
                    break;
                case "reporteImpuestos":
                    if (uCReporteImpuestos == null)
                    {
                        this.uCReporteImpuestos = new Admeli.Reportes.UCReporteImpuestos(this.formPrincipal);
                        this.formPrincipal.panelMain.Controls.Add(uCReporteImpuestos);
                        this.uCReporteImpuestos.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.uCReporteImpuestos.Location = new System.Drawing.Point(0, 0);
                        this.uCReporteImpuestos.Name = "uCReporteImpuestos";
                        this.uCReporteImpuestos.Size = new System.Drawing.Size(250, 776);
                        this.uCReporteImpuestos.TabIndex = 0;
                    }
                    else
                    {
                        this.formPrincipal.panelMain.Controls.Add(uCReporteImpuestos);
                        this.uCReporteImpuestos.reLoad();
                    }
                    formPrincipal.lblTitlePage.Text = "Producto - Categorias"; /// Titulo en el encabezado
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
            btnExistenciaProducto.Textcolor = Color.FromArgb(139, 138, 141);
            btnImpuestos.Textcolor= Color.FromArgb(139, 138, 141);
            btnFlujoEfectivo.Textcolor= Color.FromArgb(139, 138, 141);
        }


        private void btnExistenciaProducto_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset color
            togglePanelMain("reporteProductos");
            btnExistenciaProducto.Textcolor = Color.White;/// Color active
        }

        private void btnIngresoVentas_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset color
           
        }

        private void btnImpuestos_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset color
            togglePanelMain("reporteImpuestos");
            btnImpuestos.Textcolor = Color.White;/// Color active
        }

        private void btnFlujoEfectivo_Click(object sender, EventArgs e)
        {
            btnColor(); /// Reset color
            togglePanelMain("reporteFlujo");
            btnFlujoEfectivo.Textcolor = Color.White;/// Color active
        }
    }
}
