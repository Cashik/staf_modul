namespace WindowsFormsApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {


        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Measure { get; set; }

        [Required]
        public int SoldAmount { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
