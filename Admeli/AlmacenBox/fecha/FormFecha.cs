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
namespace Admeli.AlmacenBox.fecha
{
    public partial class FormFecha : Form
    {
        public  DateTime desde;
        public  DateTime hasta; 
               
        public FormFecha()
        {
            InitializeComponent();        
            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(dtpDesde.Value > dtpHasta.Value)
            {
                return; 
            }
            desde = dtpDesde.Value;
            hasta = dtpHasta.Value;
            this.Close();
        }

    }
}
