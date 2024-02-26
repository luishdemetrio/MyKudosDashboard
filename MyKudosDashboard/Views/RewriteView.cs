using Azure;
using Microsoft.Graph;
using MyKudosDashboard.Interfaces;
using Azure.AI.OpenAI;

namespace MyKudosDashboard.Views;

public class RewriteView : IRewriteView
{
    // Note: The Azure OpenAI client library for .NET is in preview.
    // Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5

    OpenAIClient client;

    public RewriteView(IConfiguration config)
    {
        client = new OpenAIClient(
                           new Uri("https://luisdemopenai.openai.azure.com/"),
                           new AzureKeyCredential(config["AZURE_OPENAI_API_KEY"]));
    }



    public async Task<string> Rewrite(string message)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = "CopilotFAQ", // Use DeploymentName for "model" with non-Azure clients
            Messages =
            {
                // The system message represents instructions or other guidance about how the assistant should behave
                new ChatRequestSystemMessage("You are responsible for rewrite better texts."),
                // User messages represent current or historical input from the end user
                new ChatRequestUserMessage($"reescreva: {message}"),

            }
        };

        Response<ChatCompletions> response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
        ChatResponseMessage responseMessage = response.Value.Choices[0].Message;
        Console.WriteLine($"[{responseMessage.Role.ToString().ToUpperInvariant()}]: {responseMessage.Content}");

        return responseMessage.Content;
        //CompletionsOptions completionsOptions = new()
        //{
        //    DeploymentName = "CopilotFAQ",
        //    Prompts = { $"melhor reescreva: {message}" },
        //    Temperature = (float)0.7,
        //    MaxTokens = 1000,

        //    NucleusSamplingFactor = (float)0.95,
        //    FrequencyPenalty = 0,
        //    PresencePenalty = 0,
        //};

        //Response<Completions> completionsResponse = client.GetCompletions(completionsOptions);
        //string completion = completionsResponse.Value.Choices[0].Text;



        
    }
}
