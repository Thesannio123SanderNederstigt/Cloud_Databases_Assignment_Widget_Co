using API.Mappings;
using API.Middleware;
using Data;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Interfaces;
using Service;
using Service.Interfaces;
using System;

IHost host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(worker => {
        worker.UseNewtonsoftJson();
        worker.UseMiddleware<ExceptionMiddleware>();
    })
    .ConfigureServices(services => {
        string connectionString = Environment.GetEnvironmentVariable("WidgetCoDataBase")!;

        // I considered only having the Processing azure function application directly hooking/connecting up to the sql database,
        // but sending all dto data through a queue to that function isn't really feasable/practical in my opinion,
        // so for demonstration purposes, the postOrder function is the only function doing this (and all the other read/write endpoints directly use/address the services (and repositories))
        services.AddDbContext<DataContext>(opts => {
            opts.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            //opts.EnableSensitiveDataLogging();
        });

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IReviewRepository, ReviewRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IReviewService, ReviewService>();

        services.AddAutoMapper(typeof(MappingProfile));
    })
    .ConfigureOpenApi()
    .Build();

host.Run();