using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.Componentes;
using Modelo;
using Admeli.Configuracion.Nuevo;
using Newtonsoft.Json;
using Entidad;
using System.IO;

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class UCImpuestoPD : UserControl
    {
        public bool lisenerKeyEvents { get; internal set; }
        private FormProductoNuevo formProductoNuevo;
        public SucursalModel sucursalModel = new SucursalModel();
        public ImpuestoModel impuestoModel = new ImpuestoModel();
        public ImpuestoProductoTodo impuestoProductoTodo = new ImpuestoProductoTodo();
        public Response response = new Response();

        public List<Impuesto> listaImpuesto = new List<Impuesto>();
        public UCImpuestoPD()
        {
            InitializeComponent();
        }

        public UCImpuestoPD(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
        }


        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panelHeader);
        }

        private void UCImpuestoPD_Paint(object sender, PaintEventArgs e)
        {
            int containerWidth = this.Size.Width;
            int itemWidth = containerWidth / 2;
            panelItem1.Size = new Size(itemWidth, 100);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeGuardar();
        }

        private void btnGuardarSalir_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeGuardarSalir();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeCerrar();
        }

        #region ================================ Root Load ================================
        private void UCImpuestoPD_Load(object sender, EventArgs e)
        {
            reLoad();
        }

        internal async void reLoad()
        {
            
            formProductoNuevo.appLoadState(true);
            //Artificio para esperar el idSucursal
            int artificio = await cargarSucursal();
            cargarImpuestoProducto();
            formProductoNuevo.appLoadState(false);
        }
        internal async Task<int> cargarSucursal()
        {
            sucursalBindingSource.DataSource = await sucursalModel.sucursales();
            return 0;
        }
        internal async void cargarImpuestoProducto()
        {
            //Cargar los impuesto del producto y los productos existentes
            impuestoProductoTodo = await impuestoModel.impuestoProductoTodo(formProductoNuevo.currentIDProducto,Convert.ToInt32(cbxSucursal.SelectedValue.ToString()));
            impuestoBindingSourceT.DataSource =impuestoProductoTodo.producto;
            impuestoBindingSourceP.DataSource = impuestoProductoTodo.todo;
        }
        #endregion

        private void btnAddSucursal_Click(object sender, EventArgs e)
        {
            FormSucursalNuevo sucursalNuevo = new FormSucursalNuevo();
            sucursalNuevo.ShowDialog();
            this.reLoad();
        }

        private void btnNuevoImpuesto_Click(object sender, EventArgs e)
        {
            //Abir la ventana de nuevo impuesto

           FormNuevoImpuesto nuevoImpuesto = new FormNuevoImpuesto();
            nuevoImpuesto.ShowDialog();
            this.reLoad();
        }

        private void btnActualizarImpuesto_Click(object sender, EventArgs e)
        {
            //Actualizar ventana de impuestos
            reLoad();
        }

        private void btnTodoAProducto_Click(object sender, EventArgs e)
        {
            // Verificando la existencia de datos en el dgvImpuestoTodo
            if (dgvImpuestoTodo.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Pasar el elemento seleccionado de dgvImpuestoTodo a dgvImpuestoProducto
            //Recuperar los elementos de BindingSource
            List<Impuesto> impuestoTodos = (List<Impuesto>) impuestoBindingSourceT.DataSource;
            List<Impuesto> impuestoProducto = (List<Impuesto>)impuestoBindingSourceP.DataSource;
            
            //Recuperar el elemento seleccionado 
            int index = dgvImpuestoTodo.CurrentRow.Index; // Identificando la fila actual del datagridview
            Impuesto filaSeleccionada = new Impuesto();
            filaSeleccionada.idImpuesto = impuestoTodos[index].idImpuesto;
            filaSeleccionada.nombreImpuesto = impuestoTodos[index].nombreImpuesto;
            filaSeleccionada.siglasImpuesto = impuestoTodos[index].siglasImpuesto;
            filaSeleccionada.valorImpuesto = impuestoTodos[index].valorImpuesto;
            filaSeleccionada.porcentual = impuestoTodos[index].porcentual;
            filaSeleccionada.porDefecto = impuestoTodos[index].porDefecto;
            filaSeleccionada.estado = impuestoTodos[index].estado;
            filaSeleccionada.enUso = impuestoTodos[index].enUso;
            //Eliminar elemento seleccionado de impuestoTodo
            impuestoTodos.RemoveAt(index);
            impuestoBindingSourceT.DataSource = null;
            impuestoBindingSourceT.DataSource = impuestoTodos;
            //Agregar elemento seleccionado a impuestoProducto
            impuestoProducto.Add(filaSeleccionada);
            impuestoBindingSourceP.DataSource = null;
            impuestoBindingSourceP.DataSource = impuestoProducto;
            
        }

        private void btnProductoATodo_Click(object sender, EventArgs e)
        {
            // Verificando la existencia de datos en el dgvImpuestoTodo
            if (dgvImpuestoProducto.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Pasar el elemento seleccionado de dgvImpuestoProducto a dgvImpuestoTodo
            //Recuperar los elementos de BindingSource
            List<Impuesto> impuestoTodos = (List<Impuesto>)impuestoBindingSourceT.DataSource;
            List<Impuesto> impuestoProducto = (List<Impuesto>)impuestoBindingSourceP.DataSource;

            //Recuperar el elemento seleccionado 
            int index = dgvImpuestoProducto.CurrentRow.Index; // Identificando la fila actual del datagridview
            Impuesto filaSeleccionada = new Impuesto();
            filaSeleccionada.idImpuesto = impuestoProducto[index].idImpuesto;
            filaSeleccionada.nombreImpuesto = impuestoProducto[index].nombreImpuesto;
            filaSeleccionada.siglasImpuesto = impuestoProducto[index].siglasImpuesto;
            filaSeleccionada.valorImpuesto = impuestoProducto[index].valorImpuesto;
            filaSeleccionada.porcentual = impuestoProducto[index].porcentual;
            filaSeleccionada.porDefecto = impuestoProducto[index].porDefecto;
            filaSeleccionada.estado = impuestoProducto[index].estado;
            filaSeleccionada.enUso = impuestoProducto[index].enUso;
            //Eliminar elemento seleccionado de impuestoProducto
            impuestoProducto.RemoveAt(index);
            impuestoBindingSourceP.DataSource = null;
            impuestoBindingSourceP.DataSource = impuestoProducto;
            //Agregar elemento seleccionado a impuestoProducto
            impuestoTodos.Add(filaSeleccionada);
            impuestoBindingSourceT.DataSource = null;
            impuestoBindingSourceT.DataSource = impuestoTodos;
        }

        private void btnActualizarImpuesto_Click_1(object sender, EventArgs e)
        {
            reLoad();
        }

        private void cbxSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            reLoad();
        }

        private void btnGuardarImpustos_Click(object sender, EventArgs e)
        {
            try
            {
                extraerImpuestosProducto();
                guardarImpuestosProducto();
                MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }  

        private async void guardarImpuestosProducto()
        {
            response = await impuestoModel.actualizarImpuestoProducto(listaImpuesto,formProductoNuevo.currentIDProducto,int.Parse(cbxSucursal.SelectedValue.ToString()));
        }

        private void extraerImpuestosProducto()
        {
            try
            {
                foreach (DataGridViewRow row in dgvImpuestoProducto.Rows)
                {
                    Impuesto impuestoFila = new Impuesto();
                    impuestoFila.idImpuesto = int.Parse(row.Cells["idImpuesto"].Value.ToString());
                    listaImpuesto.Add(impuestoFila);
                    //MessageBox.Show(row.Cells[0].Value.ToString() + " " + row.Cells[1].Value.ToString() + " " +
                    //    row.Cells[2].Value.ToString() + " " + row.Cells[3].Value.ToString()+" "+ row.Cells[5].Value.ToString()+ " "+row.Cells[6].Value.ToString());
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
