using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string name {get; set;}

        [Required(ErrorMessage="Please enter the first address line")]
        [Display(Name="Line 1")]
        public string line1 { get; set; }
        [Display(Name="Line 2")]
        public string line2 { get; set; }
        [Display(Name="Line 3")]
        public string line3 { get; set; }

        [Required(ErrorMessage= "Please enter a city name")]
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "County/State")]
        public string county {get; set;}

        [Required(ErrorMessage = "Please enter postcode/zip")]
        [Display(Name = "Postcode/Zip")]
        public string postcode { get; set; }

        [Required(ErrorMessage = "please enter a country")]
        [Display(Name = "Country")]
        public string country { get; set; }

        public bool gift { get; set; }


    }
}
