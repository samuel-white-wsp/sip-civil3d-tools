using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_Civil3D_Tools.UserInterface.ViewModel
{
	/// <summary>
	/// Represents an abstraction of the host application (AutOCAD).
	/// </summary>
	public interface IHostApplication
	{
		/// <summary>
		/// Occurs when a single circle is selected.
		/// </summary>
		event EventHandler<CircleSelectedEventArgs> CircleSelected;
	}
}
