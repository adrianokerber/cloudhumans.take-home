using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Application.Completion;

public sealed class CompletionCommand
{
    public int HelpdeskId { get; }
    public string ProjectName { get; }
    public IEnumerable<Message> Messages { get; }

    private CompletionCommand(int helpdeskId, string projectName, IEnumerable<Message> messages)
    {
        HelpdeskId = helpdeskId;
        ProjectName = projectName;
        Messages = messages;
    }

    public static Result<CompletionCommand> Create(int helpdeskId, string projectName, IEnumerable<Message> messages)
    {
        if (helpdeskId <= 0)
            Result.Failure($"helpdeskId is invalid: {helpdeskId}");
        if (string.IsNullOrWhiteSpace(projectName))
            Result.Failure($"projectName is invalid: {projectName}");
        if (!messages.Any())
            Result.Failure("No conversation messages were provided");
        
        return new CompletionCommand(helpdeskId, projectName, messages);
    }
}