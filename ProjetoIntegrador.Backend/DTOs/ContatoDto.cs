namespace ProjetoIntegrador.Backend.DTOs;

public class ContatoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Vinculo { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public int IdUsuario { get; set; }
}