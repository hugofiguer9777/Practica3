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
        int contConveyor = 0;
        private IModel modelo;
        private IIntelligentObjects intelligentObjects;
        private int contPaths = 0;
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
            crearEstaciones();
            crearRutas();
            dibujarContorno();
            //modelo.Facility.IntelligentObjects["Source1"].Properties["InterarrivalTime"].Value = "Random.Exponential(5)";

            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
        }

        public void crearRegiones()
        {
            crearTransferNode("Metropolitana", 0, 0);
            modelo.Facility.IntelligentObjects["Metropolitana"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("Norte", 5000, -20000);
            modelo.Facility.IntelligentObjects["Norte"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("NorOriente", 30000, -10000);
            modelo.Facility.IntelligentObjects["NorOriente"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("SurOriente", 15000, 20000);
            modelo.Facility.IntelligentObjects["SurOriente"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("Central", -15000, 20000);
            modelo.Facility.IntelligentObjects["Central"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("SurOccidente", -40000, 10000);
            modelo.Facility.IntelligentObjects["SurOccidente"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("NorOccidente", -40000, -20000);
            modelo.Facility.IntelligentObjects["NorOccidente"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("Peten", 10000, -50000);
            modelo.Facility.IntelligentObjects["Peten"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
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

        public void crearEntityAvion(string nombre, double x, double y)
        {
            crearObjeto("Avion", x, y);
            modelo.Facility.IntelligentObjects["Avion1"].ObjectName = nombre;
        }

        public void crearEntityTurista(string nombre, double x, double y)
        {
            crearObjeto("Turista", x, y);
            modelo.Facility.IntelligentObjects["Turista1"].ObjectName = nombre;
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

        public void crearConveyor(string nodo1, string nodo2)
        {
            intelligentObjects.CreateLink("Conveyor", ((INodeObject)modelo.Facility.IntelligentObjects[nodo1]), ((INodeObject)modelo.Facility.IntelligentObjects[nodo2]), null);
            contConveyor++;
        }

        public void crearNodeServer(string nodo1, string nodo2)
        {
            intelligentObjects.CreateLink("Path", ((INodeObject)modelo.Facility.IntelligentObjects[nodo1]), ((IFixedObject)modelo.Facility.IntelligentObjects[nodo2]).Nodes[0], null);
            contPaths++;
        }

        public void crearPathSourceNodo(string nodo1, string nodo2)
        {
            intelligentObjects.CreateLink("Path", ((IFixedObject)modelo.Facility.IntelligentObjects[nodo1]).Nodes[0], ((INodeObject)modelo.Facility.IntelligentObjects[nodo2]), null);
            contPaths++;
        }

        public void crearObjeto(string tipo, double x, double y)
        {
            intelligentObjects.CreateObject(tipo, new FacilityLocation(x, 0, y));
        }

        public void cambiarPropiedad(string objeto, string propiedad, string valor)
        {
            modelo.Facility.IntelligentObjects[objeto].Properties[propiedad].Value = valor;
        }
        
        ///MI PARTE  201503750
        public void crearEstaciones()
        {
            crearServer("Estacion_Metr", 0, -100);
            crearTransferNode("TNE_Metr", -100, -100);
            modelo.Facility.IntelligentObjects["TNE_Metr"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_Metr", 100, -100);
            modelo.Facility.IntelligentObjects["TNS_Metr"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_Metr", "Estacion_Metr");
            crearNodeServer("TNS_Metr", "Estacion_Metr");
            modelo.Facility.IntelligentObjects["Estacion_Metr"].Properties["InitialCapacity"].Value = "200";
            modelo.Facility.IntelligentObjects["Estacion_Metr"].Properties["ProcessingTime"].Value = "Random.Exponential(4)";

            crearServer("Estacion_Norte", 5000, -20100);
            crearTransferNode("TNE_Norte", 4900, -20100);
            modelo.Facility.IntelligentObjects["TNE_Norte"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_Norte", 5100, -20100);
            modelo.Facility.IntelligentObjects["TNS_Norte"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_Norte", "Estacion_Norte");
            crearNodeServer("TNS_Norte", "Estacion_Norte");
            modelo.Facility.IntelligentObjects["Estacion_Norte"].Properties["InitialCapacity"].Value = "50";
            modelo.Facility.IntelligentObjects["Estacion_Norte"].Properties["ProcessingTime"].Value = "Random.Exponential(5)";

            crearServer("Estacion_NorOri", 30000, -10100);
            crearTransferNode("TNE_NorOri", 29900, -10100);
            modelo.Facility.IntelligentObjects["TNE_NorOri"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_NorOri", 30100, -10100);
            modelo.Facility.IntelligentObjects["TNS_NorOri"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_NorOri", "Estacion_NorOri");
            crearNodeServer("TNS_NorOri", "Estacion_NorOri");
            modelo.Facility.IntelligentObjects["Estacion_NorOri"].Properties["InitialCapacity"].Value = "40";
            modelo.Facility.IntelligentObjects["Estacion_NorOri"].Properties["ProcessingTime"].Value = "Random.Exponential(3)";

            crearServer("Estacion_SurOri", 15000, 20100);
            crearTransferNode("TNE_SurOri", 14900, 20100);
            modelo.Facility.IntelligentObjects["TNE_SurOri"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_SurOri", 15100, 20100);
            modelo.Facility.IntelligentObjects["TNS_SurOri"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_SurOri", "Estacion_SurOri");
            crearNodeServer("TNS_SurOri", "Estacion_SurOri");
            modelo.Facility.IntelligentObjects["Estacion_SurOri"].Properties["InitialCapacity"].Value = "30";
            modelo.Facility.IntelligentObjects["Estacion_SurOri"].Properties["ProcessingTime"].Value = "Random.Exponential(4)";

            crearServer("Estacion_Central", -15000, 20100);
            crearTransferNode("TNE_Central", -15100, 20100);
            modelo.Facility.IntelligentObjects["TNE_Central"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_Central", -14900, 20100);
            modelo.Facility.IntelligentObjects["TNS_Central"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_Central", "Estacion_Central");
            crearNodeServer("TNS_Central", "Estacion_Central");
            modelo.Facility.IntelligentObjects["Estacion_Central"].Properties["InitialCapacity"].Value = "100";
            modelo.Facility.IntelligentObjects["Estacion_Central"].Properties["ProcessingTime"].Value = "Random.Exponential(5)";

            crearServer("Estacion_SurOcci", -40000, 10100);
            crearTransferNode("TNE_SurOcci", -40100, 10100);
            modelo.Facility.IntelligentObjects["TNE_SurOcci"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_SurOcci", -39900, 10100);
            modelo.Facility.IntelligentObjects["TNS_SurOcci"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_SurOcci", "Estacion_SurOcci");
            crearNodeServer("TNS_SurOcci", "Estacion_SurOcci");
            modelo.Facility.IntelligentObjects["Estacion_SurOcci"].Properties["InitialCapacity"].Value = "120";
            modelo.Facility.IntelligentObjects["Estacion_SurOcci"].Properties["ProcessingTime"].Value = "Random.Exponential(3)";

            crearServer("Estacion_NorOcci", -40000, -20100);
            crearTransferNode("TNE_NorOcci", -40100, -20100);
            modelo.Facility.IntelligentObjects["TNE_NorOcci"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_NorOcci", -39900, -20100);
            modelo.Facility.IntelligentObjects["TNS_NorOcci"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_NorOcci", "Estacion_NorOcci");
            crearNodeServer("TNS_NorOcci", "Estacion_NorOcci");
            modelo.Facility.IntelligentObjects["Estacion_NorOcci"].Properties["InitialCapacity"].Value = "30";
            modelo.Facility.IntelligentObjects["Estacion_NorOcci"].Properties["ProcessingTime"].Value = "Random.Exponential(6)";

            crearServer("Estacion_Peten", 10000, -50100);
            crearTransferNode("TNE_Peten", 9900, -50100);
            modelo.Facility.IntelligentObjects["TNE_Peten"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearTransferNode("TNS_Peten", 10100, -50100);
            modelo.Facility.IntelligentObjects["TNS_Peten"].Properties["OutboundLinkRule"].Value = "ByLinkWeight";
            crearNodeServer("TNE_Peten", "Estacion_Peten");
            crearNodeServer("TNS_Peten", "Estacion_Peten");
            modelo.Facility.IntelligentObjects["Estacion_Peten"].Properties["InitialCapacity"].Value = "150";
            modelo.Facility.IntelligentObjects["Estacion_Peten"].Properties["ProcessingTime"].Value = "Random.Exponential(4)";
        }
        public void crearRutas()
        {
            //METROPOLITANA
            crearConveyor("TNS_Metr", "Metropolitana");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.35";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("Metropolitana", "TNE_Metr");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_Metr", "Central");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "63";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.30";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Metr", "SurOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "124";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.15";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Metr", "NorOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "241";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.20";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //NORTE
            crearConveyor("TNS_Norte", "Norte");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.40";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("Norte", "TNE_Norte");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_Norte", "Peten");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "147";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.40";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Norte", "NorOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "138";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.10";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Norte", "NorOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "145";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.10";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //NORORIENTE
            crearConveyor("TNS_NorOri", "NorOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.20";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("NorOriente", "TNE_NorOri");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_NorOri", "Metropolitana");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "241";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.30";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_NorOri", "Norte");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "138";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.15";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_NorOri", "SurOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "231";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.05";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_NorOri", "Peten");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "282";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.30";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //SurOriente
            crearConveyor("TNS_SurOri", "SurOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.40";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("SurOriente", "TNE_SurOri");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_SurOri", "NorOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "231";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.20";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_SurOri", "Metropolitana");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "124";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.25";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_SurOri", "Central");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "154";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.15";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //Central
            crearConveyor("TNS_Central", "Central");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.35";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("Central", "TNE_Central");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_Central", "Metropolitana");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "63";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.35";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Central", "SurOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "154";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.05";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Central", "SurOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "155";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.15";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Central", "NorOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "269";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.10";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //SurOccidente
            crearConveyor("TNS_SurOcci", "SurOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.35";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("SurOccidente", "TNE_SurOcci");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_SurOcci", "NorOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "87";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.30";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_SurOcci", "Central");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "155";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.35";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //NorOccidente
            crearConveyor("TNS_NorOcci", "NorOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.40";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("NorOccidente", "TNE_NorOcci");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_NorOcci", "SurOccidente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "87";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.30";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_NorOcci", "Central");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "269";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.10";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_NorOcci", "Norte");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "145";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.20";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

            //Peten
            crearConveyor("TNS_Peten", "Peten");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.5";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("Peten", "TNE_Peten");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "0";
            crearConveyor("TNS_Peten", "Norte");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "147";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.25";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";
            crearConveyor("TNS_Peten", "NorOriente");
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["LogicalLength"].Value = "282";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["SelectionWeight"].Value = "0.25";
            modelo.Facility.IntelligentObjects["Conveyor" + contConveyor].Properties["InitialDesiredSpeed"].Value = "19.44";

        }

        public void dibujarContorno()
        {
            // --------- FRONTERA BELICE ---------
            crearTransferNode("N" + contNodos, 42000, -30000);
            crearTransferNode("N" + contNodos, 30000, -30000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 34000);

            crearTransferNode("N" + contNodos, 30000, -80000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 232000);

            crearEntityAvion("AvionArmada", 29995, -80007);
            crearSource("Armada", 29998.75, -80010);
            cambiarPropiedad("Armada", "EntityType", "AvionArmada");
            cambiarPropiedad("Armada", "MaximumArrivals", "15");
            cambiarPropiedad("Armada", "InterarrivalTime", "15");
            crearPathSourceNodo("Armada", "N" + (contNodos - 1));

            // --------- FRONTERA MEXICO ---------
            crearTransferNode("N" + contNodos, -20000, -80000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 210000);

            crearTransferNode("N" + contNodos, -20000, -60000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 83000);

            crearTransferNode("N" + contNodos, -41000, -60000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 57000);

            crearTransferNode("N" + contNodos, -27000, -49000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 79000);

            crearTransferNode("N" + contNodos, -11000, -40000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 86000);

            crearTransferNode("N" + contNodos, -11250, -30000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 46000);

            crearTransferNode("N" + contNodos, -52000, -30000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 151000);

            crearTransferNode("N" + contNodos, -63750, -7500);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 125000);

            crearTransferNode("N" + contNodos, -59000, 2000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 40000);

            crearTransferNode("N" + contNodos, -66000, 18000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 85000);

            // --------- FRONTERA PACIFICO ---------
            crearTransferNode("N" + contNodos, -54000, 36000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 60000);

            crearTransferNode("N" + contNodos, -30000, 43000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 75000);

            crearTransferNode("N" + contNodos, -3000, 40000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 63000);

            crearTransferNode("N" + contNodos, 22500, 42500);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 56000);

            // --------- FRONTERA EL SALVADOR ---------
            crearTransferNode("N" + contNodos, 40000, 16250);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 203000);

            // --------- FRONTERA HONDURAS ---------
            crearTransferNode("N" + contNodos, 50000, 4000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 63000);

            crearTransferNode("N" + contNodos, 47000, -12000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 55000);

            crearTransferNode("N" + contNodos, 60000, -29000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 138000);

            // --------- FRONTERA CARIBE ---------
            crearTransferNode("N" + contNodos, 48500, -26000);
            crearConveyor("N" + (contNodos - 2), "N" + (contNodos - 1));
            distanciaConveyor("Conveyor" + contConveyor, 96000);

            crearConveyor("N" + (contNodos - 1), "N" + (contNodos - 22));
            distanciaConveyor("Conveyor" + contConveyor, 52000);
        }

        public void distanciaConveyor(string nombre, int distancia)
        {
            modelo.Facility.IntelligentObjects[nombre].Properties["DrawnToScale"].Value = "False";
            modelo.Facility.IntelligentObjects[nombre].Properties["LogicalLength"].Value = distancia.ToString();
            modelo.Facility.IntelligentObjects[nombre].Properties["InitialDesiredSpeed"].Value = "37.2823"; // Mts por seg
        }
    }
}
