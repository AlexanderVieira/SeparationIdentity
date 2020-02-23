namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class Register
    {       
        public string Email { get; set; }        
        public string Password { get; set; }       
        public string ConfirmPassword { get; set; }
    }
}