using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitmqApp.MVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "100 karakterden büyük olamaz")]
        public string Name { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1,100)]
        public int Stock { get; set; }

        [MaxLength(100,ErrorMessage ="100 karakterden büyük olamaz")]
        public string ImageName { get; set; }
    }
}
