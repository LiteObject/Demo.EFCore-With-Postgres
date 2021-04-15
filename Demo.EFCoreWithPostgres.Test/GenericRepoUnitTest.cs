using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Demo.EFCoreWithPostgres.Data.Contexts;
using Demo.EFCoreWithPostgres.Domain;
using Xunit;
using System.Threading.Tasks;
using AutoMapper;
using Demo.EFCoreWithPostgres.AutoMapperProfiles;
using Demo.EFCoreWithPostgres.Data.Repositories;
using Demo.EFCoreWithPostgres.Domain.Specifications;

namespace Demo.EFCoreWithPostgres.Test
{
    public class GenericRepoUnitTest
    {
        [Fact]
        public async Task GetAllProducts()
        {
            // ARRANGE
            var builder = new DbContextOptionsBuilder<ProductDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var options = builder.Options;
            await using var context = new ProductDbContext(options);
            var productsInDb = GetProductData();
            await context.AddRangeAsync(productsInDb);
            await context.SaveChangesAsync();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            }).CreateMapper();

            var repo = new GenericRepository<Product, ProductDbContext>(context, mapper);

            // ACT
            var products = await repo.GetAllAsync();

            // ASSERT
            Assert.NotEmpty(products);
            Assert.True(products.Count == productsInDb.Count);
        }

        [Fact]
        public async Task GetOldProducts()
        {
            // ARRANGE
            var builder = new DbContextOptionsBuilder<ProductDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var options = builder.Options;
            await using var context = new ProductDbContext(options);
            var productsInDb = GetProductData();
            await context.AddRangeAsync(productsInDb);
            await context.SaveChangesAsync();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            }).CreateMapper();

            var repo = new GenericRepository<Product, ProductDbContext>(context, mapper);

            var oldProductSpec = new OldProductSpecification();

            // ACT
            var products = await repo.FindAsync(oldProductSpec);

            // ASSERT
            Assert.NotEmpty(products);

            // Products created 90 day ago are considered old.
            Assert.True(products[0].CreatedOn < DateTime.Today.AddDays(-90));
        }

        private List<Product> GetProductData()
        {
            return new()
            {
                new Product { Id = 1, Name = "Product One", UnitPrice = 1.5, CreatedOn = DateTime.Today },
                new Product { Id = 2, Name = "Product Two", UnitPrice = 2.5, CreatedOn = DateTime.Today },
                new Product { Id = 3, Name = "Old Product", UnitPrice = 3.55, CreatedOn = DateTime.Today.AddDays(-150), UpdatedOn = DateTime.Today }
            };
        }
    }
}
