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

namespace Admeli.Productos.Nuevo.PDetalle
{
    public partial class FormElegirCategoria : Form
    {
        private CategoriaModel categoriaModel = new CategoriaModel();
        private CategoriaProducto catProducto =new CategoriaProducto();
        private List<Categoria> categoriasTodas=new List<Categoria>();
        private List<Categoria> categoriasDelProducto=new List<Categoria>();
        private FormProductoNuevo formProductoNuevo = new FormProductoNuevo();
        private UCGeneralesPD ugGeneralesPD = new UCGeneralesPD();
        private int currentIdProducto;
        private int categoriaPrincipal;

        public FormElegirCategoria()
        {
            InitializeComponent();
            this.reLoad();
        }
        public FormElegirCategoria(FormProductoNuevo formProductoNuevo,UCGeneralesPD ugGeneralesPD,int currentIdProducto)
        {
            InitializeComponent();
            this.formProductoNuevo = formProductoNuevo;
            this.currentIdProducto = currentIdProducto;
            this.ugGeneralesPD = ugGeneralesPD;
           
        }

        private async Task cargarRegistros()
        {
            catProducto = await categoriaModel.categoriaProducto(currentIdProducto);
        }
        private async Task recargarRegistros()
        {
            CategoriaProducto categoriaP = await categoriaModel.categoriaProducto(currentIdProducto);
            categoriasTodas = categoriaP.producto;
            categoriasDelProducto = categoriaP.todo;
            actualizarComboCategoria();
        }
        #region ========================================== ROOT LOAD ==========================================
        private void FormElegirCategoria_Load(object sender, EventArgs e)
        {
            reLoad();
        }

        private async void reLoad()
        {
            await cargarRegistros();
            cargarCategoriasElegidas();
            cargarCategorias();
            await recargarRegistros();
        } 
        #endregion

