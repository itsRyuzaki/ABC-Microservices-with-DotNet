using ABC.Accessories.AutoMapper;
using ABC.Accessories.Data;
using ABC.Accessories.Facade;
using ABC.Accessories.Helpers;
using ABC.Accessories.Models.MongoDb;
using ABC.Accessories.Services;
using ABC.Accessories.Services.Blob;
using ABC.Accessories.Services.MongoDb;
using Microsoft.AspNetCore.Http.Timeouts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddRequestTimeouts(options =>
{
    // Global default policy for all endpoints
    options.DefaultPolicy = new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(10),
        TimeoutStatusCode = 504 // Gateway Timeout (default)
    };
});

// Add Cors
string origin = builder.Configuration.GetSection("AppSettings")
                                    .GetValue<string>("AppOrigin") ?? "";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .WithOrigins(origin)
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// for swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for db
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("ABC-Accessory-MongoDb"));

var mobileDbString = builder.Configuration.GetConnectionString("ABC_Mobiles_DB") ??
                    throw new InvalidOperationException("Connection string 'ABC_Mobiles_DB' not found.");

var pcDbString = builder.Configuration.GetConnectionString("ABC_Computers_DB") ??
                    throw new InvalidOperationException("Connection string 'ABC_Computers_DB' not found.");


builder.Services.AddNpgsql<MobilesDataContext>(mobileDbString);
builder.Services.AddNpgsql<ComputersDataContext>(pcDbString);

// Services for DI
builder.Services.AddAutoMapper(typeof(AccessoriesMapper));
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
builder.Services.AddSingleton<IAccessoriesHelper, AccessoriesHelper>();
builder.Services.AddScoped<IAccessoriesService, AccessoriesService>();
builder.Services.AddScoped<IAccessoriesFacade, AccessoriesFacade>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseRequestTimeouts();

app.UseAuthorization();

app.MapControllers();

app.Run();
