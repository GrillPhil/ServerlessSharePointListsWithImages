using System.Threading.Tasks;

namespace MyListApp.Graph
{
    public interface IFileService
    {
        Task<DriveItem> UploadAsync(string path, byte[] content, string contentType);
    }
}
