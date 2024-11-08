using CloudHumans.ClaudIA.Domain.Shared;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.ValueObjects;

public sealed class Message
{
    public Role Role { get; init; }
    public string Content { get; init; }
    public Maybe<IReadOnlyList<DataSection>> DataSections { get; init; }

    private Message(Role role, string content, Maybe<IReadOnlyList<DataSection>> dataSections)
    {
        Role = role;
        Content = content;
        DataSections = dataSections;
    }

    public static Result<Message> Create(Role role, string content, List<DataSection>? dataSections = null)
    {
        if (role == Role.Assistant && (dataSections is null || dataSections.Count == 0))
            return Result.Failure<Message>($"Message with ASSISTANT role must have dataSections linked");
        
        return new Message(role, content, dataSections);
    }
}