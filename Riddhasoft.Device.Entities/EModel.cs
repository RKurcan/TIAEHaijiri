using System.ComponentModel.DataAnnotations;

namespace Riddhasoft.Device.Entities
{
    public class EModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public bool IsAccessDevice { get; set; }
        public bool IsFaceDevice { get; set; }
        public Manufacture Manufacture { get; set; }
        public Brand Brand { get; set; }
    }
    public enum Manufacture
    {
        ZKT = 1
    }
    public enum Brand
    {
        ZKT = 1
    }
}
