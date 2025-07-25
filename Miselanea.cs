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
    public partial class Miselanea : Form
    {
        public Miselanea()
        {
            InitializeComponent();
        }

        private void Miselanea_Load(object sender, EventArgs e)
        {
            CargarProductosEnListBoxMis();
            panel4M.Visible = false;
            dataGridView4Mis.AutoGenerateColumns = true;
            CargarCategoriasMis();
            CargarProductosMis();
            CargarCategoriasEnGridMis();
            CargarComboBoxProductosMis();
            CargarComboBoxNombresMis();
            CargarProductosEnComboBox2Mis();
        }

        private void BtnAgregar_P_Mis_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel4M.Visible = false;
            panel2M.Visible = false;
            panel6M.Visible = false;
            panel7M.Visible = false;
            panel1M.Visible = true;
        }

        private void btn_agregarMis_Click(object sender, EventArgs e)
        {
            if (cmb_categoriaMis.SelectedIndex > 0 && cmb_categoriaMis.SelectedValue != null)
            {
                try
                {
                    int idCategoria = Convert.ToInt32(cmb_categoriaMis.SelectedValue);

                    using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
                    {
                        conn.Open();
                        string query = @"INSERT INTO Producto (nombre, precio_unitario, stock_actual, id_categoria)
                                         VALUES (@nombre, @precio, @stock, @categoria)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", txt_nombreMis.Text);
                            cmd.Parameters.AddWithValue("@precio", decimal.Parse(txt_precioMis.Text));
                            cmd.Parameters.AddWithValue("@stock", int.Parse(txt_cantidadMis.Text));
                            cmd.Parameters.AddWithValue("@categoria", idCategoria);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto insertado correctamente.");

                            CargarComboBoxProductosMis();
                            CargarComboBoxNombresMis();
                            CargarProductosMis();
                            CargarProductosEnComboBox2Mis();
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

        private void BtnEliminar_P1_Mis_Click(object sender, EventArgs e)
        {
            txt_nombreMis.Clear();
            txt_cantidadMis.Clear();
            txt_precioMis.Clear();
            cmb_categoriaMis.SelectedIndex = -1;
        }

        private void btn_actualizar_datosMis_Click(object sender, EventArgs e)
        {
            if (cmbidproductoMis.SelectedIndex < 0 || cmbidproductoMis.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un producto válido para actualizar.");
                return;
            }

            string nombre = txt_nombre_productoMis.Text.Trim();
            bool nombreValido = !string.IsNullOrWhiteSpace(nombre);

            decimal precio = 0;
            bool precioValido = decimal.TryParse(Txt_actualizar_precioMis.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out precio);

            if (!nombreValido && !precioValido)
            {
                MessageBox.Show("Debes ingresar al menos el nombre o el precio para actualizar.");
                return;
            }

            string nombreProducto = cmbidproductoMis.SelectedItem.ToString();
            int idProducto = ObtenerIdProductoMis(nombreProducto);

            if (idProducto == -1)
            {
                MessageBox.Show("Error al obtener el ID del producto.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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
                        CargarProductosMis();
                        CargarComboBoxProductosMis();
                        CargarComboBoxNombresMis();
                        CargarProductosEnComboBox2Mis();
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

        private void btn_eleiminar_productoMis_Click(object sender, EventArgs e)
        {
            string nombreProducto = cmb_eliminar_productoMis.SelectedItem?.ToString();

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
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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
                                cmb_eliminar_productoMis.Items.Remove(nombreProducto);
                                cmb_consultar_productoMis.Items.Remove(nombreProducto);
                                CargarProductosMis();
                                CargarComboBoxProductosMis();
                                CargarComboBoxNombresMis();
                                CargarProductosEnComboBox2Mis();
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

        private void BtnAgregar_Cantidad_Mis_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_nombre_productosMis.SelectedItem?.ToString();
            string cantidadTexto = txt_entrada_de_los_productoMis.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto) || !int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de entrada.");
                return;
            }

            int idProducto = ObtenerIdProductoMis(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado en la base de datos.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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
                txt_entrada_de_los_productoMis.Clear();
                CargarProductosMis();
            }
        }

        private void BtnEliminarCantidad_Mis_Click(object sender, EventArgs e)
        {
            string nombreProducto = Cmb_productos_nombre2Mis.SelectedItem?.ToString();
            string cantidadTexto = txt_de_la_salida_de_los_productosMis.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto) || !int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de salida.");
                return;
            }

            int idProducto = ObtenerIdProductoMis(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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
                txt_de_la_salida_de_los_productosMis.Clear();
                CargarProductosMis();
            }
        }

        private void btn_volverMis_Click(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();
        }

        private void bTNEntradaySalida_Ropa_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel7M.Visible = true;
            panel6M.Visible = true;
            panel2M.Visible = true;
            panel1M.Visible = true;
            panel4M.Visible = true;
        }

        private void btn_consultar_productosMis_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel7M.Visible = false;
            panel1M.Visible = true;
            panel6M.Visible = true;
            panel2M.Visible = true;
            panel4M.Visible = true;
        }

        private void btn_consultarMis_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel1M.Visible = true;
            panel4M.Visible = true;
            panel7M.Visible = false;
            panel2M.Visible = false;
            panel6M.Visible = false;
        }

        private void BtnBorrarProductosMis_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2M.Visible = true;
            panel1M.Visible = true;
            panel6M.Visible = false;
            panel4M.Visible = true;
            panel7M.Visible = false;
        }

        private void CargarProductosEnListBoxMis()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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

        private void CargarCategoriasEnGridMis()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
            {
                conn.Open();
                string query = "SELECT c.nombre_categoria as categoria, p.nombre as producto FROM Categoria c INNER JOIN Producto p ON c.id_categoria = p.id_categoria";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            dataGridView2Mis.DataSource = dt;
            dataGridView2Mis.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2Mis.RowTemplate.Height = 30;
        }

        private void CargarComboBoxProductosMis()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
            {
                conn.Open();
                string query = "SELECT nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmbidproductoMis.Items.Clear();
                    while (reader.Read())
                    {
                        cmbidproductoMis.Items.Add(reader["nombre"].ToString());
                    }
                }
            }
        }

        private void CargarComboBoxNombresMis()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
            {
                conn.Open();
                string query = "SELECT nombre FROM Producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cmb_eliminar_productoMis.Items.Clear();
                    cmb_consultar_productoMis.Items.Clear();
                    while (reader.Read())
                    {
                        string nombre = reader["nombre"].ToString();
                        cmb_eliminar_productoMis.Items.Add(nombre);
                        cmb_consultar_productoMis.Items.Add(nombre);
                    }
                }
            }
        }

        private void cmb_consultar_productoMis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_consultar_productoMis.SelectedItem != null)
            {
                string nombreProducto = cmb_consultar_productoMis.SelectedItem.ToString();
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
                {
                    conn.Open();
                    string query = @"SELECT p.id_producto, p.nombre, p.precio_unitario, p.stock_actual, c.nombre_categoria 
                                     FROM Producto p JOIN Categoria c ON p.id_categoria = c.id_categoria 
                                     WHERE p.nombre = @nombre order by id_producto";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView3Mis.DataSource = dt;
                    }
                }
            }
        }

        private void CargarProductosEnComboBox2Mis()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT nombre FROM Producto";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Cmb_nombre_productosMis.Items.Clear();
                        Cmb_productos_nombre2Mis.Items.Clear();
                        while (reader.Read())
                        {
                            string nombre = reader["nombre"].ToString();
                            Cmb_nombre_productosMis.Items.Add(nombre);
                            Cmb_productos_nombre2Mis.Items.Add(nombre);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }
        }

        private int ObtenerIdProductoMis(string nombreProducto)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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

        private void CargarProductosMis()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
            {
                conn.Open();
                string query = "SELECT p.id_producto as id, p.nombre as producto, p.precio_unitario as precio, p.stock_actual as cantidad, c.nombre_categoria as categoria FROM Producto p INNER JOIN Categoria c ON p.id_categoria = c.id_categoria order by id_producto";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            dataGridView1Mis.DataSource = dt;
            dataGridView2Mis.DataSource = dt;
            dataGridView3Mis.DataSource = dt;
            dataGridView4Mis.DataSource = dt;
            dataGridView4Mis.RowTemplate.Height = 80;
            dataGridView4Mis.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void CargarCategoriasMis()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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
                cmb_categoriaMis.DataSource = dt;
                cmb_categoriaMis.DisplayMember = "nombre_categoria";
                cmb_categoriaMis.ValueMember = "id_categoria";
                cmb_categoriaMis.SelectedIndex = 0;
                cmbcategoriaactualizacionproductoMis.DataSource = dt;
                cmbcategoriaactualizacionproductoMis.DisplayMember = "nombre_categoria";
                cmbcategoriaactualizacionproductoMis.ValueMember = "id_categoria";
                cmbcategoriaactualizacionproductoMis.SelectedIndex = 0;
            }
        }
        private void Btn_volverMis_Click_1(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();

        }

        private void panel2M_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnConsultar_Mis_Click(object sender, EventArgs e)
        {

        }

        private void cmb_consultar_productoMis_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmb_consultar_productoMis.SelectedItem != null)
            {
                string nombreProducto = cmb_consultar_productoMis.SelectedItem.ToString();
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=BasededatosdelproyectoMiscelanea;"))
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
                        dataGridView1Mis.DataSource = dt;
                    }
                }
            }
        }
    }
}