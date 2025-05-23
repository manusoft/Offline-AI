using System.Diagnostics;
using System.Text;

namespace OfflineAI;


//public class OfflineAIModel : IOfflineAIModel
//{
//    private readonly string modelPath;
//    private readonly string executablePath;
//    private readonly TimeSpan timeout;

//    public OfflineAIModel(
//        string modelPath,
//        string executablePath = "llama-cli.exe",
//        TimeSpan? timeout = null)
//    {
//        this.modelPath = modelPath;
//        this.executablePath = executablePath;
//        this.timeout = timeout ?? TimeSpan.FromSeconds(30); // default timeout
//    }

//    public async Task<string> GenerateTextAsync(string prompt, int maxTokens = 1024)
//    {
//        prompt = prompt.Replace("\"", "'");

//        var psi = new ProcessStartInfo
//        {
//            FileName = executablePath,
//            Arguments = $"--model \"{modelPath}\" --prompt \"{prompt}\" --n-predict {maxTokens}",
//            RedirectStandardOutput = true,
//            RedirectStandardError = true,
//            UseShellExecute = false,
//            CreateNoWindow = true
//        };

//        var output = new StringBuilder();
//        var error = new StringBuilder();

//        using var process = new Process { StartInfo = psi };

//        process.OutputDataReceived += (_, e) => { if (e.Data != null) output.AppendLine(e.Data); };
//        process.ErrorDataReceived += (_, e) => { if (e.Data != null) error.AppendLine(e.Data); };

//        try
//        {
//            process.Start();
//            process.BeginOutputReadLine();
//            process.BeginErrorReadLine();

//            using var cts = new CancellationTokenSource(timeout);

//            var taskCompletionSource = new TaskCompletionSource<bool>();
//            _ = Task.Run(() =>
//            {
//                process.WaitForExit();
//                taskCompletionSource.TrySetResult(true);
//            });

//            if (await Task.WhenAny(taskCompletionSource.Task, Task.Delay(Timeout.Infinite, cts.Token)) == taskCompletionSource.Task)
//            {
//                // Completed
//            }
//            else
//            {
//                // Timeout
//                if (!process.HasExited)
//                {
//                    process.Kill(true);
//                }
//            }

//            var fullOutput = output.ToString();

//            // Extract answer from <|assistant|> to the next prompt symbol or EOF
//            var assistantTag = "<|assistant|>";
//            var start = fullOutput.IndexOf(assistantTag);
//            if (start >= 0)
//            {
//                start += assistantTag.Length;
//                var end = fullOutput.IndexOf("<|", start); // optionally stop before next tag
//                if (end == -1) end = fullOutput.Length;

//                var answer = fullOutput.Substring(start, end - start).Trim();
//                return answer;
//            }

//            return $"[FAILED TO PARSE]\n{fullOutput}\n{error}";
//        }
//        catch (Exception ex)
//        {
//            return $"[EXCEPTION] {ex.Message}\n{error}";
//        }
//    }

//    // Synchronous wrapper if needed
//    public string GenerateText(string prompt, int maxTokens = 1024)
//    {
//        return GenerateTextAsync(prompt, maxTokens).GetAwaiter().GetResult();
//    }
//}


public class OfflineAIModel : IOfflineAIModel
{
    private readonly string modelPath;
    private readonly string executablePath;

    public OfflineAIModel(
        string modelPath,
        string executablePath = "llama-cli.exe")
    {
        this.modelPath = modelPath;
        this.executablePath = executablePath;
    }

    public string GenerateText(string prompt, int maxTokens = 1024)
    {
        prompt = prompt.Replace("\"", "'");

        var psi = new ProcessStartInfo
        {
            FileName = executablePath,
            Arguments = $"--model \"{modelPath}\" --prompt \"{prompt}\" --n-predict {maxTokens}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var output = new StringBuilder();
        var error = new StringBuilder();

        using var process = new Process { StartInfo = psi };

        process.OutputDataReceived += (_, e) => { if (e.Data != null) output.AppendLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data != null) error.AppendLine(e.Data); };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Wait 5 seconds for output then kill
            Thread.Sleep(15000);

            if (!process.HasExited)
            {
                process.Kill(true);
            }

            var fullOutput = output.ToString();

            // Extract answer from <|assistant|> to the next prompt symbol or EOF
            var assistantTag = "<|assistant|>";
            var start = fullOutput.IndexOf(assistantTag);
            if (start >= 0)
            {
                start += assistantTag.Length;
                var end = fullOutput.IndexOf("<|", start); // optionally stop before next tag
                if (end == -1) end = fullOutput.Length;

                var answer = fullOutput.Substring(start, end - start).Trim();
                return answer;
            }

            return $"[FAILED TO PARSE]\n{fullOutput}\n{error}";
        }
        catch (Exception ex)
        {
            return $"[EXCEPTION] {ex.Message}";
        }
    }

    public async Task<string> GenerateTextAsync(string prompt, int maxTokens = 100)
    {
        return await Task.Run(() => GenerateText(prompt, maxTokens));
    }
}
