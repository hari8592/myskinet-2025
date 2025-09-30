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
        public ProductFilter_Sort_Pagination_Specification(ProductSpecParams specParams)
                : base(x=> 
                           (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
                           (specParams.Brands.Count ==0 || specParams.Brands.Contains(x.Brand)) &&
                           (specParams.Types.Count ==0  || specParams.Types.Contains(x.Type))
                      )
        {
            //specParams.PageIndex = 2;  // you want 2nd page
            //specParams.PageSize = 5;  // each page has 5 records
            // skip= 5*(2-1) , take=5 
            //skip =5 *1 =5  , take(5)    //that you want on 2nd page
            //skip(5) and Take(5) that means it is your 2nd page.

            //specParams.PageIndex = 1;  // you want 1 page
            //specParams.PageSize = 5;  // each page has 5 records
            // skip= 5*(1-1) , take=5 
            //skip =5 *0 =0  , take(5)    //that you want on 2nd page
            //skip(0) and Take(5) that means it is your 1st page


            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            switch (specParams.Sort)
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
