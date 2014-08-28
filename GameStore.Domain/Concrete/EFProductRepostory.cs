using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Collections.Generic;


namespace GameStore.Domain.Concrete
{
    public class EFProductRepostory : IProductRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Product> products
        {
            get { return context.Products; }
        }


        public void SaveProduct(Product product)
        {
            if (product.productID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.productID);
                if (dbEntry != null)
                {
                    dbEntry.name = product.name;
                    dbEntry.description = product.description;
                    dbEntry.price = product.price;
                    dbEntry.category = product.category;
                    dbEntry.imageData = product.imageData;
                    dbEntry.imageMimeType = product.imageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct (int productID)
        {
            Product dbEntry = context.Products.Find(productID);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
