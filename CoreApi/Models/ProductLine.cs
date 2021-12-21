using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CoreApi.Models
{
    [Table("Productline")]
    public partial class ProductLine : BaseEntity
    {
        public ProductLine()
        {
            Products = new HashSet<Product>();
        }

        public string ProductLine1 { get; set; }
        public string TextDescription { get; set; }
        public string HtmlDescription { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
