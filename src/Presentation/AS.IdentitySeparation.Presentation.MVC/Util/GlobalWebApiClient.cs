using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using AS.IdentitySeparation.Presentation.MVC.Models;

namespace AS.IdentitySeparation.Presentation.MVC.Util
{
    public static class GlobalWebApiClient
    {
        public const String URI_BASE = "http://localhost:62097/";            

        public static HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(URI_BASE)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("Application/json"));
            var session = HttpContext.Current.Session;
            if (session["Token"] != null)
            {
                var tokenResponseViewModel = GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponseViewModel.AccessToken);
            }
            return client;
        }
                
        public static void StoreToken(TokenResponseViewModel token)
        {
            var session = HttpContext.Current.Session;
            session["Token"] = token;
        }
        public static TokenResponseViewModel GetToken()
        {
            var session = HttpContext.Current.Session;
            return (TokenResponseViewModel)session["Token"];
        }

    }
}