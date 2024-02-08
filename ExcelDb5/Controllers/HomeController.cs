using ExcelDb5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelDb5.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseClass _context;

        public HomeController(DatabaseClass context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<User> ReadExcelData(string filePath)
        {
            var excelRecords = new List<User>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var excelRecord = new User
                    {
                        Id = worksheet.Cells[row, 1].Value.ToString(),
                        FName = worksheet.Cells[row, 2].Value.ToString(),
                        LName = worksheet.Cells[row, 3].Value.ToString(),
                        City = worksheet.Cells[row, 4].Value.ToString()
                    };

                    excelRecords.Add(excelRecord);
                }
            }

            return excelRecords;
        }

        [HttpPost]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var filePath = file.FileName;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var excelRecords = ReadExcelData(filePath);

            _context.User.AddRange(excelRecords);
            _context.SaveChanges();

            return Ok("Data uploaded successfully");
        }
    }
}
