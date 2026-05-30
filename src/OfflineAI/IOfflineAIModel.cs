namespace OfflineAI;

public interface IOfflineAIModel
{
    Task<string> GenerateTextAsync(string prompt,
                                   Action<string>? onToken = null,
                                   int maxTokens = 1024,
                                   CancellationToken cancellationToken = default);
}