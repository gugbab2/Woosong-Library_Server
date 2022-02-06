using System.Runtime.Serialization;
namespace WCF_RESTful
{
        [DataContract]
        public class Unity_BookWish
        {
            [DataMember(Order = 0)]
            public string Title { get; set; }

            [DataMember(Order = 1)]
            public string Authors { get; set; }

            public Unity_BookWish(string title,string authors)
            {
               
                Title = title;
                Authors = authors;
               
            }
        }
}