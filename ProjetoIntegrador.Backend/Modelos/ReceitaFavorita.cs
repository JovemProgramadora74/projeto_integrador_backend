namespace ProjetoIntegrador.Backend.Modelos;

public class ReceitaFavorita
{
    protected ReceitaFavorita()
    {
    }

    public ReceitaFavorita(int usuarioId, int receitaId)
    {
        ValidarIds(usuarioId, receitaId);

        UsuarioId = usuarioId;
        ReceitaId = receitaId;
        AdicionadoEm = DateTime.UtcNow;
    }

    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public int ReceitaId { get; private set; }
    public Receita Receita { get; private set; } = null!;

    public DateTime AdicionadoEm { get; private set; } = DateTime.UtcNow;

    private static void ValidarIds(int usuarioId, int receitaId)
    {
        if (usuarioId <= 0)
            throw new Exception("O ID do usuário é inválido!");

        if (receitaId <= 0)
            throw new Exception("O ID da receita é inválido!");
    }
}