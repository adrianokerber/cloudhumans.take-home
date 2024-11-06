using CloudHumans.ClaudIA.Shared;
using CSharpFunctionalExtensions;
using Flurl.Http;

namespace CloudHumans.ClaudIA.Domain.Conversations.Services;

public sealed class EmbeddingService(string apiKey) : IService<EmbeddingService>
{
    public async Task<Result<List<double>>> ConvertIntoEmbeddings(string text)
    {
        try
        {
            var response = await "https://api.openai.com/v1/embeddings"
                .WithHeader("Authorization", $"Bearer {apiKey}")
                .PostJsonAsync(new
                {
                    model = "text-embedding-3-large",
                    input = text
                })
                .ReceiveJson<Response>();
            
            return response.Data.FirstOrDefault()!.Embedding;
        }
        catch (FlurlHttpException ex)
        {
            return Result.Failure<List<double>>(ex.Message);
        }
    }
    
    private record Response(List<MessageEmbeddings> Data);
    private record MessageEmbeddings(List<double> Embedding);
}