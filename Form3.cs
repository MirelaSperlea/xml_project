
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
using System.Xml.Xsl;
using System.Xml;
using CefSharp.WinForms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        private ChromiumWebBrowser chromiumWebBrowser;
        public string FilePath { get; }

        public Form3(string filePath)
        {
            InitializeComponent();
            FilePath = filePath;
            chromiumWebBrowser = new ChromiumWebBrowser("file:///C:/Users/mirel/OneDrive/Desktop/XIS/Proiect/bicycle_t.html");
            chromiumWebBrowser.Dock = DockStyle.Fill;
            Controls.Add(chromiumWebBrowser);
        }

        // Metoda de creare a tabelului utilizând XSL
        private void TransformXmlWithXslt(string xmlFile)
        {
            // Calea către fișierul XSLT
            string xsltFile = "C:\\Users\\mirel\\OneDrive\\Desktop\\XIS\\Proiect\\Sperlea_Mirela_341A3\\Biciclete_Sperlea_Mirela.xsl";

            // Calea către fișierul HTML 
            string htmlFile = "C:\\Users\\mirel\\OneDrive\\Desktop\\XIS\\Proiect\\bicycle_t.html";

            // Configurează setările cititorului XML pentru a permite procesarea DTD
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;

            // Creează un cititor XML cu setările configurate
            using (XmlReader reader = XmlReader.Create(xmlFile, settings))
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xsltFile);
                xslt.Transform(reader, null, File.Create(htmlFile));
            }

            Console.WriteLine("Transformarea XML în HTML a fost realizată cu succes.");
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
