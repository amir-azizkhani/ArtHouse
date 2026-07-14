using Microsoft.AspNetCore.Http;

namespace ArtHouse.Services
{
    public class ImageService
    {

        public string SaveImage(IFormFile image)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return "/images/" + fileName;
        }


        public void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var imagePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                imageUrl.TrimStart('/')
            );

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}