using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
using System.Globalization;
//using MySql.Data;

namespace Admin_Buisness
{
    public partial class Negocio_papeleria : Form

    {

        int id;

        string nombre;

        string descripcion;

        string categoria;

        double precio;

        int stock;

        string fechadeingreso;

        string fechadesalida;





        public Negocio_papeleria()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CargarProductosEnListBox();
            panel4.Visible = false;
            dataGridView1.AutoGenerateColumns = true;
            CargarCategorias();
            CargarProductos();
            CargarCategoriasEnGrid();
            CargarProductosEnComboBox();
            CargarComboBoxProductos();
            CargarProductosEnComboBox2();

            //string Cancion = @"C:\Users\salas\Downloads\papa-pizzeria.wav";

            //SoundPlayer sonido= new SoundPlayer(Cancion);

            //sonido.Play();




        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel5.Visible = false;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Visible = true;


        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel4.Visible = false;
        }

        private void btn_imagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Filter = "Imágenes (.jpg;.png;.bmp)|.jpg;.png;.bmp";
                dialogo.Title = "Seleccionar imagen del producto";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Carga la imagen en el PictureBox


                        MessageBox.Show("Imagen cargada correctamente.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al cargar la imagen:\n" + ex.Message);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txt_nombre.Clear();
            txt_cantidad.Clear();
            txt_precio.Clear();

