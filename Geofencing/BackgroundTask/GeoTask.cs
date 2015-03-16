using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;

namespace Geofencing.Task
{
	public sealed class GeoTask : IBackgroundTask
	{
		static string TaskName = "MyLocationTask";

		public async static void Register()
		{
			if (!IsTaskRegistered())
			{
				var result = await BackgroundExecutionManager.RequestAccessAsync();
				var builder = new BackgroundTaskBuilder();

				builder.Name = TaskName;
				builder.TaskEntryPoint = typeof(GeoTask).FullName;
				builder.SetTrigger(new LocationTrigger(LocationTriggerType.Geofence));

				builder.Register();

			}
		}

		public static void Unregister()
		{
			var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);
			if (entry.Value != null)
			{
				entry.Value.Unregister(true);
			}
		}

		public static bool IsTaskRegistered()
		{
			var taskRegistered = false;
			var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);

			if (entry.Value != null)
				taskRegistered = true;

			return taskRegistered;
		}
		public void Run(IBackgroundTaskInstance taskInstance)
		{
			Debug.WriteLine("Background triggered.");
			var reports = GeofenceMonitor.Current.ReadReports();
			var report = reports.FirstOrDefault(r => (r.Geofence.Id == "Home1") && (r.NewState == GeofenceState.Entered));

			if (report == null) return;

			var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
			var txtNodes = toastXmlContent.GetElementsByTagName("text");
			txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Home1 Toast Entered!"));
			txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

			var toast = new ToastNotification(toastXmlContent);
			var toastNotifier = ToastNotificationManager.CreateToastNotifier();
			toastNotifier.Show(toast);


			report = reports.FirstOrDefault(r => (r.Geofence.Id == "Work1") && (r.NewState == GeofenceState.Entered));

			if (report == null) return;

			toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
			txtNodes = toastXmlContent.GetElementsByTagName("text");
			txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Work1 Toast Entered!"));
			txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

			toast = new ToastNotification(toastXmlContent);
			toastNotifier = ToastNotificationManager.CreateToastNotifier();
			toastNotifier.Show(toast);


			report = reports.FirstOrDefault(r => (r.Geofence.Id == "Travel1") && (r.NewState == GeofenceState.Entered));

			if (report == null) return;

			toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
			txtNodes = toastXmlContent.GetElementsByTagName("text");
			txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Travel1 Toast Entered!"));
			txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

			toast = new ToastNotification(toastXmlContent);
			toastNotifier = ToastNotificationManager.CreateToastNotifier();
			toastNotifier.Show(toast);


			report = reports.FirstOrDefault(r => (r.Geofence.Id == "Work1") && (r.NewState == GeofenceState.Exited));

			if (report == null) return;

			toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
			txtNodes = toastXmlContent.GetElementsByTagName("text");
			txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Work1 Toast Exited!"));
			txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

			toast = new ToastNotification(toastXmlContent);
			toastNotifier = ToastNotificationManager.CreateToastNotifier();
			toastNotifier.Show(toast);


			report = reports.FirstOrDefault(r => (r.Geofence.Id == "Travel1") && (r.NewState == GeofenceState.Exited));

			if (report == null) return;

			toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
			txtNodes = toastXmlContent.GetElementsByTagName("text");
			txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Travel1 Toast Exited!"));
			txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

			toast = new ToastNotification(toastXmlContent);
			toastNotifier = ToastNotificationManager.CreateToastNotifier();
			toastNotifier.Show(toast);

			report = reports.FirstOrDefault(r => (r.Geofence.Id == "Home1") && (r.NewState == GeofenceState.Exited));

			if (report == null) return;

			toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
			txtNodes = toastXmlContent.GetElementsByTagName("text");
			txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Home1 Toast Exited!"));
			txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

			toast = new ToastNotification(toastXmlContent);
			toastNotifier = ToastNotificationManager.CreateToastNotifier();
			toastNotifier.Show(toast);



		}
	}
}
