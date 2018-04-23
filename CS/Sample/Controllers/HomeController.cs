using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Sample.Models;

namespace Sample.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View(UserProvider.Select());
        }
        public ActionResult GridViewPartial() {
            return PartialView(UserProvider.Select());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew([ModelBinder(typeof(DevExpressEditorsBinder))] User user) {
            if(ModelState.IsValid)
                UserProvider.Insert(user);
            else
                ViewData["UserInfo"] = user;
            return PartialView("GridViewPartial", UserProvider.Select());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Update([ModelBinder(typeof(DevExpressEditorsBinder))] User user) {
            if(ModelState.IsValid)
                UserProvider.Update(user);
            else
                ViewData["UserInfo"] = user;
            return PartialView("GridViewPartial", UserProvider.Select());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete([ModelBinder(typeof(DevExpressEditorsBinder))] User user) {
            UserProvider.Delete(user);
            return PartialView("GridViewPartial", UserProvider.Select());
        }

        public ActionResult ImageUpload() {
            UploadControlExtension.GetUploadedFiles("uploadControl", UploadControlHelper.ValidationSettings, UploadControlHelper.uploadControl_FileUploadComplete);
            return null;
        }
    }

    public class UploadControlHelper {
        public static readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe"},
            MaxFileSize = 4000000
        };

        public static void uploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
            if(e.UploadedFile.IsValid) {
                string resultFilePath = "~/Content/Avatars/" + string.Format("Avatar{0}{1}", Convert.ToString(HttpContext.Current.Session["UserID"]), Path.GetExtension(e.UploadedFile.FileName));
                e.UploadedFile.SaveAs(HttpContext.Current.Request.MapPath(resultFilePath));
                IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                if(urlResolver != null)
                    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath) + "?refresh=" + Guid.NewGuid().ToString();
            }
        }
    }
}