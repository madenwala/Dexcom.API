using System.Threading;
using System.Threading.Tasks;

namespace Dexcom.Api
{
    public interface IAuthProvider
    {
        Task<string> PromptUserForCredentialsAsync(string authenticationUrl, string callbackUrl, CancellationToken ct);
    }
}