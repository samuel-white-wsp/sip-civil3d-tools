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

namespace SIP_Civil3D_Tools
{
    public static class ObjectHelper
    {

        public static Entity GetEntity(Transaction tr, ObjectId objectId, OpenMode openMode = OpenMode.ForRead)
        {
            return (Entity)tr.GetObject(objectId, OpenMode.ForRead);
        }

        public static object GetAcadObject(Transaction tr, Entity ent)
        {
            return ent.AcadObject;
        }
        public static object GetAcadObject(Transaction tr, ObjectId objectId)
        {
            Entity ent = GetEntity(tr, objectId);
            return GetAcadObject(tr, ent);
        }

        public static PropertyDescriptorCollection GetObjectProperties(object acadObj)
        {
            return TypeDescriptor.GetProperties(acadObj);
        }
        public static PropertyDescriptorCollection GetObjectProperties(Transaction tr, Entity ent)
        {
            return TypeDescriptor.GetProperties(GetAcadObject(tr, ent));
        }
        public static PropertyDescriptorCollection GetObjectProperties(Transaction tr, ObjectId objectId)
        {
            return TypeDescriptor.GetProperties(GetAcadObject(tr, objectId));
        }

        public static ObjectIdCollection CopyObjects(Transaction tr, ObjectIdCollection objectIds, string copyLayer = "")
        {

            var idMap = new IdMapping();
            var db = Active.Database;

            db.DeepCloneObjects(objectIds, db.CurrentSpaceId, idMap, false);

            ObjectIdCollection objectIdCollection = new ObjectIdCollection();


            foreach (IdPair pair in idMap)
            {
                if (pair.IsCloned)
                {
                    var cloned = tr.GetObject(
                        pair.Value, OpenMode.ForRead) as Entity;
                    
                   
                    if (cloned != null)
                    {
                        cloned.UpgradeOpen();

                        
                        if (copyLayer != "")
                        {
                            if (LayerHelper.LayerExists(tr, copyLayer) == false)
                            {
                                LayerHelper.CreateLayer(tr, copyLayer);
                            }

                            cloned.Layer = copyLayer;
                        }

                        objectIdCollection.Add(cloned.ObjectId);
                    }
                }
            }


            return objectIdCollection;
          

            
            //Entity ent = GetEntity(tr, objectId);
            //var newObjectId = ent.Clone();
            //return newObjectId;
        }

        public static ObjectId CopyObjects(Transaction tr, ObjectId objectId, string newLayer = "")
        {
            ObjectIdCollection objectIdCollection = new ObjectIdCollection();
            objectIdCollection.Add(objectId);
            return CopyObjects(tr, objectIdCollection, newLayer)[0];

        }

            public static void PrintProperties(Transaction tr, ObjectId objectId)
        {
            object acadObj = GetAcadObject(tr, objectId);
            var props = ObjectHelper.GetObjectProperties(acadObj);

            foreach (PropertyDescriptor prop in props)
            {
                object value = prop.GetValue(acadObj);
                if (value != null)
                {
                    Active.Editor.WriteMessage("\n{0} = {1}", prop.DisplayName, value.ToString());
                }
            }
        }
    }
}
