using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Domain.Concrete;
using System.Configuration;
using GameStore.WebUI.Infrastructure.Concrete;
using GameStore.WebUI.Infrastructure.Abstract;


namespace GameStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // Test Data
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.products).Returns(new List<Product>{
            //    new Product {name = "Monopoly", price = 25},
            //    new Product {name = "Scrabble", price = 15},
            //    new Product {name = "Dice", price = 1.50m}
            //});

            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);

            kernel.Bind<IProductRepository>().To<EFProductRepostory>();
            var emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}