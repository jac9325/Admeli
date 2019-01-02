using Admeli.Componentes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Reportes
{
    public partial class elegirCamposExportar : Form
    {
        private DataGridView dgvProductos;
        public elegirCamposExportar()
        {
            InitializeComponent();
        }
        public elegirCamposExportar(DataGridView dgv)
        {
            InitializeComponent();
            this.dgvProductos = dgv;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //crear dgv para exportar
            Bloqueo.bloquear(this, true);
            try
            {
                DataGridView dgvExportar = new DataGridView();
               
                ExternalFiles.ExportarDataGridViewExcel(dgvProductos);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Bloqueo.bloquear(this, false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
