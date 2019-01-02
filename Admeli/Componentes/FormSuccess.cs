using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Componentes
{
    public partial class FormSuccess : Form
    {
        public FormSuccess()
        {
            InitializeComponent();
        }

        private void btnSuccess_Load(object sender, EventArgs e)
        {
            formFadeTransition1.ShowAsyc(this);
        }
    }
}
