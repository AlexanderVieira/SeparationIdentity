namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class Login
    {        
        public string Email { get; set; }                
        public string Password { get; set; }        
        public bool RememberMe { get; set; }
    }
}