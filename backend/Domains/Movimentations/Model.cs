using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Domains.Products;

namespace backend.Domains.Movimentations
{
    [Table("movimentations")]
    public class Movimentation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [Column("ProductId")]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
