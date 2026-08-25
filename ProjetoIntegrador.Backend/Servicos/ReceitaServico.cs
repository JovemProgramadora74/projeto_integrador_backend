using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.DTOs;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class ReceitaServico(AppDbContexto contexto)
{
    public async Task<IReadOnlyList<ReceitaExibicaoDto>> ObterTodasReceitasAsync(int? usuarioId)
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
                Curtido = usuarioId != null && r.FavoritadoPor.Any(c => c.UsuarioId == usuarioId),
                Macros = new MacrosDto
                {
                    ProteinaPorcentagem = r.Macros.ProteinaPorcentagem,
                    CarboidratosPorcentagem = r.Macros.CarboidratosPorcentagem,
                    GordurasPorcentagem = r.Macros.GordurasPorcentagem
                }
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<ReceitaExibicaoDto>> ObterReceitasFavoritasPorUsuarioAsync(int usuarioId)
    {
        return await contexto.ReceitasFavoritas
            .Where(rf => rf.UsuarioId == usuarioId)
            .Select(rf => new ReceitaExibicaoDto
            {
                Id = rf.Receita.Id,
                Titulo = rf.Receita.Titulo,
                ImagemUrl = rf.Receita.ImagemUrl,
                TagRestricao = rf.Receita.TagRestricao,
                TempoPreparoMinutos = rf.Receita.TempoPreparoMinutos,
                Dificuldade = rf.Receita.Dificuldade,
                Curtido = rf.UsuarioId == usuarioId,
                Macros = new MacrosDto
                {
                    ProteinaPorcentagem = rf.Receita.Macros.ProteinaPorcentagem,
                    CarboidratosPorcentagem = rf.Receita.Macros.CarboidratosPorcentagem,
                    GordurasPorcentagem = rf.Receita.Macros.GordurasPorcentagem
                }
            })
            .ToListAsync();
    }

    public async Task FavoritarReceitaAsync(int usuarioId, int receitaId)
    {
        var receitaExiste = await contexto.Receitas
            .AnyAsync(r => r.Id == receitaId);

        if (!receitaExiste)
            throw new Exception("A receita informada não foi encontrada!");

        var jaFavoritada = await contexto.ReceitasFavoritas
            .AnyAsync(rf => rf.UsuarioId == usuarioId && rf.ReceitaId == receitaId);

        if (jaFavoritada)
            throw new Exception("Esta receita já está salva nos seus favoritos!");

        var novoFavorito = new ReceitaFavorita(usuarioId, receitaId);

        await contexto.ReceitasFavoritas.AddAsync(novoFavorito);
        await contexto.SaveChangesAsync();
    }

    public async Task DesfavoritarReceitaAsync(int usuarioId, int receitaId)
    {
        var favorito = await contexto.ReceitasFavoritas
            .FirstOrDefaultAsync(rf => rf.UsuarioId == usuarioId && rf.ReceitaId == receitaId);

        if (favorito is null)
            throw new Exception("Esta receita não está na sua lista de favoritos!");

        contexto.ReceitasFavoritas.Remove(favorito);
        await contexto.SaveChangesAsync();
    }

    public async Task<ReceitaCompletaDto?> ObterReceitaCompletaPorIdAsync(int id)
    {
        return await contexto.Receitas.Select(receita => new ReceitaCompletaDto
        {
            Id = id,
            Titulo = receita.Titulo,
            ImagemUrl = receita.ImagemUrl,
            TagRestricao = receita.TagRestricao,
            TempoPreparoMinutos = receita.TempoPreparoMinutos,
            Dificuldade = receita.Dificuldade,
            Macros = new MacrosDto
            {
                CarboidratosPorcentagem = receita.Macros.CarboidratosPorcentagem,
                GordurasPorcentagem = receita.Macros.GordurasPorcentagem,
                ProteinaPorcentagem = receita.Macros.ProteinaPorcentagem,
            },
            Ingredientes = receita.Ingredientes,
            Passos = receita.Passos,
        }).FirstOrDefaultAsync(receita => receita.Id == id);
    }
}