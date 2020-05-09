using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fuel123.Models
    
{
    public class Fuel
    {
        
        [Key]
        [Display(Name = "Код топлива")]
        public int FuelID { get; set; }

        
        [Display(Name = "Наименование топлива")]
        public string FuelType { get; set; }

        
        [Display(Name = "Плотность топлива")]
        public float FuelDensity { get; set; }
        
        
        public virtual ICollection<Operation> Operations { get; set; }
        
 
    }
}