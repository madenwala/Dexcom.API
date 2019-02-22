using Dexcom.Api.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Dexcom.Api
{
    public interface IDexcomAuthProvider
    {
        Task<string> PromptForCredentialsAsync(string authenticationUrl, string callbackUrl, CancellationToken ct);
    }
}