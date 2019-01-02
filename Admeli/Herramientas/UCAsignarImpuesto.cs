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
using Entidad;
using Newtonsoft.Json;

namespace Admeli.Herramientas
{
    public partial class UCAsignarImpuesto : UserControl
    {
        private FormPrincipal formPrincipal;
        public bool lisenerKeyEvents { get; set; }

        private ProductoModel productoModel = new ProductoModel();
        private ImpuestoModel impuestoModel = new ImpuestoModel();

        private SucursalModel sucursalModel = new SucursalModel();
        List<ImpuestosSiglas> listImpuestos;
        List<ProductoSinImpuesto> listProductos;
        List<Sucursal> listSucursal;     
        CheckBox HeaderCheckBoxProducto = null;
        CheckBox HeaderCheckBoxImpuesto = null;
        public UCAsignarImpuesto()
        {
            InitializeComponent();
        }

        public UCAsignarImpuesto(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            lisenerKeyEvents = true; // Active lisener key events
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelContainer);
        }

        #region ============================== Root Load ==============================
        private void UCAsignarImpuesto_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarProductos(ConfigModel.sucursal.idSucursal);
                cargarImpuestos();
                cargarSucursal();

                // para el datagriwview sinCategorias
                AddHeaderCheckBoxProducto();
                dgvProducto.CellPainting += new DataGridViewCellPaintingEventHandler(dgvSelectAll_CellPaintingProducto);
                HeaderCheckBoxProducto.Click += new EventHandler(HeaderCheckBox_ClickedProducto);
                dgvProducto.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClickProducto);
                // para el datagriwview conCategorias
                AddHeaderCheckBoxImpuestos();
                dgvImpuestos.CellPainting += new DataGridViewCellPaintingEventHandler(dgvSelectAll_CellPaintingImpuesto);
                HeaderCheckBoxImpuesto.Click += new EventHandler(HeaderCheckBox_ClickedImpuesto);
                dgvImpuestos.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClickImpuesto);





            }
            lisenerKeyEvents = true; // Active lisener key events
        }
        #endregion

        #region ====================================== Loads ======================================
        private async void cargarProductos(int idSucursal)
        {
            loadState(true);
            try
            {
                /// categoriaBindingSource.DataSource = await categoriaModel.categorias21();
                /// 
                listProductos = await productoModel.listarProductoPorIdProductoCodigoNombreSinImpuesto(idSucursal);
                productoSinImpuestoBindingSource1.DataSource = listProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
        }

        private  async void cargarImpuestos()
        {
            loadState(true);
            try
            {
                listImpuestos = await impuestoModel.listarImpuestoIdImpuestoNombreSiglasByActivos();
                impuestosSiglasBindingSource.DataSource = listImpuestos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);
            }
        }

        private async void cargarSucursal()
        {
            try
            {
                listSucursal = await sucursalModel.sucursales();
                sucursalBindingSource.DataSource = listSucursal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Sucursal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        public void  AddHeaderCheckBoxProducto()
        {
            HeaderCheckBoxProducto = new CheckBox();

            HeaderCheckBoxProducto.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.dgvProducto.Controls.Add(HeaderCheckBoxProducto);


        }
        public void AddHeaderCheckBoxImpuestos() 
        {
            HeaderCheckBoxImpuesto = new CheckBox();

            HeaderCheckBoxImpuesto.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.dgvImpuestos.Controls.Add(HeaderCheckBoxImpuesto);
        }

        private void dgvSelectAll_CellPaintingProducto(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocationProducto(e.ColumnIndex, e.RowIndex);
        }
        private void dgvSelectAll_CellPaintingImpuesto(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocationImpuesto(e.ColumnIndex, e.RowIndex);
        }
        private void ResetHeaderCheckBoxLocationProducto(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.dgvProducto.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBoxProducto.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBoxProducto.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBoxProducto.Location = oPoint;
        }

        private void ResetHeaderCheckBoxLocationImpuesto(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.dgvImpuestos.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBoxImpuesto.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBoxImpuesto.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBoxImpuesto.Location = oPoint;
        }
        private void HeaderCheckBox_ClickedProducto(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dgvProducto.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dgvProducto.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells[0] as DataGridViewCheckBoxCell);
                checkBox.Value = HeaderCheckBoxProducto.Checked;
            }
        }
        private void HeaderCheckBox_ClickedImpuesto(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dgvImpuestos.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dgvImpuestos.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells[0] as DataGridViewCheckBoxCell);
                checkBox.Value = HeaderCheckBoxImpuesto.Checked;
            }
        }
        private void DataGridView_CellClickProducto(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in dgvProducto.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                HeaderCheckBoxProducto.Checked = isChecked;
            }
        }
        private void DataGridView_CellClickImpuesto(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in dgvImpuestos.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                HeaderCheckBoxImpuesto.Checked = isChecked;
            }
        }

        #endregion





        #region ==================================== Estados ====================================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            btnNuevo.Enabled = !state;
        }
        #endregion

        #region=================================
        private void cbxSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        #endregion=============================================

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            loadState(true);
            List<Dictionary<string,int>> list = new List<Dictionary<string, int>>();

            Dictionary<string, int> listProducto = new Dictionary<string, int>();
            Dictionary<string, int> listImpuesto = new Dictionary<string, int>();
            Dictionary<string, int> listSucursal = new Dictionary<string, int>();
            int numert = 0;
            foreach (DataGridViewRow row in dgvProducto.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["chbxselecProducto"] as DataGridViewCheckBoxCell);
                DataGridViewTextBoxCell idPresentacion = (row.Cells["idPresentacion"] as DataGridViewTextBoxCell);
                bool estaSeleccionado = Convert.ToBoolean(checkBox.EditedFormattedValue);
                if (estaSeleccionado)
                {

                    listProducto.Add("id" + numert++, Convert.ToInt32(idPresentacion.Value));
                }

            }
            numert = 0;
            foreach (DataGridViewRow row in dgvImpuestos.Rows)
            {
                DataGridViewCheckBoxCell checkBoxSelectImpuesto = (row.Cells["chbxselectimpuesto"] as DataGridViewCheckBoxCell);
                DataGridViewTextBoxCell idImpuesto = (row.Cells["idImpuesto"] as DataGridViewTextBoxCell);
               
                bool estaSeleccionado = Convert.ToBoolean(checkBoxSelectImpuesto.EditedFormattedValue);
             
                if (estaSeleccionado)
                {
                    listImpuesto.Add("id" + numert, Convert.ToInt32(idImpuesto.Value));
                    
                    numert++;
                }

            }
            if (cbxSucursal.SelectedValue != null)
            {
                listSucursal.Add("idSucursal" , Convert.ToInt32(cbxSucursal.SelectedValue));

            }
            else
            {

            }


            list.Add(listProducto);
            list.Add(listImpuesto);            
            list.Add(listSucursal);

            try
            {
            
              Response response=  await  impuestoModel.InsertarImpuestosProductosArray(list);

            if (response.id > 0)
              {

               MessageBox.Show(response.msj, "presentacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reLoad();  
              }

            }
            catch(Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            loadState(false);
        }

        private void cbxSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSucursal.SelectedValue != null)
                cargarProductos((int)cbxSucursal.SelectedValue);
        }

        private void tlsHeader_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panelHeaderItem1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
