using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.DTOs;

namespace ProjetoIntegrador.Backend.Servicos;

public class ReceitaServico(AppDbContexto contexto)
{
    public async Task<IReadOnlyList<ReceitaExibicaoDto>> ObterTodasReceitasAsync()
    {
        return await contexto.Receitas
            .Select(r => new ReceitaExibicaoDto
            {
                Id = r.Id,
                Titulo = r.Titulo,
                ImagemUrl = r.ImagemUrl,
                TagRestricao = r.TagRestricao,
                TempoPreparoMinutos = r.TempoPreparoMinutos,
                Dificuldade = r.Dificuldade,
                Macros = new MacrosDto
                {
                    ProteinaPorcentagem = r.Macros.ProteinaPorcentagem,
                    CarboidratosPorcentagem = r.Macros.CarboidratosPorcentagem,
                    GordurasPorcentagem = r.Macros.GordurasPorcentagem
                }
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<ReceitaExibicaoDto>> ObterReceitasFavoritasPorUsuarioAsync(string usuarioId)
    {
        return await contexto.Receitas
            .Select(r => new ReceitaExibicaoDto
            {
                Id = r.Id,
                Titulo = r.Titulo,
                ImagemUrl = r.ImagemUrl,
                TagRestricao = r.TagRestricao,
                TempoPreparoMinutos = r.TempoPreparoMinutos,
                Dificuldade = r.Dificuldade,
                Macros = new MacrosDto
                {
                    ProteinaPorcentagem = r.Macros.ProteinaPorcentagem,
                    CarboidratosPorcentagem = r.Macros.CarboidratosPorcentagem,
                    GordurasPorcentagem = r.Macros.GordurasPorcentagem
                }
            })
            .ToListAsync();
    }
}