namespace WindowsFormsApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int SalePrice { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SellerId { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Product Products { get; set; }

        public virtual Seller Sellers { get; set; }
    }
}
