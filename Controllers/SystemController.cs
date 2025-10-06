using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CdnService.Controllers
{
    [ApiController]
    [Route("api/system")]
    public class SystemController : ControllerBase
    {
        private readonly string _fileFolder;

        public SystemController(IOptions<FolderOptions> options)
        {
            var folder = options.Value.ImageFolder;
            _fileFolder = options.Value.FileFolder;
        }
        // GET api/system/file?path=relativeOrAbsolutePath
        [HttpGet("{*path}")]
        public async Task<IActionResult> GetFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return BadRequest("Path is required.");

            // Chỉ cho phép lấy file, không cho lấy folder
            if (Directory.Exists(path))
                return BadRequest("Path is a directory, not a file.");

            // Nếu là đường dẫn tương đối thì lấy từ thư mục gốc dự án
            string filePath = string.IsNullOrWhiteSpace(path)
                ? _fileFolder
                : Path.Combine(_fileFolder, path);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = ext switch
            {
                ".zip" => "application/zip",
                ".exe" => "application/octet-stream",
                ".msi" => "application/x-msi",
                ".xml" => "application/xml",
                ".json" => "application/json",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };

            // Trả về file, không load toàn bộ vào RAM
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, contentType);
        }
    }
}
