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
        var settings = new ConnectionSettings(config["Elastic:cloudId"], new BasicAuthenticationCredentials(config["Elastic:user"], config["Elastic:password"])).DefaultIndex("sniffs");
        return new ElasticClient(settings);
    }
}