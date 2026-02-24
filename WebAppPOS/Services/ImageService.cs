using WebAppPOS.Repositories.Interfaces;

namespace WebAppPOS.Services
{
    public class ImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<string?> SaveImageAsync(IFormFile? imageFile, string folderName)
        {
            return await _imageRepository.SaveImageAsync(imageFile, folderName);
        }
        public void DeleteImageAsync(string? imagePath)
        {
            _imageRepository.DeleteImageAsync(imagePath);
        }
    }
}
