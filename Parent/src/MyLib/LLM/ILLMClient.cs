using System.Threading;
using System.Threading.Tasks;

namespace MyLib.LLM;

public interface ILLMClient
{
    /// <summary>
    /// Sends a prompt to the configured LLM and returns the text response.
    /// Implementations should provide a deterministic fallback when no remote is configured.
    /// </summary>
    Task<string> QueryAsync(string prompt, CancellationToken cancellationToken = default);
}
