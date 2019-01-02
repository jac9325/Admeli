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
using Admeli.Productos.Nuevo.PDetalle.web;
using System.IO;
using System.Net;

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class UCTiendaOnlinePD : UserControl
    {
        public bool lisenerKeyEvents { get; internal set; }
        private FormProductoNuevo formProductoNuevo;

        private ComentarioModel comentarioModel = new ComentarioModel();
        private ProductoRelacionModel productoRelacionModel = new ProductoRelacionModel();

        private List<Comentario> comentarios { get; set; }
        private List<ProductoRelacion> productoRelaciones { get; set; }

        private Comentario currentComentario { get; set; }
        private ProductoRelacion currentProductoRelacion { get; set; }

        private string direccion = "";
        private string nombreArchivo = "";
        public UCTiendaOnlinePD()
        {
            InitializeComponent();
        }

        public UCTiendaOnlinePD(FormProductoNuevo formProductoNuevo)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.bottomLine(panelHeader);
        }

        private void UCTiendaOnlinePD_Load(object sender, EventArgs e)
        {
            this.reLoad();
        }

        internal void reLoad()
        {
            cargarComentarios();
            cargarRelacionProducto();

            // Cargar Datos Web 
            cargarComponentes();
            mostrarDatosWeb();
        }

        private void loadState(bool state)
        {
            formProductoNuevo.appLoadState(state);
        }

        #region ========================== Load ==========================
        private void cargarComponentes()
        {
            // Cargando el combobox ce estados
            DataTable table = new DataTable();
            table.Columns.Add("idStockProducto", typeof(string));
            table.Columns.Add("stockProducto", typeof(string));

            table.Rows.Add("en_llegada", "Venta Con Stock");
            table.Rows.Add("sin_stock", "Venta Sin Stock");
            table.Rows.Add("lista_espera", "Lista de Espera");

            cbxVentaProducto.DataSource = table;
            cbxVentaProducto.DisplayMember = "stockProducto";
            cbxVentaProducto.ValueMember = "idStockProducto";
            cbxVentaProducto.SelectedIndex = 1;
        }

        private async void cargarComentarios()
        {
            loadState(true);
            try
            {
                List<Comentario> list = await comentarioModel.comentarios(formProductoNuevo.currentIDProducto);

                // cargando los datos
                comentarios = list;
                comentarioBindingSource.DataSource = comentarios;
                dataGridViewComentarios.Refresh();

                // formato de celadas
                // decorationDataGridView();
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

        private async void cargarRelacionProducto()
        {
            loadState(true);
            try
            {
                List<ProductoRelacion> list = await productoRelacionModel.productoRelaciones(formProductoNuevo.currentIDProducto);

                // cargando los datos
                productoRelaciones = list;
                productoRelacionBindingSource.DataSource = productoRelaciones;
                dataGridViewRelaciones.Refresh();

                // formato de celadas
                // decorationDataGridView();
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

        private void mostrarDatosWeb()
        {
            textDescripcionLarga.Text = formProductoNuevo.currentProducto.descripcionLarga;
            textKeyWords.Text = formProductoNuevo.currentProducto.keywords;
            chkEnPortada.Checked = formProductoNuevo.currentProducto.enPortada;
            chkEnCategoria.Checked = formProductoNuevo.currentProducto.enCategoriaEstrella;

            chkCantidadFraccion.Checked = formProductoNuevo.currentProducto.cantidadFraccion;
            textLimiteMinimo.Text = formProductoNuevo.currentProducto.limiteMinimo.ToString();
            textLimiteMaximo.Text = formProductoNuevo.currentProducto.limiteMaximo.ToString();
            chkCantidadFraccion.Checked = formProductoNuevo.currentProducto.cantidadFraccion;
            textURLVideo.Text = formProductoNuevo.currentProducto.urlVideo;
            chkMostrarVideo.Checked = formProductoNuevo.currentProducto.mostrarVideo;
            chkMostrarWeb.Checked = formProductoNuevo.currentProducto.mostrarWeb;
            chkMostrarPrecio.Checked = formProductoNuevo.currentProducto.mostrarPrecioWeb;
            cbxVentaProducto.SelectedValue = formProductoNuevo.currentProducto.controlSinStock;
        }

        #region ============================ CRUD RELACION ============================
        private void dataGridViewComentarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarComentario();
        }

        private void btnNuevoRelacion_Click(object sender, EventArgs e)
        {
            executeNuevoRelacion();
        }

        private void btnModificarRelacion_Click(object sender, EventArgs e)
        {
            executeModificarRelacion();
        }

        private void btnEliminarRelacion_Click(object sender, EventArgs e)
        {
            executeEliminarRelacion();
        }
        private void btnActualizarRelacion_Click(object sender, EventArgs e)
        {
            executeActualizarRelacion();
        }

        private void executeNuevoRelacion()
        {
            FormRelacionNuevo formRelacion = new FormRelacionNuevo(formProductoNuevo);
            formRelacion.ShowDialog();
            cargarRelacionProducto();
        }

        private void executeModificarRelacion()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewRelaciones.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewRelaciones.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idProductoRelacion = Convert.ToInt32(dataGridViewRelaciones.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentProductoRelacion = productoRelaciones.Find(x => x.idProductoRelacion == idProductoRelacion); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormRelacionNuevo formRelacion = new FormRelacionNuevo(formProductoNuevo, currentProductoRelacion);
            formRelacion.ShowDialog();
            cargarRelacionProducto(); // recargando loas registros en el datagridview
        }

        private async void executeEliminarRelacion()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewRelaciones.Rows.Count == 0)
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
                int index = dataGridViewRelaciones.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentProductoRelacion = new ProductoRelacion(); //creando una instancia del objeto categoria
                currentProductoRelacion.idProductoRelacion = Convert.ToInt32(dataGridViewRelaciones.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await productoRelacionModel.eliminar(currentProductoRelacion); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarRelacionProducto(); // recargando el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false); // cambiando el estado
            }
        }

        private void executeActualizarRelacion()
        {
            cargarRelacionProducto();
        }
        #endregion

        #region ========================== CRUD Comentario ==========================
        private void dataGridViewRelaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            executeModificarRelacion();
        }

        private void btnNuevoComentario_Click(object sender, EventArgs e)
        {
            executeNuevoComentario();
        }

        private void btnModificarComentario_Click(object sender, EventArgs e)
        {
            executeModificarComentario();
        }

        private void btnEliminarComentario_Click(object sender, EventArgs e)
        {
            executeEliminarComentario();
        }

        private void btnActualizarComentario_Click(object sender, EventArgs e)
        {
            executeActualizarComentario();
        }

        private void executeNuevoComentario()
        {
            FormComentarioNuevo formComentario = new FormComentarioNuevo(formProductoNuevo);
            formComentario.ShowDialog();
            cargarComentarios();
        }

        private void executeModificarComentario()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewComentarios.Rows.Count == 0)
            {
                MessageBox.Show("No hay un registro seleccionado", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int index = dataGridViewComentarios.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idComentario = Convert.ToInt32(dataGridViewComentarios.Rows[index].Cells[0].Value); // obteniedo el idRegistro del datagridview

            currentComentario = comentarios.Find(x => x.idComentario == idComentario); // Buscando la registro especifico en la lista de registros

            // Mostrando el formulario de modificacion
            FormComentarioNuevo formComentario = new FormComentarioNuevo(formProductoNuevo, currentComentario);
            formComentario.ShowDialog();
            cargarComentarios(); // recargando loas registros en el datagridview
        }

        private async void executeEliminarComentario()
        {
            // Verificando la existencia de datos en el datagridview
            if (dataGridViewComentarios.Rows.Count == 0)
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
                int index = dataGridViewComentarios.CurrentRow.Index; // Identificando la fila actual del datagridview
                currentComentario = new Comentario(); //creando una instancia del objeto categoria
                currentComentario.idComentario = Convert.ToInt32(dataGridViewComentarios.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview

                loadState(true); // cambiando el estado
                Response response = await comentarioModel.eliminar(currentComentario); // Eliminando con el webservice correspondiente
                MessageBox.Show(response.msj, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarComentarios(); // recargando el datagridview
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                loadState(false); // cambiando el estado
            }
        }

        private void executeActualizarComentario()
        {
            cargarComentarios();
        }
        #endregion

        #region ============================= Guardar =============================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            formProductoNuevo.currentProducto.descripcionLarga = textDescripcionLarga.Text;
            formProductoNuevo.currentProducto.keywords = textKeyWords.Text;
            formProductoNuevo.currentProducto.enCategoriaEstrella = chkEnCategoria.Checked;
            formProductoNuevo.currentProducto.enPortada = chkEnPortada.Checked;
            formProductoNuevo.currentProducto.limiteMaximo = textLimiteMaximo.Text;
            formProductoNuevo.currentProducto.limiteMinimo = textLimiteMinimo.Text;
            formProductoNuevo.currentProducto.cantidadFraccion = chkCantidadFraccion.Checked;
            formProductoNuevo.currentProducto.urlVideo = textURLVideo.Text;
            formProductoNuevo.currentProducto.mostrarVideo = chkMostrarVideo.Checked;
            formProductoNuevo.currentProducto.mostrarWeb = chkMostrarWeb.Checked;
            formProductoNuevo.currentProducto.mostrarPrecioWeb = chkMostrarPrecio.Checked;
            formProductoNuevo.currentProducto.controlSinStock = cbxVentaProducto.SelectedValue.ToString();

            formProductoNuevo.executeGuardar();
        }
        #endregion


        private void btnGuardarSalir_Click(object sender, EventArgs e)
        {
            formProductoNuevo.currentProducto.descripcionLarga = textDescripcionLarga.Text;
            formProductoNuevo.currentProducto.keywords = textKeyWords.Text;
            formProductoNuevo.currentProducto.enCategoriaEstrella = chkEnCategoria.Checked;
            formProductoNuevo.currentProducto.enPortada = chkEnPortada.Checked;
            formProductoNuevo.currentProducto.limiteMaximo = textLimiteMaximo.Text;
            formProductoNuevo.currentProducto.limiteMinimo = textLimiteMinimo.Text;
            formProductoNuevo.currentProducto.cantidadFraccion = chkCantidadFraccion.Checked;
            formProductoNuevo.currentProducto.urlVideo = textURLVideo.Text;
            formProductoNuevo.currentProducto.mostrarVideo = chkMostrarVideo.Checked;
            formProductoNuevo.currentProducto.mostrarWeb = chkMostrarWeb.Checked;
            formProductoNuevo.currentProducto.mostrarPrecioWeb = chkMostrarPrecio.Checked;
            formProductoNuevo.currentProducto.controlSinStock = cbxVentaProducto.SelectedValue.ToString();

            formProductoNuevo.executeGuardarSalir();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            formProductoNuevo.executeCerrar();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ofdNuevaImagen.ShowDialog() == DialogResult.OK)
                {
                    string imagen = ofdNuevaImagen.FileName;
                    ptbImagenActual.Image = Image.FromFile(imagen);
                    direccion = ofdNuevaImagen.FileName;
                    nombreArchivo = Path.GetFileName(direccion);
                    lblRutaImagen.Text = nombreArchivo;
                    btnSubirFoto.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                btnSubirFoto.Enabled = false;
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }
        }

        private void btnSubirFoto_Click(object sender, EventArgs e)
        {
            upload(direccion,nombreArchivo);
        }

        public void upload(string rutaArchivo, string nombreArchivo)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://localhost/admeli/"+nombreArchivo);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("usuario", "123456");
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;
            //Ruta donde esta ubicado el archivo
            FileStream stream = File.OpenRead(rutaArchivo);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Flush();
            reqStream.Close();

            //FileInfo fileInf = new FileInfo(rutaArchivo);
            //string ftpServerIP = "localhost";
            //string ftpUserID = "usuario";
            //string ftpPassword = "123456";
            //string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
            //FtpWebRequest reqFTP;

            //reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));
            //reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            //reqFTP.KeepAlive = false;
            //reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            //reqFTP.UseBinary = true;
            //reqFTP.ContentLength = fileInf.Length;

            //int buffLength = 4048;
            //byte[] buff = new byte[buffLength];
            //int contentLen;

            //FileStream fs = fileInf.OpenRead();

            //try
            //{
            //    Stream strm = reqFTP.GetRequestStream();
            //    contentLen = fs.Read(buff, 0, buffLength);
            //    while (contentLen != 0)
            //    {
            //        strm.Write(buff, 0, contentLen);
            //        contentLen = fs.Read(buff, 0, buffLength);
            //    }
            //    strm.Close();
            //    fs.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

    }
}
