using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreApp
{
    public class Pilot:Employee
    {
        public int? FlightHours { get; set; }

        #region Related Objects
        public virtual ICollection<Flight> FlightAsPilotSet { get; set; }
        #endregion
    }
}
