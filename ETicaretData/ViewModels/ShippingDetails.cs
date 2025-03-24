using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretData.ViewModels
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="Lütfen bos gecmeyiniz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Lütfen bos gecmeyiniz")]
        public string Adres { get; set; }
        [Required(ErrorMessage = "Lütfen bos gecmeyiniz")]
        public string City { get; set; }
        [Required(ErrorMessage = "Lütfen bos gecmeyiniz")]
        public string AdresTitle { get; set; }
        
    }
}
