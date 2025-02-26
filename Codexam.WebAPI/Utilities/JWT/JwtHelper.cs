using Codexam.WebAPI.Entities;
using Codexam.WebAPI.Utilities.Encryption;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Codexam.WebAPI.Utilities.JWT
{
    public class JwtHelper : ITokenHelper
    {

        private readonly TokenOptions _tokenOptions;
        public JwtHelper(TokenOptions tokenOptions)
        {
            _tokenOptions = tokenOptions;
        }
        public AccessToken CreateToken(User user)
        {

            DateTime expirationTime = DateTime.Now.AddMinutes(_tokenOptions.ExpirationTime);
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: SetAllClaims(user),
                notBefore: DateTime.Now,
                expires: expirationTime,
                signingCredentials: signingCredentials
                );
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            string jwtToken = jwtSecurityTokenHandler.WriteToken(jwt);
            return new AccessToken() { Token = jwtToken, ExpirationTime = expirationTime };
        }

        protected IEnumerable<Claim> SetAllClaims(User user)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
            return claims;
        }
    }
}
