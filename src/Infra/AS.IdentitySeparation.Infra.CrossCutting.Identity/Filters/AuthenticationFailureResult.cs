using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Filters
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        private string _ReasonPhrase;
        public HttpRequestMessage Request { get; set; }

        public AuthenticationFailureResult(string ReasonPhrase, HttpRequestMessage request)
        {
            _ReasonPhrase = ReasonPhrase;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            responseMessage.RequestMessage = Request;
            responseMessage.ReasonPhrase = _ReasonPhrase;
            return responseMessage;
        }
    }
}