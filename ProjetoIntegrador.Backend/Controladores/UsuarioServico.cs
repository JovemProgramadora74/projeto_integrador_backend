using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Controladores;

public class UsuarioServico(AppDbContexto _contexto)
{
    public async Task AddAsync(Usuario dados)
    {
     await   _contexto.Usuarios.AddAsync(dados);
     await _contexto.SaveChangesAsync();
     
    }
}