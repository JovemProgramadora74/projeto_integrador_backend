using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Enums;
using ProjetoIntegrador.Backend.Modelos;
using ProjetoIntegrador.Backend.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var stringConexao = Environment.GetEnvironmentVariable("MYSQL_URL");
ArgumentNullException.ThrowIfNull(stringConexao);

builder.Services.AddDbContext<AppDbContexto>(options => { options.UseMySQL(stringConexao); });


builder.Services.AddScoped<AlertaServico>();
builder.Services.AddScoped<ContatoServico>();
builder.Services.AddScoped<UsuarioServico>();
builder.Services.AddScoped<TokenServico>();

var jwtKey = Environment.GetEnvironmentVariable("JWT_TOKEN_KEY") ??
             throw new Exception("A chave JWT não está configurada corretamente!");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

app.MapPost("/cadastrar", async (UsuarioCadastroDto dados, UsuarioServico servico) =>
    {
        try
        {
            var cadastro = new Usuario(dados.Nome, dados.Email, dados.Senha, dados.Username);
            await servico.AddAsync(cadastro);
            return Results.Created();
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { message = e.Message });
        }
    })
    .WithName("InserirDadosUsuario");

app.MapPost("/login", async (UsuarioLoginDto dados, UsuarioServico servico, TokenServico jwtService) =>
{
    try
    {
        var usuario = await servico.LoginAsync(dados);
        var resultado = TokenServico.CriarToken(usuario);
        return Results.Ok(new
        {
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            resultado.Token,
            resultado.ExpiresAt
        });
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
            var contatoEmergencia =
                new Contato(dados.Nome, dados.Vinculo, dados.Telefone, dados.Email, dados.IdUsuario);
            await servico.AddAsync(contatoEmergencia);
            return Results.Created();
        }
        catch (Exception e)
        {
            return Results.BadRequest(new { message = e.Message });
        }
    })
    .WithName("CadastrarContato")
    .RequireAuthorization();

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
    })
    .WithName("DispararAlerta")
    .RequireAuthorization();

app.Run();