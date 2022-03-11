using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserPortal.Context;
using UserPortal.Helper;
using UserPortal.Interfaces;
using UserPortal.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

{
    var services = builder.Services;
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddCors();
    services.AddControllers();
    services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

    services.AddSwaggerGen(swagger =>
    {
        //This is to generate the Default UI of Swagger Documentation    
        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ASP.NET 6 Web API",
            Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
        });
        // To Enable authorization using Swagger (JWT)    
        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
        });
        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                    }
                });
    });
    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    services.AddDbContext<AppDbContext>(x => x.UseNpgsql(connectionString));

    services.AddScoped<IUserService, UserService>();
    services.AddSingleton<IKafkaService, KafkaService>();
    services.AddSingleton<IHostedService, KafkaConsumerHandler>();
    services.AddSingleton<Utils>();

    // Create the ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

    // serviceScopeMock will contain my ServiceProvider
    var serviceScopeMock = new Moq.Mock<IServiceScope>();
    serviceScopeMock.SetupGet<IServiceProvider>(s => s.ServiceProvider)
        .Returns(serviceProvider);

    // serviceScopeFactoryMock will contain my serviceScopeMock
    var serviceScopeFactoryMock = new Moq.Mock<IServiceScopeFactory>();
    serviceScopeFactoryMock.Setup(s => s.CreateScope())
        .Returns(serviceScopeMock.Object);
}




var app = builder.Build();

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseDeveloperExceptionPage();
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // custom jwt auth middleware
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseMiddleware<JwtMiddleware>();
}

app.Run();
