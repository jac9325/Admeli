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

namespace Admeli.Configuracion.Nuevo
{
    public partial class FormDocumentoIdetificaionNuevo : Form
    {
        private DocumentoIdentificacionModel documentoModel = new DocumentoIdentificacionModel();

        private DocumentoIdentificacion currentDocument { get; set; }
        private bool nuevo { get; set; }
        private int currentIDDocI { get; set; }

        public FormDocumentoIdetificaionNuevo()
        {
            InitializeComponent();
            this.nuevo = true;
        }

        public FormDocumentoIdetificaionNuevo(DocumentoIdentificacion currentDocument)
        {
            InitializeComponent();
            this.currentDocument = currentDocument;
            this.currentIDDocI = currentDocument.idDocumento;
            this.nuevo = false;
        }

        #region =================== Paint ===================
        private void panelFooter_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.topLine(panelFooter);
        }
        private void FormDocumentoIdetificaionNuevo_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel12, 157, 157, 157);
        }
        #endregion

        #region ========================== Root Load ==========================
        private void FormDocumentoIdetificaionNuevo_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        private void reLoad()
        {
            cargarTipoDocumento();
            cargarDatosModificar();
        }
        #endregion

        #region =============================== Load ===============================
        private void cargarTipoDocumento()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idTipoDocumento", typeof(string));
            table.Columns.Add("tipoDocumento", typeof(string));

            table.Rows.Add("1", "Natural");
            table.Rows.Add("2", "Jurídico");

            cbxTipoDocumento.DataSource = table;
            cbxTipoDocumento.DisplayMember = "tipoDocumento";
            cbxTipoDocumento.ValueMember = "idTipoDocumento";
            cbxTipoDocumento.SelectedIndex = 0;
        }

        private void cargarDatosModificar()
        {
            if (nuevo) return;
            currentIDDocI = currentDocument.idDocumento;
            textNombreDocumentoIdenti.Text = currentDocument.nombre;
            textDigitosDocumentoIdenti.Text = currentDocument.numeroDigitos.ToString();
            cbxTipoDocumento.Text = currentDocument.tipoDocumento;
            chkActivoDI.Checked = Convert.ToBoolean(currentDocument.estado);
        } 
        #endregion

        #region ==================================== SAVE AND UPDATE ====================================
        private bool validarCampos()
        {
            if (textNombreDocumentoIdenti.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreDocumentoIdenti, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreDocumentoIdenti, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombreDocumentoIdenti, 1);

            if (textDigitosDocumentoIdenti.Text.Trim() == "")
            {
                errorProvider1.SetError(textDigitosDocumentoIdenti, "Campo obligatorio");
                Validator.textboxValidateColor(textDigitosDocumentoIdenti, 0);
                return false;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textDigitosDocumentoIdenti, 1);

            if (cbxTipoDocumento.Text == "")
            {
                errorProvider1.SetError(cbxTipoDocumento, "Alija almenos una");
                cbxTipoDocumento.Focus();
                return false;
            }
            errorProvider1.Clear();
            return true;
        }

        private async void guardar()
        {
            /////// 

            Bloqueo.bloquear(this, true);
            try
            {
                // Verificando el documento
                int documentID = (nuevo) ? 0 : currentIDDocI;
                List<DocumentoIdentificacion> listAlmacenes = await documentoModel.verificar(textNombreDocumentoIdenti.Text, documentID);
                if (listAlmacenes.Count > 0)
                {
                    errorProvider1.SetError(textNombreDocumentoIdenti, "Ya existe");
                    Validator.textboxValidateColor(textNombreDocumentoIdenti, 0);
                    return;
                }
                errorProvider1.Clear();
                Validator.textboxValidateColor(textNombreDocumentoIdenti, 1);

                // Procediendo con el guardado
                if (nuevo)
                {
                    Response response = await documentoModel.guardar(currentDocument);
                    MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Response response = await documentoModel.modificar(currentDocument);
                    MessageBox.Show(response.msj, "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Bloqueo.bloquear(this, false);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Bloqueo.bloquear(this, false);
            }
        }

        private void crearObjeto()
        {
            currentDocument = new DocumentoIdentificacion();
            if (!nuevo) currentDocument.idDocumento = currentIDDocI; // Llenar el id categoria cuando este en esdo modificar

            currentDocument.nombre = textNombreDocumentoIdenti.Text;
            currentDocument.numeroDigitos = Convert.ToInt32(textDigitosDocumentoIdenti.Text);
            currentDocument.tipoDocumento = cbxTipoDocumento.Text;
            currentDocument.estado = Convert.ToInt32(chkActivoDI.Checked);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                crearObjeto();
                guardar();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region ============================= Validacion TiempoReal =============================
        private void textDigitosDocumentoIdenti_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isNumber(e);
        }

        private async void textNombreDocumentoIdenti_Validating(object sender, CancelEventArgs e)
        {
            if (textNombreDocumentoIdenti.Text.Trim() == "")
            {
                errorProvider1.SetError(textNombreDocumentoIdenti, "Campo obligatorio");
                Validator.textboxValidateColor(textNombreDocumentoIdenti, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textNombreDocumentoIdenti, 1);

            /////// 
            try
            {
                int documentID = (nuevo) ? 0 : currentIDDocI;
                List<DocumentoIdentificacion> listAlmacenes = await documentoModel.verificar(textNombreDocumentoIdenti.Text, documentID);
                if (listAlmacenes.Count > 0)
                {
                    errorProvider1.SetError(textNombreDocumentoIdenti, "Ya existe");
                    Validator.textboxValidateColor(textNombreDocumentoIdenti, 0);
                    return;
                }
                errorProvider1.Clear();
                Validator.textboxValidateColor(textNombreDocumentoIdenti, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textDigitosDocumentoIdenti_Validated(object sender, EventArgs e)
        {
            if (textDigitosDocumentoIdenti.Text.Trim() == "")
            {
                errorProvider1.SetError(textDigitosDocumentoIdenti, "Campo obligatorio");
                Validator.textboxValidateColor(textDigitosDocumentoIdenti, 0);
                return;
            }
            errorProvider1.Clear();
            Validator.textboxValidateColor(textDigitosDocumentoIdenti, 1);
        }

        #endregion

        private void FormDocumentoIdetificaionNuevo_Shown(object sender, EventArgs e)
        {
            textNombreDocumentoIdenti.Focus();
        }
    }
}
