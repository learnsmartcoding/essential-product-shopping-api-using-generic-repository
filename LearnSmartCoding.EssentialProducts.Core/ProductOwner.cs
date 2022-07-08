using System.ComponentModel.DataAnnotations;

namespace LearnSmartCoding.EssentialProducts.Core
{
    public class ProductOwner
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string OwnerADObjectId { get; set; }
        [MaxLength(1000)]
        public string OwnerName { get; set; }
    }
}
