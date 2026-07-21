using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class TokenServico
{
    // Método adaptado: recebe o e-mail do usuário e uma lista de permissões/roles
    public static (string Token, DateTime ExpiresAt) CriarToken(Usuario usuario)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(60);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
        };

        // Chave secreta direta para teste (mínimo 32 caracteres)
        var tokenKey = Environment.GetEnvironmentVariable("JWT_TOKEN_KEY")?? throw new InvalidOperationException("A chave secreta do token não foi configurada");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);

        return (token, expiresAt);
    }
}