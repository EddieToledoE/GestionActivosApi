namespace GestionActivos.Domain.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(byte[] fileBytes, string fileName, string contentType);
        Task DeleteAsync(string fileName);
    }
}
