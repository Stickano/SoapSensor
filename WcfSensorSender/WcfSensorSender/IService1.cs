using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfSensorSender
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        int Insert(int light, int temp, int resistence, int analog);
        [OperationContract]
        List<String> Receive();
        [OperationContract]
        string Avarage(List<string> values);
    }
    
    [DataContract]
    public class SensorData
    {
        [DataMember]
        public int id;
        [DataMember]
        public int light;
        [DataMember]
        public int temp;
        [DataMember]
        public int resistence;
        [DataMember]
        public int analog;
    }
}
