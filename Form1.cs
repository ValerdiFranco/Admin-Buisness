using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin_Buisness
{

    public partial class Pantalla_Principal : Form
    {
        public Pantalla_Principal()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();
        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {

        }
    }
}
