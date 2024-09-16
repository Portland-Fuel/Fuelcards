

using DataAccess.Repositorys.IRepositorys;
using FuelcardModels.ConsoleApp;
using FuelcardModels.DataTypes;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Fuelcards.Controllers
{
    [Route("EdiController/[action]")]
    public class EdiController(IFuelcardUnitOfWork fuelcardUnitOfWork, IQueriesRepository queriesRepository) : Controller
    {
        private readonly IFuelcardUnitOfWork _fuelcardRepo = fuelcardUnitOfWork;
        private readonly IQueriesRepository _db = queriesRepository;
        [HttpPost]
        public JsonResult MoveFilesToCorrectFolder(FileUploadViewModel model)
        {
            try
            {
                if (model == null || model.Files == null || !model.Files.Any())
                {
                    throw new Exception("No files uploaded");
                }

                string uploadPath = @"C:\Portland\Fuel Trading Company\Fuelcards - Fuelcards\EDIFilesUpload";
                string archivePath = @"C:\Portland\Fuel Trading Company\Fuelcards - Fuelcards\EDIFilesArchive";

                // Ensure the upload and archive directories exist
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }

                foreach (var file in model.Files)
                {
                    string filePath = Path.Combine(uploadPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                // Perform EDI import operations here (uncomment when ready)
                 EDIs.ImportAllEdi(_fuelcardRepo, uploadPath);

                var filesInUpload = Directory.GetFiles(uploadPath);
                foreach (var filePath in filesInUpload)
                {
                    string fileName = Path.GetFileName(filePath);
                    if (model.Files.Any(f => f.FileName == fileName))
                    {
                        string archiveFilePath = Path.Combine(archivePath, fileName);
                        if (System.IO.File.Exists(archiveFilePath))
                        {
                            System.IO.File.Delete(archiveFilePath); 
                        }
                        System.IO.File.Move(filePath, archiveFilePath);
                    }
                    else
                    {
                        Console.WriteLine($"File {fileName} not handled.");
                    }
                }

                return Json("Files processed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (you could use a logging library like NLog, Serilog, etc.)
                Console.WriteLine($"An error occurred: {ex.Message}");
                Response.StatusCode = 500;
                return Json(new { error = ex.Message });
            }
        }



        [HttpPost]
        public JsonResult FindAnyFailedSites([FromBody] string[] ControlIDs)
        {
            List<Site> FailedSites = new();

            List<Site> AllSites = _db.GetAllTransactions(ControlIDs.Select(e => Convert.ToInt32(e)).ToList());
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
        public struct MaskedCardError
        {
            public string? cardNumber { get; set; }

            public string? costCentre { get; set; }

            public string? network { get; set; }


            //do not touch with a ten foot pole
        }

        [HttpPost]
        public JsonResult ProcessMaskedCardForm([FromBody]MaskedCardError Data)
        {
            _db.AddNewMaskedCard(Data);
            return Json("");
        }


        [HttpPost]
        public async Task<JsonResult> MaskedCards()
        {
            try
            {
                await _db.GetTransactionsWithoutPortlandId();
                return Json("Success");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                var CostCentres = _db.CostCentreOptions();
                var ToReturn = new
                {
                    CostCentres = CostCentres,
                    CardNumber = e.Message.Split(',')[0].ToString(),
                    Network = e.Message.Split(',')[1].ToString(),
                };
                return Json(ToReturn); 
            }
            
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
