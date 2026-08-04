using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class ReceitaServico(AppDbContexto contexto)
{
    public async Task<IEnumerable<ReceitaExibicaoDto>> ObterTodasReceitasAsync()
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