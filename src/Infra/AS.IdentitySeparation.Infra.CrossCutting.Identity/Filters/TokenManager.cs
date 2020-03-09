using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Configuration;
using System.Threading.Tasks;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Filters
{
    public class TokenManager
    {        
        private static readonly string Secret = ConfigurationManager.AppSettings["SecretKey"];

        public static string GenerateToken(string userName)
        {            
            //var secretNew = Secret.Replace("-", "");            
            byte[] key = Convert.FromBase64String(Secret);
            var securityKey = new SymmetricSecurityKey(key);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName)}),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static Task<ClaimsPrincipal> GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null) return null;                
                byte[] key = Convert.FromBase64String(Secret);
                var parameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,                    
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return Task.FromResult(principal);                
            }
            catch(Exception ex)
            {
                var result = ex.Message;
                return null;                
            }
        }
    }
}