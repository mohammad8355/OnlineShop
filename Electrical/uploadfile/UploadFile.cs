using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;
using Utility.ReturnMultipleData;

namespace PresentationLayer.uploadfile
{
    public class UploadFile
    {
        private readonly ReturnMultipleData<UploadFile> returnMultipleData;
        private readonly IWebHostEnvironment hostingEnvironment;
        public UploadFile(IWebHostEnvironment HostingEnvironment, ReturnMultipleData<UploadFile> ReturnMultipleData)
        {
            hostingEnvironment = HostingEnvironment;
            returnMultipleData = ReturnMultipleData;
        }
        public async Task<List<object>> UploadImage(string name,string destination,int? limitSize,string? format,IFormFile file)
        {
            if(file != null)
            {
                if(file.Length > limitSize && limitSize != 0)
                {
                    return await returnMultipleData.Return(false,$"حجم فایل آپلود شده بیش از حد مجاز بوده است حجم فایل باید حداقل{limitSize}باشد ") ;
                }
                else if(Path.GetExtension(file.FileName) != format && !string.IsNullOrEmpty(format))
                {
                    return await returnMultipleData.Return(false, $"فرمت فایل آپلود شده غیر مجاز است و باید فرمت فایل {format}باشد"); 
                }
                else
                {
                    var extention = Path.GetExtension(file.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, destination);
                    var newFileName = name + extention;
                    Directory.CreateDirectory(uploads);
                    var filepath = Path.Combine(uploads, newFileName);
                    
                    file.CopyTo(new FileStream(filepath, FileMode.Create));
                    return await returnMultipleData.Return(true,extention);
                }
            }
            else
            {
                return await returnMultipleData.Return(false, $"لطفا فایل را آپلود کنید"); ;
            }
        }
    }
}
