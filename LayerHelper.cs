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

namespace SIP_Civil3D_Tools
{
    static class LayerHelper
    {
        public static LayerTable GetLayerTable(Transaction tr, OpenMode openMode = OpenMode.ForRead)
        {
            return tr.GetObject(Active.Database.LayerTableId, openMode) as LayerTable;
        }
        public static void CreateLayer(Transaction tr, string layerName)
        {
            var layerTable = GetLayerTable(tr);

            if (layerTable.Has(layerName) == false)
            {
                LayerTableRecord layerTableRecord = new LayerTableRecord();
                layerTableRecord.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);
                layerTableRecord.Name = layerName;
                //Upgrade the layer table for write
                layerTableRecord.UpgradeOpen();
                layerTable.Add(layerTableRecord);
                tr.AddNewlyCreatedDBObject(layerTableRecord, true);

            }
        }
    }
}
