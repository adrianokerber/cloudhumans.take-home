using CloudHumans.ClaudIA.Domain.Conversations.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion.Mappers;

public static class RequestMapper
{
    public static Result<Message> ToMessage(this ConversationMessageRequest messageRequest)
    {
        var role = Role.Create(messageRequest.Role);
        if (role.IsFailure)
            return Result.Failure<Message>(role.Error);
        
        var message = Message.Create(role.Value, messageRequest.Content);
        if (message.IsFailure)
            return Result.Failure<Message>(message.Error);

        return message.Value;
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