using SearchSniffServer.Models;

namespace Server.Services;

public interface IUploadService
{
    Task<bool> Upload(ISniff sniff);
}