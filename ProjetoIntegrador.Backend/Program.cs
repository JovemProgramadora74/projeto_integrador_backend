using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
builder.Services.AddScoped<ReceitaServico>();

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
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarAlunos", policy => { policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Modo Desenvolvimento");
    app.MapOpenApi();
    app.UseCors("LiberarAlunos");
    app.UseStaticFiles();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// --- Endpoints ---

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

app.MapGet("/receitas", async (ReceitaServico receitaServico) =>
{
    var receitasDto = await receitaServico.ObterTodasReceitasAsync();
    return Results.Ok(receitasDto);
}).WithName("PegarReceitas");

app.MapGet("/receitas/{id:int}", (int id) =>
{
    var receitaEstatica = new
    {
        Id = id,
        Titulo = "Bolo de Cenoura Estático (Teste)",
        ImagemUrl = "http://senac47278.local/imagens/receita_1.jpg",
        TempoPreparoMinutos = 60,
        Dificuldade = "Médio",
        Rendimento = "15 porções",
        Ingredientes = new[]
        {
            "3 cenouras médias",
            "4 ovos",
            "1 xícara de óleo",
            "2 xícaras de açúcar",
            "2 e 1/2 xícaras de farinha de trigo",
            "1 colher de sopa de fermento"
        },
        ModoPreparo = new[]
        {
            "Bata as cenouras, ovos e óleo no liquidificador.",
            "Misture os secos e asse por 40 minutos."
        }
    };

    return Results.Ok(receitaEstatica);
}).WithName("PegarReceitaPorId");

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

app.MapPost("/login", async (UsuarioLoginDto dados, UsuarioServico servico) =>
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

app.MapGet("/receitas/favoritas", async (ClaimsPrincipal user, ReceitaServico receitaServico) =>
    {
        var usuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? user.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(usuarioId)) return Results.Unauthorized();

        var todasReceitas = await receitaServico.ObterReceitasFavoritasPorUsuarioAsync(usuarioId);

        if (todasReceitas.Count == 0) return Results.Ok(Enumerable.Empty<object>());

        var random = new Random();
        var quantidade = random.Next(3, 6);

        var receitasFavoritas = todasReceitas
            .OrderBy(_ => random.Next())
            .Take(quantidade);

        return Results.Ok(new
        {
            Receitas = receitasFavoritas
        });
    })
    .WithName("PegarReceitasFavoritas")
    .RequireAuthorization();

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