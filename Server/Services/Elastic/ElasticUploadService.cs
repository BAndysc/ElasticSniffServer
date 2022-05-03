using Elasticsearch.Net;
using Nest;
using SearchSniffServer.Models;
using Server.Database.Models;
using Server.Models;

namespace Server.Services.Elastic;

public class ElasticUploadService : IUploadService
{
    ElasticClient client;

    public ElasticUploadService(IElasticFactory factory)
    {
        client = factory.Factory();
    }
    
    public async Task<bool> Upload(UserModel uploader, ISniff sniff)
    {
        var document = new SniffElasticDocument(sniff);

        var existing = await client.GetAsync<SniffElasticDocument>(new GetRequest("sniffs", sniff.MD5));
        
        if (existing.IsValid && existing.Found)
        {
            var result = await client.DeleteAsync(new DeleteRequest("sniffs", new Id(sniff.MD5)));
            Console.WriteLine("DELETING by MD5 " + sniff.MD5 + " "+ (result.IsValid ? 1 : 0)+  " REMOVED");
        }
        
        var asyncIndexResponse = await client.IndexDocumentAsync(document);
        if (!asyncIndexResponse.IsValid)
        {
            Console.WriteLine(asyncIndexResponse.ToString());
            return false;
        }

        return true;
    }
}