            cmb_categoria.SelectedIndex = -1;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //conexion1.Conectar();
            //string resultado = conexion1.Conectar();
            //label6.Text = resultado;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //Ado conexion1 = new Ado();
            //DataTable productos = conexion1.Productos(); // Este método lo definiste antes
            //dgv_productos.DataSource = productos;
            //dgv_productos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
                {
                    conn.Open();
                    MessageBox.Show("¡Conexión exitosa a la base de datos!", "Conectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            if (cmb_categoria.SelectedIndex > 0 && cmb_categoria.SelectedValue != null)
            {
                try
                {
                    int idCategoria = Convert.ToInt32(cmb_categoria.SelectedValue);

                    using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
                    {
                        conn.Open();
                        string query = @"INSERT INTO Producto (nombre, precio_unitario, stock_actual, id_categoria)
                                 VALUES (@nombre, @precio, @stock, @categoria)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", txt_nombre.Text);
                            cmd.Parameters.AddWithValue("@precio", decimal.Parse(txt_precio.Text));
                            cmd.Parameters.AddWithValue("@stock", int.Parse(txt_cantidad.Text));
                            cmd.Parameters.AddWithValue("@categoria", idCategoria);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto insertado correctamente.");
                            CargarProductosEnComboBox();
                            CargarComboBoxProductos();
                            CargarProductos();
                            CargarProductosEnComboBox2();
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

        private void cmb_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void CargarCategorias()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
            {
                conn.Open();
                string query = "SELECT id_categoria, nombre_categoria FROM categoria";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow filaPorDefecto = dt.NewRow();
                filaPorDefecto["id_categoria"] = -1;
                filaPorDefecto["nombre_categoria"] = "Selecciona una categoría...";
                dt.Rows.InsertAt(filaPorDefecto, 0);

                cmb_categoria.DataSource = dt;
                cmb_categoria.DisplayMember = "nombre_categoria";
                cmb_categoria.ValueMember = "id_categoria";
                cmb_categoria.SelectedIndex = 0;

                cmbcategoriaactualizacionproducto.DataSource = dt;
                cmbcategoriaactualizacionproducto.DisplayMember = "nombre_categoria";
                cmbcategoriaactualizacionproducto.ValueMember = "id_categoria";
                cmbcategoriaactualizacionproducto.SelectedIndex = 0;
            }
        }


        private void CargarProductos()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=basededatosdelproyecto;"))
            {
                conn.Open();
                string query = "SELECT p.id_producto as id, p.nombre as producto, p.precio_unitario as precio, p.stock_actual as cantidad, c.nombre_categoria as categoria FROM Producto p inner join Categoria c on p.id_categoria = c.id_categoria";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }


            dataGridView1.DataSource = dt;

            dataGridView2.DataSource = dt;

            dataGridView3.DataSource = dt;

            dataGridView4.DataSource = dt;



            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }





        private void btn_volver_Click(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button3_Click_2(object sender, EventArgs e)
        {

        }

        private void CargarProductosEnListBox()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=basededatosdelproyecto;"))
            {
                conn.Open();
                string query = @"SELECT id_producto, nombre FROM Producto";

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


        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_3(object sender, EventArgs e)
        {
            string cadenaConexion = "server=localhost;user id=root;password=;database=Basededatosdelproyecto;";
            string nombreProducto = cmb_productos.SelectedItem?.ToString();

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
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    try
                    {
                        conexion.Open();

                        string consulta = "DELETE FROM Producto WHERE nombre = @nombre";

                        using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                        {
                            comando.Parameters.AddWithValue("@nombre", nombreProducto);
                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                MessageBox.Show("Producto eliminado correctamente.");
                                cmb_productos_nombre2.Items.Remove(nombreProducto);
                                cmb_consultar_producto.Items.Remove(nombreProducto);
                                CargarProductos();
                                CargarProductosEnComboBox();
                                CargarProductosEnComboBox2();
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

        private void CargarCategoriasEnGrid()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
            {
                conn.Open();
                string query = @"select c.nombre_categoria as categoria, p.nombre as producto
from categoria c inner
join producto p
where c.id_categoria = 2";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }





            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.RowTemplate.Height = 30;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

            panel5.Visible = true;
            panel4.Visible = false;
            panel6.Visible = false;
            panel1.Visible = true;



        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            panel1.Visible = true;
            panel6.Visible = true;
            panel5.Visible = true;
            panel4.Visible = true;

        }

        private void panel4_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void CargarProductosEnComboBox()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=basededatosdelproyecto;"))
            {
                conn.Open();
                string query = "SELECT id_producto FROM Producto group by id_producto asc";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            cmbidproducto.DisplayMember = "id_producto";
            cmbidproducto.DataSource = dt;
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_consultar_producto.SelectedItem != null)
            {
                string nombreProducto = cmb_consultar_producto.SelectedItem.ToString();
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
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
                        dataGridView3.DataSource = dt;
                    }
                }
            }
        }

        private void btn_actualizar_datos_Click(object sender, EventArgs e)
        {
            if (cmbidproducto.SelectedIndex < 0 || cmbidproducto.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona un producto válido para actualizar.");
                return;
            }

            string nombre = txt_nombre_producto.Text.Trim();
            bool nombreValido = !string.IsNullOrWhiteSpace(nombre);

            decimal precio = 0;
            bool precioValido = decimal.TryParse(Txt_actualizar_precio.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out precio);

            if (!nombreValido && !precioValido)
            {
                MessageBox.Show("Debes ingresar al menos el nombre o el precio para actualizar.");
                return;
            }

            object valorProducto = cmbidproducto.SelectedValue;
            if (valorProducto == null || !int.TryParse(valorProducto.ToString(), out int idProducto))
            {
                MessageBox.Show("Error al obtener el ID del producto.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
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
                        CargarProductos();
                        CargarProductosEnComboBox2();
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

        private void CargarComboBoxProductos()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=;database=Basededatosdelproyecto;"))
            {
                conn.Open();
                string query = "SELECT id_producto, nombre FROM Producto";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable tablaDeProductos = new DataTable();
                adapter.Fill(tablaDeProductos);

                cmbidproducto.DisplayMember = "nombre";
                cmbidproducto.ValueMember = "id_producto";
                cmbidproducto.DataSource = tablaDeProductos;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
        }

        private void button2_Click_4(object sender, EventArgs e)
        {
            panel5.Visible = true;
            panel1.Visible = true;
            panel6.Visible = false;
            panel4.Visible = true;
        }

        private void button3_Click_3(object sender, EventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            panel7.Visible = true;
            panel6.Visible = true;
            panel5.Visible = true;
            panel1.Visible = true;
            panel4.Visible = true;
            panel8.Visible = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CargarProductosEnComboBox2()
        {
            string cadenaConexion = "server=localhost;user id=root;password=;database=Basededatosdelproyecto;";

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT nombre FROM Producto";

                    using (MySqlCommand comando = new MySqlCommand(query, conexion))
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        cmb_nombre_productos.Items.Clear();
                        cmb_productos_nombre2.Items.Clear();
                        cmb_productos.Items.Clear();


                        while (reader.Read())
                        {
                            string nombre = reader["nombre"].ToString();
                            cmb_nombre_productos.Items.Add(nombre);
                            cmb_productos_nombre2.Items.Add(nombre);
                            cmb_productos.Items.Add(nombre);
                            cmb_consultar_producto.Items.Add(nombre);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            string nombreProducto = cmb_nombre_productos.SelectedItem?.ToString();
            string cantidadTexto = txt_entrada_de_los_productos.Text.Trim();


            if (string.IsNullOrEmpty(nombreProducto) || !int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de entrada.");
                return;
            }

            int idProducto = ObtenerIdProducto(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado en la base de datos.");
                return;
            }

            string cadenaConexion = "server=localhost;user id=root;password=;database=Basededatosdelproyecto;";
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();

                string insertQuery = "INSERT INTO EntradaInventario (id_producto, cantidad) VALUES (@id_producto, @cantidad)";
                using (MySqlCommand insertComando = new MySqlCommand(insertQuery, conexion))
                {
                    insertComando.Parameters.AddWithValue("@id_producto", idProducto);
                    insertComando.Parameters.AddWithValue("@cantidad", cantidad);
                    insertComando.ExecuteNonQuery();
                }

                string updateQuery = "UPDATE Producto SET stock_actual = stock_actual + @cantidad WHERE id_producto = @id_producto";
                using (MySqlCommand updateComando = new MySqlCommand(updateQuery, conexion))
                {
                    updateComando.Parameters.AddWithValue("@cantidad", cantidad);
                    updateComando.Parameters.AddWithValue("@id_producto", idProducto);
                    updateComando.ExecuteNonQuery();
                }

                MessageBox.Show("Entrada registrada correctamente.");
                txt_entrada_de_los_productos.Clear();
            }

            CargarProductos();
        }



        private void txt_de_la_salida_de_los_productos_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_4(object sender, EventArgs e)
        {
            string nombreProducto = cmb_productos_nombre2.SelectedItem?.ToString();
            string cantidadTexto = txt_de_la_salida_de_los_productos.Text.Trim();

            if (string.IsNullOrEmpty(nombreProducto) || !int.TryParse(cantidadTexto, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Verifica el producto y la cantidad de salida.");
                return;
            }

            int idProducto = ObtenerIdProducto(nombreProducto);
            if (idProducto == -1)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            string cadenaConexion = "server=localhost;user id=root;password=;database=Basededatosdelproyecto;";
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();


                string stockQuery = "SELECT stock_actual FROM Producto WHERE id_producto = @id";
                using (MySqlCommand stockComando = new MySqlCommand(stockQuery, conexion))
                {
                    stockComando.Parameters.AddWithValue("@id", idProducto);
                    int stockActual = Convert.ToInt32(stockComando.ExecuteScalar());

                    if (stockActual < cantidad)
                    {
                        MessageBox.Show($"Stock insuficiente. Solo hay {stockActual} unidades.");
                        return;
                    }
                }


                string insertQuery = "INSERT INTO SalidaInventario (id_producto, cantidad) VALUES (@id_producto, @cantidad)";
                using (MySqlCommand insertComando = new MySqlCommand(insertQuery, conexion))
                {
                    insertComando.Parameters.AddWithValue("@id_producto", idProducto);
                    insertComando.Parameters.AddWithValue("@cantidad", cantidad);
                    insertComando.ExecuteNonQuery();
                }


                string updateQuery = "UPDATE Producto SET stock_actual = stock_actual - @cantidad WHERE id_producto = @id_producto";
                using (MySqlCommand updateComando = new MySqlCommand(updateQuery, conexion))
                {
                    updateComando.Parameters.AddWithValue("@cantidad", cantidad);
                    updateComando.Parameters.AddWithValue("@id_producto", idProducto);
                    updateComando.ExecuteNonQuery();
                }

                MessageBox.Show("Salida registrada correctamente.");
                txt_entrada_de_los_productos.Clear();

                CargarProductos();

            }
        }

        private int ObtenerIdProducto(string nombreProducto)
        {
            string cadenaConexion = "server=localhost;user id=root;password=;database=Basededatosdelproyecto;";
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "SELECT id_producto FROM Producto WHERE nombre = @nombre";
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombreProducto);
                    object resultado = comando.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
            }
        }

        private void txt_entrada_de_los_productos_TextChanged(object sender, EventArgs e)
        {


        }

        private void button7_Click(object sender, EventArgs e)
        {
            string cadenaConexion = "server=localhost;user id=root;password=;database=Basededatosdelproyecto;";
            string nombreSeleccionado = cmb_consultar_producto.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(nombreSeleccionado))
            {
                MessageBox.Show("Por favor selecciona un artículo del ComboBox.");
                return;
            }

            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    string consulta = @"
                    SELECT 
    p.id_producto AS id,
    p.nombre AS producto,
    p.precio_unitario AS precio,
    p.stock_actual AS cantidad,
    c.nombre_categoria AS categoria
FROM Producto p
JOIN Categoria c ON p.id_categoria = c.id_categoria
WHERE p.nombre = @nombre;

            ";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombreSeleccionado);

                        using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                        {
                            DataTable tabla = new DataTable();
                            adaptador.Fill(tabla);
                            dataGridView3.DataSource = tabla;

                            if (tabla.Rows.Count == 0)
                            {
                                MessageBox.Show("No se encontró ningún producto con ese nombre.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al consultar el producto: " + ex.Message);
                }
            }

        }

        private void button3_Click_5(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Cargar_combo_box_por_nombre2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void cmb_productos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}