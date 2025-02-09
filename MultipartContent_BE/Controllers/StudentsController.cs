using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ExcelGeneration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("exportWithDto")]
        public async Task<IActionResult> ExportWithDto()
        {
            var students = new List<StudentDto>
        {
            new StudentDto {  Name = "John Doe", Marks = 15, },
            new StudentDto {  Name = "Jane Smith", Marks = 14,  },
            new StudentDto {  Name = "Mark Brown", Marks = 16,}
        };
            // Convert JSON data to a string
            var jsonData = System.Text.Json.JsonSerializer.Serialize(students);

            // Create JSON content
            var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            jsonContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "students"
            };

            // Sample Excel data (for demonstration, replace with actual Excel data in byte[] format)
            var excelData = _studentService.GenerateStudentReport(students); { /* Your Excel file content here */ };
            var excelContent = new ByteArrayContent(excelData);
            excelContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            excelContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "Students.xlsx"
            };

            // Create multipart content
            var multipartContent = new MultipartContent("mixed", "boundary123");
            multipartContent.Add(jsonContent); // No second argument here
            multipartContent.Add(excelContent);

            // Return the multipart response
            return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(multipartContent.ReadAsStringAsync().Result)), "multipart/mixed; boundary=boundary123");

        }

        //[HttpGet("exportWithDto")]
        //public IActionResult downlod()
        //{
        //    // Generate Excel file content
        //    var excelContent = GenerateExcelFile();

        //    // Return the file as a response
        //    var fileName = "students_report.xlsx";  // The file name for download
        //    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";  // Excel content type

        //    return File(excelContent, contentType, fileName);
        //}


    }
}
