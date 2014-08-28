using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository productRespository)
        {
            repository = productRespository;
        }

        public ViewResult Index()
        {
            return View(repository.products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.products.
                FirstOrDefault(p => p.productID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
          
                    product.imageMimeType = image.ContentType;
                    product.imageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.imageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            var product = new Product();
            product.productID = 0;
            return View("Edit", product);
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.name);
            }
            return RedirectToAction("Index");
        }
    }
}
