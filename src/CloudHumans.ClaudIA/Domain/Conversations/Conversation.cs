using CloudHumans.ClaudIA.Domain.Conversations.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations;

public sealed class Conversation : Entity<int>
{
    private int HelpdeskId { get; init; }
    
    private string ProjectName { get; init; }
    
    private readonly List<Message> _messages;

    public Message LastMessage => _messages.Last();
    
    public IReadOnlyList<Message> Messages => _messages;

    private Conversation(List<Message> messages)
    {
        _messages = messages;
    }

    public static Result<Conversation> Create(int helpdeskId, string projectName, List<Message> messages)
    {
        if (helpdeskId == 0)
            return Result.Failure<Conversation>("Conversation must have a valid helpdesk ID");
        if (string.IsNullOrWhiteSpace(projectName))
            return Result.Failure<Conversation>("Conversation must have a valid project name");
        if (messages.Count == 0)
            return Result.Failure<Conversation>("Conversation must contain at least one message");

        return new Conversation(messages);
    }

    public void AddMessage(Message message)
    {
        _messages.Add(message);
    }
    
    public bool LastMessageIsNotFromUser() => LastMessage.Role != Role.User; 
}