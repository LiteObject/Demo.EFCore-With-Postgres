using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Demo.EFCoreWithPostgres.Data.Contexts;
using Demo.EFCoreWithPostgres.Domain;
using Xunit;
using System.Threading.Tasks;

namespace Demo.EFCoreWithPostgres.Test
{
    public class ProductDbContextUnitTest
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

            // ACT
            var products = await context.Products.ToListAsync();

            // ASSERT
            Assert.NotEmpty(products);
            Assert.True(products.Count == productsInDb.Count);
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
