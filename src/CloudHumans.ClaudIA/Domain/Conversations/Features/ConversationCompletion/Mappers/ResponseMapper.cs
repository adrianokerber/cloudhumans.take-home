using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion.Mappers;

public static class ResponseMapper
{
    public static ConversationResponse ToViewModel(this Conversation conversation)
    {
        var msgs = new List<ConversationMessageResponse>();
        foreach (var message in conversation.Messages)
        {
            msgs.Add(message.ToViewModel());
        }

        return new ConversationResponse(msgs, false, null);
    }

    private static ConversationMessageResponse ToViewModel(this Message message)
        => new ConversationMessageResponse(message.Role.Name, message.Content);
}