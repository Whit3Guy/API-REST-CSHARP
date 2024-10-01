using ApiCrudDotNET.database;
using ApiCrudDotNET.Estudantes;

var builder = WebApplication.CreateBuilder(args);

// Adicionar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGetOnly",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar CORS antes das rotas
app.UseHttpsRedirection();
app.UseCors("AllowGetOnly");  // Middleware CORS deve vir antes das rotas

// Configurar rotas
app.AddEstudantesEndpoint();

app.Run();
