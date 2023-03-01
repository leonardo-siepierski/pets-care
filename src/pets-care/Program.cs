using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3e35d94786e36fdc4560abf7e910c3a7"))
    };
});

// Adicionar POLICY CLAIMS BASED aqui!
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Client", 
//         policy => policy.RequireClaim("ROLE", "USER"));
// });

// DB
builder.Services.AddDbContext<PetCareContext>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// ENVIROMENT SET
DotNetEnv.Env.Load();

using (var db = new PetCareContext())
{
    // Código executado no banco de dados aqui
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    var clientRepository = new ClientRepository();
    var salt = DateTime.Now.ToString();

    var client = new Client(){ClientId = Guid.NewGuid() , Name = "Beto Andrade", Adress = "Rua das ediondas", Email = "beto@gmail.com", Cep = "04321020", Password = clientRepository.HashPassword("senha",$"{salt}"), Role = "USER" ,CreatedAt = salt, ModifiedAt = salt };
    var client2 = new Client(){ClientId = Guid.NewGuid() , Name = "Joana Martins", Adress = "Rua das flores", Email = "joana@gmail.com", Cep = "14730000", Password = clientRepository.HashPassword("senha",$"{salt}"), Role = "USER",CreatedAt = salt, ModifiedAt = salt };
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

app.Run();
