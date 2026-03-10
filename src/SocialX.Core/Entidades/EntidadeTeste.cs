namespace SocialX.Core.Entidades;

public class EntidadeTeste
{
    public long Id { get; private set; }
    public string Nome { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataCadastro { get; private set; }

    private EntidadeTeste()
    {
        // Construtor privado para o EF Core
    }

    public EntidadeTeste(string nome, decimal valor)
    {
        Nome = nome;
        Valor = valor;
        DataCadastro = DateTime.UtcNow;
    }

    public void Atualizar(string nome, decimal valor)
    {
        Nome = nome;
        Valor = valor;
    }
}
