using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using System.Threading.Tasks;
using SIP_Civil3D_Tools.UserInterface.ViewModel;
using SIP_Civil3D_Tools.Common;

namespace SIP_Civil3D_Tools.UserInterface.AcadHost
{
	/// <summary>
	/// Implementation of <see cref="ICircleEntity"/> that wraps an AutoCAD <c>Circle</c> object.
	/// </summary>
	public class CircleEntity : ICircleEntity
	{
		private readonly ObjectId _objectId;

		/// <summary>
		/// Initializes a new instance of the <see cref="CircleEntity"/> class.
		/// </summary>
		/// <param name="objectId">The object id.</param>
		public CircleEntity(ObjectId objectId)
		{
			_objectId = objectId;
		}

		/// <summary>
		/// Gets or sets the radius of the circle.
		/// </summary>
		public double Radius
		{
			get
			{
				return _objectId.GetValue<Circle, double>(c => c.Radius);
			}
			set
			{
				Active.Document.OpenAs<Circle>(_objectId, OpenMode.ForWrite, c => c.Radius = value);
				Active.Editor.UpdateScreen();
			}
		}
	}
}