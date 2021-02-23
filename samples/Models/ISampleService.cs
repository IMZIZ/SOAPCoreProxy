using System.ServiceModel;
using System.Threading.Tasks;
 

namespace Models
{
	[ServiceContract(Namespace = "http://sample.namespace.com/service/2.19/"), XmlSerializerFormat]
	public interface ISampleService
	{
		[OperationContract]
		string GetCityStateByZipCodeRequest(string zipcode);

		[OperationContract]
		ComplexModelResponse PingComplexModel(ComplexModelInput inputModel);

		[OperationContract]
		void VoidMethod(out string s);

		[OperationContract]
		Task<int> AsyncMethod();

		[OperationContract]
		int? NullableMethod(bool? arg);

		[OperationContract]
		void XmlMethod(System.Xml.Linq.XElement xml);
	}
}
