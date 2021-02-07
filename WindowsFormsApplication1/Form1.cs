using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication1

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string bestand;
        public string path, temp, mid, file, text;
        bool nonNumberEntered;
        public string[] myfiles = new string[5];
        public string BronDirectory { get; set; }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Enabled == true)
            {
                Application.Exit();
            }
            else
            {
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                btnBewaar.Visible = false;
                btnTerug.Visible = false;
                textBox1.Enabled = true;
                radioButton5.Checked = true;
                pictureBox1.Enabled = true;
                label1.Text = "Geef de naam van het te maken bestand of klik op een van de keuzeknoppen";
                label2.Text = "(Zonder extensie wordt het een txt bestand)";
                ActiveControl = textBox1;
                textBox1.SelectionStart = textBox1.Text.Length;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();//centreren form
            label1.Text = "Geef de naam van het te maken bestand of klik op een van de keuzeknoppen";
            LoadStreamReader();
            label3.Text = temp;
            label2.ForeColor = Color.Black;
            label2.Text = "(Zonder extensie wordt het een txt bestand)";
            ActiveControl = textBox1;
            btnMaak.Enabled = false;
            radioButton1.Text = myfiles[1];
            radioButton2.Text = myfiles[2];
            radioButton3.Text = myfiles[3];
            radioButton4.Text = myfiles[4];
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            btnBewaar.Visible = false;
            btnTerug.Visible = false;
        }

        private void btnMaak_Click(object sender, EventArgs e)
        {
            Bestand();
        }

        private void Bestand()
        {
            btnMaak.Enabled = true;

            if (checkBox2.Checked == true)
            {
                if (textBox1.Text != "")
                {
                    midd();

                    if (mid != ".")
                    {
                        textBox1.Text = textBox1.Text + ".txt";
                    }

                    FolderBrowserDialog ofd = new FolderBrowserDialog();
                    ofd.Description = "Selecteer de juiste map voor het bestand:" +
                    " \r(" + textBox1.Text + ")";
                    DialogResult result = ofd.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        path = ofd.SelectedPath + @"\" + textBox1.Text;
                        maakbestand();
                    }
                    else
                    {
                        ActiveControl = textBox1;
                        textBox1.SelectionStart = textBox1.Text.Length;
                    }
                }
                else
                {
                    btnMaak.Enabled = false;
                    label2.Text = "Er moet een bestandsnaam ingegeven worden...";
                    label2.ForeColor = Color.Red;
                }
            }

            if (checkBox1.Checked == true)
            {
                if (textBox1.Text != "")
                {
                    midd();
                    if (mid != ".")
                    {
                        textBox1.Text = textBox1.Text + ".txt";
                    }

                    LoadStreamReader();
                    path = temp + @"\" + textBox1.Text;
                    maakbestand();
                }
                else
                {
                    label2.Text = "Er moet een bestandsnaam ingegeven worden...";
                    label2.ForeColor = Color.Red;
                }
            }
        }

        //Zorgt er voor dat er geen ongeldige tekens ingevoerd kunnen worden
        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            nonNumberEntered = false;
            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Oem5 || e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.Return)
            {
                nonNumberEntered = true;

                string sh = "";
                //Als de Shift is ingedrukt
                if (Control.ModifierKeys == Keys.Shift)
                {
                    sh = Control.ModifierKeys.ToString();
                }

                if (e.KeyCode == Keys.Return)
                {
                    Bestand();//Bij enter start bestand maken
                }
                else
                {
                    label2.ForeColor = Color.Red;
                    if (e.KeyCode == Keys.Oemcomma) { label2.Text = @"De - Komma - mag niet worden gebruikt..."; }
                    if (e.KeyCode == Keys.Oem5) { label2.Text = @"De - Backslash - mag niet worden gebruikt..."; }

                    if (e.KeyCode == Keys.Oem5 && sh == "Shift") { label2.Text = @"De - Verticale Streep - mag niet worden gebruikt..."; }
                    if (e.KeyCode == Keys.OemQuestion) { label2.Text = @"De - Slash - mag niet worden gebruikt..."; }
                    if (e.KeyCode == Keys.OemQuestion && sh == "Shift") { label2.Text = @"Het - Vraagteken - mag niet worden gebruikt..."; }
                }
            }
            else
            {
                if (label2.Text != "")
                {
                    label2.ForeColor = Color.Black;
                    label2.Text = "(Zonder extensie wordt het een txt bestand)";
                }
            }

        }

        //Zorgt er voor dat ongeldige tekens niet zichtbaar worden in de Textbox
        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        //Bij dubbelklik op de picture kies andere downloadmap
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            btnBewaar.Visible = true;
            btnTerug.Visible = true;

            textBox2.Text = radioButton1.Text;
            textBox3.Text = radioButton2.Text;
            textBox4.Text = radioButton3.Text;
            textBox5.Text = radioButton4.Text;
            textBox1.Enabled = false;
            label1.Text = "Klik op -Bewaar Wijzigingen- om aanpassingen te bewaren of klik op -Terug-";
            pictureBox1.Enabled = false;
            ActiveControl = btnTerug;
        }

        private void btnBewaar_Click(object sender, EventArgs e)
        {
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            btnBewaar.Visible = false;
            btnTerug.Visible = false;
            pictureBox1.Enabled = true;

            //Delete eerste de config.ini file
            File.Delete(Application.StartupPath + @"\config.ini");

            FileStream fs = new FileStream(Application.StartupPath + @"\config.ini",
            FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            using (StreamWriter sr = new StreamWriter(fs))
            {
                sr.WriteLine(label3.Text);
                sr.WriteLine(textBox2.Text);
                sr.WriteLine(textBox3.Text);
                sr.WriteLine(textBox4.Text);
                sr.WriteLine(textBox5.Text);
            }
            
                //Lees de gewijzigde filenamen
                FileStream ft = new FileStream(Application.StartupPath + @"\config.ini",
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using (StreamReader sr = new StreamReader(ft))
                {
                    string volg;
                    int tel = 0;
                    while ((volg = sr.ReadLine()) != null)
                    {
                        temp = myfiles[0];
                        myfiles[tel] = volg;
                        if (tel < 5) { tel++; }
                    }
                }

            //vul de radiobuttons weer
            radioButton1.Text = myfiles[1];
            radioButton2.Text = myfiles[2];
            radioButton3.Text = myfiles[3];
            radioButton4.Text = myfiles[4];
            textBox1.Enabled = true;
            label1.Text = "Geef de naam van het te maken bestand of klik op een van de keuzeknoppen";
            label2.Text = "(Zonder extensie wordt het een txt bestand)";
            radioButton5.Checked = true;
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        //Bij terug naar het standaard overzicht
        private void btnTerug_Click(object sender, EventArgs e)
        {
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            btnBewaar.Visible = false;
            btnTerug.Visible = false;
            textBox1.Enabled = true;
            radioButton5.Checked = true;
            pictureBox1.Enabled = true;
            label1.Text = "Geef de naam van het te maken bestand of klik op een van de keuzeknoppen";
            label2.Text = "(Zonder extensie wordt het een txt bestand)";
            ActiveControl = textBox1;
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        //Bij dubbelklik op label van downloadmap
        private void label3_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + @"\config.ini"))
            {
                FileStream fst = new FileStream(Application.StartupPath + @"\config.ini",
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using (StreamReader sr = new StreamReader(fst))
                {
                    temp = sr.ReadLine();
                }
            }

            label3.Text = temp;

            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "Selecteer de download map voor te maken bestanden:" +
            " \r(Huidige map is: " + temp + ")";

            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                label3.Text = ofd.SelectedPath;

                //Delete eerste de config.ini file
                File.Delete(Application.StartupPath + @"\config.ini");

                //Schrijf een nieuwe file
                FileStream fs = new FileStream(Application.StartupPath + @"\config.ini",
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(label3.Text);
                    sr.WriteLine(myfiles[1]);
                    sr.WriteLine(myfiles[2]);
                    sr.WriteLine(myfiles[3]);
                    sr.WriteLine(myfiles[4]);
                }
                LoadStreamReader();
            }
            ActiveControl = textBox1;
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        //laadt alle gegevens uit de config.ini file en zet die in de settingsfile
        public void LoadStreamReader()
        {
            if (File.Exists(Application.StartupPath + @"\config.ini"))
                {
                FileStream fs = new FileStream(Application.StartupPath + @"\config.ini",
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using (StreamReader sr = new StreamReader(fs))
                {
                    string volg;
                    int tel = 0;
                    while ((volg = sr.ReadLine()) != null)
                    {
                        temp = myfiles[0];
                            myfiles[tel] = volg;
                        if (tel < 5) { tel++; }
                    }
                }
            }
            else
            {
                FileStream fs = new FileStream(Application.StartupPath + @"\config.ini",
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine(@"C:\Users\Ron Doomen\Downloads");
                    sr.WriteLine("Info.txt");
                    sr.WriteLine("Serial.txt");
                    sr.WriteLine("Instructies.txt");
                    sr.WriteLine("ReadMe.txt");
                }
                LoadStreamReader();
            }
        }

        public void maakbestand()
        {
            try
            {
                using (FileStream fs = File.Create(path)) { }
                label2.ForeColor = Color.Black;
                label2.Text = "Het bestand is aangemaakt...";

                Process ExternalProcess = new Process();
                ExternalProcess.StartInfo.FileName = path;
                ExternalProcess.Start();
                //ExternalProcess.WaitForExit();
                Application.Exit();
            }
            catch (Exception e)
            {
                label2.Text = "Er is een fout opgetreden, er is geen bestand aangemaakt...";
                label2.ForeColor = Color.Red;
            }
        }

        public void midd()
        {
            text = "xxx" + textBox1.Text;
            mid = text.Substring(text.Length - 4, 1);
        }

        // Alle boxen en labels en textbox
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label1.Text = "(Dubbelklik om bestanden aan te passen...)";
            ActiveControl = textBox1;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (btnBewaar.Visible == false)
            {
                label2.ForeColor = Color.Black;
                label1.Text = "Geef de naam van het te maken bestand of klik op een van de keuzeknoppen";
                ActiveControl = textBox1;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            if (textBox1.Text == "")
            { btnMaak.Enabled = false; }
            else { btnMaak.Enabled = true; }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = myfiles[1];
            ActiveControl = btnMaak;
            textBox1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = myfiles[2];
            ActiveControl = btnMaak;
            textBox1.Enabled = false;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = myfiles[3];
            ActiveControl = btnMaak;
            textBox1.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = myfiles[4];
            ActiveControl = btnMaak;
            textBox1.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox1.Text = "";
            label2.ForeColor = Color.Black;
            label2.Text = "(Zonder extensie wordt het een txt bestand)";
            ActiveControl = textBox1;
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
            }

            if (checkBox1.Checked == false)
            {
                checkBox2.Checked = true;
            }
            ActiveControl = textBox1;
         }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }

            if (checkBox2.Checked == false)
            {
                checkBox1.Checked = true;
            }
            ActiveControl = textBox1;
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Text = "(Dubbelklik om Downloadmap aan te passen...)";
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label2.Text = "(Zonder extensie wordt het een txt bestand)";
        }

    }
}
