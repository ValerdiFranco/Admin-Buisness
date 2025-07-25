using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Admin_Buisness
{
    public partial class Pasteleria : Form
    {
        bool menuExpandidoPasteleria = true;
        public Pasteleria()
        {
            InitializeComponent();
        }

        private void Pasteleria_Load(object sender, EventArgs e)
        {
            CargarProductosEnListBoxPastel();
            panel4P.Visible = false;
            dataGridView4P.AutoGenerateColumns = true;
            CargarCategoriasPastel();
            CargarProductosPastel();
            CargarCategoriasEnGridPastel();
            CargarProductosEnComboBoxPastel();
            CargarComboBoxProductosPastel();
            CargarComboBoxNombresPastel();
            CargarProductosEnComboBox2Pastel();
        }

        private void BtnAgregar_P_Pastel_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel4P.Visible = false;
            panel5P.Visible = false;
            panel6P.Visible = false;
            panel7P.Visible = false;
            panel1P.Visible = true;
        }

        private void btn_agregarPastel_Click(object sender, EventArgs e)
        {
            if (cmb_categoriaPastel.SelectedIndex > 0 && cmb_categoriaPastel.SelectedValue != null)
            {
                try
                {
                    int idCategoria = Convert.ToInt32(cmb_categoriaPastel.SelectedValue);

                    using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
                    {
                        conn.Open();
                        string query = @"INSERT INTO Producto (nombre, precio_unitario, stock_actual, id_categoria)
                                         VALUES (@nombre, @precio, @stock, @categoria)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", txt_nombrePastel.Text);
                            cmd.Parameters.AddWithValue("@precio", decimal.Parse(txt_precioPastel.Text));
                            cmd.Parameters.AddWithValue("@stock", int.Parse(txt_cantidadPastel.Text));
                            cmd.Parameters.AddWithValue("@categoria", idCategoria);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto insertado correctamente.");

                            CargarProductosEnComboBoxPastel();
                            CargarComboBoxProductosPastel();
                            CargarComboBoxNombresPastel();
                            CargarProductosPastel();
                            CargarProductosEnComboBox2Pastel();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar producto: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona una categoría antes de continuar.");
            }
        }

        private void BtnEliminar_P1_Pastel_Click(object sender, EventArgs e)
        {
            txt_nombrePastel.Clear();
            txt_cantidadPastel.Clear();
            txt_precioPastel.Clear();
            cmb_categoriaPastel.SelectedIndex = -1;
        }

        private void btn_actualizar_datosPastel_Click(object sender, EventArgs e)
        {
            if (cmbidproductoPastel.SelectedIndex < 0 || cmbidproductoPastel.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un producto válido para actualizar.");
                return;
            }

            string nombre = txt_nombre_productoPastel.Text.Trim();
            bool nombreValido = !string.IsNullOrWhiteSpace(nombre);

            decimal precio = 0;
            bool precioValido = decimal.TryParse(Txt_actualizar_precioPastel.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out precio);

            if (!nombreValido && !precioValido)
            {
                MessageBox.Show("Debes ingresar al menos el nombre o el precio para actualizar.");
                return;
            }

            string nombreProducto = cmbidproductoPastel.SelectedItem.ToString();
            int idProducto = ObtenerIdProductoPastel(nombreProducto);

            if (idProducto == -1)
            {
                MessageBox.Show("Error al obtener el ID del producto.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
                {
                    conn.Open();

                    List<string> campos = new List<string>();
                    MySqlCommand cmd = new MySqlCommand();

                    if (nombreValido)
                    {
                        campos.Add("nombre = @nombre");
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                    }

                    if (precioValido)
                    {
                        campos.Add("precio_unitario = @precio");
                        cmd.Parameters.AddWithValue("@precio", precio);
                    }

                    string query = $"UPDATE Producto SET {string.Join(", ", campos)} WHERE id_producto = @id";
                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@id", idProducto);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MessageBox.Show("Producto actualizado correctamente.");
                        CargarProductosPastel();
                        CargarProductosEnComboBox2Pastel();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el producto con ese ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message);
            }
        }

        private void btn_eleiminar_productoPastel_Click(object sender, EventArgs e)
        {
            string nombreProducto = cmb_eliminar_productoPastel.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(nombreProducto))
            {
                MessageBox.Show("Por favor selecciona un producto para eliminar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Estás seguro que deseas eliminar el producto \"{nombreProducto}\"?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacion == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Producto WHERE nombre = @nombre";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                            int filasAfectadas = cmd.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                MessageBox.Show("Producto eliminado correctamente.");
                                cmb_eliminar_productoPastel.Items.Remove(nombreProducto);
                                cmb_consultar_productoPastel.Items.Remove(nombreProducto);
                                CargarProductosPastel();
                                CargarProductosEnComboBox2Pastel();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el producto en la base de datos.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                    }
                }
            }
        }

        private void BtnEliminarCantidad_Pastel_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_productos_nombre2Pastel.SelectedItem?.ToString();
            string cantidadTexto = txt_de_la_salida_de_los_productosPastel.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto) || !int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de salida.");
                return;
            }

            int idProducto = ObtenerIdProductoPastel(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();

                string stockQuery = "SELECT stock_actual FROM Producto WHERE id_producto = @id";
                using (MySqlCommand stockCmd = new MySqlCommand(stockQuery, conn))
                {
                    stockCmd.Parameters.AddWithValue("@id", idProducto);
                    int stockActual = Convert.ToInt32(stockCmd.ExecuteScalar());

                    if (stockActual < cantidad)
                    {
                        MessageBox.Show($"Stock insuficiente. Solo hay {stockActual} unidades.");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO SalidaInventario (id_producto, cantidad) VALUES (@id_producto, @cantidad)";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@id_producto", idProducto);
                    insertCmd.Parameters.AddWithValue("@cantidad", cantidad);
                    insertCmd.ExecuteNonQuery();
                }

                string updateQuery = "UPDATE Producto SET stock_actual = stock_actual - @cantidad WHERE id_producto = @id_producto";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@cantidad", cantidad);
                    updateCmd.Parameters.AddWithValue("@id_producto", idProducto);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Salida registrada correctamente.");
                txt_de_la_salida_de_los_productosPastel.Clear();
                CargarProductosPastel();
            }
        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();
        }

        private void bTNEntradaySalida_Pastel_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel7P.Visible = true;
            panel6P.Visible = true;
            panel5P.Visible = true;
            panel1P.Visible = true;
            panel4P.Visible = true;
        }

        private void btn_consultar_productos_Click(object sender, EventArgs e)
        {
            panel7P.Visible = false;
            panel1P.Visible = true;
            panel6P.Visible = true;
            panel5P.Visible = true;
            panel4P.Visible = true;
        }

        private void btn_consultarPastel_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel1P.Visible = true;
            panel4P.Visible = true;
            panel7P.Visible = false;
            panel5P.Visible = false;
            panel6P.Visible = false;
        }

        private void BtnBorrarProductosPastel_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel5P.Visible = true;
            panel1P.Visible = true;
            panel6P.Visible = false;
            panel4P.Visible = true;
            panel7P.Visible = false;
        }

        private void BtnAgregar_Cantidad_Pastel_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_nombre_productosPastel.SelectedItem?.ToString();
            if (!int.TryParse(txt_entrada_de_los_productoPastel.Text.Trim(), out int cantidad)
                || cantidad <= 0
                || string.IsNullOrEmpty(nombreProducto))
            {
                MessageBox.Show("Verifica el producto y la cantidad de entrada.");
                return;
            }
            int idProducto = ObtenerIdProductoPastel(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado en la base de datos.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string insertQuery = "INSERT INTO EntradaInventario (id_producto, cantidad) VALUES (@id_producto, @cantidad)";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@id_producto", idProducto);
                    insertCmd.Parameters.AddWithValue("@cantidad", cantidad);
                    insertCmd.ExecuteNonQuery();
                }

                string updateQuery = "UPDATE Producto SET stock_actual = stock_actual + @cantidad WHERE id_producto = @id_producto";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@cantidad", cantidad);
                    updateCmd.Parameters.AddWithValue("@id_producto", idProducto);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Entrada registrada correctamente.");
                txt_entrada_de_los_productoPastel.Clear();
                CargarProductosPastel();
            }
        }

        private void CargarProductosEnListBoxPastel()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT id_producto, nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id_producto"]);
                        string nombre = reader["nombre"].ToString();
                        string linea = $"[{id}] {nombre}";
                    }
                }
            }
        }

        private void CargarCategoriasEnGridPastel()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT p.id_producto as id, p.nombre as producto, p.precio_unitario as precio, p.stock_actual as cantidad, c.nombre_categoria as categoria FROM Producto p INNER JOIN Categoria c ON p.id_categoria = c.id_categoria order by id_producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            dataGridView2P.DataSource = dt;
            dataGridView2P.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2P.RowTemplate.Height = 30;
        }

        private void CargarProductosEnComboBoxPastel()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT id_producto FROM Producto ORDER BY id_producto ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmbidproductoPastel.Items.Clear();
                    while (reader.Read())
                    {
                        cmbidproductoPastel.Items.Add(reader["id_producto"].ToString());
                    }
                }
            }
        }

        private void cmb_consultar_productoPastel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_consultar_productoPastel.SelectedItem != null)
            {
                string nombreProducto = cmb_consultar_productoPastel.SelectedItem.ToString();
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
                {
                    conn.Open();
                    string query = @"SELECT p.id_producto, p.nombre, p.precio_unitario, p.stock_actual, c.nombre_categoria 
                                     FROM Producto p JOIN Categoria c ON p.id_categoria = c.id_categoria 
                                     WHERE p.nombre = @nombre";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView3P.DataSource = dt;
                    }
                }
            }
        }

        private void CargarComboBoxProductosPastel()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmbidproductoPastel.Items.Clear();
                    while (reader.Read())
                    {
                        cmbidproductoPastel.Items.Add(reader["nombre"].ToString());
                    }
                }
            }
        }

        private void CargarComboBoxNombresPastel()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmb_eliminar_productoPastel.Items.Clear();
                    while (reader.Read())
                    {
                        cmb_eliminar_productoPastel.Items.Add(reader["nombre"].ToString());
                    }
                }
            }
        }

        private void CargarProductosEnComboBox2Pastel()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nombre FROM Producto";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Cmb_nombre_productosPastel.Items.Clear();
                        Cmb_productos_nombre2Pastel.Items.Clear();
                        cmb_eliminar_productoPastel.Items.Clear();
                        cmb_consultar_productoPastel.Items.Clear();
                        while (reader.Read())
                        {
                            string nombre = reader["nombre"].ToString();
                            Cmb_nombre_productosPastel.Items.Add(nombre);
                            Cmb_productos_nombre2Pastel.Items.Add(nombre);
                            cmb_eliminar_productoPastel.Items.Add(nombre);
                            cmb_consultar_productoPastel.Items.Add(nombre);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }
        }

        private int ObtenerIdProductoPastel(string nombreProducto)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT id_producto FROM Producto WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
            }
        }

        private void CargarProductosPastel()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT p.id_producto as id, p.nombre as producto, p.precio_unitario as precio, p.stock_actual as cantidad, c.nombre_categoria as categoria FROM Producto p INNER JOIN Categoria c ON p.id_categoria = c.id_categoria";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            dataGridView4P.DataSource = dt;
            dataGridView2P.DataSource = dt;
            dataGridView3P.DataSource = dt;
            dataGridView1P.DataSource = dt;
            dataGridView4P.RowTemplate.Height = 80;
            dataGridView4P.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void CargarCategoriasPastel()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoPasteleria;"))
            {
                conn.Open();
                string query = "SELECT id_categoria, nombre_categoria FROM Categoria";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow filaPorDefecto = dt.NewRow();
                filaPorDefecto["id_categoria"] = -1;
                filaPorDefecto["nombre_categoria"] = "Selecciona una categoría...";
                dt.Rows.InsertAt(filaPorDefecto, 0);
                cmb_categoriaPastel.DataSource = dt;
                cmb_categoriaPastel.DisplayMember = "nombre_categoria";
                cmb_categoriaPastel.ValueMember = "id_categoria";
                cmb_categoriaPastel.SelectedIndex = 0;
                cmbcategoriaactualizacionproductoPastel.DataSource = dt;
                cmbcategoriaactualizacionproductoPastel.DisplayMember = "nombre_categoria";
                cmbcategoriaactualizacionproductoPastel.ValueMember = "id_categoria";
                cmbcategoriaactualizacionproductoPastel.SelectedIndex = 0;
            }
        }
        private void btn_consultar_productosPastel_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel7P.Visible = false;
            panel1P.Visible = true;
            panel6P.Visible = true;
            panel5P.Visible = true;
            panel4P.Visible = true;
        }

        private void cmb_consultar_productoPastel_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmb_consultar_productoPastel.SelectedItem != null)
            {
                string nombreProducto = cmb_consultar_productoPastel.SelectedItem.ToString();
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoPasteleria;"))
                {
                    conn.Open();
                    string query = @"SELECT p.id_producto, p.nombre, p.precio_unitario, p.stock_actual, c.nombre_categoria 
                                     FROM Producto p JOIN Categoria c ON p.id_categoria = c.id_categoria 
                                     WHERE p.nombre = @nombre";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView3P.DataSource = dt;
                    }
                }
            }
        }
    }
}