using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("ContractDetails")]
        public string ContactDetails { get; set; } = string.Empty;

        public string Region { get; set; } = string.Empty;

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
