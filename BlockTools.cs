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

                if (acBlkTbl.Has(blockName))
                {
                    blkRecId = acBlkTbl[blockName];

                    // Insert the block into the current space
                    if (blkRecId != ObjectId.Null)
                    {
                        using (BlockReference acBlkRef = new BlockReference(position, blkRecId))
                        {
                            BlockTableRecord acCurSpaceBlkTblRec;
                            acCurSpaceBlkTblRec = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                            acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                            tr.AddNewlyCreatedDBObject(acBlkRef, true);
                        }
                    }
                }
                else
                {
                    Active.Editor.WriteMessage("\nNo block found");
                }
            }
            );

        }
        
        public static void InsertingABlock(int radius=1)
        {
            //http://docs.autodesk.com/ACD/2014/ENU/index.html?url=files/GUID-2656E245-6EAA-41A3-ABE9-742868182821.htm,topicNumber=d30e720423
            // Get the current database and start a transaction
            Database acCurDb;
            acCurDb = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                ObjectId blkRecId = ObjectId.Null;

                if (!acBlkTbl.Has("CircleBlock"))
                {
                    using (BlockTableRecord acBlkTblRec = new BlockTableRecord())
                    {
                        acBlkTblRec.Name = "CircleBlock";

                        // Set the insertion point for the block
                        acBlkTblRec.Origin = new Point3d(0, 0, 0);

                        // Add a circle to the block
                        using (Circle acCirc = new Circle())
                        {
                            acCirc.Center = new Point3d(0, 0, 0);
                            acCirc.Radius = radius;

                            acBlkTblRec.AppendEntity(acCirc);

                            acBlkTbl.UpgradeOpen();
                            acBlkTbl.Add(acBlkTblRec);
                            acTrans.AddNewlyCreatedDBObject(acBlkTblRec, true);
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
                        acCurSpaceBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                        acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                        acTrans.AddNewlyCreatedDBObject(acBlkRef, true);
                    }
                }

                // Save the new object to the database
                acTrans.Commit();

                // Dispose of the transaction
            }
        }
    }
}
