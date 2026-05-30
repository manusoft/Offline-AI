using OfflineAI.Extensions;
using System.Diagnostics;
using System.Text;

namespace OfflineAI;

public class OfflineAIModel : IOfflineAIModel
{
    private readonly string _modelPath;
    private readonly string _executablePath;

    public OfflineAIModel(string modelPath, string executablePath = "llama-cli.exe")
    {
        _modelPath = modelPath;
        _executablePath = executablePath;
    }

    public async Task<string> GenerateTextAsync(string prompt,
                                                Action<string>? onToken = null,
                                                int maxTokens = 1024,
                                                CancellationToken cancellationToken = default)
    {
        prompt = prompt.Replace("\"", "'");

        // Format the prompt correctly for TinyLlama's chat template
        string formattedPrompt = $"<|user|>\n{prompt}<|assistant|>\n";

        var psi = new ProcessStartInfo
        {
            FileName = _executablePath,
            Arguments =
                $"--model \"{_modelPath}\" " +
                $"--prompt \"{formattedPrompt}\" " +
                $"--n-predict {maxTokens} " +
                $"--reverse-prompt \"<|user|>\" " +
                $"--no-display-prompt ",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        var responseBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        using var process = new Process
        {
            StartInfo = psi,
            EnableRaisingEvents = true
        };

        process.Start();

        var outputTask = Task.Run(async () =>
        {
            char[] buffer = new char[1];

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    process.KillTree();
                    throw new OperationCanceledException(cancellationToken);
                }

                int read = await process.StandardOutput.ReadAsync(buffer, 0, 1);
                if (read <= 0)
                    break;

                string token = new string(buffer, 0, read);
                responseBuilder.Append(token);

                string currentResponse = responseBuilder.ToString();

                if (currentResponse.Contains(">"))
                {
                    process.KillTree();
                    break;
                };

                // If we detect the start of the end-turn template token, kill the engine early
                if (currentResponse.Contains("<|user|>"))
                {
                    process.KillTree();
                    break;
                }

                // Don't leak layout brackets into the UI display action
                if (token == "<" && "<|user|>".StartsWith(token)) continue;
                if (token == "|" && currentResponse.EndsWith("<|")) continue;
                if (token == "u" && currentResponse.EndsWith("<|u")) continue;

                onToken?.Invoke(token);
            }
        }, cancellationToken);

        var errorTask = Task.Run(async () =>
        {
            try
            {
                while (!process.StandardError.EndOfStream)
                {
                    var line = await process.StandardError.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        errorBuilder.AppendLine(line);
                    }
                }
            }
            catch { /* Suppress stream errors during execution kill */ }
        }, cancellationToken);

        try
        {
            await Task.WhenAll(outputTask, errorTask, process.WaitForExitAsync(cancellationToken));
        }
        catch (Exception)
        {
            if (!process.HasExited) process.KillTree();

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            throw;
        }

        //string finalResult = responseBuilder.ToString().Replace("<|user|>", "").Trim();

        //if (process.ExitCode != 0 && responseBuilder.Length == 0)
        //{
        //    return $"[ERROR]\n{errorBuilder}";
        //}

        string finalResult = responseBuilder.ToString().Replace("<|user|>", "").Replace("<", "").Replace(">", "").Trim();

        if (process.ExitCode != 0 && responseBuilder.Length == 0)
        {
            return $"[ERROR]\n{errorBuilder}";
        }

        return finalResult;
    }
}