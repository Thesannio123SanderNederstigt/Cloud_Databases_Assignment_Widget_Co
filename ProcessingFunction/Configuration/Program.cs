using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;
using Service;
using Service.Interfaces;
using System;

IHost host = new HostBuilder()
    .ConfigureServices(services => {
        string connectionString = Environment.GetEnvironmentVariable("WidgetCoDataBase")!;

        services.AddDbContext<DataContext>(opts => {
            opts.UseSqlServer(connectionString);
            opts.EnableSensitiveDataLogging();
        });

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IReviewRepository, ReviewRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IReviewService, ReviewService>();

        services.AddAutoMapper(typeof(Program));
    })
    .Build();

host.Run();