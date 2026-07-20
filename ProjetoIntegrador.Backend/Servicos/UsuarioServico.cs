using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class UsuarioServico(AppDbContexto contexto)
{
    public async Task AddAsync(Usuario dados)
    {
        var emailExiste = await contexto.Usuarios.AnyAsync(x => x.Email == dados.Email);
        if (emailExiste) throw new Exception("Este e-mail já está cadastrado por outro usuário!");

        var usernameExiste = await contexto.Usuarios.AnyAsync(x => x.Username == dados.Username);
        if (usernameExiste) throw new Exception("Este nome de usuário já está em uso!");
        
        await contexto.Usuarios.AddAsync(dados);
        await contexto.SaveChangesAsync();
    }

    public async Task<Usuario> LoginAsync(UsuarioLoginDto dados)
    {
        var usuario = await contexto.Usuarios.FirstOrDefaultAsync(x => x.Email == dados.Email);

        if (usuario == null) throw new Exception("Usuário não foi encontrado!");

        if (usuario.Senha != dados.Senha) throw new Exception("Senha incorreta!");

        return usuario;
    }
}