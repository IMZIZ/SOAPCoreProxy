using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Xml.Serialization;
using SoapCore.Extensibility;
using static System.Convert;
using static System.Text.Encoding;

namespace Server
{
	public class WsMessageFilter : IMessageFilter
	{
		private readonly string _requestor;
		private readonly string _subscriber;
		private readonly string _authMissingErrorMessage = "Referenced security token could not be retrieved";
		private readonly string _authInvalidErrorMessage = "Authentication error: Authentication failed: the supplied credential is not right";

		public WsMessageFilter(string requestor, string subscriber)
		{
			_requestor = requestor;
			_subscriber = subscriber;
		}

		public WsMessageFilter(string requestor, string subscriber, string authMissingErrorMessage, string authInvalidErrorMessage)
		{
			_requestor = requestor;
			_subscriber = subscriber;
			_authMissingErrorMessage = authMissingErrorMessage;
			_authInvalidErrorMessage = authInvalidErrorMessage;
		}

		public void OnRequestExecuting(Message message)
		{
			WsAuthToken wsAuthToken;
			try
			{
				wsAuthToken = GetWsAuthToken(message);
			}
			catch (Exception)
			{
				throw new AuthenticationException(_authMissingErrorMessage);
			}

			if (!ValidateWsAuthToken(wsAuthToken))
			{
				throw new InvalidCredentialException(_authInvalidErrorMessage);
			}
		}

		private bool ValidateWsAuthToken(WsAuthToken wsAuthToken)
		{
			if (wsAuthToken.Requestor != _requestor)
			{
				return false;
			}
			if (wsAuthToken.Subscriber != _subscriber)
			{
				return false;
			}
			return true;
		}

		private WsAuthToken GetWsAuthToken(Message message)
		{
			WsAuthToken wsAuthToken = null;
			for (var i = 0; i < message.Headers.Count; i++)
			{
				//if (message.Headers["SUBSCRIBERSYSTEM"].ToUpper() == "REQUESTORSYSTEM")
				if (message.Headers.GetHeader<string>(i) == "REQUESTORSYSTEM")
				{
					using var reader = message.Headers.GetReaderAtHeader(i);
					reader.Read();
					var serializer = new XmlSerializer(typeof(string));
					wsAuthToken.Requestor = (string)serializer.Deserialize(reader);
				}

				if (message.Headers[i].Name.ToUpper() == "SUBSCRIBERSYSTEM")
				{
					using var reader = message.Headers.GetReaderAtHeader(i);
					reader.Read();
					var serializer = new XmlSerializer(typeof(string));
					wsAuthToken.Subscriber = (string)serializer.Deserialize(reader);
				}
			}

			if (wsAuthToken == null)
			{
				throw new Exception();
			}
			else
			{
				Console.WriteLine("Request message header" + wsAuthToken.Requestor);
			}


			return wsAuthToken;
		}

		public void OnResponseExecuting(Message message)
		{
			throw new NotImplementedException();
		}
	}
}
