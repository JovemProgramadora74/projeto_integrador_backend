using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Dados;
using ProjetoIntegrador.Backend.DTOs;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Servicos;

public class ContatoServico(AppDbContexto contexto)
{
    public async Task AddAsync(Contato contato)
    {
        await contexto.Contatos.AddAsync(contato);
        await contexto.SaveChangesAsync();
    }

    public async Task AtualizarAsync(int contatoId, int usuarioId, ContatoDto dados)
    {
        var contato = await contexto.Contatos.FindAsync(contatoId);

        if (contato is null)
            throw new Exception("Contato não encontrado");

        if (contato.UsuarioId != usuarioId)
            throw new Exception("Você não tem permissão para alterar este contato");

        contato.AtualizarContato(dados.Nome, dados.Vinculo, dados.Telefone, dados.Email);
        await contexto.SaveChangesAsync();
    }

    public async Task RemoverAsync(int contatoId, int usuarioId)
    {
        var contato = await contexto.Contatos.FindAsync(contatoId);

        if (contato is null)
            throw new Exception("Contato não encontrado");

        if (contato.UsuarioId != usuarioId)
            throw new Exception("Você não tem permissão para remover este contato");

        contexto.Contatos.Remove(contato);
        await contexto.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<ContatoRespostaDto>> PegarContatosPorUsuarioIdAsync(int usuarioId)
    {
        return await contexto.Contatos
            .AsNoTracking()
            .Where(contato => contato.UsuarioId == usuarioId)
            .Select(c => new ContatoRespostaDto
            {
                Id = c.Id,
                Email = c.Email,
                Nome = c.Nome,
                Vinculo = c.Vinculo,
                Telefone = c.Telefone
            })
            .ToListAsync();
    }
}