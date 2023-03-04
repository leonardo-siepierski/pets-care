using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using pets_care.Auth;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow Cors
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
    {
        build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }
));



// Auth Config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")))
    };
});

// Adicionar POLICY CLAIMS BASED aqui! Controller annotation => [Authorize(Policy = "Client")]
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Client", 
//         policy => policy.RequireClaim(ClaimTypes.Role, "USER"));
//     options.AddPolicy("Admin", 
//         policy => policy.RequireClaim(ClaimTypes.Role, "ADMIN"));
// });

// DB
builder.Services.AddDbContext<PetCareContext>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

// ENVIROMENT SET
DotNetEnv.Env.Load();

using (var db = new PetCareContext())
{
    // Código executado no banco de dados aqui
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    var cultureInfo = new CultureInfo("pt-BR");

    var clientRepository = new ClientRepository();
    var salt = DateTime.Now;
    Console.WriteLine(salt);

    var client = new Client(){ClientId = Guid.NewGuid() , Name = "Beto Andrade", Adress = "Rua das ediondas", Email = "beto@gmail.com", Cep = "04321020", Password = clientRepository.HashPassword("senha1", $"{salt}"), Role = "USER" , CreatedAt = salt, ModifiedAt = salt };
    var client2 = new Client(){ClientId = Guid.NewGuid() , Name = "Joana Martins", Adress = "Rua das flores", Email = "joana@gmail.com", Cep = "14730000", Password = clientRepository.HashPassword("senha2", $"{salt}"), Role = "USER", CreatedAt = salt, ModifiedAt = salt };
    db.Clients.Add(client);
    db.Clients.Add(client2);
    db.SaveChanges();
}


// HTTPCLIENTS
builder.Services.AddHttpClient<IViaCepService, ViaCepService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");


app.UseHttpsRedirection();

// ordem importa
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// culture
// var cultureInfo = new CultureInfo("pt-BR");
// CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
// CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
// Console.WriteLine("First day of the: " + cultureInfo.DateTimeFormat.FirstDayOfWeek.ToString());
// Console.WriteLine("First calendar week starts with: " + cultureInfo.DateTimeFormat.CalendarWeekRule.ToString());
// Console.WriteLine("First calendar week starts with: " + DateTime.Now);

app.Run();

public partial class Program { }