using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorNet5.Shared
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; }

        public Role Role { get; set; }

        [Range(typeof(DateTime), minimum: "01-01-2020", maximum: "01-01-2030")]
        [Required]
        public DateTime ExpiryDate { get; set; }

        public string Photo { get; set; }

        [NotMapped]
        public bool RenewNow { get; set; }
    }
}
