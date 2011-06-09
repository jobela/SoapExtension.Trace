namespace SoapExtension.Trace
{
	public class ConfigHelper
	{
		public static string GetPathValue()
		{
			var defaultValue = @"C:\log\";

			return ConfigWrapper.GetValue(ConfigWrapper.CONFIG_LOG, defaultValue);
		}
	}
}
