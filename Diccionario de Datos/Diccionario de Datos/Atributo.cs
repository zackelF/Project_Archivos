using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diccionario_de_Datos
{
    public class Atributo
    {

        private char[] nombre = new Char[30];
        private char tipo = 'I';
        private long tam = 0;
        private long dirAtributo = -1;
        private bool tipoIndice = false;
        private long dirSig = -1;
        


        //getes y setes de los atributos

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

        public char Tipo
        {
            get
            {
                return tipo;
            }
            set
            {
                tipo = value;
            }
        }

        public long DirAtributo
        {
            get
            {
                return dirAtributo;
            }
            set
            {
                dirAtributo = value;
            }
        }

        public long Tam
        {
            get
            {
                return tam;
            }
            set
            {
                tam = value;
            }
        }

        public bool  TipoIndice
        {
            get
            {
                return tipoIndice;
            }
            set
            {
                tipoIndice = value;
            }
        }

        public long DirSig
        {
            get
            {
                return dirSig;
            }
            set
            {
                dirSig = value;
            }
        }


        public string damenombre()
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
