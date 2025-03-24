using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretData.ViewModels
{
    public enum  EnumOrderState
    {
        [Display(Name = "Onay Bekleniyor")]
        waiting,
        [Display(Name = "Onaylandı")]
        completed

    }
}
