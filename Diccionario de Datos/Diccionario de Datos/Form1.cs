using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Diccionario_de_Datos
{
    public partial class Form1 : Form
    {
        string rutaoll;
        private int trabajo=0;
        private List<Entidad> Entidades = new List<Entidad>();
        private bool mod = false;
        public bool identi = false;
        public string rutaAbsoluta = "";
        Archivo archivo = new Archivo();
        public string ruta = "";
        public string rutalter = "";
        public string nombrearchivo = "";

        public Form1()
        {
            InitializeComponent();
            inicializaTabla();
            Atr.Enabled = false;
            textBoxEnt.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button7.Enabled = false;
            button2.Enabled = false;
        }
        public Form1(string cad)
        {
            InitializeComponent();
            inicializaTabla();
            Atr.Enabled = false;
            textBoxArch.Text = cad;
            textBoxEnt.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button7.Enabled = false;
        }

        public void volver()
        {
            object s = new object();
            EventArgs e = new EventArgs();
            Abrir_Click(s, e);
            checarentidades();
        }

        private void Abrir_Click(object sender, EventArgs e)
        {
            if (textBoxArch.Text != "")
            {
                nombrearchivo = textBoxArch.Text;
                rutaoll= Directory.GetCurrentDirectory();
                rutaAbsoluta =rutaoll+"\\" + textBoxArch.Text;
                string comprobar = rutaoll+"\\" + textBoxArch.Text + @"\diccionario.bin";
                rutalter =rutaoll+"\\" + textBoxArch.Text;
                if (File.Exists(comprobar))
                {
                    ruta = rutaoll+"\\" + textBoxArch.Text + @"\Diccionario.bin";
                    archivo = new Archivo(textBoxArch.Text + ".bin");
                    archivo.Peto = trabajo;
                    Entidades = archivo.LeeArchivo(ruta, rutaAbsoluta);
                    ActualizaTabla();
                    label3.Text = textBoxArch.Text;
                    this.Text = "Archivo en uso: " + textBoxArch.Text + ".bin";
                    MessageBox.Show("Archivo: " + textBoxArch.Text + ".bin abierto");
                }
                else
                {
                    Directory.CreateDirectory(textBoxArch.Text);
                    archivo.Path = "nuevo.bin";
                    Entidades.Clear();
                    Archivo nuevo = new Archivo();

                    archivo = nuevo;
                    string ruta2 = rutaoll+"\\" + textBoxArch.Text + @"\";
                    ruta = rutaoll+"\\" + textBoxArch.Text + @"\Diccionario.bin";
                    string nombre = "Diccionario";
                    rutaAbsoluta = rutaoll+"\\" + textBoxArch.Text;

                    archivo.creaArchivo(nombre, ruta2, 0, 0);
                    ActualizaTabla();
                    label3.Text = textBoxArch.Text;
                    this.Text = "Archivo en uso: " + archivo.Path;
                    MessageBox.Show("Archivo creado:    " + textBoxArch.Text + ".bin");

                }
                activartodo();

            }
            else
            {
                MessageBox.Show("Error 404: FILE NOT FOUND ");
            }
            checarentidades();
        }

        private void inicializaTabla()
        {
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle =
            new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font =
                new Font("Arial", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle =
                columnHeaderStyle;


            dataGridView1.Columns[0].Name = "Nombre";
            dataGridView1.Columns[1].Name = "Dir. Entidad";
            dataGridView1.Columns[2].Name = "Dir. Datos";
            dataGridView1.Columns[3].Name = "Dir. Atributos";
            dataGridView1.Columns[4].Name = "Dir. Sig. Entidad";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool identi = false;

            if (textBoxEnt.Text == "")
            {
                MessageBox.Show("Nombre de Entidad no valido");
                return;
            }


            for (int i = 0; i < Entidades.Count; i++)
            {
                var aux = new string(charAstring(textBoxEnt.Text));
                string aux2 = new string(Entidades[i].Nombre);
                if (aux2 == aux)
                {
                    identi = true;
                    MessageBox.Show("La entidad ya existe ");

                }
            }

            if (mod == false && identi != true)
            {
                long cabecera = archivo.Dame_cabecera();
                Entidad nuevo = new Entidad();
                nuevo.Nombre = charAstring(textBoxEnt.Text);
                nuevo.DirAtributos = -1;
                nuevo.DirEntidad = archivo.Tam_archivo();//modificado//sig
                nuevo.DirDatos = -1;
                nuevo.SigEntindad = -1;


                if (cabecera == -1)
                {
                    archivo.modificacabecera(nuevo.DirEntidad);
                    Entidades.Add(nuevo);
                    archivo.insertaEntidad(nuevo);
                }
                else
                {
                    Entidades.Add(nuevo);
                    ordena();
                    actualizaIndices();
                }

                textBoxArch.Text = "";
                MessageBox.Show("La entidad se agregado en el Archivo");
                archivo.CreaArchivoEntidad(textBoxEnt.Text, rutaAbsoluta);
            }

            else
            {
                if (identi != true)
                {
                    string path = rutaAbsoluta + @"\" + Entidades[dataGridView1.CurrentCell.RowIndex].quitar() + ".bin";
                    File.Delete(path);
                    Entidades[dataGridView1.CurrentCell.RowIndex].Nombre = charAstring(textBoxEnt.Text);
                    archivo.Modifica_entidad(Entidades[dataGridView1.CurrentCell.RowIndex].DirEntidad, Entidades[dataGridView1.CurrentCell.RowIndex]);
                    textBoxArch.Text = "";
                    archivo.CreaArchivoEntidad(Entidades[dataGridView1.CurrentCell.RowIndex].quitar(), rutaAbsoluta);
                    mod = false;
                }

            }

            ActualizaTabla();
            checarentidades();

            // MessageBox.Show("La entidad ya existe en el archivo");
        }

        private void ordena()
        {
            List<String> auxString = new List<String>();
            List<Entidad> auxEntidad = new List<Entidad>();
            foreach (Entidad ent in Entidades) auxString.Add(new String(ent.Nombre));
            auxString.Sort();
            //Ordena Alfabeticamenre
            foreach (String s in auxString)
            {
                foreach (Entidad e in Entidades)
                {
                    if (String.Compare(s, new string(e.Nombre)) == 0) { auxEntidad.Add(e); }
                }
            }
            Entidades.Clear();
            Entidades = auxEntidad;

        }

        private void actualizaIndices()
        {
            for (int i = 0; i < Entidades.Count - 1; i++)
            {
                Entidades[i].SigEntindad = Entidades[i + 1].DirEntidad;
            }
            for (int i = 0; i < Entidades.Count; i++)
            {
                archivo.Modifica_entidad(Entidades[i].DirEntidad, Entidades[i]);
            }
            archivo.modificacabecera(Entidades[0].DirEntidad);

        }

        private void ActualizaTabla()
        {
            dataGridView1.Rows.Clear();

            if (Entidades.Count != 0)
                dataGridView1.RowCount = Entidades.Count;

            for (int i = 0; i < Entidades.Count; i++)
            {
                string aux = new string(Entidades[i].Nombre);
                dataGridView1.Rows[i].Cells[0].Value = aux;
                dataGridView1.Rows[i].Cells[1].Value = Entidades[i].DirEntidad;
                dataGridView1.Rows[i].Cells[2].Value = Entidades[i].DirDatos;
                dataGridView1.Rows[i].Cells[3].Value = Entidades[i].DirAtributos;
                dataGridView1.Rows[i].Cells[4].Value = Entidades[i].SigEntindad;
            }
        }

        private char[] charAstring(String cadena)
        {
            char[] aux = new char[30];
            for (int i = 0; i < cadena.Count(); i++)
                aux[i] = cadena[i];

            return aux;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxEnt.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            textBoxEnt.Enabled = true;
            mod = true;
            checarentidades();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex != 0 && dataGridView1.CurrentCell.RowIndex != Entidades.Count - 1)
            {
                Entidades[dataGridView1.CurrentCell.RowIndex - 1].SigEntindad = Entidades[dataGridView1.CurrentCell.RowIndex + 1].DirEntidad;
                archivo.Modifica_entidad(Entidades[dataGridView1.CurrentCell.RowIndex - 1].DirEntidad, Entidades[dataGridView1.CurrentCell.RowIndex - 1]);
                archivo.Modifica_entidad(Entidades[dataGridView1.CurrentCell.RowIndex + 1].DirEntidad, Entidades[dataGridView1.CurrentCell.RowIndex + 1]);
            }
            else
            {
                if (dataGridView1.CurrentCell.RowIndex != 0 && dataGridView1.CurrentCell.RowIndex == Entidades.Count - 1)
                {
                    Entidades[dataGridView1.CurrentCell.RowIndex - 1].SigEntindad = -1;
                    archivo.Modifica_entidad(Entidades[dataGridView1.CurrentCell.RowIndex - 1].DirEntidad, Entidades[dataGridView1.CurrentCell.RowIndex - 1]);
                }
                else
                {
                    if (Entidades.Count != 1)
                        archivo.modificacabecera(Entidades[dataGridView1.CurrentCell.RowIndex + 1].DirEntidad);
                }
            }

            string pen = rutaAbsoluta + @"\" + Entidades[dataGridView1.CurrentCell.RowIndex].quitar() + ".bin";
            File.Delete(pen);
            Entidades.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            ActualizaTabla();
            checarentidades();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Entidades.Clear();
            archivo.Peto = trabajo;
            Entidades = archivo.LeeArchivo(ruta, rutaAbsoluta);
            dataGridView1.Rows.Clear();
            ActualizaTabla();
        }

        private void Atr_Click(object sender, EventArgs e)
        {
            ActualizaTabla();
            if (Entidades.Count > 0)
            {
                Form2 atributos = new Form2(nombrearchivo);
                atributos.Entidades = Entidades;
                atributos.archivo = archivo;
                atributos.InicializaEntidades();
                atributos.inicializaTabla();
                atributos.inicializaComponentes();
                atributos.Show();

                Entidades = atributos.Entidades;
                archivo = atributos.archivo;

                actualizaIndices();
                ActualizaTabla();
                this.Visible = false;
            }
            else
                MessageBox.Show("No hay entidades insertadas");
        }

        private void checarentidades()
        {
            if(Entidades.Count!=0 && Entidades != null)
            {
                Atr.Enabled = true;
            }
            else
            {
                Atr.Enabled = false;
            }

        }

        private void activartodo()
        {
            textBoxEnt.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button7.Enabled = true;
            button2.Enabled = true;
        }

        
    }
}
