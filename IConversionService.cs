using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace currency_to_word_conversion.service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IConversionService
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(UnexpectedServiceFault))]
        string convertNumbersToWords(string num);
    }

    [DataContract]
    public class ValidationFault
    {
        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class UnexpectedServiceFault
    {
        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
