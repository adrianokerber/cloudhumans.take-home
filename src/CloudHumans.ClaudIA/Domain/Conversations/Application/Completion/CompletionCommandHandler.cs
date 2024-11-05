using CloudHumans.ClaudIA.Shared;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Conversations.Application.Completion;

public sealed class CompletionCommandHandler : CommandHandler<CompletionCommand, string>, IService<CompletionCommandHandler>
{
    public override async Task<Result<string>> HandleAsync(CompletionCommand command, CancellationToken ct = default)
    {
        // TODO:
        /*
         * 1. flurl - Call Embeddings Generation
         * 2. flurl - Call Vector search on embeddings
         * 3. flurl - Build response
         */
        
        throw new NotImplementedException();
    }
}