namespace Web.Services;

public static class GeradorDeCodigo
{
    private const int TAMANHO_CODIGO = 5;

    public static string Gerar()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, TAMANHO_CODIGO)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());
        return result;
    }
}
