using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace GitFlowVS.Extension
{
	public class Logger
	{
		private static TelemetryClient TelemetryClient { get; set; }

		static Logger()
		{
			TelemetryClient = new TelemetryClient();
            TelemetryClient.Context.Properties["VisualStudioVersion"] = VSVersion.FullVersion.ToString();
		}

		public static void PageView(string page)
		{
			TelemetryClient.TrackPageView(page);
		}
		public static void Event(string eventName, IDictionary<string,string> properties = null  )
		{
			if( properties == null)
				TelemetryClient.TrackEvent(eventName);
			else
			{
				TelemetryClient.TrackEvent(eventName, properties, null);
			}
		}

		public static void Metric(string name, double value)
		{
			TelemetryClient.TrackMetric(name, value);
		}

		public static void Exception(Exception ex)
		{
			TelemetryClient.TrackException(ex);
		}
	}
}
