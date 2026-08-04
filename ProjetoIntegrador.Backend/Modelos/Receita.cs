namespace ProjetoIntegrador.Backend.Modelos;

public class Receita
{
    protected Receita()
    {
    }

    public Receita(string titulo, string imagemUrl, string? tagRestricao, int tempoPreparoMinutos, string dificuldade,
        MacrosNutricionais macros)
    {
        ValidarInserirTitulo(titulo);
        ValidarInserirImagemUrl(imagemUrl);
        TagRestricao = tagRestricao; // Pode ser nulo se não houver restrição
        ValidarInserirTempoPreparo(tempoPreparoMinutos);
        ValidarInserirDificuldade(dificuldade);
        Macros = macros ?? throw new Exception("Os dados macro-nutricionais não podem ser nulos!");
    }

    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string ImagemUrl { get; private set; } = string.Empty;
    public string? TagRestricao { get; private set; }
    public int TempoPreparoMinutos { get; private set; }
    public string Dificuldade { get; private set; } = string.Empty;

    // Propriedade de Navegação do Objeto de Valor
    public MacrosNutricionais Macros { get; private set; } = null!;

    private void ValidarInserirTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("O título da receita não pode estar vazio!");

        Titulo = titulo;
    }

    private void ValidarInserirImagemUrl(string imagemUrl)
    {
        if (string.IsNullOrWhiteSpace(imagemUrl))
            throw new Exception("A URL da imagem não pode estar vazia!");

        ImagemUrl = imagemUrl;
    }

    private void ValidarInserirTempoPreparo(int tempo)
    {
        if (tempo <= 0)
            throw new Exception("O tempo de preparo deve ser maior que zero minutos!");

        TempoPreparoMinutos = tempo;
    }

    private void ValidarInserirDificuldade(string dificuldade)
    {
        if (string.IsNullOrWhiteSpace(dificuldade))
            throw new Exception("A dificuldade não pode estar vazia!");

        Dificuldade = dificuldade;
    }
}

// Objeto de Valor para organizar os percentuais do card
