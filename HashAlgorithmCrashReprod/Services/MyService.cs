using HashAlgorithmCrashReprod.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeepCloner = ObjectCloner.ObjectCloner;

namespace HashAlgorithmCrashReprod.Services
{
    public class MyService
    {
        private readonly AccessTokenOptions AccessTokenOptions;

        public MyService(IOptions<AccessTokenOptions> accessTokenOptions)
        {
            AccessTokenOptions = accessTokenOptions.Value;
        }

        public async Task<bool> ValidateAccessTokenAsync(string accessToken)
        {
            TokenValidationParameters validationParameters = DeepCloner.DeepClone(AccessTokenOptions.ValidationParameters);

            JsonWebTokenHandler tokenHandler = new();
            return (await tokenHandler.ValidateTokenAsync(accessToken, validationParameters)).IsValid;
        }

        public string GenerateAccessToken(string userId)
        {
            List<Claim> tokenClaims = new()
            {
                new Claim(AccessTokenOptions.ValidationParameters.NameClaimType, userId)
            };

            JwtSecurityToken token = new JwtSecurityToken
            (
              issuer: AccessTokenOptions.ValidationParameters.ValidIssuer,
              audience: AccessTokenOptions.ValidationParameters.ValidAudience,
              claims: tokenClaims,
              expires: DateTime.UtcNow.AddMinutes(30),
              signingCredentials: new(AccessTokenOptions.ValidationParameters.IssuerSigningKey, AccessTokenOptions.ValidationParameters.ValidAlgorithms.ElementAt(0))
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
