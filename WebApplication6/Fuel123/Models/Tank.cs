using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fuel123.Models
{
    public class Tank
    {
      
        [Key]
        [Display(Name = "Код емкости")]
        public int TankID { get; set; }

       
        [Display(Name = "Наименование емкости")]
        public string TankType { get; set; }

      
        [Display(Name = "Вес")]
        public float TankWeight { get; set; }

       
        [Display(Name = "Объем")]
        public float TankVolume { get; set; }

        
        [Display(Name = "Материал")]
        public string TankMaterial { get; set; }

      
        [Display(Name = "Изображение")]
        public string TankPicture { get; set; }

      
        public virtual ICollection<Operation> Operations { get; set; }
        


    }
}