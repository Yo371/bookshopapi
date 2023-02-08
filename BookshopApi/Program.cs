using System.Text.Json.Serialization;
using BookshopApi;
using BookshopApi.DataAccess;
using BookshopApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSqlServer<BookShopContext>(configuration.GetConnectionString("UserContext"));

builder.Services.AddScoped<IStoreItemService, StoreItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddMvc().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//auth changes
builder.Services.AddVersionedApiExplorer(e =>
{
    e.GroupNameFormat = "'v'VVV";
    e.SubstituteApiVersionInUrl = true;
    e.AssumeDefaultVersionWhenUnspecified = true;
    e.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddApiVersioning(e =>
{
    e.ReportApiVersions = true;
    e.AssumeDefaultVersionWhenUnspecified = true;
    e.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddSwaggerGen(e =>
{
    e.EnableAnnotations();
    e.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookshopAPI",
        Version = "v1",
        Description = "Description"
    });

    e.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Auth Header"
    });

    e.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization(options =>
    options.AddPolicy("BasicAuthentication",
        new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build()));
builder.Services.AddControllers();
builder.Services.AddRouting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(e =>
    {
        e.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
    });
}

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();