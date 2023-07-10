
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LenzoGlobalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
            var domains = builder.Configuration.GetValue<string>("AcceptedDomains").Split(';');

            // AspNetCoreRateLimit
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();
            var DoAddCORS = builder.Configuration.GetValue<bool>("DoAddCORS");
            if (DoAddCORS)
            {
                builder.Services.AddCors(options =>
                    {
                        options.AddPolicy(name: MyAllowSpecificOrigins,
                                          policy =>
                                          {
                                              policy.WithOrigins(domains).AllowAnyHeader().AllowAnyMethod();
                                          });
                    });
            }
            // Add services to the container.
            var DoRestrict = builder.Configuration.GetValue<bool>("DoRestrict");
            if (DoRestrict)
            {
                builder.Services.AddControllers(option =>
                {
                    option.Filters.Add(new RestrictDomainAttribute(domains));
                });
            }
            else
            {
                builder.Services.AddControllers();
            }

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
                options.EnableDetailedErrors();
            });


            //builder.Services.AddRouting(option "", "{controller=Home}/{action=Index}");

            //builder.Services.Configure<Domains>(options => builder.Configuration.GetSection("Domains").Bind(options));

            //builder.Services.AddSwaggerGen();

            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseIpRateLimiting();

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();



            app.Run();








            //var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.

            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            //var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseIpRateLimiting();



            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            //app.MapControllers();

            //app.Run();
        }
    }
}