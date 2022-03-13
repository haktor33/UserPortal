using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserPortal.Entities;

namespace UserPortal.Helper
{
    public class Utils
    {
        private readonly AppSettings _appSettings;

        public Utils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateFakeJwtToken(User user)
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NDY5NTUzODYsImV4cCI6MTk1NjU3MDA1MiwiaWF0IjoxNjQ2OTU1Mzg2fQ.gl5eVAwr05x-hbz9oC2AbF_4e-srVnT2g0Gmo6f8IWk";
            return token;
        }

        public static string GetKafkaConfigValue()
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var kafkaServerPath = isDevelopment ? "localhost:9092" : "kafka:9092";
            Console.WriteLine("------------------------------------------------kafka--------------------------------");
            Console.WriteLine($"Kafka server path:{kafkaServerPath}");
            return kafkaServerPath;
        }

    }
}
