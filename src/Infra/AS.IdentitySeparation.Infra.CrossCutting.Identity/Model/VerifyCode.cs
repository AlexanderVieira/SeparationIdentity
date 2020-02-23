namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class VerifyCode
    {        
        public string Provider { get; set; }      
        public string Code { get; set; }
        public string ReturnUrl { get; set; }       
        public bool RememberBrowser { get; set; }
        public bool RememberMe { get; set; }
    }
}