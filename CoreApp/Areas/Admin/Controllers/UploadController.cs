using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoreApp.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task UploadImageForCKEditor(IList<IFormFile> upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            DateTime now = DateTime.Now;
            if (upload.Count == 0)
            {
                await HttpContext.Response.WriteAsync("No image choosed");
            }
            else
            {
                IFormFile file = upload[0];
                string filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                string imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                await HttpContext.Response.WriteAsync("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", '" + Path.Combine(imageFolder, filename).Replace(@"\", @"/") + "');</script>");
            }
        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            DateTime now = DateTime.Now;
            IFormFileCollection files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }
            else
            {
                IFormFile file = files[0];
                string filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                string imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return new OkObjectResult(Path.Combine(imageFolder, filename).Replace(@"\", @"/"));
            }
        }

        [HttpPost]
        public IActionResult DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new OkObjectResult(new { Status = false, Message = "Không tìm thấy file" });
            }
            var arrayPath = path.Split(@"/");
            var fileName = arrayPath[arrayPath.Length - 1];
            Array.Resize<string>(ref arrayPath, arrayPath.Length - 1);
            var folder = _hostingEnvironment.WebRootPath + string.Join(@"\", arrayPath);
            if (System.IO.File.Exists(Path.Combine(folder, fileName)))
            {  
                System.IO.File.Delete(Path.Combine(folder, fileName));
                return new OkObjectResult(new { Status = true, Message ="Xóa file thành công"});
            }
            else
                return new OkObjectResult(new { Status = false, Message = "Không tìm thấy file" });
        }    
    }
}