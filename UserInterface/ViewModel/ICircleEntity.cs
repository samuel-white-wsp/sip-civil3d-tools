using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_Civil3D_Tools.UserInterface.ViewModel
{
	/// <summary>
	/// Represents an abstraction of a circle entity.
	/// </summary>
	public interface ICircleEntity
	{
		/// <summary>
		/// Gets or sets the radius of the circle.
		/// </summary>
		/// <value>
		/// The radius.
		/// </value>
		double Radius { get; set; }
	}
}
