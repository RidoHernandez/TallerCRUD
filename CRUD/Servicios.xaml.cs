using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CRUD
{
    /// <summary>
    /// Lógica de interacción para Servicios.xaml
    /// </summary>
    public partial class Servicios : Window
    {
        private Window ventanaAnterior;
        private int claveSeleccionada = -1;
        private string connectionString = "Server=20.3.152.185;Port=3306;Database=Taller;Uid=ricardo;Pwd=7febrero2006;";

        public Servicios(Window ventanaanterior)
        {
            InitializeComponent();
            this.ventanaAnterior = ventanaanterior;
        }

        private void CargarServicios(string filtroNombre = "", string filtroCosto = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT Clave_servicio, Nombre_servicio, Descripcion, Costo_base, Tiempo_estimado 
                                     FROM Servicios 
                                     WHERE Nombre_servicio LIKE @nombre";

                    if (!string.IsNullOrWhiteSpace(filtroCosto))
                    {
                        query += " AND Costo_base = @costo";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", "%" + filtroNombre + "%");

                    if (!string.IsNullOrWhiteSpace(filtroCosto))
                    {
                        cmd.Parameters.AddWithValue("@costo", filtroCosto);
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgServicios.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar servicios: " + ex.Message);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreServicio.Text))
            {
                MessageBox.Show("⚠ El nombre del servicio es obligatorio.");
                txtNombreServicio.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("⚠ La descripción es obligatoria.");
                txtDescripcion.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCostoBase.Text))
            {
                MessageBox.Show("⚠ El costo base es obligatorio.");
                txtCostoBase.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTiempoEstimado.Text))
            {
                MessageBox.Show("⚠ El tiempo estimado es obligatorio.");
                txtTiempoEstimado.Focus();
                return false;
            }

            
            if (!decimal.TryParse(txtCostoBase.Text, out decimal costo))
            {
                MessageBox.Show("⚠ El costo base debe ser un número válido.");
                txtCostoBase.Focus();
                return false;
            }

            if (costo <= 0)
            {
                MessageBox.Show("⚠ El costo base debe ser mayor a 0.");
                txtCostoBase.Focus();
                return false;
            }

            if (!int.TryParse(txtTiempoEstimado.Text, out int tiempo))
            {
                MessageBox.Show("⚠ El tiempo estimado debe ser un número entero.");
                txtTiempoEstimado.Focus();
                return false;
            }

            if (tiempo <= 0)
            {
                MessageBox.Show("⚠ El tiempo estimado debe ser mayor a 0.");
                txtTiempoEstimado.Focus();
                return false;
            }

            if (txtNombreServicio.Text.Length > 150)
            {
                MessageBox.Show("⚠ El nombre del servicio no puede superar 150 caracteres.");
                txtNombreServicio.Focus();
                return false;
            }

            if (txtDescripcion.Text.Length > 500)
            {
                MessageBox.Show("⚠ La descripción no puede superar 500 caracteres.");
                txtDescripcion.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtClaveServicio.Text = "";
            txtNombreServicio.Text = "";
            txtDescripcion.Text = "";
            txtCostoBase.Text = "";
            txtTiempoEstimado.Text = "";

            claveSeleccionada = -1;
            dgServicios.SelectedItem = null;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos())
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Servicios(Nombre_servicio, Descripcion, Costo_base, Tiempo_estimado)
                                     VALUES(@nombre, @descripcion, @costo, @tiempo)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombreServicio.Text);
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@costo", txtCostoBase.Text);
                    cmd.Parameters.AddWithValue("@tiempo", txtTiempoEstimado.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("✅ Servicio agregado correctamente.");

                    LimpiarCampos();
                    CargarServicios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar servicio: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (claveSeleccionada == -1)
            {
                MessageBox.Show("⚠ Selecciona un servicio de la tabla para editar.");
                return;
            }

            if (!ValidarCampos())
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE Servicios
                                     SET Nombre_servicio=@nombre,
                                         Descripcion=@descripcion,
                                         Costo_base=@costo,
                                         Tiempo_estimado=@tiempo
                                     WHERE Clave_servicio=@clave";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombreServicio.Text);
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@costo", txtCostoBase.Text);
                    cmd.Parameters.AddWithValue("@tiempo", txtTiempoEstimado.Text);
                    cmd.Parameters.AddWithValue("@clave", claveSeleccionada);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("✅ Servicio actualizado correctamente.");

                    LimpiarCampos();
                    CargarServicios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar servicio: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (claveSeleccionada == -1)
            {
                MessageBox.Show("⚠ Selecciona un servicio de la tabla para eliminar.");
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Seguro que deseas eliminar este servicio?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirmacion == MessageBoxResult.No)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"DELETE FROM Servicios WHERE Clave_servicio=@clave";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@clave", claveSeleccionada);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("✅ Servicio eliminado correctamente.");

                    LimpiarCampos();
                    CargarServicios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar servicio: " + ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtFiltroNombre.Text.Trim();
            string costo = txtFiltroCosto.Text.Trim();

            if (!string.IsNullOrWhiteSpace(costo))
            {
                if (!decimal.TryParse(costo, out decimal c))
                {
                    MessageBox.Show("⚠ El filtro de costo debe ser un número válido.");
                    txtFiltroCosto.Focus();
                    return;
                }
            }

            CargarServicios(nombre, costo);
        }

        private void dgServicios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgServicios.SelectedItem == null)
                return;

            DataRowView fila = dgServicios.SelectedItem as DataRowView;

            if (fila == null)
                return;

            claveSeleccionada = Convert.ToInt32(fila["Clave_servicio"]);

            txtClaveServicio.Text = claveSeleccionada.ToString();
            txtNombreServicio.Text = fila["Nombre_servicio"].ToString();
            txtDescripcion.Text = fila["Descripcion"].ToString();
            txtCostoBase.Text = fila["Costo_base"].ToString();
            txtTiempoEstimado.Text = fila["Tiempo_estimado"].ToString();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
