using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SupportTicket.Services;
using System.Text;

namespace SupportTicket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("a-placeholder-dev-signing-key-for-supportticket"))
                    };
                });

            // Fix: use generic overload
            builder.Services.AddScoped<ITicketService, TicketService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data source=tickets.db"));

            builder.Services.Configure<TicketOptions>
                (builder.Configuration.GetSection("TicketOptions"));

            builder.Services.AddHealthChecks().AddCheck<TicketServiceHealthCheck>("ticketService");

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseMiddleware<RequestTimingMiddleware>();  

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHealthChecks("/health");

            app.MapControllers();

            app.Run();
        }
    }
}
