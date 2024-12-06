using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRMAudax.Tools
{
    public class AuthTools
    {
        public static string GenerateToken(string username)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            string jwtKey = configuration.GetSection("JWTAth")["JWT_Key"] ?? "";
            string jwtIssuer = configuration.GetSection("JWTAth")["JWT_Issuer"]  ?? "";
            string jwtAudience = configuration.GetSection("JWTAth")["JWT_Audience"]  ?? "";
            string jwtTokenExpiryInMinutes = configuration.GetSection("JWTAth")["JWT_TokenExpiryInMinutes"]  ?? "";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int tokenExpiryInMinutes = Convert.ToInt32(jwtTokenExpiryInMinutes);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, username),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
