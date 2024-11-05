using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Shared;

public abstract class CommandHandler<TCommand, TResultValue>
{
    public abstract Task<Result<TResultValue>> HandleAsync(TCommand command, CancellationToken ct = default);
}

public abstract class CommandHandler<TCommand>
{
    public abstract Task<Result> HandleAsync(TCommand command, CancellationToken ct = default);
}