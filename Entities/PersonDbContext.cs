using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }   
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //map entities to tables
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Country>().ToTable("Countries");

            //seed data from json files
            string personsJason = System.IO.File.ReadAllText("persons.json");
            List<Person> people = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJason);

            string countriesJason = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJason);

            modelBuilder.Entity<Person>().HasData(people);

            modelBuilder.Entity<Country>().HasData(countries);

        }

        public List<Person> SP_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE dbo.GetAllPersons").ToList();
        }
        public int Sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] {
        new SqlParameter("@Id", person.Id),
        new SqlParameter("@Name", person.Name),
        new SqlParameter("@Email", person.Email),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@Gender", person.Gender),
        new SqlParameter("@CountryId", person.CountryId),
        new SqlParameter("@Address", person.Address),
        new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
      };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters);
        }
    }
}
