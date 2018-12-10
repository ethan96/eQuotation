using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AOnlineWall.Business
{
    public class Common
    {
        public string UploadFile(string uploadPath,System.Web.UI.WebControls.FileUpload fileUpload)
        {
            try
            {
                if (fileUpload.HasFile)
                {

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);
                    string fileName = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;
                    string xxx = uploadPath.Substring(uploadPath.Length - 1);
                    if (uploadPath.Substring(uploadPath.Length - 1) != "\\")
                        uploadPath += "\\";
                    fileUpload.SaveAs(uploadPath + fileName);
                    return fileName;
                }
            }
            catch (Exception)
            {
                
            }
            return "";
        }

        public bool DeleteFile(string filePath,string fileName)
        {
            try
            {
                if (filePath.Substring(filePath.Length - 1) != "\\")
                    filePath += "\\";
                if (File.Exists(filePath + fileName))
                {
                    File.Delete(filePath + fileName);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
