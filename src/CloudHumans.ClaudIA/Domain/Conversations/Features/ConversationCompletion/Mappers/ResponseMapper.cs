using CloudHumans.ClaudIA.Domain.Conversations.ValueObjects;
using CloudHumans.ClaudIA.Domain.Shared;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion.Mappers;

public static class ResponseMapper
{
    public static ConversationResponse ToViewModel(this Conversation conversation)
    {
        var messages = new List<ConversationMessageResponse>();
        foreach (var message in conversation.Messages)
        {
            messages.Add(message.ToViewModel());
        }

        var sectionList = conversation.Messages.Last().DataSections;
        var sections = sectionList.HasValue ? sectionList.Value.ToViewModelList() : null;

        return new ConversationResponse(messages, false, sections);
    }

    private static ConversationMessageResponse ToViewModel(this Message message)
        => new ConversationMessageResponse(message.Role.Name, message.Content);

    private static ConversationRetrievedSection ToViewModel(this DataSection section)
        => new ConversationRetrievedSection(section.Score, section.Content);

    private static IEnumerable<ConversationRetrievedSection> ToViewModelList(this IEnumerable<DataSection> sections)
        => sections.Select(x => x.ToViewModel());
}