using SoapCore.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;
using System.Text;
using System.IO;

namespace Server
{
    public class MessageBroadcaster : SoapCore.Extensibility.IMessageInspector2
    {

        int transactionID = 0;

        public MessageBroadcaster()
        {
            // Temporary - Remove all files in the log directory... 
            // Doing it this way so that I can remain in the folder under File Explorer while the delete happens.
            string myFile = ".\\Requests\\";
            string requestsPath = Path.GetFullPath(myFile);
            
            if (!Directory.Exists(requestsPath))
            {
                Directory.CreateDirectory(requestsPath);
            }

            string[] logFiles = Directory.GetFiles(requestsPath);
            foreach (string fileName in logFiles)
            {
                File.Delete(fileName);
            }
        }

        public object AfterReceiveRequest(ref Message message, ServiceDescription serviceDescription)
        {
            // Temporary - to see if we can keep the same transaction ID across transaction requests and replies
            transactionID++;
            // This currently shows a number of ways to display the message details. Over time, we will narrow to the one we intend to use.

            // This section is not yet tested. It should allow us to see headers when present
            Console.WriteLine($"Total Headers found: {message.Headers.Count }");

            for (int currentHeader = 0; currentHeader < message.Headers.Count; currentHeader++)
            {

                using (XmlDictionaryReader headerReader = message.Headers.GetReaderAtHeader(currentHeader))
                {
                    headerReader.MoveToStartElement();
                    while (!headerReader.EOF)
                    {
                        //string xmlContent = headerReader.ReadContentAsString();
                        string myString = headerReader.ReadElementContentAsString();
                        Console.WriteLine($"Value for header {currentHeader}: ");
                        Console.WriteLine(myString);
                        //Console.WriteLine($"Content was {xmlContent}");
                    }
                }
            }

            // A generic look at the messsage headers
            string myHeader = message.Headers.ToString();
            Console.WriteLine($"Read from header: {myHeader}");

            // A simple write of the bodyString shows that the body is not able to see the stream details
            string bodyString = message.ToString();
            Console.WriteLine($"Message to string: {bodyString}");

            // For today, this uses a random file name dor uniqueness. 
            // In the future, a transaction ID obtained from the header will be used to ensure a unique file name.
            string myFile = ".\\Requests\\IN_" + transactionID.ToString("####0");
            MessageToFile(ref message, myFile);

            Console.WriteLine("Request: Header logging is complete.");

            // What other things might we choose to do here since we can return any object?
            return 0;

        }

        private void MessageToFile(ref Message message, string fileName)
        {
            // We copy the message to a buffer, and because it can only be used once, must create the message again from the buffer to 
            // allow the SOAP service to get access to the message. This is only necessary because we print the stream details.
            MessageBuffer buffer = message.CreateBufferedCopy(Int32.MaxValue);
            message = buffer.CreateMessage();

            // Create a copy of the buffered message for use in the print so that we don't touch the original 'message' object.
            var copy = buffer.CreateMessage();

            // Create a string representation of the message to be written to a file.
            string strMessage = MessageString(ref copy);

            // Write the output.
            using (System.IO.FileStream outStream = new System.IO.FileStream($"{fileName}.txt", System.IO.FileMode.OpenOrCreate))
            {
                StreamWriter myWriter = new StreamWriter(outStream);
                myWriter.Write(strMessage);
                myWriter.Close();
            }

        }

        public void BeforeSendReply(ref Message reply, ServiceDescription serviceDescription, object correlationState)
        {

            //For today, using a sequential counter that is incremented on each incoming request. This will re-set on each server start.
            // In the future, a transaction ID obtained from the header will be used to ensure a unique file name.
            string myFile = ".\\Requests\\OUT_" + transactionID.ToString("####0");

            // Create headers for any details that need to be in the reply.
            List<MessageHeader> headerDetails = new List<MessageHeader>();
            headerDetails.Add(MessageHeader.CreateHeader("transactionId", "namespace", transactionID));
            headerDetails.Add(MessageHeader.CreateHeader("transactionDate", "namespace", DateTime.Now.ToShortDateString()));
            
            // Add them to the outgoing SOAP reply.
            foreach (MessageHeader thisHeader in headerDetails)
            {
                reply.Headers.Add(thisHeader);
            }

            //Log the modified message as it gets sent.
            MessageToFile(ref reply, myFile);

            Console.WriteLine("Reply: Header manipulation and logging is complete.");
        }

        /// <summary>
        /// Get the XML of a Message even if it contains an unread Stream as its Body.
        /// <para>message.ToString() would contain "... stream ..." as
        ///       the Body contents.</para>
        /// </summary>
        /// <param name="m">A reference to the <c>Message</c>. </param>
        /// <returns>A String of the XML after the Message has been fully
        ///          read and parsed.</returns>
        /// <remarks>The Message <paramref cref="m"/> is re-created
        ///          in its original state.</remarks>
        String MessageString(ref Message m)
        {
            // ECT - Note: this whole thing falls apart with the voidMethod call, which should not really matter to our needs.
            // For the moment I have commented that call from the Client Sample, but it is important to note.

            // copy the message into a working buffer.
            MessageBuffer mb = m.CreateBufferedCopy(int.MaxValue);

            // re-create the original message, because "copy" changes its state.
            m = mb.CreateMessage();

            Stream s = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(s);
            mb.CreateMessage().WriteMessage(xw);
            xw.Flush();
            s.Position = 0;

            // We need to consider the maximum length of a message for this, I expect.
            byte[] bXML = new byte[s.Length];
            s.Read(bXML, 0, (int)s.Length);

            if (bXML[0] != (byte)'<')
            {
                return Encoding.UTF8.GetString(bXML, 3, bXML.Length - 3);
            }
            else
            {
                return Encoding.UTF8.GetString(bXML, 0, bXML.Length);
            }
        }
    }
}
