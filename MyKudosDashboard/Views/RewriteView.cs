using Azure;
using MyKudosDashboard.Interfaces;
using Azure.AI.OpenAI;

namespace MyKudosDashboard.Views;

public class RewriteView : IRewriteView
{
    // Note: The Azure OpenAI client library for .NET is in preview.
    // Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5

    OpenAIClient _openAIClient;
    string _deploymentName;

    string _promptInstructions;
    string _promptRewrite;

    public RewriteView(IConfiguration config)
    {
        //if Azure OpenAI is disable, this class will not be used
        //Also, when disabled, we dont have the endpoint and apikey
        if (! bool.Parse(config["AZURE_OPENAI_ENABLED"]))
            return;

        _openAIClient = new OpenAIClient(
                           new Uri(config["AZURE_OPENAI_ENDPOINT"]),
                           new AzureKeyCredential(config["AZURE_OPENAI_API_KEY"]));

        _deploymentName = config["AZURE_OPENAI_DEPLOYMENT_NAME"];

        _promptInstructions = config["AZURE_OPENAI_PROMPT_INSTRUCTIONS"];
        _promptRewrite= config["AZURE_OPENAI_PROMPT_REWRITE"];
    }



    public async Task<string> Rewrite(string message)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = _deploymentName, // Use DeploymentName for "model" with non-Azure clients
            Messages =
            {
                // The system message represents instructions or other guidance about how the assistant should behave
                new ChatRequestSystemMessage(_promptInstructions),
                // User messages represent current or historical input from the end user
                new ChatRequestUserMessage($"{_promptRewrite} : {message}"),

            }
        };

        Response<ChatCompletions> response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
        ChatResponseMessage responseMessage = response.Value.Choices[0].Message;
        Console.WriteLine($"[{responseMessage.Role.ToString().ToUpperInvariant()}]: {responseMessage.Content}");

        return responseMessage.Content;
        
    }
}
