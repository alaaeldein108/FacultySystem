namespace Demo.PL.Helper
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/Files",folderName);

            var fileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";

            var filePath=Path.Combine(folderPath, fileName);

            using var fileStream=new FileStream(filePath, FileMode.Create);
              
            file.CopyTo(fileStream);

            return fileName;

        }
        public static void DeleteFile(string imageName, string folderName)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName, imageName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

        }
    }
}
