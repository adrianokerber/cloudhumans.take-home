using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CloudHumans.ClaudIA.Infrastructure;
using CloudHumans.ClaudIA.Shared;
using FastEndpoints;

namespace CloudHumans.ClaudIA.Domain.Conversations.Application.Completion;

public class CompletionEndpoint : Endpoint<PostRequest, PostResponse>
{
    private readonly CompletionCommandHandler _commandHandler;
    private readonly HttpResponseFactory _httpResponseFactory;

    public CompletionEndpoint(CompletionCommandHandler commandHandler, HttpResponseFactory httpResponseFactory)
    {
        _commandHandler = commandHandler;
        _httpResponseFactory = httpResponseFactory;
    }
    
    public override void Configure()
    {
        Post("/conversations/completions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PostRequest req, CancellationToken ct)
    {
        var command = CompletionCommand.Create(req.HelpdeskId,
                                                                      req.ProjectName,
                                                                      req.Messages.Select(x => new Message(x.Role, x.Content)));
        if (command.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateErrorWith400("Invalid request", command.Error));
            return;
        }

        var commandHandled = await _commandHandler.HandleAsync(command.Value, ct);
        if (commandHandled.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateErrorWith400("Unhandled use case", commandHandled.Error));
        }

        await SendResultAsync(_httpResponseFactory.CreateSuccessWith200(new PostResponse(commandHandled.Value)));
    }
}

public record PostRequest(int HelpdeskId, string ProjectName, IEnumerable<PostMessage> Messages);

public record PostMessage(string Role, string Content);

public record PostResponse(string resultMessage);