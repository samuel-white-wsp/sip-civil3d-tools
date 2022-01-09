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

        public static string CurrentLayerName(Transaction tr)
        {
            LayerTableRecord ltr = (LayerTableRecord)tr.GetObject(Active.Database.Clayer, OpenMode.ForRead);
            return ltr.Name;
        }

        public static bool LayerExists(Transaction tr, string layerName)
        {
            var layerTable = GetLayerTable(tr);
            return layerTable.Has(layerName);
        }

        public static void CreateLayer(Transaction tr, string layerName)
        {
            var layerTable = GetLayerTable(tr);

            if (layerTable.Has(layerName))
            {
                Active.Write(String.Format("Layer {0} already exists", layerName));
            }
            else
            {
                LayerTableRecord layerTableRecord = new LayerTableRecord();
                layerTableRecord.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);
                layerTableRecord.Name = layerName;
                //Upgrade the layer table for write
                layerTable.UpgradeOpen();
                layerTable.Add(layerTableRecord);
                Active.Write(String.Format("Layer {0} created", layerName));
                tr.AddNewlyCreatedDBObject(layerTableRecord, true);
            }
        }

        public static List<String> GetLayerList()
        {
            List<String> layerNameList = new List<String>();
            Active.UsingTransaction(tr =>
            {
                
                LayerTable lt = GetLayerTable(tr);
                foreach (ObjectId objectId in lt)
                {
                    LayerTableRecord layerTableRecord;
                    layerTableRecord = tr.GetObject(objectId, OpenMode.ForRead) as LayerTableRecord;
                    layerNameList.Add(layerTableRecord.Name);
                }
                
            });
            return layerNameList;
        }
    }
}
