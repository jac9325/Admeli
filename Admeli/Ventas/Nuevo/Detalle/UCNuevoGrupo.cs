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
using Entidad.Location;
using Entidad;
using Admeli.Componentes;

namespace Admeli.Ventas.Nuevo.Detalle
{
    public partial class UCNuevoGrupo : UserControl
    {
        private FormClienteNuevo formClienteNuevo;

        private LocationModel locationModel = new LocationModel();
        private ProveedorModel proveedorModel = new ProveedorModel();
        private ClienteModel clienteModel = new ClienteModel();
        private GrupoClienteModel grupoClienteModel = new GrupoClienteModel(); 
        private DocumentoIdentificacionModel documentoIdentificacionModel = new DocumentoIdentificacionModel();
        private List<LabelUbicacion> labelUbicaciones { get; set; }
        private UbicacionGeografica ubicacionGeografica { get; set; }
        private SunatModel sunatModel = new SunatModel();
        Response respuesta;
        UCClienteGeneral uCClienteGeneral;
        GrupoClienteG grupoClienteG;
        public  List<GrupoCliente> grupoClientes;
        private List<DocumentoIdentificacion> documentoIdentificaciones;

        public bool lisenerKeyEvents { get; internal set; }
        private bool exiteName = false;
        public UCNuevoGrupo()
        {
            InitializeComponent();
        }

        public UCNuevoGrupo(FormClienteNuevo formClienteNuevo)
        {
            InitializeComponent();
            this.formClienteNuevo = formClienteNuevo;
        }

        private void UCProveedorGeneral_Load(object sender, EventArgs e)
        {
           // this.txtNombreGrupo.LostFocus += new EventHandler(Validate_TextBox);
        }



        #region ========================== SAVE AND UPDATE ===========================
        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!exiteName)
            {
                 grupoClienteG = new GrupoClienteG();
                 grupoClienteG.descripcion = txtDescripcion.Text;
                grupoClienteG.estado = chkEstado.Checked;
                grupoClienteG.minimoOrden = Convert.ToInt32(txtMinimoOrden.Text);
                grupoClienteG.nombreGrupo = txtNombreGrupo.Text;


                respuesta = await grupoClienteModel.guardar(grupoClienteG);
                if (respuesta.id > 0)
                {
                    txtDescripcion.Text="";
                    txtMinimoOrden.Text = "";
                    txtNombreGrupo.Text="";
                }
               grupoClientes = await clienteModel.listarGrupoClienteIdGCNombreByActivos();
               this.formClienteNuevo.togglePanelMain("general");

               this.formClienteNuevo.btnContacto.BackColor = Color.White;

            }
        }

        private async void VerificarNombreGCliente(string nombreGrupo)
        {
           respuesta = await grupoClienteModel.VerificarNombreGCliente(nombreGrupo);
        }






        private void btnClose_Click(object sender, EventArgs e)
        {
            this.formClienteNuevo.Close();
        }
        #endregion

        private void textTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {


            }
        }

        private void ll(object sender, EventArgs e)
        {

        }

        private void txtNombreGrupo_Click(object sender, EventArgs e)
        {


            
        }

        private  async void UCNuevoGrupo_Click(object sender, EventArgs e)
        {
            if (txtNombreGrupo.Text.Length > 5) { 
                respuesta = await grupoClienteModel.VerificarNombreGCliente(txtNombreGrupo.Text);
                
                lblGrupo.Text = respuesta.msj;
                if (respuesta.id == 0)
                {
                    exiteName = true;
                }
            }
        }

        private async void txtNombreGrupo_Leave(object sender, EventArgs e)
        {
            if (txtNombreGrupo.Text.Length >5) { 
                respuesta = await grupoClienteModel.VerificarNombreGCliente(txtNombreGrupo.Text);
                lblGrupo.Text = respuesta.msj;
                if (respuesta.id == 0)
                {
                    exiteName = true;
                }
            }
        }

        // TAREA hacer los cambios en todos los formularios de clientes y proveedores ver lo de paises 




    }
}
