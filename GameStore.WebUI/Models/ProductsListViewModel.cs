using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.WebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> products { get; set; }
        public PagingInfo pagingInfo { get; set; }
        public string currentCategory { get; set; }
    }
}