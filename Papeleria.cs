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
    public partial class Papeleria : Form
    {
        public Papeleria()
        {
            InitializeComponent();
        }

        private void BtnVolver_Seleccion_Pp_Click(object sender, EventArgs e)
        {
            Menu_de_seleccion menu = new Menu_de_seleccion();
            menu.Show();
            this.Hide();
        }
    }
}
