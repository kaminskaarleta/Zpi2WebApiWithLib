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
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
            file.SaveAs(path);

            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe ";
            proc.StartInfo.Arguments = string.Format(@"{0}");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            string outPut = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            proc.Close();

            return View("CheckSumResult");
        }
    }
}