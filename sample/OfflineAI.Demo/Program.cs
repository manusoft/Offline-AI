using OfflineAI;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "OfflineAI Demo";
Console.Clear();

// Prevent standard Ctrl+C from instantly killing the app globally
Console.CancelKeyPress += (sender, e) => e.Cancel = true;

DrawBanner();

var model = new OfflineAIModel(
    "Models/tinyllama-1.1b-chat-v1.0.Q8_0.gguf",
    "Models/llama-cli.exe");

Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("Model : TinyLlama 1.1B");
Console.WriteLine("Type  : /help, /clear, /exit");
Console.WriteLine();
Console.ResetColor();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("You");
    Console.ResetColor();
    Console.Write(" > ");

    string? question = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(question))
        continue;

    switch (question.Trim().ToLowerInvariant())
    {
        case "/exit":
        case "/quit":
            return;

        case "/clear":
            Console.Clear();
            DrawBanner();
            continue;

        case "/help":
            ShowHelp();
            continue;
    }

    Console.WriteLine();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("AI");
    Console.ResetColor();
    Console.WriteLine(" >");

    var sw = Stopwatch.StartNew();
    int characters = 0;

    // Set up cancellation for this specific generation run
    using var cts = new CancellationTokenSource();

    // Hook up Ctrl+C to trigger our cancellation token
    ConsoleCancelEventHandler cancelHandler = (sender, e) =>
    {
        cts.Cancel();
    };
    Console.CancelKeyPress += cancelHandler;

    try
    {
        // Pass the token to your library method (assuming OfflineAI supports CancellationToken)
        string result = await model.GenerateTextAsync(
            question,
            token =>
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(token);
                Console.ResetColor();

                characters += token.Length;
            },
            cancellationToken: cts.Token); // <--- Passing the cancellation token here
    }
    catch (OperationCanceledException)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n\n[Generation Interrupted by User]");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"\n\n[Error]: {ex.Message}");
        Console.ResetColor();
    }
    finally
    {
        // Unhook the event handler so Ctrl+C goes back to normal behavior
        Console.CancelKeyPress -= cancelHandler;
        sw.Stop();

        // Clean up the trailing lines and force a fresh line break
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('─', 60));
        Console.WriteLine($"Completed in {sw.Elapsed.TotalSeconds:F2}s | {characters:N0} chars");
        Console.WriteLine(new string('─', 60));
        Console.ResetColor();

        Console.WriteLine();

        // CRITICAL: Clear any residual input buffers before starting the next loop iteration
        while (Console.KeyAvailable)
        {
            Console.ReadKey(intercept: true);
        }
    }
}

static void DrawBanner()
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine(
@"    ____  ______ ______ _      ______ _   _ ______          _____ 
  / __ \|  ____|  ____| |    |_   _| \ | |  ____|    /\   |_   _|
 | |  | | |__  | |__  | |      | | |  \| | |__      /  \    | |  
 | |  | |  __| |  __| | |      | | | . ` |  __|    / /\ \   | |  
 | |__| | |    | |    | |____ _| |_| |\  | |____  / ____ \ _| |_ 
  \____/|_|    |_|    |______|_____|_| \_|______/_/    \_\_____|
");
    Console.ResetColor();
    Console.WriteLine("OfflineAI Console Demo");
    Console.WriteLine();
}

static void ShowHelp()
{
    Console.WriteLine();
    Console.WriteLine("Commands");
    Console.WriteLine("--------------------------------------");
    Console.WriteLine("/help  - Show commands");
    Console.WriteLine("/clear - Clear screen");
    Console.WriteLine("/exit  - Exit application");
    Console.WriteLine();
}



//using OfflineAI;

//Console.WriteLine("Hi! Ask me anything!");
//var llama = new OfflineAIModel("Models/tinyllama-1.1b-chat-v1.0.Q8_0.gguf", "Models/llama-cli.exe");

//while (true)
//{
//    Console.Write("> ");
//    var question = Console.ReadLine();
//    Console.Write("AI: ");
//    int characters = 0;
//    var result = await llama.GenerateTextAsync(question,
//        token =>
//        {
//            Console.ForegroundColor = ConsoleColor.White;
//            Console.Write(token);
//            Console.ResetColor();

//            characters += token.Length;
//        }); // outputs while generating
//    Console.WriteLine("Completed.");
//}