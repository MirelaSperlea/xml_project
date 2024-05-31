using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private object tabControl1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML files (*.xml)|*.xml|JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // Setează calea fișierului selectat în câmpul de căutare
                    textBox5.Text = selectedFilePath;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Verificăm dacă s-a selectat un fișier
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Vă rugăm să selectați un fișier XML sau JSON.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificăm dacă textBox1 sau textBox2 nu sunt goale
            /*
            if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vă rugăm să introduceți numele modelului de bicicletă sau numele clientului pentru căutare.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            */

            // Verificăm extensia fișierului
            string selectedFilePath = textBox5.Text;
            string fileExtension = Path.GetExtension(selectedFilePath);
            if (fileExtension == ".xml")
            {
                // Parsăm fișierul XML
                XmlDocument doc = new XmlDocument();
                doc.Load(selectedFilePath);

                // Găsim toate elementele <bicicleta> care au atributul model corespunzător sau care sunt închiriate de clientul specificat
                XmlNodeList bicicleteNodes = doc.SelectNodes("//bicicleta[model='" + textBox1.Text + "' or ../client[nume='" + textBox2.Text + "']]");

                // Verificăm dacă am găsit biciclete
                if (bicicleteNodes.Count > 0)
                {
                    StringBuilder result = new StringBuilder();
                    foreach (XmlNode bicicletaNode in bicicleteNodes)
                    {
                        result.AppendLine("Model: " + bicicletaNode["model"].InnerText);
                        result.AppendLine("Culoare: " + bicicletaNode["caracteristici"]["culoare"].InnerText);
                        result.AppendLine("Greutate: " + bicicletaNode["caracteristici"]["greutate"].InnerText);
                        result.AppendLine("Pret pe ora: " + bicicletaNode["pret_pe_ora"].InnerText);
                        result.AppendLine("Disponibilitate: " + bicicletaNode["disponibilitate"].InnerText);

                        // Obținem informațiile despre client
                        XmlNode clientNode = bicicletaNode.SelectSingleNode("../client");
                        if (clientNode != null)
                        {
                            result.AppendLine("Client:");
                            result.AppendLine("  Nume: " + clientNode["nume"].InnerText);
                            result.AppendLine("  Telefon: " + clientNode["contact"]["telefon"].InnerText);
                            result.AppendLine("  Email: " + clientNode["contact"]["email"].InnerText);
                        }

                        result.AppendLine();
                    }
                    textBox3.Text = result.ToString();
                }
                else
                {
                    textBox3.Text = "Nu s-au găsit biciclete cu modelul sau clientul specificat.";
                }
            }
            else if (fileExtension == ".json")
            {
                // Parsăm fișierul JSON
                string jsonContent = File.ReadAllText(selectedFilePath);
                JObject jsonObject = JObject.Parse(jsonContent);

                // Găsim toate obiectele în lista de închirieri care au bicicletele cu modelul sau clientul specificat
                var biciclete = jsonObject["inchirieri_biciclete"]["inchiriere"]
                                    .Where(b => b["bicicleta"]["model"].ToString() == textBox1.Text ||
                                                b["client"]["nume"].ToString() == textBox2.Text)
                                    .Select(b => new
                                    {
                                        Model = b["bicicleta"]["model"].ToString(),
                                        Culoare = b["bicicleta"]["caracteristici"]["culoare"].ToString(),
                                        Greutate = b["bicicleta"]["caracteristici"]["greutate"].ToString(),
                                        PretPeOra = b["bicicleta"]["pret_pe_ora"].ToString(),
                                        Disponibilitate = b["bicicleta"]["disponibilitate"].ToString(),
                                        NumeClient = b["client"]["nume"].ToString(),
                                        TelefonClient = b["client"]["contact"]["telefon"].ToString(),
                                        EmailClient = b["client"]["contact"]["email"].ToString()
                                    });

                // Verificăm dacă am găsit biciclete
                if (biciclete.Any())
                {
                    StringBuilder result = new StringBuilder();
                    foreach (var bicicleta in biciclete)
                    {
                        result.AppendLine("Model: " + bicicleta.Model);
                        result.AppendLine("Culoare: " + bicicleta.Culoare);
                        result.AppendLine("Greutate: " + bicicleta.Greutate);
                        result.AppendLine("Pret pe ora: " + bicicleta.PretPeOra);
                        result.AppendLine("Disponibilitate: " + bicicleta.Disponibilitate);
                        result.AppendLine("Client:");
                        result.AppendLine("  Nume: " + bicicleta.NumeClient);
                        result.AppendLine("  Telefon: " + bicicleta.TelefonClient);
                        result.AppendLine("  Email: " + bicicleta.EmailClient);
                        result.AppendLine();
                    }
                    textBox3.Text = result.ToString();
                }
                else
                {
                    textBox3.Text = "Nu s-au găsit biciclete cu modelul sau clientul specificat.";
                }
            }
            else
            {
                MessageBox.Show("Tipul de fișier nu este suportat. Vă rugăm să selectați un fișier XML sau JSON.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificăm dacă fișierul a fost încărcat
            string selectedFilePath = textBox5.Text;
            if (string.IsNullOrWhiteSpace(selectedFilePath))
            {
                MessageBox.Show("Vă rugăm să selectați mai întâi un fișier.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Citim conținutul fișierului 
            string fileContent = File.ReadAllText(selectedFilePath);

            // Verificăm extensia fișierului pentru a determina tipul
            string fileExtension = Path.GetExtension(selectedFilePath);

            if (fileExtension == ".json")
            {
                // Parsează conținutul JSON într-un obiect JObject
                JObject jsonObject = JObject.Parse(fileContent);
                // Verificăm selecția din comboBox
                string selectedChoice = comboBox1.SelectedItem.ToString();

                if (selectedChoice == "pret/ora")
                {
                    // Găsim toate obiectele bicicletă și le ordonăm după preț/oră
                    var biciclete = jsonObject["inchirieri_biciclete"]["inchiriere"]
                                        .OrderBy(b => (int)b["bicicleta"]["pret_pe_ora"]);

                    if (biciclete.Any())
                    {
                        // Inițializăm un StringBuilder pentru a construi rezultatul
                        StringBuilder result = new StringBuilder();

                        // Parcurgem fiecare bicicletă sortată și adăugăm informațiile în StringBuilder
                        foreach (var bicicleta in biciclete)
                        {
                            string modelBicicleta = bicicleta["bicicleta"]["model"].ToString();
                            string culoareBicicleta = bicicleta["bicicleta"]["caracteristici"]["culoare"].ToString();
                            string greutateBicicleta = bicicleta["bicicleta"]["caracteristici"]["greutate"].ToString();
                            string pretPeOraBicicleta = bicicleta["bicicleta"]["pret_pe_ora"].ToString();
                            string disponibilitateBicicleta = bicicleta["bicicleta"]["disponibilitate"].ToString();

                            // Adăugăm informațiile despre bicicletă în StringBuilder
                            result.AppendLine($"Preț pe oră: {pretPeOraBicicleta}");
                            result.AppendLine($"Model: {modelBicicleta}");
                            result.AppendLine($"Culoare: {culoareBicicleta}");
                            result.AppendLine($"Greutate: {greutateBicicleta}");
                            result.AppendLine($"Disponibilitate: {disponibilitateBicicleta}");
                            result.AppendLine();
                        }

                        // Afișăm rezultatul în textBox1
                        textBox3.Text = result.ToString();
                    }
                }
                else if (selectedChoice == "culoare")
                {
                    // Găsim toate obiectele bicicletă și le ordonăm după culoare
                    var biciclete = jsonObject["inchirieri_biciclete"]["inchiriere"]
                                        .OrderBy(b => (string)b["bicicleta"]["caracteristici"]["culoare"]);

                    if (biciclete.Any())
                    {
                        // Inițializăm un StringBuilder pentru a construi rezultatul
                        StringBuilder result = new StringBuilder();

                        // Parcurgem fiecare bicicletă sortată și adăugăm informațiile în StringBuilder
                        foreach (var bicicleta in biciclete)
                        {
                            string modelBicicleta = bicicleta["bicicleta"]["model"].ToString();
                            string culoareBicicleta = bicicleta["bicicleta"]["caracteristici"]["culoare"].ToString();
                            string greutateBicicleta = bicicleta["bicicleta"]["caracteristici"]["greutate"].ToString();
                            string pretPeOraBicicleta = bicicleta["bicicleta"]["pret_pe_ora"].ToString();
                            string disponibilitateBicicleta = bicicleta["bicicleta"]["disponibilitate"].ToString();

                            // Adăugăm informațiile despre bicicletă în StringBuilder
                            result.AppendLine($"Culoare: {culoareBicicleta}");
                            result.AppendLine($"Model: {modelBicicleta}");
                            result.AppendLine($"Greutate: {greutateBicicleta}");
                            result.AppendLine($"Preț pe oră: {pretPeOraBicicleta}");
                            result.AppendLine($"Disponibilitate: {disponibilitateBicicleta}");
                            result.AppendLine();
                        }

                        // Afișăm rezultatul în textBox1
                        textBox3.Text = result.ToString();
                    }
                }
                else
                {
                    // Opțiunea selectată nu este validă pentru fișiere JSON
                    MessageBox.Show("Opțiunea selectată nu este validă.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (fileExtension == ".xml")
            {
                // Parsează conținutul XML într-un obiect XDocument
                XDocument xmlDoc = XDocument.Parse(fileContent);
                // Verificăm selecția din comboBox
                string selectedChoice = comboBox1.SelectedItem.ToString();
                if (selectedChoice == "pret/ora")
                {
                    // Găsește toate obiectele bicicletă și le ordonează după preț/oră

                    var biciclete = xmlDoc
                        .Descendants("inchiriere")
                        .OrderBy(b => (int)b.Element("bicicleta").Element("pret_pe_ora"));


                    if (biciclete.Any())
                    {
                        // Inițializăm un StringBuilder pentru a construi rezultatul
                        StringBuilder result = new StringBuilder();

                        // Parcurgem fiecare bicicletă sortată și adăugăm informațiile în StringBuilder
                        foreach (var bicicleta in biciclete)
                        {
                            string modelBicicleta = bicicleta.Element("bicicleta").Element("model").Value;
                            string culoareBicicleta = bicicleta.Element("bicicleta").Element("caracteristici").Element("culoare").Value;
                            string greutateBicicleta = bicicleta.Element("bicicleta").Element("caracteristici").Element("greutate").Value;
                            string pretPeOraBicicleta = bicicleta.Element("bicicleta").Element("pret_pe_ora").Value;
                            string disponibilitateBicicleta = bicicleta.Element("bicicleta").Element("disponibilitate").Value;

                            // Adăugăm informațiile despre bicicletă în StringBuilder
                            result.AppendLine($"Preț pe oră: {pretPeOraBicicleta}");
                            result.AppendLine($"Model: {modelBicicleta}");
                            result.AppendLine($"Culoare: {culoareBicicleta}");
                            result.AppendLine($"Greutate: {greutateBicicleta}");
                            result.AppendLine($"Disponibilitate: {disponibilitateBicicleta}");
                            result.AppendLine();
                        }

                        // Afișăm rezultatul în textBox1
                        textBox3.Text = result.ToString();
                    }
                    else
                    {
                        textBox3.Text = "Nu s-au găsit biciclete în fișierul XML.";
                    }
                }
                else if (selectedChoice == "culoare")
                {
                    // Găsește toate obiectele bicicletă și le ordonează după culoare

                    var biciclete = xmlDoc
                        .Descendants("inchiriere")
                        .OrderBy(b => b.Element("bicicleta").Element("caracteristici").Element("culoare").Value);


                    if (biciclete.Any())
                    {
                        // Inițializăm un StringBuilder pentru a construi rezultatul
                        StringBuilder result = new StringBuilder();

                        // Parcurgem fiecare bicicletă sortată și adăugăm informațiile în StringBuilder
                        foreach (var bicicleta in biciclete)
                        {
                            string modelBicicleta = bicicleta.Element("bicicleta").Element("model").Value;
                            string culoareBicicleta = bicicleta.Element("bicicleta").Element("caracteristici").Element("culoare").Value;
                            string greutateBicicleta = bicicleta.Element("bicicleta").Element("caracteristici").Element("greutate").Value;
                            string pretPeOraBicicleta = bicicleta.Element("bicicleta").Element("pret_pe_ora").Value;
                            string disponibilitateBicicleta = bicicleta.Element("bicicleta").Element("disponibilitate").Value;

                            // Adăugăm informațiile despre bicicletă în StringBuilder
                            result.AppendLine($"Culoare: {culoareBicicleta}");
                            result.AppendLine($"Model: {modelBicicleta}");
                            result.AppendLine($"Greutate: {greutateBicicleta}");
                            result.AppendLine($"Preț pe oră: {pretPeOraBicicleta}");
                            result.AppendLine($"Disponibilitate: {disponibilitateBicicleta}");
                            result.AppendLine();
                        }

                        // Afișăm rezultatul în textBox1
                        textBox3.Text = result.ToString();
                    }


                }
                else
                {
                    // Fișierul nu are o extensie cunoscută sau suportată
                    MessageBox.Show("Fișierul selectat nu are o extensie suportată (.json sau .xml).", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


            }

        }
    

        private void button2_Click(object sender, EventArgs e)
        { 
          string filePath = textBox5.Text;
          // DisplayData(filePath);

            // Comutăm la noua pagină când utilizatorul apasă pe button2
            // Crează o instanță a Form2
            Form2 form2 = new Form2(filePath);
            form2.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = textBox5.Text;
            Form3 form3 = new Form3(filePath);
            form3.Show();

        }
    }

}
