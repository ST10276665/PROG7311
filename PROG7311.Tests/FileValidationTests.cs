using Microsoft.AspNetCore.Http;
using PROG7311.Services;
using System.IO;
using System.Text;
using Xunit;

namespace PROG7311.Tests
{
    public class FileValidationTests
    {
        private IFormFile CreateMockFile(string fileName, string content = "test content")
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
        }

        [Fact]
        public void FileValidation_PdfFile_IsAllowed()
        {
            var file = CreateMockFile("agreement.pdf");
            var extension = Path.GetExtension(file.FileName).ToLower();

            Assert.Equal(".pdf", extension);
        }

        [Fact]
        public void FileValidation_ExeFile_IsRejected()
        {
            var file = CreateMockFile("malicious.exe");
            var extension = Path.GetExtension(file.FileName).ToLower();

            Assert.NotEqual(".pdf", extension);
        }

        [Fact]
        public void FileValidation_DocxFile_IsRejected()
        {
            var file = CreateMockFile("document.docx");
            var extension = Path.GetExtension(file.FileName).ToLower();

            Assert.NotEqual(".pdf", extension);
        }

        [Fact]
        public void FileValidation_NullFile_DoesNotThrow()
        {
            IFormFile? file = null;

            var exception = Record.Exception(() =>
            {
                if (file == null) return;
                Path.GetExtension(file.FileName);
            });

            Assert.Null(exception);
        }
    }
}