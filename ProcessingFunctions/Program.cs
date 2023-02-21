using Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Interfaces;
using Service;
using Service.Interfaces;
using System;

[assembly: FunctionsStartup(typeof(Functions.Startup))]

namespace Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        string connectionString = Environment.GetEnvironmentVariable("WidgetCoDataBase")!;
        builder.Services.AddDbContext<DataContext>(opts =>
        {
            opts.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
        builder.Services.AddLogging();

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IReviewRepository, ReviewRepository>();

        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddTransient<IReviewService, ReviewService>();
    }
    public static void Main()
    {
    }
}
