using System.Security.Claims;

namespace ProjetoIntegrador.Backend.Extensoes;

public static class ClaimsPrincipalExtensions
{
    public static int? ObterUsuarioId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user.FindFirst("sub")?.Value;

        if (int.TryParse(idClaim, out var usuarioId))
            return usuarioId;

        return null;
    }
}