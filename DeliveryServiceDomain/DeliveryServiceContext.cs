using DeliveryServiceDomain.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceDomain
{
    public class DeliveryServiceContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DeliveryServiceContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<AdditionalService> AdditionalServices { get; set; }
        public DbSet<AdditionalServiceShipment> AdditionalServiceShipments { get; set; }
        public DbSet<ShipmentWeight> ShipmentWeights { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
               .UseLoggerFactory(MyLoggerFactory)
               .EnableSensitiveDataLogging()
               .UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalServiceShipment>(ass =>
            {
                ass.HasKey(ass => new { ass.AdditionalServiceId, ass.ShipmentId });
                ass.HasOne(a => a.Shipment)
                   .WithMany(b => b.AdditionalServices)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ShipmentWeight>()
               .HasMany<Shipment>(st => st.Shipments)
               .WithOne(s => s.ShipmentWeight)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shipment>()
                .OwnsOne(s => s.Sending);

            modelBuilder.Entity<Shipment>()
             .OwnsOne(s => s.Receiving);

            modelBuilder.ApplyConfiguration(new ShipmentConfiguration());
            modelBuilder.ApplyConfiguration(new AdditionalServiceConfiguration());
            modelBuilder.ApplyConfiguration(new AdditionalServiceShipmentConfiguration());
            modelBuilder.ApplyConfiguration(new ShipmentWeightConfiguration());

            Seed(modelBuilder);
		}

        private static void Seed(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AdditionalService>().HasData(
                new AdditionalService { AdditionalServiceId = 1, AdditionalServiceName = "Signed delivery note", AdditionalServicePrice = 50},
                new AdditionalService { AdditionalServiceId = 2, AdditionalServiceName = "Return receipt", AdditionalServicePrice = 50},
                new AdditionalService { AdditionalServiceId = 3, AdditionalServiceName = "Additional packaging", AdditionalServicePrice = 60},
                new AdditionalService { AdditionalServiceId = 4, AdditionalServiceName = "Personal delivery", AdditionalServicePrice = 60},
                new AdditionalService { AdditionalServiceId = 5, AdditionalServiceName = "Value insurance", AdditionalServicePrice = 80},
                new AdditionalService { AdditionalServiceId = 6, AdditionalServiceName = "Email report", AdditionalServicePrice = 30},
                new AdditionalService { AdditionalServiceId = 7, AdditionalServiceName = "SMS report", AdditionalServicePrice = 30 },
                new AdditionalService { AdditionalServiceId = 8, AdditionalServiceName = "Delivery today for tomorrow until 12h", AdditionalServicePrice = 90 },
                new AdditionalService { AdditionalServiceId = 9, AdditionalServiceName = "Delivery today for tomorrow until 19h", AdditionalServicePrice = 70 }
            );

            modelBuilder.Entity<ShipmentWeight>().HasData(
                new ShipmentWeight { ShipmentWeightId = 1, ShipmentWeightDescpription = "Up to 0,5 kg", ShipmentWeightPrice = 250},
                new ShipmentWeight { ShipmentWeightId = 2, ShipmentWeightDescpription = "Over 0,5 to 2kg", ShipmentWeightPrice = 300},
                new ShipmentWeight { ShipmentWeightId = 3, ShipmentWeightDescpription = "Over 2 to 5kg", ShipmentWeightPrice = 390},
                new ShipmentWeight { ShipmentWeightId = 4, ShipmentWeightDescpription = "Over 5 to 10kg", ShipmentWeightPrice = 510},
                new ShipmentWeight { ShipmentWeightId = 5, ShipmentWeightDescpription = "Over 10 to 20kg", ShipmentWeightPrice = 700}
            );
        }

        public IDbConnection CreateConnection()
             => new SqlConnection(_connectionString);
    }
}
