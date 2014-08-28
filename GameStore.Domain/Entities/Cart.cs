using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quntity)
        {
            CartLine line = lineCollection
                .Where(p => p.product.productID == product.productID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    product = product,
                    quantity = quntity
                });
            }
            else
            {
                line.quantity += quntity;
            }
        }

        public void RemoveLine (Product product)
        {
            lineCollection.RemoveAll(l => l.product.productID == product.productID);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.product.price * e.quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
}
