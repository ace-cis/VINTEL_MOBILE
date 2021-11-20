using System;
using System.Collections.Generic;
using System.Text;

namespace VCargo_Courier.Models
{
 public   class Deliver
    {
        public string Id { get; set; }

        public int BookingId { get; set; }
        public string Shipper { get; set; }
        public string OrderStatus { get; set; }
        public string OrderRefNo { get; set; }
        public string OrderDate { get; set; }
        public string OrdeConsignee { get; set; }
        public string OrderDestination { get; set; }
        public string hwbdate { get; set; }
        public string hwbno { get; set; }
        public string shipper { get; set; }
        public string servicemode { get; set; }
        public string description { get; set; }
        public string cw { get; set; }
        public decimal cbm { get; set; }
        public string carrier { get; set; }
        public string mwab { get; set; }
        public string numAtCard { get; set; }

        public string Client { get; set; }
    }
}
