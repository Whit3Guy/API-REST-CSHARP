namespace ApiCrudDotNET.Estudantes;

public static class EstudantesEndpoints
{
    public static void AddEstudantesEndpoint( this WebApplication app)
    {
        app.MapGet("Estudantes", () => new Estudante("Estudante1"));
        
    }
}