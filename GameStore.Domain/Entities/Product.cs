
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int productID { get; set; }
        [Display(Name="Name")]
        [Required(ErrorMessage="Please enter product name")]
        public string name { get; set; }
        [Display(Name="Desciption")]
        [Required(ErrorMessage= "Please enter a description")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }
        [Display(Name="Price")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage= "Please enter a positive price")]
        public decimal price { get; set; }
        [Display(Name="Category")]
        [Required(ErrorMessage = "Please specify a category")]
        public string category { get; set; }
        public byte[] imageData { get; set; }
        public string imageMimeType { get; set; }
    }
}
