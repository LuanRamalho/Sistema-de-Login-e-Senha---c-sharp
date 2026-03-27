using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

// Classe de modelo para o usuário (Sem ID)
public class Usuario
{
    public string Login { get; set; }
    public string Senha { get; set; }
}

public static class Database
{
    private const string FileName = "usuarios.json";

    // Inicializa o arquivo se ele não existir
    public static void Inicializar()
    {
        if (!File.Exists(FileName))
        {
            File.WriteAllText(FileName, "[]");
        }
    }

    // Lê todos os usuários do arquivo JSON
    public static List<Usuario> ObterTodos()
    {
        try
        {
            string json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }
        catch
        {
            return new List<Usuario>();
        }
    }

    // Salva a lista completa no arquivo
    public static void SalvarTodos(List<Usuario> usuarios)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(usuarios, options);
        File.WriteAllText(FileName, json);
    }

    // Método para adicionar um novo usuário com verificação de duplicata
    public static bool InserirUsuario(string login, string senha)
    {
        var usuarios = ObterTodos();
        
        if (usuarios.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
            return false;

        usuarios.Add(new Usuario { Login = login, Senha = senha });
        SalvarTodos(usuarios);
        return true;
    }
}