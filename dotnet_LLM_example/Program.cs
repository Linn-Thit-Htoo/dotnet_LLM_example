using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOllamaChatCompletion(
            modelId: "llama2", // or "llama3"
            endpoint: new Uri("http://localhost:11434")
        );

        var kernel = builder.Build();
        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are a helpful assistant.");

        while (true)
        {
            Console.Write("You: ");
            var userMessage = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userMessage))
                break;

            chatHistory.AddUserMessage(userMessage);

            var response = await chatService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine($"\nBot: {response.Content}\n");

            chatHistory.AddAssistantMessage(response.Content!);
        }
    }
}
