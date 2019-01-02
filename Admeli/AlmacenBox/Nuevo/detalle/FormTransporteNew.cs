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
using Entidad.Location;
using Entidad;
using Admeli.Componentes;
using Admeli.Compras.Nuevo;
using Admeli.AlmacenBox.buscar;


namespace Admeli.AlmacenBox.Nuevo.detalle
{
    public partial class FormTransporteNew : Form
    {

        private EmpresaTransporte empresaTransporte { get; set; }

        private LocationModel locationModel = new LocationModel();
        private ProveedorModel proveedorModel = new ProveedorModel();
        private GuiaRemisionModel guiaRemisionModel = new GuiaRemisionModel();
        private List<LabelUbicacion> labelUbicaciones { get; set; }
        public  UbicacionGeografica CurrentUbicacionGeografica { get; set; }

        private SunatModel sunatModel=new SunatModel();
        private bool bandera;
       
        public int idUbicacionGeografia { get; set; }
        
        public string cadena = "";
        public FormTransporteNew()
        {
            InitializeComponent();

            
        }

        public FormTransporteNew(FormProveedorNuevo formProveedorNuevo)
        {
            InitializeComponent();
            
        }

        private void FormTransporteNew_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscarUbicacion_Click(object sender, EventArgs e)
        {
            formGeografia formGeografia = new formGeografia();
            formGeografia.ShowDialog();
            CurrentUbicacionGeografica = formGeografia.ubicacionGeografica;
            txtUbicacion.Text = formGeografia.cadena;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            empresaTransporte = new EmpresaTransporte();

            empresaTransporte.direccion = txtDireccion.Text;
            empresaTransporte.estado = chkActivo.Checked ? 1 : 0;
            empresaTransporte.idUbicacionGeografica = CurrentUbicacionGeografica.idUbicacionGeografica;
            empresaTransporte.razonSocial = txtNombreEmpresa.Text;
            empresaTransporte.ruc = txtNroDocumento.Text;
            empresaTransporte.telefono = txtTelefono.Text;
            bloquear(true);
            try
            {

                Response response = await guiaRemisionModel.guardarETransporte(empresaTransporte);
                if (response.id > 0)
                {
                    MessageBox.Show(response.msj, "guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bloquear(false);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response.msj, "guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "btnGuardar_Click", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bloquear(false);


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
    }
}
