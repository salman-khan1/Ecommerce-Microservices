using Mango.Services.AuthAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtoptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtoptions)
        {
            _jwtoptions = jwtoptions.Value;
        }

        public string GenrateToken(ApplicationUsers applicationUsers,IEnumerable<string> roles)
        {
            var tokenHandler= new JwtSecurityTokenHandler();
            var key= Encoding.ASCII.GetBytes(_jwtoptions.Secret);
            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUsers.Email),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUsers.Id),
                new Claim(JwtRegisteredClaimNames.Name,applicationUsers.UserName)
            };
            claimList.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtoptions.Audience,
                Issuer = _jwtoptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
          
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
