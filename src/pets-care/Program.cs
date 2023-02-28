using pets_care.Models;
using pets_care.Repository;
using pets_care.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

    var client = new Client(){ClientId = Guid.NewGuid() , Name = "Beto", Adress = "Rua das ediondas", Email = "beto@gmail.com", Cep = "04321020", Password = "rwarwagvaw", CreatedAt = DateTime.Now.ToString(), ModifiedAt = DateTime.Now.ToString() };
    db.Clients.Add(client);
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
