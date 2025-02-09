namespace ExcelGeneration.Controllers
{
    public interface IStudentService
    {
        byte[] GenerateStudentReport(List<StudentDto> students);
    }

}
