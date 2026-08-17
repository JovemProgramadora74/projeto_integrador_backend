using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Enums;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class AlertaServico(AppDbContexto contexto)
{
    public async Task AddAsync(Alerta dados)
    {
        await contexto.Alertas.AddAsync(dados);
        await contexto.SaveChangesAsync();
    }

    public async Task AtualizarStatusAsync(int alertaId, int usuarioId, Status novoStatus)
    {
        var alerta = await contexto.Alertas.FindAsync(alertaId);

        if (alerta is null)
            throw new Exception("Alerta não encontrado.");

        if (alerta.UsuarioId != usuarioId)
            throw new Exception("Você não tem permissão para alterar este alerta.");

        alerta.AtualizarStatus(novoStatus);
        await contexto.SaveChangesAsync();
    }
}