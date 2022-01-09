using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SIP_Civil3D_Tools.UserInterface.ViewModel
{
	/// <summary>
	/// Represents an abstraction of the view. Provides the control logic behind the user interface.
	/// </summary>
	public class UserControlViewModel : INotifyPropertyChanged
	{
		private IHostApplication _hostApplication;
		private ICircleEntity _selectedCircle;
		private bool _isEnabled;
		private double _radius;
		private double _minRadius;
		private double _maxRadius;
		ObservableCollection<string> _layerNameCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserControlViewModel"/> class.
		/// </summary>
		public UserControlViewModel()
		{
			_isEnabled = false;
		}

		/// <summary>
		/// Gets or sets the host application.
		/// </summary>
		/// <value>
		/// The host application.
		/// </value>
		public IHostApplication HostApplication
		{
			get { return _hostApplication; }
			set
			{
				SelectedCircle = null;

				if (_hostApplication != null)
				{
					_hostApplication.CircleSelected -= CircleSelected;
				}

				_hostApplication = value;

				if (_hostApplication != null)
				{
					_hostApplication.CircleSelected += CircleSelected;
				}
			}
		}

		/// <summary>
		/// Handler for the <see cref="IHostApplication.CircleSelected"/> event.
		/// </summary>
		/// <param name="sender">The object that fired the event.</param>
		/// <param name="e">The <see cref="CP2657.WPF.ViewModel.CircleSelectedEventArgs"/> instance containing the event data.</param>
		void CircleSelected(object sender, CircleSelectedEventArgs e)
		{
			SelectedCircle = e.TheCircle;
		}

		/// <summary>
		/// Gets or sets the currently selected circle.
		/// </summary>
		/// <value>
		/// The selected circle.
		/// </value>
		public ICircleEntity SelectedCircle
		{
			get { return _selectedCircle; }
			set
			{
				_selectedCircle = value;
				RaisePropertyChanged("SelectedCircle");

				if (_selectedCircle != null)
				{
					IsEnabled = true;
					_radius = _selectedCircle.Radius;
					RaisePropertyChanged("Radius");
					MinRadius = Radius * 0.5;
					MaxRadius = Radius * 1.5;
				}
				else
				{
					IsEnabled = false;
				}
			}
		}

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Gets or sets a value indicating whether the UI is enabled (a circle is currently selected).
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the UI is enabled; otherwise, <c>false</c>.
		/// </value>
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				RaisePropertyChanged("IsEnabled");
			}
		}

		/// <summary>
		/// Gets or sets the radius of the selected circle.
		/// </summary>
		/// <value>
		/// The radius of the selected circle.
		/// </value>
		public ObservableCollection<string> LayerNameCollection
        {
			get
            {
				List<string> layerNames = LayerHelper.GetLayerList();
				_layerNameCollection = new ObservableCollection<string>(layerNames);

				return _layerNameCollection;

			}
        }


		public double Radius
		{
			get { return _radius; }
			set
			{
				_radius = value;
				RaisePropertyChanged("Radius");
				if (_selectedCircle != null)
					_selectedCircle.Radius = value;
			}
		}

		/// <summary>
		/// Gets or sets the minimum radius on a slider control.
		/// </summary>
		/// <value>
		/// The minimum radius.
		/// </value>
		public double MinRadius
		{
			get { return _minRadius; }
			set
			{
				_minRadius = value;
				RaisePropertyChanged("MinRadius");
			}
		}

		/// <summary>
		/// Gets or sets the maximum radius on a slider control.
		/// </summary>
		/// <value>
		/// The maximum radius.
		/// </value>
		public double MaxRadius
		{
			get { return _maxRadius; }
			set
			{
				_maxRadius = value;
				RaisePropertyChanged("MaxRadius");
			}
		}

		public string testString
        {
			get { return "hello world"; }
        }

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
