using HashAlgorithmCrashReprod.Helpers;
using HashAlgorithmCrashReprod.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace HashAlgorithmCrashReprod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<MyService>();

            AddAuthentication(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }

        private static void AddAuthentication(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            byte[] tokenSigningKeyBytes = Encoding.UTF8.GetBytes(configuration["AccessTokenSigningKey"]);
            SymmetricSecurityKey tokenSigningKey = new SymmetricSecurityKey(tokenSigningKeyBytes);

            TokenValidationParameters tokenValidationParameters = new()
            {
                NameClaimType = ClaimTypes.NameIdentifier,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAlgorithms = new string[] { "HS256" },
                ValidIssuer = "My Issuer",
                ValidAudience = "My Audience",
                IssuerSigningKey = tokenSigningKey,
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                }
            };

            serviceCollection.Configure<AccessTokenOptions>(x =>
            {
                x.ValidationParameters = tokenValidationParameters;
            });

            serviceCollection.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => o.TokenValidationParameters = tokenValidationParameters);
        }
    }
}