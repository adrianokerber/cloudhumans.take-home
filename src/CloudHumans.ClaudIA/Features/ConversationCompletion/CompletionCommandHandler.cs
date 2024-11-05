using CloudHumans.ClaudIA.Shared;
using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Features.ConversationCompletion;

public sealed class CompletionCommandHandler : CommandHandler<CompletionCommand, string>, IService<CompletionCommandHandler>
{
    public override async Task<Result<string>> HandleAsync(CompletionCommand command, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}