using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidad.Configuracion;
using Modelo;
using Entidad;

namespace Admeli.CajaBox
{
    public partial class UCEstadoCajas : UserControl
    {
        private FormPrincipal formPrincipal;
        private MonedaModel monedaModel = new MonedaModel();
        private CierreCajaModel cierreCajaModel = new CierreCajaModel();
        public UCEstadoCajas()
        {
            InitializeComponent();
        }
        public UCEstadoCajas(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
            this.reLoad();

        }
        public void reLoad()
        {
            cargarSucursales();
            cargarMonedas();
            darFormatoDecimales();
        }
        private void darFormatoDecimales()
        {
            dgvEstadosCuenta.Columns["totalIngreso"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvEstadosCuenta.Columns["totalEgreso"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvEstadosCuenta.Columns["total"].DefaultCellStyle.Format = ConfigModel.configuracionGeneral.formatoDecimales;
            dgvEstadosCuenta.Columns["totalIngreso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvEstadosCuenta.Columns["totalEgreso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvEstadosCuenta.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private async void cargarMonedas()
        {
            try
            {
                List<Moneda> listMoneda = await monedaModel.monedas();
                Moneda moneda = new Moneda();
                moneda.idMoneda = 0;
                moneda.moneda = "Todos";
                listMoneda.Add(moneda);
                monedaBindingSource.DataSource = listMoneda;
                cbxMoneda.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Monedas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
}
        private void cargarSucursales()
        {
            try
            {
                List<Sucursal> listSucursales = ConfigModel.listSucursales;

                Sucursal sucursal = new Sucursal();
                sucursal.idSucursal = 0;
                sucursal.nombre = "Todos";
                listSucursales.Add(sucursal);
                sucursalBindingSource.DataSource = listSucursales;
                cbxSucursales.SelectedValue = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Sucursales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            consultar();
        }
        private async void consultar()
        {
            try
            {
                List<EstadoCuentas> listaEstadoCuentas = await cierreCajaModel.listaIntesoMesnosEgresoXSucursal(Convert.ToInt32(cbxSucursales.SelectedValue),Convert.ToInt32( cbxMoneda.SelectedValue));
                estadoCuentasBindingSource.DataSource = listaEstadoCuentas;
                dgvEstadosCuenta.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar Estado Cuentas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
