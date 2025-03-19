using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen1aAv
{
    public partial class FrmSecundario : Form
    {
        public string[] ciudades { get; set; }

        public FrmSecundario(string[] ciudades)
        {
            InitializeComponent();

            this.ciudades = ciudades;
            this.Text = "Ciudades";
        }

        private void FrmSecundario_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (string item in ciudades)
            {
                listBox1.Items.Add(item);
            }
        }

        private void Añadir_Btn(object sender, EventArgs e)
        {
            AñadeTexto();
        }

        private void Eliminar_Btn(object sender, EventArgs e)
        {
            EliminaItems();
        }

        private void Aceptar_Btn(object sender, EventArgs e)
        {
            ActualizaCiudades();
            DialogResult = DialogResult.OK;
        }

        private void AñadeTexto()
        {
            string texto = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(texto))
            {
                listBox1.Items.Add(texto);
            }

            CompruebaItems();
        }

        private void EliminaItems()
        {
            List<string> selected = new List<string>();

            foreach (var item in listBox1.SelectedItems)
            {
                selected.Add(item.ToString());
            }

            foreach (var item in selected)
            {
                listBox1.Items.Remove(item);
            }

            CompruebaItems();
        }

        private void CompruebaItems()
        {
            if (listBox1.Items.Count == 4)
            {
                btnAceptar.Enabled = true;
            }
            else
            {
                btnAceptar.Enabled = false;
            }
        }

        private void ActualizaCiudades()
        {
            if (listBox1.Items.Count == 4)
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    ciudades[i] = listBox1.Items[i].ToString();
                }
            }
        }
    }
}
