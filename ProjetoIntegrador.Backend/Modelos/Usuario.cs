using System.Text.RegularExpressions;

namespace ProjetoIntegrador.Backend.Modelos;

public class Usuario
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; private set; }

    protected Usuario()
    {
    }

    public Usuario(string nome, string email)
    {
        InserirNome(nome);
        InserirEmail(email);
    }

    private void InserirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Nome do usuário não pode estar vazio!");

        if (nome.Length < 3)
            throw new Exception("Nome do usuário precisa de no mínimo 3 caracteres!");

        if (Regex.IsMatch(nome, @"[^\p{L}\s]", RegexOptions.IgnoreCase))
            throw new Exception("Nome contém caracteres inválidos");

        Nome = nome;
    }

    private void InserirEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("O email do usuário não pode estar vazio!");

        if (!Regex.IsMatch(email, @"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$", RegexOptions.IgnoreCase))
            throw new Exception("O email do usuário não é válido!");

        Email = email;
    }
}