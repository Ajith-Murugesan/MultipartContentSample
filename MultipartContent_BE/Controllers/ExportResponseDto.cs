namespace ExcelGeneration.Controllers
{
    public class ExportResponseDto 
    {
        public List<StudentDto> Students { get; set; }
        public byte[] FileDownloadLink { get; set; }
    }
}
