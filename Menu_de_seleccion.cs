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
    public partial class Menu_de_seleccion : Form
    {
        public Menu_de_seleccion()
        {
            InitializeComponent();
        }

        private void BtnVolver_Inicio_Click(object sender, EventArgs e)
        {
            Pantalla_Principal Volver = new Pantalla_Principal();
            Volver.Show();
            this.Hide();
        }

        private void BtnEntrar_Pasteleria_Click(object sender, EventArgs e)
        {
            Pasteleria EntrarPasteleria = new Pasteleria();
            EntrarPasteleria.Show();
            this.Hide();
        }

        private void Btn_Entrar_Papeleria_Click(object sender, EventArgs e)
        {
            Negocio_papeleria EntrarPapeleria = new Negocio_papeleria();
            EntrarPapeleria.Show();
            this.Hide();
        }

        private void BtnEntrar_Miselanea_Click(object sender, EventArgs e)
        {
            Miselanea EntrarMiselanea = new Miselanea();
            EntrarMiselanea.Show();
            this.Hide();
        }

        private void BtnEntrar_Ropa_Click(object sender, EventArgs e)
        {
            Ropa EntrarRopa= new Ropa();
            EntrarRopa.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
