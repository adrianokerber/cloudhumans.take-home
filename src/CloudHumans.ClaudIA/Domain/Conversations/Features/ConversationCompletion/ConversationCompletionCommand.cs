using CloudHumans.ClaudIA.Domain.Conversations.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion;

public sealed class ConversationCompletionCommand
{
    public int HelpdeskId { get; }
    public string ProjectName { get; }
    public List<Message> Messages { get; }

    private ConversationCompletionCommand(int helpdeskId, string projectName, List<Message> messages)
    {
        HelpdeskId = helpdeskId;
        ProjectName = projectName;
        Messages = messages;
    }

    public static Result<ConversationCompletionCommand> Create(int helpdeskId, string projectName, List<Message> messages)
    {
        if (helpdeskId <= 0)
            return Result.Failure<ConversationCompletionCommand>($"helpdeskId is invalid: {helpdeskId}");
        if (string.IsNullOrWhiteSpace(projectName))
            return Result.Failure<ConversationCompletionCommand>($"projectName is invalid: {projectName}");
        var newestUserMessage = messages.LastOrDefault();
        if (newestUserMessage is null || newestUserMessage.Role != Role.User)
            return Result.Failure<ConversationCompletionCommand>("Must have a message from the user to complete the conversation");
        
        return new ConversationCompletionCommand(helpdeskId, projectName, messages);
    }
}