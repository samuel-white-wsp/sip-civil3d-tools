using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_Civil3D_Tools.UserInterface.ViewModel
{
	/// <summary>
	/// Contains the event data for the <see cref="IHostApplication.CircleSelected"/> event.
	/// </summary>
	public class CircleSelectedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the circle that was selected.
		/// </summary>
		/// <value>
		/// The circle.
		/// </value>
		public ICircleEntity TheCircle { get; set; }
	}
}
