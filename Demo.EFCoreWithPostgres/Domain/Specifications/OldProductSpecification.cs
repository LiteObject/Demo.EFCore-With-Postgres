using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.EFCoreWithPostgres.Domain.Specifications
{
    public class OldProductSpecification : Specification<Product>
    {
        public override Expression<Func<Product, bool>> ToExpression() => product => product.CreatedOn < DateTime.Today.AddDays(-90);
    }
}
