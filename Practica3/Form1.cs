using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SimioAPI.Graphics;
using Simio;
using Simio.SimioEnums;
using SimioAPI;

namespace Practica3
{
    public partial class Form1 : Form
    {
        private ISimioProject proyectoApi;
        private string rutabase = "ModeloBase.spfx";
        private string rutafinal = "ModeloFinal.spfx";
        private string[] warnings;
        int contNodos = 1;
        int contPaths = 0;
        private IModel modelo;
        private IIntelligentObjects intelligentObjects;

        public Form1()
        {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            modelo = proyectoApi.Models[1];
            intelligentObjects = modelo.Facility.IntelligentObjects;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            crearModelo();
        }

        public void crearModelo()
        {
            crearRegiones();
            dibujarContorno();

            //modelo.Facility.IntelligentObjects["Source1"].Properties["InterarrivalTime"].Value = "Random.Exponential(5)";

            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
        }

        public void crearSource(string nombre, double x, double y)
        {
            crearObjeto("Source", x, y);
            modelo.Facility.IntelligentObjects["Source1"].ObjectName = nombre;
        }

        public void crearServer(string nombre, double x, double y)
        {
            crearObjeto("Server", x, y);
            modelo.Facility.IntelligentObjects["Server1"].ObjectName = nombre;
        }

        public void crearSink(string nombre, double x, double y)
        {
            crearObjeto("Sink", x, y);
            modelo.Facility.IntelligentObjects["Sink1"].ObjectName = nombre;
        }

        public void crearEntity(string nombre, double x, double y)
        {
            crearObjeto("ModelEntity", x, y);
            modelo.Facility.IntelligentObjects["ModelEntity1"].ObjectName = nombre;
        }

        public void crearTransferNode(string nombre, double x, double y)
        {
            crearObjeto("TransferNode", x, y);
            modelo.Facility.IntelligentObjects["TransferNode1"].ObjectName = nombre;
            contNodos++;
        }

        public void crearPath(string nodo1, string nodo2)
        {
            intelligentObjects.CreateLink("Path", ((INodeObject)modelo.Facility.IntelligentObjects[nodo1]), ((INodeObject)modelo.Facility.IntelligentObjects[nodo2]), null);
            contPaths++;
        }

        public void crearObjeto(string tipo, double x, double y)
        {
            intelligentObjects.CreateObject(tipo, new FacilityLocation(x, 0, y));
        }

        public void crearRegiones()
        {
            crearTransferNode("Metropolitana", 0, 0);
            crearTransferNode("Norte", 5000, -20000);
            crearTransferNode("NorOriente", 30000, -10000);
            crearTransferNode("SurOriente", 15000, 20000);
            crearTransferNode("Central", -15000, 20000);
            crearTransferNode("SurOccidente", -40000, 10000);
            crearTransferNode("NorOccidente", -40000, -20000);
            crearTransferNode("Peten", 10000, -50000);
        }

        public void dibujarContorno()
        {
            // --------- FRONTERA BELICE ---------
            crearTransferNode("N" + contNodos, 42000, -30000);
            crearTransferNode("N" + contNodos, 30000, -30000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 34000);

            crearTransferNode("N" + contNodos, 30000, -80000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 232000);

            // --------- FRONTERA MEXICO ---------
            crearTransferNode("N" + contNodos, -20000, -80000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 210000);

            crearTransferNode("N" + contNodos, -20000, -60000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 83000);

            crearTransferNode("N" + contNodos, -41000, -60000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 57000);

            crearTransferNode("N" + contNodos, -27000, -49000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 79000);

            crearTransferNode("N" + contNodos, -11000, -40000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 86000);

            crearTransferNode("N" + contNodos, -11250, -30000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 46000);

            crearTransferNode("N" + contNodos, -52000, -30000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 151000);

            crearTransferNode("N" + contNodos, -63750, -7500);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 125000);

            crearTransferNode("N" + contNodos, -59000, 2000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 40000);

            crearTransferNode("N" + contNodos, -66000, 18000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 85000);

            // --------- FRONTERA PACIFICO ---------
            crearTransferNode("N" + contNodos, -54000, 36000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 60000);

            crearTransferNode("N" + contNodos, -30000, 43000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 75000);

            crearTransferNode("N" + contNodos, -3000, 40000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 63000);

            crearTransferNode("N" + contNodos, 22500, 42500);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 56000);

            // --------- FRONTERA EL SALVADOR ---------
            crearTransferNode("N" + contNodos, 40000, 16250);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 203000);

            // --------- FRONTERA HONDURAS ---------
            crearTransferNode("N" + contNodos, 50000, 4000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 63000);

            crearTransferNode("N" + contNodos, 47000, -12000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 55000);

            crearTransferNode("N" + contNodos, 60000, -29000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 138000);

            // --------- FRONTERA CARIBE ---------
            crearTransferNode("N" + contNodos, 48500, -26000);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaPath("Path" + contPaths, 96000);

            crearPath("N" + (contNodos - 1), "N" + (contNodos - 22));
            distanciaPath("Path" + contPaths, 52000);
        }

        public void distanciaPath(string nombre, int distancia)
        {
            modelo.Facility.IntelligentObjects[nombre].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects[nombre].Properties["LogicalLength"].Value = distancia.ToString();
        }
    }
}
