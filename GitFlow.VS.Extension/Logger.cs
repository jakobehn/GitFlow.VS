using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace GitFlowVS.Extension
{
	public class Logger
	{
		private static TelemetryClient TelemetryClient { get; set; }

		static Logger()
		{
		    TelemetryClient = new TelemetryClient
		    {
		        InstrumentationKey = "0aafb28d-ec58-4d70-b42c-ad53134bee79"
		    };
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
