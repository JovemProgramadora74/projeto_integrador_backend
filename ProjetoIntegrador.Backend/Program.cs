using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var stringConexao = Environment.GetEnvironmentVariable("POSTGRES_URI");

ArgumentNullException.ThrowIfNull(stringConexao);

builder.Services.AddDbContext<AppDbContexto>(options =>
{
    options.UseNpgsql(stringConexao);
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

app.MapPost("/login", (UsuarioLoginDto dados) =>
{
    try
    {
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }

    return Results.Created();
}).WithName("FazerLogin");

app.MapPost("/contato/cadastrar", (ContatoDto dados) =>
{
    try
    {
        var contatoEmergencia = new Contato(dados.Nome, dados.Vinculo, dados.Telefone, dados.Email);
        return Results.Created();
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
}).WithName("CadastrarContato");


app.MapPost("/cadastrar", (UsuarioCadastroDto dados) =>
    {
        try
        {
            var cadastro = new Usuario(dados.Nome, dados.Email, dados.Senha, dados.Username);
            return Results.Created();
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { message = e.Message });
        }
    })
    .WithName("InserirDadosUsuario");

app.MapPost("/alerta", (AlertaDto dados) =>
{
    try
    {
        var novoAlerta = new Alerta(
            dados.IdUsuario,
            DateTime.UtcNow, 
            dados.Latitude,
            dados.Longitude,
            dados.PrecisaoGps,
            dados.Status
        );
        
        return Results.Created();
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
}).WithName("DispararAlerta");

app.Run();