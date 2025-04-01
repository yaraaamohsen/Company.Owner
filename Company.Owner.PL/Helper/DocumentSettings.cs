namespace Company.Owner.PL.Helper
{
    public class DocumentSettings
    {
        // 1. Upload
        // ImageName --> Unique name
        // IFormFile --> Consider all Files
        public static string UploadFile(IFormFile file, string folderName)
        {
            // File Path (FolderLocation / ImageName)

            // 1. Get Folder Location
            //var folderPath = "D:\\Backend Development\\ASP.NET MVC\\Company.Owner\\Company.Owner.PL\\wwwroot\\Files\\" + folderName;
            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + folderName;
            //var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName); // Could write it with slash behaviour
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);

            // 2. Get File Name and make it Unique
            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            // File Path
            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }
        // 2. Delete

        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
