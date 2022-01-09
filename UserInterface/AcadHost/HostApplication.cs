using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using SIP_Civil3D_Tools.UserInterface.ViewModel;

namespace SIP_Civil3D_Tools.UserInterface.AcadHost
{
	public class HostApplication : IHostApplication
	{
		private Document _activeDocument;

		public HostApplication()
		{
			Application.DocumentManager.DocumentBecameCurrent += (s, e) => ActiveDocument = e.Document;
			ActiveDocument = Application.DocumentManager.MdiActiveDocument;
		}

		/// <summary>
		/// Gets or sets the active document.
		/// </summary>
		public Document ActiveDocument
		{
			get { return _activeDocument; }
			set
			{
				if (_activeDocument != null)
					_activeDocument.ImpliedSelectionChanged -= ImpliedSelectionChanged;

				_activeDocument = value;

				if (_activeDocument != null)
					_activeDocument.ImpliedSelectionChanged += ImpliedSelectionChanged;
			}
		}

		/// <summary>
		/// Occurs when a circle is selected.
		/// </summary>
		public event EventHandler<CircleSelectedEventArgs> CircleSelected;

		private void ImpliedSelectionChanged(object sender, EventArgs e)
		{
			PromptSelectionResult result = _activeDocument.Editor.SelectImplied();
			if (result != null && result.Status == PromptStatus.OK && result.Value.Count == 1)
			{
				ObjectId objectId = result.Value[0].ObjectId;
				if (objectId.ObjectClass.IsDerivedFrom(RXObject.GetClass(typeof(Circle))))
				{
					RaiseCircleSelected(objectId);
					return;
				}
			}

			RaiseCircleSelected(ObjectId.Null);
		}

		/// <summary>
		/// Raises the circle selected event.
		/// </summary>
		/// <param name="objectId">The object id.</param>
		private void RaiseCircleSelected(ObjectId objectId)
		{
			if (CircleSelected != null)
			{
				CircleSelected(this, new CircleSelectedEventArgs { TheCircle = objectId.IsNull ? null : new CircleEntity(objectId) });
			}
		}
	}
}