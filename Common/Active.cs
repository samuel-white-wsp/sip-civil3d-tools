﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_Civil3D_Tools
{
    public static class Active
    {
        public static Document Document => Application.DocumentManager.MdiActiveDocument;
        public static Editor Editor => Document.Editor;
        public static Database Database => Document.Database;

        public static void UsingTransaction(Action<Transaction> action)
        {
            using (var transaction = Active.Database.TransactionManager.StartTransaction())
            {
                action(transaction);
                transaction.Commit();
            }
        }

        public static void Write(string str)
        {
            Editor.WriteMessage(str);
        }
    }
}
