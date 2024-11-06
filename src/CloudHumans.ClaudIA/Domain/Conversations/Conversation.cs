using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations;

public sealed class Conversation
{
    private readonly List<Message> _messages;

    public Message LastMessage => _messages.Last();
    
    public IReadOnlyList<Message> Messages => _messages;

    private Conversation(List<Message> messages)
    {
        _messages = messages;
    }

    public static Result<Conversation> Create(List<Message> messages)
    {
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