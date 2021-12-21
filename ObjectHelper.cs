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

        public static void CopyObjects(Transaction tr, ObjectIdCollection objectIds)
        {

            var idMap = new IdMapping();
            var db = Active.Database;
            db.DeepCloneObjects(objectIds, db.CurrentSpaceId, idMap, false);
            

            
            //Entity ent = GetEntity(tr, objectId);
            //var newObjectId = ent.Clone();
            //return newObjectId;
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
