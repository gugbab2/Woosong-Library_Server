using System;
using System.Runtime.Serialization;

namespace WCF_RESTful
{
    public class WSUlibrary_BookRental
    {
        [DataMember(Order = 0)]
        public int W_ID { get; set; }

        [DataMember(Order = 1)]
        public int B_ID { get; set; }

        [DataMember(Order = 2)]
        public string Type { get; set; }

        [DataMember(Order = 3)]
        public string Title { get; set; }

        [DataMember(Order = 4)]
        public DateTime Rentaldate { get; set; }

        [DataMember(Order = 5)]
        public DateTime Returndate { get; set; }

        [DataMember(Order = 6)]
        public int Renewcount { get; set; }

        [DataMember(Order = 7)]
        public int Overduedays { get; set; }



        public WSUlibrary_BookRental(int w_id, int b_id, string type, string title, DateTime rentaldate, DateTime returndate,
            int renewcount, int overduedays)
        {
            W_ID = w_id;
            B_ID = b_id;
            Type = type;
            Title = title;
            Rentaldate = rentaldate;
            Returndate = returndate;
            Renewcount = renewcount;
            Overduedays = overduedays;
        }
    }
}