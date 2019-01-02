using Entidad;
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

namespace Admeli.AlmacenBox.Nuevo.detalle
{

    public partial class FormMotivo : Form
    {



        private GuiaRemisionModel guiaRemisionModel = new GuiaRemisionModel();
        private MotivoTraslado motivoTraslado { get; set; }
        public FormMotivo()
        {
            InitializeComponent();
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
                }
                else
                {
                    MessageBox.Show(response.msj, "guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
