using System.Text.Json.Serialization;
using CloudHumans.ClaudIA.Domain.Shared.ValueObjects;
using CloudHumans.ClaudIA.Shared;
using CSharpFunctionalExtensions;
using Flurl;
using Flurl.Http;

namespace CloudHumans.ClaudIA.Domain.Conversations.Services;

public sealed class VectorSearchService(string apiKey) : IService<VectorSearchService>
{
    public async Task<Result<List<DataSection>>> SearchClosestData(List<double> embeddings)
    {
        try
        {
            var requestBody = new
            {
                count = true,
                select = "content, type",
                top = 10,
                filter = "projectName eq 'tesla_motors'",
                vectorQueries = new[]
                {
                    new
                    {
                        vector = embeddings,
                        k = 3,
                        fields = "embeddings",
                        kind = "vector"
                    }
                }
            };
            
            var response = await "https://claudia-db.search.windows.net/indexes/claudia-ids-index-large/docs/search"
                .SetQueryParam("api-version", "2023-11-01")
                .WithHeader("api-key", apiKey)
                .PostJsonAsync(requestBody)
                .ReceiveJson<Response>();

            return response.Value.Select(x => new DataSection(x.Score, x.Content, x.Type)).ToList();
        }
        catch (FlurlHttpException ex)
        {
            return Result.Failure<List<DataSection>>(ex.Message);
        }
    }
    
    private record Response(List<ResponseDataSection> Value);
    
    private record ResponseDataSection([property: JsonPropertyName("@search.score")] float Score,
                                                                                     string Content,
                                                                                     string Type);
}