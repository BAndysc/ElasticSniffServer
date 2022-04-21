using Elasticsearch.Net;
using Nest;

namespace Server.Services.Elastic;

public class ElasticFactory : IElasticFactory
{
    private readonly IConfiguration config;

    public ElasticFactory(IConfiguration config)
    {
        this.config = config;
    }
    
    public ElasticClient Factory()
    {
        var settings = new ConnectionSettings(config["ELASTIC_SNIFF_CLOUD_ID"] ?? config["Elastic:cloudId"], new BasicAuthenticationCredentials(config["ELASTIC_SNIFF_CLOUD_USER"] ?? config["Elastic:user"], config["ELASTIC_SNIFF_CLOUD_PASSWORD"] ?? config["Elastic:password"])).DefaultIndex("sniffs");
        return new ElasticClient(settings);
    }
}