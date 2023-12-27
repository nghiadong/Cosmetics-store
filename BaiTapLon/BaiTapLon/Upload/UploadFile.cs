namespace BaiTapLon.Upload
{
    public class UploadFile
    {
        public static string UploadAnh(int id, string controller, IFormFile file)
        {
            if (file != null)
            {

                var folderPath = Path.GetFullPath("./wwwroot/assets/img");
                folderPath = string.Format(@"{0}\{1}\{2}", folderPath, controller, id);
                Directory.CreateDirectory(folderPath);

                var filePath = string.Format(@"{0}\{1}", folderPath, file.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);

                return string.Format(@"/{0}/{1}/{2}", controller, id, file.FileName);
            }
            return string.Empty;
        }
    }
}
