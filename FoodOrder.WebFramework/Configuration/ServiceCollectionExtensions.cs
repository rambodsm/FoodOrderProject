using FoodOrder.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FoodOrder.WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddMinimalMvc(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //options.Filters.Add(new AuthorizeFilter());
            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            //services.AddSwaggerGenNewtonsoftSupport();
        }

        //public static void AddJwtAuthentication(this IServiceCollection services, JwtSetting jwtSettings)
        //{
        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //    }).AddJwtBearer(options =>
        //    {
        //        var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        //        var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ClockSkew = TimeSpan.Zero,
        //            RequireSignedTokens = true,
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        //            RequireExpirationTime = true,
        //            ValidateLifetime = true,
        //            ValidateAudience = true,
        //            ValidAudience = jwtSettings.Audience,
        //            ValidateIssuer = true,
        //            ValidIssuer = jwtSettings.Issuer,
        //            TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
        //        };

        //        options.RequireHttpsMetadata = false;
        //        options.SaveToken = true;
        //        options.TokenValidationParameters = validationParameters;
        //        options.Events = new JwtBearerEvents
        //        {
        //            OnAuthenticationFailed = context =>
        //            {
        //                //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
        //                //logger.LogError("Authentication failed.", context.Exception);

        //                if (context.Exception != null)
        //                    throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

        //                return Task.CompletedTask;
        //            },
        //            OnTokenValidated = async context =>
        //            {
        //                var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
        //                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

        //                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
        //                if (claimsIdentity.Claims?.Any() != true)
        //                    context.Fail("This token has no claims.");

        //                var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType).ToString();
        //                if (!securityStamp.HasValue())
        //                    context.Fail("This token has no security stamp");

        //                //Find user and token from database and perform your custom validation
        //                var userId = claimsIdentity.GetUserId<int>();
        //                var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

        //                //if (user.SecurityStamp != Guid.Parse(securityStamp))
        //                //    context.Fail("Token security stamp is not valid.");

        //                var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
        //                if (validatedUser == null)
        //                    context.Fail("Token security stamp is not valid.");

        //                if (!user.IsActive)
        //                    context.Fail("User is not active.");

        //                await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
        //            },
        //            OnChallenge = context =>
        //            {
        //                if (context.AuthenticateFailure != null)
        //                    throw new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
        //                throw new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);
        //            }
        //        };
        //    });
        //}
    }
}
