using CloudHumans.ClaudIA.Domain.Conversations.Services;
using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CloudHumans.ClaudIA.Shared;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion;

public sealed class ConversationCompletionCommandHandler(EmbeddingService embeddingService, VectorSearchService vectorSearchService, ContextualResponseService contextualResponseService)
    : CommandHandler<ConversationCompletionCommand, Conversation>, IService<ConversationCompletionCommandHandler>
{
    public override async Task<Result<Conversation>> HandleAsync(ConversationCompletionCommand command, CancellationToken ct = default)
    {
        var conversation = Conversation.Create(command.Messages);
        if (conversation.IsFailure)
            return Result.Failure<Conversation>(conversation.Error);
        if (conversation.Value.LastMessageIsNotFromUser())
            return Result.Failure<Conversation>("The last message must be from the user");

        var embeddings = await embeddingService.ConvertIntoEmbeddings(conversation.Value.LastMessage.Content);
        if (embeddings.IsFailure)
            return Result.Failure<Conversation>(embeddings.Error);
        
        var closestData = await vectorSearchService.SearchClosestData(embeddings.Value);
        if (closestData.IsFailure)
            return Result.Failure<Conversation>(closestData.Error);
        
        // TODO: Implement 'Smart Handover Feature'
        
        var completion = await contextualResponseService.GenerateConversationCompletion(conversation.Value.LastMessage.Content, closestData.Value);
        if (completion.IsFailure)
            return Result.Failure<Conversation>(completion.Error);

        conversation.Value.AddMessage(completion.Value);
        
        return conversation;
    }
}