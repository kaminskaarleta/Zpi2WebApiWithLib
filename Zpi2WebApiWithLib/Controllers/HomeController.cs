using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zpi2WebApiWithLib.Models;

namespace Zpi2WebApiWithLib.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckSum()
        {
            return View();
        }

        public ActionResult FileCrypt()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateSum(HttpPostedFileBase file, FileCheckSumType type)
        {

            if (file == null || file.ContentLength == 0)
            {
                return View("CheckSumResult", ResponseModel.ErrorResponse("No file selected"));

            }
            var fileName = "file_" + Guid.NewGuid().ToString();
            var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
            file.SaveAs(path);

            string result = null;
            switch (type)
            {
                case FileCheckSumType.MD5:
                    result = EncryptLibrary.CheckSum.CalculateMD5(path);
                    break;
                case FileCheckSumType.SHA1:
                    result = EncryptLibrary.CheckSum.CalculateSHA1(path);
                    break;
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return View("CheckSumResult", ResponseModel.SuccessResponse(result));
        }

        [HttpPost]
        public ActionResult EncryptFile(HttpPostedFileBase file, string key)
        {
            if (file == null || file.ContentLength == 0)
            {
                return View("FileCryptResult", ResponseModel.ErrorResponse("No file selected"));
            }
            if (string.IsNullOrEmpty(key))
            {
                return View("FileCryptResult", ResponseModel.ErrorResponse("No key specified"));
            }

            var fileId = Guid.NewGuid().ToString();
            var fileToEncryptName = "file_to_encrypt_" + fileId;
            var encryptedFileName = "encrypted_file_" + fileId;
            var fileToEncryptPath = Path.Combine(Server.MapPath("~/App_Data/"), fileToEncryptName);
            var encryptedFilePath = Path.Combine(Server.MapPath("~/App_Data/"), encryptedFileName);
            file.SaveAs(fileToEncryptPath);

            EncryptLibrary.FileCryptography.EncryptFile(fileToEncryptPath, encryptedFilePath, key);

            if (System.IO.File.Exists(fileToEncryptPath))
            {
                System.IO.File.Delete(fileToEncryptPath);
            }

            var bytes = new byte[0];
            if (System.IO.File.Exists(encryptedFilePath))
            {
                bytes = System.IO.File.ReadAllBytes(encryptedFilePath);
                System.IO.File.Delete(encryptedFilePath);
            }

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "encrypted.txt");
        }

        [HttpPost]
        public ActionResult DecryptFile(HttpPostedFileBase file, string key)
        {
            if (file == null || file.ContentLength == 0)
            {
                return View("FileCryptResult", ResponseModel.ErrorResponse("No file selected"));
            }
            if (string.IsNullOrEmpty(key))
            {
                return View("FileCryptResult", ResponseModel.ErrorResponse("No key specified"));
            }

            var fileId = Guid.NewGuid().ToString();
            var fileToDecryptName = "file_to_decrypt_" + fileId;
            var decryptedFileName = "decrypted_file_" + fileId;
            var fileToDecryptPath = Path.Combine(Server.MapPath("~/App_Data/"), fileToDecryptName);
            var decryptedFilePath = Path.Combine(Server.MapPath("~/App_Data/"), decryptedFileName);

            file.SaveAs(fileToDecryptPath);

            EncryptLibrary.FileCryptography.DecryptFile(fileToDecryptPath, decryptedFilePath, key);

            if (System.IO.File.Exists(fileToDecryptPath))
            {
                System.IO.File.Delete(fileToDecryptPath);
            }

            var bytes = new byte[0];
            if (System.IO.File.Exists(decryptedFilePath))
            {
                bytes = System.IO.File.ReadAllBytes(decryptedFilePath);
                System.IO.File.Delete(decryptedFilePath);
            }
            var text1 = System.Text.Encoding.ASCII.GetString(bytes);
            var text2 = System.Text.Encoding.UTF32.GetString(bytes);
            var text3 = System.Text.Encoding.Unicode.GetString(bytes);
            var text4 = System.Text.Encoding.Default.GetString(bytes);

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "decrypted.txt");
        }
    }
}