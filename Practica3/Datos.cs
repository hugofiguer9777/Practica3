using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimioAPI.Graphics;
using Simio;
using Simio.SimioEnums;
using SimioAPI;
using System.Windows.Forms;

namespace Practica3
{
    class Datos
    {
        private ISimioProject proyectoApi;
        private string rutabase = "[MYS1]ModeloBase_P20.spfx";
        private string rutafinal = "[MYS1]ModeloDatos_P20.spfx";
        private string[] warnings;
        int contNodos = 1;
        int contPaths = 0;
        private IModel modelo;
        private IIntelligentObjects intelligentObjects;

        public Datos()
        {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            modelo = proyectoApi.Models[1];
            intelligentObjects = modelo.Facility.IntelligentObjects;
        }

        public void crearModelo()
        {
            dibujarCarnet1();
            dibujarCarnet2();
            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
        }

        public void crearObjeto(string tipo, double x, double y)
        {
            intelligentObjects.CreateObject(tipo, new FacilityLocation(x, 0, y));
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

        public void dibujarCarnet1()
        {
            // 201503840
            dibujarDos(0, 0);       // 2
            dibujarCero(2, 0);      // 0
            dibujarUno(4, 0);       // 1
            dibujarCinco(5, 0);     // 5
            dibujarCero(7, 0);      // 0
            dibujarTres(9, 0);      // 3
            dibujarOcho(11, 0);     // 8
            dibujarCuatro(13, 0);   // 4
            dibujarCero(15, 0);     // 0
        }

        public void dibujarCarnet2()
        {
            // 201503840
            dibujarDos(0, 3);       // 2
            dibujarCero(2, 3);      // 0
            dibujarUno(4, 3);       // 1
            dibujarCinco(5, 3);     // 5
            dibujarCero(7, 3);      // 0
            dibujarTres(9, 3);      // 3
            dibujarSiete(11, 3);    // 7
            dibujarCinco(13, 3);    // 5
            dibujarCero(15, 3);     // 0
        }

        public void dibujarDos(int despX, int despY)
        {
            // ------- 2 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
        }

        public void dibujarCero(int despX, int despY)
        {
            // ------- 0 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearPath("N" + (contNodos - 1), "N" + (contNodos - 4));
        }

        public void dibujarUno(int despX, int despY)
        {
            // ------- 1 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 0 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
        }

        public void dibujarCinco(int despX, int despY)
        {
            // ------- 5 -------
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
        }

        public void dibujarTres(int despX, int despY)
        {
            // ------- 3 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 3), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
        }

        public void dibujarSiete(int despX, int despY)
        {
            // ------- 7 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
        }

        public void dibujarOcho(int despX, int despY)
        {
            // ------- 8 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 0 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearPath("N" + (contNodos - 1), "N" + (contNodos - 6));
            crearPath("N" + (contNodos - 1), "N" + (contNodos - 4));
        }

        public void dibujarCuatro(int despX, int despY)
        {
            // ------- 4 -------
            crearTransferNode("N" + contNodos, 0 + despX, 0 + despY);
            crearTransferNode("N" + contNodos, 0 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 1 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 2 + despY);
            crearPath("N" + (contNodos - 2), "N" + (contNodos - 1));
            crearTransferNode("N" + contNodos, 1 + despX, 0 + despY);
            crearPath("N" + (contNodos - 1), "N" + (contNodos - 3));
        }
    }
}
