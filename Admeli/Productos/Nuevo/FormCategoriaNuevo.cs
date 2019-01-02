using Admeli.Componentes;
using Entidad;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Productos.Nuevo
{
    public partial class FormCategoriaNuevo : Form
    {
        private CategoriaModel categoriaModel = new CategoriaModel();
        private Categoria categoria { get; set; }
        private bool nuevo { get; set; }

        #region ========================== Constructor ==========================
        public FormCategoriaNuevo()
        {
            InitializeComponent();
            nuevo = true;
            chkActivoCat.Checked = true;
        }

        public FormCategoriaNuevo(Categoria categoria)
        {
            InitializeComponent();
            this.categoria = categoria;
            this.nuevo = false;
        }
        #endregion

        #region =============================== ROOT LOAD ===============================
        private void FormCategoriaNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarCategoriaPadre();
            cargarOrdenVisualizacion();
            cargarMostrarProductoEn();
        }
        #endregion

        #region =============================== PAINT ===============================
        private void panelCateGoriaPagre_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelCateGoriaPagre, 157, 157, 157);
        }

        private void panelOrdenVisualizacion_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelOrdenVisualizacion, 157, 157, 157);
        }

        private void panelMostrarEn_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panelMostrarEn, 157, 157, 157);
        }
        #endregion



        #region ========================================== LOAD ==========================================
        private void cargarRegistrosModificar()
        {
            if (nuevo) return;
            textNombreCat.Text = categoria.nombreCategoria;
            cbxCatPadre.SelectedValue = categoria.idPadreCategoria;
            cbxOrdenVisual.SelectedValue = categoria.ordenVisualizacionProductos;
            cbxMostrarEn.SelectedValue = categoria.mostrarProductosEn;
            textNumeroColumna.Text = categoria.numeroColumnas.ToString();
            textTituloCat.Text = categoria.tituloCategoriaSeo;
            textUrlCat.Text = categoria.urlCategoriaSeo;
            textTagCat.Text = categoria.metaTagsSeo;
            textCabeceraTag.Text = categoria.cabeceraPagina;
            textPieCat.Text = categoria.piePagina;
            textOrden.Text = categoria.orden.ToString();
            chkActivoCat.Checked = Convert.ToBoolean(categoria.estado);
            chkMostrarWeb.Checked = Convert.ToBoolean(categoria.mostrarWeb);
        }

        private async void cargarCategoriaPadre()
        {
            cbxCatPadre.DataSource = await categoriaModel.categorias21();
            cbxCatPadre.DisplayMember = "nombreCategoria";
            cbxCatPadre.ValueMember = "idCategoria";
            cbxCatPadre.SelectedValue = 0;
            cargarRegistrosModificar();
        } 

        private void cargarOrdenVisualizacion()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idOrdenVisualizacion", typeof(string));
            table.Columns.Add("ordenVisualizacion", typeof(string));

            table.Rows.Add("1", "Precio: Menos a Mas");
            table.Rows.Add("2", "Precio: Mas a Menos");
            table.Rows.Add("3", "Segun Nombre");
            table.Rows.Add("4", "Fecha: Modificacion");
            table.Rows.Add("5", "Promedio de Puntuacion");
            table.Rows.Add("6", "Numero de Comentarios");

            cbxOrdenVisual.DataSource = table;
            cbxOrdenVisual.DisplayMember = "ordenVisualizacion";
            cbxOrdenVisual.ValueMember = "idOrdenVisualizacion";
        }

        private void cargarMostrarProductoEn()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idMostrarEn", typeof(string));
            table.Columns.Add("mostrarEN", typeof(string));

            table.Rows.Add("1", "En Cuadriculas");
            table.Rows.Add("2", "En Listas");

            cbxMostrarEn.DataSource = table;
            cbxMostrarEn.DisplayMember = "mostrarEN";
            cbxMostrarEn.ValueMember = "idMostrarEn";
        }
        #endregion

        #region ============================= SAVE AND UPDATE =======================
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private bool validarCampos()
        {
            bool isValid = true;        // IS Valid ============ TRUE

            if (textNombreCat.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreCat, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreCat, 0);
                isValid = false;
            }
            if (cbxCatPadre.SelectedIndex == -1)
            {
                errorProvider1.SetError(cbxCatPadre, "Campo obligatorio");
                isValid = false;
            }

            return (!isValid) ? false : true;
        }

        private async void guardar()
        {
            if (!validarCampos()) return; // Validacion del los campos

            try
            {
                cargarObjeto();
                appLoadding(true, 25);
                if (nuevo)
                {
                    Response response = await categoriaModel.guardar(categoria);
                    appLoadding(false, 100);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await categoriaModel.modificar(categoria);
                    appLoadding(false, 100);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                appLoadding(false, 0);
            }
        }

        private void cargarObjeto()
        {
            if (nuevo)
            {
                categoria = new Categoria(); // crea una nueva instancia de la categoria
                categoria.afecta = true;
            }

            categoria.cabeceraPagina = textCabeceraTag.Text.Trim();
            categoria.estado = Convert.ToInt32(chkActivoCat.Checked);
            categoria.idPadreCategoria = Convert.ToInt32(cbxCatPadre.SelectedValue);
            categoria.metaTagsSeo = textTagCat.Text.Trim();
            categoria.mostrarProductosEn = Convert.ToInt32(cbxMostrarEn.SelectedValue);
            categoria.mostrarWeb = Convert.ToInt32(chkMostrarWeb.Checked);
            categoria.nombreCategoria = textNombreCat.Text.Trim();
            categoria.numeroColumnas = (textNumeroColumna.Text.Trim() != "") ? Convert.ToInt32(textNumeroColumna.Text.Trim()) : 1;
            categoria.orden = (textOrden.Text.Trim() != "") ? Convert.ToInt32(textOrden.Text.Trim()) : 0;
            categoria.ordenVisualizacionProductos = Convert.ToInt32(cbxOrdenVisual.SelectedValue);
            categoria.piePagina = textPieCat.Text.Trim();
            categoria.tituloCategoriaSeo = textTituloCat.Text.Trim();
            categoria.urlCategoriaSeo = textUrlCat.Text.Trim();
            categoria.padre = cbxCatPadre.SelectedText.Trim();
        }
        #endregion

        #region ==================================== LOADING ====================================
        public void appLoadding(bool state, int progress = 100, bool increment = false)
        {
            btnAceptar.Enabled = !state;
            progressBar.Value = (increment) ? progressBar.Value + progress : progress;
        }
        #endregion

        #region ======================== VALIDATE REAL TIME ========================
        private void textNumeroColumna_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private void textOrden_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }
        private void textNombreCat_Validated(object sender, EventArgs e)
        {
            if (textNombreCat.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreCat, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreCat, 0);
                return;
            }
            Validator.textboxValidateColor(textNombreCat, 1);
            errorProvider1.Clear();
        }
        #endregion
    }
}
