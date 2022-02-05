using System.Runtime.Serialization;

namespace WCF_RESTful
{
    [DataContract]
    public class Unity_BookList
    {
        [DataMember(Order = 0)]
        public int B_ID { get; set; }

        [DataMember(Order = 1)]
        public string Type { get; set; }

        [DataMember(Order = 2)]
        public string Title { get; set; }

        [DataMember(Order = 3)]
        public string Author { get; set; }

        [DataMember(Order = 4)]
        public string Thumnail { get; set; }

        public Unity_BookList(int b_id, string type, string title, string author, string thumnail)
        {
            B_ID = b_id;
            Type = type;
            Title = title;
            Author = author;
            Thumnail = thumnail;
        }
    }
}