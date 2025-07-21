using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Create a kernel builder
        var builder = Kernel.CreateBuilder();

        // Add Ollama chat completion service
        builder.AddOllamaChatCompletion(
            modelId: "llama2", // or "llama3"
            endpoint: new Uri("http://localhost:11434")
        );

        // Build the kernel
        var kernel = builder.Build();

        // Get the chat completion service
        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        // Create a new chat history
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are a helpful assistant.");

        while (true)
        {
            Console.Write("You: ");
            var userMessage = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userMessage))
                break;

            chatHistory.AddUserMessage(userMessage);

            // Generate response using chat completion
            var response = await chatService.GetChatMessageContentAsync(chatHistory);

            Console.WriteLine($"\nBot: {response.Content}\n");

            chatHistory.AddAssistantMessage(response.Content!);
        }
    }
}
