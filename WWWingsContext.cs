﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCoreApp
{
    public class WWWingsContext : DbContext
    {
        public DbSet<Flight> FlightSet { get; set ; }
        public DbSet<Pilot> PilotSet { get; set; }
        public DbSet<Passenger> PassengerSet { get; set; }
        public DbSet<Booking> BookingSet { get; set; }

        public static string ConnectionString { get; set; } =
     @"Server=(LocalDB)\MSSQLLocalDB;Database=Wings_Step1;Trusted_Connection=True;MultipleActiveResultSets=True;App=EFCoreApp";

        public WWWingsContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(ConnectionString);

            builder.UseLazyLoadingProxies();

            // ????
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // composed key
            builder.Entity<Booking>().HasKey(x => new { x.FlightNo, x.PassengerID });

            // mapping many to many relationship
            builder.Entity<Booking>().HasOne(x => x.Flight).WithMany(x => x.BookingSet).HasForeignKey(x => x.FlightNo);
            builder.Entity<Booking>().HasOne(x => x.Passenger).WithMany(x => x.BookingSet).HasForeignKey(x => x.PassengerID);

            // ????
            base.OnModelCreating(builder);
        }
    }
}
