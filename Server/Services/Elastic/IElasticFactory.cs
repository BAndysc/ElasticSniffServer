using Nest;

namespace Server.Services.Elastic;

public interface IElasticFactory
{
    ElasticClient Factory();
}