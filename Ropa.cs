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
    public partial class Ropa : Form
    {
        bool menuExpandidoRopa = true;
        public Ropa()
        {
            InitializeComponent();
        }

        private void Ropa_Load(object sender, EventArgs e)
        {
            CargarProductosEnListBoxRopa();
            panel4R.Visible = false;
            dataGridView4Ropa.AutoGenerateColumns = true;
            CargarCategoriasRopa();
            CargarProductosRopa();
            CargarCategoriasEnGridRopa();
            CargarProductosEnComboBoxRopa();
            CargarComboBoxProductosRopa();
            CargarComboBoxNombresRopa();
            CargarProductosEnComboBox2Ropa();
        }

        private void BtnAgregar_P_Ropa_Click(object sender, EventArgs e)
        {
            panel4R.Visible = false;
            panel5R.Visible = false;
            panel6R.Visible = false;
            panel7R.Visible = false;
            panel1R.Visible = true;
            panel8.Visible = false;
        }

        private void btn_agregarRopa_Click(object sender, EventArgs e)
        {
            if (cmb_categoriaRopa.SelectedIndex > 0 && cmb_categoriaRopa.SelectedValue != null)
            {
                try
                {
                    int idCategoria = Convert.ToInt32(cmb_categoriaRopa.SelectedValue);

                    using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoRopa;"))
                    {
                        conn.Open();
                        string query = @"INSERT INTO Producto (nombre, precio_unitario, stock_actual, id_categoria)
                                         VALUES (@nombre, @precio, @stock, @categoria)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", txt_nombreRopa.Text);
                            cmd.Parameters.AddWithValue("@precio", decimal.Parse(txt_precioRopa.Text));
                            cmd.Parameters.AddWithValue("@stock", int.Parse(txt_cantidadRopa.Text));
                            cmd.Parameters.AddWithValue("@categoria", idCategoria);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto insertado correctamente.");

                            CargarProductosEnComboBoxRopa();
                            CargarComboBoxProductosRopa();
                            CargarComboBoxNombresRopa();
                            CargarProductosRopa();
                            CargarProductosEnComboBox2Ropa();
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

        private void BtnEliminar_P1_Ropa_Click(object sender, EventArgs e)
        {
            txt_nombreRopa.Clear();
            txt_cantidadRopa.Clear();
            txt_precioRopa.Clear();
            cmb_categoriaRopa.SelectedIndex = -1;
        }

        private void btn_actualizar_datosRopa_Click(object sender, EventArgs e)
        {
            if (cmbidproductoRopa.SelectedIndex < 0 || cmbidproductoRopa.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un producto válido para actualizar.");
                return;
            }

            string nombre = txt_nombre_productoRopa.Text.Trim();
            bool nombreValido = !string.IsNullOrWhiteSpace(nombre);

            decimal precio = 0;
            bool precioValido = decimal.TryParse(Txt_actualizar_precioRopa.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out precio);

            if (!nombreValido && !precioValido)
            {
                MessageBox.Show("Debes ingresar al menos el nombre o el precio para actualizar.");
                return;
            }

            string nombreProducto = cmbidproductoRopa.SelectedItem.ToString();
            int idProducto = ObtenerIdProductoRopa(nombreProducto);

            if (idProducto == -1)
            {
                MessageBox.Show("Error al obtener el ID del producto.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoRopa;"))
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
                        CargarProductosRopa();
                        CargarProductosEnComboBox2Ropa();
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

        private void btn_eleiminar_productoRopa_Click(object sender, EventArgs e)
        {
            string nombreProducto = cmb_eliminar_productoRopa.SelectedItem?.ToString();

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
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoRopa;"))
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
                                // Remover el item del ComboBox manualmente
                                cmb_eliminar_productoRopa.Items.Remove(nombreProducto);
                                cmb_consultar_productoRopa.Items.Remove(nombreProducto);
                                CargarProductosRopa();
                                CargarProductosEnComboBox2Ropa();
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

        private void BtnElimnarCantidad_Ropa_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_productos_nombre2Ropa.SelectedItem?.ToString();
            string cantidadTexto = txt_de_la_salida_de_los_productosRopa.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto) || !int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de salida.");
                return;
            }

            int idProducto = ObtenerIdProductoRopa(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
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
                txt_de_la_salida_de_los_productosRopa.Clear();
                CargarProductosRopa();
            }
        }

        private void btn_volver_Click(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();
        }

        private void bTNEntradaySalida_Ropa_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel7R.Visible = true;
            panel6R.Visible = true;
            panel5R.Visible = true;
            panel1R.Visible = true;
            panel4R.Visible = true;
        }

        private void btn_consultar_productos_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel7R.Visible = false;
            panel1R.Visible = true;
            panel6R.Visible = true;
            panel5R.Visible = true;
            panel4R.Visible = true;
        }

        private void btn_consultarRopa_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel1R.Visible = true;
            panel4R.Visible = true;
            panel7R.Visible = false;
            panel5R.Visible = false;
            panel6R.Visible = false;
        }

        private void BtnBorrarProductosRopa_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel5R.Visible = true;
            panel1R.Visible = true;
            panel6R.Visible = false;
            panel4R.Visible = true;
            panel7R.Visible = false;
        }

        private void BtnAgregar_Cantidad_Ropa_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_nombre_productosRopa.SelectedItem?.ToString();
            if (!int.TryParse(txt_entrada_de_los_productoRopa.Text.Trim(), out int cantidad)
                || cantidad <= 0
                || string.IsNullOrEmpty(nombreProducto))
            {
                MessageBox.Show("Verifica el producto y la cantidad de entrada.");
                return;
            }
            int idProducto = ObtenerIdProductoRopa(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado en la base de datos.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoRopa;"))
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
                txt_entrada_de_los_productoRopa.Clear();
                CargarProductosRopa();
            }
        }

        private void CargarProductosEnListBoxRopa()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
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

        private void CargarCategoriasEnGridRopa()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
            {
                conn.Open();
                string query = "SELECT p.id_producto as id, p.nombre as producto, p.precio_unitario as precio, p.stock_actual as cantidad, c.nombre_categoria as categoria FROM Producto p INNER JOIN Categoria c ON p.id_categoria = c.id_categoria order by id_producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            dataGridView2Ropa.DataSource = dt;
            dataGridView2Ropa.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2Ropa.RowTemplate.Height = 30;
        }

        private void CargarProductosEnComboBoxRopa()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
            {
                conn.Open();
                string query = "SELECT id_producto FROM Producto ORDER BY id_producto ASC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmbidproductoRopa.Items.Clear();
                    while (reader.Read())
                    {
                        cmbidproductoRopa.Items.Add(reader["id_producto"].ToString());
                    }
                }
            }
        }

        private void cmb_consultar_productoRopa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_consultar_productoRopa.SelectedItem != null)
            {
                string nombreProducto = cmb_consultar_productoRopa.SelectedItem.ToString();
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
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
                        dataGridView3Ropa.DataSource = dt;
                    }
                }
            }
        }

        private void CargarComboBoxProductosRopa()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
            {
                conn.Open();
                string query = "SELECT nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmbidproductoRopa.Items.Clear();
                    while (reader.Read())
                    {
                        cmbidproductoRopa.Items.Add(reader["nombre"].ToString());
                    }
                }
            }
        }

        private void CargarComboBoxNombresRopa()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
            {
                conn.Open();
                string query = "SELECT nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmb_eliminar_productoRopa.Items.Clear();
                    while (reader.Read())
                    {
                        cmb_eliminar_productoRopa.Items.Add(reader["nombre"].ToString());
                    }
                }
            }
        }

        private void CargarProductosEnComboBox2Ropa()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nombre FROM Producto";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Cmb_nombre_productosRopa.Items.Clear();
                        Cmb_productos_nombre2Ropa.Items.Clear();
                        cmb_eliminar_productoRopa.Items.Clear();
                        cmb_consultar_productoRopa.Items.Clear();
                        while (reader.Read())
                        {
                            string nombre = reader["nombre"].ToString();
                            Cmb_nombre_productosRopa.Items.Add(nombre);
                            Cmb_productos_nombre2Ropa.Items.Add(nombre);
                            cmb_eliminar_productoRopa.Items.Add(nombre);
                            cmb_consultar_productoRopa.Items.Add(nombre);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }
        }

        private int ObtenerIdProductoRopa(string nombreProducto)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
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

        private void CargarProductosRopa()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
            {
                conn.Open();
                string query = "SELECT p.id_producto as id, p.nombre as producto, p.precio_unitario as precio, p.stock_actual as cantidad, c.nombre_categoria as categoria FROM Producto p INNER JOIN Categoria c ON p.id_categoria = c.id_categoria";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            dataGridView4Ropa.DataSource = dt;
            dataGridView2Ropa.DataSource = dt;
            dataGridView3Ropa.DataSource = dt;
            dataGridView1Ropa.DataSource = dt;
            dataGridView4Ropa.RowTemplate.Height = 80;
            dataGridView4Ropa.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void CargarCategoriasRopa()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoRopa;"))
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
                cmb_categoriaRopa.DataSource = dt;
                cmb_categoriaRopa.DisplayMember = "nombre_categoria";
                cmb_categoriaRopa.ValueMember = "id_categoria";
                cmb_categoriaRopa.SelectedIndex = 0;
                cmbcategoriaactualizacionproductoRopa.DataSource = dt;
                cmbcategoriaactualizacionproductoRopa.DisplayMember = "nombre_categoria";
                cmbcategoriaactualizacionproductoRopa.ValueMember = "id_categoria";
                cmbcategoriaactualizacionproductoRopa.SelectedIndex = 0;
            }
        }

        private void BtnEliminarCantidad_Ropa_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_productos_nombre2Ropa.SelectedItem?.ToString();
            string cantidadText = txt_de_la_salida_de_los_productosRopa.Text.Trim();
            if (string.IsNullOrEmpty(nombreProducto)
                || !int.TryParse(cantidadText, out int cantidad)
                || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de salida.");
                return;
            }
            int idProducto = ObtenerIdProductoRopa(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado en la base de datos.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoRopa;"))
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
                txt_de_la_salida_de_los_productosRopa.Clear();
                CargarProductosRopa();
            }
        }

        private void BtnEliminarCantidad_Ropa_Click_1(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_productos_nombre2Ropa.SelectedItem?.ToString();
            string cantidadText = txt_de_la_salida_de_los_productosRopa.Text.Trim();
            if (string.IsNullOrEmpty(nombreProducto)
                || !int.TryParse(cantidadText, out int cantidad)
                || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de salida.");
                return;
            }
            int idProducto = ObtenerIdProductoRopa(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado en la base de datos.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=BasededatosdelproyectoRopa;"))
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
                txt_de_la_salida_de_los_productosRopa.Clear();
                CargarProductosRopa();
            }
        }
    }
}