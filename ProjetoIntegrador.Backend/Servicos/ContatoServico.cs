using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class ContatoServico (AppDbContexto contexto)
{
    public async Task AddAsync(Contato contato)
    {
        await contexto.Contatos.AddAsync(contato);
        await contexto.SaveChangesAsync();
    }
}