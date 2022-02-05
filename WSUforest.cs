using System.Runtime.Serialization;

namespace WCF_RESTful
{
    //데이터 계약
    [DataContract]
    public class WSUforest
    {
        [DataMember(Order = 0)]
        public int W_ID { get; set; }

        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public int Character { get; set; }

        [DataMember(Order = 3)]
        public bool Login { get; set; }

        [DataMember(Order = 4)]
        public bool Access { get; set; }

        public WSUforest(int w_id, string name, int character, bool login, bool access)
        {
            W_ID = w_id;
            Name = name;
            Character = character;
            Login = login;
            Access = access;
        }
    }
}