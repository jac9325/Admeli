using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Admeli.AlmacenBox.fecha;
using Admeli.AlmacenBox.Nuevo;
using Admeli.Componentes;
using Entidad;
using Entidad.Configuracion;
using Modelo;
using Newtonsoft.Json;

namespace Admeli.Ventas.buscar
{
    public partial class FormSeleccion : Form
    {


        // servicios necesarios

        ProveedorModel proveedorModel = new ProveedorModel();
        
        // objetos que cargan a un inicio

        private List<Proveedor> listProveedores { get; set; }
        public  List<Proveedor> listProveedoresEscogidos { get; set; }

        public Proveedor currentProveedor { get; set; }

        public bool guardar = false;
        public  bool salir  = false;
        private bool lisenerKeyEvents = true;
        private int contadorProvedores = 0;
        private List<int> idProveedores { get; set; }

        private List<DetalleV> detallePedidoProveedor { get; set; }
        public FormSeleccion()
        {
            InitializeComponent();
            listProveedoresEscogidos = new List<Proveedor>();
            txtRazonSocial.Focus();

            btnAceptar.Visible = false;
            cargarGenerarCodigo();
            lbMoneda.Visible = false;
        }
        public FormSeleccion(List<DetalleV> detallePedidoProveedor)
        {
            InitializeComponent();
            listProveedoresEscogidos = new List<Proveedor>();
            this.detallePedidoProveedor = detallePedidoProveedor;
            txtRazonSocial.Focus();
        }
        public FormSeleccion(List<DetalleV> detallePedidoProveedor , List<int> idProveedores)
        {
            InitializeComponent();
            listProveedoresEscogidos = new List<Proveedor>();
            this.detallePedidoProveedor = detallePedidoProveedor;
            this.idProveedores = idProveedores;
            txtRazonSocial.Focus();
            lbMoneda.Text += detallePedidoProveedor[0].moneda;
        }



        #region ================================ Root Load ================================

        private void FormNotaSalidaNew_Load(object sender, EventArgs e)
        {
            reLoad();
             this.ParentChanged += ParentChange; // Evetno que se dispara cuando el padre cambia // Este eveto se usa para desactivar lisener key events de este modulo
            if (TopLevelControl is Form) // Escuchando los eventos del formulario padre
            {
                (TopLevelControl as Form).KeyPreview = true;
                TopLevelControl.KeyUp += TopLevelControl_KeyUp;
            }


        }
        private void reLoad()
        {
            cargarRegistro();
        }
        #endregion

        #region ============================== Load ==============================
        private async void cargarRegistro()
        {
           
            try
            {
                listProveedores = await proveedorModel.listaProveedoresSeleccion();
                listProveedoresEscogidos.Clear();

                foreach (Proveedor p in listProveedores)
                {
                    if (idProveedores != null)
                    {
                        int i = idProveedores.Find(X=>X==p.idProveedor);
                        if(i==null || i<=0)
                            p.escogido = 0;
                        else
                        {
                            p.escogido = ++contadorProvedores;       
                        }
                    }
                    else
                    {
                        p.escogido = 0;
                    }
                }
                proveedorBindingSource.DataSource = listProveedores;
                lbnroProveedores.Text = contadorProvedores.ToString();
                if (guardar)
                {
                    
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Cargar Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            finally
            {
                loadState(false);
            }
          
        }

        private string generarCodigo()
        {

            Random obj = new Random();

            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            int longitud = posibles.Length;

            char letra;
            int longitudnuevacadena = 5;

            string nuevacadena = "";

            for (int i = 0; i < longitudnuevacadena; i++)
            {
                letra = posibles[obj.Next(longitud)];
                nuevacadena += letra.ToString();
            }
            return nuevacadena;
        }

        private void cargarGenerarCodigo() {
            plinfo.Visible = false;
            label3.Visible = false;
            label2.Visible = false;
            lbnroProveedores.Visible = false;
            EscogidoString.Visible = false;
            label9.Text = "GENERAR CODIGO DE PROVEEDORES";
        }
        #endregion


        #region=========================estados=================  
        private void loadState(bool state)
        {
            appLoadState(state);
            this.Enabled = true;
        }

        public void appLoadState(bool state)
        {
            if (state)
            {
                progressStatus.Style = ProgressBarStyle.Marquee;
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                progressStatus.Style = ProgressBarStyle.Blocks;
                this.Cursor = Cursors.Default;
            }
        }
        #endregion=========================estados=====================

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
                case Keys.F2: // productos
                    txtRazonSocial.Focus();
                    break;
                case Keys.Escape: // productos
                    cerrar(); ;
                    break;


            }




        }
        #endregion


        private string quitarC(string texto)
        {


            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);
            //coincide todo lo que no sean letras y números ascii o espacio
            //y lo reemplazamos por una cadena vacía.
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            return reg.Replace(textoNormalizado, "");

        }



