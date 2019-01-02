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
    public partial class FormMotivoNew : Form
    {

        private GuiaRemisionModel guiaRemisionModel = new GuiaRemisionModel();
        private MotivoTraslado motivoTraslado { get; set; }

        public int idUbicacionGeografia { get; set; }
        
        public string cadena = "";
        public FormMotivoNew()
        {
            InitializeComponent();

            
        }

       

        private void FormTransporteNew_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscarUbicacion_Click(object sender, EventArgs e)
        {
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            bloquear(true);
            try
            {
                motivoTraslado = new MotivoTraslado();

                motivoTraslado.nombre = txtMotivo.Text;
                motivoTraslado.estado = chkActivo.Checked ? 1 : 0;
                Response response = await guiaRemisionModel.guardarMTraslado(motivoTraslado);
                if (response.id > 0)
                {
                    MessageBox.Show(response.msj, "guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
