using SearchSniffServer.Models;
using Server.Database.Models;

namespace Server.Services;

public interface IUploadService
{
    Task<bool> Upload(UserModel uploader, ISniff sniff);
}