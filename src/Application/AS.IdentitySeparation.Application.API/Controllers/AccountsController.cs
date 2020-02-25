using AS.IdentitySeparation.Infra.CrossCutting.Identity.Configuration;
using AS.IdentitySeparation.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace AS.IdentitySeparation.Application.API.Controllers
{
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        private readonly ApplicationUserManager _AppUserManager;
        private readonly ApplicationSignInManager _AppSignInManager;

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public AccountsController(ApplicationUserManager AppUserManager, ApplicationSignInManager AppSignInManager)
        {
            _AppUserManager = AppUserManager;
            _AppSignInManager = AppSignInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(Register registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _AppUserManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                //var code = await _AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var encodedCode = HttpUtility.UrlEncode(code);
                //var callbackUrl = new Uri(@"http://localhost:49609/accounts/ConfirmEmail?userId=" + user.Id + "&code=" + encodedCode);
                //await _AppUserManager.SendEmailAsync(user.Id, "Confirme sua conta.", "Por favor confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");
                await _AppSignInManager.SignInAsync(user, false, false);
                return Ok();
            }
            
            return GetErrorResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(Login loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);            

            var result = await _AppSignInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Equals("Succeeded")) return Ok();

            return BadRequest("Usuário ou senha inválidos.");
           
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // Nenhum erro ModelState disponível para envio; retorne um BadRequest vazio.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
