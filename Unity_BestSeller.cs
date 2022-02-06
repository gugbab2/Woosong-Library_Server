using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCF_RESTful
{
    [DataContract]
    public class Unity_BestSeller
    {
        [DataMember(Order = 0)]
        public string Title { get; set; }

        [DataMember(Order = 1)]
        public string Thumbnail { get; set; }

        [DataMember(Order = 2)]
        public int Bestseller { get; set; }

        public Unity_BestSeller(string title, string thumnail, int bestsellernumber)
        {
            Title = title;
            Thumbnail = thumnail;
            Bestseller = bestsellernumber;
        }
    }
}