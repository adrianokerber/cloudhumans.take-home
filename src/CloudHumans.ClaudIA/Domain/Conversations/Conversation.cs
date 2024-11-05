using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations;

public sealed class Conversation
{
    public IEnumerable<Message> Messages { get; init; }

    private Conversation(IEnumerable<Message> messages)
    {
        Messages = messages;
    }

    public static Result<Conversation> Create(IEnumerable<Message> messages)
    {
        if (!messages.Any())
            return Result.Failure<Conversation>("No messages provided.");

        return new Conversation(messages);
    }
}