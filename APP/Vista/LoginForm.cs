using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        //BOTON ACEPTAR
        private void AceptarButton_Click(object sender, EventArgs e)
        {
            //ERROR PROVIDER
            if (UsuarioTextBox.Text == string.Empty)
            {
                PosibleErrorProvider.SetError(UsuarioTextBox, "Ingrese un usuario");
                UsuarioTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            if (string.IsNullOrEmpty(ContraseñaTextBox.Text))
            {
                PosibleErrorProvider.SetError(ContraseñaTextBox, "Ingrese una contraseña");
                ContraseñaTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            //Validad BD
            Login login = new Login(UsuarioTextBox.Text, ContraseñaTextBox.Text);
            Usuario usuario = new Usuario();
            UsuarioBD usuarioBD = new UsuarioBD();

            usuario = usuarioBD.Auntenticar(login);

            if (usuario != null)
            {
                if (usuario.EstaActivo)
                {

                    //Mostrat Menu
                    MenuForm menuForm = new MenuForm();
                    Hide();
                    menuForm.Show();
                }
                else
                {
                    MessageBox.Show("El usuario no esta Activo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Datos de usuario incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //BOTON CANCELAR
        private void CancelarButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        //BOTON VISUALIZAR CONTRASEÑA
        private void PasswordButton_Click(object sender, EventArgs e)
        {
            if (ContraseñaTextBox.PasswordChar == '*')
            {
                ContraseñaTextBox.PasswordChar = '\0';
            }
            else { ContraseñaTextBox.PasswordChar = '*'; }
        }
    }
}
