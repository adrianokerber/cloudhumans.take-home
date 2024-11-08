using CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion.Mappers;
using CloudHumans.ClaudIA.Infrastructure;
using FastEndpoints;

namespace CloudHumans.ClaudIA.Domain.Conversations.Features.ConversationCompletion;

public class ConversationCompletionEndpoint(
    ConversationCompletionCommandHandler commandHandler,
    HttpResponseFactory httpResponseFactory)
    : Endpoint<ConversationRequest, ConversationResponse>
{
    public override void Configure()
    {
        Post("/conversations/completions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConversationRequest req, CancellationToken ct)
    {
        var messages = req.Messages.ToMessageList();
        if (messages.IsFailure)
        {
            await SendResultAsync(httpResponseFactory.CreateErrorWith400("Invalid request", messages.Error));
            return;
        }
        
        var command = ConversationCompletionCommand.Create(req.HelpdeskId, req.ProjectName, messages.Value);
        if (command.IsFailure)
        {
            await SendResultAsync(httpResponseFactory.CreateErrorWith400("Invalid request", command.Error));
            return;
        }

        var result = await commandHandler.HandleAsync(command.Value, ct);
        if (result.IsFailure)
        {
            await SendResultAsync(httpResponseFactory.CreateErrorWith400("Unhandled use case", result.Error));
            return;
        }

        await SendResultAsync(httpResponseFactory.CreateSuccessWith200(result.Value.ToViewModel()));
    }
}

public record ConversationRequest(int HelpdeskId, string ProjectName, IEnumerable<ConversationMessageRequest> Messages);

public record ConversationMessageRequest(string Role, string Content);

public record ConversationResponse(List<ConversationMessageResponse> Messages, bool HandoverToHumanNeeded, IEnumerable<ConversationRetrievedSection> SectionsRetrieved);

public record ConversationRetrievedSection(float Score, string Content);

public record ConversationMessageResponse(string Role, string Content); 