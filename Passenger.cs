using System.Collections.Generic;

namespace EFCoreApp
{
    public class Passenger : Person
    {
        public int Weight { get; set; }

        public virtual ICollection<Booking> BookingSet { get; set; }   
    }
}