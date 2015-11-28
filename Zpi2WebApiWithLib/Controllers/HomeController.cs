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
                return View("CheckSumResult", ResponseModel.ErrorResponse("Brak pliku"));

            }
            var fileId = Guid.NewGuid().ToString();
            var fileName = "file_" + fileId;
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
    }
}