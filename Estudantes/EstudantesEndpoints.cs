using System.Text.Json;
using System.Text.Json.Serialization;
using ApiCrudDotNET.database;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudDotNET.Estudantes;

public static class EstudantesEndpoints
{           
    public static void AddEstudantesEndpoint( this WebApplication app)
    {

        //agrupador de rotas
        var rotasEstudantes = app.MapGroup("estudantes");

        // quero criar um post
            rotasEstudantes.MapPost("", async (AddEstudanteRequest req, AppDbContext context) => 

        {
            var exists = await context.Estudantes
            .AnyAsync(estudante => estudante.Nome == req.Nome);
            if(exists){
                return Results.Conflict("Já existe");

            }
            try{
            var novoEstudante = new Estudante(req.Nome);
            
            await context.Estudantes.AddAsync(novoEstudante);
            await context.SaveChangesAsync();
        
            return Results.Ok(novoEstudante);
            }
            catch(Exception e){
                Console.WriteLine(e);
                return Results.UnprocessableEntity("Ponha o nome, jegue");
            }
        });

        // get all
        rotasEstudantes.MapGet("", async (AppDbContext context) =>
        {
            var estudantes = await context.Estudantes
            .Where(estudante => estudante.Ativo == true).ToListAsync();
            return estudantes;
            
        });

        // get individual
        rotasEstudantes.MapGet("/{id}", async (Guid id, AppDbContext context) =>
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id);
            return estudante;
            
        });


        rotasEstudantes.MapPut("/{id}", async (Guid id, UpdateEstudanteRequest req, AppDbContext context)=>
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => id == estudante.Id);

            if(estudante == null )
            {
                return Results.NotFound("id não encontrado");
            }

            estudante.MudarNome(req.Nome); 
            return Results.Ok(estudante);
            
        });



    }
}