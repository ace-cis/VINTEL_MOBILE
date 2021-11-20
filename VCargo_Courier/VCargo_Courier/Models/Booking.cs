using System;
using System.Collections.Generic;
using System.Text;

namespace VCargo_Courier.Models
{
    public class Booking
    {

        public int bookingId { get; set; }
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public string remarks { get; set; }
        public string postingDate { get; set; }
        public string docDate { get; set; }
        public string docDueDate { get; set; }
        public string refNo { get; set; }
        public string numAtCard { get; set; }
        public string bookTo { get; set; }
        public string deliveryMode { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public string packingType { get; set; }
        public string serviceMode { get; set; }
        public string transType { get; set; }
        public string vehicle { get; set; }
        public string datePickUp { get; set; }
        public string bookedBy { get; set; }
        public double decVal { get; set; }
        public string pickUpTime { get; set; }
        public string courier { get; set; }
        public string consignee { get; set; }
        public bool isDraft { get; set; }
        // public decimal cbm { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedDate { get; set; }
        public string shipper { get; set; }
        public string client { get; set; }
        public string hwbDate { get; set; }
        public string reason { get; set; }
        public List<BookingLine> bookingLine { get; set; }
        public List<BookingStatus> bookingStatus { get; set; }


        public class BookingLine
        {
            public int id { get; set; }
            public int bookingId { get; set; }
            public int lineNum { get; set; }
            public string itemCode { get; set; }
            public string itemName { get; set; }
            //public double qty { get; set; }
            //public double lineTotal { get; set; }
            //public string uoM { get; set; }
            //public double cisUoM { get; set; }
            //public double length { get; set; }
            //public double width { get; set; }
            //public double height { get; set; }
            //public double totalWeight { get; set; }
            //public double vmw { get; set; }
            //public double decValue { get; set; }
            //public double serviceQty { get; set; }
            //public double actualWeight { get; set; }
            //public double chargeWeight { get; set; }
            public bool isDeleted { get; set; }
            public string createdBy { get; set; }
            public string createdDate { get; set; }
            public string modifiedBy { get; set; }
            public string modifiedDate { get; set; }
        }

        public class BookingStatus
        {
            // public int id { get; set; }
            public int bookingId { get; set; }
            public int lineNum { get; set; }
            public string refNo { get; set; }
            public string destination { get; set; }
            public string bookStatus { get; set; }
            public string timeStamp { get; set; }
            public string createdBy { get; set; }
            public string createdDate { get; set; }
            public string modifiedBy { get; set; }
            public string modifiedDate { get; set; }
        }
        public int transactionType {get; set;}
        public string numSeries { get; set; }


    }
}
