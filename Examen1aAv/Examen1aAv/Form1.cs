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
    public partial class Form1 : Form
    {
        private string[] ciudades;
        private Datos[] datos;
        private TextBox[,] textBoxes;
        private bool precip = false;

        ToolTip toolTip = new ToolTip();
        Timer parpadeoTimer = new Timer();

        public Form1()
        {
            InitializeComponent();

            this.Text = "KeepMordorCool";

            datos = new Datos[28];
            for (int i = 0; i < datos.Length; i++)
            {
                datos[i] = new Datos();
            }

            ciudades = new string[4] { "Barad-dûr", "Gorgoroth", "Udûn", "Lugburz" };

            parpadeoTimer.Interval = 300;
            parpadeoTimer.Tick += ParpadeoTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("T.Max.(ºC)");
            comboBox1.Items.Add("Precip l/m2");
            comboBox1.SelectedIndex = 0;

            panel1.Enabled = false;
            RellenaPanel();
            panel2.Visible = false;

            toolTip.SetToolTip(comboBox1, "Selecciona un tipo de dato");
            toolTip.SetToolTip(btnAplicar, "Guardar datos de " + comboBox1.SelectedItem.ToString());
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            GuardaInformacion();
        }

        private void Nueva(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            panel2.Visible = true;

            poblacionToolStripMenuItem.Enabled = true;
            nuevaToolStripMenuItem.Enabled = false;
        }

        private void Poblaciones(object sender, EventArgs e)
        {
            FrmSecundario form2 = new FrmSecundario(ciudades);
            DialogResult result = form2.ShowDialog();

            if (result == DialogResult.OK)
            {
                ciudades = form2.ciudades;
                RellenaPanel();
            }
        }

        private void Salir(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Realmente deseas salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void RellenaPanel()
        {
            panel2.Controls.Clear();

            int numCiudades = ciudades.Length;
            int numDias = 7;
            int cont = 0;

            textBoxes = new TextBox[numCiudades, numDias];

            int startX = 10;
            int startY = 10;
            int espacioX = 80;
            int espacioY = 40;

            for (int i = 0; i < numCiudades; i++)
            {
                Label lblCiudad = new Label();
                lblCiudad.Text = ciudades[i];
                lblCiudad.Location = new Point(startX, startY + (i * espacioY));
                lblCiudad.AutoSize = true;
                panel2.Controls.Add(lblCiudad);

                for (int j = 0; j < numDias; j++)
                {
                    TextBox txt = new TextBox();
                    txt.Size = new Size(60, 30);
                    txt.Location = new Point(startX + 120 + (j * espacioX), startY + (i * espacioY));

                    if (precip)
                    {
                        txt.Text = datos[cont].precip.ToString();
                        if (string.IsNullOrEmpty(txt.Text)) { txt.Text = "0"; }
                    }
                    else
                    {
                        txt.Text = datos[cont].tMax.ToString();
                        if (string.IsNullOrEmpty(txt.Text)) { txt.Text = "0"; }
                    }

                    txt.Name = $"txtBox{i}{j}";

                    txt.Enter += txtColorEnter;
                    txt.Leave += txtColorLeave;
                    txt.KeyDown += txtEnter;
                    panel2.Controls.Add(txt);

                    textBoxes[i, j] = txt;
                    cont++;
                }
            }
        }

        private void txtColorEnter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt != null)
            {
                txt.BackColor = Color.LightCyan;
            }
        }

        private void txtColorLeave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt != null)
            {
                txt.BackColor = Color.White;
            }
        }

        private void txtEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GuardaInformacion();
            }
        }

        private void GuardaInformacion()
        {
            int cont = 0;

            for (int i = 0; i < textBoxes.GetLength(0); i++)
            {
                for (int j = 0; j < textBoxes.GetLength(1); j++)
                {
                    TextBox txt = textBoxes[i, j];

                    if (precip)
                    {
                        if (string.IsNullOrEmpty(txt.Text))
                        {
                            datos[cont].precip = 0;
                        }
                        else
                        {
                            try
                            {
                                datos[cont].precip = double.Parse(txt.Text) > 0 ? double.Parse(txt.Text) : 0;

                            }
                            catch (FormatException)
                            {
                                datos[cont].precip = 0;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txt.Text))
                        {
                            datos[cont].tMax = 0;
                        }
                        else
                        {
                            try
                            {
                                datos[cont].tMax = double.Parse(txt.Text) > 0 ? double.Parse(txt.Text) : 0;
                            }
                            catch (FormatException)
                            {
                                datos[cont].tMax = 0;
                            }
                        }
                    }

                    if (datos[cont].tMax > 30 && datos[cont].precip == 0)
                    {
                        txt.Tag = true;
                    }
                    else
                    {
                        txt.Tag = false;
                    }
                    cont++;
                }
            }

            parpadeoTimer.Start();
        }

        private void ParpadeoTimer_Tick(object sender, EventArgs e)
        {
            foreach (TextBox txt in textBoxes)
            {
                if (txt.Tag != null && (bool)txt.Tag)
                {
                    if (txt.ForeColor == Color.Red)
                    {
                        txt.ForeColor = Color.Black;
                    }
                    else
                    {
                        txt.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            precip = !precip;
            parpadeoTimer.Stop();
            RellenaPanel();
        }
    }
}
