using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.EFCoreWithPostgres.Data.Contexts;
using Demo.EFCoreWithPostgres.Domain;
using Microsoft.EntityFrameworkCore;

namespace Demo.EFCoreWithPostgres.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product, ProductDbContext>
    {
        public ProductRepository(ProductDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<List<T1>> FindAsync<T1>(
            Expression<Func<Product, bool>> predicate,
            params Expression<Func<Product, object>>[] includeProperties)
        {
            // Needed to override for entity specific logic.
            return await this.DbSet.Where(predicate).ProjectTo<T1>(this.mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
