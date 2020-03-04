using AS.IdentitySeparation.Infra.CrossCutting.Identity.Configuration;
using AS.IdentitySeparation.Infra.CrossCutting.Identity.Filters;
using AS.IdentitySeparation.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = false
            };

            var result = await _AppUserManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var code = await _AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var encodedCode = HttpUtility.UrlEncode(code);
                var callbackUrl = new Uri(@"http://localhost:62097/Api/Accounts/ConfirmEmail?userId=" + user.Id + "&code=" + encodedCode);
                await _AppUserManager.SendEmailAsync(user.Id, "Confirme sua conta.", "Por favor confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");
                await _AppSignInManager.SignInAsync(user, false, false);
                return Ok(callbackUrl);
            }
            
            return GetErrorResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(Login loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);            

            var result = await _AppSignInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.ToString().Equals("Success")) return Ok(TokenManager.GenerateToken(loginUser.Email));

            return BadRequest("Usuário ou senha inválidos.");
           
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("VerifyCode")]
        public async Task<IHttpActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            if (!await _AppSignInManager.HasBeenVerifiedAsync())
            {
                return BadRequest("Error");
            }
            var user = await _AppUserManager.FindByIdAsync(await _AppSignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var status = "DEMO: Caso o código não chegue via " + provider + " o código é: ";
                var codigoAcesso = await _AppUserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
                return Ok(status + codigoAcesso);
            }
            return Ok(new VerifyCode { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("VerifyCode")]
        public async Task<IHttpActionResult> VerifyCode(VerifyCode model)
        {
            if (!ModelState.IsValid) BadRequest(ModelState);
            var result = await _AppSignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return Ok(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return BadRequest("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código Inválido.");
                    return BadRequest(model.Code);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Error");
            }
            var result = await _AppUserManager.ConfirmEmailAsync(userId, code);
            return Ok(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _AppUserManager.FindByNameAsync(model.Email);
                if (user == null || !(await _AppUserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Não revelar se o usuario nao existe ou nao esta confirmado
                    return BadRequest("ForgotPasswordConfirmation");
                }

                var code = await _AppUserManager.GeneratePasswordResetTokenAsync(user.Id);                
                var callbackUrl = new Uri(@"http://localhost:62097/Api/Accounts/ResetPassword?userId=" + user.Id + "&code=" + code + "&protocol=" + Request.RequestUri.Scheme);
                await _AppUserManager.SendEmailAsync(user.Id, "Esqueci minha senha", "Por favor altere sua senha clicando aqui: <a href='" + callbackUrl + "'></a>");
                //var link = callbackUrl;
                //var status = "DEMO: Caso o link não chegue: ";
                //var linkAcesso = callbackUrl;
                return Ok("ForgotPasswordConfirmation");
            }            
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _AppUserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Não revelar se o usuario nao existe ou nao esta confirmado
                return BadRequest("ResetPasswordConfirmation Account");
            }
            var result = await _AppUserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("ResetPasswordConfirmation Account");
            }
            return GetErrorResult(result);            
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SendCode")]
        public async Task<IHttpActionResult> SendCode(SendCode model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            // Generate the token and send it
            if (!await _AppSignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return BadRequest("Error");
            }
            return Ok(new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ExternalLoginCallback")]
        public async Task<IHttpActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return BadRequest("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _AppSignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Ok(returnUrl);
                case SignInStatus.LockedOut:
                    return BadRequest("Lockout");
                case SignInStatus.RequiresVerification:
                    return BadRequest("Error: " + returnUrl);
                case SignInStatus.Failure:
                default:
                    // Se ele nao tem uma conta solicite que crie uma
                    //var _returnUrl = returnUrl;
                    //var loginProvider = loginInfo.Login.LoginProvider;
                    return BadRequest("ExternalLoginConfirmation " + loginInfo.Email);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ExternalLoginConfirmation")]
        public async Task<IHttpActionResult> ExternalLoginConfirmation(ExternalLoginConfirmation model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok("Redirect Index Manage");
            }

            if (ModelState.IsValid)
            {
                // Pegar a informação do login externo.
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null) return BadRequest("ExternalLoginFailure");
                
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _AppUserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _AppUserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _AppSignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return BadRequest(returnUrl);
                    }
                }
                return GetErrorResult(result);
            }

            //var returnUrl = returnUrl;
            return BadRequest(ModelState);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [Route("LogOff")]
        public IHttpActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return Ok("Redirect Index Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_AppUserManager != null)
                {
                    _AppUserManager.Dispose();
                    //_AppUserManager = null;
                }

                if (_AppSignInManager != null)
                {
                    _AppSignInManager.Dispose();
                    //_AppSignInManager = null;
                }
            }

            base.Dispose(disposing);
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