        private async void btnAceptar_Click(object sender, EventArgs e)
        {

            if(contadorProvedores>1 )
            {
                MessageBox.Show("No puedes tener mas de un proveedor", "Seleccionar Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if ( contadorProvedores < 1)
            {
                MessageBox.Show("Cantidad de proveedores debe ser de por lo menos 1 ", "Seleccionar Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            loadState(true);
            try
            {
                // crear codigo reordenarEscogidos
               
                AsiganarPedidosProveedor asiganarPedidosProveedor = new AsiganarPedidosProveedor();

                // reordenar
                int orden = 1;
                foreach(Proveedor proveedor in listProveedores)
                {

                    if (proveedor.escogido > 0)
                        proveedor.escogido = orden++;
                }
         
                asiganarPedidosProveedor.pedidos = this.detallePedidoProveedor;
                asiganarPedidosProveedor.provedores = this.listProveedores;
                Response response = await proveedorModel.modificarEscogidos(asiganarPedidosProveedor);// 

                guardar = true;
                if (response.id > 0)
                {
                    cargarRegistro();
                    MessageBox.Show("Asignación Realizada con exito", "Asignar Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message, "Modificar Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            finally
            {

                loadState(false);
            }


        }

        private void bunifuCustomDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMotivo_KeyUp(object sender, KeyEventArgs e)
        {

            BindingList<Proveedor> filtered = new BindingList<Proveedor>(listProveedores.Where(obj => obj.razonSocial.ToUpper().Contains(txtRazonSocial.Text.Trim().ToUpper())).ToList());
            proveedorBindingSource.DataSource = filtered;
            dgvProductos.Update();

        }

       



        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvProductos.CurrentRow.Index; // Identificando la fila actual del datagridview
            int idProveedor = Convert.ToInt32(dgvProductos.Rows[index].Cells["idProveedor"].Value); // obteniedo el idCategoria del datagridview
            currentProveedor = listProveedores.Find(x => x.idProveedor == idProveedor); // Buscando la registro especifico en la lista de registros
            if (contadorProvedores < 1) {

                if (currentProveedor.escogido == 0)
                {
                    contadorProvedores++;
                    currentProveedor.escogido = contadorProvedores;
                    listProveedoresEscogidos.Add(currentProveedor);
                }

                else
                {
                    currentProveedor.escogido = 0;
                    contadorProvedores--;
                    listProveedoresEscogidos.Remove(currentProveedor);
                }


            }
            else
            {
                if (contadorProvedores == 1)
                {
                    if(currentProveedor.escogido >0)
                    {
                        currentProveedor.escogido = 0;
                        contadorProvedores--;
                        listProveedoresEscogidos.Remove(currentProveedor);
                    }
                    else
                    {
                        MessageBox.Show("Nro de proveedores Completados", "Error producto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                   
                }
                else
                {
                    // el caso de seleccionar varios pedidos con varios proveedores distintos
                    currentProveedor.escogido = 0;
                    contadorProvedores--;
                    listProveedoresEscogidos.Remove(currentProveedor);
                }

            }
               
            lbnroProveedores.Text = contadorProvedores.ToString();
            productoVentaBindingSource.DataSource = listProveedores;
            dgvProductos.Refresh();

        }


        public void cargarProducto()
        {
            if (dgvProductos.Rows.Count == 0) return;
            try
            {
                //int index = dgvProductos.CurrentRow.Index; // Identificando la fila actual del datagridview
                //int idpresentacion = Convert.ToInt32(dgvProductos.Rows[index].Cells[0].Value); // obteniedo el idCategoria del datagridview
                //currentProducto = listProductosfiltrada.Find(x => x.idPresentacion == idpresentacion); // Buscando la registro especifico en la lista de registros
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message, "Error producto", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            cerrar();


        }

        private void cerrar()
        {
            salir = true;
            this.Close();
        }

    

        private void txtMotivo_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvProductos_Enter(object sender, EventArgs e)
        {

            //if (listProductosfiltrada.Count == 1)
            //{

            //    cargarProducto();
            //}
        }

        private void FormBuscarProducto_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void FormBuscarProducto_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void FormBuscarProducto_Enter(object sender, EventArgs e)
        {
            //if (e.KeyCode == Keys.Enter && !faltaCliente && !faltaProducto)
            //{
            //    this.SelectNextControl((Control)sender, true, true, true, true);
            //}
            //if (listProductosfiltrada.Count == 1)
            //{

            //    cargarProducto();
            //}
        }

        private void dgvProductos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dgvProductos_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                e.SuppressKeyPress = true;
                cargarProducto();
            }
        }

        
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewTextBoxCell dd      = dgvProductos.CurrentCell as DataGridViewTextBoxCell;
            if (!String.IsNullOrEmpty(dd.Value.ToString()))
                Clipboard.SetDataObject(dd.Value.ToString());
           
        }

        private async void btnGenerar_Click(object sender, EventArgs e)
        {

            loadState(true);
            foreach (Proveedor P in listProveedores)
            {
               
                    Thread.Sleep(100);
                    string generar = generarCodigo();
                    P.codigoGenerado = generar;
              
            }
            try
            {
                Response response = await proveedorModel.modificarCodigos(listProveedores);// 
                if (response.id > 0)
                {
                MessageBox.Show("Generacion de Codigo Realizada con Exito", "Generar codigo Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("No se puedo Generar Codigos", "Generar codigo Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "Generar codigo Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            // recargar los proveedores
            try
            {
                
                listProveedores = await proveedorModel.listaProveedoresSeleccion();
                contadorProvedores = 0;
                foreach (Proveedor p in listProveedores)
                {
                    p.escogido = 0;

                }
                proveedorBindingSource.DataSource = listProveedores;
                lbnroProveedores.Text = contadorProvedores.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message, "Generar codigo Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            finally
            {

                loadState(false);
            }


           

        }
    }

    class CodidoGenerado
    {
        public int idProveedor { set; get; }
        public string codigoGenerado { set; get; }
    }

}
