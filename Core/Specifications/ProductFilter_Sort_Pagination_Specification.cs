using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    //this specification class having 3 specifications - Filter,Sort,and Pagination
    public class ProductFilter_Sort_Pagination_Specification : BaseSpecification<Product>
    {
        public ProductFilter_Sort_Pagination_Specification(string? brand,string? type, string? sort)
                : base(x=> (string.IsNullOrEmpty(brand) || x.Brand==brand) &&
                           (string.IsNullOrEmpty(type) || x.Type == type)
                      )
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }

        }
    }
}
