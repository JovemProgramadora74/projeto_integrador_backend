using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProjetoIntegrador.Backend.Modelos;

public class Contato
{
    protected Contato()
    {
    }

    public Contato(string nome, string vinculo, string telefone, string email, int idUsuario) 
    {
        InserirNome(nome);
        InserirVinculo(vinculo);
        InserirTelefone(telefone);
        InserirEmail(email);
        InserirIdUsuario(idUsuario);
    }

    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Vinculo { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public int IdUsuario { get; private set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario Usuario { get; private set; }

    private void InserirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new Exception("Nome do contato de emergência não pode estar vazio!");
        if (nome.Length < 3) throw new Exception("Nome do contato de emergência precisa de no mínimo 3 caracteres!");
        if (Regex.IsMatch(nome, @"[^\p{L}\s]", RegexOptions.IgnoreCase))
            throw new Exception("Nome deve conter apenas letras!");

        Nome = nome;
    }

    private void InserirVinculo(string vinculo)
    {
        if (string.IsNullOrWhiteSpace(vinculo)) throw new Exception("Vínculo não pode estar vazio!");
        if (vinculo.Length < 3) throw new Exception("Vínculo precisa de no mínimo 3 caracteres!");

        Vinculo = vinculo;
    }

    private void InserirTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone)) throw new Exception("Número de telefone não pode estar vazio!");
        if (telefone.Length != 11) throw new Exception("Número de telefone deve conter 11 caracteres!");
        if (Regex.IsMatch(telefone, "[^0-9]", RegexOptions.IgnoreCase))
            throw new Exception("O telefone deve conter apenas números!");

        Telefone = telefone;
    }

    private void InserirEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new Exception("E-mail não pode estar vazio!");
        if (email.Length > 100) throw new Exception("E-mail pode ter no máximo 100 caracteres!");
        if (!Regex.IsMatch(email, @"[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            throw new Exception("E-mail inválido!");
        Email = email;
    }

    private void InserirIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0) throw new Exception("Id inválido!");
        
        IdUsuario = idUsuario;
    }
}

public class ContatoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Vinculo { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public int IdUsuario { get;  set; }
}