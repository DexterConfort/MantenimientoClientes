using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductosForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ProductosForm()
        {
            InitializeComponent();
        }

        string tipoOperacion;
        ProductoDB productoDB = new ProductoDB();
        Producto producto = null;

        //NUEVO
        private void NuevButton_Click(object sender, System.EventArgs e)
        {
            HabilitarControles();
            CodigoTextBox.Focus();
            tipoOperacion = "Nuevo";
        }
        //MODIFICAR
        private void ModificarButton_Click(object sender, System.EventArgs e)
        {
            if (ProductosDataGridView.SelectedRows.Count > 0)
            {
                tipoOperacion = "Modificar";
                LimpiarControles();
                HabilitarControles();

                CodigoTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString();
                CodigoTextBox.ReadOnly = true;
                DescripcionTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Descripcion"].Value.ToString();
                ExistenciaTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Existencia"].Value.ToString();
                PrecioTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Precio"].Value.ToString();
                byte[] imagen = productoDB.DevolverImagen(ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                if (imagen.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(imagen);
                    ImagenPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);

                }

                EstaActivoCheckBox.Checked = Convert.ToBoolean(ProductosDataGridView.CurrentRow.Cells["EstaActivo"].Value);
            }
            else { MessageBox.Show("Seleccione un Registro"); }
        }
        //GUARDAR
        private void GuardarButton_Click(object sender, System.EventArgs e)
        {
            //ERROR PROVIDER
            if (string.IsNullOrEmpty(CodigoTextBox.Text))
            {
                PosibleErrorProvider.SetError(CodigoTextBox, "Ingrese un código");
                CodigoTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            if (string.IsNullOrEmpty(DescripcionTextBox.Text))
            {
                PosibleErrorProvider.SetError(DescripcionTextBox, "Ingrese una Descripción");
                DescripcionTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            if (string.IsNullOrEmpty(ExistenciaTextBox.Text))
            {
                PosibleErrorProvider.SetError(ExistenciaTextBox, "Ingrese una Existencia");
                ExistenciaTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();
            if (string.IsNullOrEmpty(PrecioTextBox.Text))
            {
                PosibleErrorProvider.SetError(PrecioTextBox, "Ingrese un Precio");
                PrecioTextBox.Focus();
                return;
            }
            PosibleErrorProvider.Clear();

            //GUARDAR USUARIO NUEVO
            if (tipoOperacion == "Nuevo")
            {

                producto = new Producto();
                producto.Codigo = CodigoTextBox.Text;
                producto.Descripcion = DescripcionTextBox.Text;
                producto.Existencia = Convert.ToInt32(ExistenciaTextBox.Text);
                producto.Precio = Convert.ToDecimal(PrecioTextBox.Text);
                if (ImagenPictureBox.Image != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    producto.Imagen = ms.GetBuffer();
                }
                producto.EstaActivo = EstaActivoCheckBox.Checked;

                //INSERTAR EN LA BASE DE DATOS 
                Boolean inserto = productoDB.Insertar(producto);
                if (inserto)
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerProductos();

                    MessageBox.Show("Se guardo correctamente");
                }
                else { MessageBox.Show("No se pudo Guardar"); }
            }
            //GUARDAR MODIFICACIONES DE UN USUARIO
            else if (tipoOperacion == "Modificar")
            {
                producto = new Producto();
                producto.Codigo = CodigoTextBox.Text;
                producto.Descripcion = DescripcionTextBox.Text;
                producto.Existencia = Convert.ToInt32(ExistenciaTextBox.Text);
                producto.Precio = Convert.ToDecimal(PrecioTextBox.Text);
                if (ImagenPictureBox.Image != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    producto.Imagen = ms.GetBuffer();
                }
                producto.EstaActivo = Convert.ToBoolean(EstaActivoCheckBox.Checked);

                Boolean modifico = productoDB.Editar(producto);
                if (modifico)
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerProductos();
                    MessageBox.Show("Se actualizaron correctamente");
                }
                else
                {
                    MessageBox.Show("Se actualizaron correctamente");
                }
            }
        }
        //ELIMINAR
        private void EliminarButton_Click(object sender, System.EventArgs e)
        {
            if (ProductosDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro que desea eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    Boolean elimino = productoDB.Eliminar(ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                    if (elimino)
                    {
                        DeshabilitarControles();
                        TraerProductos();
                        MessageBox.Show("Se elimino correctamente el registro");
                    }
                    else { MessageBox.Show("No se elimino el registro"); }
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
            PosibleErrorProvider.Clear();
        }
        //ADJUNTAR IMAGEN
        private void AdjuntarImagenButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult resultado = dialog.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                ImagenPictureBox.Image = Image.FromFile(dialog.FileName);
            }
        }


        //EVENTOS KEYPRESS
        private void ExistenciaTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }
        private void PrecioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            if (((sender as TextBox).Text.IndexOf('.') > -1) && ((sender as TextBox).Text.Substring((sender as TextBox).Text.IndexOf('.')).Length > 2) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }


        //HABILITAR; DESHABILITAR; LIMPIAR
        private void HabilitarControles()
        {
            CodigoTextBox.Enabled = true;
            DescripcionTextBox.Enabled = true;
            ExistenciaTextBox.Enabled = true;
            PrecioTextBox.Enabled = true;
            AdjuntarImagenButton.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            NuevoButton.Enabled = false;
            ModificarButton.Enabled = false;
        }
        private void DeshabilitarControles()
        {
            CodigoTextBox.Enabled = false;
            DescripcionTextBox.Enabled = false;
            ExistenciaTextBox.Enabled = false;
            PrecioTextBox.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            AdjuntarImagenButton.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            NuevoButton.Enabled = true;
            ModificarButton.Enabled = true;
        }
        private void LimpiarControles()
        {
            CodigoTextBox.Clear();
            DescripcionTextBox.Clear();
            ExistenciaTextBox.Clear();
            PrecioTextBox.Clear();
            EstaActivoCheckBox.Checked = false;
            ImagenPictureBox.Image = null;
        }


        //MOSTRAR USUARIOS EN EL DATA GRID VIEW
        private void ProductosForm_Load(object sender, EventArgs e)
        {
            TraerProductos();
        }
        private void TraerProductos()
        {
            DataTable dt = new DataTable();
            dt = productoDB.DevolverProductos();
            ProductosDataGridView.DataSource = dt;
        }
    }
}
