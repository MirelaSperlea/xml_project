using Newtonsoft.Json.Linq;
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

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        public string FilePath { get; }

        public Form2(string filePath)
        {
            InitializeComponent();
            FilePath = filePath;
            DisplayData(filePath);
        }



        private void DisplayData(string filePath)
        {
            try
            {
                if (Path.GetExtension(filePath).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    // Incarcam fisierul XML
                    System.Xml.Linq.XDocument xmlDoc = XDocument.Load(filePath);

                    // Restul codului pentru procesarea datelor din XML
                    // Creează un DataTable pentru a stoca datele
                    DataTable dataTable = new DataTable();

                    // Adaugă coloanele în DataTable
                    dataTable.Columns.Add("ID_Inchiriere");
                    dataTable.Columns.Add("Bicicletă");
                    dataTable.Columns.Add("Client");
                    dataTable.Columns.Add("Perioada Inchiriere");
                    dataTable.Columns.Add("Suma de Achitat");

                        
                    foreach (XElement element in xmlDoc.Descendants("inchiriere"))
                    {
                      
                        string infoBici =
                             $"Model: {element.Element("bicicleta").Element("model").Value};\r\n" +
                             $" Culoare: {element.Element("bicicleta").Element("caracteristici").Element("culoare").Value};\r\n" +
                             $" Greutate: {element.Element("bicicleta").Element("caracteristici").Element("greutate").Value};\r\n" +
                             $" Pret pe ora: {element.Element("bicicleta").Element("pret_pe_ora").Value};\r\n" +
                             $" Disponibilitate: {element.Element("bicicleta").Element("disponibilitate").Value}";
                        string infoClient =
                             $" Nume:{element.Element("client").Element("nume").Value};\n" +
                             $" Telefon:{element.Element("client").Element("contact").Element("telefon").Value};\n"+
                             $" Emial:{element.Element("client").Element("contact").Element("email").Value};\n";

                        string infoperioada =
                            $" Din {element.Element("perioada").Element("inceput").Value} \n" + 
                            $" pana la {element.Element("perioada").Element("sfarsit").Value};\n" + 
                            $" Durata:{element.Element("perioada").Element("durata").Value}";

                        // Adaugăm un rând în DataTable cu informațiile din elementul curent
                        DataRow row = dataTable.NewRow();
                        row["ID_Inchiriere"] = element.Attribute("id").Value;
                        row["Bicicletă"] = infoBici;
                        row["Client"] = infoClient;
                        row["Perioada Inchiriere"] = infoperioada;
                        row["Suma de Achitat"]= element.Element("suma").Value;
                        dataTable.Rows.Add(row);
                    }

                    // Setează sursa de date a DataGridView la DataTable
                    dataGridView1.DataSource = dataTable;
                }
                else if (Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    // Citim conținutul fișierului JSON
                    string jsonContent = File.ReadAllText(filePath);

                    // Parsăm conținutul JSON într-un obiect JObject
                    JObject jsonObject = JObject.Parse(jsonContent);

                    // Verificăm dacă fișierul JSON conține date despre biciclete
                    if (jsonObject["inchirieri_biciclete"] != null)
                    {
                        // Găsim toate obiectele de închiriere de biciclete
                        var inchirieriBiciclete = jsonObject["inchirieri_biciclete"]["inchiriere"];

                        // Inițializăm un DataTable pentru a stoca datele
                        DataTable dataTable = new DataTable();

                        // Adaugă coloanele în DataTable
                        dataTable.Columns.Add("ID_Inchiriere");
                        dataTable.Columns.Add("Bicicletă");
                        dataTable.Columns.Add("Client");
                        dataTable.Columns.Add("Perioada Inchiriere");
                        dataTable.Columns.Add("Suma de achitat");

                        // Parcurgem fiecare închiriere de bicicletă și adăugăm datele în DataTable
                        foreach (var inchiriere in inchirieriBiciclete)
                        {
                            string modelBicicleta = inchiriere["bicicleta"]["model"].ToString();
                            string culoareBicicleta = inchiriere["bicicleta"]["caracteristici"]["culoare"].ToString();
                            string greutateBicicleta = inchiriere["bicicleta"]["caracteristici"]["greutate"].ToString();
                            string pretBicicleta = inchiriere["bicicleta"]["pret_pe_ora"].ToString();
                            string dispotBicicleta = inchiriere["bicicleta"]["disponibilitate"].ToString();

                            string numeClient = inchiriere["client"]["nume"].ToString();
                            string telClient = inchiriere["client"]["contact"]["telefon"].ToString();
                            string emailClient = inchiriere["client"]["contact"]["email"].ToString();

                            string inceput = inchiriere["perioada"]["inceput"].ToString();
                            string sfarsit = inchiriere["perioada"]["sfarsit"].ToString();
                            string durata = inchiriere["perioada"]["durata"].ToString();

                            string suma = inchiriere["suma"].ToString();


                            // Adăugăm un rând în DataTable cu informațiile din închirierea curentă
                            DataRow row = dataTable.NewRow();
                            row["ID_Inchiriere"] = inchiriere["id"].ToString();
                            row["Bicicletă"] = $"Model: {modelBicicleta};" +
                                 $" Culoare: {culoareBicicleta};" +
                                 $" Greutate: {greutateBicicleta};" +
                                 $" Pret pe ora: {pretBicicleta};" +
                                 $" Disponibilitate: {dispotBicicleta};";
                            row["Client"] = $" Nume: {numeClient};" +
                                $" Telefon: {telClient};" +
                                $" Emial: {emailClient};";

                            row["Perioada Inchiriere"] = $"Din {inceput} " +
                                $" Pana {sfarsit};" +
                                $" Perioada: {durata} ";
                            row["Suma de achitat"] = suma;
                            dataTable.Rows.Add(row);
                        }

                        // Setează sursa de date a DataGridView la DataTable
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("Tipul de fișier nu este suportat. Vă rugăm să selectați un fișier XML sau JSON.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A intervenit o eroare la citirea fișierului: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayXmlData(string filePath)
        {
            try
            {
                // Incarcam fisierul XML
                XDocument xmlDoc = XDocument.Load(filePath);

                // Creează un DataTable pentru a stoca datele
                DataTable dataTable = new DataTable();

                // Adaugă coloanele în DataTable
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Model Bicicletă");
                dataTable.Columns.Add("Nume Client");
                dataTable.Columns.Add("Email Client");

                foreach (XElement element in xmlDoc.Descendants("inchiriere"))
                {
                    // Concatenăm modelul bicicletei și culoarea într-un singur șir
                    string modelSiCuloare = $"{element.Element("bicicleta").Element("model").Value} - {element.Element("bicicleta").Element("caracteristici").Element("culoare").Value}";

                    // Adaugăm un rând în DataTable cu informațiile din elementul curent
                    DataRow row = dataTable.NewRow();
                    row["ID"] = element.Attribute("id").Value;
                    row["Model Bicicletă"] = modelSiCuloare; // Folosim șirul concatenat în locul valorii modelului de bază
                    row["Nume Client"] = element.Element("client").Element("nume").Value;
                    row["Email Client"] = element.Element("client").Element("contact").Element("email").Value;
                    dataTable.Rows.Add(row);
                }

                // Setează sursa de date a DataGridView la DataTable
                dataGridView1.DataSource = dataTable;



            }
            catch (Exception ex)
            {
                MessageBox.Show($"A intervenit o eroare la citirea fișierului XML: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}