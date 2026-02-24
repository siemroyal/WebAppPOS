namespace WebAppPOS.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<string?> SaveImageAsync(IFormFile? imageFile, string folderName);
        void DeleteImageAsync(string? imagePath);
    }
}
