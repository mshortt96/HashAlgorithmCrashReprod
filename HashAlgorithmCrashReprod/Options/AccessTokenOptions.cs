using Microsoft.IdentityModel.Tokens;

namespace HashAlgorithmCrashReprod.Helpers
{
    public class AccessTokenOptions
    {
        public TokenValidationParameters ValidationParameters { get; set; }
    }
}
