using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Fuelcards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdiController : Controller
    {
        private readonly IQueriesRepository _db;

        public EdiController(IQueriesRepository queriesRepository)
        {
            _db = queriesRepository;
        }

        [HttpPost("MoveFilesToCorrectFolder")]
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


        public JsonResult FindAnyFailedSites([FromBody] List<string> ControlIDs)
        {
            List<Site> AllSites = _db.GetAllTransactions();
            return Json("");
        }
    }


    public class FileUploadViewModel
    {
        public List<IFormFile> Files { get; set; }
    }
}
