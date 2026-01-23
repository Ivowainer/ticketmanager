using Microsoft.EntityFrameworkCore;
using TicketManager.Data;
using TicketManager.Data.Seed;
using TicketManager.Repositories;
using TicketManager.Repositories.Interfaces;
using TicketManager.Services;
using TicketManager.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection String 'DefaultConnection' not found");
builder.Services.AddDbContext<TicketManagerDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

/*using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<TicketManagerDbContext>();

    await RoleSeed.SeedAsync(context);
    await UserSeed.SeedAsync(context);
}*/

app.Run();