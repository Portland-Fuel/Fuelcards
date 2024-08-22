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
        [Route("EdiController/process")]
        public async Task<IActionResult> Process(IFormFile ediFile1, IFormFile ediFile2, IFormFile ediFile3, IFormFile ediFile4)
        {
            var files = new[] { ediFile1, ediFile2, ediFile3, ediFile4 };
            var fileNames = new List<string>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    // Here we can save the file to a directory
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    fileNames.Add(file.FileName);
                }
            }

            // Return a JSON response with the processed file names
            return Json(new { message = "Files processed successfully", files = fileNames });
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

}
