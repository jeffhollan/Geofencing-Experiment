using System;
using System.Collections.Generic;
using System.Diagnostics;
using Geofencing.Task;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Geofencing
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		
		private Geolocator geolocator;
		private CoreDispatcher _cd;
		public MainPage()
		{
			this.InitializeComponent();
			_cd = Window.Current.CoreWindow.Dispatcher;
			Initialize();

			this.NavigationCacheMode = NavigationCacheMode.Required;


		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{

			// TODO: Prepare page for display here.

			// TODO: If your application contains multiple pages, ensure that you are
			// handling the hardware Back button by registering for the
			// Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
			// If you are using the NavigationHelper provided by some templates,
			// this event is handled for you.

			
			
        }




		async private void Initialize()
		{
			

			// Regardless of the answer, register the background task. If the user later adds this application
			// to the lock screen, the background task will be ready to run.

			try
			{
				// Get a geolocator object 
				geolocator = new Geolocator();

				// Get cancellation token
				var cts = new CancellationTokenSource();
				CancellationToken token = cts.Token;

				await geolocator.GetGeopositionAsync().AsTask(token);


			}
			catch (UnauthorizedAccessException)
			{


			}

			catch (Exception)
			{
				if (geolocator.LocationStatus == PositionStatus.Disabled)
				{

				}

			}


		}


		public static IList<Geofence> GetGeofences()
		{
			return GeofenceMonitor.Current.Geofences;
		}

		public static void CreateGeofence(string id, double lat, double lon, double radius)
		{
			if (GeofenceMonitor.Current.Geofences.SingleOrDefault(g => g.Id == id) != null)
			{
				Debug.WriteLine("Already exists");
				return;
			}
			var position = new BasicGeoposition();
			position.Latitude = lat;
			position.Longitude = lon;

			var geocircle = new Geocircle(position, radius);

			MonitoredGeofenceStates mask = 0;
			mask |= MonitoredGeofenceStates.Entered;
			mask |= MonitoredGeofenceStates.Exited;

			TimeSpan ts = new TimeSpan(0, 0, 1);
			if (id == "Travel1")
			{
				Debug.WriteLine("I'm travel 1 son!");
				ts = new TimeSpan(0, 0, 0, 1);

			}
			var geofence = new Geofence(id, geocircle, mask, false, ts);
			GeofenceMonitor.Current.Geofences.Add(geofence);
		}

		public static void RemoveGeofence(string id)
		{
			var geofence = GeofenceMonitor.Current.Geofences.SingleOrDefault(g => g.Id == id);

			if (geofence != null)
				GeofenceMonitor.Current.Geofences.Remove(geofence);
		}

		private async void addGeo_Click(object sender, RoutedEventArgs e)
		{
			//Geolocator geolocater = new Geolocator();
			//geolocater.DesiredAccuracyInMeters = 50;

			//try
			//{
			//	Geoposition geoposition = await geolocater.GetGeopositionAsync(
			//		maximumAge: TimeSpan.FromMinutes(5),
			//		timeout: TimeSpan.FromSeconds(10)
			//		);



			//	Debug.WriteLine("GPS: {0} , {1}", geoposition.Coordinate.Point.Position.Latitude.ToString("0.00"), geoposition.Coordinate.Point.Position.Longitude.ToString("0.00"));
			//	CreateGeofence("Geofence1", geoposition.Coordinate.Point.Position.Latitude, geoposition.Coordinate.Point.Position.Longitude, 150);
			//	GeoTask.Register();
			//}
			//catch (Exception ex)
			//{
			//	Debug.WriteLine(ex.ToString());
			//}

			CreateGeofence("Home1", 47.533462, -121.854213, 80);
		}

		private void listGeo_Click(object sender, RoutedEventArgs e)
		{
			string toRet = "Geofences:";
			foreach (Geofence g in GetGeofences())
			{
				toRet += "\n";
				toRet += String.Format("Id: {0}", g.Id);
			}
			infoTextBlock.Text = toRet;


		}

		private void clearGeo_Click(object sender, RoutedEventArgs e)
		{
			RemoveGeofence("Geofence1");
			RemoveGeofence("Home1");
			RemoveGeofence("Work1");
			RemoveGeofence("Travel1");
		}

		private void workGeo_Click(object sender, RoutedEventArgs e)
		{
			CreateGeofence("Work1", 47.676815, -122.095711, 80);
		}

		private void travelGeo_Click(object sender, RoutedEventArgs e)
		{
			CreateGeofence("Travel1", 47.536530, -121.832306, 300);
		}

		private void regTask_Click(object sender, RoutedEventArgs e)
		{
			GeoTask.Register();
		}

		private void unRegTask_Click(object sender, RoutedEventArgs e)
		{
			GeoTask.Unregister();
		}
	}
}
