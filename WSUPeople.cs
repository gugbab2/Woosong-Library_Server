using System.Runtime.Serialization;


namespace WCF_RESTful
{
    //데이터 계약
    [DataContract]
    public class WSUPeople
    {
        [DataMember(Order = 0)]
        public int W_Id { get; set; }

        [DataMember(Order = 1)]
        public string Password { get; set; }

        [DataMember(Order = 2)]
        public string Type { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }
        
        [DataMember(Order = 4)]
        public string Department { get; set; }

        public WSUPeople(int w_id, string paseeword, string type, string name, string department)
        {
            W_Id = w_id;
            Password = paseeword;
            Type = type;
            Name = name;
            Department = department;
        }
    }
}