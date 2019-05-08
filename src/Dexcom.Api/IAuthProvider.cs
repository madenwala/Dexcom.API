using System.Threading;
using System.Threading.Tasks;

namespace Dexcom.Api
{
    public interface IAuthProvider
    {
        Task<string> AuthenticationAsync(string authenticationUrl, string callbackUrl, CancellationToken ct);
    }
}