        #region ================================== CARGAR CATEGORIA ==================================
        private void cargarCategorias()
        {
            try
            {
                List<Categoria> categoriaList = catProducto.producto;
                if (categoriaList.Count <= 0 || categoriaList == null) { return; }

                //Cargando
                treeViewFrom.Nodes.Clear(); // limpiando
                 // Cargando categoria raiz

                List<TreeNode> listNode = new List<TreeNode>();

                foreach (Categoria categoria in categoriaList)
                {
                    TreeNode aux = new TreeNode(categoria.nombreCategoria);
                    aux.Name = categoria.idCategoria.ToString();
                    listNode.Add(aux);
                }

                treeviewVista(categoriaList, treeViewFrom, listNode);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void cargarCategoriasElegidas()
        {
            try
            {
                List<Categoria> categoriaList = catProducto.todo;
                if (categoriaList.Count <= 0 || categoriaList == null) { return; }

                //Cargando
                treeViewTo.Nodes.Clear(); // limpiando
                                            // Cargando categoria raiz

                List<TreeNode> listNode = new List<TreeNode>();

                foreach (Categoria categoria in categoriaList)
                {
                    TreeNode aux = new TreeNode(categoria.nombreCategoria);
                    aux.Name = categoria.idCategoria.ToString();
                    listNode.Add(aux);
                }

                treeviewVista(categoriaList, treeViewTo, listNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Listar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void treeviewVista(List<Categoria> categoriaList, TreeView tvActual, List<TreeNode> listNode)
        {
            if (categoriaList.Count == 0)
            {
                return;
            }
            else
            {
                Categoria aux = primerElemento(categoriaList);
                TreeNode auxTreeNode = buscar(aux, listNode);
                TreeNode nodePadre = buscarPadre(aux, listNode);
                if (hijos(categoriaList, aux))
                {
                    auxTreeNode.ImageIndex = 1;
                }
                if (nodePadre.Text != "")
                {
                    nodePadre.Nodes.Add(auxTreeNode);
                    treeviewVista(categoriaList, tvActual, listNode);
                }
                else
                {
                    tvActual.Nodes.Add(auxTreeNode);
                    treeviewVista(categoriaList, tvActual, listNode);
                }
            }
        }

        private Categoria primerElemento(List<Categoria> categoriaList)
        {

            Categoria lastCategori = categoriaList[0];//raiz
            categoriaList.Remove(lastCategori);
            return lastCategori;
        }

        private bool hijos(List<Categoria> categoriaList, Categoria padre)
        {
            bool tieneHijos = false;
            foreach (Categoria categoria in categoriaList)
            {
                if (categoria.idPadreCategoria == padre.idCategoria)
                {
                    tieneHijos = true;
                    break;
                }

            }
            return tieneHijos;
        }

        private TreeNode buscar(Categoria categoria, List<TreeNode> listNode)
        {
            TreeNode aux = new TreeNode();
            foreach (TreeNode node in listNode)
            {
                if (categoria.idCategoria.ToString() == node.Name)
                {
                    aux = node;
                    break;
                }

            }
            return aux;
        }

        private TreeNode buscarPadre(Categoria categoria, List<TreeNode> listNode)
        {
            TreeNode aux = new TreeNode();

            foreach (TreeNode node in listNode)
            {
                //if (categoria.padre == node.Text)
                if (categoria.idPadreCategoria.ToString() == node.Name)
                {
                    aux = node;
                    break;
                }
            }
            return aux;
        } 
        #endregion

        
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            DrawShape drawShape = new DrawShape();
            drawShape.lineBorder(panel4, 221, 225, 228,2);
            drawShape.lineBorder(panel5,221,225,228,2);
        }

        private void btnMovDerecha_Click(object sender, EventArgs e)
        {
            TreeNode tempNode = treeViewFrom.SelectedNode;
            if (tempNode != null)
            {
                //Eliminamos el nodo seleccionado en el TreeviewFrom y lo agregamos al TreeviewTo
                treeViewFrom.Nodes.Remove(treeViewFrom.SelectedNode);
                int idCategoriaSeleccionada = int.Parse(tempNode.Name);
                treeViewTo.Nodes.Add(tempNode);
                //Eliminamos la categoria seleccionada del categoriasTodad y lo agregamos al categoriasDelProducto
                Categoria categoriaTemp = categoriasTodas.Find(x => x.idCategoria == idCategoriaSeleccionada);
                categoriasTodas.Remove(categoriaTemp);
                categoriasDelProducto.Add(categoriaTemp);
                actualizarComboCategoria();
            }
        }

        private void btnMovIzquierda_Click(object sender, EventArgs e)
        {
            TreeNode tempNode = treeViewTo.SelectedNode;
            if (tempNode != null && categoriasDelProducto.Count>=2)
            {
                //Eliminamos el nodo seleccionado en el TreeviewTo y lo agregamos al TreeviewFrom 
                treeViewTo.Nodes.Remove(treeViewTo.SelectedNode);
                int idCategoriaSeleccionada = int.Parse(tempNode.Name);
                treeViewFrom.Nodes.Add(tempNode);
                //Eliminamos la categoria seleccionada del categoriasDelProducto y lo agregamos al categoriasTodas
                Categoria categoriaTemp = categoriasDelProducto.Find(x => x.idCategoria == idCategoriaSeleccionada);
                categoriasDelProducto.Remove(categoriaTemp);
                categoriasTodas.Add(categoriaTemp);
                actualizarComboCategoria();
            }
        }
        private void actualizarComboCategoria()
        {
            categoriaBindingSource.DataSource = null;
            categoriaBindingSource.DataSource = categoriasDelProducto;
            cbxCategoriaPrincipal.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            bloquear(true);
            if (categoriasDelProducto.Count >= 1)
            {
                formProductoNuevo.setCategorias(categoriasDelProducto, int.Parse(cbxCategoriaPrincipal.SelectedValue.ToString()));
                if (!formProductoNuevo.nuevo)
                {
                    // Si es producto existente se guarda sus categorias directemente
                    try
                    {
                        Response response = await formProductoNuevo.guardarCategoria();
                        MessageBox.Show(response.msj, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        
                    }
                    finally
                    {
                        bloquear(false);
                    }
                    
                }
                string textoCategoria = "";
                foreach (Categoria categoria in categoriasDelProducto)
                {
                    textoCategoria += categoria.nombreCategoria + " ,";
                }
                textoCategoria = textoCategoria.Substring(0, textoCategoria.Length - 1);
                ugGeneralesPD.cambioTextoCategoria(textoCategoria);
                Close();
            }
            else
            {

                MessageBox.Show("Ninguna Categoria seleccionada ", "Seleccionar Categoria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bloquear(false);
            }
        }

        public void bloquear(bool state)
        {
            if (state)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
            this.Enabled = !state;
        }
    }
}
