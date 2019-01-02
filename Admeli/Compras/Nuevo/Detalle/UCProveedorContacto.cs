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
using Entidad;

namespace Admeli.Compras.Nuevo.Detalle
{
    public partial class UCProveedorContacto : UserControl
    {
        private FormProveedorNuevo formProveedorNuevo;
        private ContactoModel contactoModel = new ContactoModel();
        private string nroDocumento { get; set; }
        private List<Contacto> contactos { get; set; }
        private Contacto currentContacto { get; set; }

        public UCProveedorContacto()
        {
            InitializeComponent();
        }

        public UCProveedorContacto(FormProveedorNuevo formProveedorNuevo)
        {
            InitializeComponent();
            this.formProveedorNuevo = formProveedorNuevo;
        }
        public UCProveedorContacto(FormProveedorNuevo formProveedorNuevo, string nroDocumento)
        {
            InitializeComponent();
            this.formProveedorNuevo = formProveedorNuevo;
            this.nroDocumento = nroDocumento;
        }

        private void UCProveedorContacto_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        internal void reLoad()
        {
            cargarRegistros();
        }
        private async void cargarRegistros()
        {
            formProveedorNuevo.loadStateApp(true);
            try
            {
                contactos = await contactoModel.contactos(formProveedorNuevo.currentIDProveedor);
                contactoBindingSource.DataSource = contactos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                formProveedorNuevo.loadStateApp(false);
            }
        }

        #region ==================== CRUD ====================
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            executeNuevo();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            executeModificar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            executeEliminar();
        }

        private void executeNuevo()
        {
            FormContactoNuevo formContacto = new FormContactoNuevo(this.formProveedorNuevo);
            formContacto.ShowDialog();
            cargarRegistros();
        }

        private async void executeEliminar()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Pregunta de seguridad de eliminacion
            DialogResult dialog = MessageBox.Show("¿Está seguro de eliminar este registro?", "Eliminar",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialog == DialogResult.No) return;


            try
            {
                int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentContacto = new Contacto(); //creando una instancia del objeto categoria
                currentContacto.idContacto = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                formProveedorNuevo.loadStateApp(true); // cambiando el estado 
                Response response = await contactoModel.eliminar(currentContacto); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarRegistros(); // recargando el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                formProveedorNuevo.loadStateApp(false); // cambiando el estado
            }
        }

        private void executeModificar()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridView.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idContacto = Convert.ToInt32(dataGridView.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentContacto = contactos.Find(x => x.idContacto == idContacto); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormContactoNuevo formContacto = new FormContactoNuevo(this.formProveedorNuevo, currentContacto);
            formContacto.ShowDialog();
            cargarRegistros(); // recargando loas registros en el datagridview
        }
        #endregion
    }
}
