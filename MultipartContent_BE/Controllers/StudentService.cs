using ClosedXML.Excel;
using ExcelGeneration.Controllers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class StudentService : IStudentService
{
    public byte[] GenerateStudentReport(List<StudentDto> students)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Students");

            // Add Header
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Marks";
            worksheet.Range("A1:B1").Style.Font.Bold = true;
            worksheet.Range("A1:B1").Style.Fill.BackgroundColor = XLColor.AshGrey;

            // Add Student Details
            for (int i = 0; i < students.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = students[i].Name;
                worksheet.Cell(i + 2, 2).Value = students[i].Marks;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}
