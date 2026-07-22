using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Enums;
using ProjetoIntegrador.Backend.Modelos;
using ProjetoIntegrador.Backend.Servicos;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var stringConexao = Environment.GetEnvironmentVariable("MYSQL_URL");

ArgumentNullException.ThrowIfNull(stringConexao);

builder.Services.AddDbContext<AppDbContexto>(options =>
{
    options.UseMySQL(stringConexao);
});

builder.Services.AddScoped<AlertaServico>();

builder.Services.AddScoped<ContatoServico>();

builder.Services.AddScoped<UsuarioServico>();
var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

app.MapPost("/login", async (UsuarioLoginDto dados, UsuarioServico servico) =>
{
    try
    {
        await servico.LoginAsync(dados);
        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
}).WithName("FazerLogin");

app.MapPost("/contato/cadastrar", async (ContatoDto dados, ContatoServico servico) =>
{
    try
    {
        var contatoEmergencia = new Contato(dados.Nome, dados.Vinculo, dados.Telefone, dados.Email, dados.IdUsuario);
        await servico.AddAsync(contatoEmergencia);
        return Results.Created();
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
}).WithName("CadastrarContato");


app.MapPost("/cadastrar", async (UsuarioCadastroDto dados, UsuarioServico servico) =>
    {
        try
        {
            var cadastro = new Usuario(dados.Nome, dados.Email,dados.Senha, dados.Username);
            await servico.AddAsync(cadastro);
            return Results.Created();
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { message = e.Message });
        }
    })
    .WithName("InserirDadosUsuario");

app.MapPost("/alerta", async (AlertaDto dados, AlertaServico servico) =>
{
    try
    {
        var alertaDados = new Alerta(dados.IdUsuario, DateTime.UtcNow, dados.Latitude, dados.Longitude,
            dados.PrecisaoGps, Status.Ativo);
        await servico.AddAsync(alertaDados);
        return Results.Created();
    }
    catch (Exception e)
    {
        return Results.BadRequest(new { message = e.Message });
    }
}).WithName("DispararAlerta");

app.Run();