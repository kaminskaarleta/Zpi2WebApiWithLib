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
            //var fileName = Path.GetFileName(file.FileName);
            //var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
            //file.SaveAs(path);

            //System.Diagnostics.Process clientProcess = new System.Diagnostics.Process();
            //clientProcess.StartInfo.FileName = "java";
            //clientProcess.StartInfo.WorkingDirectory = Server.MapPath("~/App_Data/");
            //clientProcess.StartInfo.Arguments = @"-jar lib.jar Encrypt " + path + " x.txt pass";
            //clientProcess.Start();
            //clientProcess.WaitForExit();
            //bool res = clientProcess.ExitCode != 0;



            return View("CheckSumResult");
        }
    }
}