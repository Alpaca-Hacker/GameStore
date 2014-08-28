
using System.Collections.Generic;
using GameStore.Domain.Entities;


namespace GameStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> products { get; }

        void SaveProduct(Product product);

        Product DeleteProduct(int productID);
    }
}
