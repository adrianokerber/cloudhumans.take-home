using CloudHumans.ClaudIA.Domain.Conversations.ValueObjects;
using CloudHumans.ClaudIA.Domain.Shared;
using CloudHumans.ClaudIA.Shared;
using CSharpFunctionalExtensions;
using Flurl.Http;

namespace CloudHumans.ClaudIA.Domain.Conversations.Services;

public sealed class ContextualResponseService(string apiKey) : IService<ContextualResponseService>
{
    private const string SystemMessageContentTemplate =
        "You are ClaudIA, the ChatBot from Tesla Motors. Use the following context to answear the user question. CONTEXT: \"{0}\"";
    
    public async Task<Result<Message>> GenerateConversationCompletion(string userMessageContent, List<DataSection> improvedDataset)
    {
        var idsContext = string.Join(Environment.NewLine, improvedDataset.Select(section => section.Content));
        var systemMessageContent = string.Format(SystemMessageContentTemplate, idsContext);
        var requestBody = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new MessageModel("system", systemMessageContent),
                new MessageModel("user", userMessageContent)
            }
        };
        
        try
        {
            var response = await "https://api.openai.com/v1/chat/completions"
                .WithHeader("Authorization", $"Bearer {apiKey}")
                .PostJsonAsync(requestBody)
                .ReceiveJson<CompletionResponse>();

            var messageResponse = response.Choices.First().Message;
            var role = Role.Create(messageResponse.role);
            if (role.IsFailure)
                return Result.Failure<Message>(role.Error);
            
            var message = Message.Create(role.Value, messageResponse.content, improvedDataset);
            if (message.IsFailure)
                return Result.Failure<Message>(message.Error);

            return message.Value;
        }
        catch (FlurlHttpException ex)
        {
            return Result.Failure<Message>(ex.Message);
        }
    }

    private record CompletionResponse(List<Choice> Choices);
    
    private record Choice(MessageModel Message);
    
    private record MessageModel(string role, string content);
}