using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Admin_Buisness
{
    
    internal class ClaseRopa
    {
        string nombre;
        int stock;
        string genero;
        string tipo;

        private string Nombre;

        public string MyPropertyNombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        private int Stock;

        public int MyPropertyStock
        {
            get { return Stock; }
            set { Stock = value; }
        }
        private string Genero;

        public string MyPropertyGenero
        {
            get { return Genero; }
            set { Genero = value; }
        }

        private string Tipo;

        public string MyPropertyTipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        
        public int AgregarStock(int cantidad = 0)
        {
            cantidad = cantidad + Stock;
            return cantidad;
        }
        public int RestarStock()
        {


            
            return 0;
        }
        public int InfomracionStock()
        {



            return 0;
        }


    }


}

