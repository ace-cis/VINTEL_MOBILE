using System;
using System.Collections.Generic;
using System.Text;

namespace VCargo_Courier.Models
{
    public class ImageModel
    {
        
        public int bookingId {get; set;}
        public string  imgType { get; set; }
        public byte[] img { get; set; }
        public string createdBy { get; set; }
    }
}
