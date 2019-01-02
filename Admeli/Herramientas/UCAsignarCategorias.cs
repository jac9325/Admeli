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
using Newtonsoft.Json;
using Entidad;

namespace Admeli.Herramientas
{
    public partial class UCAsignarCategorias : UserControl
    {
        private FormPrincipal formPrincipal;
        public bool lisenerKeyEvents { get; set; }
        private ProductoModel productoModel = new ProductoModel();
        private CategoriaModel categoriaModel = new CategoriaModel();
        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        CheckBox HeaderCheckBoxSin = null;
        CheckBox HeaderCheckBoxCon = null;
        bool IsHeaderCheckBoxClicked = false;
        public UCAsignarCategorias()
        {
            InitializeComponent();
        }

        public UCAsignarCategorias(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelContainer);
        }

        #region ========================== Root Load ==========================
        private void UCAsignarCategorias_Load(object sender, EventArgs e)
        {
            this.reLoad();


            // para el datagriwview sinCategorias
            AddHeaderCheckBox();
            dgvSinCategorias.CellPainting += new DataGridViewCellPaintingEventHandler(dgvSelectAll_CellPainting);
            HeaderCheckBoxSin.Click += new EventHandler(HeaderCheckBox_Clicked);
            dgvSinCategorias.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);
            // para el datagriwview conCategorias
            AddHeaderCheckBoxCon();
            dgvConCategoria.CellPainting += new DataGridViewCellPaintingEventHandler(dgvSelectAll_CellPaintingCon);
            HeaderCheckBoxCon.Click += new EventHandler(HeaderCheckBox_ClickedCon);
            dgvConCategoria.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClickCon);


        }

        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarCategorias();
                cargarProductos();
            }
            lisenerKeyEvents = true; // Active lisener key events
        }
        internal void refresh(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarProductos();
            }
            lisenerKeyEvents = true; // Active lisener key events
        }


        private void BindGridView()
        {           
            TotalCheckBoxes = dgvSinCategorias.RowCount;
            TotalCheckedCheckBoxes = 0;
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dgvSinCategorias.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dgvSinCategorias.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["chkBxSelect"] as DataGridViewCheckBoxCell);
                checkBox.Value = HeaderCheckBoxSin.Checked;
            }
        }
        private void HeaderCheckBox_ClickedCon(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dgvConCategoria.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dgvConCategoria.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["cbxselectConCategoria"] as DataGridViewCheckBoxCell);
                checkBox.Value = HeaderCheckBoxCon.Checked;
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in dgvSinCategorias.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["chkBxSelect"].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                HeaderCheckBoxSin.Checked = isChecked;
            }
        }

        private void DataGridView_CellClickCon(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in dgvConCategoria.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["cbxselectConCategoria"].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                HeaderCheckBoxCon.Checked = isChecked;
            }
        }
        private void dgvSelectAll_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsHeaderCheckBoxClicked)
                RowCheckBoxClick((DataGridViewCheckBoxCell)dgvSinCategorias[e.ColumnIndex, e.RowIndex]);
        }

        private void dgvSelectAll_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSinCategorias.CurrentCell is DataGridViewCheckBoxCell)
                dgvSinCategorias.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }

        private void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HeaderCheckBoxClick((CheckBox)sender);
        }

        private void dgvSelectAll_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex);
        }
        private void dgvSelectAll_CellPaintingCon(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocationCon(e.ColumnIndex, e.RowIndex);
        }
        private void AddHeaderCheckBox()
        {
            HeaderCheckBoxSin = new CheckBox();

            HeaderCheckBoxSin.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.dgvSinCategorias.Controls.Add(HeaderCheckBoxSin);
        }
        private void AddHeaderCheckBoxCon()
        {
            HeaderCheckBoxCon= new CheckBox();

            HeaderCheckBoxCon.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.dgvConCategoria.Controls.Add(HeaderCheckBoxCon);
        }

        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.dgvSinCategorias.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBoxSin.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBoxSin.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBoxSin.Location = oPoint;
        }

        private void ResetHeaderCheckBoxLocationCon(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.dgvConCategoria.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBoxCon.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBoxCon.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBoxCon.Location = oPoint;
        }



        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;

            foreach (DataGridViewRow Row in dgvSinCategorias.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells["chkBxSelect"]).Value = HCheckBox.Checked;

            dgvSinCategorias.RefreshEdit();

            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;

            IsHeaderCheckBoxClicked = false;
        }

        private void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            if (RCheckBox != null)
            {
                //Modifiy Counter;            
                if ((bool)RCheckBox.Value && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;

                //Change state of the header CheckBox.
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                    HeaderCheckBoxSin.Checked = false;
                else if (TotalCheckedCheckBoxes == TotalCheckBoxes)
                    HeaderCheckBoxSin.Checked = true;
            }
        }


        private void HeaderCheckBoxClickCon(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;

            foreach (DataGridViewRow Row in dgvSinCategorias.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells["cbxselectConCategoria"]).Value = HCheckBox.Checked;
       
            dgvSinCategorias.RefreshEdit();

            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;

            IsHeaderCheckBoxClicked = false;
        }

        private void RowCheckBoxClickCon(DataGridViewCheckBoxCell RCheckBox)
        {
            if (RCheckBox != null)
            {
                //Modifiy Counter;            
                if ((bool)RCheckBox.Value && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;

                //Change state of the header CheckBox.
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                    HeaderCheckBoxSin.Checked = false;
                else if (TotalCheckedCheckBoxes == TotalCheckBoxes)
                    HeaderCheckBoxSin.Checked = true;
            }
        }



        #endregion

        #region ===================================== Loads =====================================
        private async void cargarCategorias()
        {
            loadState(true);
            try
            {
                categoriaBindingSource.DataSource = await categoriaModel.categorias21();
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

        private async void cargarProductos()
        {
            loadState(true);
            try
            {
                productoBindingSource.DataSource = await productoModel.productosSinCategoria();
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
        #endregion

        #region ==================================== Estados ====================================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
        }



        #endregion

        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

             //label5.Text = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();

           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            //label6.Text = this.dataGridView2.CurrentRow.Cells[2].Value.ToString();
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            List<Dictionary<string,int>> list = new List<Dictionary<string, int>>();

            Dictionary<string, int> listProducto = new Dictionary<string, int>();
            Dictionary<string, int> listCategoria = new Dictionary<string, int>();
            Dictionary<string, int> listEsPrincipal = new Dictionary<string, int>();
            int numert = 0;
            foreach (DataGridViewRow row in dgvSinCategorias.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["chkBxSelect"] as DataGridViewCheckBoxCell);
                DataGridViewTextBoxCell idProducto = (row.Cells["idProducto"] as DataGridViewTextBoxCell);
                bool estaSeleccionado = Convert.ToBoolean(checkBox.EditedFormattedValue);
                if (estaSeleccionado)
                {

                    listProducto.Add("id" + numert++, Convert.ToInt32(idProducto.Value));
                }

            }
            numert = 0;
            foreach (DataGridViewRow row in dgvConCategoria.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCategoria = (row.Cells["cbxselectConCategoria"] as DataGridViewCheckBoxCell);
                DataGridViewCheckBoxCell checkBoxPrincipal = (row.Cells["cbxselecEsPrincipal"] as DataGridViewCheckBoxCell);
                DataGridViewTextBoxCell idCategoria = (row.Cells["idCategoria"] as DataGridViewTextBoxCell);
                bool estaSeleccionado = Convert.ToBoolean(checkBoxCategoria.EditedFormattedValue);
               bool esPrincipal = Convert.ToBoolean(checkBoxPrincipal.EditedFormattedValue);
                if (estaSeleccionado)
                {
                    listCategoria.Add("id" + numert, Convert.ToInt32(idCategoria.Value));
                    listEsPrincipal.Add("id" + numert, Convert.ToInt32( esPrincipal));
                    numert++;
                }

            }
            list.Add(listProducto);
            list.Add(listCategoria);            
            list.Add(listEsPrincipal);

            try
            {

            Response response=  await  categoriaModel.insertarCategoriasProductosArray(list);

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
        }

        private void dgvSinCategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // [{"id0":5,"id1":6},{"id0":1,"id1":2},{"id0":"true","id1":"true"}]

          
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //this.
        }
    }
}
