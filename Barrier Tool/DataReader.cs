using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBObject = Autodesk.AutoCAD.DatabaseServices.DBObject;

namespace SIP_Civil3D_Tools.Barrier_Tool
{
    class DataReader
    {
        public DataContainer Extract(Database acCurDb, Validations validations)
        {
            DataContainer dataContainer = new DataContainer();
            using (Transaction transaction = acCurDb.TransactionManager.StartTransaction())
            {
                foreach (ObjectId objectId in (BlockTableRecord)transaction.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(acCurDb), (OpenMode)0))
                {
                    
                    DBObject record = transaction.GetObject(objectId, (OpenMode)0);
                    if (record is BlockReference)
                        dataContainer.Add(record);
                    if (record is Alignment)
                        dataContainer.Add(record);
                }
                dataContainer._blockPostList = ((IEnumerable<BlockReference>)dataContainer._blockPostList).GroupBy<BlockReference, string>((Func<BlockReference, string>)(i => i.Name)).Select<IGrouping<string, BlockReference>, BlockReference>((Func<IGrouping<string, BlockReference>, BlockReference>)(j => ((IEnumerable<BlockReference>)j).First<BlockReference>())).ToList<BlockReference>();
                dataContainer._blockTerminalList = ((IEnumerable<BlockReference>)dataContainer._blockTerminalList).GroupBy<BlockReference, string>((Func<BlockReference, string>)(i => i.Name)).Select<IGrouping<string, BlockReference>, BlockReference>((Func<IGrouping<string, BlockReference>, BlockReference>)(j => ((IEnumerable<BlockReference>)j).First<BlockReference>())).ToList<BlockReference>();
                foreach (ObjectId objectId in (SymbolTable)(transaction.GetObject(acCurDb.LayerTableId, (OpenMode)0) as LayerTable))
                {
                    LayerTableRecord record = transaction.GetObject(objectId, (OpenMode)0) as LayerTableRecord;
                    dataContainer.Add((DBObject)record);
                }
            }
            return dataContainer;
        }

        internal class DataContainer
        {
            public List<Alignment> _alignmentList = new List<Alignment>();
            public List<BlockReference> _blockPostList = new List<BlockReference>();
            public List<BlockReference> _blockTerminalList = new List<BlockReference>();
            public List<LayerTableRecord> _layerList = new List<LayerTableRecord>();

            public void Add(DBObject record)
            {
                if (record is Alignment)
                    this._alignmentList.Add((Alignment)record);
                if (record is BlockReference)
                {
                    BlockReference blockReference = (BlockReference)record;
                    if (blockReference.Name.ToUpper().Contains("POST"))
                        this._blockPostList.Add((BlockReference)record);
                    else if (blockReference.Name.ToUpper().Contains("TERM"))
                        this._blockTerminalList.Add((BlockReference)record);
                }
                if (!(record is LayerTableRecord))
                    return;
                this._layerList.Add((LayerTableRecord)record);
            }

        }

        internal class Validations
        {
            private readonly List<Validations.Validation> _validations = new List<Validations.Validation>();

            public void Add(string message, ErrorLevel level) => this._validations.Add(new Validations.Validation()
            {
                Level = level,
                Message = message
            });

            public bool HasErrors => this._validations.Any<Validations.Validation>((Func<Validations.Validation, bool>)(v => v.Level == ErrorLevel.Error));

            public string GetAllMessages()
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Validations.Validation validation in this._validations)
                {
                    stringBuilder.Append(validation.Level.ToString() + ": " + validation.Message);
                    stringBuilder.AppendLine();
                }
                return stringBuilder.ToString();
            }

            private class Validation
            {
                public ErrorLevel Level;
                public string Message;
            }
        }

        public enum ErrorLevel
        {
            Error,
        }
    }
}
