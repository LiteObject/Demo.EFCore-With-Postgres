﻿using System;

namespace Demo.EFCoreWithPostgres.Data.Contexts
{
    using Demo.EFCoreWithPostgres.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The product db context.
    /// PS Migration Commands: https://docs.microsoft.com/en-us/ef/core/cli/powershell
    /// </summary>
    public class ProductDbContext : DbContext
    {
        public ProductDbContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDbContext"/> class.
        /// </summary>
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// The on configuring.
        /// </summary>
        /// <param name="optionsBuilder">
        /// The options builder.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /* Migrations with Multiple Providers: *****************************************************************
             * https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli
             * Two options to work with multiple providers:
             * 1. Using multiple context types
             * 2. Using one context type
             *******************************************************************************************************/

            // base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                /* optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=DemoEfCoreWithPostgres;Trusted_Connection=True;MultipleActiveResultSets=true",
                    option => { option.EnableRetryOnFailure(); }); */

                optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=Demo.01", builder => builder.EnableRetryOnFailure());
                optionsBuilder.EnableSensitiveDataLogging();
            }

            optionsBuilder.LogTo(Console.WriteLine);
        }

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new { Id = 1, Name = "Product One", UnitPrice = 1.5, CreatedOn = DateTime.Today },
                new { Id = 2, Name = "Product Two", UnitPrice = 2.5, CreatedOn = DateTime.Today },
                new { Id = 3, Name = "Old Product", UnitPrice = 3.55, CreatedOn = DateTime.Today.AddDays(-150), UpdatedOn = DateTime.Today },
                new { Id = 4, Name = "Expensive Product", UnitPrice = 150.99, CreatedOn = DateTime.Today, UpdatedOn = DateTime.Today }
            );
        }
    }
}
