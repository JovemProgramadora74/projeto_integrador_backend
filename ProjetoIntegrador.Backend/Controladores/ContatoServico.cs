using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Controladores;

public class ContatoServico (AppDbContexto _contexto)
{
    public async Task AddAsync(Contato contato)
    {
        await _contexto.Contatos.AddAsync(contato);
        await _contexto.SaveChangesAsync();
    }
}