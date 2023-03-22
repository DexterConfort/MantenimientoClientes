using Datos;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

namespace Vista
{
    public partial class ClientesForm : Form
    {
        string tipoOperacion = "";
        Cliente cliente = null;
        ClienteDB clienteDB = null;
        public ClientesForm()
        {
            InitializeComponent();
        }

        //NUEVO
        private void NuevoButton_Click(object sender, System.EventArgs e)
        {
            IdentidadTextBox.ReadOnly = false;
            tipoOperacion = "Nuevo";
            HabilitarControles();
        }
        //MODIFICAR
        private void ModificarButton_Click(object sender, System.EventArgs e)
        {
            tipoOperacion = "Modificar";
            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                HabilitarControles();
                IdentidadTextBox.ReadOnly = true;
                IdentidadTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString();
                NombreTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                CorreoTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                TelefonoTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Telefono"].Value.ToString();
                DireccionTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Direccion"].Value.ToString();
                FechaNacimientoDateTimePicker.Text = ClientesDataGridView.CurrentRow.Cells["FechaNacimiento"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ClientesDataGridView.CurrentRow.Cells["EstaActivo"].Value);
            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }
        }
        //GUARDAR
        private void GuardarButton_Click(object sender, System.EventArgs e)
        {
            //ERROR PROVIDER
            if (string.IsNullOrEmpty(IdentidadTextBox.Text))
            {
                PosibleErrorProvider.SetError(IdentidadTextBox, "Ingrese una Identidad");
                IdentidadTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            if (string.IsNullOrEmpty(NombreTextBox.Text))
            {
                PosibleErrorProvider.SetError(NombreTextBox, "Ingrese un Nombre");
                NombreTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();

            //GUARDAR USUARIO NUEVO
            if (tipoOperacion == "Nuevo")
            {
                cliente = new Cliente();

                cliente.Identidad = IdentidadTextBox.Text;
                cliente.Nombre = NombreTextBox.Text;
                cliente.Telefono = TelefonoTextBox.Text;
                cliente.Correo = CorreoTextBox.Text;
                cliente.Direccion = DireccionTextBox.Text;
                cliente.FechaNacimiento = Convert.ToDateTime(FechaNacimientoDateTimePicker.Text);
                cliente.EstaActivo = EstaActivoCheckBox.Checked;

                //INSERTAR EN LA BASE DE DATOS 
                clienteDB = new ClienteDB();
                if (clienteDB.Insertar(cliente))
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerClientes();
                    MessageBox.Show("Fue exitoso el nuevo registro");
                }
                else
                {
                    MessageBox.Show("Ocurrio un error");
                }
            }
            //GUARDAR MODIFICACIONES DE UN USUARIO
            else if (tipoOperacion == "Modificar")
            {
                cliente = new Cliente();
                clienteDB = new ClienteDB();
                cliente.Identidad = IdentidadTextBox.Text;
                cliente.Nombre = NombreTextBox.Text;
                cliente.Telefono = TelefonoTextBox.Text;
                cliente.Correo = CorreoTextBox.Text;
                cliente.Direccion = DireccionTextBox.Text;
                cliente.FechaNacimiento = Convert.ToDateTime(FechaNacimientoDateTimePicker.Text);
                cliente.EstaActivo = EstaActivoCheckBox.Checked;
                if (clienteDB.Modifico(cliente))
                {
                    TraerClientes();
                    LimpiarControles();
                    DeshabilitarControles();
                    MessageBox.Show("Registro Actualizado correctamente");
                }
            }

        }
        //ELIMINAR
        private void EliminarButton_Click(object sender, System.EventArgs e)
        {
            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro que desea eliminar este cliente", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    string identidad = ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString();
                    if (clienteDB.Eliminar(identidad))
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerClientes();
                        MessageBox.Show("Registro eliminado correctamente");
                    }
                    else
                    {
                        MessageBox.Show("Ocurrio un error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro");
            }
        }
        //CANCELAR
        private void CancelarButton_Click(object sender, System.EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }


        //LIMPIAR;HABILITAR;DESHABILITAR == CONTROLES
        private void LimpiarControles()
        {
            IdentidadTextBox.Text = null;
            NombreTextBox.Text = null;
            TelefonoTextBox.Text = null;
            CorreoTextBox.Text = null;
            DireccionTextBox.Text = null;
            FechaNacimientoDateTimePicker.Text = null;
            EstaActivoCheckBox.Checked = false;
        }
        private void HabilitarControles()
        {
            IdentidadTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            TelefonoTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            DireccionTextBox.Enabled = true;
            FechaNacimientoDateTimePicker.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            EliminarButton.Enabled = false;
            NuevoButton.Enabled = false;
            ModificarButton.Enabled = false;
        }
        private void DeshabilitarControles()
        {
            PosibleErrorProvider.Clear();
            IdentidadTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            TelefonoTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            DireccionTextBox.Enabled = false;
            FechaNacimientoDateTimePicker.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            EliminarButton.Enabled = true;
            NuevoButton.Enabled = true;
            ModificarButton.Enabled = true;
        }


        //KEY PRESS
        private void IdentidadTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }
        private void TelefonoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        //MOSTRAR USUARIOS EN EL DATA GRID VIEW
        private void TraerClientes()
        {
            clienteDB = new ClienteDB();
            DataTable dt = new DataTable();
            ClientesDataGridView.DataSource = dt = clienteDB.DevolverClientes();
        }
        private void ClientesForm_Load(object sender, System.EventArgs e)
        {
            TraerClientes();
        }
    }
}
