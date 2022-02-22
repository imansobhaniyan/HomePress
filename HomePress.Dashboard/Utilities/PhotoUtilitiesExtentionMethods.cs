using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace HomePress.Dashboard.Utilities
{
    public static class PhotoUtilitiesExtentionMethods
    {
        public static async Task<string?> SaveResizedAsync(this IFormFile file, IWebHostEnvironment environment, string directory, int size)
        {
            var relativePath = Path.Combine("uploads", directory, size.ToString(), file.FileName);

            var absolutePath = Path.Combine(environment.WebRootPath, relativePath);

            CreateDirectory(new FileInfo(absolutePath).Directory!);

            using (var fileStream = file.OpenReadStream())
            using (var image = await Image.LoadAsync(fileStream))
            {
                if (image.Width < size && image.Height < size)
                    return null;

                int width, height;

                if (image.Width > image.Height)
                {
                    width = size;
                    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                }
                else
                {
                    width = Convert.ToInt32(image.Width * size / (double)image.Height);
                    height = size;
                }

                image.Mutate(x => x.Resize(width, height).Grayscale());

                await image.SaveAsync(absolutePath);
            }

            return "/" + relativePath.Replace("\\", "/");

            static void CreateDirectory(DirectoryInfo directory)
            {
                if (!directory.Exists)
                {
                    if (directory.Parent != null)
                        CreateDirectory(directory.Parent);

                    directory.Create();
                }
            }
        }
    }
}
