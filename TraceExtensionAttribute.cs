namespace SoapExtension.Trace
{
	using System;
	using System.Web.Services.Protocols;

	[AttributeUsage(AttributeTargets.Method)]
	public class TraceExtensionAttribute : SoapExtensionAttribute
	{
		public override System.Type ExtensionType
		{
			get { return typeof(TraceExtension); }
		}

		private int _priority;

		public override int Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				_priority = value;
			}
		}

		public string Filename { get; set; }
	}
}
