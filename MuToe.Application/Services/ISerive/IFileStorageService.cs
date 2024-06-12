

namespace MuTote.Application.Services.ISerive
{
    public interface IFileStorageService
    {
        Task<string> UploadFileToDefaultAsync(Stream fileStream, string fileName);
    }
}
