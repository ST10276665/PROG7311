namespace PROG7311.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveContractFileAsync(IFormFile file)
        {
            // Only allow PDFs
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            // Generate unique filename to prevent overwrites
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folderPath = Path.Combine(_env.WebRootPath, "uploads", "contracts");

            // Create folder if it doesn't exist
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path to store in DB
            return $"/uploads/contracts/{fileName}";
        }
    }
}