using Game.Models.Execptions;
using Microsoft.AspNetCore.SignalR;

namespace Game.Filter;

public class ExceptionFilter : IHubFilter
{
    public async ValueTask<object> InvokeMethodAsync(
    HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {

        try
        {
            return await next(invocationContext);
        }
        catch(GameException e)
        {
            await invocationContext.Hub.Clients.Caller.SendAsync("AconteceuErro", e.Message);
            throw;
        }
        catch
        {
            await invocationContext.Hub.Clients.Caller.SendAsync("AconteceuErro", "Aconteceu um erro desconhecido.");
            throw;
        }
    }
}
