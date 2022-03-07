using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using SIP_Civil3D_Tools.Common;
using SIP_Civil3D_Tools.UserInterface.View;

namespace SIP_Civil3D_Tools.UserInterface.AcadHost
{
	public class MainApp : IExtensionApplication
	{
		private static PaletteSet _paletteSet;

		public void Initialize()
		{
			if (_paletteSet == null)
			{
				var ctrl = new UserControl1();
				var barr = new BarrierTool();

				_paletteSet = new PaletteSet("WPF Demo")
								  {
									  {
										  "Circle Radius",
										  //new ElementHost {AutoSize = true, Dock = DockStyle.Fill, Child = ctrl}
										  new ElementHost {AutoSize = true, Dock = DockStyle.Fill, Child = barr}
									  }
								  };

				ctrl.HostApplication = new HostApplication();
			}
			_paletteSet.Visible = true;
		}

		public void Terminate()
		{
		}
	}
}
