using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Controladores;

public class UsuarioServico(AppDbContexto contexto)
{
    public async Task<Usuario> LoginAsync(UsuarioLoginDto dados)
    {
        var usuarioBanco = await contexto.Usuarios.FirstOrDefaultAsync(x => x.Email == dados.Email);

        if (usuarioBanco == null) throw new Exception("Usuário não foi encontrado!");
        
        if (usuarioBanco.Senha != dados.Senha)  throw new Exception("Senha incorreta!");
        
        return usuarioBanco;
    }
}