using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using Utility.ReturnMultipleData;

namespace Infrustructure.uploadfile
{
    public class UploadFile
    {
        private readonly ReturnMultipleData<UploadFile> returnMultipleData;
        public UploadFile( ReturnMultipleData<UploadFile> ReturnMultipleData)
        {
            returnMultipleData = ReturnMultipleData;
        }
        public async Task<(bool result,string message)> Upload(string name,string destination,int? limitSize,List<string>? formats,IFormFile file)
        {
            var havelegalFormat = true;
            if(file != null)
            {
                if(file.Length > limitSize && limitSize != 0)
                {
                    return (false,$"حجم فایل آپلود شده بیش از حد مجاز بوده است حجم فایل باید حداقل{limitSize}باشد ") ;
                }
                else if(formats != null)
                {
                    foreach (var format in formats)
                    {
                        if (Path.GetExtension(file.FileName) == format)
                        {
                            havelegalFormat = true;
                            break;
                        }
                        else
                        {
                            havelegalFormat = false;
                        }
                    }
                    if (!havelegalFormat)
                    {
                        return  (false,"فایل فرمت مجاز را ندارد") ;
                    }
                }
                    var extention = Path.GetExtension(file.FileName);
                    var newFileName = name + extention;
                    if (!Directory.Exists(destination))
                    {
                        Directory.CreateDirectory(destination);
                    }
                    var filepath = Path.Combine(destination, newFileName);
                    var newfileStream = new FileStream(filepath, FileMode.Create);
                    file.CopyTo(newfileStream);
                    newfileStream.Close();


                    return (true, extention);
            }
            else
            {
                return (false, $"لطفا فایل را آپلود کنید");
            }
        }
        public async Task<(bool result,string message)> DeleteFile(string path)
        {
            var completePath = Path.Combine(path);
            FileInfo file = new FileInfo(completePath);
            if (file.Exists)
            {
                file.Delete();
                return (true, "");
            }
            else
            {
                return (false, "چنین فایلی وجود ندارد");
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
            var completeOldPath = Path.Combine(OldPath);
            var completeNewPath = Path.Combine(NewPath);
            if(!Directory.Exists(completeNewPath) && Directory.Exists(completeOldPath))
             Directory.Move(completeOldPath, completeNewPath);
        } 
        public void DeleteDire(string path)
        {
            var completepath = Path.Combine(path);
            if (Directory.Exists(completepath))
            {
                Directory.Delete(completepath);
            }
        }
    }
}
