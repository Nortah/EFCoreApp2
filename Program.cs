using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFCoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CreateDatabase();

            // NewPassengers
            using (var ctx = new WWWingsContext())
            {
                Passenger p = new Passenger() { GivenName = "Pierre", Weight = 7 };

                ctx.PassengerSet.Add(p);

                p = new Passenger { GivenName = "Anne", Weight = 6 };

                ctx.PassengerSet.Add(p);

                p = new Passenger { GivenName = "Toto", Weight = 8 };

                ctx.PassengerSet.Add(p);

                p = new Passenger { GivenName = "Igor", Weight = 9 };

                ctx.PassengerSet.Add(p);

                ctx.SaveChanges();
            }

            // NewPilot();
            NewPilot();

            NewFlights();


            // NewBooking
            // *****
            using (var ctx = new WWWingsContext())
            {
                ctx.BookingSet.Add(new Booking { FlightNo = 1, PassengerID = 1 });

                Flight f = ctx.FlightSet.Find(2);

                f.BookingSet.Add(new Booking { PassengerID = 1 });

                Passenger p = ctx.PassengerSet.Find(2);

                p.BookingSet.Add(new Booking { FlightNo = 1 });

                ctx.BookingSet.Add(new Booking { Flight= f, Passenger= ctx.PassengerSet.Find(3)});

                f.BookingSet.Add(new Booking { Passenger = ctx.PassengerSet.Find(4) });

                ctx.SaveChanges();
            }







            PrintFlights();

            UpdateFlights();

            UpdateFlightsDummy();

            PrintFlights();

            //DeleteFlights(ctx);
            using (var ctx = new WWWingsContext())
            {
                var q = from p in ctx.PilotSet
                        select p;

               foreach (var p in q)  
                {
                    // explicit loading
                    ctx.Entry(p).Collection(x => x.FlightAsPilotSet).Load();

                    Console.WriteLine("{0}. {1}, {2} :", p.PersonID, p.GivenName, p.FlightAsPilotSet.Count());
                    
                    foreach (var f in p.FlightAsPilotSet)
                    {
                        Console.WriteLine("---> {0} {1} {2}", f.FlightNo, f.Destination, f.Seats);
                    }
                }
            }

            using (var ctx = new WWWingsContext())
            {
                foreach (var f in ctx.FlightSet)
                {
                    ctx.Entry(f).Reference(x => x.Pilot).Load();

                    Console.WriteLine("* {0}", f.Pilot.GivenName);
                }
            }

            // optimisée, ne lance qu'1 requête, les jointures
            using (var ctx = new WWWingsContext())
            {
                // eager loading
                var q = from p in ctx.PilotSet.Include(x => x.FlightAsPilotSet)
                        select p;

                foreach (Pilot p in q)
                {
                    Console.WriteLine("{0}. {1}, {2} :", p.PersonID, p.GivenName, p.FlightAsPilotSet.Count());

                    foreach (var f in p.FlightAsPilotSet)
                    {
                        Console.WriteLine("---> {0} {1} {2}", f.FlightNo, f.Destination, f.Seats);
                    }
                }
            }

            using (var ctx = new WWWingsContext())
            {
                var q = from f in ctx.FlightSet.Include(x => x.Pilot)
                        select f;

                foreach (var f in q)
                    Console.WriteLine("* {0}", f.Pilot.GivenName);
            }

            // lazy loading
            using (var ctx = new WWWingsContext())
            {
                var q = from p in ctx.PilotSet.Include(x => x.FlightAsPilotSet)
                        select p;

                foreach (var p in q)
                {

                    Console.WriteLine("{0}. {1}, {2} :", p.PersonID, p.GivenName, p.FlightAsPilotSet.Count());

                    foreach (var f in p.FlightAsPilotSet)
                    {
                        Console.WriteLine("---> {0} {1} {2}", f.FlightNo, f.Destination, f.Seats);
                    }
                }
            }
            

            PrintFlights();

            //PrintFlightsWithCriteria(ctx);


                Console.ReadKey();
        }

        private static void NewPilot()
        {
            using (var ctx = new WWWingsContext())
            {
                Pilot p = new Pilot { Surname = "Bono", GivenName = "Jean", Salary = 23000 };

                ctx.PilotSet.Add(p);

                ctx.SaveChanges();
            }
        }

        private static void UpdateFlightsDummy()
        {
            using (var ctx = new WWWingsContext())
            {
                Flight f = new Flight { FlightNo = 2 };

                ctx.Attach(f);

                f.Seats = 303;

                ctx.SaveChanges();
            }
        }

        private static void UpdateFlights()
        {
            using (var ctx = new WWWingsContext())
            {
                Flight f = ctx.FlightSet.Find(1);

                f.Seats = 200;

                ctx.SaveChanges();
            }
        }

        private static void CreateDatabase()
        {
            using (var ctx = new WWWingsContext())
            {
                var e = ctx.Database.EnsureCreated();

                if (e)
                    Console.WriteLine("Database has been created!");
            }
        }

        private static void DeleteFlightsDummy()
        {
            using (var ctx = new WWWingsContext())
            {
                Flight f1 = new Flight { FlightNo = 1 };

                Flight f2 = new Flight { FlightNo = 2 };

                ctx.Attach(f1);

                ctx.Attach(f2);

                ctx.FlightSet.Remove(f1);

                ctx.FlightSet.Remove(f2);

                ctx.SaveChanges();
            }
        }

        private static void DeleteFlights()
        {
            using (var ctx = new WWWingsContext())
            {

                Flight f = ctx.FlightSet.Find(1);

                ctx.FlightSet.Remove(f);

                f = ctx.FlightSet.Find(2);

                ctx.FlightSet.Remove(f);

                ctx.SaveChanges();
            }
        }

        private static void NewFlights()
        {
            using (var ctx = new WWWingsContext())
            {
                Flight f = new Flight { FlightNo = 1, Departure = "GVA", Destination = "MLA", Seats = 100, Date = new DateTime()};

                //f.Pilot = ctx.PilotSet.Find(1);
                f.PilotId = 1;

                ctx.FlightSet.Add(f);

                Flight f2 = new Flight { FlightNo = 2, Departure = "MLA", Destination = "GVA", Seats = 100, Date = new DateTime() };

                //f2.Pilot = ctx.PilotSet.Find(1);
                f2.PilotId = 1;

                ctx.FlightSet.Add(f2);

                ctx.SaveChanges();
            }
        }

        private static void PrintFlightsWithCriteria()
        {
            using (var ctx = new WWWingsContext())
            {
                var q = from f in ctx.FlightSet
                        where f.Departure == "GVA"
                        select new Flight { Departure = f.Departure, Destination = f.Destination };

                foreach (Flight f in q)
                    Console.WriteLine("{0} {1}", f.Departure, f.Destination);
            }
        }

        private static void PrintFlights()
        {
            using (var ctx = new WWWingsContext())
            {
                Console.WriteLine("----------------------------------------");

                var q = from f in ctx.FlightSet
                        select f;

                foreach (Flight f in q)
                    Console.WriteLine("{0} {1} {2} {3} {4}", f.FlightNo, f.Date, f.Departure, f.Destination, f.Seats);

                Console.WriteLine("----------------------------------------");
            }
        }
    }
}
