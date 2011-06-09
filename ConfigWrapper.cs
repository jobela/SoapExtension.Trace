namespace SoapExtension.Trace
{
	using System;
	using System.Configuration;

	public class ConfigWrapper
	{
		public const string CONFIG_LOG = "LOG_PATH";

		public static string GetValue(string key, string defaultValue)
		{
			try
			{
				return ConfigurationManager.AppSettings[key];
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
	}
}
