namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class SetPassword
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}