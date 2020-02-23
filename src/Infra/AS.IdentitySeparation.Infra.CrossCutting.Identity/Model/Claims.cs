using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    [Table("AspNetClaims")]
    public class Claims
    {
        public Claims()
        {
            ClaimsId = Guid.NewGuid();
        }

        [Key]
        public Guid ClaimsId { get; set; }
                
        public string ClaimsName { get; set; }
    }
}