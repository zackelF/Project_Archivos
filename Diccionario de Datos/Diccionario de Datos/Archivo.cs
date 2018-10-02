using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Diccionario_de_Datos
{
    public class Archivo
    {

        private int peto;
        private BinaryReader reader;
        private BinaryWriter writer;
        private FileStream stream;
        private BinaryReader reader2;
        private BinaryWriter writer2;
        private FileStream stream2;
        private String path;
        private List<Entidad> E;

        //geters y seters

        public int Peto
        {
            get { return peto; }
            set { peto = value;}
        }

        public String Path
        {
            get { return path;}
            set { path = value;}
        }

        public List<Entidad> EN
        {
            get { return E; }
            set { E = value; }
           
        }
        //constructores de la clase arhivo

        public Archivo(string nom)
        {
            path = nom;
            E = new List<Entidad>();
        }

        public Archivo()
        {
            E = new List<Entidad>();
        }

        //metodos para obtener cabecera y tam archivo

        public long Dame_cabecera()
        {
            long cab;
            stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            cab = reader.ReadInt64();
            reader.Close();
            reader.Dispose();
            stream.Close();
            stream.Dispose();
            return cab;
        }

        public long Tam_archivo()
        {
            long tam;
            stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            tam = stream.Length;
            stream.Close();
            stream.Dispose();
            return tam;

        }

        public void modificacabecera(long nueva)
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer = new BinaryWriter(stream);
            stream.Seek(0, SeekOrigin.Begin);
            writer.Write(nueva);
            stream.Close();
            stream.Dispose();
            writer.Close();
            writer.Dispose();
        }

        //metodos de entidades

        public void insertaEntidad(Entidad nueva)
        {
            long pos = Dame_cabecera();
            long tam_archivo = Tam_archivo();
            stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer = new BinaryWriter(stream);
            if (pos == -1) stream.Seek(pos, SeekOrigin.Begin);
            else stream.Seek(tam_archivo, SeekOrigin.Begin);


            writer.Write(nueva.Nombre);
            writer.Write(nueva.DirAtributos);
            writer.Write(nueva.DirEntidad);
            writer.Write(nueva.DirDatos);
            writer.Write(nueva.SigEntindad);
            writer.Close();
            writer.Dispose();
            stream.Close();
            stream.Dispose();
        }

        public void CreaArchivoEntidad(string nom, string ruta)
        {

            string patas = ruta + @"\" + nom + ".bin";

            stream = new FileStream(patas, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            stream.Close();
        }

        public void Modifica_entidad(long direccion, Entidad ent)
        {

            stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            writer = new BinaryWriter(stream);
            stream.Seek(direccion, SeekOrigin.Begin);
            writer.Write(ent.Nombre);
            writer.Write(ent.DirAtributos);
            writer.Write(ent.DirEntidad);
            writer.Write(ent.DirDatos);
            writer.Write(ent.SigEntindad);
            writer.Close();
            writer.Dispose();
            stream.Close();
            stream.Dispose();
        }

        //metodo de abrir y crear archivos

        public void creaArchivo(string nom, string ruta, int p, long valor)
        {
            long cabecera = -1;
            if (p == 0 || p == 3)
            {
                path = ruta + nom + ".bin";
                stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream);
                writer.Write(cabecera);
                writer.Close();
                stream.Close();
            }

        }

        public List<Entidad> LeeArchivo(String nom, String ruta)//modificado para archivos separados 
        {
            path = nom;
            long final = 0;
            long c = 0;
            long rango = 0;
            long final2 = 0;
            long final3 = 0;
            long c2;
            long pos;
            c = Dame_cabecera();
            stream = new FileStream(nom, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(stream);
            //c = Dame_cabecera();
            if (c != -1)
            {
                stream.Position = c;
            }
            else
            {
                stream.Position = stream.Length;
            }
            final = c;
            stream.Seek(8, SeekOrigin.Begin);
            
            while (true && final != -1 && c != -1)
            {
                stream.Seek(final, SeekOrigin.Begin);
                Entidad p = new Entidad();
                p.Nombre = reader.ReadChars(30);
                p.DirAtributos = reader.ReadInt64();
                p.DirEntidad = reader.ReadInt64();
                p.DirDatos = reader.ReadInt64();
                p.SigEntindad = reader.ReadInt64();
                final2 = p.DirAtributos;
                final3 = p.DirDatos;

                while (true && final2 != -1 && c != -1)
                {

                    stream.Seek(final2, SeekOrigin.Begin);
                    Atributo t = new Atributo();
                    t.Nombre = reader.ReadChars(30);
                    t.Tipo = reader.ReadChar();
                    t.Tam = reader.ReadInt64();
                    t.DirAtributo = reader.ReadInt64();
                    t.TipoIndice = reader.ReadBoolean();
                    t.DirSig = reader.ReadInt64();
                    final2 = t.DirSig;
                    p.Atributos.Add(t);
                }


                final = p.SigEntindad;
                E.Add(p);
                
            }
            reader.Close();
            stream.Close();
            return E;
        }

        //metodos de atributos

        public void Modifica_Atributo(long direccion, Atributo atributo, int trabajo)
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            writer = new BinaryWriter(stream);
            stream.Seek(direccion, SeekOrigin.Begin);
            writer.Write(atributo.Nombre);
            writer.Write(atributo.Tipo);
            writer.Write(atributo.Tam);
            writer.Write(atributo.DirAtributo);
            writer.Write(atributo.TipoIndice);     
            writer.Write(atributo.DirSig);
            writer.Close();
            writer.Dispose();
            stream.Close();
            stream.Dispose();
        }

        public void modificaApuntadorDeAtributos(long dir, Entidad ent, long atributos)
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            writer = new BinaryWriter(stream);
            stream.Seek(dir, SeekOrigin.Begin);
            writer.Write(ent.Nombre);
            writer.Write(atributos);
            writer.Write(ent.DirEntidad);
            writer.Write(ent.DirDatos);
            writer.Write(ent.SigEntindad);
            writer.Close();
            writer.Dispose();
            stream.Close();
            stream.Dispose();
        }

        public void insertaAtributo(Atributo atributo, int trabajo)
        {
            long pos = Dame_cabecera();
            long tam_archivo = Tam_archivo();
            stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            writer = new BinaryWriter(stream);
            if (pos == -1) stream.Seek(pos, SeekOrigin.Begin);
            else stream.Seek(tam_archivo, SeekOrigin.Begin);


            writer.Write(atributo.Nombre);
            writer.Write(atributo.Tipo);
            writer.Write(atributo.Tam);
            writer.Write(atributo.DirAtributo);
            writer.Write(atributo.TipoIndice);
            writer.Write(atributo.DirSig);
            writer.Close();
            writer.Dispose();
            stream.Close();
            stream.Dispose();
        }

    }



    
}
