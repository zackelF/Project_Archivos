using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diccionario_de_Datos
{
    public class Entidad
    {
        
            private char[] nombre;
            private long dirAtributos;
            private long sigEntindad;
            private long dirDatos;
            private long dirEntidad;
            private List<Atributo> atributos;
            
            public Entidad()
            {
                nombre = new char[30];
                dirAtributos = -1;
                sigEntindad = -1;
                dirDatos = -1;
                dirEntidad = -1;
                atributos = new List<Atributo>();
            }


        //getes y setes de atributos

        public char[] Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
            }
        }

        public long DirAtributos
        {
            get
            {
                return dirAtributos;
            }
            set
            {
                dirAtributos = value;
            }
        }

        public long SigEntindad
        {
            get
            {
                return sigEntindad;
            }
            set
            {
                sigEntindad = value;
            }
        }

        public long DirDatos
        {
            get
            {
                return dirDatos;
            }
            set
            {
                dirDatos = value;
            }
        }

        public long DirEntidad
        {
            get
            {
                return dirEntidad;
            }
            set
            {
                dirEntidad = value;
            }
        }

        public List<Atributo> Atributos
        {
            get
            {
                return atributos;
            }
            set
            {
                atributos = value;
            }
        }

        public string quitar()
            {
                string res = "";
                for (int i = 0; i < nombre.Length; i++)
                {
                    if (nombre[i] != '\0')
                    {
                        res += nombre[i];
                    }
                }
                return res;

            }

        }
}
