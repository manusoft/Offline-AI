using OfflineAI;

//Console.WriteLine("Hi! Ask me anything!");

//var llama = new OfflineAIModel("Models/tinyllama-1.1b-chat-v1.0.Q8_0.gguf", "Models/llama-cli.exe");

//while (true)
//{
//    Console.Write("> ");
//    string question = Console.ReadLine();



//    var output = llama.GenerateText(question, 256); // Output is streamed inside method

//    Console.WriteLine($"AI: " + output);
//}


Console.WriteLine("Hi! Ask me anything!");
var llama = new OfflineAIModel("Models/tinyllama-1.1b-chat-v1.0.Q8_0.gguf", "Models/llama-cli.exe");

while (true)
{
    Console.Write("> ");
    var question = Console.ReadLine();
    Console.Write("AI: ");
    var output = llama.GenerateText(question, 512); // outputs while generating
    Console.WriteLine(output); // for spacing
}