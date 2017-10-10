using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

//namespace KraudFan2_0.HelpClass.ImageUpload
//{
//    static class ImageUploadClass
//    {
//        public static string UploadImage(IFormFile uploadedFile)
//        {
//            if (uploadedFile != null)
//            {
//                // путь к папке Files
//                string path = "/Files/" + uploadedFile.FileName;
//                // сохраняем файл в папку Files в каталоге wwwroot
//                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
//                {
//                    await uploadedFile.CopyToAsync(fileStream);
//                }
//                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
//                _context.Files.Add(file);
//                _context.SaveChanges();
//            }


//            return url;
//        }

//    }
//}
