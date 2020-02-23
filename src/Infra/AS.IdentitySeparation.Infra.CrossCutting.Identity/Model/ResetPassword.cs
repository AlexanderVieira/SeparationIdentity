namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class ResetPassword
    {        
        public string Email { get; set; }        
        public string Password { get; set; }        
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}