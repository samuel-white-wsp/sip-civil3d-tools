using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.ComponentModel;
using Autodesk.AutoCAD.Geometry;

using Autodesk.Civil.Runtime;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil;

using SIP_Civil3D_Tools.UserInterface.AcadHost;

namespace SIP_Civil3D_Tools
{
    public class Commands
    {
        [CommandMethod("PS_HELLO")]
        public void Hello()
        {
            //var document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            //var editor = document.Editor;

            Active.Editor.WriteMessage("\nHello World!");

        }

        [CommandMethod("InsertingABlock")]
        public void InsertABlock()
        {
            BlockTools.InsertBlock("WSP_Block_Cycle_5m - 800mm", new Autodesk.AutoCAD.Geometry.Point3d(0, 0, 0));
            Active.Editor.WriteMessage("\nInserting block!");
            //BlockTools.InsertingABlock(10);
            //BlockTools bt = new BlockTools();
            //bt.InsertingABlock();
        }

        [CommandMethod("PrintProperties")]
        public void PrintProperties()
        {

            PromptSelectionResult selection = Active.Editor.GetSelection();

            if (selection != null)
            {
                Active.UsingTransaction(tr =>
                {
                    foreach (SelectedObject sel in selection.Value)
                    {
                        Console.WriteLine(sel.ObjectId);

                        ObjectHelper.PrintProperties(tr, sel.ObjectId);
                    }
                });
            }

        }

        //TODO Command for cleaning layer using Overkill

        [CommandMethod("CopyToLayer")]
        public void CopyToLayer()
        {
            PromptSelectionResult selection = Active.Editor.GetSelection();
            PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter layer to copy to: ");

            Active.UsingTransaction(tr =>
            {
                pStrOpts.AllowSpaces = false;
            
                pStrOpts.DefaultValue = LayerHelper.CurrentLayerName(tr);

                PromptResult pStrRes = Active.Editor.GetString(pStrOpts);

                string newLayer = "";
            
                if (pStrRes.Status == PromptStatus.OK)
                {
                    newLayer = pStrRes.StringResult;
                }
          
                if (selection.Status != PromptStatus.OK)
                    return;

            
                var ids = new ObjectIdCollection(selection.Value.GetObjectIds());
                ObjectIdCollection objectId = ObjectHelper.CopyObjects(tr, ids, newLayer);

            });

        }

        [CommandMethod("ExecuteCommand")]
        public void ExecuteCommand()
        {
            var ed = Active.Editor;
            ed.Command("QSELECT");
        }

        [CommandMethod("CreateLayer")]
        public void CreateLayer()
        {
            Active.UsingTransaction(tr =>
            {
                LayerHelper.CreateLayer(tr, "test_layer");

            });

        }

        [CommandMethod("SIPTools")]
        public void SipTools()
        {
            MainApp mainApp = new MainApp();
            mainApp.Initialize();
        }

        [CommandMethod("FindNearestParallel")]
        public void FindNearestParallel()
        {
            GeometryTools.NearestParallelLine();
        }

        [CommandMethod("InsertOffsetBlock")]
        public void InsertOffsetBlock()
        {
            var ed = Active.Editor;

            var doc = Active.Document;

            using (doc.LockDocument())
            {
                Active.UsingTransaction(tr =>
                {

                    PromptEntityOptions entOpts = new PromptEntityOptions("Select an alignment");
                    entOpts.SetRejectMessage("..Not an alignment, try again!");
                    entOpts.AddAllowedClass(typeof(Alignment), true);
                    PromptEntityResult res = ed.GetEntity(entOpts);

                    if (res.Status != PromptStatus.OK)
                    {
                        return;
                    }

                    Alignment alignment = (Alignment)res.ObjectId.GetObject(OpenMode.ForRead);
                    double offset = 2000;
                    double chaingage = alignment.Length / 2;

                    Point3d location = GeometryTools.GetOffsetPoint(tr, alignment, chaingage, offset);
                    BlockTools.InsertCircleBlock(tr, location, 2000);
                });
            }
           
        }

        [CommandMethod("SIPBarrier")]
        public void Barrier()
        {
            BarrierTools.BarrierTest();
        }


    }
}