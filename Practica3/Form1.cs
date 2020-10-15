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

            //modelo.Facility.IntelligentObjects["Source1"].Properties["InterarrivalTime"].Value = "Random.Exponential(5)";

            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
        }

        public void crearRegiones()
        {
            crearTransferNode("Metropolitana", 0, 0);
            crearTransferNode("Norte", 5, -20);
            crearTransferNode("NorOriente", 30, -10);
            crearTransferNode("SurOriente", 15, 20);
            crearTransferNode("Central", -15, 20);
            crearTransferNode("SurOccidente", -40, 10);
            crearTransferNode("NorOccidente", -40, -20);
            crearTransferNode("Peten", 10, -50);
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
        }

        public void crearPath(string nodo1, string nodo2)
        {
            intelligentObjects.CreateLink("Path", ((INodeObject)modelo.Facility.IntelligentObjects[nodo1]), ((INodeObject)modelo.Facility.IntelligentObjects[nodo2]), null);
        }

        public void crearObjeto(string tipo, double x, double y)
        {
            intelligentObjects.CreateObject(tipo, new FacilityLocation(x, 0, y));
        }
    }
}
