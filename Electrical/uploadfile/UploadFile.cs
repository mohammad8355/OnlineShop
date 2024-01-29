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
        public async Task<List<object>> Upload(string name,string destination,int? limitSize,string? format,IFormFile file)
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
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var filepath = Path.Combine(uploads, newFileName);
                    var newfileStream = new FileStream(filepath, FileMode.Create);
                    file.CopyTo(newfileStream);
                    newfileStream.Close();


                    return await returnMultipleData.Return(true,extention);
                }
            }
            else
            {
                return await returnMultipleData.Return(false, $"لطفا فایل را آپلود کنید"); ;
            }
        }
        public async Task<List<object>> DeleteFile(string path)
        {
            var completePath = Path.Combine(hostingEnvironment.WebRootPath, path);
            FileInfo file = new FileInfo(completePath);
            if (file.Exists)
            {
                file.Delete();
                return await returnMultipleData.Return(true, "");
            }
            else
            {
                return await returnMultipleData.Return(false, "چنین فایلی وجود ندارد");
            }
        }
        public bool FormatChecker(IFormFile file,List<string> formats)
        {
           var fileFormat =  Path.GetExtension(file.FileName);
            foreach (var format in formats)
            {
                if(format == fileFormat)
                {
                    return true;
                }
            }
            return false;
        }
        public void ChangeDirFile(string OldPath, string NewPath)
        {
            var completeOldPath = Path.Combine(hostingEnvironment.WebRootPath, OldPath);
            var completeNewPath = Path.Combine(hostingEnvironment.WebRootPath, NewPath);
            if(!Directory.Exists(completeNewPath) && Directory.Exists(completeOldPath))
             Directory.Move(completeOldPath, completeNewPath);
        } 
        public void DeleteDire(string path)
        {
            var completepath = Path.Combine(hostingEnvironment.WebRootPath, path);
            if (Directory.Exists(completepath))
            {
                Directory.Delete(completepath);
            }
        }
    }
}
