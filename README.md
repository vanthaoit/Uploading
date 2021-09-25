using ClosedXML.Excel;
using Dapper;
using KAS.Uploading.API.Generic;
using KAS.Uploading.API.Models;
using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.BusinessFunctions.Services;
using KAS.Uploading.Models.Entities;
using KAS.Uploading.Models.Enums;
using KAS.Uploading.Models.Structs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KAS.Uploading.API.Controllers
{
    public class UploadController : APIGenericController
    {
        private readonly IDataAccessService<Menu> _dataDapperService;
        private readonly IEFCommonService<Menu, int> _MenuService;
        private readonly IConfiguration _config;

        public UploadController(IConfiguration config,
                                IEFCommonService<Menu, int> MenuService

            )
        {
            _dataDapperService = new DapperDataService<Menu>(config);
            _MenuService = MenuService;
            _config = config;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await WriteToExcelFolder();
                var resp = _MenuService.GetAll();

                //await ExecuteAsync();
                return new OkObjectResult(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine("error get Menu  " + e);
                return new OkObjectResult(null);
            }
        }

        private Task<HttpResponseMessage> WriteToExcelFolder()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                List<DMReportingModel> dmReportings = GetDMReportings();
                var totalRecords = dmReportings.Count;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    string fileName = @"DM_Reporting_" + DateTime.Now.ToString("MMddyyyy");
                    var wsheet = wb.Worksheets.Add("DM Reporting");
                    var wsheet2 = wb.Worksheets.Add("Student sheet 2");
                    wb.Style.Font.SetFontName("Times New Roman").Font.SetFontSize(11);
                    DecorateSheet(wsheet, totalRecords);

                    var headerRow = 1;

                    // define header
                    WriteOverHeader(wsheet, headerRow);

                    //Decorate all header
                    FillDecorateHeaders(wsheet, headerRow);

                    //body
                    WriteOverContent(wsheet, dmReportings);

                    // save to folder
                    wb.SaveAs(GetPathFolder(fileName));
                }
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error write to body" + ex);
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return Task.FromResult(response);
        }

        private void DecorateSheet(IXLWorksheet wsheet, int totalRecords)
        {
            wsheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            var rangeBorder = @"A1:J" + (int)(totalRecords + 1);
            wsheet.Range(rangeBorder).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            wsheet.Range(rangeBorder).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            wsheet.Range(rangeBorder).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            wsheet.Range(rangeBorder).Style.Border.RightBorder = XLBorderStyleValues.Thin;
        }

        private void WriteOverHeader(IXLWorksheet wsheet, int currentRow)
        {
            wsheet.Cell(currentRow, (int)DMReporting.Client).Value = "Client";
            wsheet.Cell(currentRow, (int)DMReporting.Payer).Value = "Payer";
            wsheet.Cell(currentRow, (int)DMReporting.VoucherNumber).Value = @"V#";
            wsheet.Cell(currentRow, (int)DMReporting.HighAcuity).Value = "High acuity (Yes/No)";
            wsheet.Cell(currentRow, (int)DMReporting.DM).Value = "DM Eligible (Yes/No)";
            wsheet.Cell(currentRow, (int)DMReporting.HasPayment).Value = "V has payment";
            wsheet.Cell(currentRow, (int)DMReporting.Has0Balance).Value = "Voucher has 0 balance";
            wsheet.Cell(currentRow, (int)DMReporting.BillingDate).Value = "Voucher Billing Date <> Null";
            wsheet.Cell(currentRow, (int)DMReporting.DateUpdated).Value = "Voucher date updated = null";
            wsheet.Cell(currentRow, (int)DMReporting.VoidDate).Value = "Voucher Void date <> Null";
        }

        private void WriteOverContent(IXLWorksheet wsheet, List<DMReportingModel> dmReportings)
        {
            try
            {
                var currentRow = 1;
                var currentFirstColumn = string.Empty;
                var currentSecondColumn = string.Empty;

                foreach (var dmReporting in dmReportings)
                {
                    currentRow++;

                    wsheet.Cell(currentRow, (int)DMReporting.Client).Value = (currentFirstColumn != dmReporting.ClientName) ? dmReporting.ClientName : string.Empty;
                    wsheet.Cell(currentRow, (int)DMReporting.Payer).Value = (currentSecondColumn != dmReporting.Payer) ? dmReporting.Payer : string.Empty;

                    wsheet.Cell(currentRow, (int)DMReporting.VoucherNumber).Value = dmReporting.VoucherNumber;

                    wsheet.Cell(currentRow, (int)DMReporting.HighAcuity).Value = dmReporting.HighAcuity == true ? "Yes" : "No";
                    wsheet.Cell(currentRow, (int)DMReporting.DM).Value = dmReporting.DM == true ? "Yes" : "No";

                    wsheet.Cell(currentRow, (int)DMReporting.HasPayment).Value = dmReporting.HasPayment != 0 ? "X" : "";
                    wsheet.Cell(currentRow, (int)DMReporting.Has0Balance).Value = dmReporting.Has0Balance != 0 ? "X" : "";
                    wsheet.Cell(currentRow, (int)DMReporting.BillingDate).Value = dmReporting.BillingDate != 0 ? "X" : "";
                    wsheet.Cell(currentRow, (int)DMReporting.DateUpdated).Value = dmReporting.DateUpdated != 0 ? "X" : "";
                    wsheet.Cell(currentRow, (int)DMReporting.VoidDate).Value = dmReporting.VoidDate != 0 ? "X" : "";

                    currentFirstColumn = dmReporting.ClientName;
                    currentSecondColumn = dmReporting.Payer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error write to body" + ex);
            }
        }

        private void FillDecorateHeaders(IXLWorksheet wsheet, int currentRow)
        {
            var totalColumns = Enum.GetValues(typeof(DMReporting)).Length;

            for (var i = 0; i < totalColumns; i++)
            {
                if (i < 5)
                    DecorateHeader(wsheet, currentRow, i + 1, XLColor.BabyPink);
                else
                    DecorateHeader(wsheet, currentRow, i + 1);
            }
        }

        private void DecorateHeader(IXLWorksheet wsheet, int currentRow, int currentColumn, XLColor color = null)
        {
            if (currentColumn < 3)
                wsheet.Column(currentColumn).Width = 30;
            else
            {
                wsheet.Column(currentColumn).Width = 15;
                wsheet.Column(currentColumn).Style.Alignment.WrapText = true;
            }

            wsheet.Row(currentRow).Height = 30;
            wsheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            wsheet.Cell(currentRow, currentColumn).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            wsheet.Cell(currentRow, currentColumn).Style.Font.Bold = true;
            if (color != null)
                wsheet.Cell(currentRow, currentColumn).Style.Fill.SetBackgroundColor(color);
            else
                wsheet.Cell(currentRow, currentColumn).Style.Fill.SetBackgroundColor(XLColor.ArylideYellow);
        }

        private string GetPathFolder(string file)
        {
            var folderPath = @"" + _config.GetValue<string>("DMReportingPathFolder");

            var fileName = file + ".xlsx";

            var pathFolder = Path.Combine(
                        folderPath,
                        fileName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return pathFolder;
        }

        private List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            for (var i = 0; i < 10; i++)
            {
                if (i < 3)
                    students.Add(new Student()
                    {
                        Id = 100,
                        Name = "Johan 100",
                        FirstName = "Cruyff " + i + i,
                        LastName = "Teo " + i
                    });
                else if (i > 2 && i < 7)
                    students.Add(new Student()
                    {
                        Id = 200,
                        Name = "Johan 200",
                        FirstName = "Cruyff " + i + i,
                        LastName = "Teo " + i
                    });
                else
                    students.Add(new Student()
                    {
                        Id = 300,
                        Name = "Johan 300",
                        FirstName = "Cruyff " + i + i,
                        LastName = "Teo " + i
                    });
            }
            return students;
        }

        private List<DMReportingModel> GetDMReportings()
        {
            List<DMReportingModel> dmReporting = new List<DMReportingModel>();
            for (var i = 0; i < 15; i++)
            {
                if (i < 3)
                    dmReporting.Add(new DMReportingModel()
                    {
                        ClientName = "Robin Van Persie",
                        Payer = "Holland",
                        VoucherNumber = 13 + i + 2,
                        DM = false,
                        HighAcuity = true,
                        HasPayment = 0,
                        Has0Balance = 0,
                        BillingDate = 0,
                        DateUpdated = 0,
                        VoidDate = 0,
                        Populate270 = true
                    });
                else if (i > 2 && i < 7)
                    dmReporting.Add(new DMReportingModel()
                    {
                        ClientName = "Arjen Robben",
                        Payer = "Netherlands",
                        VoucherNumber = 13 + i + 4,
                        DM = true,
                        HighAcuity = true,
                        HasPayment = 0,
                        Has0Balance = 0,
                        BillingDate = 56,
                        DateUpdated = 0,
                        VoidDate = 88,
                        Populate270 = true
                    });
                else
                    dmReporting.Add(new DMReportingModel()
                    {
                        ClientName = "Wesley Sneijder",
                        Payer = "Dutch",
                        VoucherNumber = 13 + i + 6,
                        DM = true,
                        HighAcuity = true,
                        HasPayment = 0,
                        Has0Balance = 13,
                        BillingDate = 0,
                        DateUpdated = 13,
                        VoidDate = 0,
                        Populate270 = true
                    });
            }
            return dmReporting;
        }

        public enum DMReporting
    {
        Client = 1,
        Payer = 2,
        VoucherNumber = 3,
        HighAcuity = 4,
        DM = 5,
        HasPayment = 6,
        Has0Balance = 7,
        BillingDate = 8,
        DateUpdated = 9,
        VoidDate = 10,
    }
    }
}