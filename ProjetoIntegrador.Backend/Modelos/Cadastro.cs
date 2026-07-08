using System.Text.RegularExpressions;

namespace ProjetoIntegrador.Backend.Modelos;

public class Cadastro
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; }  = string.Empty;
    public string Senha { get; private set; }
    public string Username { get; private set; }  = string.Empty;
    public DateTime CriadoEm { get; private set; }

    protected Cadastro()
    {
        
    }

    public Cadastro( string nome, string email, string senha, string username )
    {
        InserirNome(nome);
        InserirEmail(email);
        InserirSenha(senha);
        InserirUsername(username);
    }
    
    
    private void InserirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("Não pode estar vazio!");

        if (nome.Length < 3)
            throw new Exception("Precisa ter no mínomo 3 caracteres!");

        if (Regex.IsMatch(nome, @"[^\p{L}\s]", RegexOptions.IgnoreCase))
            throw new Exception("Nome contém caracteres invalidos!");
        
        Nome = nome;
    }

    private void InserirEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("O email não pode estar vazio!");

        if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.IgnoreCase))
            throw new Exception("O Email não é valido!");
        
        Email = email;
    }

    private void InserirSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new Exception("A senha não pode estar vazia!");
        
        if (senha.Length < 6)
            throw new Exception("A senha precisa ter no mínimo 6 caracteres!");
        
        Senha = senha;
    }
    
    private void InserirUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new Exception("O username não pode estar vazio!");
        
        if (username.Length < 3 || username.Length > 20)
            throw new Exception("O Username precisa ter entre 3 a 20 caracteres!");
            
        Username = username;
    }
}