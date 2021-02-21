
using System.Xml;
using System.Xml.Serialization;


namespace Server
{
    
	[XmlRoot("AuthToken", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
	public class WsAuthToken
	{
		[XmlElement("REQUESTORSYSTEM")]
		public string Requestor { get; set; }

		[XmlElement("SUBSCRIBERSYSTEM")]
		public string Subscriber { get; set; }


		[XmlElement("REQUESTTIMESTAMP")]
		public XmlDateTimeSerializationMode RequestTime { get; set; }

		
	}
}