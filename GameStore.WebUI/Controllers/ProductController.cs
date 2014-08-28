using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 6;
        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            var model = new ProductsListViewModel
            {
                products = repository.products
                    .Where(p => category == null || p.category == category)
                    .OrderBy(p => p.productID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                pagingInfo = new PagingInfo
                {
                    currentPage = page,
                    itemsPerPage = pageSize,
                    totalItems =  category == null ? 
                        repository.products.Count() :
                        repository.products.Where(e=> e.category == category).Count()
                },
                currentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = repository.products.FirstOrDefault(p => p.productID == productId);
            if (product !=null)
            {
                return File(product.imageData, product.imageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}