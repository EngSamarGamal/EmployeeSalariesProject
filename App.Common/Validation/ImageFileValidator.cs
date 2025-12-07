using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Common.Validators
{
    public class ImageFileValidator: AbstractValidator<IFormFile>
    {
        public ImageFileValidator()
        {
            RuleFor(file => file)
            .Must(BeAValidImage).WithMessage("The File must be less than 2 mb and be an image type");
        }
        private bool BeAValidImage(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return false;

            // رفض الملفات التي تزيد عن 2 ميجابايت
            if (file.Length > 2097152)
            {
                return false;
            }

            byte[] fileHeader = new byte[8];

            using (Stream stream = file.OpenReadStream())
            {
                stream.Read(fileHeader, 0, fileHeader.Length);
            }

            if (fileHeader[0] == 0x89 && fileHeader[1] == 0x50 && fileHeader[2] == 0x4E && fileHeader[3] == 0x47 &&
                fileHeader[4] == 0x0D && fileHeader[5] == 0x0A && fileHeader[6] == 0x1A && fileHeader[7] == 0x0A)
            {
                return true; // PNG
            }
            else if (fileHeader[0] == 0xFF && fileHeader[1] == 0xD8 && fileHeader[2] == 0xFF)
            {
                return true; // JPEG
            }
            else if (fileHeader[0] == 0x42 && fileHeader[1] == 0x4D)
            {
                return true; // BMP
            }
            else if (fileHeader[0] == 0x47 && fileHeader[1] == 0x49 && fileHeader[2] == 0x46 && fileHeader[3] == 0x38)
            {
                return true; // GIF
            }

            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                char[] buffer = new char[100];
                int charsRead = reader.Read(buffer, 0, buffer.Length);
                string fileContent = new string(buffer, 0, charsRead).TrimStart();

                if (fileContent.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase) ||
                    fileContent.StartsWith("<svg", StringComparison.OrdinalIgnoreCase))
                {
                    return true; // SVG
                }
            }

            return false; 
        }
    }
}
