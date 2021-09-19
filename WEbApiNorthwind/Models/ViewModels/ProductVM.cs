using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WEbApiNorthwind.Models.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }
    }
}