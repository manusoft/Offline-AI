using OfflineAI;

Console.WriteLine("Hi! ask me anything!");

while (true)
{
    string question = Console.ReadLine();

    var llama = new OfflineAIModel(
        "Models/tinyllama-1.1b-chat-v1.0.Q8_0.gguf",
        "Models/llama-cli.exe");

    var output = llama.GenerateText(question, 128);

    Console.WriteLine(output);
}