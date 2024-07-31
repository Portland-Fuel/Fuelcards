using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Fuelcards.Controllers
{
    [Route("EdiController/[action]")]
    public class EdiController : Controller
    {
        private readonly IQueriesRepository _db;

        public EdiController(IQueriesRepository queriesRepository)
        {
            _db = queriesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> MoveFilesToCorrectFolder(FileUploadViewModel model)
        {
            var EdiModel = HomeController.LoadEdiVmModel();

            if (model.Files == null || model.Files.Count != 4)
            {
                ModelState.AddModelError(string.Empty, "Four files are required.");
                return View("/Views/Edi/Edi.cshtml", EdiModel);

            }

            var uploadPath = @"C:\Portland\Fuel Trading Company\Fuelcards - Fuelcards\EDIFilesUpload";
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in model.Files)
            {
                    try
                    {
                        var filePath = Path.Combine(uploadPath, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Internal server error: {ex.Message}");
                        return View("UploadFiles", model);
                    }
            }

            ViewData["EdiMessage"]= "Files have been uploaded successfully!";
            return View("/Views/Edi/Edi.cshtml", EdiModel);

        }

        [HttpPost]
        public JsonResult FindAnyFailedSites([FromBody]string[] ControlIDs)
        {
            List<Site> FailedSites = new();

            List<Site> AllSites = _db.GetAllTransactions(ControlIDs.Select(e=> Convert.ToInt32(e)).ToList());
            foreach (var item in AllSites)
            {
                if (!_db.CheckSite(item))
                {
                    FailedSites.Add(item);

                }
                else
                {
                    var egg = 0;
                }
            }

          
            
            return Json(FailedSites);
        }

        [HttpPost]
        public JsonResult UploadNewFixedSite([FromBody] Site site)
        {
            try
            {
                _db.AddSiteNumberToBand(site);
                return Json("Success");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }   
    }


    public class FileUploadViewModel
    {
        public List<IFormFile> Files { get; set; }
    }
}
