using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPulse.Core.DTOs
{
    public class ProductDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Range(1,9999)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
