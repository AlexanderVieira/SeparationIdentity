using AS.IdentitySeparation.Presentation.MVC.Models;
using AS.IdentitySeparation.Presentation.MVC.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace AS.IdentitySeparation.Presentation.MVC.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class AccountsController : Controller
    {
        // GET: /Accounts/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //POST: /Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {            
            if (ModelState.IsValid)
            {
                const String URI_ADDRESS = "api/Accounts/Login";

                try
                {
                    var client = GlobalWebApiClient.GetClient();

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("email", model.Email),
                        new KeyValuePair<string, string>("password", model.Password)
                    });

                    var response = await client.PostAsync(URI_ADDRESS, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var tokenResponse = await response.Content.ReadAsStringAsync();
                        var vmTokenResponse = new TokenResponseViewModel
                        {
                            AccessToken = tokenResponse
                        };
                        GlobalWebApiClient.StoreToken(vmTokenResponse);

                        if (vmTokenResponse != null)
                        {
                            Session["Email"] = vmTokenResponse.Username.ToString();

                            var clientUser = GlobalWebApiClient.GetClient();

                            var resultLoggedUser = clientUser.GetAsync("api/Accounts/LoggedUser").Result;
                            var resultLoggedEmail = clientUser.GetAsync(@"api/profiles/profile/" + Session["Email"].ToString().EncodeBase64()).Result;

                            if (!resultLoggedEmail.IsSuccessStatusCode)
                            {
                                if (resultLoggedUser.IsSuccessStatusCode)
                                {
                                    if (resultLoggedUser != null)
                                    {
                                        var vmUser = await resultLoggedUser.Content.ReadAsAsync<ProfileViewModel>();
                                        Session["UserId"] = vmUser.ProfileVmId;
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }
                            else
                            {
                                var vmUser = await resultLoggedEmail.Content.ReadAsAsync<ProfileViewModel>();
                                Session["UserId"] = vmUser.ProfileVmId;
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        return View(model);
                    }

                    return View(model);
                }
                catch (Exception ex)
                {
                    var result = ex.Message;
                }
            }

            return View(model);
        }

        // GET: /Accounts/Register        
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Accounts/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                const String URI_ADDRESS = "api/Accounts/Register";

                try
                {
                    var client = GlobalWebApiClient.GetClient();
                    var dataJson = JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(URI_ADDRESS, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("SuccessEmail", "Accounts");
                    }
                }
                catch (Exception ex)
                {
                    var result = ex.Message;
                }
            }
            return View(model);
        }

        //GET: /Account/SuccessEmail        
        public ActionResult SuccessEmail()
        {
            ViewBag.Message = "Please check your e-mail and confirm your e-mail address.";
            return View();
        }

        //GET: /Account/ConfirmEmail        
        public ActionResult ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var confirmEmail = new ConfirmEmailViewModel()
            {
                UserId = userId,
                Code = code
            };

            return View();
        }

        // POST: /Account/ConfirmEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                const String URI_ADDRESS = "api/Account/ConfirmEmail";

                try
                {
                    var client = GlobalWebApiClient.GetClient();
                    var dataJson = JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(URI_ADDRESS, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var confirmEmailResponse = await response.Content.ReadAsAsync<ConfirmEmailViewModel>();
                        return RedirectToAction("Login", "Accounts");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }            
            return View(model);
        }


        //GET: /Account/ForgotPassword        
        public ActionResult ForgotPassword()
        {
            return View();
        }


        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                const String URI_ADDRESS = "api/Account/ForgotPassword";

                try
                {
                    var client = GlobalWebApiClient.GetClient();
                    var dataJson = JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(URI_ADDRESS, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var ForgotResponse = await response.Content.ReadAsAsync<ForgotPasswordViewModel>();
                        //return RedirectToAction("Login", "Account");
                        return RedirectToAction("ForgotPasswordConfirmation", "Accounts");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }            
            return View(model);
        }

        //GET: /Account/ForgotPasswordConfirmation        
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Message = "Please check your e-mail and confirm your e-mail address.";
            return View();
        }



        // GET: /Account/ResetPassword        
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                const String URI_ADDRESS = "api/Account/ResetPassword";

                try
                {
                    var client = GlobalWebApiClient.GetClient();
                    var dataJson = JsonConvert.SerializeObject(model);
                    var content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(URI_ADDRESS, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var ResetPasswordResponse = await response.Content.ReadAsAsync<ResetPasswordViewModel>();
                        return RedirectToAction("ResetPasswordConfirmation", "Accounts");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return View(model);
        }

        //GET: /Account/ResetPasswordConfirmation
        public ActionResult ResetPasswordConfirmation()
        {
            ViewBag.Message = "Password reset successfully!";
            return View();
        }

        //POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            if (ModelState.IsValid)
            {
                const String URI_ADDRESS = "api/Accounts/LogOff";

                try
                {
                    var client = GlobalWebApiClient.GetClient();

                    var content = new StringContent("application/json");

                    var response = await client.PostAsync(URI_ADDRESS, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Session.Clear();
                        return RedirectToAction("Login", "Accounts");
                    }
                }
                catch (Exception ex)
                {
                    var result = ex.Message;
                }
            }
            return View();
        }

    }
}