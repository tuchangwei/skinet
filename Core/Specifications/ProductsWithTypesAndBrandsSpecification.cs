using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productcParams)
        : base(x => 
             (!productcParams.BrandId.HasValue || x.ProductBrandId == productcParams.BrandId)
              && (!productcParams.TypeId.HasValue || x.ProductTypeId == productcParams.TypeId)
        )
        {
            AddInclude(x=>x.ProductBrand);//包含ProductBrand子表数据
            AddInclude(x=>x.ProductType);//包含子表ProductType数据
            AddOrderBy(x => x.Name);//默认按名称排序
            ApplyPaging(productcParams.PageSize*(productcParams.PageIndex-1), productcParams.PageSize);//分页
            if(!string.IsNullOrEmpty(productcParams.Sort))//排序
            {
                switch(productcParams.Sort) 
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;

                }

            }

        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x=>x.ProductBrand);
            AddInclude(x=>x.ProductType);
        }
    }
}