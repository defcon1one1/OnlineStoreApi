using OnlineStore.Domain.Extensions;
using OnlineStore.Domain.Interfaces.Shared;
using OnlineStore.Infrastructure.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var seederService = serviceProvider.GetRequiredService<IDataSeederService>();

    // Call the Seed method
    await seederService.Seed();
}

app.Run();

// After the application has run, resolve the ISeederService and call Seed method
