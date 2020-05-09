using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuel123.Models
{
    public class Operation
    {
    
        [Key]
        [Display(Name = "Код операции")]
        public int OperationID { get; set; }

       
        [Display(Name = "Код топлива")]
        [ForeignKey("Fuel")]
        public int FuelID { get; set; }

     
        [Display(Name = "Код емкости")]
        [ForeignKey("Tank")]
        public int TankID { get; set; }

        
        [Display(Name = "+Приход/-Расход")]
        public float? Inc_Exp { get; set; }

        
        [Display(Name = "Дата операции")]
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }

        
        public virtual Fuel Fuel { get; set; }
        
        public virtual Tank Tank { get; set; }

    }
}