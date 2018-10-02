using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diccionario_de_Datos
{
    public partial class Form2 : Form
    {
        public List<Entidad> Entidades = new List<Entidad>();
        public Archivo archivo = new Archivo();
        public bool mod = false;
        public bool identi = false;
        public Atributo indice;
        public bool bolll;
        public List<Atributo> Lat;
        public int trabajo = 0;
        public string nombrearchivo = "";

        public Form2(string n)
        {
            InitializeComponent();
            inicializaTabla();
            button3.Enabled = true;
            button1.Enabled = true;
            Modificar.Enabled = true;
            trabajo = 0;
            nombrearchivo = n;
        }

        private void Nuevo_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;

            button1.Enabled = true;
            button2.Enabled = true;
            mod = false;


            Nuevo.Enabled = false;
        }

        private void Modificar_Click(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString();
            textBox1.Enabled = true;
            textBox2.Enabled = true;

            Lat = new List<Atributo>(Entidades[comboBox1.SelectedIndex].Atributos);
            indice = new Atributo();
            bolll = false;
            for (int hol = 0; hol < Entidades[comboBox1.SelectedIndex].Atributos.Count; hol++)
            {
                if (textBox1.Text == Entidades[comboBox1.SelectedIndex].Atributos[hol].damenombre())
                {
                    indice = Entidades[comboBox1.SelectedIndex].Atributos[hol];
                    bolll = true;
                }
            }
            if (bolll == true)
            {
                Lat.Remove(indice);
            }


            Nuevo.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;

            mod = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex != 0 && dataGridView1.CurrentCell.RowIndex != Entidades[comboBox1.SelectedIndex].Atributos.Count - 1)
            {
                Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex - 1].DirSig = Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex + 1].DirAtributo;
                archivo.Modifica_Atributo(Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex - 1].DirAtributo, Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex - 1], trabajo);
                archivo.Modifica_Atributo(Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex + 1].DirAtributo, Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex + 1], trabajo);
            }
            else
            {
                if (dataGridView1.CurrentCell.RowIndex != 0 && dataGridView1.CurrentCell.RowIndex == Entidades[comboBox1.SelectedIndex].Atributos.Count - 1)
                {
                    Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex - 1].DirSig = -1;
                    archivo.Modifica_Atributo(Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex - 1].DirAtributo, Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex - 1], trabajo);
                }
                else
                {
                    if (Entidades[comboBox1.SelectedIndex].Atributos.Count != 1)
                    {
                        archivo.modificaApuntadorDeAtributos(Entidades[comboBox1.SelectedIndex].DirEntidad, Entidades[comboBox1.SelectedIndex], Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex + 1].DirAtributo);
                    }
                    else
                    {
                        archivo.modificaApuntadorDeAtributos(Entidades[comboBox1.SelectedIndex].DirEntidad, Entidades[comboBox1.SelectedIndex], -1);
                    }
                }
            }


            Entidades[comboBox1.SelectedIndex].Atributos.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            actualizaTabla();
        }

        public void actualizaTabla()
        {
            dataGridView1.Rows.Clear();

            if (Entidades[comboBox1.SelectedIndex].Atributos.Count != 0)
                dataGridView1.RowCount = Entidades[comboBox1.SelectedIndex].Atributos.Count;

            for (int i = 0; i < Entidades[comboBox1.SelectedIndex].Atributos.Count; i++)
            {
                string aux = new string(Entidades[comboBox1.SelectedIndex].Atributos[i].Nombre);
                dataGridView1.Rows[i].Cells[0].Value = aux;
                dataGridView1.Rows[i].Cells[1].Value = Entidades[comboBox1.SelectedIndex].Atributos[i].Tipo;
                dataGridView1.Rows[i].Cells[2].Value = Entidades[comboBox1.SelectedIndex].Atributos[i].Tam;
                dataGridView1.Rows[i].Cells[3].Value = Entidades[comboBox1.SelectedIndex].Atributos[i].DirAtributo;
                if (Entidades[comboBox1.SelectedIndex].Atributos[i].TipoIndice == false)
                {
                    dataGridView1.Rows[i].Cells[4].Value = "F";
                }
                else
                {
                    dataGridView1.Rows[i].Cells[4].Value = "T";
                }
                

                dataGridView1.Rows[i].Cells[5].Value = Entidades[comboBox1.SelectedIndex].Atributos[i].DirSig;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            identi = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox2.ReadOnly = true;

            button1.Enabled = false;
            button2.Enabled = false;
            Nuevo.Enabled = true;
            button3.Enabled = true;
            Modificar.Enabled = true;



            if (comboBox1.Text == "")
            {
                MessageBox.Show("Selecciona una entidad");
                return;
            }

            if (textBox1.Text == "" || textBox2.Text == "" || comboBox2.Text == "" || comboBox3.Text == "")
            { 
                textBox1.Enabled = true;
                return;
            }


            if (comboBox3.SelectedIndex == 1)      // Revisar si algun atributo ya se selecciono como clave primaria
            {
                for (int i = 0; i < Entidades[comboBox1.SelectedIndex].Atributos.Count; i++)
                {
                    if (Entidades[comboBox1.SelectedIndex].Atributos[i].TipoIndice == true && new string(Entidades[comboBox1.SelectedIndex].Atributos[i].Nombre) != new string(charAstring(textBox1.Text)))
                    {
                        MessageBox.Show("Ya has seleccionado un atributo como clave primaria");
                        return;
                    }
                }
                
            }

            string p1 = new string(charAstring(textBox1.Text));
            for (int i = 0; i < Entidades[comboBox1.SelectedIndex].Atributos.Count; i++)
            {
                if (mod == false)
                {
                    string p = new string(Entidades[comboBox1.SelectedIndex].Atributos[i].Nombre);
                    if (p1 == p)
                    {
                        identi = true;
                    }
                }
            }
            if (mod == true)
            {
                for (int i = 0; i < Lat.Count; i++)
                {

                    string p = new string(Lat[i].Nombre);
                    if (p1 == p)
                    {
                        identi = true;
                    }

                }
            }



            if (identi != true)
            {
                if (mod == false)
                {

                    long cabezera = archivo.Dame_cabecera();
                    Atributo nuevo = new Atributo();
                    nuevo.Nombre = charAstring(textBox1.Text);
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            nuevo.Tipo = 'I';
                            break;
                        case 1:
                            nuevo.Tipo = 'C';
                            break;
                    }

                    nuevo.Tam = Int16.Parse(textBox2.Text);

                    nuevo.DirAtributo = archivo.Tam_archivo();

                    if (comboBox3.SelectedIndex == 0)
                    {
                        nuevo.TipoIndice = false;
                    }
                    if (comboBox3.SelectedIndex == 1)
                    {
                        nuevo.TipoIndice = true;
                    }
                    


                    nuevo.DirSig = -1;





                    if (Entidades[comboBox1.SelectedIndex].DirAtributos == -1)
                    {
                        archivo.modificaApuntadorDeAtributos(Entidades[comboBox1.SelectedIndex].DirEntidad, Entidades[comboBox1.SelectedIndex], nuevo.DirAtributo);
                        Entidades[comboBox1.SelectedIndex].Atributos.Add(nuevo);
                        archivo.insertaAtributo(nuevo, trabajo);
                        Entidades[comboBox1.SelectedIndex].DirAtributos = nuevo.DirAtributo;
                    }
                    else {
                        Entidades[comboBox1.SelectedIndex].Atributos.Add(nuevo);
                        actualizaIndices();
                    }


                    textBox1.Text = "";
                }
                else
                {
                    Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].Nombre = charAstring(textBox1.Text);
                    Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].Tam = Int16.Parse(textBox2.Text);

                    if (comboBox3.SelectedIndex == 0)
                        Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].TipoIndice = false;
                    else
                        Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].TipoIndice = true;

                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].Tipo = 'I';
                            break;
                        case 1:
                            Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].Tipo = 'C';
                            break;
                    }

                    archivo.Modifica_Atributo(Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex].DirAtributo, Entidades[comboBox1.SelectedIndex].Atributos[dataGridView1.CurrentCell.RowIndex], trabajo);

                    textBox1.Text = "";
                    mod = false;
                }
            }
            else
            {
                MessageBox.Show("El atributo ya existe");
            }

            actualizaTabla();
        
    }

        private char[] charAstring(String cadena)
        {
            char[] aux = new char[30];
            for (int i = 0; i < cadena.Count(); i++)
                aux[i] = cadena[i];

            return aux;
        }

        private void actualizaIndices()
        {
            for (int i = 0; i < Entidades[comboBox1.SelectedIndex].Atributos.Count - 1; i++)
            {
                Entidades[comboBox1.SelectedIndex].Atributos[i].DirSig = Entidades[comboBox1.SelectedIndex].Atributos[i + 1].DirAtributo;
            }
            for (int i = 0; i < Entidades[comboBox1.SelectedIndex].Atributos.Count; i++)
            {
                archivo.Modifica_Atributo(Entidades[comboBox1.SelectedIndex].Atributos[i].DirAtributo, Entidades[comboBox1.SelectedIndex].Atributos[i], trabajo);
            }
            archivo.modificaApuntadorDeAtributos(Entidades[comboBox1.SelectedIndex].DirEntidad, Entidades[comboBox1.SelectedIndex], Entidades[comboBox1.SelectedIndex].Atributos[0].DirAtributo);

        }

        public void inicializaTabla()
        {
            dataGridView1.ColumnCount = 7;
            dataGridView1.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle =
            new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Aqua;
            columnHeaderStyle.Font =
            new Font("Verdana", 10, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle =
                columnHeaderStyle;


            dataGridView1.Columns[0].Name = "Nombre";
            dataGridView1.Columns[1].Name = "Tipo";
            dataGridView1.Columns[2].Name = "Longitud";
            dataGridView1.Columns[3].Name = "Dir. del Atributo";
            dataGridView1.Columns[4].Name = "Cve Primaria";
            dataGridView1.Columns[5].Name = "Sig Atributo";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizaTabla();
            button3.Enabled = true;
            button1.Enabled = true;
            Modificar.Enabled = true;

            if (Entidades[comboBox1.SelectedIndex].DirDatos != -1)
            {
                button3.Enabled = false;
                button1.Enabled = false;
                Modificar.Enabled = false;
                Nuevo.Enabled = false;
            }
        }

        public void InicializaEntidades()
        {
            for (int i = 0; i < Entidades.Count; i++)
            {
                string aux = new string(Entidades[i].Nombre);
                comboBox1.Items.Add(aux);
            }
        }

        public void inicializaComponentes()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            Modificar.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    textBox2.Text = sizeof(int).ToString();
                    break;
                case 1:
                    textBox2.Text = sizeof(char).ToString();
                    textBox2.Text = "30";
                    textBox2.ReadOnly = false;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Nuevo.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;

            button1.Enabled = false;
            button2.Enabled = false;

            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 nm = new Diccionario_de_Datos.Form1(nombrearchivo);
            nm.Show();
            nm.volver();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            actualizaTabla();
        }
    }
}
