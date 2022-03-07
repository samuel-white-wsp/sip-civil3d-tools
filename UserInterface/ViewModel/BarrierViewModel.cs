using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SIP_Civil3D_Tools.Barrier_Tool;
using static SIP_Civil3D_Tools.Barrier_Tool.DataReader;
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

using System.Windows.Input;

namespace SIP_Civil3D_Tools.UserInterface.ViewModel
{
    public class BarrierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Barrier> barrierCollection { get; set; }
        

        public ObservableCollection<Alignment> alignmentsCollection
        {
            get
            {
                CivilDocument activeDocument = CivilApplication.ActiveDocument;
                Document mdiActiveDocument = Application.DocumentManager.MdiActiveDocument;
                Editor editor = mdiActiveDocument.Editor;
                DataReader dataReader = new DataReader();
                Validations validations1 = new Validations();
                Database database = mdiActiveDocument.Database;
                Validations validations2 = validations1;
                DataContainer dataContainer = dataReader.Extract(database, validations2);
                List<Alignment> alignmentList = dataContainer._alignmentList;
                return new ObservableCollection<Alignment>(alignmentList);
            }
        }

        public ObservableCollection<string> barrierTypes { get; set; }
        public ObservableCollection<string> terminalBlockNames { get; set; }

        ObservableCollection<string> _layerNameCollection;
        

        public ObservableCollection<string> layerNamesCollection
        {
            get
            {
                List<string> layerNames = LayerHelper.GetLayerList();
                _layerNameCollection = new ObservableCollection<string>(layerNames);

                return _layerNameCollection;
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private class Updater : ICommand
        {
            #region ICommand Members  

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

            }

            #endregion
        }

    }


}
