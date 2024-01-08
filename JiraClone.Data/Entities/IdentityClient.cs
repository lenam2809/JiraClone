using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data.Entities
{
    public class IdentityClient
    {
        [Key]
        [StringLength(900)]
        public string IdentityClientId { get; init; }
        [Required]
        [StringLength(100)]
        public string Description { get; init; }
        [Required]
        [StringLength(2000)]
        public string SecretKey { get; init; }
        public int ClientType { get; init; }
        public bool IsActive { get; init; }
        public int RefreshTokenLifetime { get; init; }
        [StringLength(1000)]
        public string AllowedOrigin { get; init; }
    }
}
