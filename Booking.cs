using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreApp
{
    public class Booking
    {
        // composed keys, Fluent API
        public int FlightNo { get; set; }
        public int PassengerID { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
    }
}
