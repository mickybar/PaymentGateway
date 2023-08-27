using Microsoft.EntityFrameworkCore;
using PaymentGateway.Api.Mappers;
using PaymentGateway.Api.Services;
using PaymentGateway.Data.DbContexts;
using PaymentGateway.Data.Services;

namespace PaymentGateway.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PaymentsContext>(
                dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:PaymentsDBConnectionString"]));

            builder.Services.AddHttpClient(
                "CKOBankClient", c => c.BaseAddress = new Uri(builder.Configuration["CKOBankBaseAddress"]));
            builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            builder.Services.AddScoped<ICKOBankService, CKOBankService>();
            builder.Services.AddTransient<ICardMapper, CardMapper>();
            builder.Services.AddTransient<IPaymentMapper, PaymentMapper>();
            builder.Services.AddTransient<ICardService, CardService>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}