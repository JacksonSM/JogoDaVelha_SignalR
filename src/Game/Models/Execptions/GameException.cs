namespace Game.Models.Execptions;

public class GameException : SystemException
{
    public GameException(string mensagem) : base(mensagem)
    {
    }
}
