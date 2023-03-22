using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }
        //BOTON USUARIOS
        private void UsuariosToolStripButton_Click(object sender, EventArgs e)
        {
            UsuariosForm usuarioForm = new UsuariosForm();
            usuarioForm.MdiParent = this;
            usuarioForm.Show();
        }
        //BOTON PRODUCTOS
        private void ProductosToolStripButton_Click(object sender, EventArgs e)
        {
            ProductosForm productosForm = new ProductosForm();
            productosForm.MdiParent = this;
            productosForm.Show();
        }
        //BOTON CLIENTES
        private void ClientesToolStripButton_Click(object sender, EventArgs e)
        {
            ClientesForm clientesForm = new ClientesForm();
            clientesForm.MdiParent = this;
            clientesForm.Show();
        }
        //BOTON FACTURA
        private void FacturaToolStripButton_Click(object sender, EventArgs e)
        {
        }

        private void SalirBackStageButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
