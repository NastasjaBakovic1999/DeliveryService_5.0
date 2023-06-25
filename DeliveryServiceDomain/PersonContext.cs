using DeliveryServiceDomain.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryServiceDomain
{
    public class PersonContext : IdentityDbContext<Person, IdentityRole<int>, int>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PersonContext()
        {
                
        }
        public PersonContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=Delivery_Service_Database2;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PersonConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());

            SeedData(builder);
            SeedRoles(builder);

            base.OnModelCreating(builder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            PasswordHasher<Person> hasher = new PasswordHasher<Person>(
                Options.Create(new PasswordHasherOptions()
                {
                    CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
                }));

            List<Customer> users = new List<Customer>();

            Customer c1 = new Customer { Id = 1, FirstName = "Pera", LastName = "Peric", UserName = "perica", PhoneNumber = "065/111-222-33", Email = "perap@gmail.com", Address="Mije Kovacevica 7b", PostalCode="11000" };
            c1.PasswordHash = hasher.HashPassword(c1, "P3r1c4!!");
            users.Add(c1);

            Customer c2 = new Customer { Id = 2, FirstName = "Zika", LastName = "Zikic", UserName = "zikica", PhoneNumber = "064/444-555-66", Email = "zikazikic222@gmail.com", Address = "Mije Kovacevica 7b", PostalCode = "11000" };
            c2.PasswordHash = hasher.HashPassword(c2, "Z1k1c4!!");
            users.Add(c2);

            modelBuilder.Entity<Customer>().HasData(users);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "1", Name = "User", ConcurrencyStamp = "1", NormalizedName = "USER" }
                );

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> {RoleId = 1, UserId = 1 },
                new IdentityUserRole<int> {RoleId = 1, UserId = 2 }
                );
        }

        public IDbConnection CreateConnection()
           => new SqlConnection(_connectionString);
    }

}
