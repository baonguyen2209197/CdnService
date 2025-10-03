using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CdnService.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private readonly string _imageFolder;

        public ImageController(IOptions<ImageFolderOptions> options)
        {
            var folder = options.Value.ImageFolder;
            _imageFolder = options.Value.ImageFolder;
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> GetImage(string filename)
        {
            var filePath = Path.Combine(_imageFolder, filename);
            if (!System.IO.File.Exists(filePath))
                return NotFound();
            var contentType = GetContentType(filename);
            return PhysicalFile(filePath, contentType);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] IFormFileCollection files)
        {
            const int maxFiles = 20; 
            const long maxFileSize = 8 * 1024 * 1024; 
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");
            if (files.Count > maxFiles)
                return BadRequest($"Chỉ được upload tối đa {maxFiles} file/lượt.");

            int successCount = 0;
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    continue;
                if (file.Length > maxFileSize)
                    continue; 
                var filePath = Path.Combine(_imageFolder, Path.GetFileName(file.FileName));
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                successCount++;
            }
            return Ok(new { count = successCount });
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
