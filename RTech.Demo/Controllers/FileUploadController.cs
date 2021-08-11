using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Controllers
{
    public class FileUploadController : Controller
    {
        [HttpPost]
        public JsonResult Index()
        {
            //TODO: Delete Raw images and files before save scan file before save for antivirus security
            var File = Request.Files[0];
            string branchCode = "";
            var currentUser = RiddhaSession.CurrentUser;
            if (currentUser != null && currentUser.Branch!=null)
            {
                branchCode = currentUser.Branch.Code;
            }
            var id = Guid.NewGuid();

            var fileType = File.FileName.Split('.')[1];
            string [] validImageType=new string[4]{ "jpeg", "png", "jpg", "pdf"};
            if (validImageType.Contains(fileType.ToLower()))
            {
                var fileSpec = @"/Images/File/" + branchCode + "_" + id + "." + File.FileName.Split('.')[1];
                string filePath = Server.MapPath(@"/Images/File/") + branchCode + "_" + id + "." + File.FileName.Split('.')[1];
                File.SaveAs(filePath);
                return new JsonResult() { Data = fileSpec };//D:\projects\NewRepos\HamroHajiri\RTech.Demo\Images\upload photo.png
            }
            return new JsonResult() { Data = @"/Images/error.png" };  
        }
	}
}