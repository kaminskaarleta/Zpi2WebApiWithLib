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
            var filePath = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
            file.SaveAs(filePath);

            var response = PrepareResponse(filePath, type);

            RemoveFileIfExist(filePath);

            return View("CheckSumResult", response);
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

            try
            {
                EncryptLibrary.FileCryptography.EncryptFile(fileToEncryptPath, encryptedFilePath, key);
            }
            catch
            {
                return View("FileCryptResult", ResponseModel.ErrorResponse("Error on encrypting file"));
            }
            finally
            {
                RemoveFileIfExist(fileToEncryptPath);
            }

            var bytes = ReadAndDeleteFile(encryptedFilePath);

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

            try
            {
                EncryptLibrary.FileCryptography.DecryptFile(fileToDecryptPath, decryptedFilePath, key);
            }
            catch
            {
                return View("FileCryptResult", ResponseModel.ErrorResponse("Error on decrypting file"));
            }
            finally
            {
                RemoveFileIfExist(fileToDecryptPath);
            }

            var bytes = ReadAndDeleteFile(decryptedFilePath);

            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "decrypted.txt");
        }

        private byte[] ReadAndDeleteFile(string encryptedFilePath)
        {
            var bytes = new byte[0];
            if (System.IO.File.Exists(encryptedFilePath))
            {
                bytes = System.IO.File.ReadAllBytes(encryptedFilePath);
                System.IO.File.Delete(encryptedFilePath);
            }
            return bytes;
        }

        private void RemoveFileIfExist(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private ResponseModel PrepareResponse(string filePath, FileCheckSumType type)
        {
            var responseModel = new ResponseModel();

            try
            {
                switch (type)
                {
                    case FileCheckSumType.MD5:
                        responseModel.Data = EncryptLibrary.CheckSum.CalculateMD5(filePath);
                        break;
                    case FileCheckSumType.SHA1:
                        responseModel.Data = EncryptLibrary.CheckSum.CalculateSHA1(filePath);
                        break;
                }
                responseModel.Result = true;
            }
            catch (Exception)
            {
                responseModel.Data = "Error on checksum calculating";
            }

            return responseModel;
        }
    }
}