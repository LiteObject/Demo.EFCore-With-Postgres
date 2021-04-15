using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using AutoMapper.QueryableExtensions;
using Demo.EFCoreWithPostgres.AutoMapperProfiles;
using Demo.EFCoreWithPostgres.Data.Contexts;
using Demo.EFCoreWithPostgres.Data.Repositories;
using Demo.EFCoreWithPostgres.Domain;
using Demo.EFCoreWithPostgres.Domain.Specifications;
using Demo.EFCoreWithPostgres.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Demo.EFCoreWithPostgres
{
    class Program
    {
        private static readonly IConfiguration Configuration;
        private static readonly IMapper Mapper;
        
        static Program()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();

                // Newer 
                // cfg.AddMaps(typeof(Program));

                // Older
                /*var profiles = typeof(Program).Assembly.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t));
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
                }*/
            }).CreateMapper();

            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        static async Task Main()
        {
            var connectionString = Configuration.GetConnectionString("DB_CONNECTION");

            var options = new DbContextOptionsBuilder<ProductDbContext>()
                // .UseSqlServer(connectionString)
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
                .Options;

            await using var db = new ProductDbContext(options);
            // await db.Database.EnsureCreatedAsync();

            /*
            // Add a new product: 
            var oldProuct = new Product() { Name = "Old Product", UnitPrice = 3.55, CreatedOn = DateTime.Today.AddDays(-150), UpdatedOn = DateTime.Today};
            await db.Products.AddAsync(oldProuct);
            await db.SaveChangesAsync(); */

            // Option 1:
            /*var products = await db.Products.ToListAsync();
            var productDtos = new List<ProductDto>();
            products.ForEach(p => productDtos.Add(Mapper.Map<ProductDto>(p)));

            productDtos.ForEach(p => Console.WriteLine(p.Name));
            */

            // Option 2:
            /*var productDtos = await db.Products.ProjectTo<ProductDto>(Mapper.ConfigurationProvider).ToListAsync();

            productDtos.ForEach(p => Console.WriteLine(p.Name));
            */

            // Option 3:
            using var repo = new GenericRepository<Product, ProductDbContext>(db, Mapper);
            var productDtos = await repo.FindAsync<ProductDto>(p => p.Name.Contains("One"));

            // Example of specification:
            var oldProducts = await repo.FindAsync(new OldProductSpecification());
            oldProducts.ForEach(p => Console.WriteLine(p.Name));

            productDtos.ForEach(p => Console.WriteLine(p.Name));
            
            // Option 4:
            /*using var repo = new ProductRepository(db, Mapper);
            var productDtos = await repo.FindAsync<ProductDto>(p => p.Name.Contains("One"));

            foreach (var p in productDtos)
            {
                Console.WriteLine(p.Name);
            } */
        }
    }
}
