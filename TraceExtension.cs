namespace SoapExtension.Trace
{
	using System;
	using System.IO;
	using System.Web.Services.Protocols;

	public class TraceExtension : SoapExtension
	{
		private string logFolder = ConfigHelper.GetPathValue();

		private Stream oldStream;
		private Stream newStream;

		public override Stream ChainStream(Stream stream)
		{
			oldStream = stream;
			newStream = new MemoryStream();
			return newStream;
		}

		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
		{
			return ((TraceExtensionAttribute)attribute).Filename;
		}

		public override object GetInitializer(System.Type WebServiceType)
		{
			return string.Format("{0}{1}.xml", logFolder, WebServiceType.FullName);
		}

		public override void Initialize(object initializer)
		{
			return;
		}

		public override void ProcessMessage(SoapMessage soapMessage)
		{
			switch (soapMessage.Stage)
			{
				case SoapMessageStage.AfterSerialize:
					WriteOutput(soapMessage);
					break;
				case SoapMessageStage.BeforeDeserialize:
					WriteInput(soapMessage);
					break;
			}
		}

		public void WriteOutput(SoapMessage soapMessage)
		{
			try
			{
				var soapType = (soapMessage is SoapServerMessage) ? "SoapResponse" : "SoapRequest";
				var filename = string.Format("{0} - {1}.xml", soapType, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

				newStream.Position = 0;
				using (var fs = new FileStream(Path.Combine(logFolder, filename), FileMode.Append, FileAccess.Write))
				{
					using (var w = new StreamWriter(fs))
					{
						w.Flush();
						Copy(newStream, fs);
						w.Close();
						newStream.Position = 0;
						Copy(newStream, oldStream);
					}
				}
			}
			catch (Exception)
			{
				// nothing yet :)
			}

		}

		public void WriteInput(SoapMessage soapMessage)
		{
			try
			{
				var soapType = (soapMessage is SoapServerMessage) ? "SoapRequest" : "SoapResponse";
				var filename = string.Format("{0} - {1}.xml", soapType, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

				Copy(oldStream, newStream);
				using (var fs = new FileStream(Path.Combine(logFolder, filename), FileMode.Append, FileAccess.Write))
				{
					using (var w = new StreamWriter(fs))
					{
						w.Flush();
						newStream.Position = 0;
						Copy(newStream, fs);
						w.Close();
						newStream.Position = 0;
					}
				}
			}
			catch (Exception)
			{
				// nothing yet :) 
			}
		}

		private void Copy(Stream from, Stream to)
		{
			using (var reader = new StreamReader(from))
			{
				using (var writer = new StreamWriter(to))
				{
					writer.WriteLine(reader.ReadToEnd());
					writer.Flush();
				}

				reader.Close();
			}
		}
	}
}
