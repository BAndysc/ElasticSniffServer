using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;
using Server.Services.Elastic;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
var connstring = builder.Configuration["Database:connection"];
builder.Services.AddDbContext<DatabaseContext>(options => options.UseMySql(connstring, ServerVersion.AutoDetect(connstring)));
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IElasticFactory, ElasticFactory>();
builder.Services.AddTransient<IUploadService, ElasticUploadService>();
builder.Services.AddTransient<ISearchService, ElasticSearchService>();
builder.Services.AddTransient<IRandomService, RandomService>();

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// initialize db
var scope = app.Services.GetRequiredService<IServiceProvider>().CreateScope();
await scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.MigrateAsync();
scope.Dispose();

// debug create user
{
    var userService = app.Services.GetRequiredService<IUserService>();
    if (!await userService.UserExists("admin"))
    {
        var random = new RandomService();
        var password = random.GenerateRandomString(128);
        await userService.AddAdminUser("admin", password);
        Console.WriteLine("Created admin user with password: " + password);
    }
}

app.Run();