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
using Entidad.Configuracion;
using Entidad;

namespace Admeli.Configuracion
{
    public partial class UCImpuestoDocumento : UserControl
    {
        private FormPrincipal formPrincipal;



        private SucursalModel sucursalModel = new SucursalModel();
        private TipoDocumentoModel tipoDocumentoModel = new TipoDocumentoModel();
        private ImpuestoModel impuestoModel = new ImpuestoModel();
        private List<TipoDocumento> listDocumentos { get; set; }
        private List<Sucursal> listsucursal  {get; set; }

        private  List<ImpuestosSiglas> listTodosImpuestos { get; set; }
        private List<ImpuestoDocumento> listImpuestosDocumentos { get; set; }
        private ImpuestoGeneral currentImpuesto { get; set; }
        public bool lisenerKeyEvents { get; set; }


        #region===============constructor===========

        public UCImpuestoDocumento()
        {
            InitializeComponent();

            lisenerKeyEvents = true; // Active lisener key events
        }

        public UCImpuestoDocumento(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;

            lisenerKeyEvents = true; // Active lisener key events
        }

        #endregion===============constructor===========

        #region===============root load================
        private void UCImpuestoDocumento_Load(object sender, EventArgs e)
        {
            reLoad();
        }
        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarSucursales();
                cargarDocumentos();
                TodosImpuestos(); 

            }
            lisenerKeyEvents = true; // Active lisener key eventss
        }


        #endregion===============root load===========


        #region===============load================
        private async void cargarSucursales()
        {
            loadState(true);
            try
            {
                listsucursal=await sucursalModel.listarSucursalesActivos();
                sucursalBindingSource.DataSource = listsucursal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar sucursal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);

            }

        }
        private async void cargarDocumentos()
        {
            loadState(true);
            try
            {
                listDocumentos = await tipoDocumentoModel.tipoDocumentoVentas();
                tipoDocumentoBindingSource.DataSource = listDocumentos;
                cbxDocumento.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);

            }
        }
        private async void TodosImpuestos()
        {
            loadState(true);
            try
            {

               
                listTodosImpuestos = await impuestoModel.listarImpuestoIdImpuestoNombreSiglasByActivos();
                impuestoGeneralBindingSource1.DataSource = listTodosImpuestos;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cargar documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);

            }
        }


        #endregion===============load================

        #region===============estados================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelOpciones.Enabled = !state;
            panelAcciones.Enabled = !state;
           
            dgvImpuestoDocumento.Enabled = !state;
            dgvImpuestoTodos.Enabled = !state;
        }
        #endregion===============load================
        //tipodocumentoimpuesto/sucursal/1/tipodoc/3
        //Task<List<ImpuestoDocumento >> impuestoTipoDoc(int idSucursal,int idTipoDoc)
        private async void cbxDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbxDocumento.SelectedIndex == -1)
            {

                impuestoGeneralBindingSource.DataSource = null;
                TodosImpuestos();
                return;

            }
            loadState(true);
            try
            {
                listTodosImpuestos = await impuestoModel.listarImpuestoIdImpuestoNombreSiglasByActivos();
                listImpuestosDocumentos = await impuestoModel.impuestoTipoDoc((int)cbxSucursal.SelectedValue, (int)cbxDocumento.SelectedValue);
                impuestoGeneralBindingSource.DataSource = null;
                impuestoGeneralBindingSource.DataSource = listImpuestosDocumentos;
                dgvImpuestoDocumento.Refresh();
                
                loadState(true);

                if (listImpuestosDocumentos == null)
                    listImpuestosDocumentos = new List<ImpuestoDocumento>();
                foreach (ImpuestoGeneral IG in listImpuestosDocumentos)
                {
                    if (BuscarElementos(IG.idImpuesto)!=null)
                    {

                      listTodosImpuestos.Remove(listTodosImpuestos.Find(X=>X.idImpuesto==IG.idImpuesto));
                    }
                        
                 }
                impuestoGeneralBindingSource1.DataSource = null;
                impuestoGeneralBindingSource1.DataSource = listTodosImpuestos;
                dgvImpuestoTodos.Refresh();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "seleccionar documento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false);

            }
        }


        private ImpuestoGeneral BuscarElementos(int idImpuesto)
        {
            return listTodosImpuestos.Find(X => X.idImpuesto == idImpuesto);
        }

        private void dgvImpuestoTodos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (cbxDocumento.SelectedIndex == -1) return; 
            if (dgvImpuestoTodos.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            loadState(true);
            try
            {

                int index = dgvImpuestoTodos.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idImpuesto = Convert.ToInt32(dgvImpuestoTodos.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                // primero recuperamos de la lista


                currentImpuesto = listTodosImpuestos.Find(x => x.idImpuesto == idImpuesto); // Buscando la categoria en las lista de categorias

                if (listImpuestosDocumentos == null) listImpuestosDocumentos = new List<ImpuestoDocumento>();
                ImpuestoDocumento impuestoDocumento = new ImpuestoDocumento();

                impuestoDocumento.idImpuesto = currentImpuesto.idImpuesto;
                impuestoDocumento.nombreImpuesto= currentImpuesto.nombreImpuesto;
                impuestoDocumento.siglasImpuesto = currentImpuesto.siglasImpuesto;

                listImpuestosDocumentos.Add(impuestoDocumento);
                impuestoGeneralBindingSource.DataSource = null;
                impuestoGeneralBindingSource.DataSource = listImpuestosDocumentos;
                dgvImpuestoDocumento.Refresh();
                listTodosImpuestos.Remove(currentImpuesto as ImpuestosSiglas);

                impuestoGeneralBindingSource1.DataSource = null;
                impuestoGeneralBindingSource1.DataSource = listTodosImpuestos;
                dgvImpuestoTodos.Refresh();


            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "impuesto dgvImpuestoTodos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            finally
            {
                loadState(false);

            }
            

        }

        private void cbxSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSucursal.SelectedIndex == -1  || cbxDocumento.SelectedIndex == -1) return;
            TodosImpuestos();
            loadState(true);
            try
            {
            foreach (ImpuestoGeneral IG in listImpuestosDocumentos)
            {
                if (BuscarElementos(IG.idImpuesto) != null)
                {

                    listTodosImpuestos.Remove(listTodosImpuestos.Find(X => X.idImpuesto == IG.idImpuesto));
                }

            }
            impuestoGeneralBindingSource1.DataSource = null;
            impuestoGeneralBindingSource1.DataSource = listTodosImpuestos;
            dgvImpuestoTodos.Refresh();
            }
            catch(Exception ex)
            {


                MessageBox.Show("Error: " + ex.Message, "seleccionar sucursal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



            }
            finally
            {

                loadState(false);
            }




        }

        private async  void btnGuardar_Click(object sender, EventArgs e)
        {

            loadState(true);
            try
            {
                ImpuestoComprobante impuestoComprobante = new ImpuestoComprobante();
                string impuesto = ""; 
                foreach(ImpuestoDocumento ID in listImpuestosDocumentos)
                {

                    impuesto += ID.idImpuesto+",";
              
                }
                if (impuesto.Length>0)
                    impuesto = impuesto.Substring(0, impuesto.Length - 1);
                impuestoComprobante.idSucursal =(int) cbxSucursal.SelectedValue;
                impuestoComprobante.idTipoDocumento = (int)cbxDocumento.SelectedValue;
                impuestoComprobante.impuestos = impuesto;
                Response response = await impuestoModel.guardarImpuestoDocumento(impuestoComprobante);

                MessageBox.Show("Mensaje: " +response.msj+ " correctamente", "guardar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            catch(Exception ex)
            {

                MessageBox.Show("Error: " +ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
               


            }
            finally
            {

                loadState(false);

            }
           


        }

        private void dgvImpuestoDocumento_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cbxDocumento.SelectedIndex == -1) return;
            loadState(true);
            try
            {

                if (dgvImpuestoDocumento.Rows.Count == 0)
                {
                    MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int index = dgvImpuestoDocumento.CurrentRow.Index; // Identificando la fila actual del datagridview
                int idImpuesto = Convert.ToInt32(dgvImpuestoDocumento.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                // primero recuperamos de la lista


                currentImpuesto = listImpuestosDocumentos.Find(x => x.idImpuesto == idImpuesto); // Buscando la categoria en las lista de categorias

                if (listImpuestosDocumentos == null) listImpuestosDocumentos = new List<ImpuestoDocumento>();
                ImpuestosSiglas impuestoDocumento = new ImpuestosSiglas();

                impuestoDocumento.idImpuesto = currentImpuesto.idImpuesto;
                impuestoDocumento.nombreImpuesto = currentImpuesto.nombreImpuesto;
                impuestoDocumento.siglasImpuesto = currentImpuesto.siglasImpuesto;

                listTodosImpuestos.Add(impuestoDocumento);
                impuestoGeneralBindingSource1.DataSource = null;
                impuestoGeneralBindingSource1.DataSource = listTodosImpuestos;
                dgvImpuestoTodos.Refresh();

                listImpuestosDocumentos.Remove(currentImpuesto as ImpuestoDocumento);
                impuestoGeneralBindingSource.DataSource = null;
                impuestoGeneralBindingSource.DataSource = listImpuestosDocumentos;
                dgvImpuestoDocumento.Refresh();



            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "impuesto dgvImpuestoDocumento", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              
            }
            finally
            {
                loadState(false);

            }

           


        }

    

    }
}
