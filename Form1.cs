using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicaVeterinaria
{
    public partial class Form1 : Form
    {
        private MascotaRepository _repo = new MascotaRepository();
        private int _idSeleccionado = 0;

        // Controles de la interfaz
        private TextBox txtNombre = new TextBox();
        private TextBox txtEspecie = new TextBox();
        private TextBox txtRaza = new TextBox();
        private TextBox txtEdad = new TextBox();
        private Button btnGuardar = new Button();
        private Button btnEditar = new Button();
        private Button btnEliminar = new Button();
        private DataGridView dgvMascotas = new DataGridView();

        public Form1()
        {
            ConstruirInterfaz();
            CargarTabla();
        }

        private void ConstruirInterfaz()
        {
            this.Text = "Control Clínico Veterinario v2";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Etiquetas
            Label lblNombre = new Label { Text = "Nombre:", Location = new Point(20, 20), AutoSize = true };
            Label lblEspecie = new Label { Text = "Especie:", Location = new Point(20, 60), AutoSize = true };
            Label lblRaza = new Label { Text = "Raza:", Location = new Point(20, 100), AutoSize = true };
            Label lblEdad = new Label { Text = "Edad:", Location = new Point(20, 140), AutoSize = true };

            // Cajas de texto
            txtNombre.Location = new Point(90, 17); txtNombre.Width = 150;
            txtEspecie.Location = new Point(90, 57); txtEspecie.Width = 150;
            txtRaza.Location = new Point(90, 97); txtRaza.Width = 150;
            txtEdad.Location = new Point(90, 137); txtEdad.Width = 150;

            // Botones
            btnGuardar.Text = "Guardar"; btnGuardar.Location = new Point(20, 200); btnGuardar.Click += btnGuardar_Click;
            btnEditar.Text = "Editar"; btnEditar.Location = new Point(105, 200); btnEditar.Click += btnEditar_Click;
            btnEliminar.Text = "Eliminar"; btnEliminar.Location = new Point(190, 200); btnEliminar.Click += btnEliminar_Click;

            // Tabla DataGridView
            dgvMascotas.Location = new Point(270, 20);
            dgvMascotas.Size = new Size(340, 360);
            dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMascotas.MultiSelect = false;
            dgvMascotas.CellClick += dgvMascotas_CellClick;

            // Agregar controles a la pantalla
            this.Controls.AddRange(new Control[] {
                lblNombre, lblEspecie, lblRaza, lblEdad,
                txtNombre, txtEspecie, txtRaza, txtEdad,
                btnGuardar, btnEditar, btnEliminar, dgvMascotas
            });
        }

        private void CargarTabla()
        {
            dgvMascotas.DataSource = null;
            dgvMascotas.DataSource = _repo.ObtenerTodas();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtEspecie.Clear();
            txtRaza.Clear();
            txtEdad.Clear();
            _idSeleccionado = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Mascota m = new Mascota
            {
                Nombre = txtNombre.Text,
                Especie = txtEspecie.Text,
                Raza = txtRaza.Text,
                Edad = int.TryParse(txtEdad.Text, out int edad) ? edad : 0,
                ClienteID = 1
            };

            _repo.AgregarMascota(m);
            MessageBox.Show("Mascota guardada con éxito.");
            CargarTabla();
            LimpiarCampos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona una mascota de la lista.");
                return;
            }

            Mascota m = new Mascota
            {
                MascotaID = _idSeleccionado,
                Nombre = txtNombre.Text,
                Especie = txtEspecie.Text,
                Raza = txtRaza.Text,
                Edad = int.TryParse(txtEdad.Text, out int edad) ? edad : 0
            };

            _repo.ActualizarMascota(m);
            MessageBox.Show("Mascota actualizada con éxito.");
            CargarTabla();
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona una mascota de la lista para eliminar.");
                return;
            }

            _repo.EliminarMascota(_idSeleccionado);
            MessageBox.Show("Mascota eliminada con éxito.");
            CargarTabla();
            LimpiarCampos();
        }

        private void dgvMascotas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMascotas.Rows[e.RowIndex].Cells["MascotaID"].Value != null)
            {
                DataGridViewRow row = dgvMascotas.Rows[e.RowIndex];
                _idSeleccionado = Convert.ToInt32(row.Cells["MascotaID"].Value);
                txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
                txtEspecie.Text = row.Cells["Especie"].Value?.ToString() ?? "";
                txtRaza.Text = row.Cells["Raza"].Value?.ToString() ?? "";
                txtEdad.Text = row.Cells["Edad"].Value?.ToString() ?? "";
            }
        }
    }
}