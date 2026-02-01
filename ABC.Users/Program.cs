using ABC.Users.ABCMapper;
using ABC.Users.Facade;
using ABC.Users.Middleware;
using ABC.Users.Models;
using ABC.Users.Services;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Advertise supported versions in the response headers (api-supported-versions)
    options.ReportApiVersions = true;

    // CONFIGURE HEADER READER: Use "X-Api-Version" as the custom header
    options.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
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
builder.Services.Configure<UsersDatabaseSettings>(
    builder.Configuration.GetSection("ABC-Users-MongoDb"));

/** 
    Add Services to DI here 
*/

builder.Services.AddAutoMapper(typeof(ABCMapper));
builder.Services.AddSingleton<IMongoDBService, MongoDbService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserFacade, UserFacade>();


// Post builder 
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

app.UseCors();

app.UseAuthorization();

app.UseMiddleware<HttpSessionMiddleware>();

app.MapControllers();

app.Run();
