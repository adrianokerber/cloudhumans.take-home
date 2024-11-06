using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion.Mappers;

public static class RequestMapper
{
    public static Result<Message> ToMessage(this ConversationMessageRequest messageRequest)
    {
        var role = Role.Create(messageRequest.Role);
        if (role.IsFailure)
            return Result.Failure<Message>(role.Error);
        
        return new Message(role.Value, messageRequest.Content);
    }

    public static Result<List<Message>> ToMessageList(
        this IEnumerable<ConversationMessageRequest> request)
    {
        var messages = new List<Message>();
        foreach (var requestMessage in request)
        {
            var result = requestMessage.ToMessage();
            if (result.IsFailure)
                return Result.Failure<List<Message>>(result.Error);
            messages.Add(result.Value);
        }

        return messages;
    }
}