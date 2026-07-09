using ProjetoIntegrador.Backend.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

app.MapPost("/login", (LoginDto dados) =>
{
    try
    {
        var login = new Login(dados.Email, dados.Senha);
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
}).WithName("FazerLogin");

app.MapPost("/contato/cadastrar", (ContatoEmergenciaDto dados) =>
{
    try
    {
        var contatoEmergencia = new ContatoEmergencia(dados.Nome, dados.Vinculo, dados.Telefone, dados.Email);
        return Results.Created();
    }
    catch (Exception e)
    {
        return Results.BadRequest(new {message = e.Message});
    }
}).WithName("CadastrarContato"); 


app.Run();
