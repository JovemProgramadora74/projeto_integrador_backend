namespace ProjetoIntegrador.Backend.DTOs;

public class ReceitaCompletaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public string? TagRestricao { get; set; }
    public int TempoPreparoMinutos { get; set; }
    public string Dificuldade { get; set; } = string.Empty;
    public MacrosDto Macros { get; set; } = new();
    public List<string> Ingredientes { get; set; } = [];
    public List<string> ModoPreparo { get; set; } = [];
}