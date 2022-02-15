using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.ComponentModel;
using Autodesk.AutoCAD.Colors;
//using Autodesk.Consulting.Civil3D.Barriers;

using Autodesk.Civil.Runtime;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil;

namespace SIP_Civil3D_Tools
{
    static class BarrierTools
    {
        //public static void BarrierTest()
        //{
        //    Autodesk.Consulting.Civil3D.Barriers.Commands.barrierWindow = new Autodesk.Consulting.Civil3D.Barriers.Barriers();
        //    Autodesk.Consulting.Civil3D.Barriers.Commands.barrierWindow.ShowDialog();


        //}


    }

    class Barrier
    {
        public string id;
        public Alignment alignment;

        private double _startChainage;
        public double startChainage
        {
            get { return _startChainage; }
            set { _startChainage = Math.Max(0, value); }
        }

        private double _endChainage;
        public double endChainage
        {
            get { return _endChainage; }
            set
            {
                if (alignment != null)
                {
                    _endChainage = Math.Min(alignment.Length, value);
                }
                else
                {
                    _endChainage = 0;
                }
            }
        }

        public string barrierType;
        public string startTerminal;
        public string endTerminal;
        public double postSpacing;
        public string layer;
        public DBObjectCollection _dbObjectCollection;

    }
}
