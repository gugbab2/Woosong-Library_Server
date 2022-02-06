using System;
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
        public string Contents { get; set; }

        [DataMember(Order = 4)]
        public string Isbn { get; set; }

        [DataMember(Order = 5)]
        public string Authors { get; set; }
        [DataMember(Order = 6)]
        public string Publisher { get; set; }

        [DataMember(Order = 7)]
        public string Translators { get; set; }

        [DataMember(Order = 8)]
        public string Thumbnail { get; set; }

        [DataMember(Order = 9)]
        public string Status { get; set; }

        [DataMember(Order = 10)]
        public int Bestseller { get; set; }


        public Unity_BookList(int b_id, string type, string title, string contents, string isbn,
            string authors, string publisher, string translators, string thumbnail, string status, int bestseller)
        {
            B_ID = b_id;
            Type = type;
            Title = title;
            Contents = contents;
            Isbn = isbn;
            Authors = authors;
            Publisher = publisher;
            Translators = translators;
            Thumbnail = thumbnail;
            Status = status;
            Bestseller = bestseller;
        }
    }
}