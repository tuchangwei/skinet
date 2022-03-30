using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productcParams)
         : base(x => 
             (string.IsNullOrEmpty(productcParams.Search) || x.Name.ToLower().Contains(productcParams.Search)) &&
             (!productcParams.BrandId.HasValue || x.ProductBrandId == productcParams.BrandId)
              && (!productcParams.TypeId.HasValue || x.ProductTypeId == productcParams.TypeId)
        )
        {

        }
    }
}