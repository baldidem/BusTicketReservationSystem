using Microsoft.EntityFrameworkCore;
using SkylerTourism.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerTourism.Data.Concrete.EfCore
{
    public class SkylerTourismContext : DbContext
    {
        public SkylerTourismContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<City>()
                .HasData(
                new City() { CityId = 1, CityName = "Istanbul" },
                new City() { CityId = 2, CityName = "Bursa" },
                new City() { CityId = 3, CityName = "Balikesir" },
                new City() { CityId = 4, CityName = "Izmir" },
                new City() { CityId = 5, CityName = "Eskisehir" },
                new City() { CityId = 6, CityName = "Ankara" }
        );

            modelBuilder
                .Entity<Bus>()
                .HasData(
                new Bus() { BusId = 1, SeatCapacity = 30 },
                new Bus() { BusId = 2, SeatCapacity = 40 },
                new Bus() { BusId = 3, SeatCapacity = 50 }
        );

            modelBuilder
                .Entity<Passenger>()
                .HasData(
                new Passenger() { PassengerId = 1, PassengerName="Didem", PassengerSurname= "Bal", IdentityNumber="123456789", EMail="edb@gmail.com" },
                new Passenger() { PassengerId = 2, PassengerName="John", PassengerSurname= "Baby", IdentityNumber="987654321", EMail="john@hotmail.com" },
                new Passenger() { PassengerId = 3, PassengerName="Deniz", PassengerSurname= "Kaya", IdentityNumber="112233445", EMail="denizz@gmail.com" },
                new Passenger() { PassengerId = 4, PassengerName="Murat", PassengerSurname= "Cicek", IdentityNumber="234789555", EMail="mrt@gmail.com" },
                new Passenger() { PassengerId = 5, PassengerName="Ahmet", PassengerSurname= "Recel", IdentityNumber="134567879", EMail="ahmet@gmail.com" },
                new Passenger() { PassengerId = 6, PassengerName="Angelica", PassengerSurname= "Melek", IdentityNumber="100056789", EMail="angelica@gmail.com" },
                new Passenger() { PassengerId = 7, PassengerName="Clara", PassengerSurname= "Kirinti", IdentityNumber="135792468", EMail="clara@gmail.com" },
                new Passenger() { PassengerId = 8, PassengerName="Pelin", PassengerSurname= "Sarsan", IdentityNumber="101101101", EMail="pelin@gmail.com" },
                new Passenger() { PassengerId = 9, PassengerName="Yagmur", PassengerSurname= "Aslan", IdentityNumber="128934675", EMail="yagmur@gmail.com" },
                new Passenger() { PassengerId = 10, PassengerName="Ipek", PassengerSurname= "Akay", IdentityNumber="234234234", EMail="ipek@hotmail.com" },
                new Passenger() { PassengerId = 11, PassengerName="Irem", PassengerSurname= "Guncan", IdentityNumber="345345345", EMail="irmgnc@gmail.com" }
                );


            modelBuilder.
                Entity<Trip>()
                .HasData(
                new Trip() { TripId = 1, BusId=1, DepartureCityId=1, ArrivalCityId=5, Price=300, TripDate=new DateTime(2022,05,12) },
                new Trip() { TripId = 2, BusId=2, DepartureCityId=1, ArrivalCityId=5, Price=999, TripDate=new DateTime(2022, 11, 12) },
                new Trip() { TripId = 3, BusId = 3, DepartureCityId=1, ArrivalCityId=5, Price=888, TripDate=new DateTime(2022, 11, 12) },
                new Trip() { TripId = 4, BusId=1, DepartureCityId=3, ArrivalCityId=4, Price=777, TripDate=new DateTime(2022, 11, 12) },
                new Trip() { TripId = 5, BusId=1, DepartureCityId=5, ArrivalCityId=6, Price=666, TripDate=new DateTime(2022, 11, 13) }
                );

            modelBuilder
                .Entity<Ticket>()
                .HasData(
                new Ticket { TicketId=1, BusId=1, PassengerId=1,  SeatNumber=1,  TripId=1 },
                new Ticket { TicketId=2, BusId=1, PassengerId=2,  SeatNumber=12, TripId = 1 },
                new Ticket { TicketId=3, BusId=2, PassengerId=3,  SeatNumber=20, TripId = 1 },
                new Ticket { TicketId=4, BusId=3, PassengerId=1,  SeatNumber=40, TripId = 1 },
                new Ticket { TicketId=5, BusId=2, PassengerId=1,  SeatNumber=10, TripId = 1 }
                );
        }

    }
}
