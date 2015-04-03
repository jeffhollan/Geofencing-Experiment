using RestSharp.Portable;
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
		const string PushBulletAPI = "{PUT API KEY HERE}";



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
        async public void Run(IBackgroundTaskInstance taskInstance)
        {
			BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            var reports = GeofenceMonitor.Current.ReadReports();

            var report = reports.FirstOrDefault(r => (r.Geofence.Id == "Home1") && (r.NewState == GeofenceState.Entered));

            if (report != null)
                await ToastNotify("Home1 Entered", report.Geofence.Id);

            report = reports.FirstOrDefault(r => (r.Geofence.Id == "Work1") && (r.NewState == GeofenceState.Entered));

            if (report != null)
                await ToastNotify("Work1 Entered", report.Geofence.Id);
        
            report = reports.FirstOrDefault(r => (r.Geofence.Id == "Travel1") && (r.NewState == GeofenceState.Entered));

            if (report != null)
                await ToastNotify("Travel1 Entered", report.Geofence.Id);

            report = reports.FirstOrDefault(r => (r.Geofence.Id == "Work1") && (r.NewState == GeofenceState.Exited));

            if (report != null)
                await ToastNotify("Work1 Exited", report.Geofence.Id);

            report = reports.FirstOrDefault(r => (r.Geofence.Id == "Travel1") && (r.NewState == GeofenceState.Exited));

            if (report != null)
                await ToastNotify("Travel1 Exited", report.Geofence.Id);

            report = reports.FirstOrDefault(r => (r.Geofence.Id == "Home1") && (r.NewState == GeofenceState.Exited));

            if (report != null)
                await ToastNotify("Home1 Exited", report.Geofence.Id);

			deferral.Complete();

        }

        async private System.Threading.Tasks.Task ToastNotify(string txt, string geoId)
        {
			await PushBullet(txt, geoId);
            var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var txtNodes = toastXmlContent.GetElementsByTagName("text");
            txtNodes[0].AppendChild(toastXmlContent.CreateTextNode(txt));
            txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(geoId));

            var toast = new ToastNotification(toastXmlContent);
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
        }

		async private System.Threading.Tasks.Task PushBullet(string title, string body)
		{
			var client = new RestClient("https://api.pushbullet.com/v2");
			var request = new RestRequest("pushes");
			request.Method = System.Net.Http.HttpMethod.Post;
			request.AddParameter("type", "note");
			request.AddParameter("title", title);
			request.AddParameter("body", body);

			request.AddHeader("Authorization", "Bearer " + PushBulletAPI);
			try
			{
				var response = await client.Execute(request) as RestResponse;
				Debug.WriteLine("Response: {0}", response.StatusCode.ToString());

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}
    }
}
