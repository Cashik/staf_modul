namespace WindowsFormsApplication1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Seller
    {

        [Key]
        public int SellerId { get; set; }

        [Required]
        public float Commission { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Sername { get; set; }

        public virtual List<Sale> Sales { get; set; }
    }
}
