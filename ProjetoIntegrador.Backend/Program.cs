using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.DTOs;
using ProjetoIntegrador.Backend.Extensoes;
using ProjetoIntegrador.Backend.Middlewares;
using ProjetoIntegrador.Backend.Modelos;
using ProjetoIntegrador.Backend.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var stringConexao = Environment.GetEnvironmentVariable("MYSQL_URL") ??
                    throw new InvalidOperationException("A variável de ambiente MYSQL_URL não foi configurada.");

builder.Services.AddDbContext<AppDbContexto>(options => { options.UseMySQL(stringConexao); });

builder.Services.AddScoped<AlertaServico>();
builder.Services.AddScoped<ContatoServico>();
builder.Services.AddScoped<UsuarioServico>();
builder.Services.AddScoped<TokenServico>();
builder.Services.AddScoped<ReceitaServico>();

var jwtKey = Environment.GetEnvironmentVariable("JWT_TOKEN_KEY") ??
             throw new InvalidOperationException("A variavel de ambiente JWT_TOKEN_KEY não foi configurada.");

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

app.UseMiddleware<ErroMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("LiberarAlunos");
    app.UseStaticFiles();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ==========================================
// --- Endpoints ---
// ==========================================

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

// --- Receitas ---

app.MapGet("/receitas", async (ClaimsPrincipal user, ReceitaServico receitaServico) =>
{
    var usuarioId = user.ObterUsuarioId();
    
    var receitasDto = await receitaServico.ObterTodasReceitasAsync(usuarioId);
    return Results.Ok(receitasDto);
}).WithName("PegarReceitas");

app.MapGet("/receitas/{id:int}", (int id) =>
{
    var receitaEstatica = new
    {
        Id = id,
        Titulo = "Bolo de Cenoura Estático (Teste)",
        ImagemUrl = "http://sraedu41780.local/imagens/receita_1.jpg",
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

// --- Favoritos (Autenticados) ---

app.MapGet("/receitas/favoritas", async (ClaimsPrincipal user, ReceitaServico receitaServico) =>
    {
        var usuarioId = user.ObterUsuarioId();
        if (usuarioId is null) return Results.Unauthorized();

        var favoritas = await receitaServico.ObterReceitasFavoritasPorUsuarioAsync(usuarioId.Value);
        return Results.Ok(new
        {
            Receitas = favoritas
        });
    })
    .WithName("PegarReceitasFavoritas")
    .RequireAuthorization();

app.MapPost("/receitas/{id:int}/favoritar", async (int id, ClaimsPrincipal user, ReceitaServico receitaServico) =>
    {
        var strUsuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
        if (!int.TryParse(strUsuarioId, out var usuarioId))
            return Results.Unauthorized();

        await receitaServico.FavoritarReceitaAsync(usuarioId, id);
        return Results.Ok(new { message = "Receita adicionada aos favoritos com sucesso!" });
    })
    .WithName("FavoritarReceita")
    .RequireAuthorization();

app.MapDelete("/receitas/{id:int}/favoritar", async (int id, ClaimsPrincipal user, ReceitaServico receitaServico) =>
    {
        var strUsuarioId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
        if (!int.TryParse(strUsuarioId, out var usuarioId))
            return Results.Unauthorized();

        await receitaServico.DesfavoritarReceitaAsync(usuarioId, id);
        return Results.Ok(new { message = "Receita removida dos favoritos com sucesso!" });
    })
    .WithName("DesfavoritarReceita")
    .RequireAuthorization();

// --- Usuário ---

app.MapPost("/cadastrar", async (UsuarioCadastroDto dados, UsuarioServico servico) =>
    {
        var cadastro = new Usuario(dados.Nome, dados.Email, dados.Senha, dados.Username);
        await servico.AddAsync(cadastro);
        return Results.Created();
    })
    .WithName("InserirDadosUsuario");

app.MapPost("/login", async (UsuarioLoginDto dados, UsuarioServico servico) =>
{
    var usuario = await servico.LoginAsync(dados);
    var resultado = TokenServico.CriarToken(usuario);
    return Results.Ok(new
    {
        usuario.Nome,
        resultado.Token,
        resultado.ExpiresAt
    });
}).WithName("FazerLogin");

// --- Outros Serviços ---

app.MapPost("/contato/cadastrar", async (ContatoDto dados, ClaimsPrincipal user, ContatoServico servico) =>
    {
        var usuarioId = user.ObterUsuarioId();
        if (usuarioId is null) return Results.Unauthorized();

        var contatoEmergencia =
            new Contato(dados.Nome, dados.Vinculo, dados.Telefone, dados.Email, usuarioId.Value);
        await servico.AddAsync(contatoEmergencia);
        return Results.Created();
    })
    .WithName("CadastrarContato")
    .RequireAuthorization();

app.MapGet("/contato/meu", async (ClaimsPrincipal user, ContatoServico contatoServico) =>
    {
        var usuarioId = user.ObterUsuarioId();
        if (usuarioId is null) return Results.Unauthorized();

        var meusContatos = await contatoServico.PegarContatosPorUsuarioIdAsync(usuarioId.Value);
        return Results.Ok(meusContatos);
    })
    .WithName("PegarMeusContatos")
    .RequireAuthorization();

app.MapPut("/contato/{id:int}", async (int id, ContatoDto dados, ClaimsPrincipal user, ContatoServico servico) =>
    {
        var usuarioId = user.ObterUsuarioId();
        if (usuarioId is null) return Results.Unauthorized();

        await servico.AtualizarAsync(id, usuarioId.Value, dados);
        return Results.Ok(new { message = "Contato atualizado com sucesso!" });
    })
    .WithName("AtualizarContato")
    .RequireAuthorization();

app.MapDelete("/contato/{id:int}", async (int id, ClaimsPrincipal user, ContatoServico servico) =>
    {
        var usuarioId = user.ObterUsuarioId();
        if (usuarioId is null) return Results.Unauthorized();

        await servico.RemoverAsync(id, usuarioId.Value);
        return Results.Ok(new { message = "Contato removido com sucesso!" });
    })
    .WithName("RemoverContato")
    .RequireAuthorization();

app.MapPost("/alerta", async (AlertaDto dados, ClaimsPrincipal user, AlertaServico servico) =>
    {
        var usuarioId = user.ObterUsuarioId();
        if (usuarioId is null) return Results.Unauthorized();

        var alertaDados = new Alerta(usuarioId.Value, DateTime.UtcNow, dados.Latitude, dados.Longitude,
            dados.PrecisaoGps);
        await servico.AddAsync(alertaDados);
        return Results.Created();
    })
    .WithName("DispararAlerta")
    .RequireAuthorization();

app.MapPatch("/alerta/{id:int}/status",
        async (int id, AlertaStatusDto dados, ClaimsPrincipal user, AlertaServico servico) =>
        {
            var usuarioId = user.ObterUsuarioId();
            if (usuarioId is null) return Results.Unauthorized();

            if (dados.NovoStatus is null)
                return Results.BadRequest(new
                    { message = "O campo 'novoStatus' é obrigatório e deve ser: 1 (Atendido) ou 2 (FalsoAlarme)." });

            await servico.AtualizarStatusAsync(id, usuarioId.Value, dados.NovoStatus.Value);

            return Results.Ok(new { message = "Status do alerta atualizado com sucesso!" });
        })
    .WithName("AtualizarStatusAlerta")
    .RequireAuthorization();

app.Run();