using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SIP_Civil3D_Tools.UserInterface.ViewModel;


namespace SIP_Civil3D_Tools.UserInterface.View
{
    /// <summary>
    /// Interaction logic for BarrierTool.xaml
    /// </summary>
    public partial class BarrierTool : UserControl
    {
        public BarrierTool()
        {
            InitializeComponent();
            DataContext = new BarrierViewModel();
        }
    }
}
