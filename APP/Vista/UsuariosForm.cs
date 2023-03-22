using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsuariosForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }
        string tipoOperacion;
        DataTable dt = new DataTable();
        UsuarioBD usuarioBD = new UsuarioBD();
        Usuario user = new Usuario();

        //NUEVO
        private void NuevButton_Click(object sender, EventArgs e)
        {
            CodigoTextBox.ReadOnly = false;
            RolComboBox.Items.Add("Administrador");
            RolComboBox.Items.Add("Usuario");

            HabilitarControles();
            CodigoTextBox.Focus();
            tipoOperacion = "Nuevo";
        }
        //MODIFICAR
        private void ModificarButton_Click(object sender, EventArgs e)
        {
            RolComboBox.Items.Add("Administrador");
            RolComboBox.Items.Add("Usuario");
            tipoOperacion = "Modificar";
            CodigoTextBox.ReadOnly = true;

            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                CodigoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["CodigoUsuario"].Value.ToString();
                NombreTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                ContraseñaTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Contrasena"].Value.ToString();
                CorreoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                RolComboBox.Text = UsuariosDataGridView.CurrentRow.Cells["Rol"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(UsuariosDataGridView.CurrentRow.Cells["EstaActivo"].Value);
                byte[] miFoto = usuarioBD.DevolverFoto(UsuariosDataGridView.CurrentRow.Cells["CodigoUsuario"].Value.ToString());
                if (miFoto.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(miFoto);
                    FotoPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);
                }
                HabilitarControles();
            }
            else
            {
                MessageBox.Show("Seleccione un Usuario");
            }
        }
        //GUARDAR
        private void GuardarButton_Click(object sender, EventArgs e)
        {
            //ERROR PROVIDER
            if (string.IsNullOrEmpty(CodigoTextBox.Text))
            {
                PosibleErrorProvider.SetError(CodigoTextBox, "Ingrese un código");
                CodigoTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            if (string.IsNullOrEmpty(NombreTextBox.Text))
            {
                PosibleErrorProvider.SetError(NombreTextBox, "Ingrese un nombre");
                NombreTextBox.Focus();
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
            if (string.IsNullOrEmpty(RolComboBox.Text))
            {
                PosibleErrorProvider.SetError(RolComboBox, "Seleccione un Rol");
                RolComboBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();

            //GUARDAR USUARIO NUEVO
            if (tipoOperacion == "Nuevo")
            {
                user.CodigoUsuario = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Contraseña = ContraseñaTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.EstaActivo = EstaActivoCheckBox.Checked;
                if (FotoPictureBox.Image != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    FotoPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    user.Foto = ms.GetBuffer();
                }

                //INSERTAR EN LA BASE DE DATOS 
                bool inserto = usuarioBD.Insertar(user);
                if (inserto == true)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerUsuarios();
                    MessageBox.Show("Registro Guardado");
                }
                else { MessageBox.Show("No se pudo guardar el registro"); }
            }
            //GUARDAR MODIFICACIONES DE UN USUARIO
            else if (tipoOperacion == "Modificar")
            {
                user.CodigoUsuario = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Contraseña = ContraseñaTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.EstaActivo = EstaActivoCheckBox.Checked;
                if (FotoPictureBox.Image != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    FotoPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    user.Foto = ms.GetBuffer();
                }

                Boolean modifico = usuarioBD.Editar(user);
                if (modifico)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerUsuarios();
                    MessageBox.Show("Registro Actualizado Correctamente");
                }
                else { MessageBox.Show("No se pudo Actualizar el Registro"); }

            }
        }
        //ELIMINAR
        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = usuarioBD.Eliminar(UsuariosDataGridView.CurrentRow.Cells["CodigoUsuario"].Value.ToString());
                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerUsuarios();

                        MessageBox.Show("Registro Eliminado");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro");
                    }
                }

            }
            else { MessageBox.Show("Seleccione un Registro"); }
        }
        //CANCELAR
        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
            PosibleErrorProvider.Clear();
        }
        //FOTO
        private void AdjuntarFotoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult resultado = dialog.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                FotoPictureBox.Image = Image.FromFile(dialog.FileName);
            }
        }

        //HABILITAR; DESHABILITAR; LIMPIAR
        public void HabilitarControles()
        {
            CodigoTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            ContraseñaTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            RolComboBox.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            AdjuntarFotoButton.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            NuevoButton.Enabled = false;
            ModificarButton.Enabled = false;
        }
        public void DeshabilitarControles()
        {
            CodigoTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            ContraseñaTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            RolComboBox.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            AdjuntarFotoButton.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            NuevoButton.Enabled = true;
            ModificarButton.Enabled = true;
            tipoOperacion = null;
        }
        private void LimpiarControles()
        {
            CodigoTextBox.Clear();
            NombreTextBox.Clear();
            ContraseñaTextBox.Clear();
            CorreoTextBox.Clear();
            RolComboBox.Items.Clear();
            EstaActivoCheckBox.Checked = false;
            FotoPictureBox.Image = null;
        }

        //MOSTRAR USUARIOS EN EL DATA GRID VIEW
        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            TraerUsuarios();
        }
        private void TraerUsuarios()
        {
            dt = usuarioBD.DevolverUsuarios();
            UsuariosDataGridView.DataSource = dt;
        }

    }
}