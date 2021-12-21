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
                ObjectHelper.CopyObjects(tr, ids, newLayer);

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
    }
}