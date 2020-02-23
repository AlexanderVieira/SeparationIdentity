using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    [Table("AspNetClients")]
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string ClientKey { get; set; }
    }
}