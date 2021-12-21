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
        
        [CommandMethod("CopyObjects")]
        public void CopyObjects()
        {
            PromptSelectionResult selection = Active.Editor.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            Active.UsingTransaction(tr =>
            {
                var ids = new ObjectIdCollection(selection.Value.GetObjectIds());
                ObjectHelper.CopyObjects(tr, ids);

            });

        }

        [CommandMethod("ExecuteCommand")]
        public void ExecuteCommand()
        {
            var ed = Active.Editor;
            ed.Command("QSELECT");
        }

    }
}