using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class AlertaServico(AppDbContexto contexto)
{
    public async Task AddAsync(Alerta dados)
    {
        await contexto.Alertas.AddAsync(dados);
        await contexto.SaveChangesAsync();
    }
}