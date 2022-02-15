using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace SIP_Civil3D_Tools
{
    static class BlockTools
    {

        public static void InsertBlock(string blockName, Point3d position)
        {
            var db = Active.Database;

            Active.UsingTransaction(tr =>
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                ObjectId blkRecId = ObjectId.Null;

                if (!acBlkTbl.Has(blockName))
                {
                    try
                    {
                        // search for a dwg file named 'blockName' in AutoCAD search paths
                        var filename = HostApplicationServices.Current.FindFile(blockName + ".dwg", db, FindFileHint.Default);
                        // add the dwg model space as 'blockName' block definition in the current database block table
                        using (var sourceDb = new Database(false, true))
                        {
                            sourceDb.ReadDwgFile(filename, FileOpenMode.OpenForReadAndAllShare, true, "");
                            db.Insert(blockName, sourceDb, true);
                        }
                    }
                    catch
                    {
                        Active.Editor.WriteMessage($"\nBlock '{blockName}' not found.");
                        return;
                    }
                }


                // create a new block reference
                using (var br = new BlockReference(position, acBlkTbl[blockName]))
                {
                    var space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    space.AppendEntity(br);
                    tr.AddNewlyCreatedDBObject(br, true);
                }
            }
            );

        }
        
        public static void InsertCircleBlock(Transaction tr, Point3d location, double radius=1)
        {
            //http://docs.autodesk.com/ACD/2014/ENU/index.html?url=files/GUID-2656E245-6EAA-41A3-ABE9-742868182821.htm,topicNumber=d30e720423
            // Get the current database and start a transaction
            Database acCurDb;
            acCurDb = Application.DocumentManager.MdiActiveDocument.Database;

            
            // Open the Block table for read
            BlockTable acBlkTbl;
            acBlkTbl = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

            ObjectId blkRecId = ObjectId.Null;

            if (!acBlkTbl.Has("CircleBlock"))
            {
                using (BlockTableRecord acBlkTblRec = new BlockTableRecord())
                {
                    acBlkTblRec.Name = "CircleBlock";

                    // Set the insertion point for the block
                    acBlkTblRec.Origin = new Point3d(0,0,0);

                    // Add a circle to the block
                    using (Circle acCirc = new Circle())
                    {
                        acCirc.Center = location;
                        acCirc.Radius = radius;

                        acBlkTblRec.AppendEntity(acCirc);

                        acBlkTbl.UpgradeOpen();
                        acBlkTbl.Add(acBlkTblRec);
                        tr.AddNewlyCreatedDBObject(acBlkTblRec, true);
                    }

                    blkRecId = acBlkTblRec.Id;
                }
            }
            else
            {
                blkRecId = acBlkTbl["CircleBlock"];
            }

            // Insert the block into the current space
            if (blkRecId != ObjectId.Null)
            {
                using (BlockReference acBlkRef = new BlockReference(new Point3d(0, 0, 0), blkRecId))
                {
                    BlockTableRecord acCurSpaceBlkTblRec;
                    acCurSpaceBlkTblRec = tr.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                    tr.AddNewlyCreatedDBObject(acBlkRef, true);
                }

            }
        }
    }
}
