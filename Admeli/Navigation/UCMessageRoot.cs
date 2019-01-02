using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Navigation
{
    public partial class UCMessageRoot : UserControl
    {
        private FormPrincipal formPrincipal;

        public UCMessageRoot()
        {
            InitializeComponent();
        }

        public UCMessageRoot(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
        }
    }
}
