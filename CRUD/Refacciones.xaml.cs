using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace CRUD
{
    /// <summary>
    /// Lógica de interacción para Refacciones.xaml
    /// </summary>
    public partial class Refacciones : Window
    {
        private Window ventanaAnterior;
        private int codigoSeleccionado = -1;
        private string connectionString = "Server=20.3.152.185;Port=3306;Database=Taller;Uid=ricardo;Pwd=7febrero2006;";
        //private string connectionString = "Server=127.0.0.1;Port=3307;Database=Taller;Uid=root;Pwd=root;";

        public Refacciones(Window ventanaAnterior)
        {
            InitializeComponent();
            CargarRefacciones();
            this.ventanaAnterior = ventanaAnterior;
        }

        private void CargarRefacciones(string nombre = "", string marca = "", string proveedor = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT Codigo_refaccion, Nombre, Marca, Precio_unitario, Stock_actual, Stock_minimo, Proveedor
                             FROM Refacciones
                             WHERE Nombre LIKE @nombre
                             AND Marca LIKE @marca
                             AND Proveedor LIKE @proveedor
                             ORDER BY Codigo_refaccion ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");
                        cmd.Parameters.AddWithValue("@marca", "%" + marca + "%");
                        cmd.Parameters.AddWithValue("@proveedor", "%" + proveedor + "%");

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgRefacciones.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar refacciones: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo Nombre es obligatorio.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                MessageBox.Show("El campo Marca es obligatorio.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMarca.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProveedor.Text))
            {
                MessageBox.Show("El campo Proveedor es obligatorio.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtProveedor.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("El campo Precio Unitario es obligatorio.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
            {
                MessageBox.Show("Precio Unitario debe ser numérico.\nEjemplo: 250.50", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (precio <= 0)
            {
                MessageBox.Show("El Precio Unitario debe ser mayor a 0.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStockActual.Text))
            {
                MessageBox.Show("El campo Stock Actual es obligatorio.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStockActual.Focus();
                return false;
            }

            if (!int.TryParse(txtStockActual.Text, out int stockActual))
            {
                MessageBox.Show("Stock Actual debe ser un número entero.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStockActual.Focus();
                return false;
            }

            if (stockActual < 0)
            {
                MessageBox.Show("Stock Actual no puede ser negativo.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStockActual.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStockMinimo.Text))
            {
                MessageBox.Show("El campo Stock Mínimo es obligatorio.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStockMinimo.Focus();
                return false;
            }

            if (!int.TryParse(txtStockMinimo.Text, out int stockMinimo))
            {
                MessageBox.Show("Stock Mínimo debe ser un número entero.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStockMinimo.Focus();
                return false;
            }

            if (stockMinimo < 0)
            {
                MessageBox.Show("Stock Mínimo no puede ser negativo.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStockMinimo.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtMarca.Text = "";
            txtPrecio.Text = "";
            txtStockActual.Text = "";
            txtStockMinimo.Text = "";
            txtProveedor.Text = "";

            codigoSeleccionado = 0;

            dgRefacciones.SelectedItem = null;
        }

        private void dgRefacciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRefacciones.SelectedItem == null)
                return;

            DataRowView fila = (DataRowView)dgRefacciones.SelectedItem;

            codigoSeleccionado = Convert.ToInt32(fila["Codigo_refaccion"]);

            txtCodigo.Text = fila["Codigo_refaccion"].ToString();
            txtNombre.Text = fila["Nombre"].ToString();
            txtMarca.Text = fila["Marca"].ToString();
            txtPrecio.Text = fila["Precio_unitario"].ToString();
            txtStockActual.Text = fila["Stock_actual"].ToString();
            txtStockMinimo.Text = fila["Stock_minimo"].ToString();
            txtProveedor.Text = fila["Proveedor"].ToString();
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Refacciones (Nombre, Marca, Precio_unitario, Stock_actual, Stock_minimo, Proveedor)
                                     VALUES (@nombre, @marca, @precio, @stockActual, @stockMinimo, @proveedor)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@marca", txtMarca.Text.Trim());
                        cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text, CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("@stockActual", Convert.ToInt32(txtStockActual.Text));
                        cmd.Parameters.AddWithValue("@stockMinimo", Convert.ToInt32(txtStockMinimo.Text));
                        cmd.Parameters.AddWithValue("@proveedor", txtProveedor.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Refacción agregada correctamente.", "ÉXITO", MessageBoxButton.OK, MessageBoxImage.Information);

                LimpiarCampos();
                CargarRefacciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar refacción: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (codigoSeleccionado == 0)
            {
                MessageBox.Show("Selecciona una refacción en la tabla para editar.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidarCampos()) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE Refacciones 
                                     SET Nombre=@nombre, Marca=@marca, Precio_unitario=@precio,
                                         Stock_actual=@stockActual, Stock_minimo=@stockMinimo, Proveedor=@proveedor
                                     WHERE Codigo_refaccion=@codigo";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigoSeleccionado);
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@marca", txtMarca.Text.Trim());
                        cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text, CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("@stockActual", Convert.ToInt32(txtStockActual.Text));
                        cmd.Parameters.AddWithValue("@stockMinimo", Convert.ToInt32(txtStockMinimo.Text));
                        cmd.Parameters.AddWithValue("@proveedor", txtProveedor.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Refacción editada correctamente.", "ÉXITO", MessageBoxButton.OK, MessageBoxImage.Information);

                LimpiarCampos();
                CargarRefacciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar refacción: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (codigoSeleccionado == 0)
            {
                MessageBox.Show("Selecciona una refacción en la tabla para eliminar.", "VALIDACIÓN", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Seguro que deseas eliminar esta refacción?",
                "CONFIRMAR",
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

                    string query = "DELETE FROM Refacciones WHERE Codigo_refaccion=@codigo";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigoSeleccionado);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Refacción eliminada correctamente.", "ÉXITO", MessageBoxButton.OK, MessageBoxImage.Information);

                LimpiarCampos();
                CargarRefacciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar refacción: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtFiltroNombre.Text.Trim();
            string marca = txtFiltroMarca.Text.Trim();
            string proveedor = txtFiltroProveedor.Text.Trim();

            CargarRefacciones(nombre, marca, proveedor);
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
