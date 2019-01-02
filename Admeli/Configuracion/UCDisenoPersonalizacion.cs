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
using Admeli.Componentes;
using Admeli.Configuracion.Modificar;
namespace Admeli.Configuracion
{
    public partial class UCDisenoPersonalizacion : UserControl
    {
        private FormPrincipal formPrincipal;
        private FormDiseñoComprobantes comprobates;
        
        public bool lisenerKeyEvents { get; set; }
        TipoDocumentoModel tipoDocumentoModel = new TipoDocumentoModel();
        List<DiseñoDocumento> listData;
        public UCDisenoPersonalizacion()
        {
            InitializeComponent();
            
            
        }

        public UCDisenoPersonalizacion(FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.formPrincipal = formPrincipal;
        }

        #region ============================ Root Load ============================
        private void UCDisenoPersonalizacion_Load(object sender, EventArgs e)
        {
            this.reLoad();

            // Preparando para los eventos de teclado
            this.ParentChanged += ParentChange; // Evetno que se dispara cuando el padre cambia // Este eveto se usa para desactivar lisener key events de este modulo
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }
        }

        internal void reLoad(bool refreshData = true)
        {
            if (refreshData)
            {
                cargarRegistros();
               
            }
            lisenerKeyEvents = true; // Active lisener key events
        }
        #endregion

        #region ======================== KEYBOARD ========================
        // Evento que se dispara cuando el padre cambia
        private void ParentChange(object sender, EventArgs e)
        {
            // cambiar la propiedad de lisenerKeyEvents de este modulo
            if (lisenerKeyEvents) lisenerKeyEvents = false;
        }

        // Escuchando los Eventos de teclado
        private void TopLevelControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!lisenerKeyEvents) return;
            switch (e.KeyCode)
            {
                case Keys.F4:
                   // executeModificar();
                    break;
                case Keys.F5:
                    cargarRegistros();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region =========================== Paint and Decoration ===========================
        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelContainer);
        }

        private void decorationDataGridView()
        {
            /*
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                var estado = dataGridView.Rows[i].Cells.get.Value.ToString();
                dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.DeepPink;
            }*/
        }
        #endregion

        #region ======================= Loads =======================
        private async void cargarRegistros()
        {
            loadState(true);
            try
            {
                listData = await tipoDocumentoModel.tipodoc(1);
                if (listData == null) return; /// Verificar si hay datos

                // Ingresando
                tipoDocumentoBindingSource.DataSource = listData;
                dataGridView.Refresh();
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

        #region =========================== Estados ===========================
        private void loadState(bool state)
        {
            formPrincipal.appLoadState(state);
            panelCrud.Enabled = !state;
            dataGridView.Enabled = !state;
        }
        #endregion


        #region ==================== CRUD ====================

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarRegistros();
        }
        #endregion

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DiseñoDocumento diseño=  (DiseñoDocumento)tipoDocumentoBindingSource.List[e.RowIndex];
            comprobates = new FormDiseñoComprobantes(diseño, listData);
            comprobates.ShowDialog();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            cargarRegistros();
        }
    }
}
