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
        var all = app.MapGroup("all");

        // quero criar um post
            rotasEstudantes.MapPost("", async (AddEstudanteRequest req, AppDbContext context, CancellationToken ct) => 

        {
            var exists = await context.Estudantes
            .AnyAsync(estudante => estudante.Nome == req.Nome, ct);
            if(exists){
                return Results.Conflict("Já existe");

            }
            try
            
            {
            var novoEstudante = new Estudante(req.Nome);
            
            await context.Estudantes.AddAsync(novoEstudante, ct);
            await context.SaveChangesAsync(ct);

            var result_estudante = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);
        
            return Results.Ok(result_estudante);

            }

            catch(Exception e){
                Console.WriteLine(e);
                return Results.UnprocessableEntity("Ponha o nome, jegue");
            }
        });

        // get all
        rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) =>
        {
            //Agora, pra mudar os estudantes para o tipo estudantedto, em js usariamos o map, aqui, usaremos o select
            var estudantes = await context.Estudantes
            .Where(estudante => estudante.Ativo == true)
            .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
            .ToListAsync(ct);
            return estudantes;
            
        });


        all.MapGet("", async (AppDbContext context, CancellationToken ct) =>
        {
            //Agora, pra mudar os estudantes para o tipo estudantedto, em js usariamos o map, aqui, usaremos o select
            var estudantes = await context.Estudantes
            .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
            .ToListAsync(ct);
            return Results.Ok(estudantes);
            
        });

        // get individual
        rotasEstudantes.MapGet("/{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
        {
            
            var estudante = await context.Estudantes
            .SingleOrDefaultAsync(estudante => estudante.Id == id,ct);
            

            if(estudante == null){

                return Results.NotFound("Estudante não encontrado");
            }

            var estudante_retorno =  new EstudanteDto(estudante.Id, estudante.Nome);

            return Results.Ok(estudante_retorno);
            
        });


        rotasEstudantes.MapPut("/{id}", async (Guid id, UpdateEstudanteRequest req, AppDbContext context, CancellationToken ct)=>
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => id == estudante.Id, ct);

            if(estudante == null )
            {
                return Results.NotFound("id não encontrado");
            }


            estudante.MudarNome(req.Nome); 
            await context.SaveChangesAsync(ct);


            var estudante_result = new EstudanteDto(estudante.Id, estudante.Nome);

            return Results.Ok(estudante_result);
            
        });


        // fazendo um soft delete
        rotasEstudantes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) => 
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id , ct);

            if(estudante == null){

                return Results.NotFound("Estudante não encontrado");
            }

            if(estudante.Ativo == false){
                return Results.Ok("O estudante já está desativado");
            }

            estudante.Desativar();
            await context.SaveChangesAsync(ct);

            var estudante_retorno = new EstudanteDto(estudante.Id, estudante.Nome);



            return Results.Ok(estudante_retorno);

        });



    }
}